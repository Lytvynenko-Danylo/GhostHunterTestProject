using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class BtnPlay : MonoBehaviour
{
    private ISceneManager _sceneManager;
        
    [Inject]
    public void Construct(ISceneManager sceneManager)
    {
        _sceneManager = sceneManager;
    }
    
    private void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            _sceneManager.Load(SceneName.Game);
        });
    }
}
