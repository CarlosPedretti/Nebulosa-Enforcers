using System;
using System.Threading;
using UnityEngine;
using Unity.Netcode;

public class NetworkHealthSystem : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 3;

    [SerializeField] private GameObject explosion_ParticleSys;


    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>(default, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Owner);

    private void Start()
    {
        if(IsOwner) CurrentHealth.Value = maxHealth;
    }

    public void Heal(int healAmount)
    {
        CurrentHealth.Value += healAmount;
    }

    private void OnHealthChanged(int oldValue, int newValue)
    {
        if (CurrentHealth.Value <= 0)
        {
            GameObject explosionInstantiated = Instantiate(explosion_ParticleSys, transform.position, transform.rotation);
            explosionInstantiated.GetComponent<NetworkObject>().Spawn();
            if (IsServer)
            {
                DespawnPlayerServerRpc();
            }
        }
    }

    void OnEnable()
    {
        CurrentHealth.OnValueChanged += OnHealthChanged;
    }

    void OnDisable()
    {
        CurrentHealth.OnValueChanged -= OnHealthChanged;

    }


    [ServerRpc]
    private void DespawnPlayerServerRpc()
    {
        DespawnPlayerClientRpc();
    }

    [ClientRpc]
    private void DespawnPlayerClientRpc()
    {
        SpriteRenderer powerUpSprite = GetComponent<SpriteRenderer>();
        powerUpSprite.enabled = false;

        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;
    }

}