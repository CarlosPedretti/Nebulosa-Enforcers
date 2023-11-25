using System;
using System.Threading;
using UnityEngine;
using Unity.Netcode;

public class NetworkHealthSystem : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private GameObject gameover_Panel;
    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>(default,
    NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Server);
        
    private void Start()
    {
        gameObject.GetComponent<NetworkHealthSystem>().CurrentHealth.OnValueChanged += HealthChanged;

        if (IsServer) CurrentHealth.Value = maxHealth;
    }
   
    private void HealthChanged(int previousValue, int newValue)
    {
        if (!IsServer) return;

        if (newValue <= 0)
        {
            NetworkObject.Despawn();
            OnGameoverClientRPC();
        }

    }

    [ClientRpc]
    private void OnGameoverClientRPC()
    {
        gameover_Panel.SetActive(true);        
        gameObject.SetActive(false);        
    }
    private void OnDisable()
    {
        gameObject.GetComponent<NetworkHealthSystem>().CurrentHealth.OnValueChanged -= HealthChanged;
    }
}