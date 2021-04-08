using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class BulletController : MonoBehaviour
{
    //Clase padre de todos los proyectiles del juego

    [Range(0f,10f)]public float speed = 5f;

    protected Rigidbody2D rb2d;

    //Los proyectiles salen disparados al spawnearse, los proyectiles son Triggers para evitar empujar a los personajes al impactar
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector2(transform.right.x, transform.right.y) * speed;
    }
}
