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

    public Level(List<Location> loc)
    {
        PlayerLocation.Add(loc[0]);
        for(int i = 1; i < loc.Count; i++)
        {
            OpponentLocation.Add(loc[i]);
        }
    }

    public void print()
    {
        foreach (Location l in OpponentLocation)
        {
            Debug.Log($"Opponent x : {l.x}, y : {l.y}");
        }
        foreach (Location l in PlayerLocation)
        {
            Debug.Log($"Player x : {l.x}, y : {l.y}");
        }
    }
}
