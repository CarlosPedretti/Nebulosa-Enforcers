using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;

public class PlayerUI : NetworkBehaviour
{
    [SerializeField] private TMP_Text health_Text;
    [SerializeField] private GameObject canvasPlayer;

    void OnEnable()
    {
       gameObject.GetComponent<NetworkHealthSystem>().CurrentHealth.OnValueChanged += HealthChanged;
        health_Text.text =null;
    }
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            canvasPlayer.SetActive(false);
        }
    }
    private void HealthChanged(int previousValue, int newValue)
    {
        health_Text.text = newValue.ToString();
    }

    void OnDisable()
    {
        gameObject.GetComponent<NetworkHealthSystem>().CurrentHealth.OnValueChanged -= HealthChanged;

    }

}
