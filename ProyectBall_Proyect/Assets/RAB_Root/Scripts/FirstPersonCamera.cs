using UnityEngine;
using Unity.Mathematics;

public class CameraFollow : MonoBehaviour
{
    [Header("Ajustes")]
    [SerializeField] public float Sensibilidad = 100f;  // Sensibilidad del mouse
    [SerializeField] public Transform Player;          // Referencia al jugador

    [Header("Offset")]
    public Vector3 offset = new Vector3(0, 2, -5);     // Distancia de la cámara respecto al jugador

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

        // --- Movimiento: seguir al jugador ---
        transform.position = Player.position + offset;

        // --- Rotación con mouse ---
        float ValorX = Input.GetAxis("Mouse X") * Sensibilidad * Time.deltaTime;
        float ValorY = Input.GetAxis("Mouse Y") * Sensibilidad * Time.deltaTime;

        RotacionHorizontal += ValorX;
        RotacionVertical -= ValorY;

        // Limitar rotación vertical
        RotacionVertical = math.clamp(RotacionVertical, -80f, 80f);

        // Aplicar rotación vertical solo a la cámara
        transform.localRotation = Quaternion.Euler(RotacionVertical, 0, 0);

        // Aplicar rotación horizontal al jugador
        Player.Rotate(Vector3.up * ValorX);
    }
}


