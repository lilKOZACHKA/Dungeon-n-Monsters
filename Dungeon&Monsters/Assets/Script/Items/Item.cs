using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private int stackSize;

    private Slot slot;

    public Sprite MyIcon { get => icon; }
    public int StackSize { get => stackSize;}
    protected Slot Slot { get => slot; set => slot = value; }
}