//using Firebase.Analytics;
using Mycom.Tracker.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInterface : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private DataBase data;
    [Header("GUI")]
    [SerializeField] private Slider health;
    [SerializeField] private Slider arm;
    [SerializeField] private TextMeshProUGUI money;
    [SerializeField] public EventTrigger[] ets;
    [Header("Dialog")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private GameObject screenDialog;
    [Space]
    [SerializeField] private TextMeshProUGUI textDialog;
    [SerializeField] private TextMeshProUGUI textDiscription1;
    [SerializeField] private TextMeshProUGUI textDiscription2;
    [SerializeField] private TextMeshProUGUI textDiscription3;
    [Space]
    [SerializeField] private GameObject buttonDiscription1;
    [SerializeField] private GameObject buttonDiscription2;
    [SerializeField] private GameObject buttonDiscription3;
    [Space]
    [SerializeField] private Image imgDiscription1;
    [SerializeField] private Image imgDiscription2;
    [SerializeField] private Image imgDiscription3;
    [Space]
    [SerializeField] private Sprite spriteQuest;
    [SerializeField] private Sprite spriteExit;
    [SerializeField] private Sprite spriteStore;
    [SerializeField] private Sprite spriteHabar;
    [Header("Quest")]
    [SerializeField] public List<Quest> Quests = new List<Quest>();
    [SerializeField] public List<Quest> EndingQuests = new List<Quest>();
    [SerializeField] public Quest ActivQuest;
    [SerializeField] private TextMeshProUGUI titleQuest;
    [SerializeField] private TextMeshProUGUI discriptionQuest;
    [SerializeField] private RectTransform marker;
    [SerializeField] private RectTransform leftForMarker;
    [SerializeField] private RectTransform rightForMarker;
    [Header("Screen Dide")]
    [SerializeField] private Image screenDide;
    [SerializeField] private TextMeshProUGUI textDide;
    [Header("Input on Mobil")]
    [SerializeField] private GameObject screenMobil;
    [SerializeField] private Image imgNoPve;
    [SerializeField] private Image gun1;
    [SerializeField] private Image gun2;
    [SerializeField] private TextMeshProUGUI textNoPve;
    [SerializeField] private TextMeshProUGUI gunText1;
    [SerializeField] private TextMeshProUGUI gunText2;
    [Header("Window Y/n")]
    [SerializeField] private GameObject screenYn;
    [Header("PDA")]
    [SerializeField] private TextMeshProUGUI timerPDA;
    [SerializeField] private Transform forPosSky;
    [SerializeField] private Light2D light2D;
    [Header("Ammos")]
    [SerializeField] private TextMeshProUGUI textAmmos;
    [SerializeField] private TextMeshProUGUI textWeapon;
    [Header("Detector")]
    [SerializeField] private RectTransform trDetector;
    [SerializeField] private RectTransform trArt;
    [SerializeField] private GameObject objDetector;
    [Header("Save")]
    [SerializeField] private GameObject buttonSave;
    [SerializeField] private RectTransform saveIcon;
    [Header("Other")]
    public bool _isPause = false;
    public bool onDialog = false;
    public bool onPda = false;
    private Dialog dialog;
    private Enemy currentEnemyScript;
    public Camera _cam;
    [SerializeField] private Inventory _inventory;

    private CamController camC;
    private YandexAdsManager yandexAdsManager;
    private Collider2D _enemyBg;
    private List<Item> itm;
    private DoorObject door;
    private float timer = 10f;

    private void OnEnable() => player.OnSetActivDialog += OpenongBagOrDialog;

    private void OnDisable() => player.OnSetActivDialog -= OpenongBagOrDialog;

    private void Awake()
    {
        Resume();
        if (StaticVal.nameSave != null)
        {
            string json = File.ReadAllText(Application.persistentDataPath + StaticVal.nameSave);
            ObjectSave save = JsonUtility.FromJson<ObjectSave>(json);

            foreach (int i in save.idQuests)
            {
                foreach (Quest quest in data.quests)
                {
                    if (quest.id == i)
                        Quests.Add(quest);
                }
            }
            foreach (int i in save.idEndingQuests)
            {
                foreach (Quest quest in data.quests)
                {
                    if (quest.id == i)
                        EndingQuests.Add(quest);
                }
            }
            foreach (Quest quest in data.quests)
            {
                if (quest.id == save.idActivQuest)
                    ActivQuest = quest;
            }

            AudioListener.volume = save.volSound;
        }

        foreach (Image i in screenMobil.GetComponentsInChildren<Image>())
            i.color = new Color(i.color.r, i.color.g, i.color.b, StaticVal.alfaUi);
        foreach (TextMeshProUGUI t in screenMobil.GetComponentsInChildren<TextMeshProUGUI>())
            t.color = new Color(t.color.r, t.color.g, t.color.b, StaticVal.alfaUi);

        yandexAdsManager = GetComponent<YandexAdsManager>();
        _cam = Camera.main;
        if (StaticVal.type != TypePlatform.Mobile)
            screenMobil.SetActive(false);

        if (SceneManager.GetActiveScene().buildIndex <= 2 ||
            SceneManager.GetActiveScene().buildIndex == 6 ||
            SceneManager.GetActiveScene().buildIndex == 8)
            buttonSave.SetActive(true);
        else
            buttonSave.SetActive(false);
    }

    private void Update()
    {
        money.text = StaticVal.money.ToString() + "руб.";
        health.value = player._Object.Health;

        if (timer >= 0)
            timer -= Time.deltaTime;

        if (player._weapon._numGun == 0)
        {
            gun1.color = new Color(gun1.color.r, gun1.color.g, gun1.color.b, StaticVal.alfaUi + 0.25f);
            gun2.color = new Color(gun2.color.r, gun2.color.g, gun2.color.b, StaticVal.alfaUi);

            gunText1.color = new Color(gunText1.color.r, gunText1.color.g, gunText1.color.b, StaticVal.alfaUi + 0.25f);
            gunText2.color = new Color(gunText2.color.r, gunText2.color.g, gunText2.color.b, StaticVal.alfaUi);
        }
        else if (player._weapon._numGun == 1)
        {
            gun1.color = new Color(gun1.color.r, gun1.color.g, gun1.color.b, StaticVal.alfaUi);
            gun2.color = new Color(gun2.color.r, gun2.color.g, gun2.color.b, StaticVal.alfaUi + 0.25f);

            gunText1.color = new Color(gunText1.color.r, gunText1.color.g, gunText1.color.b, StaticVal.alfaUi);
            gunText2.color = new Color(gunText2.color.r, gunText2.color.g, gunText2.color.b, StaticVal.alfaUi + 0.25f);
        }

        if (player._Object.Arm > 0)
        {
            arm.value = player._Object.Arm;
        }
        else
        {
            arm.gameObject.SetActive(false);
        }

        if (ActivQuest != null)
        {
            titleQuest.gameObject.SetActive(true);
            discriptionQuest.gameObject.SetActive(true);
            titleQuest.text = ActivQuest.title;
            discriptionQuest.text = ActivQuest.header;
        }
        else
        {
            titleQuest.gameObject.SetActive(false);
            discriptionQuest.gameObject.SetActive(false);
        }

        SetPosMerker();
        SetTextForAmmo();
        SetTimers();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(player.transform.position, new Vector3(2, 3f, 0));
    }

    public void SaveProgress()
    {
        player.Save(player.transform.position, forPosSky.localPosition, StaticVal.light, true, "/save.json");
        StartCoroutine(SavesAnimation());
    }

    IEnumerator SavesAnimation()
    {
        saveIcon.gameObject.SetActive(true);
        int _numCirkle = 0;
        while (_numCirkle < 2)
        {
            saveIcon.eulerAngles -= new Vector3(0, 0, 2f);
            if (saveIcon.eulerAngles.z >= 178 && saveIcon.eulerAngles.z < 180)
            {
                _numCirkle += 1;
            }
            yield return new WaitForEndOfFrame();
        }
        saveIcon.gameObject.SetActive(false);
    }

    public void SetTimers()
    {
        string time = "";

        if (StaticVal.time[0] < 10)
            time += "0" + StaticVal.time[0].ToString();
        else
            time += StaticVal.time[0].ToString();
        if (StaticVal.time[1] < 10)
            time += ":0" + StaticVal.time[1].ToString();
        else
            time += ":" + StaticVal.time[1].ToString();

        timerPDA.text = time;
    }

    public void Exit()
    {
        Resume();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void OpenWindowToMoveScene(DoorObject obj)
    {
        Pause();
        screenYn.SetActive(true);
        door = obj;
    }

    public void GetTouchedF(bool f)
    {
        player.PressedF();
    }

    public void SceneYes()
    {
        player.Save(door.pos, forPosSky.localPosition, StaticVal.light);
        StaticVal.nameSave = "/sceneData.json";
        yandexAdsManager.ShowingAd();
    }

    public void Loader(object sender, EventArgs args)
    {
        Resume();
        if (StaticVal.trecker_id_android != null && SceneManager.GetActiveScene().buildIndex == 9)
        {
            MyTracker.TrackEvent("tutorial_complete");
        }
        SceneManager.LoadScene(door.SceneName, LoadSceneMode.Single);
    }

    private void SetPosMerker()
    {
        if (ActivQuest != null)
        {
            marker.gameObject.SetActive(true);
            if (ActivQuest.FindItem(_inventory, false) > 0)
            {
                Vector2 pos = _cam.WorldToScreenPoint(ActivQuest.pointReturn[SceneManager.GetActiveScene().buildIndex]);
                marker.position = pos;
            }
            else
            {
                Vector2 pos = _cam.WorldToScreenPoint(ActivQuest.point[SceneManager.GetActiveScene().buildIndex]);
                marker.position = pos;
            }

            if (marker.position.x < leftForMarker.position.x)
            {
                Vector2 pos = new Vector2(leftForMarker.position.x, marker.position.y);
                marker.position = pos;
            }
            else if (marker.position.x > rightForMarker.position.x + 100)
            {
                Vector2 pos = new Vector2(rightForMarker.position.x + 100, marker.position.y);
                marker.position = pos;
            }
        }
        else
        {
            marker.gameObject.SetActive(false);
        }
    }

    private void SetTextForAmmo()
    {
        if (player._weapon._guns[player._weapon._numGun] != null)
        {
            textAmmos.text = _inventory.FindAmmos(player._weapon._guns[player._weapon._numGun]).ToString();
            textWeapon.text = player._weapon._guns[player._weapon._numGun].currentAmmos.ToString();
        }
        else
        {
            textAmmos.text = "--";
            textWeapon.text = "--";
        }
    }

    public void GetArt()
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(player.transform.position, new Vector2(2, 3), 0, whatIsGround);

        if (cols.Length > 0)
        {
            foreach (Collider2D col in cols)
            {
                if (col.GetComponents<Artifact>().Length > 0 && col.GetComponent<Artifact>().enabled)
                {
                    if (_inventory.AddItemsToNull(col.GetComponent<Artifact>().item, 1, 90))
                        Destroy(col.gameObject);

                    if (StaticVal.trecker_id_android != null)
                    {
                        Dictionary<string, string> param = new Dictionary<string, string>();
                        param["ID"] = col.GetComponent<Artifact>().item.id.ToString();

                        MyTracker.TrackEvent("take_habar", param);
                    }
                }
            }
        }
    }

    public Dialog GetDialog(Vector2 _pos)
    {
        _cam.ScreenToWorldPoint(_pos);
        Collider2D[] cols = Physics2D.OverlapBoxAll(player.transform.position, new Vector2(2, 3), 0, whatIsGround);
        Enemy _enemy;
        EnemyMutant _enemyMut;

        foreach (Collider2D col in cols)
        {
            if (col.CompareTag("Enemy") && col.GetComponentsInParent<Enemy>().Length > 0)
            {
                _enemy = col.GetComponentInParent<Enemy>();

                if (_enemy.Dialog.Length <= 0 && !_enemy.IsDided)
                    return null;

                if (!_enemy.IsDided)
                {
                    if (_enemy.insShop)
                        itm = _enemy.shop;

                    foreach (Quest q in Quests)
                    {
                        if (_enemy.Name == q.NameTo && q.item != null && q.FindItem(_inventory) != -1)
                        {
                            if (StaticVal.trecker_id_android != null)
                            {
                                Dictionary<string, string> param = new Dictionary<string, string>();
                                param["ID"] = q.id.ToString();

                                MyTracker.TrackEvent("take_habar", param);
                            }
                            Dialog dl = q.startDialog;
                            currentEnemyScript = _enemy;
                            EndingQuests.Add(q);
                            Quests.Remove(q);
                            if (q == ActivQuest)
                                ActivQuest = FindOtherActivQest();
                            return dl;
                        }
                        else if (q.NameTo == _enemy.Name && q.item == null)
                        {
                            if (StaticVal.trecker_id_android != null)
                            {
                                Dictionary<string, string> param = new Dictionary<string, string>();
                                param["ID"] = q.id.ToString();

                                MyTracker.TrackEvent("take_habar", param);
                            }
                            if (q.NameTo == "Бармен" && q.id == 1)
                                StaticVal.peopls[1, 0] = false;
                            currentEnemyScript = _enemy;
                            Dialog dl = q.startDialog;

                            EndingQuests.Add(q);
                            Quests.Remove(q);
                            if (q == ActivQuest)
                                ActivQuest = FindOtherActivQest();
                            return dl;
                        }

                        if (q.NameFrom == _enemy.Name)
                        {
                            currentEnemyScript = _enemy;
                            return q.doingtDialog;
                        }
                    }
                   
                    if (_enemy.Name == "Volk")
                    {
                        foreach (Quest qq in EndingQuests)
                            if (qq.id == 1)
                                return null;
                        foreach (Quest qq in Quests)
                            if (qq.id == 1)
                                return null;
                    }  
                    else if (_enemy.Name == "Баян")
                    {
                        foreach (Quest qq in EndingQuests)
                            if (qq.id == 20)
                            {
                                Debug.Log("You l");
                                return null;
                            }
                                
                        foreach (Quest qq in Quests)
                            if (qq.id == 20)
                                return null;
                    }
                    else if (_enemy.Name == "Ящер")
                    {
                        foreach (Quest qq in EndingQuests)
                            if (qq.id == 40)
                                return null;
                    }


                    currentEnemyScript = _enemy;
                    return _enemy.Dialog[0];
                }
                else
                {
                    _inventory.GetAllBag(_enemy.Bag);
                    _enemyBg = col;
                    player.OpenEnemyBag();
                    return null;
                }
            }
            else if (col.CompareTag("Enemy") && col.GetComponentsInParent<EnemyMutant>().Length > 0)
            {
                _enemyMut = col.GetComponentInParent<EnemyMutant>();

                if (_enemyMut.IsDided)
                {
                    _inventory.GetAllBag(_enemyMut.Bag);
                    _enemyBg = col;
                    player.OpenEnemyBag();
                    return null;
                }
            }
        }

        return null;
    }

    public bool FindQuest(Quest findQ)
    {
        foreach (Quest q in Quests)
            if (q.id == findQ.id)
                return true;

        return false;
    }

    public Quest FindOtherActivQest()
    {
        foreach (Quest q in Quests)
            if (q.isActiv == true)
                return q;

        return null;
    }

    public Quest FindOtherQest(string _name)
    {
        foreach (Quest q in Quests)
            if (q.NameTo == _name)
                return q;

        return null;
    }

    public void Dide()
    {
        Pause();
        StartCoroutine(ScreenOpen(screenDide.gameObject, screenDide));
    }

    IEnumerator ScreenOpen(GameObject _obj, Image _img)
    {
        _obj.gameObject.SetActive(true);
        while (_img.color.a < 1)
        {
            _img.color += new Color(0, 0, 0, 0.005f);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(2f);

        StaticVal.nameSave = "/save.json";
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void SetArtPos(float x)
    {
        if (x == -100)
        {
            trArt.gameObject.SetActive(false);
            return;
        }

        trArt.gameObject.SetActive(true);
        
        if (x > -2 && x < 2)
        {
            trArt.localPosition = new Vector3(0 , trArt.localPosition.y, trArt.localPosition.z);
        }
        else
        {
            if (x < -2)
            {
                trArt.localPosition = new Vector3(x * 20, trArt.localPosition.y, trArt.localPosition.z);
            }
            else if (x > 2)
            {
                trArt.localPosition = new Vector3(x * 20, trArt.localPosition.y, trArt.localPosition.z);
            }
        }
    }

    public void OpenDetector()
    {
        StartCoroutine(SetDetectorActiv(objDetector, trDetector, 30));
        player.det.isScan = true;
    }

    public void CloseDetector()
    {
        StartCoroutine(SetDetectorActiv(objDetector, trDetector, -30));
        player.det.isScan = false;
    }

    IEnumerator SetDetectorActiv(GameObject _obj, RectTransform _tr, int num)
    {
        if (num > 0) 
            _tr.localPosition = new Vector3(0, -2000, 0);
        _obj.SetActive(true);
        while ((_tr.localPosition.y < 0 && num > 0) || (_tr.localPosition.y > -2000 && num < 0))
        {
            _tr.localPosition += new Vector3(0, num, 0);
            yield return new WaitForSeconds(0.005f);
        }

        if (num < 0)
            _obj.SetActive(false);
    }

    public void StartPrintNoPve()
    {
        StartCoroutine(SetAlfaNoPve());
    }

    IEnumerator SetAlfaNoPve()
    {
        imgNoPve.gameObject.SetActive(true);
        imgNoPve.color = new Color(imgNoPve.color.r, imgNoPve.color.g, imgNoPve.color.b, 1);
        textNoPve.color = new Color(textNoPve.color.r, textNoPve.color.g, textNoPve.color.b, 1);
        yield return new WaitForSeconds(1f);
        while (imgNoPve.color.a >= 0)
        {
            imgNoPve.color = new Color(imgNoPve.color.r, imgNoPve.color.g, imgNoPve.color.b, imgNoPve.color.a - 0.01f);
            textNoPve.color = new Color(textNoPve.color.r, textNoPve.color.g, textNoPve.color.b, textNoPve.color.a - 0.01f);
            yield return new WaitForEndOfFrame();
        }
        imgNoPve.gameObject.SetActive(false);
    }

    public void SetUpAlfa(Image _img)
    {
        _img.color = new Color(_img.color.r, _img.color.g, _img.color.b, StaticVal.alfaUi + 0.25f);
    }

    public void SetDownAlfa(Image _img)
    {
        _img.color = new Color(_img.color.r, _img.color.g, _img.color.b, StaticVal.alfaUi);
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        AudioListener.volume = 1.0f;
        _isPause = false;
    }

    public void Pause()
    {
        Time.timeScale = 0.0f;
        AudioListener.volume = 0.0f;
        _isPause = true;
    }

    public void ResumeAd(object sender, EventArgs args)
    {
        Time.timeScale = 1.0f;
        AudioListener.volume = 1.0f;
        _isPause = false;
    }

    public void PauseAd(object sender, EventArgs args)
    {
        Time.timeScale = 0.0f;
        AudioListener.volume = 0.0f;
        _isPause = true;
    }

    public void UpdateDialog(Dialog _dialog)
    {
        textDialog.text = _dialog.title;

        buttonDiscription1.gameObject.SetActive(false);
        buttonDiscription2.gameObject.SetActive(false);
        buttonDiscription3.gameObject.SetActive(false);

        if (_dialog.money > 0)
        {
            StaticVal.money += _dialog.money;
        }

        if (_dialog.item != null)
        {
            _inventory.AddItemsToNull(_dialog.item, _dialog.countItem, 75);
        }

        if (_dialog.descriptions.Length >= 1)
        {
            buttonDiscription1.gameObject.SetActive(true);
            textDiscription1.text = _dialog.descriptions[0];
            if (_dialog.typeDescriptions[0] == TypeDescription.Dialog)
            {
                imgDiscription1.gameObject.SetActive(false);
            }
            else
            {
                imgDiscription1.gameObject.SetActive(true);
                if (_dialog.typeDescriptions[0] == TypeDescription.Quest)
                {
                    imgDiscription1.sprite = spriteQuest;
                }
                else if (_dialog.typeDescriptions[0] == TypeDescription.Exit)
                {
                    imgDiscription1.sprite = spriteExit;
                }
                else if (_dialog.typeDescriptions[0] == TypeDescription.Store)
                {
                    imgDiscription1.sprite = spriteStore;
                }
                else if (_dialog.typeDescriptions[0] == TypeDescription.Habar)
                {
                    imgDiscription1.sprite = spriteHabar;
                }
            }
        }
        if (_dialog.descriptions.Length >= 2)
        {
            buttonDiscription2.gameObject.SetActive(true);
            textDiscription2.text = _dialog.descriptions[1];
            if (_dialog.typeDescriptions[1] == TypeDescription.Dialog)
            {
                imgDiscription2.gameObject.SetActive(false);
            }
            else
            {
                imgDiscription2.gameObject.SetActive(true);
                if (_dialog.typeDescriptions[1] == TypeDescription.Quest)
                {
                    imgDiscription2.sprite = spriteQuest;
                }
                else if (_dialog.typeDescriptions[1] == TypeDescription.Exit)
                {
                    imgDiscription2.sprite = spriteExit;
                }
                else if (_dialog.typeDescriptions[0] == TypeDescription.Store)
                {
                    imgDiscription2.sprite = spriteStore;
                }
                else if (_dialog.typeDescriptions[0] == TypeDescription.Habar)
                {
                    imgDiscription2.sprite = spriteHabar;
                }
            }
        }
        if (_dialog.descriptions.Length >= 3)
        {
            buttonDiscription3.gameObject.SetActive(true);
            textDiscription3.text = _dialog.descriptions[2];
            if (_dialog.typeDescriptions[2] == TypeDescription.Dialog)
            {
                imgDiscription3.gameObject.SetActive(false);
            }
            else
            {
                imgDiscription3.gameObject.SetActive(true);
                if (_dialog.typeDescriptions[2] == TypeDescription.Quest)
                {
                    imgDiscription3.sprite = spriteQuest;
                }
                else if (_dialog.typeDescriptions[2] == TypeDescription.Exit)
                {
                    imgDiscription3.sprite = spriteExit;
                }
                else if (_dialog.typeDescriptions[0] == TypeDescription.Store)
                {
                    imgDiscription3.sprite = spriteStore;
                }
                else if (_dialog.typeDescriptions[0] == TypeDescription.Habar)
                {
                    imgDiscription3.sprite = spriteHabar;
                }
            }
        }
    }

    public void SetActivButtons(bool isActiv)
    {
        for (int i = 0; i < ets.Length; i++)
        {
            ets[i].StopAllCoroutines();
        }
    }

    public void OnClickButton(int _button)
    {
        if (dialog != null)
        {
            if (dialog.typeDescriptions[_button] == TypeDescription.Dialog)
            {
                UpdateDialog(dialog.descriptionsObject[_button]);
                dialog = dialog.descriptionsObject[_button];
            }
            else if (dialog.typeDescriptions[_button] == TypeDescription.Quest)
            {
                Quests.Add(dialog.quest[_button]);
                ActivQuest = dialog.quest[_button];
                if (dialog.name == "Бармен" && dialog.id == 2 && !StaticVal.peopls[1, 4])
                {
                    StaticVal.peopls[1, 1] = true;
                    StaticVal.peopls[1, 0] = false;
                } 
                if (dialog.name == "Бармен" && dialog.id == 2 && !StaticVal.peopls[1, 4])
                    StaticVal.peopls[1, 1] = true;
                if (dialog.name == "Ящер" && (dialog.id == 173 || dialog.id == 175))
                    StaticVal.peopls[3, 5] = true;
                if (dialog.name == "Алекс" && dialog.id == 75)
                {
                    currentEnemyScript.Dialog[0] = null;
                    StaticVal.peopls[3, 3] = true;
                    StaticVal.peopls[3, 4] = true;
                    StaticVal.peopls[1, 1] = false;
                    StaticVal.peopls[1, 5] = false;
                    StaticVal.peopls[4, 4] = true;
                }
                onDialog = false;
                screenDialog.SetActive(false);
            }
            else if (dialog.typeDescriptions[_button] == TypeDescription.Store)
            {
                player.OpenShop(itm);
                onDialog = false;
                screenDialog.SetActive(false);
            }
            else if (dialog.typeDescriptions[_button] == TypeDescription.Habar)
            {
                player.OpenHabar();
                onDialog = false;
                screenDialog.SetActive(false);
            }
            else if (dialog.typeDescriptions[_button] == TypeDescription.Lok)
            {
                if (dialog.name == "Проводник" && dialog.id != 55)
                {
                    player.Save(new Vector2(-99, 1), forPosSky.localPosition, StaticVal.light);
                    StaticVal.nameSave = "/sceneData.json";
                    Resume();
                    SceneManager.LoadScene("Poliana", LoadSceneMode.Single);
                }
                else if (dialog.name == "Проводник")
                {
                    player.Save(new Vector2(-86, 1), forPosSky.localPosition, StaticVal.light);
                    StaticVal.nameSave = "/sceneData.json";
                    Resume();
                    StaticVal.peopls[3, 0] = true;
                    StaticVal.peopls[3, 1] = true;
                    StaticVal.peopls[3, 2] = true;
                    SceneManager.LoadScene("NewTirretor", LoadSceneMode.Single);
                }
            }
            else
            {
                onDialog = false;
                screenDialog.SetActive(false);
                if (dialog.name == "Бармен" && dialog.id == 10)
                {
                    StaticVal.peopls[3, 0] = false;
                    StaticVal.peopls[3, 1] = false;
                    StaticVal.peopls[3, 2] = false;
                } 

                if (dialog.name == "Администратор")
                {
                    StaticVal.peopls[1, 5] = false;
                    StaticVal.peopls[1, 4] = true;
                    StaticVal.peopls[3, 4] = true;
                    StaticVal.peopls[1, 1] = false;
                    StaticVal.peopls[3, 0] = false;
                    StaticVal.peopls[3, 1] = false;
                    StaticVal.peopls[3, 2] = false;
                }
            }
        }
    }

    public void SetBag(List<ItemInventory> items)
    {
        if (_enemyBg != null)
        {
            if (_enemyBg.GetComponentsInParent<EnemyMutant>().Length > 0)
                _enemyBg.GetComponentInParent<EnemyMutant>().Bag = items;
            else
                _enemyBg.GetComponentInParent<Enemy>().Bag = items;
        }

        _enemyBg = null;
    }

    public void OpenongBagOrDialog()
    {
        if (!onDialog)
        {
            Dialog _d = GetDialog(transform.position);
            if (_d != null)
            {
                onDialog = true;
                screenDialog.SetActive(true);
                dialog = _d;
                UpdateDialog(dialog);
            }
            else
                dialog = null;
        }
        else
        {
            onDialog = false;
            screenDialog.SetActive(false);
        }
    }
}
