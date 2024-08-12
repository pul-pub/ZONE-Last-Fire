using RuStore.AppUpdate;
using UnityEngine;
using UnityEngine.UI;

public class AppUpdate : MonoBehaviour
{
    private void Awake()
    {
        RuStoreAppUpdateManager.Instance.Init();
    }

    public AppUpdateInfo GetAppUpdateInfo()
    {
        AppUpdateInfo info = null;

        RuStoreAppUpdateManager.Instance.GetAppUpdateInfo(
            onFailure: (error) => { },
            onSuccess: (result) => { info = result; });

        return info;
    }

    public void StartFlexibleUpdate()
    {
        RuStoreAppUpdateManager.Instance.StartUpdateFlow(
            UpdateType.FLEXIBLE,
            onFailure: (error) => { },
            onSuccess: (result) => { });
    }
}
