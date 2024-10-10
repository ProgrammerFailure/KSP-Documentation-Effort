using System.Collections;
using System.Collections.Generic;
using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Actions;

public class ActionPartResourceDrain : ActionModule
{
	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, order = 40, resetValue = "0", guiName = "#autoLOC_8000008", Tooltip = "#autoLOC_8000009")]
	public float timePeriod;

	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, order = 30, resetValue = "0", guiName = "#autoLOC_8000010", Tooltip = "#autoLOC_8000011")]
	public float amountToDrain;

	[MEGUI_VesselPartSelect(order = 10, resetValue = "0", gapDisplay = true, guiName = "#autoLOC_8000012", Tooltip = "#autoLOC_8000013")]
	public VesselPartIDPair vesselPartIDs;

	[MEGUI_Dropdown(order = 20, SetDropDownItems = "APRD_SetDropDownValues", guiName = "#autoLOC_8000014", Tooltip = "#autoLOC_8000015")]
	public string resourceName = "";

	public Part part;

	public ProtoPartSnapshot protopart;

	public PartResourceDefinition resourcedef;

	public bool vesselDrain;

	public Vessel vessel;

	public bool vesselFound;

	public bool partLoaded;

	public bool partFound;

	public bool setup;

	public float drainPerSecond;

	public static float drainEveryXSecs = 0.5f;

	public static double epsilon = 1E-15;

	public float leftToDrain;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000007");
		vesselPartIDs = new VesselPartIDPair();
		restartOnSceneLoad = true;
		leftToDrain = amountToDrain;
	}

	public override void OnVesselPersistentIdChanged(uint oldId, uint newId)
	{
		if (vesselPartIDs.VesselID == oldId)
		{
			vesselPartIDs.VesselID = newId;
			Debug.LogFormat("[ActionPartResourceDrain]: Node ({0}) VesselId changed from {1} to {2}", node.id, oldId, newId);
		}
	}

	public override void OnPartPersistentIdChanged(uint vesselID, uint oldId, uint newId)
	{
		if (vesselPartIDs.VesselID == vesselID && vesselPartIDs.partID == oldId)
		{
			vesselPartIDs.partID = newId;
			Debug.LogFormat("[ActionPartResourceDrain]: Node ({0}) PartId changed from {1} to {2}", node.id, oldId, newId);
		}
	}

	public override void OnVesselsUndocking(Vessel oldVessel, Vessel newVessel)
	{
		if (vesselPartIDs.VesselID != oldVessel.persistentId && vesselPartIDs.VesselID != newVessel.persistentId)
		{
			return;
		}
		int num = 0;
		while (true)
		{
			if (num < newVessel.parts.Count)
			{
				if (vesselPartIDs.partID == newVessel.parts[num].persistentId)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		vesselPartIDs.VesselID = newVessel.persistentId;
		Debug.LogFormat("[ActionPartResourceDrain]: Node ({0}) VesselId changed on undocking from {1} to {2}", node.id, oldVessel.persistentId, newVessel.persistentId);
	}

	public override IEnumerator Fire()
	{
		isRunning = true;
		if (!setup)
		{
			if (!Setup())
			{
				isRunning = false;
				yield return null;
			}
			setup = true;
		}
		while (true)
		{
			if (vesselDrain)
			{
				if (vessel == null)
				{
					vesselFound = findVessel();
					if (!vesselFound)
					{
						Debug.LogWarning("[MissionExpansions] ActionPartResourceDrain failed to find vessel id: " + vesselPartIDs.VesselID);
						yield break;
					}
				}
				drainVessel();
			}
			else
			{
				if ((partLoaded && part == null) || (!partLoaded && protopart == null))
				{
					partFound = findPart();
					if (!partFound)
					{
						break;
					}
				}
				drainPart();
			}
			if ((double)leftToDrain > epsilon)
			{
				yield return new WaitForSeconds(drainEveryXSecs);
			}
			if (!((double)leftToDrain > epsilon))
			{
				isRunning = false;
				yield break;
			}
		}
		Debug.LogWarning("[MissionExpansions] ActionPartResourceDrain failed to find part id: " + vesselPartIDs.partID);
	}

	public bool Setup()
	{
		if (vesselPartIDs.VesselID != 0 && amountToDrain != 0f && !(resourceName == "") && (bool)FlightGlobals.fetch)
		{
			if (vesselPartIDs.VesselID != 0 && vesselPartIDs.partID == 0)
			{
				vesselDrain = true;
				vesselFound = findVessel();
				if (!vesselFound)
				{
					Debug.LogWarning("[ActionPartResourceDrain] Failed to find vessel id: " + vesselPartIDs.VesselID);
					return false;
				}
			}
			else
			{
				vesselDrain = false;
				partFound = findPart();
				if (!partFound)
				{
					Debug.LogWarning("[ActionPartResourceDrain] Failed to find part id: " + vesselPartIDs.partID);
					return false;
				}
			}
			resourcedef = PartResourceLibrary.Instance.GetDefinition(resourceName);
			if (resourcedef == null)
			{
				Debug.LogWarning("[MissionExpansions] ActionPartResourceDrain failed to find resource: " + resourceName);
				return false;
			}
			if (timePeriod <= 0f)
			{
				drainPerSecond = amountToDrain;
			}
			else
			{
				drainPerSecond = amountToDrain / timePeriod;
			}
			leftToDrain = amountToDrain;
			return true;
		}
		Debug.LogWarning("[ActionPartResourceDrain] Failed to initialize. vesselId =" + vesselPartIDs.VesselID + " partpersistentId =" + vesselPartIDs.partID + " amountToDrain =" + amountToDrain + " resourceName =" + resourceName);
		return false;
	}

	public bool findVessel()
	{
		vesselFound = false;
		uint key = node.mission.CurrentVesselID(node, vesselPartIDs.VesselID);
		if (FlightGlobals.PersistentVesselIds.ContainsKey(key))
		{
			vesselFound = true;
			vessel = FlightGlobals.PersistentVesselIds[key];
		}
		return vesselFound;
	}

	public bool findPart()
	{
		bool flag = false;
		partLoaded = FlightGlobals.FindLoadedPart(vesselPartIDs.partID, out part);
		if (!partLoaded)
		{
			return FlightGlobals.FindUnloadedPart(vesselPartIDs.partID, out protopart);
		}
		return true;
	}

	public void drainVessel()
	{
		float num = drainPerSecond * drainEveryXSecs;
		if (vessel.loaded)
		{
			double num2 = vessel.RequestResource(vessel.rootPart, resourcedef.id, num, usePriority: false);
			if (num2 <= 0.0)
			{
				leftToDrain = 0f;
			}
			else
			{
				GameEvents.Mission.onNodeChangedVesselResources.Fire(node, vessel, vessel.rootPart, null);
			}
			leftToDrain -= (float)num2;
			return;
		}
		float num3 = 0f;
		for (int i = 0; i < vessel.protoVessel.protoPartSnapshots.Count; i++)
		{
			float num4 = drainUnloadedPart(vessel.protoVessel.protoPartSnapshots[i]);
			if (!(num4 <= 0f))
			{
				num3 += num4;
				break;
			}
		}
		if (num3 <= 0f)
		{
			leftToDrain = 0f;
		}
		leftToDrain -= num3;
	}

	public void drainPart()
	{
		float num = drainPerSecond * drainEveryXSecs;
		if (partLoaded)
		{
			double num2 = part.RequestResource(resourcedef.id, num, ResourceFlowMode.NO_FLOW);
			if (num2 <= 0.0)
			{
				leftToDrain = 0f;
			}
			else
			{
				GameEvents.Mission.onNodeChangedVesselResources.Fire(node, vessel, part, null);
			}
			leftToDrain -= (float)num2;
		}
		else
		{
			float num3 = drainUnloadedPart(protopart);
			if (num3 <= 0f)
			{
				leftToDrain = 0f;
			}
			else
			{
				GameEvents.Mission.onNodeChangedVesselResources.Fire(node, vessel, null, protopart);
			}
			leftToDrain -= num3;
		}
	}

	public float drainUnloadedPart(ProtoPartSnapshot InProtoPart)
	{
		float num = drainPerSecond * drainEveryXSecs;
		int num2 = 0;
		while (true)
		{
			if (num2 < InProtoPart.resources.Count)
			{
				if (InProtoPart.resources[num2].resourceName == resourceName)
				{
					break;
				}
				num2++;
				continue;
			}
			return 0f;
		}
		if (InProtoPart.resources[num2].amount >= (double)num)
		{
			InProtoPart.resources[num2].amount -= num;
			return num;
		}
		return 0f;
	}

	public List<MEGUIDropDownItem> APRD_SetDropDownValues()
	{
		List<MEGUIDropDownItem> list = new List<MEGUIDropDownItem>();
		IEnumerator<PartResourceDefinition> enumerator = PartResourceLibrary.Instance.resourceDefinitions.GetEnumerator();
		while (enumerator.MoveNext())
		{
			PartResourceDefinition current = enumerator.Current;
			list.Add(new MEGUIDropDownItem(current.name, current.name, current.displayName));
		}
		enumerator.Dispose();
		return list;
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (node != null)
		{
			node.UpdateInfoOnRefresh = true;
		}
		if (field.name == "vesselPartIDs" && node.mission != null)
		{
			string text = "";
			string vesselName = "";
			bool flag = false;
			VesselSituation vesselSituation = node.mission.GetVesselSituationByVesselID(vesselPartIDs.VesselID);
			if (vesselSituation != null)
			{
				vesselName = vesselSituation.vesselName;
			}
			else if (node.mission.UpdateFromMappedVesselIDs(vesselPartIDs.VesselID, ref vesselName, ref vesselSituation))
			{
				flag = true;
				if (vesselSituation == null)
				{
					text = Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8000001"), vesselName) + "\n";
				}
			}
			if (vesselSituation != null)
			{
				text = Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8000001"), vesselName) + "\n";
				if (flag)
				{
					text = text + Localizer.Format("#autoLOC_6002491", vesselSituation.vesselName) + "\n";
				}
			}
			return text + Localizer.Format("#autoLOC_8004190", field.guiName, Localizer.Format(vesselPartIDs.partName));
		}
		if (field.name == "resourceName")
		{
			string text2 = Localizer.Format("#autoLOC_6003083");
			if (!string.IsNullOrEmpty(resourceName))
			{
				resourcedef = PartResourceLibrary.Instance.GetDefinition(resourceName);
				if (resourcedef != null)
				{
					text2 = resourcedef.displayName;
				}
			}
			return Localizer.Format("#autoLOC_8004190", field.guiName, text2);
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004004");
	}

	public override void RunValidation(MissionEditorValidator validator)
	{
		if (vesselPartIDs != null)
		{
			vesselPartIDs.ValidatePartAgainstCraft(node, validator);
		}
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("timePeriod", timePeriod);
		node.AddValue("amountToDrain", amountToDrain);
		node.AddValue("resourceName", resourceName);
		node.AddValue("leftToDrain", leftToDrain);
		vesselPartIDs.Save(node);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("timePeriod", ref timePeriod);
		node.TryGetValue("amountToDrain", ref amountToDrain);
		node.TryGetValue("resourceName", ref resourceName);
		node.TryGetValue("leftToDrain", ref leftToDrain);
		vesselPartIDs.Load(node);
	}
}
