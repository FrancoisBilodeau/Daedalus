using UnityEngine;
using System.Collections;
using System;

public class TimeRecorded
{
    string playerName;
    TimeSpan time;

    public TimeRecorded(string playerName, TimeSpan time)
    {
        this.playerName = playerName;
        this.time = time;
    }

    public string getPlayerName()
    {
        return playerName;
    }

    public TimeSpan getTime()
    {
        return time;
    }
}
