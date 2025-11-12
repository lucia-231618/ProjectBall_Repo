using UnityEngine;
using UnityEngine.SceneManagement;

// Clase que controla la lógica central del juego:
// - Conteo de polizones
// - Tiempo del nivel
// - Determina victoria/derrota y carga la escena correspondiente
public class GameTimer : MonoBehaviour
{
    [Header("Time System")]
    public float timeLimit = 60f;          // Tiempo máximo para completar el nivel
    private float timeRemaining;           // Tiempo restante actual
    public float penaltyTime = 10f;        // Tiempo a restar al tocar un objeto incorrecto
    private bool gameEnded = false;        // Evita que siga actualizando tras terminar

    [Header("Polizones")]
    public int totalPolizones = 5;         // Número total de polizones en el nivel
    private int foundPolizones = 0;        // Contador de polizones recogidos

    [Header("Scene Management")]
    public int winSceneIndex = 2;          // Escena que se carga al ganar
    public int loseSceneIndex = 3;         // Escena que se carga al perder

    // Referencia opcional al script de UI
    public GameUI gameUI;

    // Start: se llama al iniciar el nivel
    void Start()
    {
        timeRemaining = timeLimit;  // Inicializa el temporizador
        if (gameUI != null)
            gameUI.UpdateAll(foundPolizones, totalPolizones, timeRemaining); // Actualiza la UI inicial
    }

    // Update: se llama cada frame
    void Update()
    {
        if (gameEnded) return;           // Evita seguir contando tras finalizar

        timeRemaining -= Time.deltaTime; // Resta el tiempo transcurrido en segundos

        if (timeRemaining <= 0f)         // Si se acaba el tiempo
        {
            timeRemaining = 0f;
            EndGame(false);              // Game Over
        }

        if (gameUI != null)              // Actualiza solo la UI del temporizador
            gameUI.UpdateTimer(timeRemaining);
    }

    // Llamado cuando el jugador recoge un polizón
    public void AddPolizon()
    {
        if (gameEnded) return;           // No hacer nada si el juego terminó

        foundPolizones++;                // Incrementa el contador
        if (gameUI != null)
            gameUI.UpdatePoints(foundPolizones, totalPolizones); // Actualiza UI de polizones

        if (foundPolizones >= totalPolizones) // Si se recogieron todos, gana
            EndGame(true);
    }

    // Llamado cuando el jugador toca un objeto incorrecto
    public void RemoveTime(float amount)
    {
        if (gameEnded) return;

        timeRemaining -= amount;
        if (timeRemaining < 0f) timeRemaining = 0f;

        if (gameUI != null)
            gameUI.UpdateTimer(timeRemaining); // Actualiza temporizador

        if (timeRemaining <= 0f)
            EndGame(false);                    // Game Over si se acaba el tiempo
    }

    // Termina el juego y carga la escena correspondiente
    private void EndGame(bool won)
    {
        gameEnded = true;                   // Bloquea actualizaciones
        int sceneToLoad = won ? winSceneIndex : loseSceneIndex; // Decide escena
        SceneManager.LoadScene(sceneToLoad); // Carga escena
    }

    // Métodos públicos para obtener info desde otros scripts si es necesario
    public float GetTimeRemaining() => timeRemaining;
    public int GetFoundPolizones() => foundPolizones;
}

