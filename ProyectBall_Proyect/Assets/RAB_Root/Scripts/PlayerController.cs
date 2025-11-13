using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Editor References")]
    public Rigidbody playerRb; //Referencia al Rigidbody del player
    public AudioSource playerAudio; //Ref al emisor de sonidos del player

    [Header("Movement Parameters")]
    public float speed = 3;
    public Vector2 moveInput; //Almacén del input de movimiento

    [Header("Jump Parameters")]
    public float jumpForce = 6;
    public bool isGrounded = true;

    [Header("Respawn System")]
    public float fallLimit = -10;
    public Transform respawnPoint;

    [Header("Sound Configuration")]
    public AudioClip[] soundCollection;

    [Header("Referencias")]
    [SerializeField] private Transform camara;
    private CharacterController controlador;

    [Header("Gravedad")]
    [SerializeField] private float GravedadDelJugador = -9f;
    private Vector3 velocidadVertical;

    private void Awake()
    {
        controlador = GetComponent<CharacterController>();

        if (camara == null && Camera.main != null)
            camara = Camera.main.transform;
    }

    void Start()
    {

    }

    void Update()
    {
        if (transform.position.y <= fallLimit)
            Respawn();

        MoverJugadorEnPlano();   // Aquí se aplica el movimiento con respecto a la cámara
        AplicarGravedad();
    }

    private void MoverJugadorEnPlano()
    {
        // Capturamos las teclas (AWSD y Flechas)
        float ValorHorizontal = Input.GetAxisRaw("Horizontal");
        float ValorVertical = Input.GetAxisRaw("Vertical");

        // Calculamos hacia donde mira la cámara solo en eje XZ
        Vector3 adelanteCamara = camara.forward;
        Vector3 derechaCamara = camara.right;
        adelanteCamara.y = 0f;
        derechaCamara.y = 0f;
        adelanteCamara.Normalize();
        derechaCamara.Normalize();

        // Combina input con dirección de la cámara
        Vector3 direccionplano = derechaCamara * ValorHorizontal + adelanteCamara * ValorVertical;

        // Normaliza para que diagonal no sea más rápida
        if (direccionplano.sqrMagnitude > 0.0001f)
            direccionplano.Normalize();

        // --- NUEVO: mover al jugador usando CharacterController ---
        if (controlador != null)
        {
            controlador.Move(direccionplano * speed * Time.deltaTime);
        }

        // --- NUEVO: actualizar la rotación horizontal del jugador con la cámara ---
        if (direccionplano != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, direccionplano, 0.1f);
        }
    }

    private void AplicarGravedad()
    {
        velocidadVertical.y += GravedadDelJugador * Time.deltaTime;
        controlador.Move(velocidadVertical * Time.deltaTime);

        if (controlador.isGrounded && velocidadVertical.y < 0)
        {
            velocidadVertical.y = -2f;
        }
    }

    private void FixedUpdate()
    {
        PhysicalMovement();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Devuelve la capacidad de saltar
        }
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Respawn();
        }
    }

    void PhysicalMovement()
    {
        playerRb.AddForce(Vector3.right * speed * moveInput.x);
        playerRb.AddForce(Vector3.forward * speed * moveInput.y);
    }

    void Jump()
    {
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        PlaySFX(0);
    }

    void Respawn()
    {
        transform.position = respawnPoint.position;
        playerRb.linearVelocity = new Vector3(0, 0, 0);
        PlaySFX(2);
    }

    public void PlaySFX(int soundToPlay)
    {
        playerAudio.PlayOneShot(soundCollection[soundToPlay]);
    }

    #region Input Methods

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded == true)
        {
            isGrounded = false;
            Jump();
        }
    }

    #endregion
}

