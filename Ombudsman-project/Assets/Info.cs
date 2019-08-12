using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info : MonoBehaviour
{
    public Node node;

    public void NextNode() {
        GameObject.Find("MissionPanel").GetComponent<MissionController>().LoadMission(node); 
    }
}
