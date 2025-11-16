using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { JumpBoost, SpeedBoost }  // Solo JumpBoost y SpeedBoost
    public PowerUpType tipo = PowerUpType.JumpBoost;  // Elige en el Inspector: qué tipo es este power-up
    public float duracion = 5f;  // Duración en segundos (para JumpBoost) o tiempo añadido (para SpeedBoost)
}
