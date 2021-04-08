using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartContainersController : MonoBehaviour
{
    public Sprite[] containerStatus;

    private Image[] containers;

    public Image[] Containers { get => containers; }

    //Rellena todos los corazones al iniciar el juego
    void Start()
    {
        containers = GetComponentsInChildren<Image>();
        foreach (Image img in containers)
        {
            img.sprite = containerStatus[2];
        }
    }

    //Función que comprueba la vida del player y le establece los corazones en base a su vida restante y su vida maxima, 
    //de este modo se podrían añadir corazones o modificar la vida del player y seguiría funcionando igual en este caso
    public void UpdatePlayerHealth(float health, float maxHealth)
    {
        //Uso la variable index para llevar un recuento de las iteraciones y hacer el calculo de la vida respecto a los corazones
        int index = 1;

        //Esta variable es para comprobar si hay que pintar corazones, 
        //en caso de que un corazón este vacio los siguientes quedan vacios automáticamente
        bool paintHeart = true;

        foreach (Image img in containers)
        {
            if (health > 0 && paintHeart)
                if (health >= (maxHealth/containers.Length) * index)
                {
                    img.sprite = containerStatus[2];
                    if (health == (maxHealth / containers.Length) * index)
                        paintHeart = false;
                }
                else
                {
                    img.sprite = containerStatus[1];
                    paintHeart = false;
                }
            else
            {
                img.sprite = containerStatus[0];
            }
            index++;
        }
    }
}