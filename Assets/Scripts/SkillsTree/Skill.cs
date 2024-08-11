using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeModifier { Speed, Stelth, Lerning };

[CreateAssetMenu(menuName = "Skill", fileName = "Null")]
public class Skill : ScriptableObject
{
    public int Id;
    public string Name;
    public TypeModifier Modifier;
    public float LevelModifier;
    public int CountXP;
    public Sprite img;

    public Skill afterSkill;
    public Skill postSkill_1;
    public Skill postSkill_2;
}
