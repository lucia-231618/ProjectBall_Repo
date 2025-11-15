using UnityEngine;
using Unity.Mathematics;  // Opcional, puedes usar Mathf.Clamp en su lugar

public class CameraFollow : MonoBehaviour
{
    [Header("Ajustes")]
    [SerializeField] public float Sensibilidad = 50f;  // Sensibilidad del mouse
    [SerializeField] public Transform Player;          // Referencia al jugador

    [Header("Offset")]
    public Vector3 offset = new Vector3(0, 4, -5);     // Distancia de la cámara respecto al jugador

    [Header("Estados")]
    [SerializeField] public float RotacionHorizontal = 0f;
    [SerializeField] public float RotacionVertical = 0f;

    void Start()
    {
        // Bloquear y ocultar cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (Player == null)
        {
            Debug.LogWarning("Camara: asigna (Player) en el Inspector.");
            return;
        }

        // --- Rotación con mouse ---
        float ValorX = Input.GetAxis("Mouse X") * Sensibilidad * Time.deltaTime;
        float ValorY = Input.GetAxis("Mouse Y") * Sensibilidad * Time.deltaTime;

        RotacionHorizontal += ValorX;
        RotacionVertical -= ValorY;

        // Limitar rotación vertical
        RotacionVertical = math.clamp(RotacionVertical, -80f, 80f);

        // --- Calcular offset rotado alrededor del player ---
        // Rotar el offset solo en el eje Y (horizontal) para que la cámara orbite
        Vector3 rotatedOffset = Quaternion.Euler(0, RotacionHorizontal, 0) * offset;

        // --- Movimiento: seguir al jugador con offset rotado ---
        transform.position = Player.position + rotatedOffset;

        // --- Aplicar rotación completa a la cámara (horizontal + vertical) ---
        // Esto hace que la cámara mire hacia el player y aplique la inclinación vertical
        transform.rotation = Quaternion.Euler(RotacionVertical, RotacionHorizontal, 0);

        // --- Opcional: Aplicar rotación horizontal al jugador (para que gire con la cámara) ---
        // Si no quieres que el player gire, comenta esta línea
        Player.Rotate(Vector3.up * ValorX);
    }
}
