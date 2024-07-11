using UnityEngine;

public enum TypeItem { Ammo, Weapon, Eat, Habar, Cothl, Pnv, Repair, Medical, PDA, Detector, NULL, DELL, SHOP, HABAR }
public enum TypeAmmo { Pistol_9x19, Shotgun_12, Automato_7x39, Automato_5x56, Automato_9x39, NULL }
public enum TypeMediac { Arma, Scientist, Standart, Bandage, NULL }
public enum TypeAnimationWeapon { Pistol, Gun, Shotgun };

public class DataBase : MonoBehaviour
{
    [SerializeField] public Item[] items;
    [SerializeField] public Gun[] guns;
    [SerializeField] public Armor[] armors;
    [SerializeField] public Quest[] quests;
}
