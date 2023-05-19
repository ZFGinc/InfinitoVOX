using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class SceneTransition : MonoBehaviour
{
    private Animator _animator;

    private static SceneTransition Instance;
    private static bool shouldPlayOpeningAnimation = true;
    public static string CurrentScene { get; private set; } = "Menu";

    private AsyncOperation loadingSceneOperation;

    public static void SwitchToScene(string sceneName)
    {
        CurrentScene = sceneName;
        Instance._animator.SetTrigger("sceneClosing");
        Instance.loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);
        Instance.loadingSceneOperation.allowSceneActivation = false;
    }

    private void Start()
    {
        Instance = this;

        _animator = GetComponent<Animator>();

        if (shouldPlayOpeningAnimation)
        {
            _animator.SetTrigger("sceneOpening");
            Time.timeScale = 1f;
            
            shouldPlayOpeningAnimation = false;
        }

        if(CurrentScene != "Menu")
        {
            Time.timeScale = 0f;
        }

        Debug.Log(CurrentScene);
    }

    public void OnAnimationOver()
    {
        shouldPlayOpeningAnimation = true;
        loadingSceneOperation.allowSceneActivation = true;
    }
}
