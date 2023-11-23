using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class Factory : NetworkBehaviour
{

    [SerializeField] private Entity[] _entitys;

    private Dictionary<string, Entity> _idToEntity;

    [SerializeField] private int destroyDelay;



    private void Awake()
    {
        _idToEntity = new Dictionary<string, Entity>();

        foreach (var entity in _entitys) 
        {
            _idToEntity.Add(entity.Id, entity);
        }
    }

    [ServerRpc]
    void SpawnEntityServerRpc(string id, Vector3 position)
    {
        if (!_idToEntity.TryGetValue(id, out var _entity))
        {
            Debug.Log("Object with id: " + id + " does not exist");
            return;
        }

        var entityInstantiated = Instantiate(_entity, position, Quaternion.identity);
        entityInstantiated.GetComponent<NetworkObject>().Spawn();


        StartCoroutine(DestroyEntityAfterDelay(entityInstantiated));
    }

    public void Create(string id, Transform entityPosition)
    {
        if (IsServer)
        {
            SpawnEntityServerRpc(id, entityPosition.position);
        }
        else
        {
            Debug.Log("Create method should only be called on the server.");
        }
    }

    private IEnumerator DestroyEntityAfterDelay(Entity entityToDestroy)
    {
        yield return new WaitForSeconds(destroyDelay);

        if (entityToDestroy != null)
        {
            entityToDestroy.GetComponent<NetworkObject>().Despawn();
            Destroy(entityToDestroy);
        }
    }


}
