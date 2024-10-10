using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ns11;
using ns3;
using ns9;
using UnityEngine;

[Serializable]
public class ShipConstruct : IEnumerable<Part>, IEnumerable, IShipconstruct
{
	public static int lastCompatibleMajor = 0;

	public static int lastCompatibleMinor = 18;

	public static int lastCompatibleRev = 0;

	public string shipName = string.Empty;

	public string shipDescription = string.Empty;

	public uint persistentId;

	public Quaternion rotation;

	public string highestPriorityName = "";

	public bool[] OverrideDefault;

	public KSPActionGroup[] OverrideActionControl;

	public KSPAxisGroup[] OverrideAxisControl;

	public string[] OverrideGroupNames;

	public EditorFacility shipFacility;

	public bool shipPartsUnlocked = true;

	public Vector3 shipSize;

	public VesselDeltaV vesselDeltaV;

	[NonSerialized]
	public PartSet resourcePartSet;

	public HashSet<Part> cachedResourcePartSetParts;

	public ulong steamPublishedFileId;

	public string missionFlag = "";

	public List<Part> parts;

	public Part vesselNamedBy;

	public VesselType vesselType;

	public List<Part> Parts => parts;

	public Part this[int index]
	{
		get
		{
			return parts[index];
		}
		set
		{
			parts[index] = value;
		}
	}

	public int Count => parts.Count;

	public ShipConstruct()
	{
		parts = new List<Part>();
		shipSize = Vector3.zero;
		persistentId = FlightGlobals.GetUniquepersistentId();
		rotation = Quaternion.identity;
		OverrideDefault = new bool[Vessel.NumOverrideGroups];
		OverrideActionControl = new KSPActionGroup[Vessel.NumOverrideGroups];
		OverrideAxisControl = new KSPAxisGroup[Vessel.NumOverrideGroups];
		OverrideGroupNames = new string[Vessel.NumOverrideGroups];
		vesselDeltaV = null;
	}

	public ShipConstruct(EditorFacility facility)
	{
		parts = new List<Part>();
		shipFacility = facility;
		shipSize = Vector3.zero;
		persistentId = FlightGlobals.GetUniquepersistentId();
		OverrideDefault = new bool[Vessel.NumOverrideGroups];
		OverrideActionControl = new KSPActionGroup[Vessel.NumOverrideGroups];
		OverrideAxisControl = new KSPAxisGroup[Vessel.NumOverrideGroups];
		OverrideGroupNames = new string[Vessel.NumOverrideGroups];
		vesselDeltaV = null;
	}

	public ShipConstruct(string shipName, EditorFacility facility, List<Part> parts)
	{
		this.shipName = shipName;
		shipFacility = facility;
		this.parts = parts;
		for (int i = 0; i < parts.Count; i++)
		{
			parts[i].ship = this;
		}
		shipSize = ShipConstruction.CalculateCraftSize(this);
		persistentId = FlightGlobals.GetUniquepersistentId();
		OverrideDefault = new bool[Vessel.NumOverrideGroups];
		OverrideActionControl = new KSPActionGroup[Vessel.NumOverrideGroups];
		OverrideAxisControl = new KSPAxisGroup[Vessel.NumOverrideGroups];
		OverrideGroupNames = new string[Vessel.NumOverrideGroups];
		vesselDeltaV = null;
	}

	public ShipConstruct(string shipName, string shipDescription, Part rootPart)
	{
		this.shipName = shipName;
		this.shipDescription = shipDescription;
		parts = new List<Part>();
		AddToConstruct(rootPart);
		shipSize = ShipConstruction.CalculateCraftSize(this);
		persistentId = FlightGlobals.GetUniquepersistentId();
		OverrideDefault = new bool[Vessel.NumOverrideGroups];
		OverrideActionControl = new KSPActionGroup[Vessel.NumOverrideGroups];
		OverrideAxisControl = new KSPAxisGroup[Vessel.NumOverrideGroups];
		OverrideGroupNames = new string[Vessel.NumOverrideGroups];
		vesselDeltaV = null;
	}

	public ShipConstruct(string shipName, string shipDescription, List<Part> parts)
	{
		this.shipName = shipName;
		this.shipDescription = shipDescription;
		this.parts = parts;
		for (int i = 0; i < parts.Count; i++)
		{
			parts[i].ship = this;
		}
		shipSize = ShipConstruction.CalculateCraftSize(this);
		persistentId = FlightGlobals.GetUniquepersistentId();
		OverrideDefault = new bool[Vessel.NumOverrideGroups];
		OverrideActionControl = new KSPActionGroup[Vessel.NumOverrideGroups];
		OverrideAxisControl = new KSPAxisGroup[Vessel.NumOverrideGroups];
		OverrideGroupNames = new string[Vessel.NumOverrideGroups];
		vesselDeltaV = null;
	}

	public ConfigNode SaveShip()
	{
		GameEvents.onAboutToSaveShip.Fire(this);
		ConfigNode configNode = new ConfigNode();
		configNode.AddValue("ship", shipName);
		configNode.AddValue("version", Versioning.version_major + "." + Versioning.version_minor + "." + Versioning.Revision);
		configNode.AddValue("description", shipDescription.Replace('\n', '\u00a8'));
		configNode.AddValue("type", shipFacility.ToString());
		shipSize = ShipConstruction.CalculateCraftSize(this);
		configNode.AddValue("size", KSPUtil.WriteVector(shipSize));
		configNode.AddValue("steamPublishedFileId", steamPublishedFileId);
		FlightGlobals.CheckVesselpersistentId(persistentId, null, removeOldId: false, addNewId: false);
		configNode.AddValue("persistentId", persistentId);
		configNode.AddValue("rot", rotation);
		configNode.AddValue("missionFlag", missionFlag);
		configNode.AddValue("vesselType", vesselType);
		if (OverrideDefault != null)
		{
			configNode.AddValue("OverrideDefault", ConfigNode.WriteBoolArray(OverrideDefault));
		}
		if (OverrideActionControl != null)
		{
			configNode.AddValue("OverrideActionControl", ConfigNode.WriteEnumIntArray(OverrideActionControl));
		}
		if (OverrideAxisControl != null)
		{
			configNode.AddValue("OverrideAxisControl", ConfigNode.WriteEnumIntArray(OverrideAxisControl));
		}
		if (OverrideGroupNames != null)
		{
			configNode.AddValue("OverrideGroupNames", ConfigNode.WriteStringArray(OverrideGroupNames));
		}
		int count = parts.Count;
		StringBuilder stringBuilder = StringBuilderCache.Acquire();
		for (int i = 0; i < count; i++)
		{
			Part part = parts[i];
			part.onBackup();
			ConfigNode configNode2 = configNode.AddNode("PART");
			configNode2.AddValue("part", part.partInfo.name + "_" + part.craftID);
			configNode2.AddValue("partName", part.partName);
			configNode2.AddValue("persistentId", part.persistentId);
			configNode2.AddValue("pos", KSPUtil.WriteVector(part.transform.position));
			configNode2.AddValue("attPos", KSPUtil.WriteVector(part.attPos));
			configNode2.AddValue("attPos0", KSPUtil.WriteVector(part.attPos0));
			configNode2.AddValue("rot", KSPUtil.WriteQuaternion(part.transform.rotation));
			configNode2.AddValue("attRot", KSPUtil.WriteQuaternion(part.attRotation));
			configNode2.AddValue("attRot0", KSPUtil.WriteQuaternion(part.attRotation0));
			configNode2.AddValue("mir", KSPUtil.WriteVector(part.mirrorVector));
			configNode2.AddValue("symMethod", part.symMethod.ToString());
			configNode2.AddValue("autostrutMode", part.autoStrutMode.ToString());
			configNode2.AddValue("rigidAttachment", part.rigidAttachment.ToString());
			configNode2.AddValue("istg", part.inverseStage);
			configNode2.AddValue("resPri", part.resourcePriorityOffset);
			configNode2.AddValue("dstg", part.defaultInverseStage);
			configNode2.AddValue("sidx", part.inStageIndex);
			configNode2.AddValue("sqor", part.manualStageOffset);
			configNode2.AddValue("sepI", part.separationIndex);
			configNode2.AddValue("attm", (int)part.attachMode);
			configNode2.AddValue("sameVesselCollision", part.sameVesselCollision);
			configNode2.AddValue("modCost", part.GetModuleCosts(part.partInfo.cost));
			configNode2.AddValue("modMass", part.GetModuleMass(part.partInfo.partPrefab.mass));
			configNode2.AddValue("modSize", part.GetModuleSize(part.prefabSize));
			if (part.customPartData != string.Empty)
			{
				configNode2.AddValue("cData", part.customPartData);
			}
			int count2 = part.children.Count;
			for (int j = 0; j < count2; j++)
			{
				configNode2.AddValue("link", part.children[j].partInfo.name + "_" + part.children[j].craftID);
			}
			count2 = part.symmetryCounterparts.Count;
			for (int k = 0; k < count2; k++)
			{
				configNode2.AddValue("sym", part.symmetryCounterparts[k].partInfo.name + "_" + part.symmetryCounterparts[k].craftID);
			}
			if (part.srfAttachNode.attachedPart != null)
			{
				stringBuilder.Clear();
				stringBuilder.Append(part.srfAttachNode.id).Append(",").Append(part.srfAttachNode.attachedPart.partInfo.name)
					.Append("_")
					.Append(part.srfAttachNode.attachedPart.craftID);
				if (!string.IsNullOrEmpty(part.srfAttachNode.srfAttachMeshName))
				{
					stringBuilder.Append(",").Append(part.srfAttachNode.srfAttachMeshName);
				}
				else
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(",").Append(KSPUtil.WriteVector(part.srfAttachNode.position, "|"));
				stringBuilder.Append(",").Append(KSPUtil.WriteVector(part.srfAttachNode.orientation, "|"));
				stringBuilder.Append(",").Append(KSPUtil.WriteVector(part.srfAttachNode.position, "|"));
				configNode2.AddValue("srfN", stringBuilder.ToString());
			}
			count2 = part.attachNodes.Count;
			for (int l = 0; l < count2; l++)
			{
				AttachNode attachNode = part.attachNodes[l];
				string value = KSPUtil.WriteVector(attachNode.originalPosition, "|");
				string value2 = KSPUtil.WriteVector(attachNode.originalOrientation, "|");
				string value3 = KSPUtil.WriteVector(attachNode.position, "|");
				string value4 = KSPUtil.WriteVector(attachNode.orientation, "|");
				string value5 = ((attachNode.attachedPart != null) ? attachNode.attachedPart.partInfo.name : "Null");
				string value6 = ((attachNode.attachedPart != null) ? attachNode.attachedPart.craftID.ToString() : "0");
				stringBuilder.Clear();
				stringBuilder.Append(attachNode.id).Append(",").Append(value5)
					.Append("_")
					.Append(value6)
					.Append("_")
					.Append(value)
					.Append("_")
					.Append(value2)
					.Append("_")
					.Append(value3)
					.Append("_")
					.Append(value4);
				configNode2.AddValue("attN", stringBuilder.ToString());
			}
			part.Events.OnSave(configNode2.AddNode("EVENTS"));
			part.Actions.OnSave(configNode2.AddNode("ACTIONS"));
			part.OnSave(configNode2.AddNode("PARTDATA"));
			count2 = part.Modules.Count;
			for (int m = 0; m < count2; m++)
			{
				part.Modules[m].Save(configNode2.AddNode("MODULE"));
			}
			count2 = part.Resources.Count;
			for (int n = 0; n < count2; n++)
			{
				part.Resources[n].Save(configNode2.AddNode("RESOURCE"));
			}
			if (part.vesselNaming != null)
			{
				part.vesselNaming.Save(configNode2.AddNode("VESSELNAMING"));
			}
		}
		stringBuilder.Release();
		return configNode;
	}

	public bool LoadShip(ConfigNode root, uint persistentID)
	{
		string errorString = "";
		return LoadShip(root, persistentID, returnErrors: false, out errorString);
	}

	public bool LoadShip(ConfigNode root, uint persistentID, bool returnErrors, out string errorString)
	{
		errorString = "";
		parts = new List<Part>();
		List<List<uint>> list = new List<List<uint>>();
		List<List<uint>> list2 = new List<List<uint>>();
		string partName = string.Empty;
		string craftID = string.Empty;
		string nodeID = string.Empty;
		string attnPartID = string.Empty;
		string attachMeshName = string.Empty;
		Vector3 attnPos = Vector3.zero;
		Vector3 attnRot = Vector3.zero;
		Vector3 attnActualPos = Vector3.zero;
		Vector3 attnActualRot = Vector3.zero;
		List<uint> list3 = new List<uint>();
		Dictionary<uint, uint> dictionary = new Dictionary<uint, uint>();
		persistentId = persistentID;
		shipFacility = EditorFacility.None;
		int i = 0;
		for (int count = root.values.Count; i < count; i++)
		{
			ConfigNode.Value value = root.values[i];
			switch (value.name)
			{
			case "rot":
				rotation = KSPUtil.ParseQuaternion(value.value);
				break;
			case "description":
				shipDescription = value.value.Replace('\u00a8', '\n');
				break;
			case "size":
				shipSize = KSPUtil.ParseVector3(value.value);
				break;
			case "type":
				shipFacility = (EditorFacility)Enum.Parse(typeof(EditorFacility), value.value);
				break;
			case "version":
				if (KSPUtil.CheckVersion(value.value, lastCompatibleMajor, lastCompatibleMinor, lastCompatibleRev) != VersionCompareResult.COMPATIBLE)
				{
					if (returnErrors)
					{
						errorString = Localizer.Format("#autoLOC_6002428");
					}
					else
					{
						PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), "Load Ship Failed", Localizer.Format("#autoLOC_6002426"), Localizer.Format("#autoLOC_6002428"), Localizer.Format("#autoLOC_417274"), persistAcrossScenes: true, HighLogic.UISkin);
					}
					return false;
				}
				break;
			case "OverrideGroupNames":
				OverrideGroupNames = ParseExtensions.ArrayMinSize(Vessel.NumOverrideGroups, ParseExtensions.ParseArray(value.value, StringSplitOptions.None));
				break;
			case "steamPublishedFileId":
				steamPublishedFileId = ulong.Parse(value.value);
				break;
			case "missionFlag":
				missionFlag = value.value;
				break;
			case "OverrideAxisControl":
				ParseExtensions.TryParseEnumIntArray<KSPAxisGroup>(value.value, out OverrideAxisControl);
				OverrideAxisControl = ParseExtensions.ArrayMinSize(Vessel.NumOverrideGroups, OverrideAxisControl);
				break;
			case "persistentId":
				if (persistentId == 0)
				{
					persistentId = uint.Parse(value.value);
				}
				break;
			case "ship":
				shipName = value.value;
				break;
			case "vesselType":
				vesselType = (VesselType)Enum.Parse(typeof(VesselType), value.value);
				break;
			case "OverrideDefault":
				ParseExtensions.TryParseBoolArray(value.value, out OverrideDefault);
				OverrideDefault = ParseExtensions.ArrayMinSize(Vessel.NumOverrideGroups, OverrideDefault);
				break;
			case "OverrideActionControl":
				ParseExtensions.TryParseEnumIntArray<KSPActionGroup>(value.value, out OverrideActionControl);
				OverrideActionControl = ParseExtensions.ArrayMinSize(Vessel.NumOverrideGroups, OverrideActionControl);
				break;
			}
		}
		persistentId = FlightGlobals.CheckVesselpersistentId(persistentId, null, removeOldId: false, addNewId: false);
		HashSet<string> hashSet = new HashSet<string>();
		int j = 0;
		for (int count2 = root.nodes.Count; j < count2; j++)
		{
			ConfigNode configNode = root.nodes[j];
			AvailablePart availablePart = null;
			partName = KSPUtil.GetPartName(configNode.GetValue("part"));
			availablePart = PartLoader.getPartInfoByName(partName);
			if ((availablePart == null || !availablePart.partPrefab) && !hashSet.Contains(partName))
			{
				hashSet.Add(partName);
			}
		}
		if (hashSet.Count > 0)
		{
			string text = string.Empty;
			HashSet<string>.Enumerator enumerator = hashSet.GetEnumerator();
			while (enumerator.MoveNext())
			{
				text = text + enumerator.Current + "\n";
			}
			if (returnErrors)
			{
				errorString = Localizer.Format("#autoLOC_6002425", shipName, text);
			}
			else
			{
				PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), "Craft Loading Error", Localizer.Format("#autoLOC_6002424"), Localizer.Format("#autoLOC_6002425", shipName, text), Localizer.Format("#autoLOC_417274"), persistAcrossScenes: true, HighLogic.UISkin);
			}
			return false;
		}
		int k = 0;
		for (int count3 = root.nodes.Count; k < count3; k++)
		{
			ConfigNode configNode = root.nodes[k];
			AvailablePart availablePart = null;
			Part part = null;
			List<uint> list4 = new List<uint>();
			List<uint> list5 = new List<uint>();
			int l = 0;
			for (int count4 = configNode.values.Count; l < count4; l++)
			{
				ConfigNode.Value value = configNode.values[l];
				switch (value.name)
				{
				case "attRot0":
					part.attRotation0 = KSPUtil.ParseQuaternion(value.value);
					break;
				case "srfN":
				{
					Vector3 secAxis = Vector3.zero;
					bool attachNodeInfo = KSPUtil.GetAttachNodeInfo(value.value, ref nodeID, ref attnPartID, ref attnPos, ref attachMeshName, ref attnRot, ref secAxis);
					AttachNode attachNode = part.srfAttachNode;
					attachNode.attachedPartId = uint.Parse(attnPartID);
					attachNode.srfAttachMeshName = attachMeshName;
					if (attachNodeInfo)
					{
						attachNode.originalPosition = (attachNode.position = attnPos);
						attachNode.orientation = (attachNode.originalOrientation = attnRot);
						attachNode.secondaryAxis = (attachNode.originalSecondaryAxis = secAxis);
					}
					break;
				}
				case "link":
					list4.Add(uint.Parse(KSPUtil.GetLinkID(value.value)));
					break;
				case "sidx":
					part.inStageIndex = int.Parse(value.value);
					break;
				case "attm":
					part.attachMode = (AttachModes)int.Parse(value.value);
					break;
				case "pSym":
					list5.Add(uint.Parse(value.value));
					break;
				case "sepI":
					part.separationIndex = int.Parse(value.value);
					break;
				case "symMethod":
					part.symMethod = (SymmetryMethod)Enum.Parse(typeof(SymmetryMethod), value.value);
					break;
				case "resPri":
					part.resourcePriorityOffset = int.Parse(value.value);
					break;
				case "rot":
					part.transform.rotation = KSPUtil.ParseQuaternion(value.value);
					break;
				case "attN":
				{
					bool attachNodeInfo = KSPUtil.GetAttachNodeInfo(value.value, ref nodeID, ref attnPartID, ref attnPos, ref attnRot, ref attnActualPos, ref attnActualRot);
					AttachNode attachNode = part.FindAttachNode(nodeID);
					uint num = uint.Parse(attnPartID);
					if (attachNode != null)
					{
						attachNode.attachedPartId = num;
						if (attachNodeInfo)
						{
							attachNode.originalPosition = attnPos;
							if (!float.IsPositiveInfinity(attnRot.x))
							{
								attachNode.originalOrientation = attnRot;
							}
							if (!float.IsPositiveInfinity(attnActualPos.x))
							{
								attachNode.position = attnActualPos;
							}
							if (!float.IsPositiveInfinity(attnActualRot.x))
							{
								attachNode.orientation = attnActualRot;
							}
						}
					}
					else if (num != 0)
					{
						Debug.Log("[Part: " + part.name + "] Part with ID " + attnPartID + " was attached to attach node " + nodeID + " which is no longer present in the part!");
					}
					break;
				}
				case "sameVesselCollision":
					part.sameVesselCollision = bool.Parse(value.value);
					break;
				case "rigidAttachment":
					part.rigidAttachment = bool.Parse(value.value);
					break;
				case "cData":
					part.customPartData = value.value;
					break;
				case "dstg":
					part.defaultInverseStage = int.Parse(value.value);
					break;
				case "pos":
					part.transform.position = KSPUtil.ParseVector3(value.value);
					break;
				case "sqor":
					part.manualStageOffset = int.Parse(value.value);
					break;
				case "persistentId":
					part.persistentId = uint.Parse(value.value);
					break;
				case "part":
					KSPUtil.GetPartInfo(value.value, ref partName, ref craftID);
					availablePart = PartLoader.getPartInfoByName(partName);
					if (availablePart != null && (bool)availablePart.partPrefab)
					{
						part = UnityEngine.Object.Instantiate(availablePart.partPrefab);
						part.gameObject.SetActive(value: true);
						if ((bool)FlightGlobals.fetch)
						{
							FlightGlobals.PersistentLoadedPartIds.Remove(part.persistentId);
						}
						part.name = partName;
						part.craftID = uint.Parse(craftID);
						part.partInfo = availablePart;
						part.symMethod = ((shipFacility == EditorFacility.const_2) ? SymmetryMethod.Mirror : SymmetryMethod.Radial);
						part.ship = this;
						if (!ResearchAndDevelopment.PartTechAvailable(availablePart))
						{
							shipPartsUnlocked = false;
						}
						break;
					}
					if (returnErrors)
					{
						errorString = Localizer.Format("#autoLOC_6002427", partName);
					}
					else
					{
						PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), "Loading Failed", Localizer.Format("#autoLOC_6002426"), Localizer.Format("#autoLOC_6002427", partName), Localizer.Format("#autoLOC_417274"), persistAcrossScenes: true, HighLogic.UISkin);
					}
					return false;
				case "attPos0":
					part.attPos0 = KSPUtil.ParseVector3(value.value);
					break;
				case "istg":
					part.inverseStage = int.Parse(value.value);
					break;
				case "autostrutMode":
					part.autoStrutMode = (Part.AutoStrutMode)Enum.Parse(typeof(Part.AutoStrutMode), value.value);
					break;
				case "attRot":
					part.attRotation = KSPUtil.ParseQuaternion(value.value);
					break;
				case "attPos":
					part.attPos = KSPUtil.ParseVector3(value.value);
					break;
				case "mir":
					part.SetMirror(KSPUtil.ParseVector3(value.value));
					break;
				case "sym":
					list5.Add(uint.Parse(KSPUtil.GetLinkID(value.value)));
					break;
				}
			}
			int moduleIndex = 0;
			int m = 0;
			for (int count5 = configNode.nodes.Count; m < count5; m++)
			{
				ConfigNode configNode2 = configNode.nodes[m];
				switch (configNode2.name)
				{
				case "MODULE":
					part.LoadModule(configNode2, ref moduleIndex);
					break;
				case "ACTIONS":
					part.Actions.OnLoad(configNode2);
					break;
				case "PARTDATA":
					part.OnLoad(configNode2);
					break;
				case "RESOURCE":
					part.SetResource(configNode2);
					break;
				case "VESSELNAMING":
					part.vesselNaming = new VesselNaming(configNode2);
					break;
				case "EVENTS":
					part.Events.OnLoad(configNode2);
					break;
				case "EFFECTS":
					part.LoadEffects(configNode2);
					break;
				}
			}
			uint num2 = part.persistentId;
			part.persistentId = FlightGlobals.CheckPartpersistentId(part.persistentId, part, removeOldId: false, addNewId: true, persistentID);
			if (part.persistentId != num2 && num2 != 0)
			{
				if (dictionary.ContainsKey(num2))
				{
					Debug.LogWarningFormat("[ShipConstruct]: The same persistentID existed on two parts in this ship - cannot track all part changes for {0}({1})", availablePart.name, num2);
				}
				else
				{
					dictionary.Add(num2, part.persistentId);
				}
			}
			parts.Add(part);
			list3.Add(part.craftID);
			list.Add(list4);
			list2.Add(list5);
		}
		List<uint> list6 = new List<uint>();
		List<uint> list7 = new List<uint>();
		int n = 0;
		for (int count6 = parts.Count; n < count6; n++)
		{
			Part part2 = parts[n];
			list7 = list2[n];
			list6 = list[n];
			int num3 = 0;
			for (int count7 = list6.Count; num3 < count7; num3++)
			{
				Part partByCraftID = parts.GetPartByCraftID(list6[num3]);
				if (!(partByCraftID == null))
				{
					partByCraftID.setParent(part2);
					partByCraftID.transform.parent = part2.transform;
					continue;
				}
				Debug.LogErrorFormat("[ShipConstruct]: Unable to load Craft. Part {0}-{1} Contains Invalid reference to part {2} File corrupted.", part2.partInfo.title, part2.craftID, list6[num3]);
				PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), "Loading Failed", Localizer.Format("#autoLOC_6002426"), Localizer.Format("#autoLOC_6002427", list6[num3]), Localizer.Format("#autoLOC_417274"), persistAcrossScenes: true, HighLogic.UISkin);
				return false;
			}
			int num4 = 0;
			for (int count8 = list7.Count; num4 < count8; num4++)
			{
				Part partByCraftID2 = parts.GetPartByCraftID(list7[num4]);
				part2.symmetryCounterparts.Add(partByCraftID2);
			}
			if (part2.srfAttachNode != null)
			{
				part2.srfAttachNode.owner = parts[n];
				part2.srfAttachNode.FindAttachedPart(parts);
			}
			int num5 = 0;
			for (int count9 = part2.attachNodes.Count; num5 < count9; num5++)
			{
				AttachNode attachNode2 = part2.attachNodes[num5];
				attachNode2.owner = part2;
				attachNode2.FindAttachedPart(parts);
			}
		}
		int num6 = 0;
		for (int count10 = parts.Count; num6 < count10; num6++)
		{
			Part part2 = parts[num6];
			part2.partTransform = part2.transform;
			part2.orgPos = part2.transform.root.InverseTransformPoint(part2.transform.position);
			part2.orgRot = Quaternion.Inverse(part2.transform.root.rotation) * part2.transform.rotation;
			part2.packed = true;
			part2.InitializeModules();
			if (part2.isTrackingShipConstructIDChanges(out var modules))
			{
				for (int num7 = 0; num7 < modules.Count; num7++)
				{
					modules[num7].UpdatePersistentIDs(dictionary);
				}
			}
		}
		StageManager.SetStageCount(parts);
		UpdateVesselNaming();
		if (HighLogic.LoadedSceneIsEditor)
		{
			if (vesselDeltaV == null)
			{
				vesselDeltaV = VesselDeltaV.Create(this);
			}
			else
			{
				vesselDeltaV.UpdateModuleEngines();
			}
		}
		Debug.Log(Localizer.Format(shipName) + " loaded!");
		return true;
	}

	public bool LoadShip(ConfigNode root)
	{
		return LoadShip(root, 0u);
	}

	public bool AreAllPartsConnected()
	{
		int count = parts.Count;
		Part part;
		do
		{
			if (count-- > 0)
			{
				part = parts[count];
				continue;
			}
			return true;
		}
		while (!(part.parent == null) || part.children.Count != 0);
		return false;
	}

	[Obsolete("because it would only work in flight. In the editor you have to check the manifest for isControlSource parts. If these parts have ModuleCommand check if it has enough crew through the manifest.")]
	public bool isControllable()
	{
		bool result = false;
		int count = parts.Count;
		for (int i = 0; i < count; i++)
		{
			if (parts[i].isControlSource > Vessel.ControlLevel.NONE)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public void Clear()
	{
		parts.Clear();
	}

	public void Add(Part p)
	{
		parts.Add(p);
	}

	public void Remove(Part p)
	{
		parts.Remove(p);
	}

	public bool Contains(Part p)
	{
		return parts.Contains(p);
	}

	public IEnumerator<Part> GetEnumerator()
	{
		return parts.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return parts.GetEnumerator();
	}

	public void AddToConstruct(Part rootPart)
	{
		if (!parts.Contains(rootPart))
		{
			parts.Add(rootPart);
			rootPart.ship = this;
		}
		int count = rootPart.children.Count;
		for (int i = 0; i < count; i++)
		{
			AddToConstruct(rootPart.children[i]);
		}
	}

	public float GetShipCosts(out float dryCost, out float fuelCost)
	{
		return GetShipCosts(out dryCost, out fuelCost, null);
	}

	public float GetShipCosts(out float dryCost, out float fuelCost, VesselCrewManifest vcm)
	{
		float num = 0f;
		dryCost = 0f;
		fuelCost = 0f;
		int count = parts.Count;
		while (count-- > 0)
		{
			Part part = parts[count];
			AvailablePart partInfo = part.partInfo;
			float num2 = partInfo.cost + part.GetModuleCosts(partInfo.cost);
			float num3 = 0f;
			int count2 = part.Resources.Count;
			while (count2-- > 0)
			{
				PartResource partResource = part.Resources[count2];
				PartResourceDefinition info = partResource.info;
				num2 -= info.unitCost * (float)partResource.maxAmount;
				num3 += info.unitCost * (float)partResource.amount;
			}
			dryCost += num2;
			fuelCost += num3;
		}
		num += dryCost + fuelCost;
		if (vcm != null)
		{
			num += vcm.GetCrewInventoryCosts();
		}
		return num;
	}

	public float GetShipMass(out float dryMass, out float fuelMass)
	{
		return GetShipMass(out dryMass, out fuelMass, null);
	}

	public float GetShipMass(out float dryMass, out float fuelMass, VesselCrewManifest vcm)
	{
		float num = 0f;
		dryMass = 0f;
		fuelMass = 0f;
		int count = parts.Count;
		while (count-- > 0)
		{
			Part part = parts[count];
			part.UpdateMass();
			float mass = part.mass;
			float num2 = 0f;
			int count2 = part.Resources.Count;
			while (count2-- > 0)
			{
				PartResource partResource = part.Resources[count2];
				PartResourceDefinition info = partResource.info;
				num2 += info.density * (float)partResource.amount;
			}
			dryMass += mass;
			fuelMass += num2;
		}
		num += dryMass + fuelMass;
		if (vcm != null)
		{
			num += (float)vcm.CrewCount * PhysicsGlobals.KerbalCrewMass;
			num += vcm.GetCrewInventoryMass();
			num += vcm.GetCrewResourceMass();
		}
		return num;
	}

	public float GetTotalMass()
	{
		float dryMass;
		float fuelMass;
		return GetShipMass(out dryMass, out fuelMass, ShipConstruction.ShipManifest);
	}

	public bool UpdateVesselNaming(bool noGameEvent = false)
	{
		Part part = VesselNaming.FindPriorityNamePart(this);
		if (part == null)
		{
			vesselNamedBy = null;
			return false;
		}
		RunVesselNamingUpdates(part, noGameEvent);
		return true;
	}

	public void RunVesselNamingUpdates(Part pNewName, bool noGameEvent)
	{
		string from = shipName;
		bool flag = false;
		if (HighLogic.LoadedSceneIsEditor && vesselNamedBy == null)
		{
			vesselNamedBy = EditorLogic.RootPart;
		}
		if (vesselNamedBy == null)
		{
			vesselNamedBy = pNewName;
		}
		if (vesselNamedBy.persistentId != pNewName.persistentId)
		{
			highestPriorityName = pNewName.vesselNaming.vesselName;
			vesselType = pNewName.vesselNaming.vesselType;
			vesselNamedBy = pNewName;
			flag = true;
		}
		else if (highestPriorityName != vesselNamedBy.vesselNaming.vesselName)
		{
			highestPriorityName = pNewName.vesselNaming.vesselName;
			vesselType = pNewName.vesselNaming.vesselType;
			flag = true;
		}
		else if (vesselType != vesselNamedBy.vesselNaming.vesselType)
		{
			vesselType = pNewName.vesselNaming.vesselType;
			flag = true;
		}
		pNewName.RefreshVesselNamingPAWDisplay();
		if (flag && !noGameEvent)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6005087", pNewName.vesselNaming.vesselName), 5f);
			GameEvents.onEditorVesselNamingChanged.Fire(new GameEvents.HostedFromToAction<ShipConstruct, string>(this, from, shipName));
		}
	}

	public void GetConnectedResourceTotals(int id, bool simulate, out double amount, out double maxAmount, bool pulling = true)
	{
		if (resourcePartSet != null)
		{
			resourcePartSet.GetConnectedResourceTotals(id, out amount, out maxAmount, pulling, simulate);
			return;
		}
		amount = 0.0;
		maxAmount = 0.0;
	}

	public double RequestResource(Part part, int id, double demand, bool usePriority, bool simulate)
	{
		if (resourcePartSet != null)
		{
			return resourcePartSet.RequestResource(part, id, demand, usePriority, simulate);
		}
		return 0.0;
	}

	public void UpdateResourceSets()
	{
		UpdateResourceSets(null, null);
	}

	public void UpdateResourceSets(List<Part> inputParts, SCCFlowGraph sccGraph)
	{
		if (inputParts == null)
		{
			inputParts = parts;
		}
		if (inputParts.Count == 0)
		{
			return;
		}
		HashSet<Part> b = new HashSet<Part>(inputParts);
		if (cachedResourcePartSetParts == null || !cachedResourcePartSetParts.DeepCompare(b))
		{
			PartSet.BuildPartSets(inputParts, null, PartSet.PartBuildSetOptions.Simulate, sccGraph);
			cachedResourcePartSetParts = b;
			int count = inputParts.Count;
			for (int i = 0; i < count; i++)
			{
				inputParts[i].ship = this;
			}
			if (resourcePartSet == null)
			{
				resourcePartSet = new PartSet(this, cachedResourcePartSetParts);
			}
			else
			{
				resourcePartSet.RebuildVessel(this, new HashSet<Part>(inputParts));
			}
		}
	}
}
