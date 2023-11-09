using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text health_Text;
    public TMP_Text Health_Text { get { return health_Text; } }
}
