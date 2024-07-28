using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quests", fileName = "None")]
public class Quest : ScriptableObject
{
    public int id;
    public string NameTo;
    public string NameFrom;
    public bool isActiv = true;
    [Header("Текст")]
    public string header;
    public string title;
    [Header("Место и начальный диалог")]
    public Vector2[] point = new Vector2[5];
    public Vector2[] pointReturn = new Vector2[5];
    public Dialog startDialog;
    public Dialog doingtDialog;
    [Header("Предмет")]
    public Item item;
    public int count;

    public int FindItem(Inventory inventory, bool del = true)
    {
        if (item != null)
            return inventory.FindItem(item, count, del);
        return -1;
    }
}
