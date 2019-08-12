using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GainsController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject.Find("popularity").GetComponent<Text>().text = Profile.gains.popularity + "";
        GameObject.Find("trust").GetComponent<Text>().text = Profile.gains.trust + "";
        GameObject.Find("energy").GetComponent<Text>().text = Profile.gains.energy + "";
        GameObject.Find("days").GetComponent<Text>().text = Profile.gains.days + "";
        GameObject.Find("unlocking_trust").GetComponent<Text>().text = Profile.gains.unlocking_trust + "";
    }
}
