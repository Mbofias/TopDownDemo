using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100f, speed = 3f, slowDuration = 1f, perceptionRadius = 2.5f;
    public AudioClip damage;

    protected Vector3 patrolPoint;
    protected GameObject target;
    protected Rigidbody2D rb2d;
    protected Animator animator;
    protected float health, mana;
    protected List<Transform> targets = new List<Transform>();
    protected SpriteRenderer sprt;
    protected bool slowed = false;

    public GameObject Target { get => target; set => target = value; }
    public Rigidbody2D RB2D { get => rb2d; set => rb2d = value; }
    public float Health { get => health; set => health = value; }
    public float Mana { get => mana; set => mana = value; }
    public Animator Animator { get => animator; set => animator = value; }
    public Vector3 PatrolPoint { get => patrolPoint; set => patrolPoint = value; }

    public virtual Vector3 GetClosestTarget()
    {
        //Esta función elige el player mas cercano de la lista de jugadores
        if (targets.Count > 1)
            if ((targets[0].position - transform.position).sqrMagnitude > (targets[1].position - transform.position).sqrMagnitude)
                return targets[1].position;
        return targets[0].position;
    }

    public virtual void TakeDamage(Bofias.SpellElement element)
    {
        //Función genérica de recibir daño de los enemigos, la llama siempre una clase externa
        AudioSource.PlayClipAtPoint(damage, transform.position);
        health -= 10;
    }

    public void ResetPatrol()
    {
        //Función que setea el punto de patrulla a la posición del player
        patrolPoint = transform.position;
    }

    //Funciones que aplican y eliminan el estado de congelación
    public void ApplyIceDebuff()
    {
        speed *= .8f;
        sprt.color = Color.cyan;
    }
    public void DismissIceDebuff()
    {
        speed /= .8f;
        sprt.color = Color.white;
    }
}
