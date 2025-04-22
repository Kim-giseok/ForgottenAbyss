using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIInitializer : MonoBehaviour
{
    [SerializeField] private string[] skipScenes = { "StartScene" }; // UIManager 생성 생략할 씬 여기서 추가

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject); // GameInitializer 계속 유지
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded( Scene scene, LoadSceneMode mode)
    {
        string currentScene = scene.name;

        foreach (string skipScene in skipScenes)
        {
            if (currentScene == skipScene)
            {
                return;
            }
        }

        if (UIManager.Instance == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/UIManager");

            if (prefab != null)
            {
                Instantiate(prefab);
            }
        }
    }
}
