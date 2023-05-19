using UnityEngine;

public enum SceneName : byte
{
    Null = 0,
    Default = 1,
    Winter,
    Desert
}

public class MenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject _menuInGame;
    [SerializeField] private GameObject _panelNotConnection;
    [SerializeField] private RewardDay _rewardDay;

    [SerializeField] private SceneName _sceneName;

    private void Awake()
    {
        if (SceneTransition.CurrentScene == "Menu")
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
                _panelNotConnection.SetActive(true);
            else
                _rewardDay.CheckReward();
        }
    }

    public void StartGame()
    {
        SceneTransition.SwitchToScene(GetSceneName(_sceneName));
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        StartGame();
    }

    public void GoToMenu()
    {
        SceneTransition.SwitchToScene("Menu");
    }

    public void OpenMenuInGame()
    {
        Time.timeScale = 0f;
        _menuInGame.SetActive(true);
    }

    public void CloseMenuInGame()
    {
        Time.timeScale = 1f;
        _menuInGame.SetActive(false);
    }

    public void OpenOtherGamesWindow()
    {
        Application.OpenURL("https://play.google.com/store/apps/dev?id=5553838984040552484");
    }

    public static string GetSceneName(SceneName value)
    {
        switch (value)
        {
            case SceneName.Default: return "DefaultMap";
            case SceneName.Winter: return "WinterMap";
        }
        return "ERROR";
    }

    public static SceneName GetSceneName(string value)
    {
        switch (value)
        {
            case "DefaultMap" : return SceneName.Default;
            case "WinterMap" : return SceneName.Winter;
        }
        return SceneName.Null;
    }
}
