using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointAtNearestEnemy : MonoBehaviour
{
    Image image;
    public float baseRadius = 30;

    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    private void Update()
    {
        //Get the nearest enemy to the center of the screen
        GameObject enemy = GetClosestEnemy(GetEnemiesInRange(baseRadius));
        //Disable if there is no enemy or if the nearest enemy is on screen
        if (enemy != null && !enemy.GetComponent<Renderer>().isVisible)
        {
            image.enabled = true;
            //Get the direction to the enemy
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Vector2.zero);
            worldPos.z = 0f;
            Vector3 dir = enemy.transform.position - worldPos;
            dir.Normalize();
            //Position this object near the edge of the screen in that direction
            Vector3 newPos = new Vector3
            (
                dir.x * Screen.width * 0.95f,
                dir.y * Screen.height * 0.95f,
                0
            );
            rectTransform.anchoredPosition = newPos;
            rectTransform.up = dir;
        }
        else
        {
            image.enabled = false;
        }
    }
    ///<summary> Gets the closest enemy to the center of the screen from an array of enemies.</summary>
    GameObject GetClosestEnemy(GameObject[] enemies)
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Vector3.zero);
        foreach (GameObject potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }

    ///<summary> Gets all of the enemies within a certain distance of the player.
    ///If none are found, it recursively checks a larger area, up to five times the initial size</summary>
    GameObject[] GetEnemiesInRange(float radius)
    {
        List<GameObject> enemies = new List<GameObject>();
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(PlayerManager.instance.transform.position, radius, LayerMask.GetMask("Creature"));
        //Get the GameObject associated with the found colliders
        foreach(Collider2D enemy in enemyColliders)
        {
            enemies.Add(enemy.gameObject);
        }
        
        if(enemies.Count == 0 && radius < baseRadius * 5)
        {
            return GetEnemiesInRange(radius * 1.5f);
        }
        return enemies.ToArray();
    }
}
