using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static Inventory instance;

    public static Inventory MyInstance 
    {
        get 
        { if (instance == null)
            {
                instance = FindObjectOfType<Inventory>();
            }   
            return instance;
        }
    }   

    private Slot fromSlot;

[SerializeField]
    private Item[] items;

    private List<Bag> bags = new List<Bag>();


    public void AddBag(Bag bag)
    {
        bags.Add(bag);
    }
    
    
    public void AddItem(Item item)
{
    if (item.MyStackSize > 0)
    {
        if(PlaceInStack(item))
        {
            return;
        }
    }
    PlaceInEmpty(item);
}
    public bool CanAddBag
    {
        get { return bags.Count < 1;}
    }

    public Slot FromSlot { 
        get
    {
        return fromSlot;
    }
    set {
        fromSlot = value;
        if (value != null)
        {
            fromSlot.icon.color = Color.grey;
        }
    }
    }

    private void PlaceInEmpty(Item item)
    {
        foreach(Bag bag in bags)
        {
            if (bag.MyBagScript.AddItem(item))
            {
                return ;
            }
        }
    }
    private bool PlaceInStack(Item item)
    {
        foreach (Bag bag in bags)
        {
          foreach(Slot slots in bag.MyBagScript.MySlots) 
          {
            if(slots.StackItem(item))
            {
                return true;
            }
          }
        }
        return false;
    }
 



    private void Awake()
        { Bag bag = (Bag)Instantiate(items[0]);
        bag.Initialize(15);
        bag.Use();
        Debug.Log("Item added to inventory: " + bag.name);}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(15);
            AddItem(bag);

            Debug.Log("Item added to inventory: " + bag.name);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)){
            AddItem((Armor)Instantiate(items[1]));
            AddItem((Armor)Instantiate(items[2]));
            AddItem((Armor)Instantiate(items[3]));
            AddItem((Armor)Instantiate(items[4]));
            AddItem((Armor)Instantiate(items[5]));
            
        
            Debug.Log("Item added to inventory: ");
        }

        // if (Input.GetKeyDown(KeyCode.Alpha3)){
        //     Armor armor = (Armor)Instantiate(items[2]);
        //     AddItem(armor);
        
        //     Debug.Log("Item added to inventory: " + armor.name);
        // }
        // if (Input.GetKeyDown(KeyCode.Alpha4)){
        //     Armor armor = (Armor)Instantiate(items[3]);
        //     AddItem(armor);
        
        //     Debug.Log("Item added to inventory: " + armor.name);
        // }

        // if (Input.GetKeyDown(KeyCode.Alpha5)){
        //     Armor armor = (Armor)Instantiate(items[4]);
        //     AddItem(armor);
        
        //     Debug.Log("Item added to inventory: " + armor.name);
        // }

        // if (Input.GetKeyDown(KeyCode.Alpha6)){
        //     Armor armor = (Armor)Instantiate(items[5]);
        //     AddItem(armor);
        
        //     Debug.Log("Item added to inventory: " + armor.name);
        // }
    }

}

