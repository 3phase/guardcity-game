using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetController : MonoBehaviour
{
    [SerializeField]
    Image backgroundImage;

    [SerializeField]
    Canvas canvas;

    [SerializeField]
    PlanetAlien alienPrefab;

    Planet planetInfo;

    Dictionary<Vector2, PlanetAlien> alienPositions = new Dictionary<Vector2, PlanetAlien>(); // map canvas position to alien.

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    public void InitializePlanet(Planet planet)
    {
        Debug.Log(planet.aliens.Count);
        if(planet == null)
        {
            throw new UnityException("Attempting to initialize planet with no info.");
        }
        Debug.Log("Loaded planet " + planet.name);
        planetInfo = planet;

        foreach(AlienPosition alienPosition in planetInfo.alien_coordinates)
        {
            Vector2 position = new Vector2(alienPosition.xCoord, alienPosition.yCoord);
            alienPositions.Add(position, null);
        }


        ImageController imageController = FindObjectOfType<ImageController>();
        imageController.GetSprite(planetInfo.background_image, (Sprite sprite) =>
        {
            backgroundImage.sprite = sprite;
        });

        LoadAliens();

        canvas.sortingOrder = 25;
        canvas.overrideSorting = true;
    }


    public void LoadAliens()
    {
        if(planetInfo.aliens.Count == 0)
        {
            Debug.LogError("Planet has no aliens to load!");
            return;
        }

        foreach(Alien alienInfo in planetInfo.aliens)
        {
            Vector2 relativePosition = GetFreeAlienRelativePosition();
            if (relativePosition == new Vector2(-1, -1))
            {
                Debug.LogWarning("No more space for aliens! Stopping at alien " + alienInfo.name);
                return;
            }
            Vector2 position = new Vector2(
                Mathf.Lerp(0, canvas.pixelRect.width, relativePosition.x),
                Mathf.Lerp(0, canvas.pixelRect.height, relativePosition.y)
            );
            PlanetAlien alien = Instantiate(alienPrefab, position, Quaternion.identity);
            alien.SetAlienInfo(alienInfo);
            alien.transform.SetParent(canvas.transform);
            alienPositions[relativePosition] = alien;
        }
    }


    public Vector2 GetFreeAlienRelativePosition()
    {
        foreach(Vector2 relativePosition in alienPositions.Keys)
        {
            if(alienPositions[relativePosition] == null)
            {
                return relativePosition;
            }
        }

        return new Vector2(-1, -1);
    }

    public Planet GetPlanetInfo()
    {
        return planetInfo;
    }
}
