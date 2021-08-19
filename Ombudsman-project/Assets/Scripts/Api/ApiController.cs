using UnityEngine;
using System.Net;
using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using UnityEngine.Networking;
using System.Collections;

public class Token
{
    public string token;
}


public class ApiController : MonoBehaviour
{
    public delegate void WebRequestOnUserReceive(User user);
    public delegate void WebRequestOnNodeReceive(Node node);
    public delegate void WebRequestOnNodeListReceive(List<Node> node);
    public delegate void WebRequestOnMissionReceive(Mission mission);
    public delegate void WebRequestOnPlanetReceive(Planet planet);
    public delegate void WebRequestOnPlanetListReceive(List<Planet> planets);

    private delegate void WebRequestOnDataReceiveDelegate(string responseContent);
    private delegate void WebRequestOnFinishDelegate();


    private static readonly string LOGIN_URL = "https://testhost-laravel.herokuapp.com/login";
    private static readonly string API_URL = "https://testhost-laravel.herokuapp.com/api/";
    private static readonly string API_LOGIN_URL = "https://testhost-laravel.herokuapp.com/api/login";
    private static readonly string API_USERNAME = "nikola.s.sotirov@gmail.com";
    private static readonly string API_PASS = "asdf";

    private static readonly float tokenRequestTimeout = 20f; // Longer time because sometimes first request takes really long for some reason.
    private static readonly float apiRequestTimeout = 5f;


    private static string token;

    private static GainsController gainsController;

    private static ApiController APIController;


    #region #region APIGetters

    public static ApiController GetApiController()
    {
        return APIController;
    }

    public IEnumerator GetUser(WebRequestOnUserReceive onUserReceiveDelegate)
    {
        yield return StartCoroutine(MakeAPIRequest("user", (string responseContent) => {
            User user = GetDeserializedJson<User>(responseContent);

            onUserReceiveDelegate(user);
        }));
    }


    public IEnumerator GetNode(int nodeId, WebRequestOnNodeReceive onNodeReceiveDelegate)
    {
        yield return StartCoroutine(MakeAPIRequest("mission_node/" + nodeId, (string responseContent) =>
        {
            Debug.Log("Loading Node " + nodeId + ": " + responseContent);
            APIMissionNode deserializedObj = GetDeserializedJson<APIMissionNode>(responseContent);

            Node node = new Node();
            node.id = deserializedObj.current_node.id;
            node.dialog = deserializedObj.current_node.dialog;
            node.speaker = deserializedObj.current_node.speaker;
            node.options = new List<Node>();

            foreach (var obj in deserializedObj.options)
            {
                Node newOption = new Node();
                
                newOption.id = obj.node.id;
                newOption.speaker = obj.node.speaker;
                newOption.dialog = obj.node.dialog;
                newOption.gains = obj.node.gains;
                newOption.option_player_dialog = obj.node.option_dialog;
                newOption.unlocking_trust = obj.node.unlocking_trust;
                Debug.Log(newOption.id + ": " + newOption.option_player_dialog);
                node.options.Add(newOption);
            }

            onNodeReceiveDelegate(node);
        }));
    }

    public IEnumerator GetNodes(List<int> nodeIds, WebRequestOnNodeListReceive onNodeListReceiveDelegate)
    {
        string nodeRequestString = string.Join(",", nodeIds.ToArray());
        yield return StartCoroutine(MakeAPIRequest("mission_nodes?node_ids=" + nodeRequestString, (string responseContent) =>
        {
            Debug.Log("Multiple Node Id Content: " + responseContent);
            
        }));
    }


    public IEnumerator GetMission(int alien_id, int mission_order, WebRequestOnMissionReceive onMissionReceiveDelegate)
    {
        yield return StartCoroutine(MakeAPIRequest(string.Format("alien/{0}/mission/{1}", alien_id, mission_order), (string responseContent) =>
        {
            Mission mission = GetDeserializedJson<Mission>(responseContent);

            onMissionReceiveDelegate(mission);
        }));
    }


    public IEnumerator GetPlanet(int planetId, WebRequestOnPlanetReceive onPlanetReceiveDelegate)
    {
        yield return StartCoroutine(MakeAPIRequest("planet/" + planetId.ToString(), (string responseContent) =>
        {
            Planet planet = GetDeserializedJson<Planet>(responseContent);

            onPlanetReceiveDelegate(planet);
        }));
    }

    public IEnumerator GetPlanetsInRange(int startRangePopularity, int endRangePopularity, WebRequestOnPlanetListReceive onPlanetsReceiveDelegate)
    {
        yield return StartCoroutine(MakeAPIRequest("planets/between/" + startRangePopularity + "/" + endRangePopularity, (string responseContent) =>
        {
            Debug.Log("Got Planets");
            APIPlanetList planetList = GetDeserializedJson<APIPlanetList>(responseContent);
            
            onPlanetsReceiveDelegate(planetList.planets);
        }));
    }

    #endregion

    #region #region Implementation
    private void Awake()
    {
        if (APIController)
        {
            Destroy(this);
        }
        else
        {
            APIController = this;
        }
    }


    private void Start()
    {
        gainsController = GainsController.GetInstance();
        StartCoroutine(GetToken(API_USERNAME, API_PASS));
    }


    private static IEnumerator RequestCoroutine(UnityWebRequest request)
    {
        var operation = request.SendWebRequest();
        while (request.result == UnityWebRequest.Result.InProgress)
        {
            yield return new WaitForEndOfFrame();
        }

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError("Network error occured while trying to execute request to " + request.url);
            yield break;
        }

        if (request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("HTTP error occured while trying to execute request to " + request.url + "\n" +
                           "Code returned: " + request.responseCode);
            yield break;
        }
    }


    private IEnumerator RequestDataCoroutine(UnityWebRequest request, WebRequestOnDataReceiveDelegate dataDelegate)
    {
        yield return request;

        dataDelegate(request.downloadHandler.text);
    }


    private IEnumerator RequestCoroutineCallback(UnityWebRequest request, WebRequestOnFinishDelegate onFinishDelegate)
    {
        yield return RequestCoroutine(request);

        onFinishDelegate();
    }


    private IEnumerator GetToken(string username, string password)
    {
        Debug.Log("GetToken called!");
        List<IMultipartFormSection> authenticationFormData = new List<IMultipartFormSection>();
        authenticationFormData.Add(new MultipartFormDataSection("email", username));
        authenticationFormData.Add(new MultipartFormDataSection("password", password));
       
        UnityWebRequest loginRequest = UnityWebRequest.Post(LOGIN_URL, authenticationFormData);
        yield return StartCoroutine(RequestCoroutine(loginRequest));
        Debug.Log("LoginRequest: " + loginRequest.downloadHandler.text);

        UnityWebRequest tokenRequest = UnityWebRequest.Post(API_LOGIN_URL, authenticationFormData);
        yield return StartCoroutine(RequestCoroutine(tokenRequest));
        Debug.Log("TokenRequest: " + tokenRequest.downloadHandler.text);

        

        token = JsonUtility.FromJson<Token>(tokenRequest.downloadHandler.text).token;

        if (loginRequest.result != UnityWebRequest.Result.Success)
        {
            MessageBox messageBox = MessageBoxHandler.boxHandler.DisplayMessage("Failed to get token!",
                new MessageBoxChoiceBehaviour("Try Again"),
                new MessageBoxChoiceBehaviour("Quit", () =>
                {
                    Application.Quit(-1);
                })
            );
            yield return messageBox.BlockUntilClicked();

            Debug.Log(messageBox.GetClickedOptionIndex());
            if (messageBox.GetClickedOptionIndex() == 0)
            {
                Debug.Log("Call GetToken again!");
                yield return StartCoroutine(GetToken(username, password));
            }
        }
        Debug.Log("End GetToken");
    }




    private IEnumerator MakeAPIRequest(string value, WebRequestOnDataReceiveDelegate onDataReceiveDelegate)
    {
        while (token == null) // Token retrieval is started during start.
        {
            yield return new WaitForEndOfFrame();
        }

        string url = API_URL + value;

        Debug.Log("Start API Request to " + url);
        UnityWebRequest contentRequest = UnityWebRequest.Get(url);
        contentRequest.SetRequestHeader("Content-Type", "application/json");
        contentRequest.SetRequestHeader("Bearer", token);

        StartCoroutine(RequestCoroutine(contentRequest));

        float elapsedContentRequestTime = 0;
        while (contentRequest.result == UnityWebRequest.Result.InProgress)
        {
            elapsedContentRequestTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
            if (elapsedContentRequestTime >= apiRequestTimeout)
            {
                throw new TimeoutException("Failed to make request to API in time!");
            }
        }
        Debug.Log("Finished API Request to " + url);

        onDataReceiveDelegate(contentRequest.downloadHandler.text);
    }

    
    private T GetDeserializedJson<T>(string value)
    {
        return JsonUtility.FromJson<T>(value);
    }

    #endregion
}