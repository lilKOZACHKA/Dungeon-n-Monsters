using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;
public class Slot : MonoBehaviour, IPointerClickHandler, IClickable
{
    private ObservableStack<Item> items = new ObservableStack<Item>();
    
    [SerializeField]
    public Image icon;
[SerializeField]
    private Text stackSize;

    public bool IsEmpty
    {
        get { return items.Count == 0; }
    }

    public bool IsFull
    {
        get { if (IsEmpty)
            {
            return false;
            }
        return true;
        }
    }

    public Item MyItem { 
        get
        { if(!IsEmpty)
        { return items.Peek();}
        return null; 

        }
        
     }

    public Image MyIcon { get; set;}

    public int MyCount {get {return items.Count;}}

    public void RemoveItem(Item item)
    {
        if(!IsEmpty){ items.Pop(); }

    }

    public void Clear()
    {
        if(items.Count > 0)
        {
            items.Clear();
        }
    }

    public bool AddItem(Item item)
    {
        items.Push(item);
        icon.sprite = item.MyIcon;
        icon.color = Color.white;
        item.MySlot = this;
        
        return true;

    }


    public void OnPointerClick(PointerEventData eventData)
    {
    if (eventData.button == PointerEventData.InputButton.Left)
    {   
        if (Inventory.MyInstance.FromSlot == null && !IsEmpty)
        {
            HandScript.MyInstance.TakeMoveable(MyItem as IMoveable);
            Inventory.MyInstance.FromSlot = this;
            Inventory.MyInstance.FromSlot.icon.color = new Color(0,0,0,0);
        }
        else if (Inventory.MyInstance.FromSlot == null && IsEmpty)
        {
            if (HandScript.MyInstance.MyMoveable is Armor)
            {
                Armor armor = (Armor)HandScript.MyInstance.MyMoveable;
                    HandScript.MyInstance.Drop();
                    Debug.Log("item added to inventory");
            
            }
        }
        else if (Inventory.MyInstance.FromSlot != null)
        {
            if (PutItemBack() || SwapItems(Inventory.MyInstance.FromSlot) || AddItems(Inventory.MyInstance.FromSlot.items))
            {
                HandScript.MyInstance.Drop();
                Inventory.MyInstance.FromSlot = null;
            }
        }
    }


        if(eventData.button == PointerEventData.InputButton.Right)
        {
            UseItem();
        }

           }

    public bool AddItems(ObservableStack<Item> newItems)
    {
        if(IsEmpty || newItems.Peek().GetType() == MyItem.GetType())
        {
            int count = newItems.Count;

            for(int i = 0; i < count; i++)
            {
                if (IsFull)
                {
                    return false;
                }
                AddItem(newItems.Pop());
            }
            return true;
        }
        return false;
    }

    public void UseItem()
    {   
        if ( MyItem is IUseable)
        {
            (MyItem as IUseable).Use();
        }
        
    }

    public bool StackItem(Item item)
    {
        if (!IsEmpty && item.name == MyItem.name && items.Count < MyItem.MyStackSize)
        {
            items.Push(item);
            item.MySlot = this;
            return true;
        }
        return false;
    }

    private bool PutItemBack()
    {
        if(Inventory.MyInstance.FromSlot == this)
        { 
        Inventory.MyInstance.FromSlot.icon.color = Color.white;
          
          return true;
        }

        return false;
    }

    private bool SwapItems( Slot from)
    {
        if (IsEmpty)
        {
            return false;
        }
        if (from.MyCount.GetType() != MyItem.GetType() || from.MyCount+MyCount > MyItem.MyStackSize)
        {
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.items);
            from.items.Clear();
            from.AddItems(items);
            items.Clear();
            AddItems(tmpFrom);
            return true;
        }
        return false;
    }

}