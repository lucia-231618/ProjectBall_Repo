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
    public GameUI gameUI;  // Agregado para SpeedBoost (asigna en Inspector)

    private Rigidbody rb;

    // Variables para power-ups
    private float originalJumpForce;
    private bool isJumpBoosted = false;

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
                CameraFollow cameraScript = FindFirstObjectByType<CameraFollow>();
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

        // Inicializar valores de power-ups
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
    }

    private void MoverJugador()
    {
        if (camara == null) return;

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

        if (direccionplano != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, direccionplano, 0.1f);
        }
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);  // Usa jumpForce (modificado por power-ups)
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

    // Métodos para power-ups
    public void ActivateJumpBoost(float duration)
    {
        if (!isJumpBoosted)
        {
            isJumpBoosted = true;
            jumpForce *= 1.5f;  // Aumenta el salto (cambia 1.5f si quieres más/menos, ej. 2f)
            StartCoroutine(DeactivateJumpBoost(duration));
            Debug.Log("JumpBoost activado por " + duration + " segundos");
        }
    }

    private System.Collections.IEnumerator DeactivateJumpBoost(float duration)
    {
        yield return new WaitForSeconds(duration);
        jumpForce = originalJumpForce;
        isJumpBoosted = false;
        Debug.Log("JumpBoost desactivado");
    }

    public void ActivateSpeedBoost(float timeToAdd)
    {
        if (gameUI != null)
            gameUI.RemoveTime(-timeToAdd);  // Añade tiempo al contador
        Debug.Log("SpeedBoost: añadido " + timeToAdd + " segundos al contador");
    }
}




