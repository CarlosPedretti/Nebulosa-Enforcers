using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkObjectPool : NetworkBehaviour
{
    public static NetworkObjectPool Singleton { get; private set; }

    Dictionary<int, List<GameObject>> poolDictionary = new Dictionary<int, List<GameObject>>();

    [SerializeField] private List<ObjectsToPool> poolPrefab = new List<ObjectsToPool>();

    public void Awake()
    {
        if (Singleton != null && Singleton != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Singleton = this;
        }
    }

    public override void OnNetworkSpawn()
    {
        InitializePool();
    }

    public override void OnDestroy()
    {
        if (Singleton == this)
        {
            Singleton = null;
        }

        base.OnDestroy();
    }
    private void InitializePool()
    {
        foreach (ObjectsToPool typePrefab in poolPrefab)
        {
            CreatePool(typePrefab.objectPrefab, typePrefab.poolSize, typePrefab.container);
        }
    }

    public void CreatePool(GameObject prefab, int poolSize, Transform container = null)
    {
        int poolKey = prefab.GetInstanceID();

        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new List<GameObject>());

            for (int i = 0; i < poolSize; i++)
            {
                var newObject = Instantiate(prefab, container);
                newObject.SetActive(false);
                poolDictionary[poolKey].Add(newObject);
            }
        }
    }
    public GameObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        int poolKey = prefab.GetInstanceID();

        if (poolDictionary.ContainsKey(poolKey))
        {
            List<GameObject> pool = poolDictionary[poolKey];

            for (int i = 0; i < pool.Count; i++)
            {
                if (!pool[i].gameObject.activeInHierarchy)
                {
                    pool[i].SetActive(true);
                    pool[i].transform.position = position;
                    pool[i].transform.rotation = rotation;
                    return pool[i];
                }
            }
            GameObject newObject = Instantiate(prefab);
            newObject.SetActive(true);
            newObject.transform.position = position;
            newObject.transform.rotation = rotation;

            poolDictionary[poolKey].Add(newObject);
            return newObject;
        }
        return null;
    }
}

[System.Serializable]
public class ObjectsToPool
{
    public GameObject objectPrefab;
    public Transform container;
    public int poolSize;
}