using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] players = new GameObject[3];
    private Camera cam;
    private CoopCameraController coop;
    private SingleCameraController single;

    //Spawnea a los players necesarios dependiendo de la selección del modo de juego en el menú y selecciona el tipo de cámara
    void Awake()
    {
        //Guarda los dos scripts de la cámara, esto lo he mantenido por si en algún momento se decide cambiar 
        //la dinámica de muerte en el modo cooperativo y se tenga que cambiar de cámara a media partida
        cam = Camera.main;
        coop = cam.GetComponent<CoopCameraController>();
        single = cam.GetComponent<SingleCameraController>();
        if (GameManager.Instance.players == 1)
        {
            Instantiate(players[0], transform.position, transform.rotation);
            coop.enabled = false;
        }
        else 
        {
            Instantiate(players[1], transform.position + Vector3.right, transform.rotation);
            Instantiate(players[2], transform.position + Vector3.left, transform.rotation);
            single.enabled = false;
        }

        //Una vez completado se elimina el objeto
        Destroy(gameObject);
    }
}
