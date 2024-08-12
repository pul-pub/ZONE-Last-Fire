using Mycom.Tracker.Unity;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Security.Cryptography;
using TMPro;
using RuStore.AppUpdate;


public class PlayConstructor : MonoBehaviour
{
    [Header("Constructor")]
    [SerializeField] private GameObject objButtonNext;
    [SerializeField] private GameObject objButtonDone;
    [SerializeField] private TMP_InputField namePlayer;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Slider[] characteristicsSlider;
    [SerializeField] private TextMeshProUGUI[] characteristics;
    [Header("Player")]
    [SerializeField] private Image imgFace;
    [SerializeField] private Image imgBody;
    [SerializeField] private Image imgRig;
    [Header("Data sprits")]
    [SerializeField] private Sprite[] faces;
    [SerializeField] private Sprite[] bodys;
    [SerializeField] private Sprite[] rigs;
    [Header("Charecter List")]
    [SerializeField] private UnityEngine.Object objChar;
    [SerializeField] private Transform parent;
    [SerializeField] private RectTransform contentTrans;

    private int idGun = -1;
    private int idColth = -1;

    private float score = 7;

    private void Awake()
    {
        if (File.Exists(Application.persistentDataPath + "/ids.json"))
        {
            AddSavesCharecters();
        }
        else
        {
            DataIds ids = new DataIds();
            ids.ids = new string[1] { null };

            string json = JsonUtility.ToJson(ids, true);
            File.WriteAllText(Application.persistentDataPath + "/ids.json", json);

            if (File.Exists(Application.persistentDataPath + "/save.json"))
            {
                string id = GenerateUniqueId();

                json = File.ReadAllText(Application.persistentDataPath + "/save.json");
                ObjectSave save = JsonUtility.FromJson<ObjectSave>(json);

                StaticVal.idSkills[0] = 0;
                StaticVal.characteristics[0] = 0;
                StaticVal.characteristics[1] = 0;
                StaticVal.characteristics[2] = 0;

                save.name = "Сохранение до v.1.1";
                save.idFace = 0;
                save.idSkills = StaticVal.idSkills;
                save.characteristics = StaticVal.characteristics;
                save.timeCreated = System.DateTime.Now.ToString();

                json = JsonUtility.ToJson(save, true);
                File.WriteAllText(Application.persistentDataPath + "/" + id + ".json", json);

                json = File.ReadAllText(Application.persistentDataPath + "/ids.json");
                ids = JsonUtility.FromJson<DataIds>(json);
                string[] id_list = new string[ids.ids.Length + 1];
                if (ids.ids[0] != null && ids.ids[0] != "")
                {
                    for (int i = 0; i < ids.ids.Length; i++)
                    {
                        id_list[i] = ids.ids[i];
                        id_list[ids.ids.Length] = id;
                    }
                    ids.ids = id_list;
                }
                else
                {
                    ids.ids[0] = id;
                }

                json = JsonUtility.ToJson(ids, true);
                File.WriteAllText(Application.persistentDataPath + "/ids.json", json);

                File.Delete(Application.persistentDataPath + "/save.json");

                AddSavesCharecters();
            }
        }

        objButtonDone.SetActive(false);
    }

    private void AddSavesCharecters()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/ids.json");
        DataIds ids = JsonUtility.FromJson<DataIds>(json);

        foreach (string id in ids.ids)
        {
            if (id != null && id != "")
            {
                GameObject gObj = Instantiate(objChar, Vector3.zero, new Quaternion(0, 0, 0, 0), parent) as GameObject;
                
                gObj.GetComponentInChildren<Button>().onClick.AddListener(delegate { Contline(id); });

                json = File.ReadAllText(Application.persistentDataPath + "/" + id + ".json");
                ObjectSave save = JsonUtility.FromJson<ObjectSave>(json);
                gObj.GetComponentsInChildren<TextMeshProUGUI>()[0].text = save.name;
                gObj.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Создан: " + save.timeCreated;

                gObj.name = id;
                contentTrans.sizeDelta += new Vector2(0, 250);
            }
        }
    }

    private void Update()
    {
        scoreText.text = "Нераспределенные очки: " + score;
        if (namePlayer.text != "")
        {
            objButtonNext.SetActive(true);
            if (idColth != -1 &&
                idGun != -1 &&
                StaticVal.idFace != -1)
            {
                objButtonDone.SetActive(true);
            }
            else
            {
                objButtonDone.SetActive(false);
            }
        }
        else
        {
            objButtonNext.SetActive(false);
        }

        if (StaticVal.idFace != -1)
        {
            imgFace.sprite = faces[StaticVal.idFace];
        }
        if (idColth != -1)
        {
            if (idColth == 94)
            {
                imgBody.sprite = bodys[0];
                imgRig.sprite = rigs[0];
            }
            else if (idColth == 99)
            {
                imgBody.sprite = bodys[1];
                imgRig.sprite = rigs[1];
            }
        }
    }

    private void Contline(string id)
    {
        if (GetComponent<AppUpdate>().GetAppUpdateInfo().updateAvailability == AppUpdateInfo.UpdateAvailability.UPDATE_AVAILABLE)
        {
            GetComponent<AppUpdate>().StartFlexibleUpdate();
            return;
        }
        else if (GetComponent<AppUpdate>().GetAppUpdateInfo().updateAvailability == AppUpdateInfo.UpdateAvailability.DEVELOPER_TRIGGERED_UPDATE_IN_PROGRESS)
        {
            return;
        }
        
        StaticVal.idSesion = id;
        StaticVal.nameSave = "/" + StaticVal.idSesion + ".json";
        string json = File.ReadAllText(Application.persistentDataPath + StaticVal.nameSave);
        ObjectSave save = JsonUtility.FromJson<ObjectSave>(json);

        for (int i = 0; i < 8; i++)
            StaticVal.peopls[1, i] = save.peopls1[i];
        for (int i = 0; i < 8; i++)
            StaticVal.peopls[2, i] = save.peopls2[i];
        for (int i = 0; i < 8; i++)
            StaticVal.peopls[3, i] = save.peopls3[i];
        for (int i = 0; i < 8; i++)
            StaticVal.peopls[4, i] = save.peopls4[i];
        StaticVal.money = save.money;
        StaticVal.time = save.time;
        StaticVal.idRain = save.isRain;
        StaticVal.light = save.lights;

        StaticVal.idFace = save.idFace;
        StaticVal.idSkills = save.idSkills;
        StaticVal.characteristics = save.characteristics;
        StaticVal.name = save.name;
        StaticVal.notSelectedXP = save.notSelectedXP;

        SceneManager.LoadScene(save.indexScene, LoadSceneMode.Single);
    }

    public static string GenerateUniqueId()
    {
        // Создаем массив байтов размером 32 байта (256 бит)
        byte[] randomBytes = new byte[32];

        // Используем RNGCryptoServiceProvider для заполнения массива случайными байтами
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(randomBytes);
        }

        // Преобразуем байты в строку Base64
        string uniqueId = Convert.ToBase64String(randomBytes);

        // Удаляем символы '+' и '/' из Base64, которые могут вызвать проблемы в некоторых контекстах
        uniqueId = uniqueId.Replace('+', '-').Replace('/', '_');

        // Удаляем последний символ '=' из Base64, если он есть
        uniqueId = uniqueId.TrimEnd('=');

        return uniqueId;
    }

    public void CreateCharacter()
    {
        if (GetComponent<AppUpdate>().GetAppUpdateInfo().updateAvailability == AppUpdateInfo.UpdateAvailability.UPDATE_AVAILABLE)
        {
            GetComponent<AppUpdate>().StartFlexibleUpdate();
            return;
        }
        else if (GetComponent<AppUpdate>().GetAppUpdateInfo().updateAvailability == AppUpdateInfo.UpdateAvailability.DEVELOPER_TRIGGERED_UPDATE_IN_PROGRESS)
        {
            return;
        }

        StaticVal.idSesion = GenerateUniqueId();
        StaticVal.nameSave = "/" + StaticVal.idSesion + ".json";

        StaticVal.name = namePlayer.text;
        StaticVal.characteristics[0] = int.Parse(characteristics[0].text); 
        StaticVal.characteristics[1] = int.Parse(characteristics[1].text);
        StaticVal.characteristics[2] = int.Parse(characteristics[2].text);

        ObjectSave save = new ObjectSave();

        int[] idItem = new int[29];
        int[] coutItem = new int[29];
        int[] condItem = new int[29];
        int[] indificatorItem = new int[29];
        int[] listQests = new int[1] { 0 };
        int[] listEndingQests = null;

        save.currentAmmos[0] = 0;
        save.name = StaticVal.name;

        for (int i = 0; i < 8; i++)
            save.peopls1[i] = StaticVal.peoplsStart[1, i];
        for (int i = 0; i < 8; i++)
            save.peopls2[i] = StaticVal.peoplsStart[2, i];
        for (int i = 0; i < 8; i++)
            save.peopls3[i] = StaticVal.peoplsStart[3, i];
        for (int i = 0; i < 8; i++)
            save.peopls4[i] = StaticVal.peoplsStart[4, i];

        for (int i = 0; i < 29; i++)
        {
            idItem[i] = 0;
        }
        for (int i = 0; i < 29; i++)
        {
            coutItem[i] = 0;
        }
        for (int i = 0; i < 29; i++)
        {
            condItem[i] = 100;
        }
        for (int i = 0; i < 29; i++)
        {
            indificatorItem[i] = i;
        }
        idItem[25] = idGun;
        coutItem[25] = 1;
        if (idGun != 80)
        {
            idItem[0] = 13;
            coutItem[0] = 20;
        }
        else
        {
            idItem[0] = 14;
            coutItem[0] = 10;
        }
        idItem[28] = idColth;
        coutItem[28] = 1;
        save.idQuests = listQests;
        save.idEndingQuests = listEndingQests;

        save.idActivQuest = 0;
        save.idItem = idItem;
        save.coutItem = coutItem;
        save.condItem = condItem;
        save.indificatorItem = indificatorItem;
        save.falgGun = true;
        save._numGun = 0;
        save.pos = new Vector2(-67, 1);
        save.time = new int[2] { 8, 0 };
        save.isRain = false;
        save.lights = 0.59f;
        save.posSky = new Vector2(0, 13.5f);
        save.health = 100;
        save.armor = 0;
        save.money = 1000;
        save.indexScene = 1;

        save.alfaUi = StaticVal.alfaUi;
        save.volSound = StaticVal.volSound;
        save.vibroMode = StaticVal.vibroMode;
        save.promptMode = StaticVal.promptMode;
        save.FPSMode = StaticVal.FPSMode;

        save.idFace = StaticVal.idFace;
        save.idSkills = StaticVal.idSkills;
        save.characteristics = StaticVal.characteristics;
        save.timeCreated = System.DateTime.Now.ToString();

        string json = JsonUtility.ToJson(save, true);
        File.WriteAllText(Application.persistentDataPath + StaticVal.nameSave, json);

        json = File.ReadAllText(Application.persistentDataPath + "/ids.json");
        DataIds ids = JsonUtility.FromJson<DataIds>(json);
        string[] id_list = new string[ids.ids.Length + 1];
        if (ids.ids[0] != null && ids.ids[0] != "")
        {
            for (int i = 0; i < ids.ids.Length; i++)
            {
                id_list[i] = ids.ids[i];
                id_list[ids.ids.Length] = StaticVal.idSesion;
            }
            ids.ids = id_list;
        }
        else
        {
            ids.ids[0] = StaticVal.idSesion;
        }

        json = JsonUtility.ToJson(ids, true);
        File.WriteAllText(Application.persistentDataPath + "/ids.json", json);
        /*
        if (StaticVal.trecker_id_android != null)
        {
            MyTracker.TrackEvent("tutorial_begin");
        }
        */
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void SetIdFase(int id)
    {
        StaticVal.idFace = id;
    }

    public void SetGun(int id)
    {
        idGun = id;
    }

    public void SetColth(int id)
    {
        idColth = id;
    }

    public void Add()
    {
        for (int i = 0; i < characteristics.Length; i++) 
        {
            if (float.Parse(characteristics[i].text) < characteristicsSlider[i].value && score > 0)
            {
                score += float.Parse(characteristics[i].text) - characteristicsSlider[i].value;
                characteristics[i].text = characteristicsSlider[i].value.ToString();
            }
            else if (float.Parse(characteristics[i].text) > characteristicsSlider[i].value)
            {
                score += float.Parse(characteristics[i].text) - characteristicsSlider[i].value;
                characteristics[i].text = characteristicsSlider[i].value.ToString();
            }
        }
    }
}
