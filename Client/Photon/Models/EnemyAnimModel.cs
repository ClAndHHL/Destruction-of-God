using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAnimModel
{
    public List<EnemyAnimProperty> list = new List<EnemyAnimProperty>();
}

public class EnemyAnimProperty
{
    public string guid;
    public bool isIdle;
    public bool isWalk;
    public bool isAttack;
    public bool isTakeDamage;
    public bool isDie;
}
