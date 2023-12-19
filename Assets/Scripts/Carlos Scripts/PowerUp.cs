using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PowerUp : NetworkBehaviour
{

    [SerializeField] private PowerUpData powerUpData;

     private GameObject originalBulletPrefab;
     private GameObject originalRocketPrefab;

     private float originalFireRate;
     private float originalRocketFireRate;

     private List<Transform> originalFirePoints = new List<Transform>();
     private List<Transform> originalRocketPoints = new List<Transform>();

     private bool originalCanUseRockets;


    private void Update()
    {
        if (IsServer)
        {
            float yBottom = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)).y;

            if (transform.position.y <= yBottom - 10)
            {
                DespawnPowerUpServerRpc();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //PlayerLogic playerLogic = collision.gameObject.GetComponent<PlayerLogic>();
        NetworkHealthSystem healthSystem = collision.gameObject.GetComponent<NetworkHealthSystem>();

        Weapon weapon = collision.gameObject.GetComponent<Weapon>();

        if (weapon != null)
        {
            StoreOriginalValues(weapon);

            EstablishPowerUpValues(weapon);

            if (healthSystem != null)
            {
                PowerUpApplyHeal(healthSystem);
            }

            StartCoroutine(RevertChanges(weapon));

            Debug.Log("FUNCIONES EJECUTADAS");
            
        }
    }

    private void StoreOriginalValues(Weapon weapon)
    {
        if (!powerUpData.useDefaultFirePoint)
        {
            originalBulletPrefab = weapon.bulletPrefab;
            originalFireRate = weapon.fireRate;
            originalFirePoints.Clear();
            originalFirePoints.AddRange(weapon.firePoints);
        }

        if (powerUpData.canUseRockets)
        {
            originalCanUseRockets = weapon.canUseRockets;
            originalRocketPrefab = weapon.rocketPrefab;
            originalRocketFireRate = weapon.rocketFireRate;
            originalRocketPoints.Clear();
            originalRocketPoints.AddRange(weapon.rocketPoints);
        }
    }

    private void EstablishPowerUpValues(Weapon weapon)
    {
        if (!powerUpData.useDefaultFirePoint)
        {
            weapon.bulletPrefab = powerUpData.bulletPrefab;
            weapon.fireRate = powerUpData.fireRate;
            weapon.firePoints.Clear();
            weapon.firePoints = GetFirePointsFromIndices(weapon.firePointsAvaible, powerUpData.firePointIndices);
        }

        if (powerUpData.canUseRockets)
        {
            weapon.canUseRockets = powerUpData.canUseRockets;
            weapon.rocketPrefab = powerUpData.rocketPrefab;
            weapon.rocketFireRate = powerUpData.rocketFireRate;
            weapon.rocketPoints.Clear();
            weapon.rocketPoints = GetRocketPointsFromIndices(weapon.rocketPointsAvaible, powerUpData.rocketPointsIndices);
        }
    }

    public void RestoreOriginalValues(Weapon weapon)
    {
        Debug.Log("RestoreOriginalValues ejecutado");
        if (!powerUpData.useDefaultFirePoint)
        {
            weapon.bulletPrefab = originalBulletPrefab;
            weapon.fireRate = originalFireRate;
            weapon.firePoints.Clear();
            weapon.firePoints.AddRange(originalFirePoints);
        }

        if (powerUpData.canUseRockets)
        {
            weapon.canUseRockets = originalCanUseRockets;
            weapon.rocketPrefab = originalRocketPrefab;
            weapon.rocketFireRate = originalRocketFireRate;
            weapon.rocketPoints.Clear();
            weapon.rocketPoints.AddRange(originalRocketPoints);
        }
    }

    private void PowerUpApplyHeal(NetworkHealthSystem healthSystem)
    {
        if (powerUpData.canHeal)
        {
            Debug.Log("Vida actual = " + healthSystem.CurrentHealth.Value);
            healthSystem.Heal(powerUpData.quantityOfHeal);
            Debug.Log("Vida actual despues de curar = " + healthSystem.CurrentHealth.Value);
        }
    }



    private IEnumerator RevertChanges(Weapon weapon)
    {
        SpriteRenderer powerUpSprite = GetComponent<SpriteRenderer>();
        powerUpSprite.enabled = false;

        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;

        Debug.Log("RevertChanges EJECUTADO!");
        yield return new WaitForSeconds(powerUpData.powerUpDuration);


        RestoreOriginalValues(weapon);

        if (IsServer)
        {
            DespawnPowerUpServerRpc();
        }
    }



    private List<Transform> GetFirePointsFromIndices(List<Transform> firePointAvaible, List<int> firePointIndices)
    {
        List<Transform> firePoints = new List<Transform>(firePointIndices.Count);

        for (int i = 0; i < firePointIndices.Count; i++)
        {
            int index = firePointIndices[i];
            if (index >= 0 && index < firePointAvaible.Count)
            {
                firePoints.Add(firePointAvaible[index]);
            }
        }

        return firePoints;
    }

    private List<Transform> GetRocketPointsFromIndices(List<Transform> rocketPointAvaible, List<int> rocketPointIndices)
    {
        List<Transform> rocketPoints = new List<Transform>(rocketPointIndices.Count);

        for (int i = 0; i < rocketPointIndices.Count; i++)
        {
            int index = rocketPointIndices[i];
            if (index >= 0 && index < rocketPointAvaible.Count)
            {
                rocketPoints.Add(rocketPointAvaible[index]);
            }
        }

        return rocketPoints;
    }


    [ServerRpc]
    private void DespawnPowerUpServerRpc()
    {
        NetworkObject.Despawn();
        NetworkObjectPool.Singleton.ReturnNetworkObject(NetworkObject, powerUpData.powerUp);

    }


}
