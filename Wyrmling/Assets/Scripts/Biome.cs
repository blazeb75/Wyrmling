using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Biome")]
public class Biome : ScriptableObject
{
    public GameObject prefab;
    public float weight = 1;
    //Minimum distance from spawn.
    public int minDistance = 1;
    public int maxDistance = int.MaxValue;

}
