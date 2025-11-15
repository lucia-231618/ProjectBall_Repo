using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public void Activar(GameObject player)
    {
        Debug.Log("PowerUp activado!");
    }
}


public class PickUp : MonoBehaviour
{
    public float tiempoQueDa = 5f;
}


// Script de interacción por tecla para el jugador
public class PlayerInteractor : MonoBehaviour
{
    [Header("Sound References")]
    public PlayerController playerCont;   // Para reproducir sonidos del jugador

    [Header("Timer Reference")]
    public GameUI gameUI;           // Referencia al script GameUI (maneja polizones, tiempo, etc.)

    private GameObject nearbyObject;      // Objeto con el que se puede interactuar

    void Update()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detección existente para Polizon e Interactuable
        if (other.CompareTag("Polizon") || other.CompareTag("Interactuable"))
        {
            nearbyObject = other.gameObject;
        }
        // NUEVO: Detección automática para Pickups y PowerUps
        else if (other.CompareTag("Pickup"))  // Asume tag "Pickup" para objetos recolectables
        {
            CollectPickUp(other.gameObject);
        }
        else if (other.CompareTag("Powerup"))  // Asume tag "PowerUp"
        {
            CollectPowerUp(other.gameObject);
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
            if (gameUI != null)
                gameUI.AddPolizon();                    // Incrementa contador
            if (playerCont != null)
                playerCont.PlaySFX(1);                    // Reproduce sonido de recogida
        }

        nearbyObject = null; // Limpiamos la referencia
    }
    private void CollectPickUp(GameObject pickUp)
    {
        float tiempoSumado = 5f; // tiempo por defecto

        PickUp pickUpScript = pickUp.GetComponent<PickUp>();
        if (pickUpScript != null)
            tiempoSumado = pickUpScript.tiempoQueDa;

        if (gameUI != null)
            gameUI.RemoveTime(-tiempoSumado);


        if (playerCont != null)
            playerCont.PlaySFX(1);

        Destroy(pickUp);
    }

    private void CollectPowerUp(GameObject powerUp)
    {
        PowerUp powerUpScript = powerUp.GetComponent<PowerUp>();
        if (powerUpScript != null)
            powerUpScript.Activar(gameObject);

        if (playerCont != null)
            playerCont.PlaySFX(3);

        Destroy(powerUp);
    }

}
