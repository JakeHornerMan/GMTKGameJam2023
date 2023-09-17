using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IconOverflow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI overflowCount;

    [HideInInspector] public List<ObjectInfo> overflowObjects;

    private void Start()
    {
        overflowCount.text = $"+{overflowObjects.Count}";
    }
}
