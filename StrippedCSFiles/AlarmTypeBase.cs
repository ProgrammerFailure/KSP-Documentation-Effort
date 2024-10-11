using System;
using System.Runtime.CompilerServices;
using KSP.UI;
using KSP.UI.Screens;

[Serializable]
public abstract class AlarmTypeBase : AppUI_Data, ICloneable
{
	public string title;

	public string description;

	public double ut;

	public double eventOffset;

	private uint id;

	public string iconURL;

	public string soundURLOverride;

	public int soundRepeatsOverride;

	private bool triggered;

	private bool actioned;

	[AppUI_Label(guiName = "#autoLOC_8003617", order = -1)]
	public string vesselName;

	private AppUIMemberLabel vesselNameLabel;

	private double currentUT;

	private AlarmUIDisplayMode displayMode;

	private AlarmClockMessageDialog dialogObject;

	public uint vesselId;

	private Vessel vessel;

	public AlarmActions actions;

	public uint Id
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool Triggered
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool Actioned
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsAlarmVesselTheAvailableVessel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string TypeName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsMapNodeDefined
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double TimeToAlarm
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public double TimeToEvent
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public bool IsEditing
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsActive
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vessel Vessel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool PauseGame
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool HaltWarp
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool ShowMessage
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AlarmTypeBase()
	{
		throw null;
	}

	public abstract string GetDefaultTitle();

	public abstract bool RequiresVessel();

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual MapObject.ObjectType MapNodeType()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool ShowAlarmMapObject(MapObject mapObject)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool InitializeFromMapObject(MapObject mapObject)
	{
		throw null;
	}

	public abstract bool CanSetAlarm(AlarmUIDisplayMode displayMode);

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual string CannotSetAlarmText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ScenarioUpdate(bool cooldownActive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public sealed override void UIInputPanelUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void UpdateTimeToAlarm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnManeuversLoaded(Vessel vessel, PatchedConicSolver solver)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnScenarioUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnInputPanelUpdate(AlarmUIDisplayMode displayMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public sealed override void UIInputPanelDataChanged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnUIInputPanelDataChanged(AlarmUIDisplayMode displayMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void UIInitialization(AlarmUIDisplayMode displayMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnUIInitialization(AlarmUIDisplayMode displayMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void UIEndInitialization(AlarmUIDisplayMode displayMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnUIEndInitialization(AlarmUIDisplayMode displayMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void TriggerAlarm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnTriggered()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ActionAlarm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnActioned()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public sealed override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnAlarmLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public sealed override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnAlarmSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual AlarmTypeBase CloneAlarm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public object Clone()
	{
		throw null;
	}
}
