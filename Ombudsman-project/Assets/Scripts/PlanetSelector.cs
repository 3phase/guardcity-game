using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSelector : MonoBehaviour
{
    [SerializeField]
    RectTransform planetSelectionPanel;
    
    [SerializeField]
    RectTransform scrollPanel;

    [SerializeField]
    PlanetView planetViewPrefab;

    [SerializeField]
    int planetSideOffset = 200;

    ApiController APIController;

    ImageController imageController;

    void Start()
    {
        APIController = FindObjectOfType<ApiController>();
        imageController = FindObjectOfType<ImageController>();

        StartCoroutine(LoadPlanets());
    }

    private IEnumerator LoadPlanets()
    {
        yield return APIController.GetPlanetsInRange(-1, 10, (List<Planet> planets) =>
        {
            int planetIndex = 0; // not using i in a for loop because might be asynchronous.
            foreach (Planet planet in planets)
            {
                Vector3 planetLocation = planetSelectionPanel.transform.position;
                planetLocation.x = planetSideOffset * (planetIndex + 1);

                imageController.GetSprite(planet.image_filename, (Sprite sprite) =>
                {
                    PlanetView planetView = Instantiate(planetViewPrefab, planetSelectionPanel);
                    planetView.InitializePlanet(sprite, planet.name, planet.id);
                    planetView.transform.position = planetLocation;
                });
                planetIndex++;
            }
        });
    }
}
