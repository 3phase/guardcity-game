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

public static class ApiController
{

    private static async Task<dynamic> getDeserializedJson(string value)
    {
        string url = "http://testhost-laravel.herokuapp.com/api/" + value;
        var jsonResponse = await makeRequest(url);
        var deserializedObj = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
        return deserializedObj;
    }

    private static HttpClient client;
    private static string token;

    private static async Task<string> getToken(string username, string passoword)
    {
        string requestUrl = "http://testhost-laravel.herokuapp.com/login";

        client = new HttpClient();

        var values = new Dictionary<string, string> {
                { "email", username },
                { "password", passoword }
            };

        var content = new FormUrlEncodedContent(values);
        var response = await client.PostAsync(requestUrl, content);
        var responseString = await response.Content.ReadAsStringAsync();
        requestUrl = "https://testhost-laravel.herokuapp.com/api/login";
        response = await client.PostAsync(requestUrl, content);
        responseString = await response.Content.ReadAsStringAsync();
        token = JsonConvert.DeserializeObject<dynamic>(responseString).token.Value;

        return token;
    }

    private static async Task<string> makeRequest(string url)
    {
        if (token == null)
        {
            string token = await getToken("nikola.s.sotirov@gmail.com", "asdf");
        }

        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync(url);

        var responseString = await response.Content.ReadAsStringAsync();

        return responseString;
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
}