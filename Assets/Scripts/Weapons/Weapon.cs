using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : NetworkBehaviour
{
    private PlayerInput playerInput;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private List<Transform> firePoints = new List<Transform>();
    [SerializeField] private float fireRate = 0.5f;
    private float nextFireTime;
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        if (playerInput.actions["Shoot"].IsPressed() && Time.time >= nextFireTime)
        {
            foreach (Transform firePoint in firePoints)
            {
                SpawnBulletServerRPC(firePoint.position, firePoint.rotation);
            }

            nextFireTime = Time.time + fireRate;

        }
    }


    [ServerRpc]
    private void SpawnBulletServerRPC(Vector3 position, Quaternion rotation)
    {

        {
            NetworkObject instansiatedBullet = NetworkObjectPooll.Singleton.GetNetworkObject(bulletPrefab, position, rotation);
            //GameObject instansiatedBullet = Instantiate(bulletPrefab, position, rotation);
            if (!instansiatedBullet.IsSpawned) instansiatedBullet.Spawn(true);
        }


    }
}
