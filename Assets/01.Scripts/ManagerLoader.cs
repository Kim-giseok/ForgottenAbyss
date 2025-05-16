using UnityEngine;
using UnityEngine.AddressableAssets;

public class ManagerLoader: MonoBehaviour
{
    public static bool isLoaded = false;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Load()
    {
        Addressables.LoadAssetsAsync<GameObject>("Manager", null).Completed += (handle) =>
        {
            foreach (var prefab in handle.Result)
            {
                Instantiate(prefab);
            }
        };
        
        isLoaded = true;
    }
}