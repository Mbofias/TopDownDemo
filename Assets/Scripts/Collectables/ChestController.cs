using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public GameObject reward;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    //Si el player se acerca al cofre con una llave plateada la gasta para abrir el mismo.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(GameManager.Instance.playerSilverKeys > 0)
            {
                GameManager.Instance.playerSilverKeys--;
                //En caso de quedarse sin llaves plateadas desactivamos la llave plateada del hud
                if (GameManager.Instance.playerSilverKeys <= 0)
                    GameObject.FindGameObjectWithTag("Respawn").GetComponent<HUDController>().silverKey.enabled=false;
                anim.SetTrigger("OnOpen");
                Invoke("Drop", .33f);
            }
        }
    }

    //Instancia una moneda y se destruye el cofre
    private void Drop()
    {
        Instantiate(reward, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
