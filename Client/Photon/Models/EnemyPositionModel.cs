﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyPositionModel
{
    public List<EnemyPositionProperty> list = new List<EnemyPositionProperty>();
}

public class EnemyPositionProperty
{
    public string guid;
    public Vector3Object position;
    public Vector3Object eulerAngles;
}
