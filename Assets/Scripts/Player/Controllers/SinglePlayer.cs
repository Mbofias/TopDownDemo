using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayer : PlayerController
{
    void Start()
    {
        //Establece todas las variables del player en el start
        health = maxHealth;
        heartContainers = GameObject.FindGameObjectWithTag("WizardHealth").GetComponent<HeartContainersController>();
        rb2d = GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        InvokeRepeating("PeakDamage", .5f, .5f);
        GameManager.SetHealth(gameObject, true);
        isAlive = true;
    }
    void Update()
    {
        GetMovement();
        Attack();
    }
    void FixedUpdate()
    {
        Move();
        Rotate();
    }


    //------------------------------------------------------------------------------------

    //Funciones de control de ataque para el singlePlayer, coge el Input del teclado y del mando
    private void Attack()
    {
        if ((Input.GetButtonDown("Fire1") || Input.GetAxis("Fire1") > 0) && !IsShooting)
        {
            IsShooting = true;
            Shoot(0);
            Invoke("DisableShooting", shootCD);
        }
        else if ((Input.GetButtonDown("Fire2") || Input.GetAxis("Fire2") > 0) && !IsShooting)
        {
            IsShooting = true;
            Shoot(1);
            Invoke("DisableShooting", shootCD);
        }
        else if (Input.GetButtonDown("Jump") && !IsShooting && goBack.Sprt.enabled)
        {
            IsShooting = true;
            Rewind();
            Invoke("DisableShooting", shootCD);
        }
    }

    //Recoge los valores de los axis de movimiento del player
    private void GetMovement()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
    }

    //Mueve al player y modifica la variable de movimiento del animator
    private void Move()
    {
        Bofias.MoveCharacter(rb2d, h, v, speed);
        animator.SetFloat("speed", Mathf.Max(Mathf.Abs(h), Mathf.Abs(v)));
    }

    //Rota al player dependiendo de los controles actuales del player usa el input del joystick o del ratón
    private void Rotate()
    {
        if (GameManager.Instance.playerController[0] == Bofias.UsingInput.KEYBOARD)
            rb2d.transform.rotation = Bofias.RotateCharacter(rb2d.transform, Bofias.GetMouseWorldPosition(Input.mousePosition), false);
        else
            rb2d.transform.rotation = Bofias.RotateCharacter(rb2d.transform, Input.GetAxis("Xaxis"));
    }

    
    private void Shoot(int index)
    {
        Vector3 vectorToTarget = Vector3.zero;

        //En caso de estar usando teclado y ratón dispara hacia el ratón
        if (GameManager.Instance.playerController[0] == Bofias.UsingInput.KEYBOARD)
            vectorToTarget = Bofias.GetMouseWorldPosition(Input.mousePosition) - transform.position;
        else
        {
            //En caso contrario calcula la rotación del joystick para disparar en dicha dirección
            float haxis = Input.GetAxis("Xaxis");
            float vaxis = Input.GetAxis("Yaxis");
            if (haxis != 0 || vaxis != 0)
                vectorToTarget = new Vector3(transform.position.x + haxis, transform.position.y + vaxis) - transform.position;
        }

        //Si el joystick no se esta moviendo se dispara en la misma dirección que el ultimo proyectil
        if (vectorToTarget == Vector3.zero)
            Instantiate(spells[index], projectileSpawn.position, projectileSpawn.rotation);
        else
        {
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            projectileSpawn.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Instantiate(spells[index], projectileSpawn.position, projectileSpawn.rotation);
        }
    }
    private void Rewind()
    {
        //Ejecuta la mecánica de "rewind", modifica la posición del player y llama a la función del HUD que controla el sprite
        GameObject.FindGameObjectWithTag("Respawn").GetComponent<HUDController>().UsedCD(0);
        rb2d.transform.SetPositionAndRotation(goBack.positions[0].position, goBack.positions[0].rotation);
        goBack.CastTrail(rewindCD);
    }
    private void DisableShooting()
    {
        isShooting = false;
    }
}
