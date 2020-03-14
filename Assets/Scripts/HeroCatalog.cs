using System.Collections.Generic;
using UnityEngine;

public class HeroCatalog : MonoBehaviour
{
    [System.Serializable]
    struct HeroInfo
    {
        public string heroName;
        public GameObject heroPrefab;
    }

    [SerializeField]
    private List<HeroInfo> heroList = new List<HeroInfo>();

    public static HeroCatalog Instance;
    
    void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    public GameObject FindHeroByName(string name)
    {
        return heroList.Find(x => x.heroName == name).heroPrefab;
    }
}
