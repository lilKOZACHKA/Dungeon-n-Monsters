using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openclose : MonoBehaviour
{
      public GameObject inventory;
    private bool inventoryOn;

    private void Start() 
{
    inventoryOn = true;
}
public void Chest(){
    if(inventoryOn == false) {
        inventoryOn = true; 
        inventory.SetActive(true);}

    else if (inventoryOn == true) {
    inventoryOn = false;
    inventory.SetActive(false); }
}
}
