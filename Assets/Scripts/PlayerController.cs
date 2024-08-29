using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento
    public float jumpForce = 10f; // Fuerza del salto
    public int maxJumps = 2; // Número de saltos permitidos (1 para salto normal, 2 para doble salto)
    public float damageForce = 5f; // Fuerza aplicada al recibir daño

    private int jumpsLeft; // Saltos restantes
    private bool isGrounded; // Si el jugador está tocando el suelo
    private bool isTakingDamage; // Si el jugador está recibiendo daño
    private Rigidbody2D rb;
    private Animator animator;

    // Nombres de las animaciones (puedes cambiarlos desde el inspector)
    public string idleAnimation = "Idle";
    public string runAnimation = "Run";
    public string jumpAnimation = "Jump";
    public string doubleJumpAnimation = "DoubleJump";
    public string fallAnimation = "Fall";
    public string damageAnimation = "Damage";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpsLeft = maxJumps;
    }

    void Update()
    {
        if (isTakingDamage) return; // Evitar el movimiento si está recibiendo daño

        float moveInput = Input.GetAxisRaw("Horizontal");

        // Aplicar movimiento horizontal tanto en el suelo como en el aire
        if (moveInput != 0)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1); // Flip para mirar a la dirección de movimiento
        }

        // Animaciones para el estado de movimiento
        if (rb.velocity.y < 0 && !isGrounded)
        {
            // Caída
            animator.Play(fallAnimation);
        }
        else if (rb.velocity.y > 0 && !isGrounded)
        {
            // Si es el primer salto, reproducir salto normal; si es el segundo, reproducir doble salto
            if (jumpsLeft == maxJumps - 1)
            {
                animator.Play(jumpAnimation); // Salto normal
            }
            else
            {
                animator.Play(doubleJumpAnimation); // Doble salto
            }
        }
        else if (isGrounded)
        {
            // Si está en el suelo
            if (moveInput != 0)
            {
                // Si se está moviendo horizontalmente
                animator.Play(runAnimation);
            }
            else
            {
                // Si está quieto en el suelo
                animator.Play(idleAnimation);
            }
        }

        // Salto y doble salto
        if (Input.GetButtonDown("Jump") && jumpsLeft > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Aplicar fuerza vertical

            if (jumpsLeft == maxJumps)
            {
                animator.Play(jumpAnimation); // Salto normal
            }
            else
            {
                animator.Play(doubleJumpAnimation); // Doble salto
            }

            jumpsLeft--;
            isGrounded = false; // Una vez que salta, ya no está en el suelo
        }
    }

    // Detección del suelo usando colisiones con el Tilemap o cualquier objeto etiquetado como "Ground"
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpsLeft = maxJumps; // Restablecer los saltos al tocar el suelo
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // No está tocando el suelo
        }
    }

    // Función para recibir daño
    public void TakeDamage(Vector2 damageDirection)
    {
        if (!isTakingDamage)
        {
            isTakingDamage = true;
            animator.Play(damageAnimation);
            rb.AddForce(damageDirection * damageForce, ForceMode2D.Impulse);
            StartCoroutine(DamageCooldown());
        }
    }

    // Cooldown para el daño, evitando recibir daño constantemente
    IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        isTakingDamage = false;
    }
}
