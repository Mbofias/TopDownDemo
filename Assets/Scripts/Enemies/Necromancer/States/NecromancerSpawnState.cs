using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerSpawnState : NecromancerState
{
    public NecromancerSpawnState(NecromancerController parent)
    {
        //Guarda una referencia al script del nigromante
        enemy = parent;
        Rotate();
    }

    //Invoca un esqueleto
    public override void Attack()
    {
        enemy.SpawnAvailable = false;
        enemy.CurrentSkeletons++;
        enemy.Spawn();
    }

    //Una vez ha invocado al esqueleto vuelve al estado de huir del player
    public override NecromancerState GetNextState()
    {
        if (!enemy.SpawnAvailable)
            return new NecromancerRunState(enemy);
        else
            return this;
    }

    //Se gira hacia el player al invocar un esqueleto
    public override void Move()
    {
        Bofias.MoveCharacter(enemy.RB2D, enemy.GetClosestTarget(), enemy.speed);
    }
}
