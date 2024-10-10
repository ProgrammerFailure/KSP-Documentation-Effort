using System;
using UnityEngine;

public class ProtoTargetInfo : IConfigNode
{
	public enum Type
	{
		Vessel,
		PartModule,
		Part,
		CelestialBody,
		Generic,
		Null
	}

	public Type targetType;

	public uint partUID;

	public Guid vesselId;

	public string uniqueName;

	public int partModuleIndex;

	public ProtoTargetInfo(ProtoTargetInfo pTI)
	{
		targetType = pTI.targetType;
		partUID = pTI.partUID;
		vesselId = pTI.vesselId;
		uniqueName = pTI.uniqueName;
		partModuleIndex = pTI.partModuleIndex;
	}

	public ProtoTargetInfo(ITargetable tgt)
	{
		if (tgt is PartModule)
		{
			targetType = Type.PartModule;
			PartModule partModule = tgt as PartModule;
			partModuleIndex = partModule.part.Modules.IndexOf(partModule);
			partUID = partModule.part.flightID;
			vesselId = partModule.part.vessel.id;
		}
		else if (tgt is Part)
		{
			targetType = Type.Part;
			partUID = (tgt as Part).flightID;
			vesselId = tgt.GetVessel().id;
		}
		else if (tgt is Vessel)
		{
			targetType = Type.Vessel;
			vesselId = tgt.GetVessel().id;
		}
		else if (tgt is CelestialBody)
		{
			targetType = Type.CelestialBody;
			uniqueName = (tgt as CelestialBody).name;
		}
		else if (tgt != null)
		{
			targetType = Type.Generic;
			uniqueName = tgt.GetTransform().name;
		}
		else
		{
			targetType = Type.Null;
		}
	}

	public ProtoTargetInfo()
	{
		targetType = Type.Null;
	}

	public ProtoTargetInfo(ConfigNode node)
	{
		targetType = Type.Null;
		Load(node);
	}

	public void Load(ConfigNode node)
	{
		if (node.HasValue("type"))
		{
			targetType = (Type)Enum.Parse(typeof(Type), node.GetValue("type"));
		}
		if (node.HasValue("vesselID"))
		{
			vesselId = new Guid(node.GetValue("vesselID"));
		}
		if (node.HasValue("partID"))
		{
			partUID = uint.Parse(node.GetValue("partID"));
		}
		if (node.HasValue("moduleIdx"))
		{
			partModuleIndex = int.Parse(node.GetValue("moduleIdx"));
		}
		if (node.HasValue("tgtId"))
		{
			uniqueName = node.GetValue("tgtId");
		}
	}

	public void Save(ConfigNode node)
	{
		if (targetType != Type.Null)
		{
			node.AddValue("type", targetType.ToString());
			switch (targetType)
			{
			case Type.Vessel:
				node.AddValue("vesselID", vesselId.ToString());
				break;
			case Type.PartModule:
				node.AddValue("vesselID", vesselId.ToString());
				node.AddValue("partID", partUID);
				node.AddValue("moduleIdx", partModuleIndex);
				break;
			case Type.Part:
				node.AddValue("vesselID", vesselId.ToString());
				node.AddValue("partID", partUID);
				break;
			case Type.CelestialBody:
			case Type.Generic:
				node.AddValue("tgtId", uniqueName);
				break;
			}
		}
	}

	public ITargetable FindTarget()
	{
		switch (targetType)
		{
		default:
			return null;
		case Type.Vessel:
			return FlightGlobals.FindVessel(vesselId);
		case Type.PartModule:
		{
			Part part = FlightGlobals.FindPartByID(partUID);
			PartModule partModule = null;
			if ((bool)part)
			{
				partModule = part.Modules[partModuleIndex];
			}
			if ((bool)partModule)
			{
				return partModule as ITargetable;
			}
			return FlightGlobals.FindVessel(vesselId);
		}
		case Type.Part:
		{
			Part part = FlightGlobals.FindPartByID(partUID);
			if ((bool)part && part is ITargetable)
			{
				return part as ITargetable;
			}
			return FlightGlobals.FindVessel(vesselId);
		}
		case Type.CelestialBody:
			return FlightGlobals.GetBodyByName(uniqueName);
		case Type.Generic:
		{
			GameObject gameObject = GameObject.Find(uniqueName);
			if ((bool)gameObject)
			{
				MonoBehaviour[] components = gameObject.GetComponents<MonoBehaviour>();
				int num = components.Length;
				if (num == 0)
				{
					return null;
				}
				int num2 = 0;
				ITargetable targetable;
				while (true)
				{
					if (num2 < num)
					{
						targetable = components[num2] as ITargetable;
						if (targetable != null)
						{
							break;
						}
						num2++;
						continue;
					}
					return null;
				}
				return targetable;
			}
			return null;
		}
		}
	}
}
