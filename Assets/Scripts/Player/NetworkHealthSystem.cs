using System;
using System.Threading;
using UnityEngine;
using Unity.Netcode;

public class NetworkHealthSystem : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 3;
    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>(default, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Owner);

    //[SerializeField] private PlayerHealthBar HealthBar;

    //public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    //public int CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }

    //public static Action<DamageType, float> OnDamage;
    //public static Action Gameover;


    private void Start()
    {
        if(IsOwner) CurrentHealth.Value = maxHealth;
    }

    public void Heal(int healAmount)
    {
        CurrentHealth.Value += healAmount;
    }

}
