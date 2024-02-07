using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup : MonoBehaviour
{
    private inventory Inventory;
    public GameObject slotButton;
    private void Start()
    {
        Inventory = GameObject.FindGameObjectWithTag("UI").GetComponent<inventory>();
    }
}
