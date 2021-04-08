using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    //Controlador de monedas, en caso de no haber comprado la llave dorada aún se encarga de enviar a la interfaz la actualización de las monedas
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            GameManager.Instance.playerCoins++;
            // Aquí uso el try/catch para evitar que de un error al intentar acceder a un elemento que ya no existe(en el UpdateCoins())
            try
            {
                GameObject.FindGameObjectWithTag("Respawn").GetComponent<HUDController>().UpdateCoins();
            }
            catch (Exception e)
            {
                print(e);
            }
            Destroy(gameObject);
        }
    }
}
