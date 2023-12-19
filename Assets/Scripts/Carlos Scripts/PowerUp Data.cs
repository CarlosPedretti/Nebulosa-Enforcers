using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp Data", menuName = "PowerUp Data")]
public class PowerUpData : ScriptableObject
{

    public float powerUpDuration;
    public GameObject powerUp;

    [Header("Bullet Config")]
    [Space(15)]

    public bool useDefaultFirePoint = true;

    public GameObject bulletPrefab;

    public float fireRate = 0.5f;

    public List<int> firePointIndices = new List<int>();
    [Space(20)]




    [Header("Heal Config")]
    [Space(15)]

    public bool canHeal  = false;

    public int quantityOfHeal = 1;
    [Space(20)]


    [Header("Rocket Config")]
    [Space(15)]

    public bool canUseRockets = false;

    public GameObject rocketPrefab;

    public float rocketFireRate = 1.0f;

    public List<int> rocketPointsIndices = new List<int>();



}
