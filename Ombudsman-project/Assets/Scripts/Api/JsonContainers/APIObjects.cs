using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO: Make the API return the objects needed.

[System.Serializable]
public class APIBaseNode
{
    public int id;
    public string dialog;
    public string speaker;
}

[System.Serializable]
public class APIPivotNode : APIBaseNode
{
    Pivot pivot;
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
    public APIBaseNode node;
    public Gains gains;
}

