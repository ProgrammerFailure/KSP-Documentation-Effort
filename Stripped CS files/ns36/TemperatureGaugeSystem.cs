using System.Collections.Generic;
using UnityEngine;

namespace ns36;

public class TemperatureGaugeSystem : MonoBehaviour
{
	public TemperatureGauge temperatureGaugePrefab;

	public float zMin;

	public float zSeperation;

	public Vessel activeVessel;

	public int partCount = -1;

	public List<TemperatureGauge> gauges = new List<TemperatureGauge>();

	public int gaugeCount;

	public List<TemperatureGauge> visibleGauges = new List<TemperatureGauge>();

	public int visibleGaugeCount;

	public static TemperatureGaugeSystem Instance { get; set; }

	public virtual void Awake()
	{
		Instance = this;
	}

	public virtual void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public virtual void Update()
	{
		if (!FlightGlobals.ready || !HighLogic.LoadedSceneIsFlight)
		{
			return;
		}
		if (GameSettings.TEMPERATURE_GAUGES_MODE >= 1 && CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.Flight)
		{
			if (TestRecreate())
			{
				CreateGauges();
			}
			if (TestRebuild())
			{
				RebuildGaugeList();
			}
			if (gaugeCount == 0)
			{
				return;
			}
			gauges.Sort();
			visibleGaugeCount = 0;
			bool flag = false;
			for (int i = 0; i < gaugeCount; i++)
			{
				gauges[i].GaugeUpdate();
				if (gauges[i].gaugeActive)
				{
					if (visibleGauges.Count > visibleGaugeCount && visibleGauges[visibleGaugeCount] != gauges[i])
					{
						flag = true;
						visibleGauges[visibleGaugeCount] = gauges[i];
					}
					visibleGaugeCount++;
				}
			}
			if (!flag)
			{
				return;
			}
			int num = 0;
			while (true)
			{
				if (num < visibleGaugeCount)
				{
					if (num > visibleGauges.Count - 1 || visibleGauges[num] == null)
					{
						break;
					}
					visibleGauges[num].rTrf.SetSiblingIndex(num);
					num++;
					continue;
				}
				return;
			}
			CreateGauges();
		}
		else if (gaugeCount > 0)
		{
			DestroyGauges();
		}
	}

	public virtual bool TestRecreate()
	{
		if (FlightGlobals.ActiveVessel != activeVessel)
		{
			return true;
		}
		return false;
	}

	public virtual bool TestRebuild()
	{
		if (FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.parts.Count != partCount)
		{
			return true;
		}
		return false;
	}

	public virtual void CreateGauges()
	{
		DestroyGauges();
		if (FlightGlobals.ActiveVessel == null)
		{
			return;
		}
		activeVessel = FlightGlobals.ActiveVessel;
		partCount = activeVessel.parts.Count;
		for (int i = 0; i < partCount; i++)
		{
			Part part = activeVessel.parts[i];
			if (part.thermalMass != 0.0)
			{
				TemperatureGauge temperatureGauge = Object.Instantiate(temperatureGaugePrefab);
				temperatureGauge.transform.SetParent(base.transform, worldPositionStays: false);
				temperatureGauge.Setup(part, PhysicsGlobals.TemperatureGaugeHighlightThreshold, PhysicsGlobals.TemperatureGaugeThreshold);
				gauges.Add(temperatureGauge);
				visibleGauges.Add(temperatureGauge);
				gaugeCount++;
			}
		}
	}

	public virtual void DestroyGauges()
	{
		for (int i = 0; i < gaugeCount; i++)
		{
			if (gauges[i] != null)
			{
				Object.Destroy(gauges[i].gameObject);
			}
		}
		gauges.Clear();
		gaugeCount = 0;
		visibleGauges.Clear();
		visibleGaugeCount = 0;
		activeVessel = null;
		partCount = -1;
	}

	public virtual void RebuildGaugeList()
	{
		if (FlightGlobals.ActiveVessel == null)
		{
			DestroyGauges();
			return;
		}
		activeVessel = FlightGlobals.ActiveVessel;
		partCount = activeVessel.parts.Count;
		int index = gaugeCount;
		while (index-- > 0)
		{
			if (gauges[index] == null)
			{
				gauges.RemoveAt(index);
				gaugeCount--;
				continue;
			}
			bool flag = false;
			for (int i = 0; i < partCount; i++)
			{
				if (gauges[index].Part.persistentId == activeVessel.parts[i].persistentId)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				Object.Destroy(gauges[index].gameObject);
				gauges.RemoveAt(index);
				gaugeCount--;
			}
		}
		int count = visibleGauges.Count;
		while (count-- > 0)
		{
			if (visibleGauges[count] == null)
			{
				visibleGauges.RemoveAt(count);
			}
			bool flag2 = false;
			for (int j = 0; j < gaugeCount; j++)
			{
				if (visibleGauges[count].Part.persistentId == gauges[j].Part.persistentId)
				{
					flag2 = true;
					break;
				}
			}
			if (!flag2)
			{
				Object.Destroy(visibleGauges[count].gameObject);
				visibleGauges.RemoveAt(count);
			}
		}
		for (int k = 0; k < partCount; k++)
		{
			Part part = activeVessel.parts[k];
			if (part.thermalMass == 0.0)
			{
				continue;
			}
			bool flag2 = false;
			for (int l = 0; l < gaugeCount; l++)
			{
				if (part.persistentId == gauges[l].Part.persistentId)
				{
					flag2 = true;
					break;
				}
			}
			if (!flag2)
			{
				TemperatureGauge temperatureGauge = Object.Instantiate(temperatureGaugePrefab);
				temperatureGauge.transform.SetParent(base.transform, worldPositionStays: false);
				temperatureGauge.Setup(part, PhysicsGlobals.TemperatureGaugeHighlightThreshold, PhysicsGlobals.TemperatureGaugeThreshold);
				gauges.Add(temperatureGauge);
				visibleGauges.Add(temperatureGauge);
				gaugeCount++;
			}
		}
		visibleGaugeCount = visibleGauges.Count;
	}
}
