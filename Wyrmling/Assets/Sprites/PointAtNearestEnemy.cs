using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointAtNearestEnemy : MonoBehaviour
{
    Image image;
    public GameObject debug;
    public float baseRadius = 30;

    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    private void Update()
    {
        image.enabled = true;
        GameObject enemy = GetClosestEnemy(GetEnemiesInRange(baseRadius));
        debug = enemy;
        if (enemy != null && !enemy.GetComponent<Renderer>().isVisible)
        {
            rectTransform.anchoredPosition = Vector2.zero;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(rectTransform.anchoredPosition);
            worldPos.z = 0f;
            Vector3 dir = enemy.transform.position - worldPos;
            dir.Normalize();
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

    GameObject[] GetEnemiesInRange(float radius)
    {
        List<GameObject> enemies = new List<GameObject>();
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(PlayerManager.instance.transform.position, radius, LayerMask.GetMask("Creature"));
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
