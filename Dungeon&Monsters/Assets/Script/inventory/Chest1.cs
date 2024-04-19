using UnityEngine;
using UnityEngine.UI;

public class Chest1 : MonoBehaviour, IInteractable
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite openSprite, closedSprite;

    private bool isOpen;
[SerializeField]
    private CanvasGroup canvasGroup;

    private void Start()
    {
        // Изначально сундук закрыт
        isOpen = false;
        spriteRenderer.sprite = closedSprite;
    }

    public void Interact()
    {
        // Если сундук открыт, закрываем его
        if (isOpen)
        {
            StopInteract();
        }
        // Иначе открываем его
        else
        {
            isOpen = true;
            spriteRenderer.sprite = openSprite;
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void StopInteract()
    {
       isOpen = false;
       spriteRenderer.sprite = closedSprite;
       canvasGroup.alpha = 0;
       canvasGroup.blocksRaycasts = false;  
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, если объект, входящий в зону коллайдера сундука, является игроком
        if (other.CompareTag("Player"))
        {
            // При подходе игрока к сундуку, меняем его спрайт на открытый
            Interact();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Проверяем, если объект, покидающий зону коллайдера сундука, является игроком
        if (other.CompareTag("Player"))
        {
            // При уходе игрока от сундука, возвращаем его спрайт на закрытый
            StopInteract();
        }
    }

}
