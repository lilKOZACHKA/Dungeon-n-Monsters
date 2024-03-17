using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Скорость передвижения игрока
    public float jumpForce = 10f; // Сила прыжка
    public Transform groundCheck; // Точка для проверки нахождения на земле
    public LayerMask groundLayer; // Слой земли
    public float groundCheckRadius = 0.2f; // Радиус проверки нахождения на земле

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool isFacingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Получаем компонент Rigidbody2D
    }

    void Update()
    {
        // Проверяем, находится ли игрок на земле
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Получаем ввод от игрока
        float moveInput = Input.GetAxis("Horizontal");

        // Поворачиваем спрайт игрока
        if ((moveInput > 0 && !isFacingRight) || (moveInput < 0 && isFacingRight))
        {
            Flip();
        }

        // Применяем силу движения
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Прыжок
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    // Поворачиваем спрайт игрока
    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
