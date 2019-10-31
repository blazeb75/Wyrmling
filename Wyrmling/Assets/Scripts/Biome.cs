using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Contains a biome's prefab, spawn restrictions, spawn frequency, and the random objects it contains.
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/Biome")]
public class Biome : ScriptableObject
{
    [Tooltip("Prefab containing the biome floor and any guarenteed objects.")]
    public GameObject prefab;
    [Tooltip("All of the objects that will be randomly placed in each instance of the biome")]
    public EnvironmentalObject[] randomlyPlacedObjects;
    [Tooltip("Relative chance for the biome to spawn")]
    public float weight = 1;
    [Tooltip("Minimum distance from spawn. 0 = At spawn")]
    public int minDistance = 1;
    [Tooltip("Maximum distance from spawn. 0 = At spawn")]
    public int maxDistance = int.MaxValue;

    /// <summary>
    /// Contains a prefab and the number of instances that can spawn in a single biome instance
    /// </summary>
    [System.Serializable]
    public class EnvironmentalObject
    {
        public GameObject prefab;
        public int maxQuantity = 15;
    }

}
