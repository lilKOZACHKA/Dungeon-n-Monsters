using System.Collections;
using System.Collections.Generic;
using Cainos.PixelArtPlatformer_VillageProps;
using UnityEngine;

public class openclose : MonoBehaviour
{
      public GameObject inventory;
    private bool inventoryOn;

    public Chest1 chest;


    private IInteractable interactable;

    private void Start() 
{
    inventoryOn = false;
    inventory.SetActive(false);
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
