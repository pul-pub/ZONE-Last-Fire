using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventory
{
    public Item item;

    public bool selected;
    public TypeItem type = TypeItem.NULL;
    public int cond = 100;
    public int count;

    public GameObject itemGameObj;
    public TextMeshProUGUI textObj;
    public Image imgObj;
    public Image imgBgObj;

    public int INDIFICATOR;
}
