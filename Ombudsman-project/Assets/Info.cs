using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info : MonoBehaviour
{
    public Node node { get; set; }

    public void NextNode() {
        GameObject.Find("MissionPanel").GetComponent<MissionController>().loadNode(node); 
    }
}
