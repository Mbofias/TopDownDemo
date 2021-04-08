using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public GameObject key, coin;
    public Transform keySpawner;
    public AudioClip damage;

    private MeshRenderer textRender;
    private TextMesh phrase;
    private bool soldKey;
    private GameObject[] peaks;

    // Start is called before the first frame update
    void Start()
    {
        //establece las variables del NPC
        phrase = GetComponentInChildren<TextMesh>();
        textRender = GetComponentInChildren<MeshRenderer>();
        peaks = GameObject.FindGameObjectsWithTag("Peak");
        soldKey = false;

        //modifica los valores del TextRender para que se renderice por encima de los elementos del mapa
        textRender.sortingLayerName = "Spells";
        textRender.sortingOrder = 0;
        textRender.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Cuando el player se acerca se activa el texto  y comprueba si tiene que venderle la llave
            textRender.enabled = true;
            if (GameManager.Instance.playerCoins >= 5 && !soldKey)
                GiveKey();
            else if (soldKey)
                SoldKeyMessage();
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Cuando el jugador se aleja del npc se desactiva el texto
            //Cuando están los picos activados y el jugador se aleja del npc se desactivan
            textRender.enabled = false;
            if (PeakController.Activated)
                foreach (GameObject peak in peaks)
                {
                    peak.GetComponent<PeakController>().Deactivate();
                }
        }
    }

    private void GiveKey () 
    {
        //Esta función vende la llave al player
        soldKey = true;
        GameManager.Instance.playerCoins = 0;
        GameObject.FindGameObjectWithTag("Respawn").GetComponent<HUDController>().DestroyCoins();
        phrase.text = "Muchas gracias,\ntoma tu llave!";
        Instantiate(key, keySpawner.position, keySpawner.rotation);
    }
    private void SoldKeyMessage()
    {
        //Esta función se llama cuando el player ya ha comprado la llave y se acerca al npc
        //Una vez ejecutada esta función el player puede matar al NPC
        phrase.text = "No tengo mas llaves,\nve a molestar a otro";
        gameObject.tag = "NPC";
    }

    public void Die()
    {
        //El NPC suelta las 5 monedas otra vez al morir
        AudioSource.PlayClipAtPoint(damage, transform.position);
        Instantiate(coin, transform.position + Vector3.left, new Quaternion(0,0,0,0));
        Instantiate(coin, transform.position + Vector3.right, new Quaternion(0, 0, 0, 0));
        Instantiate(coin, transform.position + Vector3.up, new Quaternion(0, 0, 0, 0));
        Instantiate(coin, transform.position + Vector3.down, new Quaternion(0, 0, 0, 0));
        Instantiate(coin, transform.position, new Quaternion(0, 0, 0, 0));
        Destroy(gameObject);
    }
}
