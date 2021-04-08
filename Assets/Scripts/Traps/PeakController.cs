using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeakController : MonoBehaviour
{
    public Sprite[] states;

    private BoxCollider2D coll;
    private Animator anim;
    private static bool activated;
    private SpriteRenderer sprt;

    public static bool Activated { get => activated; }
    public BoxCollider2D Coll { get => coll; set => coll = value; }
    //Se activan los pinchos al iniciar el nivel
    void Start()
    {
        activated = true;
        anim = GetComponent<Animator>();
        anim.enabled = false;
        sprt = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }
    //Si los pinchos estan activados y un jugador se sube sobre ellos 
    //se suman a al variable numerica del player que indica cuantos pinchos está pisando
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && activated)
        {
            collision.gameObject.GetComponent<PlayerController>().PeaksPressing++;
        }
    }
    // Cuando el player se sale del trigger de un pincho se le resta 1 a la misma variable
    //así nos aseguramos de evitar que el jugador deje de recibir daño de los pinchos si se mueve de unos pinchos a otros
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && activated)
        {
            collision.gameObject.GetComponent<PlayerController>().PeaksPressing--;
        }
    }

    //Funciones de activación y desactivación de los pinchos
    public void Activate()
    {
        sprt.sprite = states[1];
        activated = true;
    }
    public void Deactivate()
    {
        sprt.sprite = states[0];
        activated = false;
    }
}
