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
    public delegate void WebRequestOnMissionReceive(Mission mission);
    public delegate void WebRequestOnPlanetReceive(Planet planet);
    public delegate void WebRequestOnPlanetsReceive(List<Planet> planets);

    private delegate void WebRequestOnDataReceiveDelegate(string responseContent);
    private delegate void WebRequestOnFinishDelegate();

    private static readonly string LOGIN_URL = "https://testhost-laravel.herokuapp.com/login";
    private static readonly string API_URL = "https://testhost-laravel.herokuapp.com/api/";
    private static readonly string API_LOGIN_URL = "https://testhost-laravel.herokuapp.com/api/login";
    private static readonly string API_USERNAME = "nikola.s.sotirov@gmail.com";
    private static readonly string API_PASS = "asdf";


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
            Debug.Log("Node json: " + responseContent);

            APIMissionNode deserializedObj = GetDeserializedJson<APIMissionNode>(responseContent);

            Node node = new Node();
            node.id = deserializedObj.current_node.id;
            node.dialog = deserializedObj.current_node.dialog;
            node.speaker = deserializedObj.current_node.speaker;
            node.options = new List<Node>();

            foreach (var obj in deserializedObj.options)
            {
                if(obj.unlocking_trust > gainsController.GetGains().trust) { return;  }

                Node newOption = new Node();
                
                newOption.id = obj.node.id;
                newOption.speaker = obj.node.speaker;
                newOption.dialog = obj.node.dialog;
                
                newOption.gains.popularity = obj.node.gains.popularity;
                newOption.gains.trust = obj.node.gains.trust;
                newOption.gains.energy = obj.node.gains.energy;
                newOption.gains.days = obj.node.gains.days;

                node.options.Add(newOption);
            }

            onNodeReceiveDelegate(node);
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

    public IEnumerator GetPlanetsInRange(int startRangePopularity, int endRangePopularity, WebRequestOnPlanetsReceive onPlanetsReceiveDelegate)
    {
        yield return StartCoroutine(MakeAPIRequest("planets/between/" + startRangePopularity + "/" + endRangePopularity, (string responseContent) =>
        {
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
        gainsController = GainsController.GetGainsController();
        StartCoroutine(GetToken(API_USERNAME, API_PASS));
    }


    private static IEnumerator RequestCoroutine(UnityWebRequest request)
    {
        var operation = request.SendWebRequest();
        while (operation.isDone == false)
        {
            yield return new WaitForEndOfFrame();
        }

        if (request.isNetworkError)
        {
            Debug.LogError("Network error occured while trying to execute request to " + request.url);
            yield break;
        }

        if (request.isHttpError)
        {
            Debug.LogError("HTTP error occured while trying to execute request to " + request.url + "\n" +
                           "Code returned: " + request.responseCode);
            yield break;
        }
    }


    private IEnumerator RequestDataCoroutine(UnityWebRequest request, WebRequestOnDataReceiveDelegate dataDelegate)
    {
        yield return RequestCoroutine(request);

        dataDelegate(request.downloadHandler.text);
    }


    private IEnumerator RequestCoroutineCallback(UnityWebRequest request, WebRequestOnFinishDelegate onFinishDelegate)
    {
        yield return RequestCoroutine(request);

        onFinishDelegate();
    }


    private IEnumerator GetToken(string username, string password)
    {
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

        Debug.Log("Got token: " + token);
    }




    private IEnumerator MakeAPIRequest(string value, WebRequestOnDataReceiveDelegate onDataReceiveDelegate)
    {
        while (token == null) // Token retrieval is started during start.
        {
            yield return new WaitForEndOfFrame();
        }

        string url = API_URL + value;
        UnityWebRequest contentRequest = UnityWebRequest.Get(url);
        contentRequest.SetRequestHeader("Content-Type", "application/json");
        contentRequest.SetRequestHeader("Bearer", token);
        
        yield return StartCoroutine(RequestDataCoroutine(contentRequest, onDataReceiveDelegate));
    }

    
    private T GetDeserializedJson<T>(string value)
    {
        return JsonUtility.FromJson<T>(value);
    }

    #endregion
}