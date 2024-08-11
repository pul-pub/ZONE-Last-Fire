using UnityEngine;

[CreateAssetMenu(menuName = "Armor", fileName = "None")]
public class Armor : ScriptableObject
{
    public int id;
    [Header("Info")]
    public int bulletResistance;
    public int tearProtection;
    [Header("Grafics")]
    public Sprite spriteBody;
    public bool headIsMask = true;
    public Sprite spriteHead;
    public Sprite spriteRig;
    public Vector2 offset;
}
