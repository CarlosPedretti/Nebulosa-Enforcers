using System;
using System.Threading;
using UnityEngine;
using Unity.Netcode;

public class NetworkHealthSystem : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 3;

    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>(default, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Owner);

    private void Start()
    {
        if(IsOwner) CurrentHealth.Value = maxHealth;
    }

    public void Heal(int healAmount)
    {
        CurrentHealth.Value += healAmount;
    }
}