using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSpell : BulletController
{
    public Bofias.SpellElement element;

    //Función que detecta la colision del proyectil del jugador, rompe cajas, hace daño a enemigos 
    //y mata al npc una vez llegado a cierto diálogo

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Boss":
                collision.gameObject.GetComponent<BossController>().CurrentState.TakeDamage(element);
                break;
            case "Enemy":
                collision.gameObject.GetComponent<Enemy>().TakeDamage(element);
                break;
            case "Box":
                collision.gameObject.GetComponent<BoxController>().Break();
                break;
            case "NPC":
                if(!collision.isTrigger)
                    collision.gameObject.GetComponent<NPCController>().Die();
                break;
            default: break;
        }

        //así no colisiona con triggers
        if (!collision.isTrigger)
            Destroy(gameObject);
    }
}
