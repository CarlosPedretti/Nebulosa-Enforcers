using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "New Enemy Type")]
public class EnemyConfig : ScriptableObject
{
    [SerializeField] private string enemyName;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int health;
    [SerializeField] private float speed;

    public string EnemyName { get { return enemyName; } }
    public GameObject EnemyPrefab { get { return enemyPrefab; } }
    public int Health { get { return health; } }
    public float Speed { get { return speed; } }
}