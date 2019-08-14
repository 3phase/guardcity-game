using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public int id { get; set; }
    public string dialog { get; set; }
    public string speaker { get; set; }
    public Pivot pivot { get; set; }
    public List<Node> options { get; set; }
    public Gains gains { get; set; }
}
