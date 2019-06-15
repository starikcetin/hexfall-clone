using System.Diagnostics.Contracts;
using UnityEngine;

public class Singleton<T> : MonoBehaviour
    where T : Singleton<T>
{
    public static T Instance { get; private set; }

    /// <summary>
    /// Do not override this in children. Override _Awake() instead.
    /// </summary>
    private void Awake()
    {
        EnsureSingleton();
        _Awake();
    }

    /// <summary>
    /// Do not override this in children. Override _OnDestroy() instead.
    /// </summary>
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }

        _OnDestroy();
    }


    private void EnsureSingleton()
    {
        if (Instance == null)
        {
            Instance = (T) this;
            DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            DestroyImmediate(this);
        }
    }

    /// <summary>
    /// Override this in implementors instead of Awake().
    /// </summary>
    protected virtual void _Awake()
    {
    }

    /// <summary>
    /// Override this in implementors instead of OnDestroy().
    /// </summary>
    protected virtual void _OnDestroy()
    {
    }

    [ContractInvariantMethod]
    private void Invariant()
    {
        // Let's know if we do something ill-advised.
        Contract.Assert(Instance != null && Instance == this);
    }
}
