using UnityEngine;

public class ObjectSave
{
    public Vector2 pos;
    public bool isSave = false;
    public int indexScene;

    #region INVENTORY
    public int money;
    public int[] idItem;
    public int[] coutItem;
    public int[] condItem;
    public int[] indificatorItem;
    #endregion

    #region WEAPON_ON_PLAYER
    public int[] currentAmmos = new int[2];
    public int _numGun;
    public bool falgGun;
    #endregion

    #region QUESTS
    public int idActivQuest;
    public int[] idQuests;
    public int[] idEndingQuests;
    public int[] idsQuests;
    #endregion

    #region STATICVAL.PEOPLS
    public bool[] peopls1 = new bool[8] { true, false, true, true, false, true, true, true };
    public bool[] peopls2 = new bool[8] { false, false, false, true, false, true, true, true };
    public bool[] peopls3 = new bool[8] { false, false, false, false, false, false, true, true };
    public bool[] peopls4 = new bool[8] { false, false, false, false, false, false, true, true };
    public bool[] peopls10 = new bool[8] { false, false, false, false, false, false, true, true };
    public bool[] peopls11 = new bool[8] { false, false, false, false, false, false, true, true };
    #endregion

    #region TIMES
    public Vector2 posSky;
    public float lights;
    public int[] time;
    public bool isRain;
    #endregion  

    #region SETTINGS
    public float alfaUi;
    public float volSound;
    public bool vibroMode;
    public bool promptMode;
    public int FPSMode;
    #endregion

    #region SHELTER
    public Vector2[] posFurniture;
    public int[] idsFurniture;
    #endregion

    #region CHARECTER
    public float health;
    public float armor;
    public string name;

    public int notSelectedXP;

    public int idFace;
    public int[] characteristics;
    public int[] idSkills;
    #endregion

    public string timeCreated;
}
