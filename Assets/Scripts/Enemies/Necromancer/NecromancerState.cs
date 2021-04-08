using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NecromancerState
{
    public float startWalkingAgain;
    protected NecromancerController enemy;

    public abstract NecromancerState GetNextState();
    public abstract void Move();
    public abstract void Attack();

    //Función que rota hacia la posición del player
    public virtual void Rotate()
    {
        enemy.transform.rotation = Bofias.RotateCharacter(enemy.transform, enemy.GetClosestTarget(), false);
    }
}
