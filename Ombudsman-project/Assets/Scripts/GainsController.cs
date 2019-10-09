using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GainsController : MonoBehaviour
{
    private Gains gains = new Gains();
    
    [SerializeField]
    private ResourceView popularity;

    [SerializeField]
    private ResourceView energy;

    [SerializeField]
    private ResourceView trust;

    [SerializeField]
    private ResourceView days;

    private static GainsController gainsController;
    
    public static GainsController GetGainsController()
    {
        return gainsController;
    }

    private void Awake()
    {
        if(gainsController)
        {
            Destroy(this);
        }
        else
        {
            gainsController = this;
        }
    }

    public void UpdateGains(Gains deltaGains)
    {
        gains += deltaGains;

        popularity.SetAmount(gains.popularity);
        energy.SetAmount(gains.energy);
        trust.SetAmount(gains.trust);
        days.SetAmount(gains.days);
    }

    public Gains GetGains()
    {
        return gains;
    }
}
