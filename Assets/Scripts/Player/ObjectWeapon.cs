using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectWeapon : MonoBehaviour
{
    [Header("Sprits Weapon")]
    [SerializeField] private GameObject weapon;
    [SerializeField] private Transform hend;
    [SerializeField] private GameObject storeOnWeapon;
    [SerializeField] private GameObject storeForReload;

    private SpriteRenderer _weaponSprit;
    private SpriteRenderer _storeOnWeaponSR;
    private SpriteRenderer _storeForReloadSR;

    [Header("Shooting")]
    [SerializeField] private Object _bullet;
    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _pointStart;

    [Header("Armors")]
    [SerializeField] private SpriteRenderer body;
    [SerializeField] private SpriteRenderer head;
    [SerializeField] private SpriteRenderer rig;

    [SerializeField] public Animator _anim;

    [SerializeField] private Inventory _inventory;

    private Player player;
    private AudioSource _audioSource;   
    public Gun[] _guns = new Gun[2];
    public int _numGun = 0;
    private float _timeBtwShot = 1f;
    public bool _flagGun = true;
    
    public bool _isReload = false;
    public bool _isShoot = false;

    private void Awake()
    {
        _weaponSprit = weapon.GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();

        _storeOnWeaponSR = storeOnWeapon.GetComponent<SpriteRenderer>();
        _storeForReloadSR = storeForReload.GetComponent<SpriteRenderer>();

        player = GetComponentInParent<Player>();

    }

    private void Update()
    {
        if (_isShoot)
            Shooting();

        if (_timeBtwShot >= 0f)
            _timeBtwShot -= Time.deltaTime;
    }

    public void Load()
    {
        string json = File.ReadAllText(Application.persistentDataPath + StaticVal.nameSave);
        ObjectSave save = JsonUtility.FromJson<ObjectSave>(json);

        _numGun = save._numGun;
        _flagGun = save.falgGun;
        FindWeaponOnInventory();
        OnSetArmors();
        UpdateWeapon();
        if (_guns[0] != null)
            _guns[0].currentAmmos = save.currentAmmos[0];
        if (_guns[1] != null)
            _guns[1].currentAmmos = save.currentAmmos[1];
    }

    public void FindWeaponOnInventory()
    {
        Gun[] guns_2 = _inventory.FindGun();
        if (guns_2[0] == null && _guns[0] != null)
        {
            foreach (Item i in player.data.items)
            {
                if (i.type == TypeItem.Ammo && i.typeAmmo == _guns[0].typeAmmo && _guns[0].currentAmmos > 0)
                    _inventory.AddItemsToNull(i, _guns[0].currentAmmos, 100);
            }
        }
        if (guns_2[1] == null && _guns[1] != null)
        {
            foreach (Item i in player.data.items)
            {
                if (i.type == TypeItem.Ammo && i.typeAmmo == _guns[1].typeAmmo && _guns[1].currentAmmos > 0)
                    _inventory.AddItemsToNull(i, _guns[1].currentAmmos, 100);
            }
        }
        _guns = guns_2;
        UpdateWeapon();
    }

    public void UpdateWeapon()
    {
        if (_guns[_numGun] == null)
            _flagGun = false;

        if (_flagGun)
        {
            weapon.SetActive(true);
            _anim.SetBool("OnGun", true);
        }
        else
        {
            weapon.SetActive(false);
            _anim.SetBool("OnGun", false);
        }

        if (_guns[_numGun] != null)
        {
            _weaponSprit.sprite = _guns[_numGun].img;

            _storeForReloadSR.sprite = _guns[_numGun].imgStore;

            storeOnWeapon.gameObject.SetActive(_guns[_numGun].isGraficStore);
            storeOnWeapon.transform.localPosition = _guns[_numGun].pointOfGraficStore;
            _storeOnWeaponSR.sprite = _guns[_numGun].imgStore;
        }
    }

    public void OnGunOne()
    {
        if (_numGun != 0)
        {
            _numGun = 0;
            _flagGun = true;
        }
        else if (_numGun == 0 && _flagGun)
        {
            _flagGun = false;
        }
        else if (_numGun == 0 && !_flagGun)
        {
            _flagGun = true;
        }

        if (_guns[0] == null)
        {
            _flagGun = false;
        }
    }

    public void OnGunTwo()
    {
        if (_numGun != 1)
        {
            _numGun = 1;
            _flagGun = true;
        }
        else if (_numGun == 1 && _flagGun)
        {
            _flagGun = false;
        }
        else if (_numGun == 1 && !_flagGun)
        {
            _flagGun = true;
        }

        if (_guns[1] == null)
        {
            _flagGun = false;
        }
    }

    public void Shooting()
    {
        if (SceneManager.GetActiveScene().buildIndex > 2 &&
            SceneManager.GetActiveScene().buildIndex != 6 &&
            SceneManager.GetActiveScene().buildIndex != 8)
        {
            if (_flagGun == true && _guns[_numGun] != null && _guns[_numGun].currentAmmos >= 1 && !_isReload && _timeBtwShot <= 0f)
            {
                _guns[_numGun].Shoot(_bullet, _parent, _pointStart, TypeBullet.Enemy);
                _timeBtwShot = _guns[_numGun].startTimeBtwShot;

                _anim.enabled = false;
                if (player._Object.isSit)
                {
                    transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z + _guns[_numGun].verticalRecoilSit);
                    hend.localEulerAngles = new Vector3(0, 0, hend.transform.localEulerAngles.z + _guns[_numGun].verticalRecoilSit);
                }
                else
                {
                    transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z + _guns[_numGun].verticalRecoil);
                    hend.localEulerAngles = new Vector3(0, 0, hend.transform.localEulerAngles.z + _guns[_numGun].verticalRecoil);
                }

                _audioSource.clip = _guns[_numGun].soundShoot;
                _audioSource.Play();

                if (StaticVal.vibroMode)
                    Handheld.Vibrate();
            }
        }
    }

    public void Reloading()
    {
        if (_inventory.FindAmmos(_guns[_numGun]) > 0 && _guns[_numGun].currentAmmos != _guns[_numGun].ammo &&
                _isReload == false && _flagGun)
        {
            _isReload = true;
            if (_guns[_numGun].typeAnim == TypeAnimationWeapon.Pistol)
                _anim.SetTrigger("ReloadPistol");
            else
                _anim.SetTrigger("ReloadGun");
        }
    }

    public void Reload()
    {
        int ammoReloading = _guns[_numGun].Reload(_inventory.FindAmmos(_guns[_numGun]));
        _inventory.SetAmmos(ammoReloading);
        _inventory.UpdateInventory();
        _isReload = false;
    }

    public Sprite GetSpriteStor()
    {
        return _guns[_numGun].imgStore;
    }

    public void Shoot(bool b)
    {
        _isShoot = b;

        if (!b)
        {
            transform.localEulerAngles = Vector3.zero;
            hend.localEulerAngles = new Vector3(0, 0, 90);
            _anim.enabled = true;
        }
    }

    public void SoundSpusk()
    {
        if ((SceneManager.GetActiveScene().buildIndex <= 2 ||
            SceneManager.GetActiveScene().buildIndex == 6 ||
            SceneManager.GetActiveScene().buildIndex == 8 ||
            _guns[_numGun].currentAmmos <= 0) && _flagGun)
        {
            _audioSource.clip = _guns[_numGun].soundSpusk;
            _audioSource.Play();
        }

        if ((SceneManager.GetActiveScene().buildIndex <= 2 ||
            SceneManager.GetActiveScene().buildIndex == 6 ||
            SceneManager.GetActiveScene().buildIndex == 8)
            && _flagGun)
        {
            player._playerInterface.StartPrintNoPve();
        }
    }

    public void SetArmor()
    {
        Debug.Log(player._Object.armor.id);

        if (player._Object.armor != null)
        {
            body.sprite = player._Object.armor.spriteBody;
            head.sprite = player._Object.armor.spriteHead;
            rig.sprite = player._Object.armor.spriteRig;
        }
        else
        {
            body.sprite = player.data.armors[0].spriteBody;
            head.sprite = player.data.armors[0].spriteHead;
            rig.sprite = player.data.armors[0].spriteRig;
        }
    }

    public void OnSetArmors()
    {
        player._Object.armor = _inventory.FindArmor();
        SetArmor();
    }
}
