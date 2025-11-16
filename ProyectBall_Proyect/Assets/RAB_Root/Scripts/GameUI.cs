using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Text pointsText;      // Texto para mostrar los puntos
    public Text timerText;       // Texto para mostrar el tiempo restante
    public Text polizonText;     // Texto para mostrar el contador de polizones

    [Header("Audio")]
    public AudioSource audioSource;  // Para reproducir sonidos de penalización

    [Header("Game Logic")]
    public float timeLimit = 60f;          // Tiempo máximo para completar el nivel
    public float penaltyTime = 10f;        // Tiempo a restar al tocar un objeto incorrecto
    public int totalPolizones = 5;         // Número total de polizones en el nivel
    public string winSceneName = "WinScene";  // Nombre de la escena de victoria
    public string loseSceneName = "LoseScene"; // Nombre de la escena de derrota

    private int points = 0;                // Contador de puntos
    private float timeRemaining;           // Tiempo restante actual
    private int foundPolizones = 0;        // Contador de polizones recogidos
    private bool gameEnded = false;        // Evita que siga actualizando tras terminar

    void Awake()
    {
        DontDestroyOnLoad(gameObject);  // El objeto GameUI no se destruye al cambiar escenas
    }

    void Start()
    {
        ResetGame();  // Reinicia valores al iniciar
    }

    void Update()
    {
        if (gameEnded) return;           // Evita seguir contando tras finalizar

        timeRemaining -= Time.deltaTime; // Resta el tiempo transcurrido en segundos

        if (timeRemaining <= 0f)         // Si se acaba el tiempo
        {
            timeRemaining = 0f;
            EndGame(false);              // Game Over
        }

        UpdateTimerUI(timeRemaining);    // Actualiza la UI del temporizador
    }

    // Método para sumar puntos
    public void AddPoints(int amount)
    {
        points += amount;
        UpdatePointsUI();
    }

    // Llamado cuando el jugador recoge un polizón
    public void AddPolizon()
    {
        if (gameEnded) return;           // No hacer nada si el juego terminó

        foundPolizones++;                // Incrementa el contador
        UpdatePolizonUI();

        if (foundPolizones >= totalPolizones) // Si se recogieron todos, gana
            EndGame(true);
    }

    // Llamado cuando el jugador toca un objeto incorrecto
    public void RemoveTime(float amount)
    {
        if (gameEnded) return;

        timeRemaining -= amount;
        if (timeRemaining < 0f) timeRemaining = 0f;

        UpdateTimerUI(timeRemaining); // Actualiza temporizador

        if (timeRemaining <= 0f)
            EndGame(false);                    // Game Over si se acaba el tiempo
    }

    // Método para reproducir sonido de penalización
    public void PlayPenaltySFX()
    {
        if (audioSource != null)
            audioSource.Play();
    }

    // Termina el juego y carga la escena correspondiente
    private void EndGame(bool won)
    {
        gameEnded = true;                   // Bloquea actualizaciones
        string sceneToLoad = won ? winSceneName : loseSceneName; // Decide escena
        SceneManager.LoadScene(sceneToLoad); // Carga escena
        Destroy(gameObject, 0.1f);  // Destruye este objeto en escenas finales
    }

    // Reinicia los valores del juego (llámalo si pierdes o reinicias manualmente)
    public void ResetGame()
    {
        points = 0;
        timeRemaining = timeLimit;
        foundPolizones = 0;
        gameEnded = false;
        UpdatePointsUI();
        UpdateTimerUI(timeRemaining);
        UpdatePolizonUI();
    }

    // Actualiza el texto de puntos
    private void UpdatePointsUI()
    {
        if (pointsText != null)
            pointsText.text = "Puntos: " + points.ToString();
    }

    // Actualiza el texto del timer
    private void UpdateTimerUI(float time)
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    // Actualiza el texto de polizones
    private void UpdatePolizonUI()
    {
        if (polizonText != null)
            polizonText.text = "Polizones: " + foundPolizones.ToString() + "/" + totalPolizones.ToString();
    }

    // Métodos públicos para obtener info desde otros scripts si es necesario
    public float GetTimeRemaining() => timeRemaining;
    public int GetFoundPolizones() => foundPolizones;
    public int GetPoints() => points;
}
