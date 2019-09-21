using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Biome")]
public class Biome : ScriptableObject
{
    public GameObject prefab;
    public EnvironmentalObject[] randomlyPlacedObjects;
    public float weight = 1;
    //Minimum distance from spawn.
    public int minDistance = 1;
    public int maxDistance = int.MaxValue;

    [System.Serializable]
    public class EnvironmentalObject
    {
        public GameObject prefab;
        //public int weight = 1;
        public int maxQuantity = 15;
    }

}
