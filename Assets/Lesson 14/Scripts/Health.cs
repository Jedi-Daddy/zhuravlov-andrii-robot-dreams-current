using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health stats")]
    [SerializeField]
    private int _maxHealth = 100;
    private int _currentHealth;

    public event Action<float> HealthChanged;
    
    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = _maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeHealth(-10);
        }
    }

    private void ChangeHealth(int value)
    { 
        _currentHealth = value;
        if (_currentHealth < 0)
        {
            Death();
        }
        else
        {
            float _currentHealthAsPercanrage = (float)_currentHealth / _maxHealth;
            HealthChanged?.Invoke(_currentHealthAsPercanrage);
        }
    }

    private void Death()
    {
        HealthChanged?.Invoke(0);
        Debug.Log("You are death!");
    }
}
