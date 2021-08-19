using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlanetAlien : MonoBehaviour
{
    [SerializeField]
    Button button;

    [SerializeField]
    Image image;

    Alien alienInfo;

    int missionNodeId = 1;
    

    private void Awake()
    {
        button.onClick.AddListener(() =>
        {
            StartCoroutine(StartMission());
        });
    }

    private IEnumerator StartMission()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Mission", LoadSceneMode.Additive);

        Mission mission = null;
        Coroutine coroutine = StartCoroutine(FindObjectOfType<ApiController>().GetMission(alienInfo.id, missionNodeId, (Mission requestMission) =>
        {
            mission = requestMission;
        }));

        while(asyncLoad.isDone == false)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return coroutine;
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Mission"));

        MissionController missionController = FindObjectOfType<MissionController>();
        missionController.StartMission(mission.starting_node_id, this);
    }

    public void CompleteMission()
    {
        // TODO: More stuff should probably be here.
        missionNodeId++;
    }


    public void SetAlienInfo(Alien alien)
    {
        alienInfo = alien;
        ImageController imageController = FindObjectOfType<ImageController>();
        StartCoroutine(imageController.GetSprite(alienInfo.picture_path, (Sprite sprite) =>
        {
            button.image.sprite = sprite;
        }));
    }

    public Alien GetAlienInfo()
    {
        return alienInfo;
    }
}
