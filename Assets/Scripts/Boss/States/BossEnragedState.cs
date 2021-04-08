using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnragedState : BossState
{
    // Estado que controla la ultima fase del boss;

    public BossEnragedState()
    {
        //Seteamos todos los valores necesarios en el constructor del estado

        //Hay valores hardcodeados pero porque no tenia muy claro donde crear las 
        //variables publicas de cada estado para modificarlas desde el editor
        speedMultiplier = 2.5f;
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossController>();
        boss.Animator.SetTrigger("EnterEnrage");
        SetMultiplier();

        //Modificamos el tiempo entre ataques del boss para que se ejecute acorde a la fase actual;
        boss.AttackCD = 1.8f;
        boss.CancelInvoke("Attack");
        boss.InvokeRepeating("Attack", 0, boss.AttackCD);
    }

    //Cambia a la fase de muerte al llegar a 0 de vida;
    public override BossState GetNextState()
    {
        if (boss.Health > 0)
            return this;
        else
            return new BossDeathState();
    }

    //Se modifica la función de recibir daño del boss en esta fase
    public override void TakeDamage(Bofias.SpellElement element)
    {
        AudioSource.PlayClipAtPoint(boss.damage, boss.transform.position);
        //Suena el audio cuando recibe daño

        switch (element)
        {
            case Bofias.SpellElement.FIRE:
                boss.Health -= 5;
                break;
            case Bofias.SpellElement.ICE:
                boss.Health -= 35;
                boss.Froze();
                break;
            default: break;
        }

        //Se actualiza la barra de vida del boss en el hud
        boss.healBar.localScale = new Vector3(boss.Health / boss.maxHealth, 1, 1);
    }
}
