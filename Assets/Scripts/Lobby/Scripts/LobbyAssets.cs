using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyAssets : MonoBehaviour {



    public static LobbyAssets Instance { get; private set; }


    [SerializeField] private Sprite asset1Sprite;
    [SerializeField] private Sprite asset2Sprite;
    [SerializeField] private Sprite asset3Sprite;
    [SerializeField] private Sprite asset4Sprite;

    private void Awake() {
        Instance = this;
    }

    public Sprite GetSprite(LobbyManager.PlayerCharacter playerCharacter) {
        switch (playerCharacter) {
            default:
            case LobbyManager.PlayerCharacter.Asset1:   return asset1Sprite;
            case LobbyManager.PlayerCharacter.Asset2:   return asset2Sprite;
            case LobbyManager.PlayerCharacter.Asset3:   return asset3Sprite;
            case LobbyManager.PlayerCharacter.Asset4:   return asset4Sprite;
        }
    }

}