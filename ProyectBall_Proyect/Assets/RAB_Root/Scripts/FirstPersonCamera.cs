using UnityEngine;
using UnityEngine.InputSystem; // Necesario para el nuevo Input System

public class CameraOrbitBall_InputSystem : MonoBehaviour
{
    [Header("Referencias")]
    public Transform ball; // La pelota o el objeto a seguir

    [Header("Ajustes de cámara")]
    public float distance = 5f;
    public float height = 2f;
    public float mouseSensitivity = 1f;
    public float smoothSpeed = 10f;

    private float yaw = 0f;
    private float pitch = 20f;
    private Vector3 currentVelocity;

    // Referencia a los Input Actions generados
    private PlayerControls controls;
    private Vector2 lookInput;

    void Awake()
    {
        controls = new PlayerControls(); // Creamos la instancia del input
    }

    void OnEnable()
    {
        controls.Camera.Look.Enable(); // Activamos la acción
    }

    void OnDisable()
    {
        controls.Camera.Look.Disable(); // La desactivamos al salir
    }

    void Start()
    {
        // 🔒 Bloquear cursor dentro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 🖱️ Leemos el input del mouse
        lookInput = controls.Camera.Look.ReadValue<Vector2>();

        // Aplicamos sensibilidad
        yaw += lookInput.x * mouseSensitivity;
        pitch -= lookInput.y * mouseSensitivity;

        // Limitamos ángulo vertical
        pitch = Mathf.Clamp(pitch, -10f, 60f);
    }

    void LateUpdate()
    {
        if (ball == null) return;

        // Calculamos la rotación de la cámara
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // Posición deseada (detrás y arriba del jugador)
        Vector3 desiredPosition = ball.position - rotation * Vector3.forward * distance + Vector3.up * height;

        // Movimiento suave
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, 1f / smoothSpeed);

        // Mirar hacia la pelota
        transform.LookAt(ball.position + Vector3.up * 1f);
    }
}

