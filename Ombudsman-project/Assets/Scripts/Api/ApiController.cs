using UnityEngine;
using System.Net;
using System;
using System.IO;
public static class ApiController {

    public static User GetUser() {
        string jsonResponse = MakeRequest("http://testhost-laravel.herokuapp.com/getUser");
        return DeserializeJson<User>(jsonResponse);
    }
    
    public static Node GetNode(int nodeId) {
        string jsonResponse = MakeRequest("http://testhost-laravel.herokuapp.com/mission_node/" + nodeId);
        return DeserializeJson<Node>(jsonResponse);
    }

    public static Planet GetPlanet(int planetId) {
        string jsonResponse = MakeRequest("http://testhost-laravel.herokuapp.com/planet/" + planetId);
        return DeserializeJson<Planet>(jsonResponse);
    }

    public static Planet[] GetPlanets() {
        string jsonResponse = MakeRequest("http://testhost-laravel.herokuapp.com/planets/");
        return DeserializeJson<Planet[]>(jsonResponse);
    }

    private static T DeserializeJson<T>(string jsonResponse) {
        T obj = JsonUtility.FromJson<T>(jsonResponse);
        return obj;
    }

    private static string MakeRequest(string url) {
        
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format(url));
        request.Method = "GET";
        request.Headers.Add("Authorization", "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImp0aSI6IjFiZDMxMWQwZTljY2EyNjA1ZjVmMjg4ODI1YmQyMjlkMzBjYzQ3OGFmMjRmZjEwYzYyZWJiZGM4YTdhNDYyNzI1MWE4YWEwMzcwZWQ3NzQwIn0.eyJhdWQiOiIyMSIsImp0aSI6IjFiZDMxMWQwZTljY2EyNjA1ZjVmMjg4ODI1YmQyMjlkMzBjYzQ3OGFmMjRmZjEwYzYyZWJiZGM4YTdhNDYyNzI1MWE4YWEwMzcwZWQ3NzQwIiwiaWF0IjoxNTYzOTc1NDI5LCJuYmYiOjE1NjM5NzU0MjksImV4cCI6MTU5NTU5NzgyOSwic3ViIjoiMSIsInNjb3BlcyI6W119.hEUrCA_4sIGVRisWtDhvFdGiG5-hdZM6eLq3z2UF_ywwhwwuFfHU-P08k_MJk9l7bPpguLHPdc3S3m-u1IMQQFb83zMwgeA7XB2voXzcr8cNCjVAg4XsBDt2JKcL5xVfYTVwPJJtRIkD8_OT_oeSURWxlpGg_4MXaA8amzE2MGCMPM2h6-JAjegjeVkRYN8Adh7-WNARyWu1aw304YpW1cTy3Z4WPDNdN99YVH1pcP3ZGk8vgM7pUVyxL3BHeN2xBmFq16PaR8gpuCffiKjkEMiXCEXyt4KBfQf3Y28-5cZjtW5KDM4m8Xc33lkqxgUrbzbRlJUMK6-VV7CBJjg9Y590ATLYkqf9Q51a9ljt0YCFN1HItPm47RwAVKv1px-pagvA0i3GCu6XVdyQPV20YUQLHjQGWOBENocWTU9Uo6Pl16GeiA1pXr3lTQj-BEXqmzxXP6ucrCAJc7evcG7IO7DXaRZ8_iH86BFY0gSjFFegM0I-LsHBhnqewU_yyAd8ynrJQ93qYCculpzlEObfDQUXtkzT5w_pYyw9zpaNke3i8R5t_uUg1AaANpxE9yT0toeF1AyYjV6GZs2Dgd4mEKSX1Y79--7WYfLSEQWcke_BlrQ9GkxEcM9z_QwdnkfGq9cDb4y2Ds9l7vVCp-fKSNwZ-BfCmWJTU5vEcGG6DDM");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        return jsonResponse;
    }
}