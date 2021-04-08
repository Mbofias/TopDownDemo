using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeathState : BossState
{
    // Estado para añadir funcionalidades extras al morir el boss, acabar al final si hay tiempo.
    public BossDeathState()
    {
        //Modifico la velocidad, guardo un acceso al script del boss y ejecuto la función de muerte del boss
        speedMultiplier = 0f;
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossController>();
        SetMultiplier();
        boss.Die();
    }
    public override BossState GetNextState()
    {
        return this;
    }
}
