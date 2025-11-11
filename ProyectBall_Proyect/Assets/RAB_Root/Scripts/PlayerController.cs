using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Editor References")]
    public Rigidbody playerRb; //Referencia al Rigidbody del player
    public AudioSource playerAudio; //Ref al emisor de sonidos del player

    [Header("Movement Parameters")]
    public float speed = 10;
    public Vector2 moveInput; //Almacén del input de movimiento de los periféricos que usamos para jugar

    [Header("Jump Parameters")]
    public float jumpForce = 6;
    public bool isGrounded = true;

    [Header("Respawn System")]
    public float fallLimit = -10;
    public Transform respawnPoint;

    [Header("Sound Configuration")]
    public AudioClip[] soundCollection;

    public void SumarPolizon()
    {
        foundPolizones++;
        PlaySFX(1); // sonido opcional
    }

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
        //CinematicMovement();
        //Respawn por altura
        if (transform.position.y <= fallLimit)
        {
            Respawn();
        }
       
        Contador();



    }

    private void FixedUpdate()
    {
        //Update para calcular movimientos físicos
        PhysicalMovement();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; //Devuelve la capacidad de saltar
        }
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Respawn();
        }
    }

    private void Contador()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;  //Cada frame se resta Time.deltaTime.
            UpdateTimerUI();
        }
        else
        {
            GameOver();                       //Si llega a 0 ? GameOver() y se reinicia la escena
        }

        if (foundPolizones >= totalPolizones)
        {
            WinLevel();                      //Si recoges todos los polizones ? WinLevel() y se carga la siguiente escena
        }
    }

    private void UpdateTimerUI()  //Función para actualizar la UI del tiempo
    {
        if (timeRemaining != null)
    }

    private void WinLevel()
    {
        // Cuando recoges todos los polizones, carga la siguiente escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GameOver()
    {
        // Cuando el tiempo llega a 0, reinicia la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void CinematicMovement()
    {
        //Movimiento = (Dirección * velocidad * input)
        //Necesitais multiplicar el movimiento por Time.deltaTime
        transform.Translate(Vector3.right * speed * moveInput.x * Time.deltaTime);
        transform.Translate(Vector3.forward * speed * moveInput.y * Time.deltaTime);
    }

    void PhysicalMovement()
    {
        //Añadir una fuerza al rigidbody = (Dirección * velocidad * input)
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
        //Sustituir el transform.position del player por el del punto de respawn
        transform.position = respawnPoint.position;
        //Resetear el valor de aceleración del rigidbody
        playerRb.linearVelocity = new Vector3(0,0,0);
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
