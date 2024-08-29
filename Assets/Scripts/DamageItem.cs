using System.Collections;
using UnityEngine;

public class DamageItem : MonoBehaviour
{
    public string collectAnimation = "Collect"; // Nombre de la animación de recolección del objeto

    private Animator animator; // Animator del objeto

    void Start()
    {
        animator = GetComponent<Animator>(); // Obtener el Animator del objeto
    }

    // Detectar colisión con el jugador
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Obtener el componente de PlayerController del jugador
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player != null)
            {
                // Llamar a la función de daño del jugador (en este caso, simplemente activa el estado de daño)
                player.TakeDamage(Vector2.zero); // Puedes ajustar el Vector2 según la dirección del daño

                // Iniciar la animación de recolección
                StartCoroutine(PlayCollectAnimationAndDestroy());
            }
        }
    }

    // Corrutina para reproducir la animación de recolección y luego destruir el objeto
    IEnumerator PlayCollectAnimationAndDestroy()
    {
        // Reproducir la animación de recolección
        animator.Play(collectAnimation);

        // Esperar a que la animación de recolección termine
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Destruir el objeto después de la animación
        Destroy(gameObject);
    }
}
