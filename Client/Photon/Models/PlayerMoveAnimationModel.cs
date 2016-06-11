using UnityEngine;
using System.Collections;
using System;

public class PlayerMoveAnimationModel
{
    public bool IsMove { get; set; }
    //public DateTime Time { get; set; }

    public string time;

    public void SetTime(DateTime dateTime)
    {
        time = dateTime.ToString("yyyyMMddHHmmssffff");  //年月日小时分钟秒毫秒
    }

    public DateTime GetTime()
    {
        DateTime dateTime = DateTime.ParseExact(time, "yyyyMMddHHmmssffff", System.Globalization.CultureInfo.CurrentCulture);
        return dateTime;
    }
}
