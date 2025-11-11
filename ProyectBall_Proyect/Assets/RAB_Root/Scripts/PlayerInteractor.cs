using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Points System")]
    public int foundPolizones = 0;  //Cuantos polizones llevas
    public int totalPolizones = 5;  //Total de polizones en el nivel
    public TMP_Text pointsText;     //Texto en la UI para mostrar polizones
   
    [Header("Scene Management")] 
    public int sceneToLoad = 2;     //Escena a cargar al ganar

    [Header("Sound References")]
    public PlayerController playerCont; //Ref de sonido al recoger los polizones

    private void UpdateUI() //Actualiza el texto de la UI cada vez que recoges un polizón.
    {
        if (pointsText != null)
            pointsText.text = "Polizones: " + foundPolizones + " / " + totalPolizones;
    }

    private void WinLevel()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foundPolizones = 0;
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        // Revisar si ya recogiste todos los polizones
        if (foundPolizones >= totalPolizones)
        {
            WinLevel();
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Polizon")) // Detecta objetos etiquetados como "Polizon"
        {
            foundPolizones++;

            other.gameObject.SetActive(false);     // Desactiva el polizón en lugar de destruirlo

        if (playerCont != null)                    // Reproduce sonido del player
                playerCont.PlaySFX(1);

                         UpdateUI();               // Actualiza UI
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

}
