using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageDoor : MonoBehaviour
{
    [SerializeField] private float upDistance;
    [SerializeField] private float moveDuration;
    [SerializeField] private float scaleDuration;

    private void Start()
    {
        transform.DOMove(transform.position + transform.up * upDistance, moveDuration);
        transform.DOScaleY(0, scaleDuration);
    }
}
