using UnityEngine;
using TMPro;

// Clase que controla solo la interfaz de usuario:
// - Textos de polizones y temporizador
// - Colores de advertencia y parpadeo
// - Sonidos relacionados con la UI
public class GameUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text timerText;   // Texto que muestra el tiempo
    public TMP_Text pointsText;  // Texto que muestra polizones

    [Header("Warnings")]
    public float lowTimeThreshold = 10f;  // Tiempo mínimo para activar advertencia visual
    public Color normalColor = Color.white; // Color normal del texto
    public Color warningColor = Color.red;  // Color cuando el tiempo es bajo
    public float blinkSpeed = 5f;          // Velocidad del parpadeo

    [Header("Sound Effects")]
    public AudioSource audioSource;        // Fuente de audio
    public AudioClip polizonClip;          // Sonido al recoger polizón
    public AudioClip penaltyClip;          // Sonido al perder tiempo

    // Actualiza todos los elementos de la UI al inicio
    public void UpdateAll(int foundPolizones, int totalPolizones, float timeRemaining)
    {
        UpdatePoints(foundPolizones, totalPolizones);
        UpdateTimer(timeRemaining);
    }

    // Actualiza contador de polizones en pantalla
    public void UpdatePoints(int foundPolizones, int totalPolizones)
    {
        if (pointsText != null)
            pointsText.text = $"Polizones: {foundPolizones} / {totalPolizones}";

        // Reproduce sonido al recoger polizón
        if (audioSource != null && polizonClip != null)
            audioSource.PlayOneShot(polizonClip);
    }

    // Actualiza el temporizador y controla el parpadeo si queda poco tiempo
    public void UpdateTimer(float timeRemaining)
    {
        if (timerText == null) return;

        timerText.text = $"Tiempo: {Mathf.CeilToInt(timeRemaining)}s"; // Muestra segundos enteros

        if (timeRemaining <= lowTimeThreshold) // Parpadeo de advertencia
        {
            float lerp = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            timerText.color = Color.Lerp(normalColor, warningColor, lerp);
        }
        else
        {
            timerText.color = normalColor;
        }
    }

    // Reproduce sonido de penalización
    public void PlayPenaltySFX()
    {
        if (audioSource != null && penaltyClip != null)
            audioSource.PlayOneShot(penaltyClip);
    }
}

