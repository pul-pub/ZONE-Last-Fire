using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase.Extensions;
using Firebase;
using Firebase.Analytics;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject objButton;
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderHDU;
    [SerializeField] private Toggle toggleVibroMode;
    [SerializeField] private Toggle togglePromptMode;
    [SerializeField] private Image[] imgs;

    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                StaticVal.firebaseApp = FirebaseApp.DefaultInstance;
                Debug.Log("Begin start Firebase on application! Name modul: " + StaticVal.firebaseApp);
            }
            else
            {
                Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });

        string json = File.ReadAllText(Application.persistentDataPath + "/save.json");
        ObjectSave save = JsonUtility.FromJson<ObjectSave>(json);

        if (save != null)
        {
            if (save.isSave)
            {
                objButton.SetActive(true);
            }

            sliderHDU.value = save.alfaUi;
            sliderMusic.value = save.volSound;
            toggleVibroMode.isOn = save.vibroMode;
            togglePromptMode.isOn = save.promptMode;
            UpdateMods();
        }
    }

    public void UpdateMods()
    {
        StaticVal.alfaUi = sliderHDU.value;
        StaticVal.volSound = sliderMusic.value;
        StaticVal.vibroMode = toggleVibroMode.isOn;
        StaticVal.promptMode = togglePromptMode.isOn;

        foreach(Image img in imgs)
        {
            if (img != null)
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, sliderHDU.value);
            }
        }
    }

    public void Play()
    {
        StaticVal.nameSave = null;
        for(int i = 0; i < 8; i++)
            StaticVal.peopls[1, i] = StaticVal.peoplsStart[1, i];
        for (int i = 0; i < 8; i++)
            StaticVal.peopls[2, i] = StaticVal.peoplsStart[2, i];
        for (int i = 0; i < 8; i++)
            StaticVal.peopls[3, i] = StaticVal.peoplsStart[3, i];
        StaticVal.time = new int[2] { 8, 0 };
        StaticVal.idRain = false;
        StaticVal.light = 0.59f;
        StaticVal.money = 2000;
        /*
        string json = File.ReadAllText(Application.persistentDataPath + "/save.json");
        ObjectSave save = JsonUtility.FromJson<ObjectSave>(json);

        save.alfaUi = StaticVal.alfaUi;
        save.volSound = StaticVal.volSound;
        save.vibroMode = StaticVal.vibroMode;
        save.promptMode = StaticVal.promptMode;

        json = JsonUtility.ToJson(save, true);
        File.WriteAllText(Application.persistentDataPath + "/save.json", json);
        */
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventTutorialBegin);
        SceneManager.LoadScene("LERN", LoadSceneMode.Single);
    }

    public void Contline()
    {
        StaticVal.nameSave = "/save.json";
        string json = File.ReadAllText(Application.persistentDataPath + StaticVal.nameSave);
        ObjectSave save = JsonUtility.FromJson<ObjectSave>(json);

        for (int i = 0; i < 8; i++)
            StaticVal.peopls[1, i] = save.peopls1[i];
        for (int i = 0; i < 8; i++)
            StaticVal.peopls[2, i] = save.peopls2[i];
        for (int i = 0; i < 8; i++)
            StaticVal.peopls[3, i] = save.peopls3[i];
        StaticVal.money = save.money;
        StaticVal.time = save.time;
        StaticVal.idRain = save.isRain;
        StaticVal.light = save.lights;

        SceneManager.LoadScene(save.indexScene, LoadSceneMode.Single);
    }

    public void OpenURL()
    {
        Application.OpenURL("https://discord.gg/fHYwnNShVA");
    }

    public void Exit()
    {
        /*
        StaticVal.nameSave = "/save.json";

        string json = File.ReadAllText(Application.persistentDataPath + "/save.json");
        ObjectSave save = JsonUtility.FromJson<ObjectSave>(json);

        save.alfaUi = StaticVal.alfaUi;
        save.volSound = StaticVal.volSound;
        save.vibroMode = StaticVal.vibroMode;
        save.promptMode = StaticVal.promptMode;

        json = JsonUtility.ToJson(save, true);
        File.WriteAllText(Application.persistentDataPath + "/save.json", json);
        */
        Application.Quit();
    }
}
