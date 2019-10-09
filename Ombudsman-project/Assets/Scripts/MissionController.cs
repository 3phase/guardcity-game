﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionController : MonoBehaviour
{
    Mission mission;
    public RectTransform optionPrefab;

    [SerializeField]
    private TMP_Text missionProblem;

    [SerializeField]
    private Button startButton;

    [SerializeField]
    private Button missionEndButton;

    [SerializeField]
    private RectTransform choicesPanel;

    [SerializeField]
    private float choiceTopMargin;

    [SerializeField]
    private RectTransform playerAvatar;

    [SerializeField]
    private RectTransform alienAvatar;

    private List<GameObject> options = new List<GameObject>();
    private ApiController APIController;
    private GainsController gainsController;

    private void Start()
    {
        APIController = ApiController.GetApiController();
        gainsController = GainsController.GetGainsController();
        startButton.onClick.AddListener(StartMission);
    }

    public void StartMission()
    {
        startButton.gameObject.SetActive(false);
        StartCoroutine(StartMissionCoroutine());
    }

    private IEnumerator StartMissionCoroutine()
    {
        Planet planet = null;
        yield return StartCoroutine(APIController.GetPlanet(1, (Planet requestPlanet) => 
        {
            planet = requestPlanet;
        }));
        
        Node node = null;
        yield return APIController.GetNode(41, (Node requestNode) =>
        {
            node = requestNode;
        });

        StartCoroutine(LoadNode(node));
    }

    public void EndMission(Node node) {
        missionEndButton.gameObject.SetActive(true);
    }

    public void ClearOptions() {
        foreach (var option in options)
        {
            Destroy(option);
        }

        options.Clear();
    }

    public IEnumerator LoadNode(Node node)
    {
        if (node == null) {
            Debug.LogError("node == null");
            yield break;
        }

        missionProblem.text = node.dialog;

        ClearOptions();
        

        if (node.speaker == "player")
        {
            playerAvatar.gameObject.SetActive(true);
            alienAvatar.gameObject.SetActive(false);
        }
        else
        {
            playerAvatar.gameObject.SetActive(false);
            alienAvatar.gameObject.SetActive(true);
        }


        if (node.options.Count == 0) {
            EndMission(node);
        }
        else
        {
            GainsController.GetGainsController().UpdateGains(node.gains);

            // Load options
            for (int i = 0; i < node.options.Count; i++)
            {
                int nodeIndex = i; // because coroutine is asynchronous.
                
                StartCoroutine(APIController.GetNode(node.options[i].id, (Node requestedNode) =>
                {
                    requestedNode.gains = node.options[nodeIndex].gains; // Workaround because gains is not in current_node in json.

                    var option = Instantiate(optionPrefab, choicesPanel.transform);
                    option.name = "Option" + requestedNode.id;

                    Vector3 optionPosition = choicesPanel.transform.position;

                    float yOffset = (3 - nodeIndex) * (option.rect.height + choiceTopMargin) + choicesPanel.rect.height / 4; 
                    float choicesPanelTopY = choicesPanel.transform.position.y + choicesPanel.rect.height / 2 - option.rect.height / 2;
                    optionPosition.y = choicesPanelTopY - yOffset;
                    option.transform.position = optionPosition;

                    option.GetComponentInChildren<TMP_Text>().text = node.options.Count == 1 ? "->" : requestedNode.dialog;
                    option.GetComponent<Info>().node = requestedNode;

                    options.Add(option.gameObject);
                }));
            }
        }

        

    }
}
