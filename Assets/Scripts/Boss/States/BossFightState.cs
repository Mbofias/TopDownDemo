using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightState : BossState
{
    //Estado que controla la primera fase del boss.

    public BossFightState()
    {
        //Seteamos todos los valores necesarios en el constructor del estado

        //Hay valores hardcodeados pero porque no tenia muy claro donde crear las 
        //variables publicas de cada estado para modificarlas del editor
        speedMultiplier = 1f;
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossController>();
        SetMultiplier();
        boss.AttackCD = 3f; 
    }

    //Cuando el boss llega a media vida realiza el primer cambio de fase
    public override BossState GetNextState()
    {
        if (boss.Health >= boss.maxHealth * .5f)
            return this;
        else
        {
            boss.speed = ResetMultiplier();
            return new BossSpecialState();
        }
    }
}
