using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text health_Text;

    void OnEnable()
    {
        GetComponent<NetworkHealthSystem>().CurrentHealth.OnValueChanged += HealthChanged;
    }

    private void HealthChanged(int previousValue, int newValue)
    {
        health_Text.text = newValue.ToString();
    }

    void OnDisable()
    {
        GetComponent<NetworkHealthSystem>().CurrentHealth.OnValueChanged -= HealthChanged;

    }
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
}
