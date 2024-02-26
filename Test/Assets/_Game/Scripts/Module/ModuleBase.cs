using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public enum ModuleStatus
{
    None,
    Uninitialized,
    Initialized,
    PostInitialized,
    Active,
    Deactivated,
    Pause,
    Reactivated,
}

[Serializable]
public class ModuleEvents
{
    public UnityAction<ModuleStatus> OnBeforeStatus;
    public UnityAction<ModuleStatus> OnAfterStatus;
        
    public void ClearAllEvents()
    {
        OnBeforeStatus = null;
        OnAfterStatus = null;
    }
}

public class ModuleBase : SerializedMonoBehaviour
{
    [field: TabGroup("Module Status", "Module Status")]
    [field: SerializeField]
    public ModuleStatus Status { get; protected set; }

    [field: TabGroup("Module Status", "Module Events")]
    [field: SerializeField]
    public ModuleEvents ModuleEvents { get; private set; }

    public virtual IEnumerator IE_SwitchStatus(ModuleStatus moduleStatus)
    {
        switch (moduleStatus)
        {
            case ModuleStatus.Uninitialized:
                yield return IE_Deinitialize();
                break;
            case ModuleStatus.Initialized:
                yield return IE_Initialize();
                break;
            case ModuleStatus.PostInitialized:
                yield return IE_PostInitialize();
                break;
            case ModuleStatus.Active:
                yield return IE_Activate();
                break;
            case ModuleStatus.Deactivated:
                yield return IE_Deactivate();
                break;
            case ModuleStatus.Pause:
                yield return IE_Pause();
                break;
            case ModuleStatus.Reactivated:
                yield return IE_ReActivate();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(moduleStatus), moduleStatus, null);
        }
    }

    public virtual IEnumerator IE_Initialize()
    {
        ModuleEvents.OnBeforeStatus?.Invoke(ModuleStatus.Initialized);
        Status = ModuleStatus.Initialized;
        ModuleEvents.OnAfterStatus?.Invoke(Status);
        yield break;
    }

    public virtual IEnumerator IE_PostInitialize()
    {
        ModuleEvents.OnBeforeStatus?.Invoke(ModuleStatus.PostInitialized);
        Status = ModuleStatus.PostInitialized;
        ModuleEvents.OnAfterStatus?.Invoke(Status);
        yield break;
    }

    public virtual IEnumerator IE_Deinitialize()
    {
        ModuleEvents.OnBeforeStatus?.Invoke(ModuleStatus.Uninitialized);
        Status = ModuleStatus.Uninitialized;

        ModuleEvents.OnAfterStatus?.Invoke(Status);
        yield break;
    }

    public virtual IEnumerator IE_Activate()
    {
        ModuleEvents.OnBeforeStatus?.Invoke(ModuleStatus.Active);
        Status = ModuleStatus.Active;
        ModuleEvents.OnAfterStatus?.Invoke(Status);
        yield break;
    }

    public virtual IEnumerator IE_ReActivate()
    {
        ModuleEvents.OnBeforeStatus?.Invoke(ModuleStatus.Reactivated);
        Status = ModuleStatus.Reactivated;
        ModuleEvents.OnAfterStatus?.Invoke(Status);
        yield return null;
        Status = ModuleStatus.Active;
    }

    public virtual IEnumerator IE_Deactivate()
    {
        ModuleEvents.OnBeforeStatus?.Invoke(ModuleStatus.Deactivated);
        Status = ModuleStatus.Deactivated;
        ModuleEvents.OnAfterStatus?.Invoke(Status);
        yield break;
    }

    public virtual IEnumerator IE_Pause()
    {
        ModuleEvents.OnBeforeStatus?.Invoke(ModuleStatus.Pause);
        Status = ModuleStatus.Pause;
        ModuleEvents.OnAfterStatus?.Invoke(Status);
        yield break;
    }

    public virtual bool IsActive()
    {
        return Status == ModuleStatus.Active;
    }

    protected virtual void Update()
    {
        if (!IsActive()) return;
    }

    protected virtual void FixedUpdate()
    {
        if (!IsActive()) return;
    }

    protected virtual void LateUpdate()
    {
        if (!IsActive()) return;
    }
}
