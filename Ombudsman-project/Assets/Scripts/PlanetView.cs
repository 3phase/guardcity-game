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

    public void InitializePlanet(Texture2D texture, string name)
    {
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        image.sprite = sprite;
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
