using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[Serializable]
public class Level
{
    public List<Location> OpponentLocation = new List<Location>();
    public List<Location> PlayerLocation = new List<Location>();
    public List<Location> WallLocation = new List<Location>();

    System.Random random = new System.Random();

    public Level()
    {
        for (int i = -3; i < 3; i++)
        {
            OpponentLocation.Add(new Location(i, 3));
        }
        for (int i = -2; i < 3; i += 2)
        {
            PlayerLocation.Add(new Location(i, -3));
        }
    }

    public Level(List<Location> loc, int WallCount)
    {
        PlayerLocation.Add(loc[0]);
        for(int i = 1; i < loc.Count - WallCount; i++)
        {
            OpponentLocation.Add(loc[i]);
        }
        for(int i = loc.Count - WallCount; i < loc.Count; i++)
        {
            WallLocation.Add(loc[i]);
        }
    }
}
