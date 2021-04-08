using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenKey : MonoBehaviour
{    
    //Controlador de la llave dorada, setea la variable a true en el GM y activa la imagen de la llave en la interfaz
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            GameManager.Instance.playerGoldenKey = true;
            GameObject.FindGameObjectWithTag("Respawn").GetComponent<HUDController>().goldenKey.enabled = true;

            Destroy(gameObject);
        }
    }
}
