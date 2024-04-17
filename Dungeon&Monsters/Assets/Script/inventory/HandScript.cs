using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class HandScript : MonoBehaviour
{
    private static HandScript instance;
    public static HandScript MyInstance
    {
        get{
            if (instance == null)
            {
                instance = FindObjectOfType<HandScript>();
            }
            return instance;
        }
    }
    private Image icon;
    void Start()
    { icon = GetComponent<Image>();}

    void Update()
{
    if (icon != null)
    {
        icon.transform.position = Input.mousePosition + offset;

        if(Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && MyInstance.MyMoveable != null )
         {
            DeleteItem();
         }

        
    }
    else
    {
        Debug.LogWarning("Icon is not assigned.");
    }
}


    public IMoveable MyMoveable {get; set; }
    
[SerializeField]
    private Vector3 offset;

    public void TakeMoveable(IMoveable moveable)
    {
        this.MyMoveable = moveable;
        icon.sprite = moveable.MyIcon;
        icon.color = Color.white;   
    }

    public IMoveable Put()
    {
        IMoveable tmp = MyMoveable;
        MyMoveable = null;
        icon.color = new Color(0, 0, 0, 0);
        return tmp;
    }

    public void Drop()
    {
        MyMoveable = null;
        icon.color = new Color(0, 0, 0,0);   
    }


    public void DeleteItem()
    {
    
         if (MyMoveable is Item && Inventory.MyInstance.FromSlot != null)
         {
            (MyMoveable as Item).MySlot.Clear();
         }

         Drop();

         Inventory.MyInstance.FromSlot = null;
    }
}
