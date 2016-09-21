using System;
using UnityEngine;

public class EngineEvents : MonoBehaviour
{
    public event Action OnUpdate;
    public event Action OnStart;
    public event Action OnGameEnd;

    private void Update()
    {
        if (OnUpdate != null)
        {
            OnUpdate.Invoke();
        }
    }

    private void Start()
    {
        if (OnStart != null)
        {
            OnStart.Invoke();
        }
    }

    private void OnDestroy()
    {
        if (OnGameEnd != null)
        {
            OnGameEnd.Invoke();
        }
    }
}