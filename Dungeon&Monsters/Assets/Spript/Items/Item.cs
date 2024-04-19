using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject, IMoveable
{
    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private int stackSize;

    private Slot slot;

    public Sprite MyIcon { get => icon; }
    public int MyStackSize { get => stackSize;}
    public Slot MySlot { get => slot; set => slot = value; }

    public void Remove()
    {
        if(MySlot != null)
        {
           MySlot.RemoveItem(this); 
        }
    }
}