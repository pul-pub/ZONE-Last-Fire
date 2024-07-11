using UnityEngine;

public class YandexAdsManager : MonoBehaviour
{
    [SerializeField] private bool isShowAd = true;
    [SerializeField] private float delayAd = 120f;
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

    public void ShowingAd()
    {
        if (isShowAd && StaticVal.time[0] * 60 + StaticVal.time[1] >= StaticVal.timer)
        {
            ad.ShowInterstitial(pi);

            StaticVal.timer += 90;
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
