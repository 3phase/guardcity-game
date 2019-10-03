using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info : MonoBehaviour
{
    public Node node { get; set; }

    public void NextNode() {
        StartCoroutine(FindObjectOfType<MissionController>().LoadNode(node)); 
    }
}
