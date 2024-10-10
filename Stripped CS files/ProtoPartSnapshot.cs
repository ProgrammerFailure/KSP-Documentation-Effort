using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Expansions;
using Expansions.Missions.Adjusters;
using ns11;
using UnityEngine;

public class ProtoPartSnapshot
{
	public Dictionary<string, KSPParseable> partStateValues;

	public Part partRef;

	public Part rootPartPrefab;

	public string partName;

	public uint craftID;

	public uint flightID;

	public uint missionID;

	public uint launchID;

	public uint persistentId;

	public bool sameVesselCollision;

	public Vector3d position;

	public Quaternion rotation;

	public Vector3 mirror;

	public SymmetryMethod symMethod;

	public AvailablePart partInfo;

	public ProtoPartSnapshot parent;

	public int parentIdx;

	public List<ProtoPartSnapshot> symLinks;

	public List<int> symLinkIdxs;

	public List<AttachNodeSnapshot> attachNodes;

	public AttachNodeSnapshot srfAttachNode;

	public int stageIndex;

	public int inverseStageIndex;

	public int resourcePriorityOffset;

	public int defaultInverseStage;

	public int seqOverride;

	public int inStageIndex;

	public int separationIndex;

	public string customPartData;

	public int attachMode;

	public ProtoVessel pVesselRef;

	public List<ProtoCrewMember> protoModuleCrew;

	public List<int> protoCrewIndicesBackup;

	public List<string> protoCrewNames;

	public float mass;

	public double temperature;

	public double skinTemperature;

	public double skinUnexposedTemperature;

	public double staticPressureAtm;

	public float explosionPotential;

	public int state;

	public int PreFailState;

	public bool shielded;

	public bool attached;

	public Part.AutoStrutMode autostrutMode;

	public bool rigidAttachment;

	public List<ProtoPartModuleSnapshot> modules;

	public List<ProtoPartResourceSnapshot> resources;

	public List<string> partRendererBoundsIgnore;

	public ConfigNode partEvents;

	public ConfigNode partActions;

	public ConfigNode partEffects;

	public ConfigNode partData;

	public string flagURL;

	public string refTransformName;

	public float moduleCosts;

	public float moduleMass;

	public string moduleVariantName;

	public int moduleCargoStackableQuantity = 1;

	public VesselNaming vesselNaming;

	public Part partPrefab
	{
		get
		{
			if (partInfo == null)
			{
				return null;
			}
			return partInfo.partPrefab;
		}
	}

	public ProtoPartSnapshot(Part PartRef, ProtoVessel protoVessel)
		: this(PartRef, protoVessel, preCreate: false)
	{
	}

	public ProtoPartSnapshot(Part PartRef, ProtoVessel protoVessel, bool preCreate)
	{
		pVesselRef = protoVessel;
		partRef = PartRef;
		partRef.onBackup();
		partStateValues = new Dictionary<string, KSPParseable>();
		modules = new List<ProtoPartModuleSnapshot>();
		resources = new List<ProtoPartResourceSnapshot>();
		partRendererBoundsIgnore = new List<string>();
		partName = partRef.partInfo.name;
		craftID = partRef.craftID;
		flightID = partRef.flightID;
		missionID = partRef.missionID;
		launchID = partRef.launchID;
		persistentId = partRef.persistentId;
		sameVesselCollision = partRef.sameVesselCollision;
		partInfo = PartLoader.getPartInfoByName(partName);
		mirror = partRef.mirrorVector;
		symMethod = partRef.symMethod;
		inverseStageIndex = partRef.inverseStage;
		resourcePriorityOffset = partRef.resourcePriorityOffset;
		defaultInverseStage = partRef.defaultInverseStage;
		seqOverride = partRef.manualStageOffset;
		inStageIndex = partRef.inStageIndex;
		separationIndex = partRef.separationIndex;
		customPartData = partRef.customPartData;
		attachMode = (int)partRef.attachMode;
		attached = partRef.isAttached;
		autostrutMode = partRef.autoStrutMode;
		rigidAttachment = partRef.rigidAttachment;
		mass = partRef.mass;
		shielded = partRef.ShieldedFromAirstream;
		temperature = partRef.temperature;
		skinTemperature = partRef.skinTemperature;
		skinUnexposedTemperature = partRef.skinUnexposedTemperature;
		staticPressureAtm = partRef.staticPressureAtm;
		explosionPotential = partRef.explosionPotential;
		state = (int)partRef.State;
		PreFailState = (int)partRef.PreFailState;
		moduleCosts = partRef.GetModuleCosts(partRef.partInfo.cost);
		moduleMass = partRef.GetModuleMass(partRef.partInfo.cost);
		moduleVariantName = "";
		if (partRef.variants != null && partRef.variants.SelectedVariant != null)
		{
			moduleVariantName = partRef.variants.SelectedVariant.Name;
		}
		moduleCargoStackableQuantity = 1;
		if (partRef.isCargoPart())
		{
			moduleCargoStackableQuantity = partRef.FindModuleImplementing<ModuleCargoPart>().stackableQuantity;
		}
		partRef.onFlightStateSave(partStateValues);
		partRef.protoPartSnapshot = this;
		protoModuleCrew = new List<ProtoCrewMember>();
		protoCrewIndicesBackup = new List<int>();
		protoCrewNames = new List<string>();
		if (partRef.protoModuleCrew != null)
		{
			for (int i = 0; i < partRef.protoModuleCrew.Count; i++)
			{
				ProtoCrewMember protoCrewMember = partRef.protoModuleCrew[i];
				protoModuleCrew.Add(protoCrewMember);
				protoCrewNames.Add(protoCrewMember.name);
				protoVessel.AddCrew(protoCrewMember);
			}
		}
		for (int j = 0; j < partRef.Modules.Count; j++)
		{
			modules.Add(new ProtoPartModuleSnapshot(partRef.Modules[j]));
		}
		position = partRef.orgPos;
		rotation = partRef.orgRot;
		for (int k = 0; k < partRef.Resources.Count; k++)
		{
			resources.Add(new ProtoPartResourceSnapshot(partRef.Resources[k]));
		}
		partRendererBoundsIgnore = partRef.partRendererBoundsIgnore;
		partEvents = new ConfigNode("EVENTS");
		partRef.Events.OnSave(partEvents);
		partActions = new ConfigNode("ACTIONS");
		partRef.Actions.OnSave(partActions);
		partData = new ConfigNode("PARTDATA");
		partRef.OnSave(partData);
		flagURL = PartRef.flagURL;
		if (partRef.GetReferenceTransform() != null)
		{
			refTransformName = partRef.GetReferenceTransform().name;
		}
		else
		{
			refTransformName = "";
		}
		if (partRef.vesselNaming != null)
		{
			vesselNaming = partRef.vesselNaming;
		}
		GameEvents.onProtoPartSnapshotSave.Fire(new GameEvents.FromToAction<ProtoPartSnapshot, ConfigNode>(this, null));
	}

	public ProtoPartSnapshot(ConfigNode node, ProtoVessel protoVessel, Game st)
	{
		GameEvents.onProtoPartSnapshotLoad.Fire(new GameEvents.FromToAction<ProtoPartSnapshot, ConfigNode>(this, node));
		pVesselRef = protoVessel;
		partStateValues = new Dictionary<string, KSPParseable>();
		attachNodes = new List<AttachNodeSnapshot>();
		symLinkIdxs = new List<int>();
		protoCrewIndicesBackup = new List<int>();
		protoCrewNames = new List<string>();
		modules = new List<ProtoPartModuleSnapshot>();
		resources = new List<ProtoPartResourceSnapshot>();
		partRendererBoundsIgnore = new List<string>();
		customPartData = "";
		flagURL = "";
		refTransformName = "";
		moduleCosts = 0f;
		moduleMass = 0f;
		moduleVariantName = "";
		moduleCargoStackableQuantity = 1;
		for (int i = 0; i < node.values.Count; i++)
		{
			ConfigNode.Value value = node.values[i];
			switch (value.name)
			{
			case "launchID":
				launchID = uint.Parse(value.value);
				break;
			case "srfN":
				srfAttachNode = new AttachNodeSnapshot(value.value);
				break;
			case "moduleCargoStackableQuantity":
				moduleCargoStackableQuantity = int.Parse(value.value);
				break;
			case "rotation":
				rotation = KSPUtil.ParseQuaternion(value.value);
				break;
			case "tempExt":
				skinTemperature = double.Parse(value.value);
				break;
			case "resPri":
				resourcePriorityOffset = int.Parse(value.value);
				break;
			case "sidx":
				inStageIndex = int.Parse(value.value);
				break;
			case "attm":
				attachMode = int.Parse(value.value);
				break;
			case "sepI":
				separationIndex = int.Parse(value.value);
				break;
			case "symMethod":
				symMethod = (SymmetryMethod)Enum.Parse(typeof(SymmetryMethod), value.value);
				break;
			case "mirror":
				mirror = KSPUtil.ParseVector3d(value.value);
				break;
			case "rigidAttachment":
			{
				if (bool.TryParse(value.value, out var result4))
				{
					rigidAttachment = result4;
				}
				break;
			}
			case "attN":
				attachNodes.Add(new AttachNodeSnapshot(value.value));
				break;
			case "tempExtUnexp":
				skinUnexposedTemperature = double.Parse(value.value);
				break;
			case "sameVesselCollision":
				sameVesselCollision = bool.Parse(value.value);
				break;
			case "uid":
				flightID = uint.Parse(value.value);
				break;
			case "modCost":
				moduleCosts = float.Parse(value.value);
				break;
			case "dstg":
				defaultInverseStage = int.Parse(value.value);
				break;
			case "state":
				state = int.Parse(value.value);
				break;
			case "persistentId":
			{
				uint result3 = 0u;
				if (!uint.TryParse(value.value, out result3) && !uint.TryParse(value.value.Split(',')[0].Trim(), out result3))
				{
					result3 = 0u;
				}
				persistentId = result3;
				break;
			}
			case "cData":
				customPartData = value.value;
				break;
			case "modMass":
				moduleMass = float.Parse(value.value);
				break;
			case "sqor":
				seqOverride = int.Parse(value.value);
				break;
			case "position":
				position = KSPUtil.ParseVector3d(value.value);
				break;
			case "crew":
			{
				int result2 = -1;
				if (int.TryParse(value.value, out result2))
				{
					protoCrewIndicesBackup.Add(result2);
				}
				else
				{
					protoCrewNames.Add(value.value);
				}
				break;
			}
			case "name":
				partName = value.value;
				partInfo = PartLoader.getPartInfoByName(partName);
				break;
			case "autostrutMode":
				autostrutMode = (Part.AutoStrutMode)Enum.Parse(typeof(Part.AutoStrutMode), value.value);
				break;
			case "attached":
				attached = bool.Parse(value.value);
				break;
			case "shielded":
				bool.TryParse(value.value, out shielded);
				break;
			case "flag":
				flagURL = value.value;
				break;
			case "sym":
				symLinkIdxs.Add(int.Parse(value.value));
				break;
			case "istg":
				inverseStageIndex = int.Parse(value.value);
				break;
			case "mid":
				missionID = uint.Parse(value.value);
				break;
			case "temp":
				temperature = double.Parse(value.value);
				break;
			case "staticPressureAtm":
				staticPressureAtm = double.Parse(value.value);
				break;
			case "mass":
				mass = float.Parse(value.value);
				break;
			case "expt":
				explosionPotential = float.Parse(value.value);
				break;
			case "parent":
				parentIdx = int.Parse(value.value);
				break;
			case "cid":
				craftID = uint.Parse(value.value);
				break;
			case "partRendererBoundsIgnore":
			{
				string[] array = value.value.Split(',');
				for (int j = 0; j < array.Length; j++)
				{
					array[j] = array[j].Trim();
					partRendererBoundsIgnore.Add(array[j]);
				}
				break;
			}
			case "moduleVariantName":
				moduleVariantName = value.value;
				break;
			case "PreFailState":
			{
				int result = 0;
				if (!int.TryParse(value.value, out result) && !int.TryParse(value.value.Split(',')[0].Trim(), out result))
				{
					result = 0;
				}
				PreFailState = result;
				break;
			}
			default:
				partStateValues.Add(value.name, new KSPParseable(value.value));
				break;
			case "rTrf":
				refTransformName = value.value;
				break;
			case "connected":
				break;
			}
		}
		persistentId = FlightGlobals.CheckProtoPartSnapShotpersistentId(persistentId, this, removeOldId: false, addNewId: true);
		for (int k = 0; k < node.nodes.Count; k++)
		{
			ConfigNode configNode = node.nodes[k];
			switch (configNode.name)
			{
			case "MODULE":
				modules.Add(new ProtoPartModuleSnapshot(configNode));
				break;
			case "ACTIONS":
				partActions = new ConfigNode();
				configNode.CopyTo(partActions);
				break;
			case "PARTDATA":
				partData = new ConfigNode();
				configNode.CopyTo(partData);
				break;
			case "RESOURCE":
				resources.Add(new ProtoPartResourceSnapshot(configNode));
				break;
			case "VESSELNAMING":
				vesselNaming = new VesselNaming(configNode);
				break;
			case "EVENTS":
				partEvents = new ConfigNode();
				configNode.CopyTo(partEvents);
				break;
			case "EFFECTS":
				partEffects = new ConfigNode();
				configNode.CopyTo(partEffects);
				break;
			}
		}
		if (protoModuleCrew == null)
		{
			protoModuleCrew = new List<ProtoCrewMember>();
		}
		if (st != null)
		{
			int l = 0;
			for (int count = protoCrewIndicesBackup.Count; l < count; l++)
			{
				protoCrewNames.Add(st.CrewRoster[protoCrewIndicesBackup[l]].name);
			}
			int m = 0;
			for (int count2 = protoCrewNames.Count; m < count2; m++)
			{
				ProtoCrewMember protoCrewMember = st.CrewRoster[protoCrewNames[m]];
				protoModuleCrew.Add(protoCrewMember);
				protoVessel.AddCrew(protoCrewMember);
			}
		}
	}

	public void storePartRefs()
	{
		symLinks = new List<ProtoPartSnapshot>();
		symLinkIdxs = new List<int>();
		for (int i = 0; i < pVesselRef.protoPartSnapshots.Count; i++)
		{
			ProtoPartSnapshot protoPartSnapshot = pVesselRef.protoPartSnapshots[i];
			if (protoPartSnapshot.partRef == partRef.parent)
			{
				parent = protoPartSnapshot;
				parentIdx = pVesselRef.protoPartSnapshots.IndexOf(parent);
			}
			if (partRef.symmetryCounterparts.Contains(protoPartSnapshot.partRef))
			{
				symLinks.Add(protoPartSnapshot);
				symLinkIdxs.Add(pVesselRef.protoPartSnapshots.IndexOf(protoPartSnapshot));
			}
		}
		attachNodes = new List<AttachNodeSnapshot>();
		srfAttachNode = new AttachNodeSnapshot(partRef.srfAttachNode, pVesselRef);
		for (int j = 0; j < partRef.attachNodes.Count; j++)
		{
			attachNodes.Add(new AttachNodeSnapshot(partRef.attachNodes[j], pVesselRef));
		}
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("name", partName);
		node.AddValue("cid", craftID);
		node.AddValue("uid", flightID);
		node.AddValue("mid", missionID);
		node.AddValue("persistentId", persistentId);
		node.AddValue("launchID", launchID);
		node.AddValue("parent", parentIdx);
		node.AddValue("position", KSPUtil.WriteVector(position));
		node.AddValue("rotation", KSPUtil.WriteQuaternion(rotation));
		node.AddValue("mirror", KSPUtil.WriteVector(mirror));
		node.AddValue("symMethod", symMethod);
		node.AddValue("istg", inverseStageIndex);
		node.AddValue("resPri", resourcePriorityOffset);
		node.AddValue("dstg", defaultInverseStage);
		node.AddValue("sqor", seqOverride);
		node.AddValue("sepI", separationIndex);
		node.AddValue("sidx", inStageIndex);
		node.AddValue("attm", attachMode);
		node.AddValue("sameVesselCollision", sameVesselCollision);
		if (customPartData != string.Empty)
		{
			node.AddValue("cData", customPartData);
		}
		if (symLinkIdxs != null)
		{
			int i = 0;
			for (int count = symLinkIdxs.Count; i < count; i++)
			{
				node.AddValue("sym", symLinkIdxs[i]);
			}
		}
		if (srfAttachNode != null)
		{
			node.AddValue("srfN", srfAttachNode.Save());
		}
		else
		{
			node.AddValue("srfN", "None, -1");
		}
		if (attachNodes != null)
		{
			int j = 0;
			for (int count2 = attachNodes.Count; j < count2; j++)
			{
				node.AddValue("attN", attachNodes[j].Save());
			}
		}
		node.AddValue("mass", mass);
		node.AddValue("shielded", shielded);
		node.AddValue("temp", temperature);
		node.AddValue("tempExt", skinTemperature);
		node.AddValue("tempExtUnexp", skinUnexposedTemperature);
		node.AddValue("staticPressureAtm", staticPressureAtm);
		node.AddValue("expt", explosionPotential);
		node.AddValue("state", state);
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			node.AddValue("PreFailState", PreFailState);
		}
		node.AddValue("attached", attached);
		node.AddValue("autostrutMode", autostrutMode);
		node.AddValue("rigidAttachment", rigidAttachment);
		node.AddValue("flag", flagURL);
		node.AddValue("rTrf", refTransformName);
		node.AddValue("modCost", moduleCosts);
		node.AddValue("modMass", moduleMass);
		node.AddValue("moduleVariantName", moduleVariantName);
		node.AddValue("moduleCargoStackableQuantity", moduleCargoStackableQuantity);
		if (partRendererBoundsIgnore.Count > 0)
		{
			node.AddValue("partRendererBoundsIgnore", string.Join(",", partRendererBoundsIgnore.ToArray()));
		}
		if (partStateValues.Count > 0)
		{
			Dictionary<string, KSPParseable>.Enumerator enumerator = partStateValues.GetEnumerator();
			while (enumerator.MoveNext())
			{
				node.AddValue(enumerator.Current.Key, enumerator.Current.Value.Save());
			}
		}
		int k = 0;
		for (int count3 = protoCrewNames.Count; k < count3; k++)
		{
			node.AddValue("crew", protoCrewNames[k]);
		}
		if (partEvents != null)
		{
			partEvents.CopyTo(node.AddNode("EVENTS"));
		}
		if (partActions != null)
		{
			partActions.CopyTo(node.AddNode("ACTIONS"));
		}
		if (partEffects != null)
		{
			partEffects.CopyTo(node.AddNode("EFFECTS"));
		}
		if (partData != null)
		{
			partData.CopyTo(node.AddNode("PARTDATA"));
		}
		if (modules != null)
		{
			int l = 0;
			for (int count4 = modules.Count; l < count4; l++)
			{
				modules[l].Save(node.AddNode("MODULE"));
			}
		}
		if (resources != null)
		{
			int m = 0;
			for (int count5 = resources.Count; m < count5; m++)
			{
				resources[m].Save(node.AddNode("RESOURCE"));
			}
		}
		if (vesselNaming != null)
		{
			vesselNaming.Save(node.AddNode("VESSELNAMING"));
		}
		GameEvents.onProtoPartSnapshotSave.Fire(new GameEvents.FromToAction<ProtoPartSnapshot, ConfigNode>(this, node));
	}

	public void ConfigurePart(Vessel vesselRef, Part partToConfigure, AvailablePart PartInfo, bool setPosition)
	{
		partToConfigure.gameObject.SetActive(value: true);
		partToConfigure.protoPartSnapshot = this;
		partToConfigure.Fields.SetOriginalValue();
		if (setPosition)
		{
			partToConfigure.transform.localPosition = position;
			partToConfigure.transform.rotation = rotation;
			partToConfigure.orgPos = position;
			partToConfigure.orgRot = rotation;
		}
		partToConfigure.name = partName;
		partToConfigure.craftID = craftID;
		partToConfigure.flightID = flightID;
		partToConfigure.missionID = missionID;
		partToConfigure.launchID = launchID;
		partToConfigure.partInfo = PartInfo;
		partToConfigure.vessel = vesselRef;
		partToConfigure.packed = true;
		if ((bool)FlightGlobals.fetch)
		{
			FlightGlobals.PersistentLoadedPartIds.Remove(partToConfigure.persistentId);
		}
		partToConfigure.persistentId = persistentId;
		partToConfigure.persistentId = FlightGlobals.CheckPartpersistentId(persistentId, partRef, removeOldId: true, addNewId: true);
		if (persistentId != partToConfigure.persistentId)
		{
			persistentId = partToConfigure.persistentId;
		}
		partToConfigure.inverseStage = inverseStageIndex;
		partToConfigure.resourcePriorityOffset = resourcePriorityOffset;
		partToConfigure.defaultInverseStage = defaultInverseStage;
		partToConfigure.separationIndex = separationIndex;
		partToConfigure.inStageIndex = inStageIndex;
		partToConfigure.manualStageOffset = seqOverride;
		partToConfigure.attachMode = (AttachModes)attachMode;
		partToConfigure.isAttached = attached;
		partToConfigure.autoStrutMode = autostrutMode;
		partToConfigure.rigidAttachment = rigidAttachment;
		partToConfigure.customPartData = customPartData;
		partToConfigure.sameVesselCollision = sameVesselCollision;
		partToConfigure.mass = mass;
		partToConfigure.explosionPotential = explosionPotential;
		partToConfigure.temperature = temperature;
		partToConfigure.skinTemperature = skinTemperature;
		partToConfigure.skinUnexposedTemperature = skinUnexposedTemperature;
		partToConfigure.staticPressureAtm = staticPressureAtm;
		partToConfigure.ResumeState = (PartStates)state;
		partToConfigure.PreFailState = (PartStates)PreFailState;
		if (partRendererBoundsIgnore.Count > 0)
		{
			partToConfigure.partRendererBoundsIgnore = partRendererBoundsIgnore;
		}
		else if (partInfo.partPrefab.partRendererBoundsIgnore.Count > 0)
		{
			partToConfigure.partRendererBoundsIgnore = partInfo.partPrefab.partRendererBoundsIgnore;
		}
		partToConfigure.flagURL = flagURL;
		if (refTransformName != string.Empty)
		{
			Transform transform = partToConfigure.FindModelTransform(refTransformName) ?? partToConfigure.transform.Find(refTransformName);
			if (transform == null)
			{
				transform = partToConfigure.transform;
			}
			partToConfigure.SetReferenceTransform(transform);
		}
		else
		{
			partToConfigure.SetReferenceTransform(partToConfigure.transform);
		}
		partToConfigure.protoModuleCrew = protoModuleCrew;
		partToConfigure.SetMirror(mirror);
		partToConfigure.symMethod = symMethod;
		if (partActions != null)
		{
			partToConfigure.Actions.OnLoad(partActions);
		}
		if (partEvents != null)
		{
			partToConfigure.Events.OnLoad(partEvents);
		}
		if (partEffects != null)
		{
			partToConfigure.Effects.OnLoad(partEffects);
		}
		if (partData != null)
		{
			partToConfigure.OnLoad(partData);
		}
		int moduleIndex = 0;
		int i = 0;
		for (int count = modules.Count; i < count; i++)
		{
			modules[i].Load(partToConfigure, ref moduleIndex);
		}
		int j = 0;
		for (int count2 = resources.Count; j < count2; j++)
		{
			resources[j].Load(partToConfigure);
		}
		partToConfigure.vesselNaming = vesselNaming;
		if ((bool)rootPartPrefab)
		{
			UnityEngine.Object.DestroyImmediate(rootPartPrefab.gameObject);
		}
		partToConfigure.RegisterCrew();
	}

	public Part CreatePart()
	{
		GameEvents.onProtoPartSnapshotLoad.Fire(new GameEvents.FromToAction<ProtoPartSnapshot, ConfigNode>(this, null));
		PartVariant partVariant = null;
		int count = modules.Count;
		while (count-- > 0)
		{
			if (modules[count].moduleName == "ModulePartVariants" && modules[count].moduleRef != null)
			{
				partVariant = (modules[count].moduleRef as ModulePartVariants).SelectedVariant;
			}
		}
		if (partVariant != null)
		{
			partInfo.variant = partVariant;
		}
		AvailablePart availablePart = partInfo;
		if (availablePart == null)
		{
			return null;
		}
		partRef = UnityEngine.Object.Instantiate(availablePart.partPrefab);
		if (partRef == null)
		{
			return null;
		}
		ConfigurePart(null, partRef, availablePart, setPosition: true);
		if (partRef.variants != null && availablePart.variant != null)
		{
			partRef.variants.SetVariant(availablePart.variant.Name);
		}
		return partRef;
	}

	public Part Load(Vessel vesselRef, bool loadAsRootPart)
	{
		GameEvents.onProtoPartSnapshotLoad.Fire(new GameEvents.FromToAction<ProtoPartSnapshot, ConfigNode>(this, null));
		AvailablePart availablePart = partInfo;
		if (vesselRef.isEVA)
		{
			availablePart = LoadEVAInfoBySuitGender(protoModuleCrew[0]);
		}
		if (availablePart == null)
		{
			return null;
		}
		partRef = UnityEngine.Object.Instantiate(availablePart.partPrefab);
		if (partRef == null)
		{
			return null;
		}
		partRef.gameObject.SetActive(value: true);
		partRef.protoPartSnapshot = this;
		partRef.Fields.SetOriginalValue();
		if (loadAsRootPart)
		{
			partRef.transform.position = vesselRef.transform.position;
			partRef.transform.rotation = vesselRef.transform.rotation;
			partRef.orgPos = Vector3.zero;
			partRef.orgRot = Quaternion.identity;
			List<Transform> list = new List<Transform>();
			for (int i = 0; i < partRef.transform.childCount; i++)
			{
				Transform child = partRef.transform.GetChild(i);
				list.Add(child);
			}
			int j = 0;
			for (int count = list.Count; j < count; j++)
			{
				list[j].SetParent(vesselRef.transform);
			}
			Animation component = partRef.GetComponent<Animation>();
			if (component != null)
			{
				Component[] components = vesselRef.GetComponents<Component>();
				int num = components.Length;
				while (num-- > 0)
				{
					Component component2 = components[num];
					if (component2 is Animation)
					{
						UnityEngine.Object.DestroyImmediate(component2);
						components[num] = null;
					}
				}
				Animation animation = vesselRef.gameObject.AddComponent<Animation>();
				if (animation == null)
				{
					Debug.LogError("*** Loading as root part, anim is null");
				}
				else
				{
					IEnumerator enumerator = component.GetEnumerator();
					while (enumerator.MoveNext())
					{
						AnimationState animationState = enumerator.Current as AnimationState;
						if (animationState == null)
						{
							Debug.LogError("*** Laoding as root part, anim *state* is null");
						}
						else
						{
							animation.AddClip(animationState.clip, animationState.name);
						}
					}
				}
			}
			Animator component3 = partRef.GetComponent<Animator>();
			if (component3 != null)
			{
				Component[] components2 = vesselRef.GetComponents<Component>();
				int num2 = components2.Length;
				while (num2-- > 0)
				{
					Component component4 = components2[num2];
					if (component4 is Animator)
					{
						UnityEngine.Object.DestroyImmediate(component4);
						components2[num2] = null;
					}
				}
				vesselRef.gameObject.AddComponent<Animator>();
				vesselRef.gameObject.GetComponent<Animator>().runtimeAnimatorController = component3.runtimeAnimatorController;
				vesselRef.gameObject.GetComponent<Animator>().cullingMode = AnimatorCullingMode.CullUpdateTransforms;
			}
			Part part = vesselRef.gameObject.AddComponent(partRef.GetType()) as Part;
			int k = 0;
			for (int num3 = partRef.PartAttributes.publicFields.Length; k < num3; k++)
			{
				FieldInfo fieldInfo = partRef.PartAttributes.publicFields[k];
				fieldInfo.SetValue(part, fieldInfo.GetValue(partRef));
			}
			part.partTransform = vesselRef.transform;
			ConfigNode node = new ConfigNode();
			partRef.SaveEffects(node);
			part.LoadEffects(node);
			int count2 = partRef.Resources.Count;
			for (int l = 0; l < count2; l++)
			{
				PartResource partResource = partRef.Resources[l];
				node = new ConfigNode();
				partResource.Save(node);
				part.AddResource(node);
			}
			PartModule[] components3 = partRef.GetComponents<PartModule>();
			int m = 0;
			for (int num4 = components3.Length; m < num4; m++)
			{
				PartModule partModule = components3[m];
				PartModule partModule2 = part.AddModule(partModule.moduleName);
				if (partModule2 == null)
				{
					continue;
				}
				int n = 0;
				for (int num5 = partModule.ModuleAttributes.publicFields.Length; n < num5; n++)
				{
					FieldInfo fieldInfo = partModule.ModuleAttributes.publicFields[n];
					if (!fieldInfo.IsLiteral && !fieldInfo.IsInitOnly)
					{
						fieldInfo.SetValue(partModule2, fieldInfo.GetValue(partModule));
					}
				}
				partModule2.Fields.SetOriginalValue();
				int num6 = 0;
				for (int num7 = partRef.PartAttributes.publicFields.Length; num6 < num7; num6++)
				{
					FieldInfo fieldInfo = partRef.PartAttributes.publicFields[num6];
					if (!fieldInfo.FieldType.IsValueType && fieldInfo.GetValue(partRef) == partModule)
					{
						fieldInfo.SetValue(part, partModule2);
					}
				}
			}
			if (partRef.GetComponent<kerbalExpressionSystem>() != null)
			{
				vesselRef.gameObject.AddComponent<kerbalExpressionSystem>();
				vesselRef.gameObject.GetComponent<kerbalExpressionSystem>().evaPart = vesselRef.gameObject.GetComponent<Part>();
				vesselRef.gameObject.GetComponent<kerbalExpressionSystem>().animator = vesselRef.gameObject.GetComponent<Animator>();
				vesselRef.gameObject.GetComponent<kerbalExpressionSystem>().kerbalEVA = vesselRef.gameObject.GetComponent<KerbalEVA>();
			}
			rootPartPrefab = partRef;
			partRef = vesselRef.GetComponent<Part>();
			partRef.stackIcon = new ProtoStageIcon(partRef);
			if ((bool)FlightGlobals.fetch)
			{
				FlightGlobals.PersistentLoadedPartIds.Remove(part.persistentId);
			}
		}
		else
		{
			partRef.transform.parent = vesselRef.transform;
			partRef.transform.localPosition = position;
			partRef.transform.rotation = vesselRef.transform.rotation * rotation;
			partRef.orgPos = position;
			partRef.orgRot = rotation;
			partRef.setParent(vesselRef.parts[parentIdx]);
			partRef.transform.parent = partRef.parent.transform;
		}
		ConfigurePart(vesselRef, partRef, availablePart, setPosition: false);
		return partRef;
	}

	public AvailablePart LoadEVAInfoBySuitGender(ProtoCrewMember protoCrew)
	{
		if (ExpansionsLoader.IsExpansionAnyKerbalSuitInstalled())
		{
			switch (protoCrew.suit)
			{
			default:
				return null;
			case ProtoCrewMember.KerbalSuit.Default:
				return protoCrew.gender switch
				{
					ProtoCrewMember.Gender.Female => PartLoader.getPartInfoByName("kerbalEVAfemale"), 
					ProtoCrewMember.Gender.Male => PartLoader.getPartInfoByName("kerbalEVA"), 
					_ => null, 
				};
			case ProtoCrewMember.KerbalSuit.Vintage:
				if (ExpansionsLoader.IsExpansionKerbalSuitInstalled(ProtoCrewMember.KerbalSuit.Vintage))
				{
					return protoCrew.gender switch
					{
						ProtoCrewMember.Gender.Female => PartLoader.getPartInfoByName("kerbalEVAfemaleVintage"), 
						ProtoCrewMember.Gender.Male => PartLoader.getPartInfoByName("kerbalEVAVintage"), 
						_ => null, 
					};
				}
				return protoCrew.gender switch
				{
					ProtoCrewMember.Gender.Female => PartLoader.getPartInfoByName("kerbalEVAfemale"), 
					ProtoCrewMember.Gender.Male => PartLoader.getPartInfoByName("kerbalEVA"), 
					_ => null, 
				};
			case ProtoCrewMember.KerbalSuit.Future:
				if (ExpansionsLoader.IsExpansionKerbalSuitInstalled(ProtoCrewMember.KerbalSuit.Future))
				{
					return protoCrew.gender switch
					{
						ProtoCrewMember.Gender.Female => PartLoader.getPartInfoByName("kerbalEVAfemaleFuture"), 
						ProtoCrewMember.Gender.Male => PartLoader.getPartInfoByName("kerbalEVAFuture"), 
						_ => null, 
					};
				}
				return protoCrew.gender switch
				{
					ProtoCrewMember.Gender.Female => PartLoader.getPartInfoByName("kerbalEVAfemale"), 
					ProtoCrewMember.Gender.Male => PartLoader.getPartInfoByName("kerbalEVA"), 
					_ => null, 
				};
			case ProtoCrewMember.KerbalSuit.Slim:
				return protoCrew.gender switch
				{
					ProtoCrewMember.Gender.Female => PartLoader.getPartInfoByName("kerbalEVASlimSuitFemale"), 
					ProtoCrewMember.Gender.Male => PartLoader.getPartInfoByName("kerbalEVASlimSuit"), 
					_ => null, 
				};
			}
		}
		return protoCrew.suit switch
		{
			ProtoCrewMember.KerbalSuit.Slim => protoCrew.gender switch
			{
				ProtoCrewMember.Gender.Female => PartLoader.getPartInfoByName("kerbalEVASlimSuit"), 
				ProtoCrewMember.Gender.Male => PartLoader.getPartInfoByName("kerbalEVASlimSuit"), 
				_ => null, 
			}, 
			ProtoCrewMember.KerbalSuit.Default => protoCrew.gender switch
			{
				ProtoCrewMember.Gender.Female => PartLoader.getPartInfoByName("kerbalEVAfemale"), 
				ProtoCrewMember.Gender.Male => PartLoader.getPartInfoByName("kerbalEVA"), 
				_ => null, 
			}, 
			_ => null, 
		};
	}

	public void Init(Vessel vesselRef)
	{
		int i = 0;
		for (int count = symLinkIdxs.Count; i < count; i++)
		{
			partRef.symmetryCounterparts.Add(vesselRef.parts[symLinkIdxs[i]]);
		}
		partRef.srfAttachNode.attachedPart = srfAttachNode.findAttachedPart(vesselRef);
		partRef.srfAttachNode.srfAttachMeshName = srfAttachNode.srfAttachMeshName;
		for (int j = 0; j < partRef.attachNodes.Count; j++)
		{
			AttachNodeSnapshot attachNodeSnapshot = FindAttachNodeSnapShot(partRef.attachNodes[j].id, attachNodes);
			if (attachNodeSnapshot != null && attachNodeSnapshot.partIdx != -1)
			{
				partRef.attachNodes[j].attachedPart = attachNodeSnapshot.findAttachedPart(vesselRef);
				partRef.attachNodes[j].srfAttachMeshName = attachNodeSnapshot.srfAttachMeshName;
			}
			else
			{
				partRef.attachNodes[j].attachedPart = null;
				partRef.attachNodes[j].srfAttachMeshName = "";
			}
		}
		partRef.onFlightStateLoad(partStateValues);
	}

	public AttachNode FindAttachNode(string name, List<AttachNode> nodes)
	{
		int num = 0;
		int count = nodes.Count;
		while (true)
		{
			if (num < count)
			{
				if (nodes[num].id == name)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return nodes[num];
	}

	public AttachNodeSnapshot FindAttachNodeSnapShot(string name, List<AttachNodeSnapshot> nodes)
	{
		int num = 0;
		int count = nodes.Count;
		while (true)
		{
			if (num < count)
			{
				if (nodes[num].id == name)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return nodes[num];
	}

	public bool HasCrew(string name)
	{
		int num = 0;
		int count = protoModuleCrew.Count;
		while (true)
		{
			if (num < count)
			{
				if (protoCrewNames[num] == name || protoModuleCrew[num].name == name)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public ProtoCrewMember GetCrew(string name)
	{
		int num = 0;
		int count = protoModuleCrew.Count;
		while (true)
		{
			if (num < count)
			{
				if (protoCrewNames[num] == name || protoModuleCrew[num].name == name)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return protoModuleCrew[num];
	}

	public ProtoCrewMember GetCrew(int index)
	{
		return protoModuleCrew[index];
	}

	public int GetCrewIndex(string name)
	{
		int num = 0;
		int count = protoModuleCrew.Count;
		while (true)
		{
			if (num < count)
			{
				if (protoCrewNames[num] == name || protoModuleCrew[num].name == name)
				{
					break;
				}
				num++;
				continue;
			}
			return -1;
		}
		return num;
	}

	public void RemoveCrew(string name)
	{
		int num = 0;
		int count = protoModuleCrew.Count;
		while (true)
		{
			if (num < count)
			{
				if (protoCrewNames[num] == name || protoModuleCrew[num].name == name)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		protoCrewNames.RemoveAt(num);
		protoModuleCrew.RemoveAt(num);
	}

	public void RemoveCrew(int index)
	{
		protoCrewNames.RemoveAt(index);
		protoModuleCrew.RemoveAt(index);
	}

	public void RemoveCrew(ProtoCrewMember pcm)
	{
		int num = 0;
		int count = protoModuleCrew.Count;
		while (true)
		{
			if (num < count)
			{
				if (protoModuleCrew[num] == pcm)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		protoCrewNames.RemoveAt(num);
		protoModuleCrew.RemoveAt(num);
	}

	public ProtoPartModuleSnapshot FindModule(string moduleName)
	{
		ProtoPartModuleSnapshot result = null;
		int count = modules.Count;
		while (count-- > 0)
		{
			ProtoPartModuleSnapshot protoPartModuleSnapshot = modules[count];
			if (protoPartModuleSnapshot.moduleName == moduleName)
			{
				result = protoPartModuleSnapshot;
				break;
			}
		}
		return result;
	}

	public ProtoPartModuleSnapshot FindModule(PartModule pm, int moduleIndex)
	{
		ProtoPartModuleSnapshot protoPartModuleSnapshot = null;
		if (moduleIndex < modules.Count)
		{
			protoPartModuleSnapshot = modules[moduleIndex];
			if (protoPartModuleSnapshot.moduleName != pm.moduleName)
			{
				protoPartModuleSnapshot = FindModule(pm.moduleName);
			}
		}
		else
		{
			protoPartModuleSnapshot = FindModule(pm.moduleName);
		}
		return protoPartModuleSnapshot;
	}

	public void AddProtoPartModuleAdjusters(List<AdjusterPartModuleBase> ModuleAdjusters)
	{
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			for (int i = 0; i < modules.Count; i++)
			{
				modules[i].AddPartModuleAdjusterList(ModuleAdjusters);
			}
		}
	}

	public void RemoveProtoPartModuleAdjusters(List<AdjusterPartModuleBase> ModuleAdjusters)
	{
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			for (int i = 0; i < modules.Count; i++)
			{
				modules[i].RemovePartModuleAdjusterList(ModuleAdjusters);
			}
		}
	}

	public void ProtoPartRepair()
	{
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory"))
		{
			state = PreFailState;
			for (int i = 0; i < modules.Count; i++)
			{
				modules[i].ProtoPartModuleRepair();
			}
			GameEvents.onProtoPartRepair.Fire(this);
		}
	}
}
