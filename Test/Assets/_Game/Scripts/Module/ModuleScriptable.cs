using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Game.ModuleSystem.ModuleScriptable
{
    public abstract class ModuleScriptable : SerializedScriptableObject
    {
        [field: TabGroup("Module Status", "Module Status")]
        [field: SerializeField]
        public ModuleStatus Status { get; private set; }

        [field: TabGroup("Module Status", "Module Events")]
        [field: SerializeField]
        public ModuleEvents ModuleEvents { get; private set; }

        protected virtual void Awake()
        { }

        protected virtual void OnEnable()
        {
            Status = ModuleStatus.Uninitialized;
        }

        public void ForceSetStatus(ModuleStatus newStatus)
        {
            ModuleEvents.OnBeforeStatus?.Invoke(newStatus);
            Status = newStatus;
            ModuleEvents.OnAfterStatus?.Invoke(Status);
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

        public virtual IEnumerator IE_Deactivate()
        {
            ModuleEvents.OnBeforeStatus?.Invoke(ModuleStatus.Deactivated);
            Status = ModuleStatus.Deactivated;
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

        public virtual bool IsActive()
        {
            return Status == ModuleStatus.Active;
        }

        public virtual void Update()
        {
            if (!IsActive()) return;
        }

        public virtual void FixedUpdate()
        {
            if (!IsActive()) return;
        }

        public virtual void LateUpdate()
        {
            if (!IsActive()) return;
        }
    }
}