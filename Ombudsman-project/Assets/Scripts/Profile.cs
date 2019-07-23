using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Profile
{
    public static int id;
    public static string name;
    public static string alienImagePath;
    public static string currentPlanet;

    public static void SetProfile(User user) {
        Profile.id = user.id;
        Profile.name = user.name;
        Profile.alienImagePath = user.alienImagePath;
        Profile.currentPlanet = user.currentPlanet;
    }
}
