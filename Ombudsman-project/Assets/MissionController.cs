using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//WORK IN PROGRESS :D
public class MissionController : MonoBehaviour
{
    Mission mission;
    Node currentNode;
    public GameObject optionPrefab;
    private Text missionProblem;
    private List<GameObject> options;
    
    void Awake()
    {
        Planet planet = ApiController.GetPlanet(1);

        Node node = ApiController.GetNode(61);

        missionProblem = GameObject.Find("Problem").GetComponent<Text>();
        missionProblem.text = node.dialog_file_path;

        for (int i = 0; i < node.options.Count; i++)
        {
            var option = Instantiate(optionPrefab, missionProblem.transform);
            option.GetComponent<Text>().text = node.options[i].dialog_file_path;
            options.Add(option);
        }
    }

    void Update()
    {
        /*missionProblem.text = mission.nodes[currentNodeIndex].problem;

        for (int i = 0; i < options.Count; i++)
        {
            options[i].GetComponent<Text>().text = mission.nodes[currentNodeIndex].options[i]; 
        }*/
    }

    public void AddNewNode() {
        // Node node = ApiController.GetNode(0, 0);
        // mission.nodes.Add(node);
    }

    public void CompleteNode() {
        //change gains
        AddNewNode();
    }
}
