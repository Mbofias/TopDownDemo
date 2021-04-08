using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    //La puerta se abre cuando el jugador choca con ella teniendo una llave dorada
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && GameManager.Instance.playerGoldenKey)
        {
            Destroy(gameObject);
            GameManager.Instance.playerGoldenKey = false;
            GameObject.FindGameObjectWithTag("Respawn").GetComponent<HUDController>().goldenKey.enabled = false;
        }
    }
}
