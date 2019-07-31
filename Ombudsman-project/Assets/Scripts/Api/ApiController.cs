using UnityEngine;
using System.Net;
using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

public static class ApiController
{

    private static dynamic getDeserializedJson(string value)
    {
        string url = "http://testhost-laravel.herokuapp.com/api/" + value;
        var jsonResponse = MakeRequest(url);
        var deserializedObj = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
        return deserializedObj;
    }

    public static User GetUser()
    {
        string jsonResponse = MakeRequest("getUser");
        dynamic deserializedObj = getDeserializedJson(jsonResponse);

        var user = new User();

        user.name = deserializedObj.name;

        return user;
    }

    public static Node GetNode(int nodeId)
    {
        dynamic deserializedObj = getDeserializedJson("mission_node/" + nodeId);

        Node node = new Node();
        node.id = deserializedObj["0"].id;
        node.dialog_file_path = deserializedObj["0"].dialog_file_path;

        node.options = deserializedObj.options.ToObject<IList<Node>>();

        return node;
    }

    public static Mission GetMission(int alien_id, int mission_id)
    {
        dynamic deserializedObj = getDeserializedJson(string.Format("alien/{0}/mission/{1}", alien_id, mission_id));

        var mission = new Mission();
        mission.alien = deserializedObj.alien;
        mission.current_node_id = deserializedObj.starting_node_id;

        return mission;
    }

    public static Planet GetPlanet(int planetId)
    {
        Planet planet = new Planet();
        dynamic deserializedObj = getDeserializedJson("planet/" + planetId.ToString());

        planet.name = deserializedObj.name;
        planet.level = deserializedObj.level;
        planet.reachable_population = deserializedObj.reachable_population;

        return planet;
    }

    public static List<Planet> getPlanets()
    {
        return getDeserializedJson("planets").planets.ToObject<IList<Planet>>();
    }

    public static List<Planet> getAliens()
    {
        return getDeserializedJson("aliens").aliens.ToObject<IList<Alien>>();
    }

    private static string MakeRequest(string url)
    {

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format(url));
        request.Method = "GET";
        request.Headers.Add("Authorization", "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImp0aSI6IjM2M2EzZjMzMTNjODBjZjUxZjBkOTcwNDFiMWRkMDNjOGYzNjZiZWFiZjg4NDMzN2EzMTAxNTNlOGJhMmEyZTZiZmI1YmM5MmZjNzY3NjBhIn0.eyJhdWQiOiIxIiwianRpIjoiMzYzYTNmMzMxM2M4MGNmNTFmMGQ5NzA0MWIxZGQwM2M4ZjM2NmJlYWJmODg0MzM3YTMxMDE1M2U4YmEyYTJlNmJmYjViYzkyZmM3Njc2MGEiLCJpYXQiOjE1NjQ0OTUwOTQsIm5iZiI6MTU2NDQ5NTA5NCwiZXhwIjoxNTk2MTE3NDk0LCJzdWIiOiIxIiwic2NvcGVzIjpbXX0.lEY_vqSYG4QM72xDnuUcHC2lUuK-QWLpqo-eaOHljP_WJyYFSjAM5tgGFGw1oyXwUdPtPXNdC7svZE2v765RPMuyGBWGjY0iaNpVMACRhSW6J1evOgA0WruzolDYj5mrseb-pjeTH0ZL54AqG22t7dCR0W5UQoCQxFjZgKLw_H3PrnmZ31iRAmbP0_fXKlv5orJnwtNUBSrlm0COZGQ1zC7uyeggt2s_AutHiW7o_YJw4X0C0NYb-IZLR1gC1ns8B7oP2XQezldQFMU3WndEQKWg4rssR0FdDTEapfgJBZ-S5d0P6B2buoMGvgk2fPJwmPH3gjDopy77p0rIn727dyKE9QFpDDWpyX351OtcK5J9byjjV23D30SDsdbuBAJDkocbJmMSOaFFMAdYdjxpvJXg06LmaZLzkaoNrPLdUqGmahp843KrgPzIbTxW9-Ly_53PvLbi97KWCnKeR856wKwi5kM-daY21LdRez83agtF90Cv_FQsY4Cegdn6DlDonLLvqIS7tBQeWzf6-AHgf-SEBjqnR18cOT7Kr902FH3HrQnW2SgLHdEbuQZuBDlYEzGqmya0dlmAigbmtc4-ZCFZW572N-TdG-6O-w23q3ClF1Rwk270ODA8a_TtLlVCQqqNGX2ZJPkoHraHmTMaBbhV6w0XI5vdhlK7XFE0wFw");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        return jsonResponse;
    }
}