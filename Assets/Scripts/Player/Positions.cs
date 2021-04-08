using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Positions
{
    //Este struct guarda la posición y rotación para la mecánica de rewind
    public Positions(Transform values)
    {
        position = values.position;
        rotation = values.rotation;
    }
    public Vector3 position;
    public Quaternion rotation;
}
