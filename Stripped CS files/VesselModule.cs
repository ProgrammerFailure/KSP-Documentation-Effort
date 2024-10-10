using System;
using UnityEngine;

public class VesselModule : MonoBehaviour
{
	[Flags]
	public enum Activation
	{
		FlightScene = 1,
		NonFlightScenes = 2,
		LoadedVessels = 4,
		UnloadedVessels = 8,
		Never = 0,
		AllScenes = 3,
		LoadedOrUnloaded = 0xC,
		Always = 0xFF
	}

	public Vessel vessel;

	[HideInInspector]
	[SerializeField]
	public BaseFieldList fields;

	public Vessel Vessel
	{
		get
		{
			return vessel;
		}
		set
		{
			vessel = value;
		}
	}

	public BaseFieldList Fields => fields;

	public virtual int GetOrder()
	{
		return 999;
	}

	public virtual Activation GetActivation()
	{
		return Activation.Always;
	}

	public void Awake()
	{
		fields = new BaseFieldList(this);
		vessel = GetComponent<Vessel>();
		OnAwake();
	}

	public void Start()
	{
		OnStart();
	}

	public void Load(ConfigNode node)
	{
		Fields.Load(node);
		OnLoad(node);
	}

	public void Save(ConfigNode node)
	{
		Fields.Save(node);
		OnSave(node);
	}

	public virtual void OnAwake()
	{
	}

	public virtual void OnStart()
	{
	}

	public virtual void OnLoad(ConfigNode node)
	{
	}

	public virtual void OnSave(ConfigNode node)
	{
	}

	public virtual void OnLoadVessel()
	{
	}

	public virtual void OnUnloadVessel()
	{
	}

	public virtual void OnGoOnRails()
	{
	}

	public virtual void OnGoOffRails()
	{
	}

	public virtual bool ShouldBeActive()
	{
		Activation activation = GetActivation();
		switch (activation)
		{
		case Activation.Always:
			return true;
		case Activation.Never:
			return false;
		default:
			if ((activation & Activation.FlightScene) == 0 && HighLogic.LoadedSceneIsFlight)
			{
				return false;
			}
			if ((activation & Activation.NonFlightScenes) == 0 && !HighLogic.LoadedSceneIsFlight)
			{
				return false;
			}
			if ((activation & Activation.LoadedVessels) == 0 && vessel.loaded)
			{
				return false;
			}
			if ((activation & Activation.UnloadedVessels) == 0 && !vessel.loaded)
			{
				return false;
			}
			return true;
		}
	}
}
