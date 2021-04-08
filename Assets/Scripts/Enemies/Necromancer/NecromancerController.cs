using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerController : Enemy
{
    public float spawnCD;
    public int maxSkeletons;
    public GameObject skeleton, key;
    public Transform skeletonSpawner;
    public bool hasKey = false;

    private int currentSkeletons;
    private bool spawnAvailable;
    private NecromancerState currentState;

    public bool SpawnAvailable { get => spawnAvailable; set => spawnAvailable = value; }
    public int CurrentSkeletons { get => currentSkeletons; set => currentSkeletons = value; }
    public NecromancerState CurrentState { get => currentState; }

    void Start()
    {
        //Setea los valores del enemigo
        health = maxHealth;

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            targets.Add(player.transform);
        }
        
        //En caso de ser un nigromante invocado por el boss directamente detecta al player, entrando al estado de huida
        //en caso contrario el nigromante patrulla hasta detectar al player, así que entra en el estado de patrulla
        if (transform.parent == null)
            currentState = new NecromancerPatrolState(this);
        else
        {
            currentState = new NecromancerRunState(this);
            transform.parent = null;
        }
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        animator = gameObject.GetComponent<Animator>();
        spawnAvailable = true;
        sprt = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        currentState.Attack();
        if (health <= 0)
            Die();
    }
    void FixedUpdate()
    {
        currentState.Move();
        currentState.Rotate();
    }
    void LateUpdate()
    {
        currentState = currentState.GetNextState();
    }

    public override void TakeDamage(Bofias.SpellElement element)
    {
        AudioSource.PlayClipAtPoint(damage, transform.position);

        switch (element)
        {
            case Bofias.SpellElement.FIRE:
                health -= 35;
                break;
            case Bofias.SpellElement.ICE:
                health -= 5;
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

    //Función de invocación de los esqueletos
    public void Spawn()
    {
        Instantiate(skeleton, skeletonSpawner.position, skeletonSpawner.rotation, transform);
        Invoke("SetSpawnAvailable", spawnCD);
    }
    private void SetSpawnAvailable()
    {
        spawnAvailable = true;
    }

    //Cuando muere, en caso de tener una llave, invoca la llave
    private void Die()
    {
        if (hasKey)
            Instantiate(key, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
