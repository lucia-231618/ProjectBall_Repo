using UnityEngine;

// Script de interacción por tecla para el jugador
public class PlayerInteractor : MonoBehaviour
{
    [Header("Sound References")]
    public PlayerController playerCont;   // Para reproducir sonidos del jugador

    [Header("Timer Reference")]
    public GameTimer gameTimer;           // Para actualizar polizones y tiempo

    [Header("Interaction Settings")]
    public KeyCode interactKey = KeyCode.E; // Tecla para interactuar
    public float interactionRange = 3f;     // Distancia máxima para interactuar

    private GameObject nearbyObject;      // Objeto con el que se puede interactuar

    void Update()
    {
        // Si hay un objeto cerca y el jugador presiona la tecla de interacción
        if (nearbyObject != null && Input.GetKeyDown(interactKey))
        {
            InteractWithObject();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Guardamos el objeto si es interactuable o polizón
        if (other.CompareTag("Polizon") || other.CompareTag("Interactuable"))
        {
            nearbyObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Limpiamos la referencia si el jugador se aleja
        if (other.gameObject == nearbyObject)
        {
            nearbyObject = null;
        }
    }

    void InteractWithObject()
    {
        if (nearbyObject == null) return;

        // Caso 1: Polizón
        if (nearbyObject.CompareTag("Polizon"))
        {
            nearbyObject.SetActive(false);                 // Desactiva el objeto
            if (gameTimer != null)
                gameTimer.AddPolizon();                    // Incrementa contador
            if (playerCont != null)
                playerCont.PlaySFX(1);                    // Reproduce sonido de recogida
        }
        // Caso 2: Objeto incorrecto
        else if (nearbyObject.CompareTag("Interactuable"))
        {
            if (gameTimer != null)
            {
                gameTimer.RemoveTime(gameTimer.penaltyTime); // Resta tiempo
                if (gameTimer.gameUI != null)
                    gameTimer.gameUI.PlayPenaltySFX();      // Sonido penalización
            }
            if (playerCont != null)
                playerCont.PlaySFX(2);                      // Sonido opcional
        }

        nearbyObject = null; // Limpiamos la referencia
    }
}
