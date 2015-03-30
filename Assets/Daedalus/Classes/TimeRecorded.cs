using System;

public class TimeRecorded
{

    public TimeRecorded(string playerName, TimeSpan time)
    {
        this.PlayerName = playerName;
        this.Time = time;
    }

    public TimeSpan Time { get; set; }

    public string PlayerName { get; set; }
}
