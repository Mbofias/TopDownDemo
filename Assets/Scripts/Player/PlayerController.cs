using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Clase padre de todos los posibles personajes jugables, el personaje para jugar solo y los dos cooperativos
    public AudioClip damage;
    public float maxHealth = 100f, speed = 3f, rewindCD = 10f, shootCD = 1.5f;
    public GameObject[] spells;
    public Transform projectileSpawn;
    public RewindController goBack;

    protected Rigidbody2D rb2d;
    protected float health, h, v;
    protected Animator animator;
    protected bool isShooting, rewindOnCD, isAlive;
    protected HeartContainersController heartContainers;
    protected int peaksPressing;

    //Getters y Setters para las variables privadas/protected;
    public float Health { get => health; set => health = value; }
    public float H { get => h; set => h = value; }
    public float V { get => v; set => v = value; }
    public bool RewindOnCD { get => rewindOnCD; set => rewindOnCD = value; }
    public bool IsShooting { get => isShooting; set => isShooting = value; }
    public int PeaksPressing { get => peaksPressing; set => peaksPressing = value; }
    public Animator Animator { get => animator; set => animator = value; }
    public Rigidbody2D RB2D { get => rb2d; set => rb2d = value; }
    public bool IsAlive { get => isAlive; set => isAlive = value; }

    // Esta función se llama cada medio segundo, el player recibe daño en caso de estar sobre una trampa de pinchos activa;
    private void PeakDamage()
    {
        if (peaksPressing > 0)
        {
            TakeDamage(true);
        }
    }

    // Funcion llamada por enemigos y ataques enemigos para que el player reciba daño, quita exactamente 1 o 1/2 corazones;
    public virtual void TakeDamage(bool critical)
    {
        AudioSource.PlayClipAtPoint(damage, transform.position);

        //Si es un golpe critico el player ierde un corazón, si no es critico pierde medio corazón
        if (critical)
            health -= maxHealth / heartContainers.Containers.Length;
        else
            health -= maxHealth / (heartContainers.Containers.Length * 2);

        //Actualiza el HUD
        heartContainers.UpdatePlayerHealth(health, maxHealth);

        //Comprueba si el player esta muerto y tiene que saltar la pantalla de GameOver
        GameManager.GameOverTest();
        if (health <= 0)
            isAlive = false;
    }
}
