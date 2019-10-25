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
        gains.popularity += deltaGains.popularity;
        gains.trust += deltaGains.trust;
        gains.energy += deltaGains.energy;
        gains.days += deltaGains.days;

        popularity.Alter(deltaGains.popularity);
        energy.Alter(deltaGains.energy);
        trust.Alter(deltaGains.trust);
        days.Alter(deltaGains.days);
    }

    public Gains GetGains()
    {
        return gains;
    }
}
