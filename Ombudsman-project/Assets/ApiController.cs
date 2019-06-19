using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ApiController : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        
    }

    void Start()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Post("https://jsonplaceholder.typicode.com/posts", "");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
            Processjson(www.downloadHandler.text);

        }
    }

    private void Processjson(string jsonString)
    {
        string json = JsonUtility.ToJson(jsonString);
        var myObject = JsonUtility.FromJson<string>(json);
        Debug.Log(myObject);
    }
}
