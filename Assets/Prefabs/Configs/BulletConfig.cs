using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletConfig", menuName = "New Bullet Type")]
public class BulletConfig : ScriptableObject
{
    [SerializeField] private string bulletName;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float speed;
    [SerializeField] private int damage;

    public string BulletName { get { return bulletName; } }
    public GameObject BulletPrefab { get { return bulletPrefab; } }
    public float Speed { get { return speed; } }
    public int Damage { get { return damage; } }
}