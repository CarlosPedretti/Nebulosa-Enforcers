using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;


public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    public NetworkVariable<int> gameTimer = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public NetworkVariable<bool> showGameCanvas = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [SerializeField] private int waitTime;

    [SerializeField] private Transform gameCanvas;
    [SerializeField] private Transform stadisticsContainer;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Button restartButton;



    private void Awake()
    {
        Instance = this;
    }

    public void StartTimer()
    {
        StartCoroutine(Timer());
    }
    public IEnumerator Timer()
    {
        for (int i = waitTime; i > 0; i--)
        {
            gameTimer.Value = i;
            timerText.text = FormatTime(gameTimer.Value);

            yield return new WaitForSeconds(1f);
        }

        timerText.text = "00:00";

        GameOver();
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void GameOver()
    {
        ShowStadisticsServerRPC();

        if (IsServer)
        {
            restartButton.gameObject.SetActive(true);
        }
    }

    void PlayerWithMoreKills()
    {
        int maxEnemysKilledValue = 0;
        ulong playerWithMoreKillsID = 0;

        foreach (ushort pID in NetworkManager.Singleton.ConnectedClientsIds)
        {
            int currentEnemysKilled = NetworkManager.Singleton.ConnectedClientsList[pID].PlayerObject.GetComponent<PlayerLogic>().enemysKilled.Value;

            if (currentEnemysKilled > maxEnemysKilledValue)
            {
                maxEnemysKilledValue = currentEnemysKilled;
                playerWithMoreKillsID = pID;
            }
        }

        Debug.Log($"El jugador con más enemigos destruidos es: {playerWithMoreKillsID} con {maxEnemysKilledValue} enemigos destruidos.");
    }


    public void HideGameCanvas()
    {
        gameCanvas.gameObject.SetActive(false);
    }

    private void TimerChanged(int oldValue, int newValue)
    {
        timerText.text = FormatTime(newValue);
    }

    void OnEnable()
    {
        gameTimer.OnValueChanged += TimerChanged;
    }

    void OnDisable()
    {
        gameTimer.OnValueChanged -= TimerChanged;

    }

    [ServerRpc]
    public void ShowStadisticsServerRPC()
    {
        ShowStadisticsClientRPC();
    }

    [ClientRpc]
    public void ShowStadisticsClientRPC()
    {
        stadisticsContainer.gameObject.SetActive(true);
        ScoreUI.Instance.UpdateScore();
    }

}
