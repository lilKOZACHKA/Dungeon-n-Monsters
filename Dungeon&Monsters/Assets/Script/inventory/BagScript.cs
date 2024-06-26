using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagScript : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab;

    private List<Slot> slots = new List<Slot>();

    public List<Slot> MySlots { get { return slots;}}

    public void AddSlots(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
        {
          Slot slot = Instantiate(slotPrefab, transform).GetComponent<Slot>();
          slots.Add(slot);
        }
    }

    public bool AddItem(Item item)
    {
        foreach(Slot slot in slots)
        {
          if (slot.IsEmpty)
          {
            slot.AddItem(item);
            return true;

          }  
        }
        return false;
    }

}
