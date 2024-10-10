using System;
using System.Collections.Generic;
using Expansions.Missions;
using ns11;
using ns2;
using ns9;
using TMPro;
using UnityEngine;

public class VesselLabels : MonoBehaviour
{
	public enum InfoLevel
	{
		None,
		Icon,
		Range,
		Ident
	}

	public enum HoverBehaviour
	{
		NoChange,
		RevealNext,
		RevealAll
	}

	[Serializable]
	public class VesselLabelType
	{
		public Sprite sprite;

		public Color labelColor;

		public float minDrawDistance;

		public float maxDrawDistance;

		public float maxRangeDistance;

		public float maxIdentDistance;

		public bool fadeByDistance;

		public float alphaMin;

		public float alphaMax;

		public float iconSize = 12f;

		public HoverBehaviour hoverBehaviour = HoverBehaviour.RevealNext;
	}

	public VesselLabel labelPrefab;

	public NodeLabel nodeLabelPrefab;

	public VesselLabelType ShipLabel;

	public VesselLabelType DebrisLabel;

	public VesselLabelType DefaultLabel;

	public VesselLabelType FlagLabel;

	public VesselLabelType EVALabel;

	public VesselLabelType BaseLabel;

	public VesselLabelType StationLabel;

	public VesselLabelType NodeLabel;

	public List<BaseLabel> labels = new List<BaseLabel>();

	public string metricUnit;

	public static VesselLabels Instance;

	public Vector3 canvasCoordinates;

	public void Awake()
	{
		if (Instance != null)
		{
			Debug.Log("[VesselLabels] Only one instance can exist at any time. Destroying potential usurper.", Instance.gameObject);
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Instance = this;
		GameEvents.onVesselCreate.Add(OnVesselCreate);
		GameEvents.onVesselDestroy.Add(OnVesselDestroy);
		GameEvents.onGameSceneLoadRequested.Add(OnSceneLoad);
		metricUnit = Localizer.Format("#autoLOC_7001405");
	}

	public void OnDestroy()
	{
		GameEvents.onVesselCreate.Remove(OnVesselCreate);
		GameEvents.onVesselDestroy.Remove(OnVesselDestroy);
		GameEvents.onGameSceneLoadRequested.Remove(OnSceneLoad);
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void OnVesselCreate(Vessel v)
	{
		CreateVesselLabel(v);
	}

	public void OnVesselDestroy(Vessel v)
	{
		DestroyVesselLabel(v);
	}

	public void OnSceneLoad(GameScenes scene)
	{
		DestroyAllLabels();
	}

	public void CreateVesselLabel(Vessel v)
	{
		if (!(labelPrefab == null))
		{
			VesselLabel vesselLabel = UnityEngine.Object.Instantiate(labelPrefab);
			vesselLabel.transform.SetParent(base.transform);
			vesselLabel.Setup(this, v);
			if (GameSettings.FLT_VESSEL_LABELS)
			{
				ITargetable currentTarget = GetCurrentTarget();
				Vessel activeVessel = FlightGlobals.ActiveVessel;
				Vector3 activePosition = ((activeVessel != null) ? activeVessel.transform.position : Vector3.zero);
				ProcessLabel(vesselLabel, activeVessel, currentTarget, activePosition);
			}
			else
			{
				vesselLabel.Disable();
			}
			labels.Add(vesselLabel);
		}
	}

	public static NodeLabel AddNodeLabel(ITestNodeLabel testModule)
	{
		if (Instance != null)
		{
			return Instance.CreateNodeLabel(testModule);
		}
		return null;
	}

	public NodeLabel CreateNodeLabel(ITestNodeLabel testModule)
	{
		if (nodeLabelPrefab == null)
		{
			return null;
		}
		NodeLabel nodeLabel = UnityEngine.Object.Instantiate(nodeLabelPrefab);
		nodeLabel.transform.SetParent(base.transform);
		nodeLabel.Setup(this, testModule);
		if (GameSettings.FLT_VESSEL_LABELS)
		{
			ITargetable currentTarget = GetCurrentTarget();
			Vessel activeVessel = FlightGlobals.ActiveVessel;
			Vector3 activePosition = ((activeVessel != null) ? activeVessel.transform.position : Vector3.zero);
			ProcessLabel(nodeLabel, activeVessel, currentTarget, activePosition);
		}
		else
		{
			nodeLabel.Disable();
		}
		labels.Add(nodeLabel);
		return nodeLabel;
	}

	public static void RemoveNodeLabel(NodeLabel label)
	{
		UnityEngine.Object.Destroy(label.gameObject);
		if (Instance != null)
		{
			Instance.labels.Remove(label);
		}
	}

	public void DestroyVesselLabel(Vessel v)
	{
		VesselLabel label = GetLabel(v);
		if (label != null)
		{
			UnityEngine.Object.Destroy(label.gameObject);
			labels.Remove(label);
		}
	}

	public void DestroyAllLabels()
	{
		int i = 0;
		for (int count = labels.Count; i < count; i++)
		{
			BaseLabel baseLabel = labels[i];
			if (baseLabel != null)
			{
				UnityEngine.Object.Destroy(baseLabel.gameObject);
			}
		}
		labels.Clear();
	}

	public void DestroyNodeLabels()
	{
		for (int num = labels.Count - 1; num >= 0; num--)
		{
			NodeLabel nodeLabel = labels[num] as NodeLabel;
			if (nodeLabel != null)
			{
				UnityEngine.Object.Destroy(nodeLabel.gameObject);
				labels.RemoveAt(num);
			}
		}
	}

	public VesselLabel GetLabel(Vessel v)
	{
		int num = 0;
		int count = labels.Count;
		VesselLabel vesselLabel;
		while (true)
		{
			if (num < count)
			{
				vesselLabel = labels[num] as VesselLabel;
				if (vesselLabel != null && vesselLabel.vessel == v)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return vesselLabel;
	}

	public NodeLabel GetLabel(MENode node)
	{
		int num = 0;
		int count = labels.Count;
		NodeLabel nodeLabel;
		while (true)
		{
			if (num < count)
			{
				nodeLabel = labels[num] as NodeLabel;
				if (nodeLabel != null && nodeLabel.node == node)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return nodeLabel;
	}

	public VesselLabelType GetLabelType(Vessel v)
	{
		switch (v.vesselType)
		{
		case VesselType.Debris:
			return DebrisLabel;
		default:
			return DefaultLabel;
		case VesselType.Probe:
		case VesselType.Relay:
		case VesselType.Rover:
		case VesselType.Ship:
		case VesselType.Plane:
			return ShipLabel;
		case VesselType.Station:
			return StationLabel;
		case VesselType.Base:
			return BaseLabel;
		case VesselType.const_11:
			return EVALabel;
		case VesselType.Flag:
			return FlagLabel;
		}
	}

	public VesselLabelType GetLabelType(ITestNodeLabel testModule)
	{
		return NodeLabel;
	}

	public void OnDoubleClickLabel(VesselLabel l)
	{
		Vessel vessel = GetCurrentTarget()?.GetVessel();
		Vessel vessel2 = l.vessel;
		if (!vessel2.DiscoveryInfo.HaveKnowledgeAbout(DiscoveryLevels.StateVectors))
		{
			SpaceTracking.StartTrackingObject(vessel2);
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_482391", vessel2.DiscoveryInfo.displayName.Value), 5f, ScreenMessageStyle.UPPER_LEFT);
		}
		FlightGlobals.fetch.SetVesselTarget((vessel2 != vessel) ? vessel2 : null);
	}

	public void LateUpdate()
	{
		if (GameSettings.TOGGLE_LABELS.GetKeyDown())
		{
			GameSettings.FLT_VESSEL_LABELS = !GameSettings.FLT_VESSEL_LABELS;
			if (GameSettings.FLT_VESSEL_LABELS)
			{
				EnableAllLabels();
			}
			else
			{
				DisableAllLabels();
			}
		}
		if (GameSettings.FLT_VESSEL_LABELS)
		{
			ITargetable currentTarget = GetCurrentTarget();
			Vessel activeVessel = FlightGlobals.ActiveVessel;
			Vector3 activePosition = ((activeVessel != null) ? activeVessel.transform.position : Vector3.zero);
			int i = 0;
			for (int count = labels.Count; i < count; i++)
			{
				ProcessLabel(labels[i], activeVessel, currentTarget, activePosition);
			}
		}
	}

	public void ProcessLabel(BaseLabel label, Vessel activeVessel, ITargetable target, Vector3 activePosition)
	{
		if (!MapView.MapIsEnabled && FlightGlobals.ready)
		{
			VesselLabelType labelType = label.labelType;
			Vessel vessel = null;
			MENode mENode = null;
			bool hover = label.Hover;
			NodeLabel nodeLabel = label as NodeLabel;
			VesselLabel vesselLabel = label as VesselLabel;
			if (vesselLabel != null)
			{
				vessel = vesselLabel.vessel;
			}
			else if (nodeLabel != null)
			{
				mENode = nodeLabel.node;
			}
			if ((vessel == null || vessel == activeVessel) && mENode == null)
			{
				label.Disable();
				return;
			}
			Vessel vessel2 = target?.GetVessel();
			bool flag = vessel2 != null && vessel == vessel2;
			Vector3 vector = Vector3.zero;
			if (vesselLabel != null)
			{
				vector = (flag ? target.GetTransform().position : vessel.transform.position);
			}
			else if (mENode != null)
			{
				if (!nodeLabel.testModule.HasNodeLabel())
				{
					label.Disable();
					return;
				}
				vector = nodeLabel.testModule.GetWorldPosition();
			}
			float num = Vector3.SqrMagnitude(vector - activePosition);
			if (num > labelType.maxDrawDistance * labelType.maxDrawDistance)
			{
				label.Disable();
				return;
			}
			float num2 = Mathf.Sqrt(num);
			if (!hover && !flag && num2 < labelType.minDrawDistance)
			{
				((RectTransform)label.transform).anchoredPosition = UIMasterController.WorldToMainCanvas(vector, FlightCamera.fetch.mainCamera);
				label.Disable();
				return;
			}
			InfoLevel infoLevel = InfoLevel.Icon;
			if (num2 < labelType.maxRangeDistance)
			{
				infoLevel = InfoLevel.Range;
			}
			else if (num2 < labelType.maxIdentDistance)
			{
				infoLevel = InfoLevel.Ident;
			}
			if (hover)
			{
				switch (labelType.hoverBehaviour)
				{
				case HoverBehaviour.RevealAll:
					infoLevel = InfoLevel.Ident;
					break;
				case HoverBehaviour.RevealNext:
					if (infoLevel < InfoLevel.Ident)
					{
						infoLevel++;
					}
					break;
				}
			}
			if (infoLevel != 0)
			{
				canvasCoordinates = UIMasterController.WorldToMainCanvas(vector, FlightCamera.fetch.mainCamera);
				((RectTransform)label.transform).anchoredPosition = canvasCoordinates;
				if (canvasCoordinates.z < 0f)
				{
					label.Disable();
					return;
				}
				string text = ((num2 < 1200f) ? Localizer.Format("#autoLOC_482486", num2.ToString("0.0")) : ((num2 / 1000f).ToString("0.0") + metricUnit));
				Color color = (flag ? XKCDColors.ElectricLime : labelType.labelColor);
				label.Enable();
				label.text.color = color;
				label.icon.color = color;
				label.text.fontStyle = (flag ? FontStyles.Bold : FontStyles.Normal);
				switch (infoLevel)
				{
				case InfoLevel.Icon:
					label.text.text = string.Empty;
					break;
				case InfoLevel.Range:
					label.text.text = text;
					break;
				case InfoLevel.Ident:
				{
					string text2 = "";
					if (vessel != null)
					{
						text2 = vessel.GetDisplayName();
					}
					else if (mENode != null)
					{
						text2 = Localizer.Format(mENode.Title);
					}
					label.text.text = text2 + "\n" + text;
					if (mENode != null)
					{
						string extraText = nodeLabel.testModule.GetExtraText();
						if (!string.IsNullOrEmpty(extraText))
						{
							TextMeshProUGUI text3 = label.text;
							text3.text = text3.text + "\n" + Localizer.Format(extraText);
						}
					}
					break;
				}
				}
			}
			else
			{
				label.Disable();
			}
		}
		else
		{
			label.Disable();
		}
	}

	public void DisableAllLabels()
	{
		int i = 0;
		for (int count = labels.Count; i < count; i++)
		{
			BaseLabel baseLabel = labels[i];
			if (baseLabel != null)
			{
				baseLabel.Disable();
			}
		}
	}

	public void EnableAllLabels()
	{
		int i = 0;
		for (int count = labels.Count; i < count; i++)
		{
			BaseLabel baseLabel = labels[i];
			if (baseLabel != null)
			{
				baseLabel.Disable();
			}
		}
	}

	public ITargetable GetCurrentTarget()
	{
		if (FlightGlobals.fetch == null)
		{
			return null;
		}
		return FlightGlobals.fetch.VesselTarget;
	}
}
