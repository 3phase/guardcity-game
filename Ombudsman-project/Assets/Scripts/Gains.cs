using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Gains
{
    public int popularity;
    public int trust;
    public int energy;
    public int days;

    public static Gains operator +(Gains gains1, Gains gains2)
    {
        Gains gains = new Gains();
        gains.popularity = gains1.popularity + gains2.popularity;
        gains.trust = gains1.trust + gains2.trust;
        gains.energy = gains1.energy + gains2.energy;
        gains.days = gains1.days + gains2.days;
        return gains;
    }

}
