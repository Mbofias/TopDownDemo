using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get => instance; }

    [Range(1, 2)] public int players = 1;
    public int playerCoins = 0;
    public int playerSilverKeys = 0;
    private PlayerController player1, player2;
    public bool playerGoldenKey = false, gameCompleted = false;
    //Esta variable está introducida para el singleplayer y la he creado como array
    //para poder implementar una selección de controles en el modo cooperativo de forma sencilla
    public Bofias.UsingInput[] playerController;

    //La clase GameManager usando el patrón Singleton
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = this;
        else
            DestroyImmediate(gameObject);

        //Por defecto el player usa Teclado y Ratón
        playerController = new Bofias.UsingInput[players];
        playerController[0] = Bofias.UsingInput.KEYBOARD;

        RestartGame();
    }

    //Setea los valores del juego por defecto y carga el menú principal.
    public static void RestartGame()
    {
        instance.playerSilverKeys = 0;
        instance.playerGoldenKey = false;
        instance.playerCoins = 0;
        instance.gameCompleted = false;
        instance.player1 = null;
        instance.player2 = null;
        BoxController.boxesWithReward = 0;
        SceneManager.LoadScene("MainMenu");
    }

    //Se establece la variable de vida en el GameManager, en este caso esto lo uso sobretodo para el multiplayer
    //para tener un control de la vida de ambos jugadores a la hora de acabar la partida con muerte
    public static void SetHealth(GameObject player, bool player1) 
    {
        if (player1)
            instance.player1 = player.GetComponent<PlayerController>();
        else
            instance.player2 = player.GetComponent<PlayerController>();
    }

    //Esta función controla si los jugadores mueren para hacer saltar la pantalla de gameOver

    //Originalmente planeaba cambiar la cámara al modo singleplayer cuando muriera un jugador, pero ya que los ataques
    //se reparten en el cooperativo y la idea es que los jugadores tengan que cooperar entre si
    //he decidido que si un jugador recibe un golpe de gracia el otro le cede uno de sus corazones hasta que ambos se queden sin vida
    //para que ambos jugadores tengan que cooperar intentando superar el juego
    public static void GameOverTest()
    {
        if (instance.players == 2)
        {
            if (instance.player1.Health <= 0 && instance.player2.Health <= 0)
               SceneManager.LoadScene("EndScreen");
            //Comprobamos con estos valores para saber si el personaje "vivo" tiene vida suficiente para cederle medio corazón a su compañero
            else if (instance.player1.Health > instance.player1.maxHealth / 10 && instance.player2.Health <= 0)
            {
                instance.player2.Health += instance.player2.maxHealth / 10;
                instance.player2.IsAlive = true;
                instance.player1.TakeDamage(true); 
            }
            else if ((instance.player2.Health > instance.player2.maxHealth / 10 && instance.player1.Health <= 0))
            {
                instance.player1.Health += instance.player2.maxHealth / 10;
                instance.player1.IsAlive = true;
                instance.player2.TakeDamage(true);
            }

            //En caso de que alguno de los jugadores este muerto al final de estas comprobaciones se acaba la partida
            if(!instance.player1.IsAlive || !instance.player2.IsAlive)
                SceneManager.LoadScene("EndScreen");
        }
        else
        {
            if (instance.player1.Health <= 0)
                SceneManager.LoadScene("EndScreen");
        }
    }
}
