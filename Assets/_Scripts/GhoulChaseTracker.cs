using UnityEngine;
using System;

public class GhoulChaseTracker : MonoBehaviour
{
    public static GhoulChaseTracker Instance;

    public event Action<int> OnChaseCountChanged;

    private int _chasingCount = 0;
    public int ChasingCount => _chasingCount;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RegisterChase()
    {
        _chasingCount++;
        OnChaseCountChanged?.Invoke(_chasingCount);
    }

    public void UnregisterChase()
    {
        _chasingCount = Mathf.Max(0, _chasingCount - 1);
        OnChaseCountChanged?.Invoke(_chasingCount);
    }
}