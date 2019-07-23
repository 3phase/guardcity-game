using UnityEngine;
using System.Net;
using System;
using System.IO;
public static class ApiController {

    public static User GetUser() {
        string jsonResponse = MakeRequest("localhost:3000/get-user/");
        return DeserializeJson<User>(jsonResponse);
    }
    
    public static Node GetNode(int planetId, int alienId) {
        string jsonResponse = MakeRequest("localhost:3000/planets/" + planetId + "/aliens/" + alienId + "/missions/init");
        return DeserializeJson<Node>(jsonResponse);
    }

    public static Planet GetPlanet(int planetId) {
        string jsonResponse = MakeRequest("localhost:3000/planets/" + planetId);
        return DeserializeJson<Planet>(jsonResponse);
    }

    public static Planet[] GetPlanets() {
        string jsonResponse = MakeRequest("localhost:3000/planets/");
        return DeserializeJson<Planet[]>(jsonResponse);
    }

    private static T DeserializeJson<T>(string jsonResponse) {
        T obj = JsonUtility.FromJson<T>(jsonResponse);
        return obj;
    }

    private static string MakeRequest(string url) {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format(url));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        return jsonResponse;
    }
}