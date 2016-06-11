using UnityEngine;
using System.Collections;

public class Vector3Object  //专门用来传递Vector3
{
    public double x { get; set; }
    public double y { get; set; }
    public double z { get; set; }

    public Vector3Object()
    {

    }

    public Vector3Object(Vector3 temp)
    {
        x = temp.x;
        y = temp.y;
        z = temp.z;
    }

    public Vector3 ToVector3()
    {
        Vector3 temp = Vector3.zero;
        temp.x = (float)x;
        temp.y = (float)y;
        temp.z = (float)z;
        return temp;
    }
}
