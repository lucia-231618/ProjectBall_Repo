using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [Header("Time System")]
    public float timeLimit = 60f; // Tiempo máximo en segundos
    private float timeRemaining;  // Tiempo que queda
    public float penaltyTime = 10f; // Tiempo que resta al tocar un objeto no polizón
    private int totalPolizones;   // Total de polizones en la escena
    private int foundPolizones = 0; // Cuántos has encontrado
    private bool gameEnded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
