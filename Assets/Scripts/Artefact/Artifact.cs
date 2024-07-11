using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : MonoBehaviour
{
    public string name;
    public Item item;
    public ItemInventory itemI;

    private void Awake()
    {
        itemI = AddItem(item, 1, 90);
    }

    public ItemInventory AddItem(Item item, int count, int cond)
    {
        ItemInventory ii = new ItemInventory();

        ii.item = item; //даём ади
        ii.cond = cond;
        ii.count = count;    //даём количество
        if (ii.count > 1 && (item.type == TypeItem.Weapon || item.type == TypeItem.Cothl))
        {
            ii.count = 1;
        }
        if (ii.count > 128 && ii.item.type == TypeItem.Ammo)
        {
            ii.count = 128;
        }
        else if (ii.count > 4 && (ii.item.type == TypeItem.Eat || ii.item.type == TypeItem.Medical))
        {
            ii.count = 4;
        }
        else if (ii.count > 8 && (ii.item.type == TypeItem.PDA || ii.item.type == TypeItem.Pnv ||
            ii.item.type == TypeItem.Detector || ii.item.type == TypeItem.Habar ||
            ii.item.type == TypeItem.Repair))
        {
            ii.count = 8;
        }

        return ii;
    }
}
