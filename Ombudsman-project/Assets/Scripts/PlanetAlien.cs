using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetAlien : MonoBehaviour
{
    [SerializeField]
    Button button;

    [SerializeField]
    Image image;

    Alien alienInfo;


    private void Awake()
    {
        button.onClick.AddListener(() =>
        {
            Debug.Log("Attempt to load alien mission");
        });
    }


    public void SetAlienInfo(Alien alien)
    {

        Debug.Log("Set alien info with image " + alien.picture_path);
        alienInfo = alien;
        ImageController imageController = FindObjectOfType<ImageController>();
        imageController.GetSprite(alienInfo.picture_path, (Sprite sprite) =>
        {
            button.image.sprite = sprite;
        });
    }
}
