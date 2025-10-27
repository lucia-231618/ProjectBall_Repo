using UnityEngine;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("Configuración del jugador")]
    public float speed = 10f; // Velocidad de movimiento

    [Header("Configuración del juego")]
    public float timeLimit = 60f; // Tiempo máximo en segundos
    private float timeRemaining;  // Tiempo que queda
    private int totalPolizones;   // Total de polizones en la escena
    private int foundPolizones = 0; // Cuántos has encontrado
    private bool gameEnded = false;

    [Header("Interfaz UI")]
    public Text timerText;
    public Text polizonesText;
    public Text infoText;

    private Rigidbody rb;

    void Start()
    {
 
    }

    
        // Update is called once per frame
        void Update()
    {
        
    }
}
