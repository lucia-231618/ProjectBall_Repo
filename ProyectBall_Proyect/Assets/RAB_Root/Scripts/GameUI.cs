using UnityEngine;
using UnityEngine.UI;  // Cambia a using TMPro; si usas TextMeshPro
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [Header("UI Elements")]  // Ya no necesitas asignar estos manualmente
    private Text timerText;       // Ahora privado, se asigna automáticamente
    private Text polizonText;     // Ahora privado, se asigna automáticamente

    [Header("Audio")]
    public AudioSource audioSource;  // Esto sí puedes asignarlo manualmente si quieres, o buscarlo automáticamente también

    [Header("Game Logic")]
    public float timeLimit = 60f;          // Tiempo máximo para completar el nivel
    public float penaltyTime = 10f;        // Tiempo a restar al tocar un objeto incorrecto
    public int totalPolizones = 5;         // Número total de polizones en el nivel
    public string winSceneName = "WinScene";  // Nombre de la escena de victoria
    public string loseSceneName = "LoseScene"; // Nombre de la escena de derrota

    private float timeRemaining;           // Tiempo restante actual
    private int foundPolizones = 0;        // Contador de polizones recogidos
    private bool gameEnded = false;        // Evita que siga actualizando tras terminar

    void Awake()
    {
        DontDestroyOnLoad(gameObject);  // El objeto GameUI no se destruye al cambiar escenas
    }

    void Start()
    {
        // Asignar automáticamente los textos buscando por nombre
        timerText = GameObject.Find("TimerText")?.GetComponent<Text>();
        polizonText = GameObject.Find("PolizonText")?.GetComponent<Text>();

        // Si no encuentra alguno, muestra un warning (opcional, para depuración)
        if (timerText == null) Debug.LogWarning("No se encontró TimerText en la escena.");
        if (polizonText == null) Debug.LogWarning("No se encontró PolizonText en la escena.");

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
        timeRemaining = timeLimit;
        foundPolizones = 0;
        gameEnded = false;
        UpdateTimerUI(timeRemaining);
        UpdatePolizonUI();
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
}
