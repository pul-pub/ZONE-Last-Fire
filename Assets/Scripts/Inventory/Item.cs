using UnityEngine;

[CreateAssetMenu(menuName = "Items for inventory", fileName = "Item")]
public class Item : ScriptableObject
{
    [Header("����������� �������� - " +
        "\n1x - �������" +
        "\n2� - ����������" +
        "\n3� - ���" +
        "\n4� - �����" +
        "\n5� - �����������" +
        "\n6� - ���. �����" +
        "\n7�-8� - ������")]
    public int id;
    public string Name;
    public int money;
    public int maxCount;
    public string info;
    public TypeItem type;
    public TypeAmmo typeAmmo;
    public TypeMediac typeMediac;
    [Space]
    public Sprite imgBG;
    public Sprite imgSelected;
    public Sprite img;
    public Sprite img_50;
}
