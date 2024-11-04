using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Reflection;
using System;

public class VarTriger : MonoBehaviour
{
    public bool oneInvoke;
    private bool invoked;
    public UnityEvent invokeEvent;
    public MonoBehaviour selectedScript;
    public FieldInfo value;
    public ValueType typeInfo;
    private void Start()
    {
        
    }

    private void Update()
    {
        if (selectedScript == null || value == null||(oneInvoke==true&&invoked==true))
        {
            this.enabled = false;
            return;
        }
    }
}
