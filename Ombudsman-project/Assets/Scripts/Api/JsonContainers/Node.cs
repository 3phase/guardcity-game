﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public int id;
    public string dialog_file_path;
    public List<Node> options;
}