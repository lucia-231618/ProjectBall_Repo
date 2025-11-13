using UnityEngine;
<<<<<<< HEAD
<<<<<<< HEAD
=======
using Unity.Mathematics;
>>>>>>> 214a04f34c489a1e73df1fd02993fc54d901e823
=======
using Unity.Mathematics;
>>>>>>> 214a04f34c489a1e73df1fd02993fc54d901e823

public class CameraFollow : MonoBehaviour
{
    [Header("Ajustes")]
<<<<<<< HEAD
<<<<<<< HEAD
    [SerializeField] private float sensibilidad = 100f;
    [SerializeField] private Transform player;

    [Header("Offset")]
    [SerializeField] private Vector3 offset = new Vector3(0, 2, -5); // altura y distancia detrás del jugador

    [Header("Rotación")]
    private float rotacionVertical = 10f; // ángulo de cámara ligeramente arriba
    private float rotacionHorizontal = 0f;

    void Start()
    {
        if (player == null)
            Debug.LogWarning("CameraFollow: asigna un jugador en el Inspector.");

=======
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
>>>>>>> 214a04f34c489a1e73df1fd02993fc54d901e823
=======
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
>>>>>>> 214a04f34c489a1e73df1fd02993fc54d901e823
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
<<<<<<< HEAD
<<<<<<< HEAD
        if (player == null) return;

        // --- Input del mouse ---
        float mouseX = Input.GetAxis("Mouse X") * sensibilidad * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidad * Time.deltaTime;

        rotacionHorizontal += mouseX;
        rotacionVertical -= mouseY;
        rotacionVertical = Mathf.Clamp(rotacionVertical, -30f, 30f); // limita ángulo vertical

        // --- Calculamos posición de la cámara detrás del jugador ---
        Quaternion rotacion = Quaternion.Euler(rotacionVertical, rotacionHorizontal, 0);
        Vector3 posicionDeseada = player.position + rotacion * offset;

        transform.position = posicionDeseada;
        transform.LookAt(player.position + Vector3.up * 1f); // mirar al centro de la pelota/jugador

        // --- Rotación horizontal del jugador solo en Y ---
        Vector3 jugadorEuler = player.eulerAngles;
        jugadorEuler.y = rotacionHorizontal;
        player.eulerAngles = jugadorEuler;
=======
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

=======
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

>>>>>>> 214a04f34c489a1e73df1fd02993fc54d901e823
        // Limitar rotación vertical
        RotacionVertical = math.clamp(RotacionVertical, -80f, 80f);

        // Aplicar rotación vertical solo a la cámara
        transform.localRotation = Quaternion.Euler(RotacionVertical, 0, 0);

        // Aplicar rotación horizontal al jugador
        Player.Rotate(Vector3.up * ValorX);
<<<<<<< HEAD
>>>>>>> 214a04f34c489a1e73df1fd02993fc54d901e823
=======
>>>>>>> 214a04f34c489a1e73df1fd02993fc54d901e823
    }
}


