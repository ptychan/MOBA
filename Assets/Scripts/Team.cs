using UnityEngine;

public class Team : MonoBehaviour
{
    public enum Faction
    {
        Sunny,
        Gloomy
    };

    public Faction faction = Faction.Sunny;
}
