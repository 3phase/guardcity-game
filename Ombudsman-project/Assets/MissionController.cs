﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionController : MonoBehaviour
{
    Mission mission;
    public GameObject optionPrefab;
    private Text missionProblem;
    private List<GameObject> options;

    public async void StartMission()
    {
        Planet planet = await ApiController.GetPlanet(1);

        options = new List<GameObject>();

        Node node = await ApiController.GetNode(41);

        LoadMission(node);
    }

    public void ClearOptions() {
        foreach(var option in options) {
            Destroy(option);
        }
        options.Clear();
    }

    public void EndMission() {
        var canvas = GameObject.Find("MissionPanel").GetComponent<CanvasGroup>();
        canvas.alpha = 0;
        canvas.interactable = false;
        canvas.blocksRaycasts = false;
    }

    public async void LoadMission(Node node) {
        
        missionProblem = GameObject.Find("Problem").GetComponent<Text>();
        missionProblem.text = node.dialog;

        //Profile.gains.popularity += node.gains.popularity;
       // Profile.gains.trust += node.gains.trust;
        //Profile.gains.energy += node.gains.energy;
        //Profile.gains.days += node.gains.days;
       // Profile.gains.unlocking_trust += node.gains.unlocking_trust;

        Debug.Log("HERE");

        ClearOptions();
        
        if(node.options.Count == 0) {
            EndMission();
        } else {
            for (int i = 0; i < node.options.Count; i++)
            {
                var option = Instantiate(optionPrefab, missionProblem.transform);
                option.name = "Option" + i;
                option.transform.position = new Vector3(missionProblem.transform.position.x, missionProblem.transform.position.y - (i + 1)*75, missionProblem.transform.position.z);
                option.GetComponentInChildren<Text>().text = node.options[i].dialog;
                option.GetComponent<Info>().node = await ApiController.GetNode(node.options[i].pivot.next_id);
                options.Add(option);
            }
        }
    }
}
