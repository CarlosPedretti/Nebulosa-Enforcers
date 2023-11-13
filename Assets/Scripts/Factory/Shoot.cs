using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Shoot : NetworkBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float bulletDestroyDelay = 3;

    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetButtonDown("Fire1"))
        {
            SpawnBulletServerRPC(spawnPoint.position, spawnPoint.rotation);

        }
    }

    [ServerRpc]
    private void SpawnBulletServerRPC(Vector3 position, Quaternion rotation)
    {
       GameObject bulletInstantiated = Instantiate(bullet, position, rotation);
        bulletInstantiated.GetComponent<NetworkObject>().Spawn();

        StartCoroutine(DestroyBulletAfterDelay(bulletInstantiated));
    }


    private IEnumerator DestroyBulletAfterDelay(GameObject bulletToDestroy)
    {
        yield return new WaitForSeconds(bulletDestroyDelay);

        if (bulletToDestroy != null)
        {
            bulletToDestroy.GetComponent<NetworkObject>().Despawn();
            Destroy(bulletToDestroy);
        }
    }
}
