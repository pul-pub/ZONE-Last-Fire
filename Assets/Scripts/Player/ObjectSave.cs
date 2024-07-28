using UnityEngine;

public class ObjectSave 
{
    public int[] idItem;
    public int[] coutItem;
    public int[] condItem;
    public int[] indificatorItem;
    public int[] currentAmmos = new int[2];
    public int _numGun;
    public bool falgGun;
    public int idActivQuest;
    public int[] idQuests;
    public int[] idEndingQuests;

    public bool[] peopls1 = new bool[8] { true, false, true, true, false, true, true, true };
    public bool[] peopls2 = new bool[8] { false, false, false, true, false, true, true, true };
    public bool[] peopls3 = new bool[8] { false, false, false, false, false, false, true, true };

    public float health;
    public float armor;
    public Vector2 pos;
    public Vector2 posSky;
    public float lights;
    public int[] time;
    public bool isRain;
    public int money;

    public int[] idsQuests;
    public bool isSave = false;
    public int indexScene;

    //---------------Õ¿—“–Œ… »----------------
    public float alfaUi;
    public float volSound;
    public bool vibroMode;
    public bool promptMode;
    public int FPSMode;
    //---------------Õ¿—“–Œ… »----------------
}
