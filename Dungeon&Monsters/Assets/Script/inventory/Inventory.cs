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

[SerializeField]
    private Item[] items;

    private List<Bag> bags = new List<Bag>();

    public void AddBag(Bag bag)
    {
        bags.Add(bag);
    }
    
    
    public void AddItem(Item item)
{
    foreach (Bag bag in bags) // Добавляем bags в ваш цикл foreach
    {
        if (bag.MyBagScript.AddItem(item))
        { 
            Debug.Log("Item added to bag: " + item.name + " in " + bag.name);
            return; 
        }
        Debug.Log("1 " + bag.name);
    }
}



    private void Awake()
        { Bag bag = (Bag)Instantiate(items[0]);
        bag.Initialize(10);
        bag.Use();
        Debug.Log("Item added to inventory: " + bag.name);}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)){
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(10);
            AddItem(bag);

            Debug.Log("Item added to inventory: " + bag.name);
        }
    }

}

