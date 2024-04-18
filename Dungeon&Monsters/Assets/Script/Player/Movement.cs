using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Скорость передвижения игрока

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Получаем компонент Rigidbody2D
        spriteRenderer = GetComponent<SpriteRenderer>(); // Получаем компонент SpriteRenderer
    }

    void FixedUpdate()
    {
        // Получаем ввод от игрока
        float moveInput = Input.GetAxis("Horizontal");

        // Применяем силу движения
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Поворачиваем спрайт игрока
        if (moveInput > 0)
        {
            spriteRenderer.flipX = false; // Отражаем спрайт по горизонтали, если движение вправо
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = true; // Отражаем спрайт по горизонтали, если движение влево
        }
    }
}
