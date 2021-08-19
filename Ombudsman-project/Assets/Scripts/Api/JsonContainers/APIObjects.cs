using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO: Make the API return the objects needed.
[System.Serializable]
public class APIPlanetList
{
    public List<Planet> planets;
}


[System.Serializable]
public class APIBaseNode
{
    public int id;
    public string dialog;
    public string speaker;
}

[System.Serializable]
public class APIGainsNode : APIBaseNode
{
    public Gains gains;
    public string option_dialog;
    public int unlocking_trust;
}

[System.Serializable]
public class APIMissionNode
{
    public APIBaseNode current_node;
    public List<APINodeOption> options;
}


[System.Serializable]
public class APINodeOption
{
    public APIGainsNode node;
}

