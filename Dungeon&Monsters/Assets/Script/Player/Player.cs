using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private IInteractable interactable;

    public void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.tag == "Interactable") {
         {
             interactable = collision.GetComponent<IInteractable>();
                interactable.Interact();
                
                

            }
    }
}

public void OnTriggerExit2D(Collider2D collision)
{
    if (collision.tag == "Interactable" ){
    if(interactable != null)
    {
        interactable.StopInteract();
        interactable = null;
    }
    }
}
}
