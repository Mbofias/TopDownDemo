using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    public Sprite[] options;
    public Image logo;
    public Text message;

    //Controlador del menú de GameOver/Has Ganado
    void Start()
    {
        //Si has completado el juego se muestra como menú de "Has Ganado"
        if (GameManager.Instance.gameCompleted)
        {
            message.text = "HAS COMPLETADO EL JUEGO";
            message.color = Color.green;
            logo.sprite = options[0];
        }
        //En caso contrario se muestra como "Game Over"
        else
        {
            message.text = "HAS MUERTO";
            message.color = Color.red;
            logo.sprite = options[1];
        }
    }

    public void RestartGame()
    {
        GameManager.RestartGame();
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
