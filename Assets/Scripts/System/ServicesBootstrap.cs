using UnityEngine;
using Code.Service;
using Systems.CentralizeEventSystem;

public class ServicesBootstrap : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (!ServiceProvider.Instance.ContainsService<CentralizeEventSystem>())
            ServiceProvider.Instance.AddService<CentralizeEventSystem>(new CentralizeEventSystem());
    }
}