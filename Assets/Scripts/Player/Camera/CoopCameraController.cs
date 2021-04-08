using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoopCameraController : MonoBehaviour
{
    [Range(0f, 1f)] public float cameraZone = 0.5f;
    [Range(0f, 1f)] public float smoothFactor = 0.1f;

    private float zoneH, zoneW;
    private List<Transform> targets = new List<Transform>();
    private Vector3 playersCentralPoint;
    private Bounds deadZoneCube;

    void Start()
    {
        //Guarda la cámara y establece una zona "segura" dentro de la cual tiene que estar el punto medio entre los 
        //dos jugadores para que la cámara se quede quieta
        zoneH = Camera.main.orthographicSize * cameraZone;
        zoneW = zoneH * Camera.main.aspect;
        deadZoneCube = new Bounds(transform.position, new Vector3(zoneW * 2, zoneH * 1.5f, 1f));

        //Guarda los jugadores en la lista de "targets"
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            targets.Add(player.transform);
        }
    }

    void Update()
    {
        //Si el punto medio de los dos jugadores esta fuera de la "zona segura" esta se mueve con una interpolación
        playersCentralPoint = targets[0].position + (targets[1].position - targets[0].position) / 2;
        playersCentralPoint.z = transform.position.z;
        transform.position = Vector3.Lerp(transform.position, playersCentralPoint, smoothFactor);
    }
}
