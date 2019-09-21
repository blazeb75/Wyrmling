using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager instance;

    [SerializeField] Biome[] biomes;
    [SerializeField] float biomeSize;
    [SerializeField] List<WorldTile> spawnedBiomes;
    [SerializeField] List<Vector2> visitedPositions;

    List<Biome> validBiomes;

    //WorldTile represents an environment in the world
    [System.Serializable]
    struct WorldTile
    {        
        public string name;
        public GameObject obj;
        public Vector2 position;

        public WorldTile(Biome biome, GameObject instance, Vector2 pos)
        {
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

        validBiomes = new List<Biome>();
        spawnedBiomes = new List<WorldTile>();
        visitedPositions = new List<Vector2>();
        
    }
    private void Start()
    {
        SpawnBiome(new Vector2(0, 0));
        StartCoroutine(CheckPlayerExploration());
    }

    IEnumerator CheckPlayerExploration()
    {
        while (true)
        {
            if (!visitedPositions.Contains(GetPlayerPosition()))
            {
                visitedPositions.Add(GetPlayerPosition());
                SpawnBiomesAroundPlayer();                
            }
            yield return new WaitForSeconds(3f);
        }
    }

    //Get the player location in terms of biomes, where 0,0 is the starting biome and 0,1 is the one immediately above
    Vector2 GetPlayerPosition()
    {
        Vector2 playerPos = PlayerManager.instance.transform.position;
        playerPos.x = Mathf.Floor(playerPos.x / biomeSize);
        playerPos.y = Mathf.Floor(playerPos.y / biomeSize);
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
    
    Biome SpawnBiome(Vector2 position)
    {
        validBiomes.Clear();
        foreach(Biome biome in biomes)
        {
            if(biome.minDistance >= position.magnitude && biome.maxDistance <= position.magnitude)
            {
                validBiomes.Add(biome);
            }
        }

        //Spawn a biome
        Vector3 pos = new Vector3(position.x, position.y, 1);
        Biome newBiome = GetRandomWeightedBiome();
        WorldTile newTile = new WorldTile(
            newBiome,
            Instantiate(newBiome.prefab, pos, newBiome.prefab.transform.rotation, transform),
            pos
            );
        spawnedBiomes.Add(newTile);
        return newBiome;
    }

    Biome GetRandomWeightedBiome()
    {
        float weightedSum = 0f;
        foreach (Biome biome in validBiomes)
        {
            weightedSum += biome.weight;
        }

        int index = 0;
        while(index < validBiomes.Count - 1)
        {
            if(Random.Range(0,weightedSum) < validBiomes[index].weight)
            {
                return validBiomes[index];
            }
            else
            {
                weightedSum -= validBiomes[index].weight;
            }
        }

        //There is only one item left; return it
        return validBiomes[0];
    }
}
