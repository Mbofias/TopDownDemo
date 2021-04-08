using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossState
{
    public float startWalkingAgain;
    protected float speedMultiplier;
    protected BossController boss;
    protected bool isAttacking = false;

    public abstract BossState GetNextState();

    //El movimiento por defecto del boss, este solo cambia en la segunda fase
    public virtual void Move()
    {
        if (!isAttacking)
        {
            Bofias.MoveCharacter(boss.RB2D, boss.GetClosestTarget(), boss.speed);
            boss.Animator.SetFloat("Speed", 1);
        }
        else
            boss.Animator.SetFloat("Speed", 0);
    }

    // El ataque del boss durante la primera y la ultima fase, 8 proyectiles en direcciones distintas cada 45 grados
    public virtual void Attack()
    {
        isAttacking = true;
        int totalProjectiles = 8;

        //Rota 360 grados en el eje z, disparando un proyectil cada 45 grados;
        for (float projectiles = 0; projectiles < totalProjectiles; projectiles++)
        {
            Vector3 tmp = new Vector3(0, 0, (360 / totalProjectiles) * projectiles);
            boss.projectileSpawn.rotation = Quaternion.Euler(tmp);
            boss.Shoot();
            boss.projectile.transform.rotation.Set(0, 0, 0, 0);
        }
    }

    //Función que se llama por otros objetos para inflingir daño al boss, es virtual porque hay una fase del boss que recibe daño diferente de las demás.
    public virtual void TakeDamage(Bofias.SpellElement element)
    {
        AudioSource.PlayClipAtPoint(boss.damage, boss.transform.position);
        //Sonido del efecto de recibir daño de los enemigos

        switch (element)
        {
            case Bofias.SpellElement.FIRE:
                boss.Health -= 25;
                break;
            case Bofias.SpellElement.ICE:
                boss.Health -= 10;
                boss.Froze();
                break;
            default: break;
        }

        //Actualiza la barra de vida del boss en el hud
        boss.healBar.localScale = new Vector3(boss.Health / boss.maxHealth, 1, 1);
    }

    //Función para rotar al boss, es virtual porque una de las fases del boss este gira huyendo del jugador.
    public virtual void Rotate()
    {
        boss.transform.rotation = Bofias.RotateCharacter(boss.transform, boss.GetClosestTarget(), false);
    }

    //Función que añade un pequeño delay al movimiento del player para que se quede parado un momento cuando ataca
    public virtual void endAttacking()
    {
        if (startWalkingAgain > 0)
            startWalkingAgain -= Time.deltaTime;
        else
        {
            startWalkingAgain = 1f;
            isAttacking = false;
        }
    }

    //Modifica la velocidad del boss dependiendo de la fase, antes de cambiar de fase se vuelve a colocar en su velocidad original;
    protected virtual float SetMultiplier()
    {
        return boss.speed * speedMultiplier;
    }
    protected virtual float ResetMultiplier()
    {
        return boss.speed / speedMultiplier;
    }
}
