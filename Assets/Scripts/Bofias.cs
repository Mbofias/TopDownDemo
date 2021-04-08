using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bofias
{

    // Estos Enums son usados por el GameManager y el Player uno, y el otro por los hechizos, estan colocados aquí porque estaba previsto usar el elemento con los enemigos también.
    public enum UsingInput
    {
        KEYBOARD, CONTROLLER
    }
    public enum SpellElement
    {
        FIRE, ICE
    }

    //----------------------------------------------


    /// <summary>
    /// Get the position of the mouse.
    /// </summary>
    /// <param name="screenPos">Mouse position on the screen</param>
    /// <returns>Returns a Vector3 on the mouse position</returns>
    public static Vector3 GetMouseWorldPosition(Vector3 screenPos)
    {
        screenPos.z = 0f;
        return Camera.main.ScreenToWorldPoint(screenPos);
    }

    /// <summary>
    /// Move the rigidbody using velocity.
    /// </summary>
    /// <param name="rb2d">The Rigidbody you want to move</param>    
    ///<param name="h">X axis value</param>
    ///<param name="v">Y axis value</param>
    ///<param name="speed">speed value</param>
    /// <returns>Returns a rigidbody with the new speed</returns>
    public static Rigidbody2D MoveCharacter(Rigidbody2D rb2d, float h, float v, float speed)
    {
        rb2d.velocity = new Vector3(h * speed, v * speed, 0);
        return rb2d;
    }

    /// <summary>
    /// Move the rigidbody towards a vector.
    /// </summary>
    /// <param name="rb2d">The Rigidbody you want to move</param>    
    ///<param name="target">the vector you are moving towards</param>
    ///<param name="speed">speed value</param>
    /// <returns>Returns a rigidbody with the new position</returns>
    public static Rigidbody2D MoveCharacter(Rigidbody2D rb2d, Vector3 target, float speed)
    {
        Vector3 newPos = Vector3.MoveTowards(rb2d.transform.position, target, speed * Time.deltaTime);
        rb2d.transform.position = newPos;
        return rb2d;
    }

    /// <summary>
    /// Rotates the transform to look a certain point.
    /// </summary>
    /// <param name="character">The Transform you want to rotate</param>    
    ///<param name="lookingPoint">the vector you are looking at</param>
    ///<param name="lookAway">Want to look on the oposite direction?</param>
    /// <returns>Returns a quaternion that looks towards the looking point.</returns>
    public static Quaternion RotateCharacter(Transform character, Vector3 lookingPoint, bool lookAway)
    {
        if (lookAway)
            if (lookingPoint.x >= character.position.x)
                return new Quaternion(0, 180, 0, 0);
            else
                return new Quaternion(0, 0, 0, 0);
        else
            if (lookingPoint.x >= character.position.x)
                return new Quaternion(0, 0, 0, 0);
            else
                return new Quaternion(0, 180, 0, 0);
    }

    /// <summary>
    /// Rotates the transform to look at the same direction a joystick is pointig.
    /// </summary>
    /// <param name="character">The Transform you want to rotate</param>    
    ///<param name="joystick">The axis on the joystick</param>
    /// <returns>Returns a quaternion that looks towards the joystick direction.</returns>
    public static Quaternion RotateCharacter(Transform character, float joystick)
    {
        if (joystick > .05)
            return new Quaternion(0, 0, 0, 0);
        else if (joystick < -.05)
            return new Quaternion(0, 180, 0, 0);
        else
            return character.rotation;
    }

    // Las siguientes funciones aplican un debuff que realentiza al recibir un ataque de hielo


}
