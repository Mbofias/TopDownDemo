using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    //Este objeto controla cuando activar el boss, ya que necesitaba un objeto que controlase cuando el player
    //iniciaba el enfrentamiento, he preferido tener el boss instanciado de entrada y desactivarlo y activarlo mediante este componente
    //en vez de instanciarlo durante la partida.
    private BossController boss;
    private GameObject[] peaks;
    public GameObject bossHealthBar;

    void Start()
    {
        //Desactivamos el boss al empezar el nivel
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossController>();
        peaks = GameObject.FindGameObjectsWithTag("Peak");

        boss.enabled = false;
    }

    //Cuando el player entra en la sala del boss se activa el boss y se suben los pinchos, 
    //también se convierten en colliders para evitar que el boss o los nigromantes salgan de la sala

    //Si están jugando dos personas se transportan a los jugadores a la sala del boss, para evitar que uno de ellos se quede fuera.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            boss.enabled = true;
            foreach(GameObject peak in peaks)
            {
                peak.GetComponent<PeakController>().Activate();
                peak.GetComponent<BoxCollider2D>().isTrigger = false;
            }
            if (GameManager.Instance.players > 1)
            {
                foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
                {
                    player.transform.position = collision.transform.position;
                }
            }
            bossHealthBar.SetActive(true);

            Destroy(gameObject);
        }
        //Una vez terminada su función este objeto se destruye
    }
}
