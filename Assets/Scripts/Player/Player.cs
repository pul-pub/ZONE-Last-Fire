using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Player : MonoBehaviour
{
    public event System.Action<TypeInventory> OnSetActivInventory;
    public event System.Action OnSetActivDialog;

    [Header("Move")]
    [SerializeField] private MoveedObject mObject;
    [SerializeField] private BoxCollider2D mCollider;
    [SerializeField] private Animator _anim;

    [Header("Input")]
    [SerializeField] private GameObject buttonUp;
    [SerializeField] private GameObject buttonDown;

    [Header("Scripts")]
    [SerializeField] public PlayerInterface _playerInterface;
    [SerializeField] public Inventory inventory;
    [SerializeField] public CamController controller;
    [SerializeField] public Detector det;
    [SerializeField] public DataBase data;

    public ObjectWeapon _weapon;

    public MoveedObject _Object;
    private Vector3 _vector;
    private Rigidbody2D _rb;
    private AudioSource _audioSource;
    public bool touchF = false;

    private float timer = 1f;

    private void OnEnable()
    {
        inventory.OnSetWeapon += _weapon.FindWeaponOnInventory;
        inventory.OnSetWeapon += _weapon.OnSetArmors;
        inventory.Loaded += _weapon.Load;
    }

    private void OnDisable()
    {
        inventory.OnSetWeapon -= _weapon.FindWeaponOnInventory;
        inventory.OnSetWeapon -= _weapon.OnSetArmors;
        inventory.Loaded -= _weapon.Load;
    }


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _audioSource = _anim.gameObject.GetComponent<AudioSource>();
        _weapon = GetComponentInChildren<ObjectWeapon>();
        det = GetComponent<Detector>();
        _Object = Clone(mObject, _rb, transform, mCollider);
        controller = inventory.gameObject.GetComponent<CamController>();

        if (StaticVal.nameSave != null)
        {
            string json = File.ReadAllText(Application.persistentDataPath + StaticVal.nameSave);
            ObjectSave save = JsonUtility.FromJson<ObjectSave>(json);

            transform.position = save.pos;
            _Object.Health = save.health;
            _Object.Arm = save.armor;
            //StaticVal.peopls = save.peoples;
        }
    }

    void Update()
    {
        SetGun();
        if (StaticVal.type == TypePlatform.PC)
        {
            _vector = new Vector3(Input.GetAxis("Horizontal"), 0);

            if (Input.GetMouseButton(0))
                _weapon.Shoot(true);
            else
                _weapon.Shoot(false);

            if (Input.GetKeyDown(KeyCode.R))
                _weapon.Reloading();

            if (Input.GetKeyDown(KeyCode.F))
                OnSetActivDialog.Invoke();

            if (Input.GetKeyDown(KeyCode.E))
                OnSetActivInventory.Invoke(TypeInventory.Outfit);

            if (Input.GetKeyDown(KeyCode.Q))
                _weapon._anim.SetTrigger("Bolt");


            if (Input.GetKeyDown(KeyCode.S) && !_Object.isSit)
            {
                _Object.SitDowen(new Vector2(0, 0.35f));
                _anim.SetBool("sit", _Object.isSit);
            }
            else if (Input.GetKeyDown(KeyCode.Space) && !_Object.isSit)
            {
                OnUp();
            }
            else if (Input.GetKeyDown(KeyCode.Space) && _Object.isSit)
            {
                _Object.SendUp(new Vector2(0, 0));
                _anim.SetBool("sit", _Object.isSit);
            }
        }

        if (_Object.Health <= 0)
            _playerInterface.Dide();

        if (_Object.Health < 100)
        {
            if (timer >= 0)
                timer -= Time.deltaTime;
            else
            {
                _Object.Health += 1f;
                timer = 2f;
            }
        }
    }

    public void PressedF()
    {
        OnSetActivDialog.Invoke();
    }

    private void FixedUpdate()
    {
        _Object.Move(_vector, true);

        if (_vector.x != 0 && !_Object.isSit)
        { 
            _anim.SetBool("isGo", true);
            if (!_audioSource.isPlaying)
            {
                _audioSource.time = 0;
                _audioSource.Play();
            }
        }
        else
        {
            _anim.SetBool("isGo", false);
            if (_audioSource.isPlaying)
                _audioSource.Stop();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, new Vector3(1, 3f, 0));
    }

    private void SetGun()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            _weapon.OnGunOne();
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            _weapon.OnGunTwo();

        _weapon.UpdateWeapon();
    }

    private MoveedObject Clone(MoveedObject old, Rigidbody2D _rb, Transform _tr, BoxCollider2D _colliderLeg)
    {
        MoveedObject New = new MoveedObject();

        New.rb = _rb;
        New.tr = _tr;
        New.colliderLeg = _colliderLeg;

        New.Health = old.Health;
        New.Arm = old.Arm;
        New.Speed = old.Speed;
        New.ForceJamp = old.ForceJamp;
        New.WhatIsGround = old.WhatIsGround;
        New.toRight = old.toRight;
        New.isSit = old.isSit;

        return New;
    }

    public void Save(Vector2 pos, Vector2 posSky, float lights, bool isSave = false, string _name = "/sceneData.json")
    {
        ObjectSave save = new ObjectSave();

        int[] idItem = new int[29];
        int[] coutItem = new int[29];
        int[] condItem = new int[29];
        int[] indificatorItem = new int[29];
        int[] listQests = new int[_playerInterface.Quests.Count];
        int[] listEndingQests = new int[_playerInterface.EndingQuests.Count];
        bool[,] peoples = new bool[8, 8];

        if (_weapon._guns[0] != null)
            save.currentAmmos[0] = _weapon._guns[0].currentAmmos;
        if (_weapon._guns[1] != null)
            save.currentAmmos[1] = _weapon._guns[1].currentAmmos;

        for (int i = 0; i < 8; i++)
            save.peopls1[i] = StaticVal.peopls[1, i];
        for (int i = 0; i < 8; i++)
            save.peopls2[i] = StaticVal.peopls[2, i];
        for (int i = 0; i < 8; i++)
            save.peopls3[i] = StaticVal.peopls[3, i];

        for (int i = 0; i < 29; i++) 
        {
            idItem[i] = inventory.items[i].item.id;
        }
        for (int i = 0; i < 29; i++)
        {
            coutItem[i] = inventory.items[i].count;
        }
        for (int i = 0; i < 29; i++)
        {
            condItem[i] = inventory.items[i].cond;
        }
        for (int i = 0; i < 29; i++)
        {
            indificatorItem[i] = inventory.items[i].INDIFICATOR;
        }
        for (int i = 0; i < _playerInterface.Quests.Count; i++)
        {
            listQests[i] = _playerInterface.Quests[i].id;
        }
        for (int i = 0; i < _playerInterface.EndingQuests.Count; i++)
        {
            listEndingQests[i] = _playerInterface.EndingQuests[i].id;
        }
        save.idQuests = listQests;
        save.idEndingQuests = listEndingQests;
        if (_playerInterface.ActivQuest != null)
            save.idActivQuest = _playerInterface.ActivQuest.id;
        else
            save.idActivQuest = -1;
        save.idItem = idItem;
        save.coutItem = coutItem;
        save.condItem = condItem;
        save.indificatorItem = indificatorItem;
        save.falgGun = _weapon._flagGun;
        save._numGun = _weapon._numGun;
        save.pos = pos;
        save.time = StaticVal.time;
        save.isRain = StaticVal.idRain;
        save.lights = lights;
        save.posSky = posSky;
        //save.peoples = peoples;
        save.isSave = isSave;
        save.health = _Object.Health;
        save.armor = _Object.Arm;
        save.money = StaticVal.money;
        if (isSave)
        {
            save.indexScene = SceneManager.GetActiveScene().buildIndex;
        }

        save.alfaUi = StaticVal.alfaUi;
        save.volSound = StaticVal.volSound;
        save.vibroMode = StaticVal.vibroMode;
        save.promptMode = StaticVal.promptMode;

        string json = JsonUtility.ToJson(save, true);
        File.WriteAllText(Application.persistentDataPath + _name, json);
    }

    public void OpenShop(List<Item> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].type == TypeItem.Ammo)
                inventory.AddItem(inventory.items, 25 + 15 + 4 + i, items[i], 10, 100);
            else
                inventory.AddItem(inventory.items, 25 + 15 + 4 + i, items[i], 1, 100);
        }
        OnSetActivInventory.Invoke(TypeInventory.Shop);
    }

    public void OpenEnemyBag()
    {
        OnSetActivInventory.Invoke(TypeInventory.EnemyBag);
    }

    public void OpenHabar()
    {
        OnSetActivInventory.Invoke(TypeInventory.Habar);
    }

    public void LoadSave()
    {
        string json = File.ReadAllText(Application.persistentDataPath + StaticVal.nameSave);
        ObjectSave save = JsonUtility.FromJson<ObjectSave>(json);
    }

    public void Medic(TypeMediac type)
    {
        if (type == TypeMediac.Bandage)
            _Object.Health += 8;
        else if (type == TypeMediac.Standart)
            _Object.Health += 20;
        else if (type == TypeMediac.Arma)
            _Object.Health += 48;
        else if (type == TypeMediac.Scientist)
            _Object.Health += 60;

        if (_Object.Health > 100)
            _Object.Health = 100;
    }

    //-------------------------Mobil Input------------------------------

    public void OnLeft()
    {
        _vector = new Vector3(-1, 0, 0);
    }

    public void OnLeftDown()
    {
        _vector = Vector3.zero;
    }

    public void OnRight()
    {
        _vector = new Vector3(1, 0, 0);
    }

    public void OnRightDown()
    {
        _vector = Vector3.zero;
    }

    public void OnUp()
    {
        if (!_Object.isSit)
        {
            _Object.Jamp();
            _anim.SetTrigger("Jump");
        }
        else if (_Object.isSit)
        {
            _Object.SendUp(new Vector2(0, 0));
            _anim.SetBool("sit", _Object.isSit);
            controller.Up();
            buttonDown.SetActive(true);
        }
    }

    public void OnDown()
    {
        if (!_Object.isSit)
        {
            _Object.SitDowen(new Vector2(0, 0.35f));
            _anim.SetBool("sit", _Object.isSit);
            controller.Dowen();
            buttonDown.SetActive(false);
        }
    }

    public void OnBolt()
    {
        if (!_weapon._isReload && !_weapon._isShoot)
        {
            _weapon._anim.SetTrigger("Bolt");
        }
    }

    public void OnOpenInv()
    {
        OnSetActivInventory.Invoke(TypeInventory.Outfit);
    }

    public void OnCloseInv()
    {
        OnSetActivInventory.Invoke(TypeInventory.Outfit);
    }

    public void OnCloseHabar()
    {
        OnSetActivInventory.Invoke(TypeInventory.CLOSE);
    }
}
