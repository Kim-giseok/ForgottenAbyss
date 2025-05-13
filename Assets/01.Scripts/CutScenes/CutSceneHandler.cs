using UnityEngine;

public class CutSceneHandler: MonoBehaviour
{
    private GameObject uiManager;
    
    private void SetCutSceneMode(bool isCutsceneMode)
    {
        if (isCutsceneMode)
        {
            uiManager?.SetActive(false);
            if(GameManager.Instance) GameManager.Instance.PausePlayer();
        }
        else
        {
            uiManager?.SetActive(true);
            if(GameManager.Instance) GameManager.Instance.PausePlayer(false);
        }
    }
    
    public void OnAgisNarrationEnd()
    {
        EnemiesPool.Instance.Create(Enemy.Agis, transform.position);
        Destroy(gameObject);
    }

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>()?.gameObject;
    }

    private void Start()
    {
        GlobalLightHandler.instance.Light.color = Color.gray;
        SetCutSceneMode(true);
    }

    private void OnDestroy()
    {
        GlobalLightHandler.instance.Light.color = Color.white;
        SetCutSceneMode(false);
    }
}