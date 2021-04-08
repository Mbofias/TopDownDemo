using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public AnimationClip explosion;

    void Start()
    {
        //Elimina la explosión cuando acaba la animación
        Destroy(gameObject, explosion.length);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        //Si la explosión le da al player le quita 1 corazón de vida
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(true);
        }
    }
}
