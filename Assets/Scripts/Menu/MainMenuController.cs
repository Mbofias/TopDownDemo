using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject help;
    private bool howToPlay;

    void Start()
    {
        //Se desactiva el menú de ayuda
        howToPlay = false;
        help = GameObject.FindGameObjectWithTag("Help");
        help.SetActive(false);
    }

    //Todos los botones están desactivados en caso de estar activo el menú de ayuda
    //Las siguientes funciones establecen en el GameManager si se juega en solitario o en cooperativo
    public void SinglePlayer()
    {
        if (!howToPlay)
        {
            GameManager.Instance.players = 1;
            SceneManager.LoadScene("InGame");
        }
    }
    public void Cooperative()
    {
        if (!howToPlay)
        {
            GameManager.Instance.players = 2;
            SceneManager.LoadScene("InGame");
        }
    }

    //Activa y desactiva el menú de ayuda
    public void HowToPlay()
    {
        if (howToPlay)
        {
            help.SetActive(false);
            howToPlay = false;
        }
        else
        {
            help.SetActive(true);
            howToPlay = true;
        }
    }

    public void Exit()
    {
        if (!howToPlay)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
