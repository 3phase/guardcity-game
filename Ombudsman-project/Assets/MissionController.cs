using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionController : MonoBehaviour
{
    Mission mission;
    public GameObject optionPrefab;
    private Text missionProblem;
    private List<GameObject> options = new List<GameObject>();

    public async void StartMission()
    {
        gameObject.GetComponentInParent<Canvas>().enabled = false;

        Planet planet = await ApiController.GetPlanet(1);

        Node node = await ApiController.GetNode(41);

        loadNode(node);
    }

    public void EndMission(Node node) {
        missionProblem = GameObject.Find("Problem").GetComponent<Text>();
        missionProblem.text = node.dialog;

        var option = Instantiate(optionPrefab, missionProblem.transform);
        option.name = "End Mission";
        option.transform.position = new Vector3(missionProblem.transform.position.x,
                missionProblem.transform.position.y - 100, missionProblem.transform.position.z);
        option.GetComponentInChildren<Text>().text = "Край на мисията";
        option.GetComponent<Button>().onClick.AddListener(delegate { DestroyCanvas(); });
    }

    private void DestroyCanvas() {
        var canvas = GameObject.Find("MissionPanel").GetComponent<CanvasGroup>();
        canvas.alpha = 0;
        canvas.interactable = false;
        canvas.blocksRaycasts = false;

        return;
    }

    public void ClearOptions() {
        foreach (var option in options)
        {
            Destroy(option);
        }

        options.Clear();
    }

    public async void loadNode(Node node) {
        missionProblem = GameObject.Find("Problem").GetComponent<Text>();
        if (node == null) {
            return;
        }

        missionProblem.text = node.dialog;

        //Profile.gains.popularity += node.gains.popularity;
        // Profile.gains.trust += node.gains.trust;
        //Profile.gains.energy += node.gains.energy;
        //Profile.gains.days += node.gains.days;
        // Profile.gains.unlocking_trust += node.gains.unlocking_trust;

        ClearOptions();

        if (node.options.Count == 0) {
            EndMission(node);
        } else {
            for (int i = 0; i < node.options.Count; i++)
            {
                var option = Instantiate(optionPrefab, missionProblem.transform);
                option.name = "Option" + i;
                option.transform.position = new Vector3(missionProblem.transform.position.x, missionProblem.transform.position.y - (i + 1)*100, missionProblem.transform.position.z);

                if (node.options[i].speaker != "player" || node.options.Count == 1)
                {
                    option.GetComponentInChildren<Text>().text = "->";
                }
                else {
                    option.GetComponentInChildren<Text>().text = node.options[i].dialog;
                }

                option.GetComponent<Info>().node = await ApiController.GetNode(node.options[i].id);
                options.Add(option);
            }
        }
    }
}
