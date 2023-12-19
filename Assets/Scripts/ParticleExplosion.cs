using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ParticleExplosion : NetworkBehaviour
{
   
    public void OnParticleSystemStopped()
    {
        if(!IsServer) return;
        NetworkObject.Despawn(true);
    }
}