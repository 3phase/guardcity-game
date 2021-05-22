using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GainsController : MonoBehaviour
{
    [SerializeField]
    Gains defaultGains;

    [SerializeField]
    private int energyRestoration;
    
    [SerializeField]
    private ResourceView popularity;

    [SerializeField]
    private ResourceView energy;

    [SerializeField]
    private ResourceView trust;

    [SerializeField]
    private ResourceView days;

    private static GainsController gainsController;

    private Gains gains = new Gains();

    
    public static GainsController GetInstance()
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
            UpdateGains(defaultGains, false);
        }
    }

    public void UpdateGains(Gains deltaGains, bool useAnimation = true)
    {
        gains += deltaGains;

        popularity.Alter(deltaGains.GetPopularity(), useAnimation);
        energy.Alter(deltaGains.GetEnergy(), useAnimation);
        trust.Alter(deltaGains.GetTrust(), useAnimation);
        days.Alter(deltaGains.GetDays(), useAnimation);
    }

    public Gains GetGains()
    {
        return gains;
    }

    public void RestoreEnergy()
    {
        UpdateGains(new Gains(0, 0, energyRestoration - gains.GetEnergy(), 1));
    }
}
