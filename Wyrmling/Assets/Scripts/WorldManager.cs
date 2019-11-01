using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager instance;

    [SerializeField] Biome[] biomes;
    [SerializeField] float biomeSize = 50;
    [SerializeField] List<WorldTile> spawnedBiomes;
    [SerializeField] List<Vector2> visitedPositions;

    List<Biome> validBiomes;
    List<float> weights;

    //A WorldTile represents a biome instance
    [System.Serializable]
    struct WorldTile
    {        
        public string name;
        public Biome biome;
        public GameObject obj;
        public Vector2 position;

        public WorldTile(Biome biome, GameObject instance, Vector2 pos)
        {
            this.biome = biome;
            name = biome.prefab.name;
            obj = instance;
            position = pos;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Duplicate world manager instance found", this.gameObject);
            Destroy(this);
        }

        transform.position = new Vector3(0, 0, 0);

        //Initialise lists
        validBiomes = new List<Biome>();
        weights = new List<float>();
        spawnedBiomes = new List<WorldTile>();
        visitedPositions = new List<Vector2>();        
    }

    private void Start()
    {
        SpawnBiome(new Vector2(0, 0));
        StartCoroutine(CheckPlayerExploration());
    }

    //Periodically checks if the player has moved to an unexplored biome
    //Becomes more frequent as the player grows
    IEnumerator CheckPlayerExploration()
    {
        while (true)
        {
            if (!visitedPositions.Contains(GetPlayerPosition()))
            {
                visitedPositions.Add(GetPlayerPosition());
                SpawnBiomesAroundPlayer();                
            }
            yield return new WaitForSeconds(Mathf.Sqrt(3f / PlayerManager.instance.transform.localScale.y));
        }
    }

    //Get the player location in terms of biomes, where 0,0 is the starting biome and 0,1 is the one immediately above
    Vector2 GetPlayerPosition()
    {
        Vector2 playerPos = PlayerManager.instance.transform.position;
        playerPos.x = Mathf.Round(playerPos.x / biomeSize);
        playerPos.y = Mathf.Round(playerPos.y / biomeSize);
        return playerPos;
    }

    void SpawnBiomesAroundPlayer()
    {
        Vector2 playerPos = GetPlayerPosition();

        //Get the locations around the player
        List<Vector2> positions = new List<Vector2>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                positions.Add(new Vector2(playerPos.x + i, playerPos.y + j));
            }         
        }
        positions.Remove(playerPos);
        
        //Remove the positions which are already occopied
        foreach(WorldTile tile in spawnedBiomes)
        {
            if (positions.Contains(tile.position))
            {
                positions.Remove(tile.position);
            }
        }

        //Spawn biomes
        foreach(Vector2 pos in positions)
        {            
            SpawnBiome(pos);
        }
    }
    
    void SpawnBiome(Vector2 position)
    {
        //Debug.Log("Spawning biome at " + position);
        validBiomes.Clear();
        weights.Clear();
        foreach(Biome biome in biomes)
        {
            if(biome.minDistance <= position.magnitude && biome.maxDistance >= position.magnitude)
            {
                validBiomes.Add(biome);
                weights.Add(biome.weight);
            }
        }

        //Spawn a biome
        Vector3 pos = new Vector3(position.x * biomeSize, position.y * biomeSize, 0);
        int index = GetRandomWeightedIndex(weights.ToArray());

        Biome newBiome = validBiomes[index];
        WorldTile newTile = new WorldTile(
            newBiome,
            Instantiate(newBiome.prefab, pos, newBiome.prefab.transform.rotation, transform),
            position
            );

        //Add the tile to the list of existing tiles
        spawnedBiomes.Add(newTile);

        //Generate terrain
        GenerateEnvironmtentalObjects(newTile);
    }

    float[] GetWeightArray(Biome[] objects)
    {
        float[] weights = new float[objects.Length];
        for (int i = 0; i < objects.Length; i++)
        {
            weights[i] = objects[i].weight;
        }
        return weights;
    }

    int GetRandomWeightedIndex(float[] weights)
    {
        float weightedSum = 0f;
        foreach (float weight in weights)
        {
            weightedSum += weight;
        }

        int index = 0;
        while(index < weights.Length - 1)
        {
            if(Random.Range(0, weightedSum) < weights[index])
            {
                return index;
            }
            else
            {
                weightedSum -= weights[index++];
            }
        }
        
        return index;
    }

    void GenerateEnvironmtentalObjects(WorldTile tile)
    {
        List<Collider2D> results = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter();
        //For each specified asset, spawn it the specified number of times
        foreach (Biome.EnvironmentalObject asset in tile.biome.randomlyPlacedObjects)
        {
            for (int i = 1; i <= asset.maxQuantity; i++)
            {
                Vector3 position = new Vector3(Random.Range(-biomeSize/2f, biomeSize/2f), Random.Range(-biomeSize / 2f, biomeSize / 2f), 0);
                position += tile.obj.transform.position;
                GameObject newObj = Instantiate(asset.prefab, position, asset.prefab.transform.rotation, tile.obj.transform);

                //Make sure it isn't overlapping. If it is, destroy it
                if(newObj.TryGetComponent(out Collider2D collider))
                {
                    collider.OverlapCollider(filter, results);
                    //Ignore colliders that belong to the same object
                    List<int> ignoredResults = new List<int>();
                    foreach(Collider2D col in results)
                    {
                        if (col.gameObject == collider.gameObject)
                            ignoredResults.Add(results.IndexOf(col));
                    }
                    foreach(int index in ignoredResults)
                    {
                        results.RemoveAt(index);
                    }
                    ignoredResults.Clear();
                    //If there's an overlapping collider, undo the spawn
                    if (results.Count > 0)
                    {
                        Destroy(newObj);
                    }
                }
            }
        }
    }
}
