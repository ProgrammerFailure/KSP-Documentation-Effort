using System;
using System.Collections.Generic;
using Experience.Effects;
using UnityEngine;

namespace CommNet;

public class CommNetVessel : VesselModule
{
	public Action OnNetworkUpdate;

	public bool hasScienceAntenna;

	[KSPField(isPersistant = true)]
	public VesselControlState controlState;

	[KSPField(isPersistant = true)]
	public bool canComm = true;

	public bool inPlasma;

	public double plasmaMult = 1.0;

	public CommPath controlPath = new CommPath();

	public double signalDelay;

	public List<ICommNetControlSource> commandSources = new List<ICommNetControlSource>();

	public int partCountCache;

	public bool networkInitialised;

	public bool unloadedDoOnce = true;

	public bool overridePreUpdate;

	public bool overridePostUpdate;

	public bool doUnloadedUpdate;

	public CommNode comm;

	public bool IsConnected { get; set; }

	public bool CanScience
	{
		get
		{
			if (hasScienceAntenna)
			{
				return IsConnectedHome;
			}
			return false;
		}
	}

	public bool IsConnectedHome { get; set; }

	public VesselControlState ControlState => controlState;

	public bool CanComm => canComm;

	public bool InPlasma => inPlasma;

	public CommPath ControlPath => controlPath;

	public SignalStrength Signal => controlPath.signal;

	public double SignalStrength => controlPath.signalStrength;

	public double SignalDelay => signalDelay;

	public List<ICommNetControlSource> CommandSources => commandSources;

	public CommNode Comm
	{
		get
		{
			return comm;
		}
		set
		{
			comm = value;
		}
	}

	public override void OnAwake()
	{
		if ((bool)vessel)
		{
			vessel.connection = this;
		}
	}

	public override void OnStart()
	{
		if (vessel.vesselType != VesselType.Flag && vessel.vesselType > VesselType.Unknown && vessel.vesselType != VesselType.DeployedSciencePart)
		{
			comm = new CommNode(base.transform);
			comm.OnNetworkPreUpdate = OnNetworkPreUpdate;
			comm.OnNetworkPostUpdate = OnNetworkPostUpdate;
			comm.OnLinkCreateSignalModifier = GetSignalStrengthModifier;
			vessel.connection = this;
			networkInitialised = false;
			if (CommNetNetwork.Initialized)
			{
				OnNetworkInitialized();
			}
			GameEvents.CommNet.OnNetworkInitialized.Add(OnNetworkInitialized);
			if (HighLogic.LoadedScene == GameScenes.TRACKSTATION)
			{
				GameEvents.onPlanetariumTargetChanged.Add(OnMapFocusChange);
			}
		}
		else
		{
			vessel.vesselModules.Remove(this);
			vessel.connection = null;
			UnityEngine.Object.Destroy(this);
		}
	}

	public virtual void OnDestroy()
	{
		CommNetNetwork.Remove(comm);
		GameEvents.CommNet.OnNetworkInitialized.Remove(OnNetworkInitialized);
		GameEvents.onPlanetariumTargetChanged.Add(OnMapFocusChange);
	}

	public virtual void OnMapFocusChange(MapObject target)
	{
		if (target.vessel == vessel)
		{
			doUnloadedUpdate = true;
			unloadedDoOnce = true;
		}
		else
		{
			doUnloadedUpdate = false;
		}
	}

	public virtual void Update()
	{
		if (vessel.loaded)
		{
			int count = vessel.Parts.Count;
			if (count != partCountCache)
			{
				partCountCache = count;
				FindCommandSources();
			}
			UpdateControlState();
		}
	}

	public virtual void OnNetworkInitialized()
	{
		networkInitialised = true;
		CommNetNetwork.Add(comm);
	}

	public virtual void OnNetworkPreUpdate()
	{
		if (!unloadedDoOnce && !vessel.loaded && !overridePreUpdate)
		{
			inPlasma = false;
		}
		else
		{
			UpdateComm();
			CalculatePlasmaMult();
		}
		comm.precisePosition = vessel.GetWorldPos3D();
	}

	public virtual void CalculatePlasmaMult()
	{
		inPlasma = false;
		if (vessel.loaded && !vessel.packed && HighLogic.CurrentGame.Parameters.CustomParams<CommNetParams>().plasmaBlackout && vessel.externalTemperature > PhysicsGlobals.CommNetTempForBlackout && vessel.atmDensity > PhysicsGlobals.CommNetDensityForBlackout)
		{
			double num = vessel.dynamicPressurekPa * vessel.srfSpeed;
			if (num > PhysicsGlobals.CommNetQTimesVelForBlackoutMin)
			{
				plasmaMult = UtilMath.InverseLerp(PhysicsGlobals.CommNetQTimesVelForBlackoutMin, PhysicsGlobals.CommNetQTimesVelForBlackoutMax, num);
				inPlasma = plasmaMult > 0.0;
			}
		}
	}

	public virtual double GetSignalStrengthModifier(CommNode b)
	{
		if (canComm)
		{
			if (inPlasma)
			{
				double num = Vector3d.Dot((b.precisePosition - vessel.CoMD).normalized, vessel.srf_vel_direction);
				double num2 = ((num < PhysicsGlobals.CommNetDotForBlackoutMin) ? 0.0 : ((!(num > PhysicsGlobals.CommNetDotForBlackoutMax)) ? UtilMath.InverseLerp(PhysicsGlobals.CommNetDotForBlackoutMin, PhysicsGlobals.CommNetDotForBlackoutMax, num) : 1.0));
				num2 *= plasmaMult;
				num2 = 1.0 - num2;
				if (num2 < PhysicsGlobals.CommNetBlackoutThreshold)
				{
					return 0.0;
				}
				num2 = (num2 - PhysicsGlobals.CommNetBlackoutThreshold) / (1.0 - PhysicsGlobals.CommNetBlackoutThreshold);
				if (num2 > 1.0)
				{
					num2 = 1.0;
				}
				return num2;
			}
			return 1.0;
		}
		return 0.0;
	}

	public virtual void UpdateComm()
	{
		if (base.gameObject == null || vessel == null)
		{
			return;
		}
		if (comm.name != base.gameObject.name)
		{
			comm.name = base.gameObject.name;
		}
		if (comm.displayName != vessel.GetDisplayName())
		{
			comm.displayName = vessel.GetDisplayName();
		}
		ICommAntenna commAntenna = null;
		int num = -1;
		ICommAntenna commAntenna2 = null;
		double num2 = 0.0;
		double num3 = 0.0;
		double num4 = 0.0;
		double num5 = 0.0;
		ICommAntenna commAntenna3 = null;
		double num6 = 0.0;
		double num7 = 0.0;
		double num8 = 0.0;
		double num9 = 0.0;
		ICommAntenna commAntenna4 = null;
		ICommAntenna commAntenna5 = null;
		ProtoPartSnapshot protoPartSnapshot = null;
		ProtoPartModuleSnapshot mSnap = null;
		bool flag = false;
		comm.isControlSource = false;
		comm.isControlSourceMultiHop = false;
		CommNode.AntennaInfo antennaRelay = comm.antennaRelay;
		CommNode.AntennaInfo antennaTransmit = comm.antennaTransmit;
		double power = 0.0;
		antennaTransmit.power = 0.0;
		antennaRelay.power = power;
		hasScienceAntenna = false;
		int index = (vessel.loaded ? vessel.Parts.Count : vessel.protoVessel.protoPartSnapshots.Count);
		while (index-- > 0)
		{
			Part part;
			if (vessel.loaded)
			{
				part = vessel.Parts[index];
			}
			else
			{
				protoPartSnapshot = vessel.protoVessel.protoPartSnapshots[index];
				part = protoPartSnapshot.partInfo.partPrefab;
			}
			int count = part.Modules.Count;
			while (count-- > 0)
			{
				PartModule partModule = part.Modules[count];
				if (partModule is IRelayEnabler)
				{
					if (vessel.loaded)
					{
						if ((partModule as IRelayEnabler).CanRelay())
						{
							flag = true;
						}
					}
					else if ((partModule as IRelayEnabler).CanRelayUnloaded(protoPartSnapshot.FindModule(partModule, count)))
					{
						flag = true;
					}
				}
				else if (partModule is ICommAntenna)
				{
					commAntenna = partModule as ICommAntenna;
					if (vessel.loaded)
					{
						if (!commAntenna.CanComm())
						{
							continue;
						}
					}
					else if (!commAntenna.CanCommUnloaded(mSnap = protoPartSnapshot.FindModule(partModule, count)))
					{
						continue;
					}
					double num10 = (vessel.loaded ? commAntenna.CommPower : commAntenna.CommPowerUnloaded(mSnap));
					if (commAntenna.CommType != AntennaType.RELAY)
					{
						if (commAntenna.CommType != 0)
						{
							hasScienceAntenna = true;
						}
						if (num10 > num2)
						{
							commAntenna2 = commAntenna;
							num2 = num10;
						}
						if (commAntenna.CommCombinable)
						{
							num3 += num10;
							num4 += num10 * commAntenna.CommCombinableExponent;
							if (commAntenna4 == null || num10 > num5)
							{
								commAntenna4 = commAntenna;
								num5 = num10;
							}
						}
						continue;
					}
					hasScienceAntenna = true;
					if (num10 > num6)
					{
						commAntenna3 = commAntenna;
						num6 = num10;
						if (commAntenna3.CommCombinable)
						{
							commAntenna5 = commAntenna;
						}
					}
					if (commAntenna.CommCombinable)
					{
						num7 += num10;
						num8 += num10 * commAntenna.CommCombinableExponent;
						if (commAntenna5 == null || num10 > num9)
						{
							commAntenna5 = commAntenna;
							num9 = num10;
						}
					}
				}
				else
				{
					if (!(partModule is ModuleProbeControlPoint))
					{
						continue;
					}
					ModuleProbeControlPoint moduleProbeControlPoint = partModule as ModuleProbeControlPoint;
					if (vessel.loaded)
					{
						if (!moduleProbeControlPoint.CanControl())
						{
							continue;
						}
					}
					else if (!moduleProbeControlPoint.CanControlUnloaded(mSnap = protoPartSnapshot.FindModule(partModule, count)))
					{
						continue;
					}
					if (moduleProbeControlPoint.minimumCrew > 0)
					{
						if (num < 0)
						{
							List<ProtoCrewMember> vesselCrew = vessel.GetVesselCrew();
							num = 0;
							int count2 = vesselCrew.Count;
							while (count2-- > 0)
							{
								if (vesselCrew[count2].HasEffect<FullVesselControlSkill>() && !vesselCrew[count2].inactive)
								{
									num++;
								}
							}
						}
						if (num >= moduleProbeControlPoint.minimumCrew)
						{
							comm.isControlSource = true;
						}
					}
					else
					{
						comm.isControlSource = true;
					}
					if (moduleProbeControlPoint.multiHop)
					{
						comm.isControlSourceMultiHop = true;
					}
				}
			}
		}
		if (commAntenna2 != null)
		{
			double num11 = num3 + num7;
			bool flag2 = num5 > num9;
			if (num11 > 0.0)
			{
				double num12 = (flag2 ? num5 : num9);
				if (num11 != num12)
				{
					double y = (num4 + num8) / num11;
					num11 = num12 * Math.Pow(num11 / num12, y);
				}
				if (num7 > 0.0 && num7 != num9)
				{
					num7 = num9 * Math.Pow(num7 / num9, num8 / num7);
				}
			}
			if (num3 > 0.0 && num11 > num2)
			{
				commAntenna2 = ((commAntenna5 == null) ? commAntenna4 : (flag2 ? commAntenna4 : commAntenna5));
				comm.antennaTransmit.combined = true;
			}
			else
			{
				num11 = num2;
				comm.antennaTransmit.combined = false;
			}
			comm.antennaTransmit.power = num11;
			comm.antennaTransmit.rangeCurve = commAntenna2.CommRangeCurve;
			comm.scienceCurve = commAntenna2.CommScienceCurve;
		}
		if (commAntenna3 != null)
		{
			double num13;
			if (num7 > num6)
			{
				num13 = num7;
				commAntenna3 = commAntenna5;
				comm.antennaRelay.combined = true;
			}
			else
			{
				num13 = num6;
				comm.antennaRelay.combined = false;
			}
			comm.antennaRelay.power = num13;
			comm.antennaRelay.rangeCurve = commAntenna3.CommRangeCurve;
			if (commAntenna2 == null || num13 > comm.antennaTransmit.power)
			{
				comm.scienceCurve = commAntenna3.CommScienceCurve;
			}
		}
		if (flag && comm.antennaTransmit.power > comm.antennaRelay.power)
		{
			comm.antennaRelay.power = comm.antennaTransmit.power;
			comm.antennaRelay.combined = comm.antennaTransmit.combined;
			comm.antennaRelay.rangeCurve = comm.antennaTransmit.rangeCurve;
			comm.antennaTransmit.power = 0.0;
		}
		comm.antennaTransmit.power *= HighLogic.CurrentGame.Parameters.CustomParams<CommNetParams>().rangeModifier;
		comm.antennaRelay.power *= HighLogic.CurrentGame.Parameters.CustomParams<CommNetParams>().rangeModifier;
	}

	public virtual void OnNetworkPostUpdate()
	{
		if (!unloadedDoOnce && !vessel.loaded && !overridePostUpdate)
		{
			return;
		}
		CreateControlConnection();
		if (vessel.loaded)
		{
			int count = commandSources.Count;
			for (int i = 0; i < count; i++)
			{
				commandSources[i].UpdateNetwork();
			}
			UpdateControlState();
		}
		if (OnNetworkUpdate != null)
		{
			OnNetworkUpdate();
		}
		unloadedDoOnce = doUnloadedUpdate;
	}

	public void UpdateControlState()
	{
		controlState = VesselControlState.None;
		canComm = false;
		int count = commandSources.Count;
		for (int i = 0; i < count; i++)
		{
			VesselControlState controlSourceState = commandSources[i].GetControlSourceState();
			if (controlSourceState > controlState)
			{
				controlState = controlSourceState;
			}
			canComm |= commandSources[i].IsCommCapable();
		}
	}

	public virtual bool CreateControlConnection()
	{
		bool isConnected = IsConnected;
		bool isConnectedHome = IsConnectedHome;
		if (!comm.Net.FindHome(comm, controlPath))
		{
			comm.Net.FindClosestControlSource(comm, controlPath);
		}
		IsConnected = controlPath.signal != CommNet.SignalStrength.None;
		IsConnectedHome = IsConnected && controlPath.Last.hopType == HopType.Home;
		bool num = isConnected != IsConnected;
		bool flag = isConnectedHome != IsConnectedHome;
		if (num)
		{
			GameEvents.CommNet.OnCommStatusChange.Fire(vessel, IsConnected);
		}
		if (flag)
		{
			GameEvents.CommNet.OnCommHomeStatusChange.Fire(vessel, IsConnectedHome);
		}
		return num;
	}

	public virtual IScienceDataTransmitter GetBestTransmitter()
	{
		if (!IsConnected)
		{
			return null;
		}
		IScienceDataTransmitter result = null;
		float num = float.MaxValue;
		double cost = controlPath.First.start[controlPath.First.end].cost;
		cost *= cost;
		double power = controlPath.First.end.antennaRelay.power;
		bool combined = ((comm.antennaRelay.power > comm.antennaTransmit.power) ? comm.antennaRelay.combined : comm.antennaTransmit.combined);
		int count = vessel.Parts.Count;
		while (count-- > 0)
		{
			Part part = vessel.Parts[count];
			int count2 = part.Modules.Count;
			while (count2-- > 0)
			{
				PartModule partModule = part.Modules[count2];
				if (partModule is IScienceDataTransmitter scienceDataTransmitter && scienceDataTransmitter.CanTransmit() && (!(partModule is ICommAntenna commAntenna) || commAntenna.CanScienceTo(combined, power, cost)))
				{
					float transmitterScore = ScienceUtil.GetTransmitterScore(scienceDataTransmitter);
					if (transmitterScore < num)
					{
						num = transmitterScore;
						result = scienceDataTransmitter;
					}
				}
			}
		}
		return result;
	}

	public virtual Vessel.ControlLevel GetControlLevel()
	{
		if ((ControlState & VesselControlState.Full) > VesselControlState.None)
		{
			return Vessel.ControlLevel.FULL;
		}
		if ((ControlState & VesselControlState.Partial) > VesselControlState.None)
		{
			if ((ControlState & VesselControlState.Kerbal) > VesselControlState.None)
			{
				return Vessel.ControlLevel.PARTIAL_MANNED;
			}
			return Vessel.ControlLevel.PARTIAL_UNMANNED;
		}
		return Vessel.ControlLevel.NONE;
	}

	public void RegisterCommandSource(ICommNetControlSource cmd)
	{
		commandSources.AddUnique(cmd);
	}

	public void UnregisterCommandSource(ICommNetControlSource cmd)
	{
		commandSources.Remove(cmd);
	}

	public void FindCommandSources()
	{
		commandSources.Clear();
		for (int i = 0; i < partCountCache; i++)
		{
			Part part = vessel.Parts[i];
			int j = 0;
			for (int count = part.Modules.Count; j < count; j++)
			{
				if (part.Modules[j] is ICommNetControlSource item)
				{
					commandSources.Add(item);
				}
			}
		}
	}
}
