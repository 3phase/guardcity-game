using UnityEngine;
using System.Net;
using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using UnityEngine.Networking;

public static class ApiController
{

    private static readonly string LOGIN_URL = "http://testhost-laravel.herokuapp.com/login";
    private static readonly string API_LOGIN_URL = "https://testhost-laravel.herokuapp.com/api/login";
    private static int FRAME_DELAY_MS
    {
        // If targetFrameRate is -1 it is not set pick reasonable value.
        get { return Application.targetFrameRate == -1 ? 33 : (1 / Application.targetFrameRate * 1000); } 
    }

    private static async Task<dynamic> getDeserializedJson(string value)
    {
        string url = "http://testhost-laravel.herokuapp.com/api/" + value;
        var jsonResponse = await makeRequest(url);
        var deserializedObj = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
        return deserializedObj;
    }

    private static HttpClient client;
    private static string token;

    private static async Task<string> getToken(string username, string password)
    {
        List<IMultipartFormSection> authenticationFormData = new List<IMultipartFormSection>();
        authenticationFormData.Add(new MultipartFormDataSection("email", username));
        authenticationFormData.Add(new MultipartFormDataSection("password", password));

        // @Nikola - zashto se pravi tova??? - Krasi
        UnityWebRequest loginRequest = UnityWebRequest.Post(LOGIN_URL, authenticationFormData);
        await WaitForRequest(loginRequest.SendWebRequest());

        UnityWebRequest tokenRequest = UnityWebRequest.Post(API_LOGIN_URL, authenticationFormData);
        await WaitForRequest(tokenRequest.SendWebRequest());

        string token = JsonConvert.DeserializeObject<dynamic>(tokenRequest.downloadHandler.text).token.Value;

        return token;
    }

    private static async Task<string> makeRequest(string url)
    {
        if (token == null)
        {
            token = await getToken("nikola.s.sotirov@gmail.com", "asdf");
        }

        UnityWebRequest contentRequest = UnityWebRequest.Get(url);
        contentRequest.SetRequestHeader("Content-Type", "application/json");
        contentRequest.SetRequestHeader("Bearer", token);

        await WaitForRequest(contentRequest.SendWebRequest());

        return contentRequest.downloadHandler.text;
    }

    public static async Task<User> GetUser()
    {
        string jsonResponse = await makeRequest("user");
        dynamic deserializedObj = getDeserializedJson(jsonResponse);

        var user = new User();

        user.name = deserializedObj.name;
        return user;
    }

    public static async Task<Node> GetNode(int nodeId)
    {
        dynamic deserializedObj = await getDeserializedJson("mission_node/" + nodeId);

        Node node = new Node();
        node.id = deserializedObj.current_node.id;
        node.dialog = deserializedObj.current_node.dialog;
        node.options = new List<Node>();
        foreach (var obj in deserializedObj.options) {
            Node newOption = new Node();

            newOption.id = obj.node.id;
            newOption.speaker = obj.node.speaker;
            newOption.dialog = obj.node.dialog;

            newOption.gains = new Gains();
            newOption.gains.popularity = obj.gains.popularity;
            newOption.gains.trust = obj.gains.trust;
            newOption.gains.energy = obj.gains.energy;
            newOption.gains.days = obj.gains.days;
            newOption.gains.unlocking_trust = obj.gains.unlocking_trust;

            node.options.Add(newOption);
        }

        return node;
    }

    public static async Task<Mission> GetMission(int alien_id, int mission_id)
    {
        dynamic deserializedObj = await getDeserializedJson(string.Format("alien/{0}/mission/{1}", alien_id, mission_id));

        var mission = new Mission();
        mission.alien = deserializedObj.alien;
        mission.current_node_id = deserializedObj.starting_node_id;

        return mission;
    }

    public static async Task<Planet> GetPlanet(int planetId)
    {
        Planet planet = new Planet();
        dynamic deserializedObj =  await getDeserializedJson("planet/" + planetId.ToString());

        planet.name = deserializedObj.name;
        planet.level = deserializedObj.level;
        planet.unlockingPopularity = deserializedObj.unlocking_popularity;

        return planet;
    }

    private static async Task WaitForRequest(UnityWebRequestAsyncOperation webRequestOperation)
    {
        while(webRequestOperation.isDone == false)
        {
            await Task.Delay(FRAME_DELAY_MS);
        }
    }
}