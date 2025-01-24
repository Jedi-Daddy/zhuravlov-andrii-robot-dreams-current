using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson2 : MonoBehaviour
{
    [SerializeField] private int _firstIntegerNumber;
    [SerializeField] private int _secondIntegerNumber;
    [SerializeField] private float _firstFloatNumber;
    [SerializeField] private string _firstText;
    [SerializeField] private bool _firstCheck;

    [ContextMenu("Hello world")]
    private void HelloWorld()
    {
        Debug.Log("Hello world");
    }

    [ContextMenu("Add")]
    private void Add()
    {
        int result = _firstIntegerNumber + _secondIntegerNumber;
        Debug.Log(result);
    }
}
