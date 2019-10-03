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

    public void InitializePlanet(Texture texture, string name)
    {
        image.material.mainTexture = texture;
        nameText.text = name;
        
        button.onClick.AddListener(() =>
        {
            StartCoroutine(LoadPlanet());
        });
    }

    private IEnumerator LoadPlanet()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Planet", LoadSceneMode.Additive);
        
        while (!asyncLoad.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Planet"));
    }
}
