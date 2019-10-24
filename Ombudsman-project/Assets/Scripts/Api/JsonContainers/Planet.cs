using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class AlienPosition
{
    public float xCoord;
    public float yCoord;
}

[System.Serializable]
public class Planet
{
    public int id;
    public string name;
    public string image_filename;
    public string background_image;
    // public int unlocking_popularity;
    public List<Alien> aliens = new List<Alien>();
    public List<AlienPosition> alien_coordinates = new List<AlienPosition>();
}
