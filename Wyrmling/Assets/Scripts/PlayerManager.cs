using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public UnityEvent OnFoodConsumed;
    public UnityEvent OnPlayerGrown;

    [Header("Objects - Assign in inspector")]
    public GameObject nose;

    [Header("Components (assigned at runtime)")]
    public Growth growth;
    public Mouth mouth;
    public Movement movement;

    [Header("Dynamic references")]
    public Vector3 headForward;
    public GameObject target;

    public Vector3 TargetPosition
    {
        get
        {
            if (target == null)
            {
                return InputManager.instance.lastMousePosition;
            }
            else
            {
                return target.transform.position;
            }
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
            throw new System.Exception("Duplicate player manager found");
        }

        growth = GetComponentInChildren<Growth>();
        mouth = GetComponentInChildren<Mouth>();
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        CheckCursorTarget();
    }

    //If the cursor is over a creature, store it
    void CheckCursorTarget()
    {
        if (InputManager.instance.GetKey("press"))
        {
            int layerMask = LayerMask.GetMask("Creature", "Food"); 
            RaycastHit2D hit = Physics2D.Raycast(Mouse.Position, Vector2.zero, Mathf.Infinity, layerMask);
            //Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, layerMask);
            if (hit.collider != null)
            {
                target = hit.collider.gameObject;
                return;
            }
            else
            {
                target = null;
            }

        }
       
    }
}
