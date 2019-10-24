using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlanetView : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Text nameText;

    [SerializeField]
    Image image;

    [SerializeField]
    Button button;

    int planetId;

    public void InitializePlanet(Sprite sprite, string name, int planetId)
    {
        image.sprite = sprite;
        nameText.text = name;
        this.planetId = planetId;
        
        button.onClick.AddListener(() =>
        {
            StartCoroutine(LoadPlanet());
        });
    }

    private IEnumerator LoadPlanet()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Planet", LoadSceneMode.Additive);
        
        Planet planetInfo = null;
        Coroutine coroutine = StartCoroutine(FindObjectOfType<ApiController>().GetPlanet(planetId, (Planet detailedPlanetInfo) =>
        {
            planetInfo = detailedPlanetInfo;
            Debug.Log("Alien count: " + detailedPlanetInfo.aliens.Count);
            Debug.Log("Alien position count: " + detailedPlanetInfo.alien_coordinates.Count);
        }));

        while (!asyncLoad.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return coroutine;
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Planet"));

        PlanetController planetController = FindObjectOfType<PlanetController>();
        planetController.InitializePlanet(planetInfo);
    }
}
