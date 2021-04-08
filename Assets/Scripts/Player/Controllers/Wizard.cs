using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : PlayerController
{
    private Camera cam;

    void Start()
    {
        //Establece todas las variables del player en el start
        isAlive = true;
        health = maxHealth;
        heartContainers = GameObject.FindGameObjectWithTag("WizardHealth").GetComponent<HeartContainersController>();
        rb2d = GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        cam = Camera.main;
        InvokeRepeating("PeakDamage", .5f, .5f);
        GameManager.SetHealth(gameObject, true);
    }

    //Las acciones del player se actualizan solo si esta vivo
    void Update()
    {
        if (isAlive)
        {
            GetMovement();
            Attack();
        }
    }
    void FixedUpdate()
    {
        if (isAlive)
        {
            Move();
            Rotate();
        }
    }

    //--------------------------------------------------------

    //Funciones de control de ataque para el player 1, coge el Input del teclado y ratón únicamente
    private void Attack()
    {
        if (Input.GetButtonDown("WizardAttack") && !IsShooting)
        {
            IsShooting = true;
            Shoot(0);
            Invoke("DisableShooting", shootCD);
        }
        else if (Input.GetButtonDown("WizardRewind") && !IsShooting && goBack.Sprt.enabled)
        {
            IsShooting = true;
            Rewind();
            Invoke("DisableShooting", shootCD);
        }
    }

    //Coge los valores del axis que controla el movimiento del player 1
    private void GetMovement()
    {
        h = Input.GetAxis("WizardHorizontal");
        v = Input.GetAxis("WizardVertical");
    }

    //Controla el movimiento para evitar que se salga de los limites de la pantalla
    //frenando al player si se sale de la cámara
    private void Move()
    {
        if ((cam.transform.position.x + cam.orthographicSize * cam.aspect > transform.position.x) && (cam.transform.position.x - cam.orthographicSize * cam.aspect < transform.position.x) && (cam.transform.position.y + cam.orthographicSize > transform.position.y) && (cam.transform.position.y - cam.orthographicSize < transform.position.y))
            Bofias.MoveCharacter(rb2d, h, v, speed);
        else if (((cam.transform.position.x + cam.orthographicSize * cam.aspect <= transform.position.x) && h > 0) || ((cam.transform.position.x - cam.orthographicSize * cam.aspect >= transform.position.x) && h < 0))
            Bofias.MoveCharacter(rb2d, 0, v, speed);
        else if (((cam.transform.position.y + cam.orthographicSize <= transform.position.y) && v > 0) || ((cam.transform.position.y - cam.orthographicSize <= transform.position.y) && v < 0))
            Bofias.MoveCharacter(rb2d, h, 0, speed);
        animator.SetFloat("speed", Mathf.Max(Mathf.Abs(h), Mathf.Abs(v)));
    }

    //Función de rotación del player 1, usando el ratón
    private void Rotate()
    {
        rb2d.transform.rotation = Bofias.RotateCharacter(rb2d.transform, Bofias.GetMouseWorldPosition(Input.mousePosition), false);
    }

    //Dispara, funciona igual que la función del SinglePlayer, pero solo detecta el control del ratón
    private void Shoot(int index)
    {
        Vector3 vectorToTarget = Bofias.GetMouseWorldPosition(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        projectileSpawn.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Instantiate(spells[index], projectileSpawn.position, projectileSpawn.rotation);
    }

    //Ejecuta la mecánica de "rewind", modifica la posición del player y llama a la función del HUD que controla el sprite
    private void Rewind()
    {
        GameObject.FindGameObjectWithTag("Respawn").GetComponent<HUDController>().UsedCD(0);
        rb2d.transform.SetPositionAndRotation(goBack.positions[0].position, goBack.positions[0].rotation);
        goBack.CastTrail(rewindCD);
    }
    private void DisableShooting()
    {
        isShooting = false;
    }
}
