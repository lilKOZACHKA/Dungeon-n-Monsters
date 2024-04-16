using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
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
}
