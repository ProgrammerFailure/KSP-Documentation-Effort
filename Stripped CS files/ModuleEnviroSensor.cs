using System.Collections.Generic;
using ns9;

public class ModuleEnviroSensor : PartModule, IResourceConsumer
{
	public enum SensorType
	{
		TEMP,
		GRAV,
		const_2,
		PRES
	}

	[KSPField]
	public SensorType sensorType;

	[KSPField(guiFormat = "F3", guiActive = true, guiName = "#autoLOC_237023", guiUnits = "")]
	public string readoutInfo = "Off";

	[KSPField(isPersistant = true)]
	public bool sensorActive;

	public List<PartResourceDefinition> consumedResources;

	public static string cacheAutoLOC_7001406;

	public static string cacheAutoLOC_6004058;

	public static string cacheAutoLOC_7001413;

	public static string cacheAutoLOC_7001410;

	public static string cacheAutoLOC_6004059;

	public static string cacheAutoLOC_6004060;

	public static string cacheAutoLOC_237153;

	public List<PartResourceDefinition> GetConsumedResources()
	{
		return consumedResources;
	}

	[KSPAction("#autoLOC_6001431")]
	public void ToggleAction(KSPActionParam param)
	{
		if (param.type != 0 && (param.type != KSPActionType.Toggle || sensorActive))
		{
			sensorActive = false;
		}
		else
		{
			sensorActive = true;
		}
	}

	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001431")]
	public void Toggle()
	{
		sensorActive = !sensorActive;
	}

	public override void OnAwake()
	{
		if (consumedResources == null)
		{
			consumedResources = new List<PartResourceDefinition>();
		}
		else
		{
			consumedResources.Clear();
		}
		int i = 0;
		for (int count = resHandler.inputResources.Count; i < count; i++)
		{
			consumedResources.Add(PartResourceLibrary.Instance.GetDefinition(resHandler.inputResources[i].name));
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		if (resHandler.inputResources.Count == 0 && (node.HasValue("resourceName") || node.HasValue("powerConsumption") || base.part.partInfo == null || base.part.partInfo.partPrefab == null))
		{
			string value = "ElectricCharge";
			float value2 = 0.0075f;
			node.TryGetValue("resourceName", ref value);
			node.TryGetValue("powerConsumption", ref value2);
			ModuleResource moduleResource = new ModuleResource();
			moduleResource.name = value;
			moduleResource.title = KSPUtil.PrintModuleName(value);
			moduleResource.id = value.GetHashCode();
			moduleResource.rate = value2;
			resHandler.inputResources.Add(moduleResource);
		}
	}

	public void FixedUpdate()
	{
		if (sensorActive && base.part.started)
		{
			_ = TimeWarp.fixedDeltaTime;
			if (resHandler.UpdateModuleResourceInputs(ref readoutInfo, 1.0, 0.9, returnOnFirstLack: true))
			{
				if (!(UIPartActionController.Instance != null) || !UIPartActionController.Instance.ItemListContains(base.part, includeSymmetryCounterparts: false))
				{
					return;
				}
				switch (sensorType)
				{
				case SensorType.TEMP:
					readoutInfo = base.part.temperature.ToString("0.##") + " " + cacheAutoLOC_7001406;
					break;
				case SensorType.GRAV:
					if (base.vessel.orbit.altitude <= base.vessel.orbit.referenceBody.Radius * 3.0)
					{
						readoutInfo = Localizer.Format("#autoLOC_237120", FlightGlobals.getGeeForceAtPosition(base.transform.position).magnitude.ToString("00.00"));
					}
					else
					{
						readoutInfo = cacheAutoLOC_6004058;
					}
					break;
				case SensorType.const_2:
					readoutInfo = base.vessel.geeForce.ToString("00.0##") + cacheAutoLOC_7001413;
					break;
				case SensorType.PRES:
				{
					double staticPressurekPa = base.vessel.staticPressurekPa;
					if (staticPressurekPa > 0.0001)
					{
						readoutInfo = staticPressurekPa.ToString("0.00##") + " " + cacheAutoLOC_7001410;
					}
					else if (staticPressurekPa > 0.0)
					{
						readoutInfo = cacheAutoLOC_6004059;
					}
					else
					{
						readoutInfo = cacheAutoLOC_6004060;
					}
					break;
				}
				}
			}
			else if (readoutInfo != cacheAutoLOC_237153)
			{
				readoutInfo = cacheAutoLOC_237153;
			}
		}
		else
		{
			readoutInfo = cacheAutoLOC_237153;
		}
	}

	public override string GetInfo()
	{
		return resHandler.PrintModuleResources();
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLoc_6003040");
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_7001406 = Localizer.Format("#autoLOC_7001406");
		cacheAutoLOC_6004058 = Localizer.Format("#autoLOC_6004058");
		cacheAutoLOC_7001413 = Localizer.Format("#autoLOC_7001413");
		cacheAutoLOC_7001410 = Localizer.Format("#autoLOC_7001410");
		cacheAutoLOC_6004059 = Localizer.Format("#autoLOC_6004059");
		cacheAutoLOC_6004060 = Localizer.Format("#autoLOC_6004060");
		cacheAutoLOC_237153 = Localizer.Format("#autoLOC_237153");
	}
}
