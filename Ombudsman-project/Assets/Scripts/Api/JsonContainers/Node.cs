﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public int id { get; set; }
    public string dialog { get; set; }

    public string speaker { get; set; }
    public int unlocking_trust { get; set; }
    public List<Node> options { get; set; }
    public string option_player_dialog { get; set; }
    public Gains gains = new Gains();
}


