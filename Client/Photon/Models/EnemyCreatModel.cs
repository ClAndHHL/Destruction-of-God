using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyCreatModel //传递敌人创建时候的数据
{
    public List<EnemyProperty> list = new List<EnemyProperty>();
}

public class EnemyProperty  //存储创建一个敌人所需要的属性
{
    public string guid;
    public string perfabName;
    public int targetRoleId;
    public Vector3Object position;
}
