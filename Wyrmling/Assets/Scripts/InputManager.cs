﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    [Header("Inspector Variables")]
    [SerializeField] ControlBindings pcControls;
    [SerializeField] ControlBindings mobileControls;

    ControlBindings controls;
    [Header("Debugging variables, do not edit")]
    public Vector3 lastMousePosition = new Vector3(0, 5, 0);

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            throw new System.Exception("Duplicate input manager found");
        }

        //Set controls for current platform
        if (Application.isMobilePlatform)
        {
            controls = mobileControls;
        }
        else
        {
            controls = pcControls;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(controls.press))
        {
                lastMousePosition = Mouse.Position();            
        }
    }
    
    //Returns Input.GetKey based on the name of a keybinding rather than the name of the key itself
    public bool GetKey(string key)
    {
        return Input.GetKey((KeyCode)controls.GetType().GetField(key).GetValue(controls));
    }
}
