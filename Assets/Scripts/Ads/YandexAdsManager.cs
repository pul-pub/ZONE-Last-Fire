using RuStore.Review;
using UnityEngine;

public class YandexAdsManager : MonoBehaviour
{
    [SerializeField] private bool isShowAd = true;
    [SerializeField] private int delayAd = 120;
    private ObjectAdInterstitial ad;
    private PlayerInterface pi;

    private void Awake()
    {
        pi = GetComponent<PlayerInterface>();
        FindObjects();
    }

    private void FindObjects()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "AdsObject" && obj.scene.name == "DontDestroyOnLoad")
            {
                Debug.Log("Found DontDestroyOnLoad object: " + obj.name);
                ad = obj.GetComponent<ObjectAdInterstitial>();
            }
        }
    }

    public void SowingScreenReview()
    {
        if (StaticVal.time[0] * 60 + StaticVal.time[1] >= 720)
        {
#if !UNITY_EDITOR
            RuStoreReviewManager.Instance.RequestReviewFlow(
                onFailure: (error) => { },
                onSuccess: () => {
                    RuStoreReviewManager.Instance.LaunchReviewFlow(
                        onFailure: (error) => { },
                        onSuccess: () => { });
                });
#endif
        }
    }

    public void ShowingAd()
    {
        if (StaticVal.time[0] * 60 + StaticVal.time[1] >= StaticVal.timer)
        {
#if !UNITY_EDITOR
            ad.ShowInterstitial(pi);
#else
            Debug.Log("Ad vived");
            pi.Loader(null, null);
#endif
            StaticVal.timer += delayAd;
            if (StaticVal.timer >= 1440)
            {
                StaticVal.timer = 0;
            }
        }
        else 
        {
            pi.Loader(null, null);
        }
    }
}
