using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public PlayerController playerCont;  // Referencia a PlayerController
    public GameUI gameUI;  // Referencia a GameUI
    private GameObject nearbyObject;

    void Update()
    {
        // Agrega aquí: if (Input.GetKeyDown(KeyCode.E)) InteractWithObject();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Polizon") || other.CompareTag("Interactuable"))
        {
            nearbyObject = other.gameObject;
        }
        else if (other.CompareTag("Pickup"))
        {
            CollectPickUp(other.gameObject);
        }
        else if (other.CompareTag("Powerup"))  // Tag genérico para power-ups
        {
            CollectPowerUp(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == nearbyObject)
        {
            nearbyObject = null;
        }
    }

    void InteractWithObject()
    {
        if (nearbyObject == null) return;

        if (nearbyObject.CompareTag("Polizon"))
        {
            nearbyObject.SetActive(false);
            if (gameUI != null)
                gameUI.AddPolizon();
            if (playerCont != null)
                playerCont.PlaySFX(1);
        }

        nearbyObject = null;
    }

    private void CollectPickUp(GameObject pickUp)
    {
        float tiempoSumado = 5f;
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
        {
            switch (powerUpScript.tipo)
            {
                case PowerUp.PowerUpType.JumpBoost:
                    if (playerCont != null)
                        playerCont.ActivateJumpBoost(powerUpScript.duracion);
                    break;
                case PowerUp.PowerUpType.SpeedBoost:
                    if (playerCont != null)
                        playerCont.ActivateSpeedBoost(powerUpScript.duracion);  // Usa duración como tiempo añadido
                    break;
            }
        }

        if (playerCont != null)
            playerCont.PlaySFX(3);

        Destroy(powerUp);  // Desaparece el power-up
    }
}