using System.Collections.Generic;
using UnityEngine;

public class EnemyMutant : MonoBehaviour
{
    [SerializeField] public bool IsDided = false;
    [SerializeField] public Object didePrefb;
    [SerializeField] private int xp;
    [Header("Move")]
    [SerializeField] private MoveedObject mObject;
    [SerializeField] private BoxCollider2D mCollider;
    [SerializeField] private Animator _anim;
    [Header("AI")]
    [SerializeField] public Transform target;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float sizeCheckMove;
    [SerializeField] private float sizeCheckShoot;
    [Header("Attack")]
    [SerializeField] private float dm = 10;
    [Header("Bag")]
    [SerializeField] public List<ItemInventory> Bag = new List<ItemInventory>();
    [SerializeField] public List<Item> toBag = new List<Item>();

    public MoveedObject _Object;
    private Vector3 _vector;
    private Rigidbody2D _rb;

    private float _timeBtwShot = 1f;
    public bool _isShoot = false;
    private bool ex = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _Object = Clone(mObject, _rb, transform, null);

        Bag.Add(AddItem(toBag[0], 1, 75));
    }

    private void Update()
    {
        if (!IsDided)
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
                    if (_timeBtwShot <= 0 && _isShoot)
                    {
                        _anim.SetTrigger("isAttack");
                        _timeBtwShot = 1;
                        col.GetComponentInParent<Player>()._Object.TakeDamage(dm, TypeDamage.Gap);
                    }
                }
            }

            if (_timeBtwShot > 0)
            {
                _timeBtwShot -= Time.deltaTime;
            }
        }

        if (_Object.Health <= 0)
        {
            mCollider.offset = new Vector2(0, -0.7f);
            _vector = Vector3.zero;
            IsDided = true;
            transform.eulerAngles = new Vector3(0, 0, -180);

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
                                    exPop.str = "+" + (xp + 2) + " ќпыта";
                                    StaticVal.notSelectedXP += (xp + 2);
                                }
                                else if (s.LevelModifier == 2)
                                {
                                    exPop.str = "+" + (xp + 4) + " ќпыта";
                                    StaticVal.notSelectedXP += (xp + 4);
                                }
                                else if (s.LevelModifier == 3)
                                {
                                    exPop.str = "+" + (xp + 6) + " ќпыта";
                                    StaticVal.notSelectedXP += (xp + 6);
                                }
                            }
                        }
                    }
                }

                if (exPop.str == null || exPop.str == "")
                {
                    exPop.str = "+" + xp + " ќпыта";
                    StaticVal.notSelectedXP += xp;
                }

                ex = true;
            }
        }

        if (IsDided && !FindNullItem())
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (!_isShoot)
            _Object.Move(_vector, true);
        else
            _Object.Move(_vector, false);

        if (_vector.x != 0)
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

        ii.item = item; //даЄм ади
        ii.cond = cond;
        ii.count = count;    //даЄм количество
        Debug.Log(item);
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
