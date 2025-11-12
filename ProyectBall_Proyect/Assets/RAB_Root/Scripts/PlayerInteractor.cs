using UnityEngine;

// Clase que detecta la interacción del jugador con objetos del nivel:
// - Polizones (objetos a recoger)
// - Objetos incorrectos (penalización de tiempo)
public class PlayerInteractor : MonoBehaviour
{
    [Header("Sound References")]
    public PlayerController playerCont; // Reproduce SFX del jugador al interactuar

    [Header("Timer Reference")]
    public GameTimer gameTimer;         // Referencia al temporizador central

    private void OnTriggerEnter(Collider other)
    {
        // Caso 1: Polizón
        if (other.gameObject.CompareTag("Polizon"))
        {
            other.gameObject.SetActive(false);       // Desactiva el objeto
            if (gameTimer != null)
                gameTimer.AddPolizon();              // Incrementa contador central de polizones
            if (playerCont != null)
                playerCont.PlaySFX(1);              // Sonido opcional del jugador
        }

        // Caso 2: Objeto incorrecto
        else if (other.gameObject.CompareTag("Interactuable"))
        {
            if (gameTimer != null)
            {
                gameTimer.RemoveTime(gameTimer.penaltyTime); // Resta tiempo
                if (gameTimer.gameUI != null)
                    gameTimer.gameUI.PlayPenaltySFX();      // Reproduce sonido de penalización
            }

            if (playerCont != null)
                playerCont.PlaySFX(2);                      // Sonido opcional del jugador
        }
    }
}
