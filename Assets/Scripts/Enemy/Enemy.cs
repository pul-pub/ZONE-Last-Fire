using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public bool IsEnemyForPlayer = true;
    [SerializeField] public bool IsDided = false;
    [SerializeField] public bool IsMoveedEnemy = true;
    [SerializeField] public string Name;
    [SerializeField] public Object didePrefb;
    [SerializeField] private int xp;
    [Space]
    [Header("Move")]
    [SerializeField] private MoveedObject mObject;
    [SerializeField] private BoxCollider2D mCollider;
    [SerializeField] private Animator _anim;
    [SerializeField] private Animator _animArm;
    [Header("AI")]
    [SerializeField] public Transform target;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float sizeCheckMove;
    [SerializeField] private float sizeCheckShoot;
    [Header("Weapon")]
    [SerializeField] private GameObject weapon;
    [SerializeField] private Gun gun;
    [SerializeField] private SpriteRenderer spriteWeapon;
    [SerializeField] private GameObject store;
    [SerializeField] private GameObject storeReload;
    [SerializeField] private Object bullet;
    [SerializeField] public Transform parent;
    [SerializeField] private GameObject pointStart;
    [Header("Armor")]
    [SerializeField] private Armor armor;
    [SerializeField] private bool isAvtomaticalSetHeader;
    [SerializeField] private SpriteRenderer body;
    [SerializeField] private SpriteRenderer head;
    [SerializeField] private SpriteRenderer rig;
    [Header("Dialogs and Quests")]
    [SerializeField] public Dialog[] Dialog;
    [SerializeField] public Quest Quests;
    [SerializeField] public bool insShop = false;
    [SerializeField] public List<Item> shop = new List<Item>();
    [Header("Bag")]
    [SerializeField] public List<ItemInventory> Bag = new List<ItemInventory>();
    [SerializeField] private bool RandomBag = false;
    [SerializeField] public List<Item> toBag = new List<Item>();
    [SerializeField] public Item ammos;
    [SerializeField] public Item medic;
    [SerializeField] public Item pda;
    [SerializeField] public Item eat;

    public MoveedObject _Object;
    private Vector3 _vector;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteStoreReload;
    private SpriteRenderer _spriteStore;
    private Transform _transformStore;

    private float _timeBtwShot = 1f;
    public bool _isShoot = false;
    public bool _flagGun = true;
    private bool _isReload = false;
    private bool _isDl = false;
    private Gun _gun;
    private Camera _camera;
    private bool ex = false;

    private void Awake()
    {
        _camera = Camera.main;
        _spriteStore = store.GetComponent<SpriteRenderer>();
        _spriteStoreReload = storeReload.GetComponent<SpriteRenderer>();
        _transformStore = store.GetComponent<Transform>();
        _rb = GetComponent<Rigidbody2D>();

        _Object = Clone(mObject, _rb, transform, mCollider);

        if (RandomBag)
        {
            if (ammos != null)
            {
                Bag.Add(AddItem(ammos, UnityEngine.Random.Range(1, 10), 100));
            }
            if (medic != null)
            {
                if (UnityEngine.Random.Range(0, 3) == 0)
                    Bag.Add(AddItem(medic, 1, 100));
            }
            if (pda != null)
            {
                if (UnityEngine.Random.Range(0, 10) == 0)
                    Bag.Add(AddItem(pda, 1, UnityEngine.Random.Range(10, 45)));
            }
            if (eat != null)
            {
                if (UnityEngine.Random.Range(0, 6) == 0)
                    Bag.Add(AddItem(medic, 1, 100));
            }
        }
        else
        {
            for (int i = 0; i < toBag.Count; i++)
            {
                Bag.Add(AddItem(toBag[i], Random.Range(1, 10), Random.Range(10, 40)));
            }
        }

        _Object.armor = armor;
        body.sprite = armor.spriteBody;
        if (isAvtomaticalSetHeader) 
            head.sprite = armor.spriteHead;
        rig.sprite = armor.spriteRig;
        _gun = gun.Clone();
        _gun.startTimeBtwShot += 0.75f;

        Reload();
    }

    private void Update()
    {
        SetGun();
        if (!IsDided && IsEnemyForPlayer && IsMoveedEnemy)
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, sizeCheckMove, whatIsGround);
            Collider2D[] cols2 = Physics2D.OverlapCircleAll(transform.position, sizeCheckShoot, whatIsGround);

            foreach (Collider2D col in cols)
            {
                if (col.gameObject.CompareTag("Player"))
                {
                    _vector = new Vector3((target.position - transform.position).normalized.x, 0);
                    _isShoot = false;
                }
            }
            foreach (Collider2D col in cols2)
            {
                if (col.gameObject.CompareTag("Player"))
                {
                    _vector = new Vector3((target.position - transform.position).normalized.x, 0);
                    _isShoot = true;
                }
            }

            if (_Object.Health < 100)
            {
                _vector = new Vector3((target.position - transform.position).normalized.x, 0);
                _isShoot = true;
            }

            float x_cam = _camera.WorldToScreenPoint(transform.position).x;
            if (x_cam < -50 || x_cam > _camera.pixelWidth + 50)
            {
                _isShoot = false;
            }

            if (_timeBtwShot <= 0)
            {
                if (_flagGun == true && _gun != null && _gun.currentAmmos >= -20 && !_isReload && _isShoot)
                {
                    _gun.Shoot(bullet, parent, pointStart, TypeBullet.Player);
                    _timeBtwShot = _gun.startTimeBtwShot;
                }
            }
            else if (_timeBtwShot > 0)
            {
                _timeBtwShot -= Time.deltaTime;
            }
        }


        if (_Object.Health <= 0)
        {
            _vector = Vector3.zero;
            _rb.velocity = Vector3.zero;
            _rb.gravityScale = 0f;
            mCollider.enabled = false;
            IsDided = true;
            _flagGun = false;
            transform.position = new Vector2(transform.position.x, -0.5f);
            transform.eulerAngles = new Vector3(0, 0, -90);

            if (!ex)
            {
                GameObject popup = Instantiate(didePrefb, transform.position, Quaternion.identity) as GameObject;
                ExperiencePopup exPop = popup.GetComponent<ExperiencePopup>();
                popup.transform.SetParent(null);

                foreach (int i in StaticVal.idSkills)
                {
                    if (i != -1)
                    {
                        foreach (Skill s in StaticVal.dataBase.skils)
                        {
                            if (s.Modifier == TypeModifier.Lerning)
                            {
                                if (s.LevelModifier == 1)
                                {
                                    exPop.str = "+" + (xp + 2) + " Опыта"; 
                                    StaticVal.notSelectedXP += (xp + 2);
                                }
                                else if (s.LevelModifier == 2)
                                {
                                    exPop.str = "+" + (xp + 4) + " Опыта";
                                    StaticVal.notSelectedXP += (xp + 4);
                                }
                                else if (s.LevelModifier == 3)
                                {
                                    exPop.str = "+" + (xp + 6) + " Опыта";
                                    StaticVal.notSelectedXP += (xp + 6);
                                }
                            }
                        }
                    }
                }

                if (exPop.str == null || exPop.str == "")
                {
                    exPop.str = "+" + xp + " Опыта";
                    StaticVal.notSelectedXP += xp;
                }

                ex = true;
            }
        }

        if (Name == "Проводник"  && !_isDl)
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 1, whatIsGround);

            foreach (Collider2D col in cols)
            {
                if (col.CompareTag("Player"))
                {
                    target.gameObject.GetComponent<Player>().PressedF();
                    _isDl = true;
                }
            }       
        }
        if (Name == "Майор" && !_isDl)
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 0.1f, whatIsGround);

            foreach (Collider2D col in cols)
            {
                if (col.CompareTag("Player"))
                {
                    target.gameObject.GetComponent<Player>().PressedF();
                    _isDl = true;
                }
            }
        }
        if (Name == "Алекс" && !_isDl)
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 0.1f, whatIsGround);

            foreach (Collider2D col in cols)
            {
                if (col.CompareTag("Player"))
                {
                    target.gameObject.GetComponent<Player>().PressedF();
                    _isDl = true;
                }
            }
        }
        if (Name == "Дед" && !_isDl)
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 0.1f, whatIsGround);

            foreach (Collider2D col in cols)
            {
                if (col.CompareTag("Player"))
                {
                    target.gameObject.GetComponent<Player>().PressedF();
                    _isDl = true;
                }
            }
        }

        if (IsDided && !FindNullItem())
        {
            if (Name == "Труп-1")
                StaticVal.peopls[3, 3] = false;
            else
                Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (!_isShoot) 
            _Object.Move(_vector, true);
        else
            _Object.Move(_vector, false);

        if (_vector.x != 0 && !_isShoot)
        {
            _anim.SetBool("isGo", true);
        }
        else
        {
            _anim.SetBool("isGo", false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, sizeCheckMove);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, sizeCheckShoot);
    }

    private void SetGun()
    {
        if (_flagGun)
        {
            weapon.SetActive(true);
            _animArm.SetBool("OnGun", true);
        }
        else
        {
            weapon.SetActive(false);
            _animArm.SetBool("OnGun", false);
        }

        UpdateSpriteWeapons();
    }

    private void UpdateSpriteWeapons() 
    {
        if (_gun != null)
        {
            spriteWeapon.sprite = _gun.img;

            _spriteStoreReload.sprite = _gun.imgStore;
            _transformStore.localPosition = _gun.pointOfGraficStore;
            _spriteStore.sprite = _gun.imgStore;
            store.gameObject.SetActive(_gun.isGraficStore);
        }
    }

    public void Reload()
    {
        _gun.Reload(100);
        _isReload = false;
    }

    private bool FindNullItem()
    {
        foreach (ItemInventory ii in Bag)
            if (ii.item.id != 0)
                return true;

        return false;
    }

    private MoveedObject Clone(MoveedObject old, Rigidbody2D _rb, Transform _tr, BoxCollider2D _colliderLeg)
    {
        MoveedObject New = new MoveedObject();

        New.rb = _rb;
        New.tr = _tr;
        New.colliderLeg = _colliderLeg;

        New.Health = old.Health;
        New.Speed = old.Speed;
        New.ForceJamp = old.ForceJamp;
        New.WhatIsGround = old.WhatIsGround;
        New.toRight = old.toRight;
        New.isSit = old.isSit;

        return New;
    }

    public ItemInventory AddItem(Item item, int count, int cond)
    {
        ItemInventory ii = new ItemInventory();

        ii.item = item; //даём ади
        ii.cond = cond;
        ii.count = count;    //даём количество
        if (ii.count > 1 && (item.type == TypeItem.Weapon || item.type == TypeItem.Cothl))
        {
            ii.count = 1;
        }
        if (ii.count > 128 && ii.item.type == TypeItem.Ammo)
        {
            ii.count = 128;
        }
        else if (ii.count > 4 && (ii.item.type == TypeItem.Eat || ii.item.type == TypeItem.Medical))
        {
            ii.count = 4;
        }
        else if (ii.count > 8 && (ii.item.type == TypeItem.PDA || ii.item.type == TypeItem.Pnv ||
            ii.item.type == TypeItem.Detector || ii.item.type == TypeItem.Habar ||
            ii.item.type == TypeItem.Repair))
        {
            ii.count = 8;
        }

        return ii;
    }
}
