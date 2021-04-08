using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseLayer;
    private bool isPaused;

    //Script que controla el menú de pausa InGame
    void Start()
    {
        //Si estas jugando en cooperativo se elimina el botón de cambio de controles
        if (GameManager.Instance.players != 1)
            Destroy(GameObject.FindGameObjectWithTag("Control"));
        pauseLayer.SetActive(false);
        isPaused = false;
    }
    //En el update se comprueba si se ha activado/desactivado el menú de pausa
    void Update()
    {
        if (Input.GetButtonDown("Pause") && !isPaused)
        {
            ShowPauseMenu();
        }
        else if (Input.GetButtonDown("Pause") && isPaused)
        {
            HidePauseMenu();
        }
    }

    //Funciones de activación y desactivación del menú de pausa
    private void ShowPauseMenu()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseLayer.SetActive(true);
    }
    public void HidePauseMenu()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseLayer.SetActive(false);
    }

    //Función para el botón de volver al menú
    public void GoBackToMainMenu()
    {
        Time.timeScale = 1f;
        GameManager.RestartGame();
    }

    //Función para cambiar entre el uso de Mando y Teclado (Solo funciona en singleplayer)
    public void ChangeController()
    {
        switch (GameManager.Instance.playerController[0])
        {
            case Bofias.UsingInput.CONTROLLER:
                GameManager.Instance.playerController[0] = Bofias.UsingInput.KEYBOARD;
                break;
            case Bofias.UsingInput.KEYBOARD:
                GameManager.Instance.playerController[0] = Bofias.UsingInput.CONTROLLER;
                break;
            default: break;
        }
    }
}
