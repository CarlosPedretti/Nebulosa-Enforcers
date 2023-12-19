using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine.UI;
using Unity.Netcode;

public class ScorePlayerSingleUI : MonoBehaviour {


    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI playerKillsCount;
    [SerializeField] private TextMeshProUGUI playerPointsCount;
    [SerializeField] private Image characterImage;

    private PlayerLogic playerLogic;

    public void UpdatePlayer(PlayerLogic playerLogic)
    {
        playerKillsCount.text = playerLogic.enemysKilled.Value.ToString();
        playerPointsCount.text = playerLogic.pointsEarned.Value.ToString();
        characterImage.sprite = playerLogic.currentSprite;
    }


}