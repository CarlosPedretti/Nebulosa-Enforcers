using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerLogic : NetworkBehaviour
{
    public static PlayerLogic Instance { get; private set; }

    [SerializeField] public Sprite currentSprite;

    public int playerID;
    public string playerName;

    public NetworkVariable<int> enemysKilled = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> pointsEarned = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            currentSprite = spriteRenderer.sprite;
        }
    }

}
