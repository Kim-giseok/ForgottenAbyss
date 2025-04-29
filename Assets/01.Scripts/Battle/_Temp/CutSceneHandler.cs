using UnityEngine;

public class CutSceneHandler: MonoBehaviour
{
    private GameObject uiManager;
    
    private void SetCutSceneMode(bool isCutsceneMode)
    {
        if (isCutsceneMode)
        {
            uiManager.SetActive(false);
            GameManager.Instance.PausePlayer(true);
        }
        else
        {
            uiManager.SetActive(true);
            GameManager.Instance.PausePlayer(false);
        }
    }
    
    public void OnAgisNarrationEnd()
    {
        Destroy(gameObject);
        EnemyRespawner.Instance.Create(Enemies.Enemy.Agis, transform.position);
    }

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>().gameObject;
    }

    private void Start()
    {
        GlobalLightHandler.instance.light.color = Color.gray;
        SetCutSceneMode(true);
    }

    private void OnDestroy()
    {
        GlobalLightHandler.instance.light.color = Color.white;
        SetCutSceneMode(false);
    }
}