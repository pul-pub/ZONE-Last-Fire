using UnityEngine;

public enum TypeBullet { Player, Enemy };

[CreateAssetMenu(menuName = "Gun for inventory", fileName = "Gun")]
public class Gun : ScriptableObject
{
    [Header("For inventory")]
    public string name;
    public int id;
    [Header("For grafics")]
    public bool isGraficStore;
    public Sprite img;
    public Sprite imgStore;
    public Vector2 pointOfGraficStore;
    public TypeAnimationWeapon typeAnim;
    [Space]
    public Sprite imgReload;
    public AudioClip soundShoot;
    public AudioClip soundSpusk;
    [Header("For reload weapon")]
    public TypeAmmo typeAmmo;
    public string animationReload;
    [Header("For shoot")]
    public float dm;
    public int ammo;
    public float verticalRecoil;
    public float verticalRecoilSit;
    public float startTimeBtwShot;

    public int currentAmmos = 0;

    public void Shoot(Object bullet, Transform parent, GameObject _pointStart, TypeBullet _type)
    {
        currentAmmos--;

        if (typeAmmo == TypeAmmo.Shotgun_12)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject obj = Object.Instantiate(bullet, _pointStart.transform.position,
                    _pointStart.transform.rotation, parent) as GameObject;
                Bullet bulletScript = obj.GetComponent<Bullet>();

                obj.transform.eulerAngles = new Vector3(0, 0, obj.transform.eulerAngles.z + Random.Range(-8f, 8f));
                bulletScript.typeBullet = _type;
            }
        }
        else
        { 
            GameObject obj = Object.Instantiate(bullet, _pointStart.transform.position, _pointStart.transform.rotation, parent) 
                as GameObject;
            Bullet bulletScript = obj.GetComponent<Bullet>();

            bulletScript.typeBullet = _type;
            bulletScript.damage = dm;
        }
    }

    public int Reload(int _ammos)
    {
        int reason = ammo - currentAmmos;
        int returnAmmos = 0;

        if (_ammos >= reason)
        {
            returnAmmos += reason;
            currentAmmos += reason;
        }
        else
        {
            currentAmmos += _ammos;
            returnAmmos = _ammos;
        }

        return returnAmmos;
    }

    public Gun Clone()
    {
        Gun newGun = new Gun();

        newGun.name = name;
        newGun.id = id;
        newGun.isGraficStore = isGraficStore;
        newGun.img = img;
        newGun.imgStore = imgStore;
        newGun.pointOfGraficStore = pointOfGraficStore;
        newGun.typeAnim = typeAnim;
        newGun.imgReload = imgReload;
        newGun.soundShoot = soundShoot;
        newGun.soundSpusk = soundSpusk;
        newGun.typeAmmo = typeAmmo;
        newGun.animationReload = animationReload;
        newGun.dm = dm;
        newGun.ammo = ammo;
        newGun.verticalRecoil = verticalRecoil;
        newGun.verticalRecoilSit = verticalRecoilSit;
        newGun.startTimeBtwShot = startTimeBtwShot;
        newGun.currentAmmos = currentAmmos;

        return newGun;
    }
}
