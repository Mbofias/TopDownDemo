using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpell : BulletController
{
    //Función del proyectil del jefe, hace daño al player, quitandole un corazón entero de vida
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(true);
        Destroy(gameObject);
    }
}
