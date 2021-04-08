using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleCameraController : MonoBehaviour
{
    [Range(0f, 1f)] public float cameraZone = 0.5f;
    [Range(0f, 1f)] public float smoothFactor = 0.1f;

    private float zoneH, zoneW;
    private Transform target;
    private Bounds safeZoneCube;

    void Start()
    {
        //Establece la "zona segura" de la cámara
        zoneH = Camera.main.orthographicSize * cameraZone;
        zoneW = zoneH * Camera.main.aspect;
        safeZoneCube = new Bounds(transform.position, new Vector3(zoneW * 2, zoneH * 1.5f, 1f));

        //Selecciona al player al que tiene que seguir la cámara
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        //Si el player esta fuera de la "zona segura" la playa se mueve mediante una interpolación siguiendo al player
        float x, y;
        if (!SafeZone( out x, out y))
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(x, y, 0f), smoothFactor);
        }
    }

    //Función que comprueba si el player esta fuera de la "zona segura" de la cámara
    private bool SafeZone(out float x, out float y)
    {
        safeZoneCube.center = transform.position;
        if (!safeZoneCube.Contains(target.position))
        {
            Vector3 point = safeZoneCube.ClosestPoint(target.position);
            x = target.position.x - point.x;
            y = target.position.y - point.y;
            return false;
        }
        x = y = 0;

        return true;
    }
}
