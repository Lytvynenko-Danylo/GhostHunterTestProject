using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public interface IEnemy
{
	event Action<IEnemy> OnShootDown;
	event Action<IEnemy> OnArrived;
	GameObject GameObject { get; }
	void Move(Vector3 start, Vector3 end, float speed = 0);
}

public class Ghost : MonoBehaviour, IEnemy
{
	public event Action<IEnemy> OnShootDown;
	public event Action<IEnemy> OnArrived;

	[SerializeField] private List<GameObject> _skins;
	[SerializeField] private GameObject _skinKilled;
	[Header("Settings")] [SerializeField] private float _speedGhost;
	[SerializeField] private float _durationDestroying;

	private Button _button;
	private Vector3 _defaultScale;

	public GameObject GameObject => gameObject;

	private void Awake()
	{
		_defaultScale = transform.localScale;
		_button = GetComponent<Button>();
		_button.onClick.AddListener(OnClickHandler);
	}

	private void Start()
	{
		transform.localScale = _defaultScale;

		// Show random skin
		int showSkinIndex = Random.Range(0, _skins.Count);
		for (int i = 0; i < _skins.Count; i++)
		{
			_skins[i].SetActive(i == showSkinIndex);
		}
		_skinKilled.SetActive(false);
	}

	public void Move(Vector3 start, Vector3 end, float speed = 0)
	{
		if(_button)
			_button.interactable = true;
		Start();

		if (speed < 0.0001f)
		{
			speed = _speedGhost;
		}
		
		// Move ghost
		transform.DOMove(end, speed).From(start)
			.SetEase(Ease.OutCubic).OnComplete(() =>
		{
			OnArrived?.Invoke(this);
		});
	}

	private void OnClickHandler()
	{
		// To prevent the user from clicking on the ghost multiple times
		_button.interactable = false;

		// Show shoot down skin
		_skins.ForEach(s => s.SetActive(false));
		_skinKilled.SetActive(true);

		// Show shoot down animation 
		transform.DOKill();
		transform.DOScale(Vector3.zero, _durationDestroying).SetEase(Ease.InBack).OnComplete(() =>
		{
			OnShootDown?.Invoke(this);
		});
	}
}