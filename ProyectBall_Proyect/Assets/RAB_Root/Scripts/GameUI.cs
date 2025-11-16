using UnityEngine;
using TMPro;  // Para TextMeshPro
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI combinedText;    // Texto único para puntos y polizones
    public TextMeshProUGUI timerText;       // Texto para mostrar el tiempo restante

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

        // Buscar automáticamente el texto combinado por tag
        if (combinedText == null)
        {
            GameObject combinedObj = GameObject.FindWithTag("CombinedText");
            if (combinedObj != null)
            {
                combinedText = combinedObj.GetComponent<TextMeshProUGUI>();
                Debug.Log("Texto combinado (TMP) encontrado y asignado.");
            }
            else
            {
                Debug.LogError("No se encontró un GameObject con tag 'CombinedText'.");
            }
        }

        // Buscar el texto del timer por tag
        if (timerText == null)
        {
            GameObject timerObj = GameObject.FindWithTag("TimerText");
            if (timerObj != null)
            {
                timerText = timerObj.GetComponent<TextMeshProUGUI>();
                Debug.Log("Texto del timer (TMP) encontrado y asignado.");
            }
            else
            {
                Debug.LogError("No se encontró un GameObject con tag 'TimerText'.");
            }
        }
    }

    void Start()
    {
        ResetGame();  // Reinicia valores al iniciar
        Debug.Log("GameUI iniciado. Valores iniciales: Puntos=" + points + ", Polizones=" + foundPolizones + "/" + totalPolizones + ", Tiempo=" + timeRemaining);
    }

    void Update()
    {
        if (gameEnded) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            Debug.Log("Tiempo agotado. Fin del juego.");
            EndGame(false);
        }

        UpdateTimerUI(timeRemaining);
    }

    // Método para sumar puntos
    public void AddPoints(int amount)
    {
        if (gameEnded) return;
        points += amount;
        Debug.Log("Puntos sumados: " + amount + ". Total puntos: " + points);
        UpdateCombinedUI();
    }

    // Llamado cuando el jugador recoge un polizón
    public void AddPolizon()
    {
        if (gameEnded) return;
        foundPolizones++;
        Debug.Log("Polizón recogido. Total polizones: " + foundPolizones + "/" + totalPolizones);
        UpdateCombinedUI();

        if (foundPolizones >= totalPolizones)
        {
            Debug.Log("Todos los polizones recogidos. Victoria!");
            EndGame(true);
        }
    }

    // Llamado cuando el jugador toca un objeto incorrecto
    public void RemoveTime(float amount)
    {
        if (gameEnded) return;
        timeRemaining -= amount;
        if (timeRemaining < 0f) timeRemaining = 0f;
        Debug.Log("Tiempo restado: " + amount + ". Tiempo restante: " + timeRemaining);
        UpdateTimerUI(timeRemaining);

        if (timeRemaining <= 0f)
        {
            Debug.Log("Tiempo agotado por penalización. Fin del juego.");
            EndGame(false);
        }
    }

    // Método para reproducir sonido de penalización
    public void PlayPenaltySFX()
    {
        if (audioSource != null)
        {
            audioSource.Play();
            Debug.Log("Sonido de penalización reproducido.");
        }
    }

    // Termina el juego y carga la escena correspondiente
    private void EndGame(bool won)
    {
        gameEnded = true;
        string sceneToLoad = won ? winSceneName : loseSceneName;
        Debug.Log("Fin del juego. Cargando escena: " + sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
        Destroy(gameObject, 0.1f);
    }

    // Reinicia los valores del juego
    public void ResetGame()
    {
        points = 0;
        timeRemaining = timeLimit;
        foundPolizones = 0;
        gameEnded = false;
        Debug.Log("Juego reiniciado.");
        UpdateCombinedUI();
        UpdateTimerUI(timeRemaining);
    }

    // Actualiza el texto combinado
    private void UpdateCombinedUI()
    {
        if (combinedText != null)
        {
            combinedText.text = "Fixed: " + points.ToString() + " | Anomalies: " + foundPolizones.ToString() + "/" + totalPolizones.ToString();
            Debug.Log("Texto combinado actualizado: " + combinedText.text);
        }
        else
        {
            Debug.LogError("combinedText es null. No se puede actualizar.");
        }
    }

    // Actualiza el texto del timer
    private void UpdateTimerUI(float time)
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            Debug.Log("Texto del timer actualizado: " + timerText.text);
        }
        else
        {
            Debug.LogError("timerText es null. No se puede actualizar.");
        }
    }

    // Métodos públicos para obtener info
    public float GetTimeRemaining() => timeRemaining;
    public int GetFoundPolizones() => foundPolizones;
    public int GetPoints() => points;
}

