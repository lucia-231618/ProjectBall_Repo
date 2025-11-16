using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [Header("Editor References")]
    public AudioSource playerAudio;

    [Header("Movement Parameters")]
    public float speed = 3;
    public Vector2 moveInput;

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
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Falta Rigidbody en el GameObject!");
            enabled = false;
            return;
        }

        // Intentar asignar cámara automáticamente
        if (camara == null)
        {
            if (Camera.main != null)
                camara = Camera.main.transform;
            else
            {
                // Buscar cámara con CameraFollow si no hay Main Camera
                CameraFollow cameraScript = FindFirstObjectByType<CameraFollow>();  // Cambiado para evitar deprecación
                if (cameraScript != null)
                    camara = cameraScript.transform;
                else
                {
                    Debug.LogError("PlayerController: No se encontró una cámara. Asigna manualmente en el Inspector.");
                    enabled = false;
                    return;
                }
            }
        }

        if (playerAudio == null)
            Debug.LogWarning("Falta AudioSource asignado!");
        //Valores powerup
        originalJumpForce = jumpForce;
    }

    void Update()
    {
        if (transform.position.y <= fallLimit)
            Respawn();
    }

    void FixedUpdate()
    {
        MoverJugador();
        // AplicarGravedad();  // Comentado: Unity maneja gravedad por defecto. Descomenta si necesitas custom.
    }

    private void MoverJugador()
    {
        if (camara == null) return;  // Evitar errores si no se asignó

        float ValorHorizontal = moveInput.x;
        float ValorVertical = moveInput.y;

        Vector3 adelanteCamara = camara.forward;
        Vector3 derechaCamara = camara.right;
        adelanteCamara.y = 0f;
        derechaCamara.y = 0f;
        adelanteCamara.Normalize();
        derechaCamara.Normalize();

        Vector3 direccionplano = derechaCamara * ValorHorizontal + adelanteCamara * ValorVertical;

        if (direccionplano.sqrMagnitude > 0.0001f)
            direccionplano.Normalize();

        rb.MovePosition(rb.position + direccionplano * speed * Time.fixedDeltaTime);

        // Rotar al jugador hacia la dirección (solo si hay movimiento)
        if (direccionplano != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, direccionplano, 0.1f);
        }
    }

    // private void AplicarGravedad()  // Deshabilitado por defecto
    // {
    //     if (!isGrounded)
    //     {
    //         rb.AddForce(Vector3.down * 9.8f, ForceMode.Acceleration);
    //     }
    // }

    void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            PlaySFX(0);
        }
    }

    void Respawn()
    {
        rb.position = respawnPoint.position;
        rb.linearVelocity = Vector3.zero;
        PlaySFX(2);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    public void PlaySFX(int soundToPlay)
    {
        if (playerAudio != null && soundCollection.Length > soundToPlay)
            playerAudio.PlayOneShot(soundCollection[soundToPlay]);
    }

    #region Input Methods
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Jump();
        }
    }
    #endregion

    //Intento de añadir los valores a los pickup
   
    // Variables para powerups (agrega al inicio de la clase, con las otras variables)
    private float originalJumpForce;
    private bool isJumpBoosted = false;

    // En el método Start() (o crea uno si no existe)
    void Start()
    {
        originalJumpForce = jumpForce;  // Guarda el valor original del salto
    }

    // Método para activar jump boost
    public void ActivateJumpBoost(float duration)
    {
        if (!isJumpBoosted)
        {
            isJumpBoosted = true;
            jumpForce *= 1.5f;  // Aumenta el salto (cambia 1.5f por lo que quieras, ej. 2f)
            StartCoroutine(DeactivateJumpBoost(duration));
        }
    }

    // Corutina para desactivar
    private System.Collections.IEnumerator DeactivateJumpBoost(float duration)
    {
        yield return new WaitForSeconds(duration);
        jumpForce = originalJumpForce;  // Restaura el salto
        isJumpBoosted = false;
    }

}



