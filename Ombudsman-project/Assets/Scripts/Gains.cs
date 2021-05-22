using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Gains
{
    [SerializeField]
    private int popularity;

    [SerializeField]
    private int trust;

    [SerializeField]
    private int energy;

    [SerializeField]
    private int days;


    public Gains() {}

    public Gains(int popularity, int trust, int energy, int days)
    {
        this.popularity = popularity;
        this.trust = trust;
        this.energy = energy;
        this.days = days;
    }

    public int GetPopularity()
    {
        return popularity;
    }

    public int GetTrust()
    {
        return trust;
    }

    public int GetEnergy()
    {
        return energy;
    }

    public int GetDays()
    {
        return days;
    }

    public static Gains operator +(Gains a, Gains b)
    {
        return new Gains(a.popularity + b.popularity, a.trust + b.trust, a.energy + b.energy, a.days + b.days);
    }

    public static Gains operator -(Gains a, Gains b)
    {
        return new Gains(a.popularity - b.popularity, a.trust - b.trust, a.energy - b.energy, a.days - b.days);
    }


}
