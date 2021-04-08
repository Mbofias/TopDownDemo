using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindController : MonoBehaviour
{
    public List<Positions> positions;
    private Transform player;
    private SpriteRenderer sprt, playerSprt;

    public SpriteRenderer Sprt { get => sprt; set => sprt = value; }

    //Script que controla la mecánica de "rewind", esta mecánica guarda posiciones y rotaciones previas del jugador
    //para poder volver a la posición de 2.5 segundos antes
    void Start()
    {
        positions = new List<Positions>();
        sprt = GetComponent<SpriteRenderer>();
        player = gameObject.transform.parent;
        playerSprt = player.gameObject.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        //En caso de tener una lista con menos de 75 posiciones se añade una posición extra a la lista
        if (positions.Count < 75)
            positions.Add(new Positions(player));
        else
        {
            //Si ya hay 75 posiciones en la lista se borra la primera posicion guardada y se añade una posición nueva
            positions.RemoveAt(0);
            positions.Add(new Positions(player));
        }

        //Después se establece la posición de la sombra en la posición de la primera entrada de la lista
        transform.SetPositionAndRotation(positions[0].position, positions[0].rotation);
        sprt.sprite = playerSprt.sprite;
    }

    public void CastTrail(float cd)
    {
        //Cuando se usa la habilidad del "rewind" se reinicia la lista y se hace un Invoke para volver a dejar la habilidad disponible
        //después de un tiempo de CD
        positions = new List<Positions>();
        sprt.enabled = false;
        Invoke("TrialAvailable", cd);
    }

    private void TrialAvailable()
    {
        sprt.enabled = true;
    }
}