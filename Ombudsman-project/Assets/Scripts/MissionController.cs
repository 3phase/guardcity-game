using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionController : MonoBehaviour
{

    public RectTransform optionPrefab;

    [SerializeField]
    private Canvas canvas;

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

    [SerializeField]
    private Image backgroundImage;


    private Dictionary<int, Node> nodeCache = new Dictionary<int, Node>(); // id - node
    private List<int> nodesLoading = new List<int>();

    private List<GameObject> options = new List<GameObject>();
    private ApiController APIController;
    private GainsController gainsController;

    private Mission mission;

    private PlanetAlien alien;

    private void Start()
    {
        APIController = ApiController.GetApiController();
        gainsController = GainsController.GetInstance();
    }

    public void StartMission(int startingMissionNode, PlanetAlien alien)
    {
        this.alien = alien;
        if (alien != null)
        {
            ImageController imageController = FindObjectOfType<ImageController>();
            StartCoroutine(imageController.GetSprite(FindObjectOfType<PlanetController>().GetPlanetInfo().background_image, (Sprite sprite) =>
            {
                backgroundImage.sprite = sprite;
            }));

            StartCoroutine(imageController.GetSprite(alien.GetAlienInfo().picture_path, (Sprite sprite) =>
            {       
                alienAvatar.SetAvatar(sprite, alien.GetAlienInfo().name);
            }));
        }
        else
        {
            Debug.LogWarning("Starting mission while not connected to alien!");
        }

        

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
        missionEndButton.onClick.AddListener(() =>
        {
            alien.CompleteMission();

            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            Scene planetScene = SceneManager.GetSceneByName("Planet");
            SceneManager.SetActiveScene(planetScene);
            missionEndButton.gameObject.SetActive(false);
        });
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


        GainsController.GetInstance().UpdateGains(node.gains);
        if (node.options.Count == 0) {
            EndMission(node);
        }
        else
        {
            List<Coroutine> coroutineRequests = new List<Coroutine>();
            List<Node> optionNodes = new List<Node>();
            Debug.Log("Possible Options Count: " + node.options.Count);
            foreach(Node option in node.options)
            {
                Coroutine requestCoroutine = StartCoroutine(GetNode(option.id, (Node node) =>
                {
                    optionNodes.Add(node);
                }));
                coroutineRequests.Add(requestCoroutine);
            }

            while(node.options.Count > optionNodes.Count)
            {
                yield return new WaitForEndOfFrame();
            }


            foreach (Node optionNode in optionNodes)
            {
                var option = Instantiate(optionPrefab, choicesPanel.transform);
                option.name = "Option" + optionNode.id;

                string dialogText = node.options.Count == 1 ? "->" : optionNode.id + ": " + optionNode.dialog;
                if (optionNode.unlocking_trust > GainsController.GetInstance().GetGains().GetTrust())
                    dialogText = "Locked";

                option.GetComponentInChildren<TMP_Text>().text = dialogText;
                    
                option.GetComponent<Info>().node = optionNode;
                options.Add(option.gameObject);

                foreach (Node choice in optionNode.options)
                {
                    if (nodeCache.ContainsKey(choice.id) == false)
                    {
                        StartCoroutine(PreloadNode(choice.id));
                    }
                }
            }
        }
    }

    private IEnumerator GetNode(int nodeId, Action<Node> onNodeLoaded)
    {
        Node node;
        if(nodeCache.TryGetValue(nodeId, out node))
        {
            onNodeLoaded(node);
        }
        else if(nodesLoading.Contains(nodeId))
        {
            while (nodeCache.TryGetValue(nodeId, out node) == false)
            {
                yield return new WaitForEndOfFrame();
            }
            onNodeLoaded(node);
        }
        else
        {
            yield return APIController.GetNode(nodeId, (Node node) =>
            {
                onNodeLoaded(node);
            });
        }
    }

    private IEnumerator PreloadNode(int nodeId)
    {
        nodesLoading.Add(nodeId);
        yield return APIController.GetNode(nodeId, (Node node) =>
        {
            nodeCache.Add(nodeId, node);
            Debug.Log(node.speaker);
        });
        Debug.Log("Successfully Preloaded Node With Id: " + nodeId);
        nodesLoading.Remove(nodeId);
    }
}
