using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 
/// </summary>
public class InputInit : MonoBehaviour
{
    private InputReaderSO input;
    
    private void Awake()
    {
        input = ScriptableObject.CreateInstance<InputReaderSO>();
    }

     public InputReaderSO GetInput()
    {
        return input;
    }
}
