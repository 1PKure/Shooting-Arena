using UnityEngine;
using Code.Service;
using Systems.CentralizeEventSystem;

public static class EditorServiceSafetyNet
{
#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void EnsureServices()
    {
        if (!ServiceProvider.Instance.ContainsService<CentralizeEventSystem>())
        {
            ServiceProvider.Instance.AddService<CentralizeEventSystem>(new CentralizeEventSystem());
        }
    }
#endif
}