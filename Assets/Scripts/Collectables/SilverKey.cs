using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilverKey : MonoBehaviour
{
    //Controlador de las llaves plateadas, añade una al contador de llaves plateadas y activa la imagen de la llave en la interfaz
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            GameManager.Instance.playerSilverKeys++;
            GameObject.FindGameObjectWithTag("Respawn").GetComponent<HUDController>().silverKey.enabled = true;
            Destroy(gameObject);
        }
    }
}
