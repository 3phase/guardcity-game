using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Planet
{
    public int id;
    public string name;
    public int level;
    public string image_filename;
    public int unlocking_popularity;
    public List<Alien> aliens;
}
