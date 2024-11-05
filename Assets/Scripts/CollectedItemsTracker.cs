using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectedItemsTracker : MonoBehaviour
{
    [SerializeField] private int _collectedItems;

    public UnityAction<int> ItemsChanged;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            _collectedItems++;
            OnItemsChanged(_collectedItems);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            _collectedItems--;
            OnItemsChanged(_collectedItems);
        }
    }

    public void OnItemsChanged(int itemNum) => ItemsChanged?.Invoke(itemNum);
}
