using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : BagScript
{   
    [SerializeField]
    private Item[] items;
    void Awake ()
    {
        AddSlots(20);

        AddItem((Armor)Instantiate(items[0]));
        AddItem((Armor)Instantiate(items[1]));
        AddItem((Armor)Instantiate(items[2]));
        AddItem((Armor)Instantiate(items[3]));
        AddItem((Armor)Instantiate(items[4]));
    }
}
