using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bag", menuName = "Items/Bag", order = 1)]

public class Bag : Item, IUseable
{
    private int slots;

    [SerializeField]
    protected GameObject bagPrefab;

    public BagScript MyBagScript { get; set;}
    public int Slots { get => slots;}

    public void Initialize(int slots){
        this.slots = slots;
    }

    public void Use()
    {
        if(Inventory.MyInstance.CanAddBag){
        Remove();
        MyBagScript = Instantiate(bagPrefab, Inventory.MyInstance.transform).GetComponent<BagScript>();
        MyBagScript.AddSlots(slots);
        Inventory.MyInstance.AddBag(this);
        }
    }

    
}

