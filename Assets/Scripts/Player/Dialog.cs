using UnityEngine;

public enum TypeDescription { Dialog, Exit, Quest, Store, Repair, Habar, StoreOutfit, Lok }

[CreateAssetMenu(menuName = "Dialog", fileName = "None")]
public class Dialog : ScriptableObject
{
    public int id;
    public string name;
    [Header("������� �� �����")]
    public int money = -1;
    public Item item;
    public int countItem;
    [Header("�����")]
    public string title;
    [Header("������")]
    public string[] descriptions = new string[3];
    public Dialog[] descriptionsObject = new Dialog[3];
    public Quest[] quest = new Quest[3];
    public TypeDescription[] typeDescriptions = new TypeDescription[3];
}
