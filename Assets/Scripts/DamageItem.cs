using System.Collections;
using UnityEngine;

public class DamageItem : MonoBehaviour
{
    public string collectAnimation = "Collect"; // Nombre de la animaci�n de recolecci�n del objeto

    private Animator animator; // Animator del objeto

    void Start()
    {
        animator = GetComponent<Animator>(); // Obtener el Animator del objeto
    }

    // Detectar colisi�n con el jugador
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Obtener el componente de PlayerController del jugador
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player != null)
            {
                // Llamar a la funci�n de da�o del jugador (en este caso, simplemente activa el estado de da�o)
                player.TakeDamage(Vector2.zero); // Puedes ajustar el Vector2 seg�n la direcci�n del da�o

                // Iniciar la animaci�n de recolecci�n
                StartCoroutine(PlayCollectAnimationAndDestroy());
            }
        }
    }

    // Corrutina para reproducir la animaci�n de recolecci�n y luego destruir el objeto
    IEnumerator PlayCollectAnimationAndDestroy()
    {
        // Reproducir la animaci�n de recolecci�n
        animator.Play(collectAnimation);

        // Esperar a que la animaci�n de recolecci�n termine
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Destruir el objeto despu�s de la animaci�n
        Destroy(gameObject);
    }
}
