using System;
using System.Reflection;
using ns11;
using ns2;
using ns9;
using UnityEngine;

[Serializable]
public abstract class AlarmTypeBase : AppUI_Data, ICloneable
{
	public string title = "";

	public string description = "";

	public double ut;

	public double eventOffset;

	public uint id;

	public string iconURL = "default";

	public string soundURLOverride = "";

	public int soundRepeatsOverride = -1;

	public bool triggered;

	public bool actioned;

	[AppUI_Label(guiName = "#autoLOC_8003617", order = -1)]
	public string vesselName = "";

	public AppUIMemberLabel vesselNameLabel;

	public double currentUT;

	public AlarmUIDisplayMode displayMode;

	public AlarmClockMessageDialog dialogObject;

	public uint vesselId;

	public Vessel vessel;

	public AlarmActions actions;

	public uint Id => id;

	public bool Triggered => triggered;

	public bool Actioned => actioned;

	public bool IsAlarmVesselTheAvailableVessel
	{
		get
		{
			if (AlarmClockScenario.IsVesselAvailable)
			{
				return AlarmClockScenario.AvailableVessel.persistentId == vesselId;
			}
			return false;
		}
	}

	public string TypeName => GetType().Name;

	public bool IsMapNodeDefined => MapNodeType() != MapObject.ObjectType.Null;

	public double TimeToAlarm { get; set; }

	public double TimeToEvent { get; set; }

	public bool IsEditing => uiPanel != null;

	public bool IsActive
	{
		get
		{
			AlarmTypeBase alarm;
			return AlarmClockScenario.TryGetAlarm(id, out alarm);
		}
	}

	public Vessel Vessel
	{
		get
		{
			FlightGlobals.FindVessel(vesselId, out vessel);
			return vessel;
		}
	}

	public bool PauseGame => actions.warp == AlarmActions.WarpEnum.PauseGame;

	public bool HaltWarp => actions.warp == AlarmActions.WarpEnum.KillWarp;

	public bool ShowMessage
	{
		get
		{
			if (actions.message != AlarmActions.MessageEnum.Yes)
			{
				if (actions.message == AlarmActions.MessageEnum.YesIfOtherVessel)
				{
					if (!(FlightGlobals.ActiveVessel == null))
					{
						return FlightGlobals.ActiveVessel.persistentId != vesselId;
					}
					return true;
				}
				return false;
			}
			return true;
		}
	}

	public AlarmTypeBase()
	{
		id = AlarmClockScenario.GetUniqueAlarmID();
		vesselId = 0u;
		actions = new AlarmActions();
	}

	public abstract string GetDefaultTitle();

	public abstract bool RequiresVessel();

	public virtual MapObject.ObjectType MapNodeType()
	{
		return MapObject.ObjectType.Null;
	}

	public virtual bool ShowAlarmMapObject(MapObject mapObject)
	{
		return false;
	}

	public virtual bool InitializeFromMapObject(MapObject mapObject)
	{
		return true;
	}

	public abstract bool CanSetAlarm(AlarmUIDisplayMode displayMode);

	public virtual string CannotSetAlarmText()
	{
		return Localizer.Format("#autoLOC_8003573");
	}

	public void ScenarioUpdate(bool cooldownActive)
	{
		UpdateTimeToAlarm();
		if (!cooldownActive)
		{
			OnScenarioUpdate();
			UpdateTimeToAlarm();
		}
	}

	public sealed override void UIInputPanelUpdate()
	{
		if (vesselNameLabel != null)
		{
			vesselNameLabel.gameObject.SetActive(Vessel != null);
			if (Vessel != null && vesselName != Vessel.GetDisplayName())
			{
				vesselName = Vessel.GetDisplayName();
				if (uiPanel != null)
				{
					uiPanel.RefreshUI();
				}
			}
		}
		OnInputPanelUpdate(displayMode);
		UpdateTimeToAlarm();
	}

	public void UpdateTimeToAlarm()
	{
		currentUT = Planetarium.GetUniversalTime();
		TimeToAlarm = ut - currentUT;
		TimeToEvent = ut + eventOffset - currentUT;
	}

	public virtual void OnManeuversLoaded(Vessel vessel, PatchedConicSolver solver)
	{
	}

	public virtual void OnScenarioUpdate()
	{
	}

	public virtual void OnInputPanelUpdate(AlarmUIDisplayMode displayMode)
	{
	}

	public sealed override void UIInputPanelDataChanged()
	{
		UpdateTimeToAlarm();
		OnUIInputPanelDataChanged(displayMode);
	}

	public virtual void OnUIInputPanelDataChanged(AlarmUIDisplayMode displayMode)
	{
	}

	public void UIInitialization(AlarmUIDisplayMode displayMode)
	{
		this.displayMode = displayMode;
		OnUIInitialization(displayMode);
	}

	public virtual void OnUIInitialization(AlarmUIDisplayMode displayMode)
	{
	}

	public void UIEndInitialization(AlarmUIDisplayMode displayMode)
	{
		if (uiPanel != null)
		{
			vesselNameLabel = (AppUIMemberLabel)uiPanel.GetControl("vesselName");
		}
		OnUIEndInitialization(displayMode);
	}

	public virtual void OnUIEndInitialization(AlarmUIDisplayMode displayMode)
	{
	}

	public void TriggerAlarm()
	{
		triggered = true;
		bool flag = false;
		if (ShowMessage)
		{
			dialogObject = AlarmClockMessageDialog.Spawn(this, ActionAlarm);
			flag = dialogObject != null;
		}
		if (actions.playSound && !AlarmClockScenario.PlaySound(soundURLOverride, soundRepeatsOverride))
		{
			string text = ((soundURLOverride != "") ? soundURLOverride : AlarmClockScenario.Instance.settings.soundName);
			Debug.LogWarning("[AlarmTypeBase]: Couldnt play sound for " + title + ":" + text);
		}
		switch (actions.warp)
		{
		default:
			TimeWarp.fetch.CancelAutoWarp();
			TimeWarp.SetRate(0, instant: true);
			break;
		case AlarmActions.WarpEnum.PauseGame:
			TimeWarp.fetch.CancelAutoWarp();
			TimeWarp.SetRate(0, instant: true);
			if (ShowMessage)
			{
				FlightDriver.SetPause(pauseState: true);
			}
			break;
		case AlarmActions.WarpEnum.DoNothing:
			break;
		}
		OnTriggered();
		GameEvents.onAlarmTriggered.Fire(this);
		if (!flag)
		{
			ActionAlarm();
		}
	}

	public virtual void OnTriggered()
	{
	}

	public void ActionAlarm()
	{
		actioned = true;
		if (actions.deleteWhenDone)
		{
			AlarmClockScenario.DeleteAlarm(id);
		}
		if (PauseGame && ShowMessage)
		{
			FlightDriver.SetPause(pauseState: false);
		}
		GameEvents.onAlarmActioned.Fire(this);
		OnActioned();
	}

	public virtual void OnActioned()
	{
	}

	public sealed override void OnLoad(ConfigNode node)
	{
		if (!node.TryGetValue("id", ref id))
		{
			id = AlarmClockScenario.GetUniqueAlarmID();
		}
		node.TryGetValue("title", ref title);
		node.TryGetValue("description", ref description);
		node.TryGetValue("ut", ref ut);
		node.TryGetValue("eventOffset", ref eventOffset);
		node.TryGetValue("triggered", ref triggered);
		node.TryGetValue("actioned", ref actioned);
		node.TryGetValue("soundURL", ref soundURLOverride);
		node.TryGetValue("soundRepeats", ref soundRepeatsOverride);
		vesselId = 0u;
		node.TryGetValue("vesselId", ref vesselId);
		actions.Load(node);
		OnAlarmLoad(node);
	}

	public virtual void OnAlarmLoad(ConfigNode node)
	{
	}

	public sealed override void OnSave(ConfigNode node)
	{
		node.AddValue("type", TypeName);
		node.AddValue("id", id);
		node.AddValue("title", title);
		node.AddValue("description", description);
		node.AddValue("ut", ut);
		node.AddValue("eventOffset", eventOffset);
		node.AddValue("triggered", triggered);
		node.AddValue("actioned", actioned);
		if (soundURLOverride != "")
		{
			node.AddValue("soundURL", soundURLOverride);
		}
		if (soundRepeatsOverride > 0)
		{
			node.AddValue("soundRepeatsOverride", soundRepeatsOverride);
		}
		node.AddValue("vesselId", vesselId);
		actions.Save(node);
		OnAlarmSave(node);
	}

	public virtual void OnAlarmSave(ConfigNode node)
	{
	}

	public virtual AlarmTypeBase CloneAlarm()
	{
		return (AlarmTypeBase)Clone();
	}

	public object Clone()
	{
		AlarmTypeBase alarmTypeBase = (AlarmTypeBase)Activator.CreateInstance(GetType());
		Type type = GetType();
		FieldInfo[] fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		for (int i = 0; i < fields.Length; i++)
		{
			fields[i].SetValue(alarmTypeBase, fields[i].GetValue(this));
		}
		while (type != null && type != typeof(AlarmTypeBase))
		{
			type = type.BaseType;
			if (type != null)
			{
				fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
				for (int j = 0; j < fields.Length; j++)
				{
					fields[j].SetValue(alarmTypeBase, fields[j].GetValue(this));
				}
			}
		}
		return alarmTypeBase;
	}
}
