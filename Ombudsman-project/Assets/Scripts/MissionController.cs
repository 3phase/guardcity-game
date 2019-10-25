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
    private Button missionEndButton;

    [SerializeField]
    private RectTransform choicesPanel;

    [SerializeField]
    private float choiceTopMargin;

    [SerializeField]
    private Avatar playerAvatar;

    [SerializeField]
    private Avatar alienAvatar;

    private List<GameObject> options = new List<GameObject>();
    private ApiController APIController;
    private GainsController gainsController;

    private void Start()
    {
        APIController = ApiController.GetApiController();
        gainsController = GainsController.GetGainsController();
    }

    public void StartMission(int startingMissionNode, Alien alien)
    {
        ImageController imageController = FindObjectOfType<ImageController>();
        imageController.GetSprite(alien.picture_path, (Sprite sprite) =>
        {
            alienAvatar.SetAvatar(sprite, alien.name);
        });

        if(startingMissionNode == -1)
        {
            throw new UnityException("Attempting to start mission when there are no more missions with this alien!");
        }
        StartCoroutine(StartMissionCoroutine(startingMissionNode));
    }

    private IEnumerator StartMissionCoroutine(int startingMissionNode)
    {
        Node node = null;
        yield return APIController.GetNode(startingMissionNode, (Node requestNode) =>
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


        GainsController.GetGainsController().UpdateGains(node.gains);
        if (node.options.Count == 0) {
            EndMission(node);
        }
        else
        {
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
