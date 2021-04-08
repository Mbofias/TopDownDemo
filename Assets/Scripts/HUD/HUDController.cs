using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    float[] rewindCDstatus = new float[2];
    float rewindTotalCD;
    public Image[] coolDown;
    public Text coins;
    public Image goldenKey, silverKey;
    public GameObject bossHealthBar;
    private RectTransform barPercent;

    public RectTransform BarPercent { get => barPercent; set => barPercent = value; }

    //Desactiva la barra de vida del boss al principio del juego
    //En caso de jugar un solo player elimina todos los elementos de la interfaz que son para el modo cooperativo
    void Start()
    {
        bossHealthBar.SetActive(false);

        if (GameManager.Instance.players == 1)
        {
            //Elimina la parte del HUD del player 2 y convierte el array del "rewind" en un array de un solo elemento
            Destroy(GameObject.FindGameObjectWithTag("WitchHealth"));
            Destroy(coolDown[1].gameObject);
            rewindCDstatus = new float[1];
        }

        //Una vez eliminado (si es necesario) el HUD del player dos se setea el estado del HUD
        rewindTotalCD = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().rewindCD;
        for (int index = 0; index < rewindCDstatus.Length; index++)
        {
            rewindCDstatus[index] = rewindTotalCD;
        }
        goldenKey.enabled = false;
        silverKey.enabled = false;
        UpdateCoins();
    }

    //En el update comprueba el estado de la mecánica de "rewind"
    private void Update()
    {
        for (int index = 0; index < rewindCDstatus.Length; index++)
        {
            if (rewindCDstatus[index] < rewindTotalCD)
            {
                //En caso de estar en CD actualiza la transparencia del sprite que indica el CD
                rewindCDstatus[index] += Time.deltaTime;
                if (rewindCDstatus[index] > rewindTotalCD)
                    rewindCDstatus[index] = rewindTotalCD;
                UpdateRewind(index);
            }
        }
    }

    //Funciones para controlar la imagen que indica el CD del rewind
    public void UsedCD(int index)
    {
            rewindCDstatus[index] = 0;
    }
    public void UpdateRewind(int index)
    {
            Color tmp = coolDown[index] .color;
            tmp.a = rewindCDstatus[index] / rewindTotalCD;
            coolDown[index].color = tmp;
    }

    //Funciones para controlar el contador de monedas
    public void UpdateCoins()
    {
        coins.text = GameManager.Instance.playerCoins + "/5";
    }

    //Función que destruye el contador de monedas del HUD
    public void DestroyCoins()
    {
        Destroy(coins.transform.parent.gameObject);
    }
}