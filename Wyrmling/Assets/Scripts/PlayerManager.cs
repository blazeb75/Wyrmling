using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public UnityEvent OnFoodConsumed;
    public UnityEvent OnPlayerGrown;

    [Header("Components (assigned at runtime)")]
    public Growth growth;
    public Mouth mouth;

    public Vector3 headForward;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            throw new System.Exception("Duplicate player manager found");
        }

        growth = GetComponentInChildren<Growth>();
        mouth = GetComponentInChildren<Mouth>();
    }    
}
