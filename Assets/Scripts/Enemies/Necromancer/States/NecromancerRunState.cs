using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerRunState : NecromancerState
{
    public NecromancerRunState(NecromancerController parent)
    {        
        //Guarda una referencia al script del nigromante
        enemy = parent;
    }

    public override void Attack()
    {
        //This state doesn't attack
    }

    //Comprueba si puede invocar algun esqueleto y cuandos esqueletos tiene invocados para cambiar al estado de Invocación
    public override NecromancerState GetNextState()
    {
        if (enemy.SpawnAvailable && enemy.CurrentSkeletons < enemy.maxSkeletons)
            return new NecromancerSpawnState(enemy);
        else
            return this;
    }

    //Se mueve huyendo del player mas cercano
    public override void Move()
    {
        Bofias.MoveCharacter(enemy.RB2D, enemy.GetClosestTarget(), -enemy.speed);
    }

    //Rota al nigromando mirando en dirección contraria al player
    public override void Rotate()
    {
        enemy.transform.rotation = Bofias.RotateCharacter(enemy.transform, enemy.GetClosestTarget(), true);
    }
}
