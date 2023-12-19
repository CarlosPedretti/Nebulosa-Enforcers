using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : NetworkBehaviour
{
    private PlayerInput playerInput;

    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public GameObject rocketPrefab;

    [SerializeField] public List<Transform> firePoints = new List<Transform>();
    [SerializeField] public List<Transform> firePointsAvaible = new List<Transform>();

    [SerializeField] public bool canUseRockets;

    [SerializeField] public List<Transform> rocketPoints = new List<Transform>();
    [SerializeField] public List<Transform> rocketPointsAvaible = new List<Transform>();

    [SerializeField] public float fireRate = 0.5f;
    [SerializeField] public float rocketFireRate = 0.9f;

    private PlayerLogic playerLogic;

    private float nextFireTime;
    private float nextRocketTime;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        playerLogic = GetComponent<PlayerLogic>();
    }

    void Update()
    {
        if (!IsOwner) return;

        if (playerInput.actions["Shoot"].IsPressed() && Time.time >= nextFireTime)
        {
            foreach (Transform firePoint in firePoints)
            {
                SpawnBulletServerRPC(firePoint.position, firePoint.rotation, playerLogic.playerID);
            }

            nextFireTime = Time.time + fireRate;

        }

        if (playerInput.actions["Rocket"].IsPressed() && Time.time >= nextRocketTime && canUseRockets)
        {
            foreach (Transform rocketPoint in rocketPoints)
            {
                SpawnRocketServerRPC(rocketPoint.position, rocketPoint.rotation, playerLogic.playerID);
            }

            nextRocketTime = Time.time + rocketFireRate;
        }
    }


    [ServerRpc]

    private void SpawnBulletServerRPC(Vector3 position, Quaternion rotation, int pID)
    {
        {
            NetworkObject instansiatedBullet = NetworkObjectPool.Singleton.GetNetworkObject(bulletPrefab, position, rotation);
            if (!instansiatedBullet.IsSpawned) instansiatedBullet.Spawn(true);

            instansiatedBullet.GetComponent<Bullet>().GetShooterId(pID);
        }
    }

    [ServerRpc]
    private void SpawnRocketServerRPC(Vector3 position, Quaternion rotation, int pID)
    {
        {
            NetworkObject instansiatedBullet = NetworkObjectPool.Singleton.GetNetworkObject(rocketPrefab, position, rotation);
            if (!instansiatedBullet.IsSpawned) instansiatedBullet.Spawn(true);

            instansiatedBullet.GetComponent<Missile>().GetShooterId(pID);
        }
    }
}