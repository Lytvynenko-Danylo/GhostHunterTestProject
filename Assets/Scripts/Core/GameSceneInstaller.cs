using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
	[SerializeField] private Ghost _ghostPrefab;
	public override void InstallBindings()
	{
		Container.Bind<IEnemy>().FromInstance(_ghostPrefab).AsSingle().Lazy();
	}
}