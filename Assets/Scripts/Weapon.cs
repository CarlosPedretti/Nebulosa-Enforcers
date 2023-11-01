using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : NetworkBehaviour
{
    private PlayerInput playerInput;
    private bool IsShooting;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] List<Transform> firePoints = new List<Transform>();
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        if (playerInput.actions["Shoot"].IsPressed())
        {
            foreach (Transform firePoint in firePoints)
            {
                SpawnBulletServerRPC(firePoint.position, firePoint.rotation);
            }

            Debug.Log("IsShooting: " + IsShooting);
        }

    }


    [ServerRpc]
    private void SpawnBulletServerRPC(Vector3 position, Quaternion rotation)
    {

        {
            GameObject instansiatedBullet = Instantiate(bulletPrefab, position, rotation);
            instansiatedBullet.GetComponent<NetworkObject>().Spawn();
        }


    }
}
