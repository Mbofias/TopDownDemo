using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public int maxRewardsOnBoxes;
    public GameObject reward;
    [HideInInspector] public static int boxesWithReward;
    private bool hasReward; 
    private Animator anim;

    private void Awake()
    { 
        //Elegimos aleatoriamente las cajas que contienen monedas en el awake
        if (Random.Range(0, 2) > 0 && boxesWithReward < maxRewardsOnBoxes) 
        {
            hasReward = true;
            boxesWithReward++;
        }
    }

    private void Start()
    {
        //En el remoto caso que no haya suficientes monedas aseguramos en el start que se generen las monedas necesarias
        if (boxesWithReward < maxRewardsOnBoxes)
        {
            hasReward = true;
            boxesWithReward++;
        }
        anim = GetComponent<Animator>();
        anim.enabled = false;
    }

    //Función para Romper la caja y disparar la animación
    public void Break()
    {
        Invoke("Drop", .33f);
        anim.enabled = true;
    }

    //Si la caja contenia una moneda la instanciamos, si no directamente la destruimos
    private void Drop()
    {
        if (hasReward)
            Instantiate(reward, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
