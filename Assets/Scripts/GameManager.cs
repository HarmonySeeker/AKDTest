using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text ItemsCounterText;
    [SerializeField] private CollectedItemsTracker collectedItemsTracker;

    [SerializeField] private int _winNum;
    [SerializeField] private bool _won = false;

    private void OnEnable()
    {
        collectedItemsTracker.ItemsChanged += ChangeCounter;
    }

    private void OnDisable()
    {
        if (!_won)
        {
            collectedItemsTracker.ItemsChanged -= ChangeCounter;
        }
    }

    private void Start()
    {
        ItemsCounterText.text = $"0 / {_winNum}";
    }

    private void ChangeCounter(int newNum)
    {
        ItemsCounterText.text = $"{newNum} / {_winNum}";

        if (newNum == _winNum)
        {
            Win();
        }
    }

    private void Win()
    {
        ItemsCounterText.text = "You won!";
        collectedItemsTracker.ItemsChanged -= ChangeCounter;
        _won = true;
    }
}
