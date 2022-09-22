using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private const float MOVE_OFFSET_ANIMETION = -1.5f;
    
    [SerializeField] private TMP_Text _text;
    [Header("Settings")]
    [SerializeField] private int _placeToSpawn;
    [SerializeField] private int _countEnemyOnScene;
    [SerializeField] private float _timeToSpawn;
    [Header("Position")]
    [SerializeField] private Transform _spawnPlace;
    [SerializeField] private Transform _enemyPlace;

    private GameObject _enemyPrefab;
    private List<RectTransform> _enemiesPlace;
    private Dictionary<IEnemy, bool> _enemies; //where IEnemy - enemy object, bool - is active
    private RectTransform _lastplace;
    private int _enemyCountDie;
    
    [Inject]
    public void Construct(IEnemy enemyPrefab)
    {
        _enemyPrefab = enemyPrefab.GameObject;
    }
    
    private void Awake()
    {
        // Сreate places from which ghosts will fly out
        _enemiesPlace ??= new List<RectTransform>();
        for (int i = 0; i < _placeToSpawn; i++)
        {
            RectTransform spawn = Instantiate(_enemyPrefab, _spawnPlace).GetComponent<RectTransform>();
            spawn.gameObject.SetActive(true);
            _enemiesPlace.Add(spawn);
        }
        // We create a pool of ghosts from where we will pull out
        _enemies ??= new Dictionary<IEnemy, bool>();
        for (int i = 0; i < _countEnemyOnScene; i++)
        {
            GameObject enemyObject = Instantiate(_enemyPrefab, _enemyPlace);
            if (enemyObject.TryGetComponent<IEnemy>(out var enemy))
            {
                enemy.GameObject.SetActive(false);
                enemy.OnArrived += OnArrivedHandler;
                enemy.OnShootDown += OnShootDownHandler;
                _enemies.Add(enemy, false);
            }
        }

        StartCoroutine(SpawnEnemies());
    }

    private void OnDestroy()
    {
        StopCoroutine(SpawnEnemies());
    }
    
    /// <summary>
    ///  Show enemies with some frequency 
    /// </summary>
    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            RectTransform place;
            do
            {
                place = _enemiesPlace[Random.Range(0, _placeToSpawn)];
            } while (_placeToSpawn > 1 && place == _lastplace);

            _lastplace = place;

            KeyValuePair<IEnemy, bool> enemy = _enemies.FirstOrDefault(e => !e.Value);
            if (enemy.Key != null)
            {
                _enemies[enemy.Key] = !enemy.Value;
                if(enemy.Key.GameObject.TryGetComponent(out RectTransform enemyRect))
                {
                    enemyRect.sizeDelta = place.sizeDelta;
                    enemyRect.position = place.position;
                    enemy.Key.GameObject.SetActive(true);
                }

                Vector3 placePosition = place.position;
                enemy.Key.Move(placePosition, new Vector3(placePosition.x, placePosition.y * MOVE_OFFSET_ANIMETION + Screen.height, placePosition.z));
            }
            
            yield return new WaitForSeconds(_timeToSpawn);
        }
    }

    private void OnShootDownHandler(IEnemy enemy)
    {
        // Update score
        _enemyCountDie++;
        _text.text = $"Score: {_enemyCountDie.ToString()}";
       
        OnArrivedHandler(enemy);
    }

    private void OnArrivedHandler(IEnemy enemy)
    {
        // Return to the pool of available enemies
        _enemies[enemy] = false;
    }
}
