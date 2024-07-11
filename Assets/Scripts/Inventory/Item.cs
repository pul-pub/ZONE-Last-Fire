using UnityEngine;

[CreateAssetMenu(menuName = "Items for inventory", fileName = "Item")]
public class Item : ScriptableObject
{
    [Header("Индификатор предмета - " +
        "\n1x - потроны" +
        "\n2х - устройства" +
        "\n3х - еда" +
        "\n4х - хабар" +
        "\n5х - медикаменты" +
        "\n6х - рем. части" +
        "\n7х-8х - оружие")]
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
