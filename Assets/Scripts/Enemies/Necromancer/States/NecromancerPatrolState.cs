using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerPatrolState : NecromancerState
{
    public NecromancerPatrolState(NecromancerController parent)
    {
        //Guarda una referencia al script del nigromante
        enemy = parent;
        enemy.ResetPatrol();
    }

    //En este caso la función de ataque actualiza el punto de patrulla del nigromante
    public override void Attack()
    {
        if (Vector3.Distance(enemy.transform.position, enemy.PatrolPoint) < .1 || enemy.PatrolPoint == null)
        {
            float angle = Random.Range(0f, 359f);

            Vector3 towardsPatrolPoint = Quaternion.AngleAxis(angle, Vector3.forward) * (-enemy.transform.up);
            enemy.PatrolPoint = enemy.transform.position + towardsPatrolPoint * enemy.perceptionRadius;
            enemy.Invoke("ResetPatrol", 2f);
        }
    }

    //Comprueba si detecta al player para cambiar el estado
    public override NecromancerState GetNextState()
    {
        if ((enemy.GetClosestTarget() - enemy.transform.position).sqrMagnitude < enemy.perceptionRadius * enemy.perceptionRadius)
            return new NecromancerSpawnState(enemy);
        else
            return this;
    }

    //Mueve al nigromante hacia el punto de patrulla
    public override void Move()
    {
        Bofias.MoveCharacter(enemy.RB2D, enemy.PatrolPoint, enemy.speed);
    }
}