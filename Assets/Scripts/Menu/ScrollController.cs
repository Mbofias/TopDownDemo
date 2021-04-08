using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollController : MonoBehaviour
{
    public Transform left, right;
    public float speed;

    private SpriteRenderer[] sprs;
    private float movement;
    //Controlador del scroll

    void Start()
    {
        //Guarda los sprites en el array
        sprs = GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {
        //Mueve el sprite de la derecha hacia arriba y el de la izquierda hacia abajo
        movement = speed * Time.deltaTime;

        foreach (SpriteRenderer spr in sprs)
        {
            spr.transform.Translate(new Vector3(movement, 0, 0));

            if (spr.transform.position.x == left.position.x)
            {
                if (spr.transform.position.y < right.position.y)
                {
                    spr.transform.SetPositionAndRotation(right.position, right.rotation);
                }
            }
            if (spr.transform.position.x == right.position.x)
            {
                if (spr.transform.position.y > left.position.y)
                {
                    spr.transform.SetPositionAndRotation(left.position, left.rotation);
                }
            }
        }
        //Cuando los sprites salen de la pantalla se cambian de lado y de dirección
    }
}
