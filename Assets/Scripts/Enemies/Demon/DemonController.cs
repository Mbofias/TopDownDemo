using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonController : Enemy
{
    public GameObject explosion;
    public float explosionRadius = .5f;
    private bool playerDetected;

    void Start()
    {
        //Setea los valores del enemigo
        health = maxHealth;
        playerDetected = false;
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            targets.Add(player.transform);
        }
        sprt = gameObject.GetComponent<SpriteRenderer>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();

        //Reestablece la posición para empezar a patrullar
        ResetPatrol();
    }
    void Update()
    {
        Attack();
        if (health <= 0)
            Die();
    }
    void FixedUpdate()
    {
            Move();
    }

    //Función de recibir daño, funciona igual que las otras funciones de recibir daño
    public override void TakeDamage(Bofias.SpellElement element)
    {
        AudioSource.PlayClipAtPoint(damage, transform.position);

        switch (element)
        {
            case Bofias.SpellElement.FIRE:
                health -= 5;
                break;
            case Bofias.SpellElement.ICE:
                health -= 25;
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

    //La función de ataque del demonio, comprueba si el player esta lo bastante cerca, y en dicho caso explota
    private void Attack()
    {
        Vector3 toPlayer = (GetClosestTarget() - transform.position);
        if (toPlayer.sqrMagnitude <= explosionRadius * explosionRadius)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Die();
        }
    }


    private void DisableSlow()
    {
        slowed = false;
        DismissIceDebuff();
    }

    //Si aún no ha detectado al player, el demonio va eligiendo un punto de patrulla diferente cada 2 segundos
    //y se mueve por el mapa de forma aleatoria
    private void Move()
    {
        if (!playerDetected)
        {
            if (Vector3.Distance(transform.position, PatrolPoint) < .1 || patrolPoint == null)
            {
                float angle = Random.Range(0f, 360f);

                Vector3 towardsPatrolPoint = Quaternion.AngleAxis(angle, Vector3.forward) * (transform.position + Vector3.up * perceptionRadius);
                PatrolPoint = transform.position + towardsPatrolPoint;
                Invoke("ResetPatrol", 2f);
            }
            Bofias.MoveCharacter(rb2d, PatrolPoint, speed);
        }
        //Cuando ha detectado al player este persigue al player
        else
            Bofias.MoveCharacter(rb2d, GetClosestTarget(), speed);

        if ((GetClosestTarget() - transform.position).sqrMagnitude < perceptionRadius * perceptionRadius)
        {
            playerDetected = true;
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }
}