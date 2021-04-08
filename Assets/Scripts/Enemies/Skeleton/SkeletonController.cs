using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : Enemy
{
    private NecromancerController owner;

    void Start()
    {
        //Setea los valores del enemigo
        health = maxHealth;

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            targets.Add(player.transform);
        }

        sprt = gameObject.GetComponent<SpriteRenderer>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();

        //Guarda una referencia del script del padre(invocador), y después separa al esqueleto del nigromante
        owner = GetComponentInParent<NecromancerController>();
        transform.parent = null;
    }
    void Update()
    {
        //Si el esqueleto baja de 0 de vida este muere
        if (health <= 0)
            Die();
    }
    void FixedUpdate()
    {
        //Si el nigromante que ha invocado al esqueleto muere, este muere con el
        if (owner == null)
            Die();
        else
            Move();
    }

    //Cuando impacta con el player le inflige  1/2 corazón de daño
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(false);
        }
    }

    //Función de recibir daño del esqueleto, funciona igual que las demás funciones de recibir daño de los enemigos
    public override void TakeDamage(Bofias.SpellElement element)
    {
        AudioSource.PlayClipAtPoint(damage, transform.position);

        switch (element)
        {
            case Bofias.SpellElement.FIRE:
                health -= 15;
                break;
            case Bofias.SpellElement.ICE:
                health -= 10;
                if (!slowed)
                {
                    slowed = true;
                    ApplyIceDebuff();
                    Invoke("DisableSlow", slowDuration);
                }
                else
                {
                    CancelInvoke("DisableSlow");
                    Invoke("DisableSlow", slowDuration);
                }
                break;
            default: break;
        }
    }

    //Función que quita el estado de congelación
    private void DisableSlow()
    {
        slowed = false;
        DismissIceDebuff();
    }

    //Función que mueve el personaje, en este caso el esqueleto se mueve hacia el jugador mas cercano a su invocador
    //para proteger al nigromante
    private void Move()
    {
        Bofias.MoveCharacter(rb2d, owner.GetClosestTarget(), speed);
    }

    //Cuando el esqueleto muere resta uno a la variable de esqueletos invocados de su invocador
    private void Die()
    {
        owner.CurrentSkeletons--;
        Destroy(gameObject);
    }
}
