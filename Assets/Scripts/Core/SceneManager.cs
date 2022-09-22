public enum SceneName
{
    Start = 0,
    Game = 1
}

public interface ISceneManager
{
    void Load(SceneName name);
}

public class SceneManager: ISceneManager
{
    public void Load(SceneName name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)name);
    }
}
