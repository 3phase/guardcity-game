using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public int id;
    public string dialog_file_path;
    public Pivot pivot;
    public List<Node> options;
    public Gains gains;

}
