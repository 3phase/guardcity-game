using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionDebugStarter : MonoBehaviour
{
    [SerializeField]
    int testAlienId = 1;

    [SerializeField]
    int missionOrder = 1;

    IEnumerator Start()
    {
        Mission mission = null;
        yield return StartCoroutine(FindObjectOfType<ApiController>().GetMission(testAlienId, missionOrder, (Mission requestMission) =>
        {
            mission = requestMission;
        }));
        FindObjectOfType<MissionController>().StartMission(mission.starting_node_id, null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
