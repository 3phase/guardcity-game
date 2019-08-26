using System;
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
    public GameObject optionPrefab;

    [SerializeField]
    private TMP_Text missionProblem;

    [SerializeField]
    private Button startButton;

    private List<GameObject> options = new List<GameObject>();
    private ApiController APIController;

    private void Start()
    {
        APIController = ApiController.GetApiController();
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
        missionProblem.text = node.dialog;

        var option = Instantiate(optionPrefab, missionProblem.transform);
        option.name = "End Mission";
        option.transform.position = new Vector3(missionProblem.transform.position.x,
                missionProblem.transform.position.y - 100, missionProblem.transform.position.z);
        option.GetComponentInChildren<TMP_Text>().text = "Край на мисията";
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

    public IEnumerator LoadNode(Node node)
    {
        if (node == null) {
            yield break;
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
                int nodeIndex = i;
                StartCoroutine(APIController.GetNode(node.options[i].id, (Node requestedNode) =>
                {
                    var option = Instantiate(optionPrefab, missionProblem.transform);
                    option.name = "Option" + requestedNode.id;
                    option.transform.position = new Vector3(missionProblem.transform.position.x, missionProblem.transform.position.y - (nodeIndex + 1) * 100, missionProblem.transform.position.z);

                    if (node.options[nodeIndex].speaker != "player" || node.options.Count == 1)
                    {
                        option.GetComponentInChildren<TMP_Text>().text = "->";
                    }
                    else
                    {
                        option.GetComponentInChildren<TMP_Text>().text = requestedNode.dialog;
                    }
                    
                    option.GetComponent<Info>().node = requestedNode;
                    options.Add(option);
                }));
            }
        }
    }
}
