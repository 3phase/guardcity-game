using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//WORK IN PROGRESS :D
public class MissionController : MonoBehaviour
{
    Mission mission;
    public int currentNodeIndex = 0;
    public GameObject optionPrefab;
    private Text missionProblem;
    private List<GameObject> options;
    // Start is called before the first frame update
    void Awake()
    {
        Planet planet = ApiController.GetPlanet(1);
        Debug.Log(planet.name);
        // Node node = ApiController.GetNode(1);
        // mission.nodes.Add(node);

        // missionProblem = GameObject.Find("Problem").GetComponent<Text>();
        // Debug.Log(missionProblem);
        // Debug.Log("asdff");
        // for (int i = 0; i < node.options.Length; i++)
        // {
        //     var option = Instantiate(optionPrefab, missionProblem.transform);
        //     option.GetComponent<Text>().text = mission.nodes[currentNodeIndex].options[i];
        //     options.Add(option);
        // }
    }

    void Update()
    {
        missionProblem.text = mission.nodes[currentNodeIndex].problem;
        for (int i = 0; i < options.Count; i++)
        {
            options[i].GetComponent<Text>().text = mission.nodes[currentNodeIndex].options[i]; 
        }
    }

    public void AddNewNode() {
        // Node node = ApiController.GetNode(0, 0);
        // mission.nodes.Add(node);
    }

    public void CompleteNode() {
        //change gains
        AddNewNode();
        currentNodeIndex++;
    }
}
