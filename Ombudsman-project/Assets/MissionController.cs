using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        options = new List<GameObject>();
        Debug.Log(planet.name);

        Node node = ApiController.GetNode(61);

        missionProblem = GameObject.Find("Problem").GetComponent<Text>();
        missionProblem.text = node.dialog_file_path;

        Mission sampleMission = ApiController.GetMission(11, 1);

        Debug.Log(sampleMission.current_node_id);
        Debug.Log(sampleMission.alien);

        for (int i = 0; i < node.options.Count; i++)
        {
            Debug.Log(node.options[i].dialog_file_path);
            var option = Instantiate(optionPrefab, missionProblem.transform);
            option.transform.position = new Vector3(missionProblem.transform.position.x, missionProblem.transform.position.y - (i + 1)*75, missionProblem.transform.position.z);
            option.GetComponentInChildren<Text>().text = node.options[i].dialog_file_path;
            options.Add(option);
        }
    }

    void Update()
    {
        
    }
}
