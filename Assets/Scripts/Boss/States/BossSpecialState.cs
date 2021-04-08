using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpecialState : BossState
{
    //Estado que controla la fase intermedia del boss

    public BossSpecialState()
    {
        //Seteamos todos los valores necesarios en el constructor del estado

        //Hay valores hardcodeados pero porque no tenia muy claro donde crear las 
        //variables publicas de cada estado para modificarlas del editor 
        speedMultiplier = .5f;
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossController>();
        SetMultiplier();
        boss.AttackCD = 6f;
        boss.CancelInvoke("Attack");
        boss.InvokeRepeating("Attack", boss.AttackCD, boss.AttackCD);
    }

    public override BossState GetNextState()
    {
        //Si la vida del boss baja del 35% o ha invocado una cantidad establecida de nigromantes, este cambia de fase
        if (boss.Health >= boss.maxHealth * .35f && boss.Necromancers < boss.maxNecromancerSpawns)
            return this;
        else
            return new BossEnragedState();
    }

    public override void Move()
    {
        // El movimiento del boss en esta fase, vigila no acercarse demasiado a las paredes, para no invocar nigromantes fuera del mapa.
        if (!isAttacking)
        {  
            RaycastHit2D left = Physics2D.Raycast(boss.transform.position, Vector3.left, 1.5f, LayerMask.GetMask("Walls"));
            RaycastHit2D right = Physics2D.Raycast(boss.transform.position, Vector3.right, 1.5f, LayerMask.GetMask("Walls"));

           if ((right && boss.GetClosestTarget().x < boss.transform.position.x) || (left && boss.GetClosestTarget().x > boss.transform.position.x))
                Bofias.MoveCharacter(boss.RB2D, new Vector3(boss.transform.position.x, boss.GetClosestTarget().y, 0), -boss.speed);
            else
                Bofias.MoveCharacter(boss.RB2D, boss.GetClosestTarget(), -boss.speed);

            boss.Animator.SetFloat("Speed", 1);
        }
        else
            boss.Animator.SetFloat("Speed", 0);
    }

    public override void Attack()
    {
        // El ataque del boss en esta fase, invoca nigromantes, 2 en singleplayer y 4 en el modo cooperativo.
        isAttacking = true;
        if (GameManager.Instance.players == 1)
        {
            //SINGLE PLAYER
            for (int spawn = 0; spawn < boss.necromancerSpawners.Length / 2; spawn++)
            {
                boss.Spawn(spawn);
            }
        }
        else
        {
            //COOP
            for (int spawn = 0; spawn < boss.necromancerSpawners.Length; spawn++)
            {
                boss.Spawn(spawn);
            }
        }
    }

    //En esta fase el boss rota en dirección contraria al player, ya que huye del mismo
    public override void Rotate()
    {
        boss.transform.rotation = Bofias.RotateCharacter(boss.transform, -boss.GetClosestTarget(), true);
    }

}
