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
        //Obtiene el Rigidbody de la bola para poder moverla con física.
        rb = GetComponent<Rigidbody>();
        timeRemaining = timeLimit;

        // Contamos cuántos polizones hay en la escena
        totalPolizones = GameObject.FindGameObjectsWithTag("Polizon").Length;

       
        infoText.text = "¡Encuentra las anomalías!"; 

    }

    void FixedUpdate()
    {
        if (gameEnded) return;

        // Movimiento de la bola
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameEnded) return;

        // Restar tiempo cada frame
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            EndGame(false); // Se acabó el tiempo → pierdes
        }
    }
    void OnTriggerEnter(Collider other)
    {
        // Si el jugador toca un Polizon
        if (other.CompareTag("Polizon"))
        {
            other.gameObject.SetActive(false); // lo “recogemos”
            foundPolizones++;

            // Si ya encontró todos → gana
            if (foundPolizones >= totalPolizones)
                EndGame(true);
        }
    }
}

