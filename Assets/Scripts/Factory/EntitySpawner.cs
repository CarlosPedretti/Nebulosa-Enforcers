using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Unity.Netcode;

public class EntitySpawner : NetworkBehaviour
{
    [SerializeField] private Factory _factory;
    [SerializeField] private Transform[] _transforms;

    public void SpawnEntityCapsule()
    {
        if (!IsOwner) return;
        _factory.Create("Capsule", _transforms[0]);
    }

    public void SpawnEntityCircle()
    {
        if (!IsOwner) return;
        _factory.Create("Circle", _transforms[1]);
    }

    public void SpawnEntitySquare()
    {
        if (!IsOwner) return;
        _factory.Create("Square", _transforms[2]);
    }

}
