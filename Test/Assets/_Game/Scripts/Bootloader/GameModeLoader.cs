using System.Collections;
using Game.ModuleSystem.ModuleScriptable;
using UnityEngine;


public class GameModeLoader : ModuleScriptable
{
    public GameModes gameMode;
    

    public virtual IEnumerator SwitchModuleStatus(ModuleStatus moduleStatus)
    {
        yield return moduleStatus.IE_SwitchAllManagerStatus();
    }
    
    
    protected override void Awake()
    {
        base.Awake();
    }
    public override IEnumerator IE_Initialize()
    {
        return base.IE_Initialize();
    }
    public override IEnumerator IE_PostInitialize()
    {
        UIAccess.DisableAll(Enum_UI_Canvas.Canvas_Start);
        return base.IE_PostInitialize();
    }
    public override IEnumerator IE_Deinitialize()
    {
        return base.IE_Deinitialize();
    }
    public override IEnumerator IE_Activate()
    {
        UIAccess.DisableAll(Enum_UI_Canvas.Canvas_GamePlay);
        return base.IE_Activate();
    }
    public override IEnumerator IE_Deactivate()
    {
        return base.IE_Deactivate();
    }
    public override bool IsActive()
    {
        return base.IsActive();
    }
    public override void Update()
    {
        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public override void LateUpdate()
    {
        base.LateUpdate();
    }
}
