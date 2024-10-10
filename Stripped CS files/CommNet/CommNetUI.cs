using System;
using System.Collections.Generic;
using System.ComponentModel;
using ns9;
using UnityEngine;
using Vectrosity;

namespace CommNet;

public class CommNetUI : MonoBehaviour
{
	public enum DisplayMode
	{
		[Description("#autoLOC_6003083")]
		None,
		[Description("#autoLOC_6003084")]
		FirstHop,
		[Description("#autoLOC_6003085")]
		Path,
		[Description("#autoLOC_6003086")]
		VesselLinks,
		[Description("#autoLOC_6003087")]
		Network
	}

	public static int ModeCount = Enum.GetValues(typeof(DisplayMode)).Length;

	public Color colorHigh = new Color(0f, 1f, 0f, 1f);

	public Color colorLow = new Color(0.5f, 0f, 0f, 1f);

	public Color colorWhole = XKCDColors.BabyBlue;

	public float colorLerpPower = 1.5f;

	public float lineWidth2D = 4f;

	public float lineWidth3D = 1f;

	public bool swapHighLow;

	public bool smoothColor;

	public static DisplayMode Mode = DisplayMode.Path;

	public static DisplayMode ModeTrackingStation = DisplayMode.Network;

	public static DisplayMode ModeFlightMap = DisplayMode.Path;

	public static float _lowColorBrightness = 0.25f;

	public Material lineMaterial;

	public Texture lineTexture;

	public bool draw3dLines;

	public bool refreshLines;

	public bool isShown;

	public bool useTSBehavior;

	public Vessel vessel;

	public List<Vector3> points;

	public VectorLine line;

	public static Texture telemetryTexture;

	public static Material telemetryMaterial;

	public MapObject obj;

	public static CommNetUI Instance { get; set; }

	public static float LowColorBrightnessFactor
	{
		get
		{
			return _lowColorBrightness;
		}
		set
		{
			_lowColorBrightness = Mathf.Clamp(value, 0.25f, 1f);
			if (Instance != null)
			{
				Instance.colorLow = new Color(_lowColorBrightness, 0f, 0f, 1f);
			}
		}
	}

	public virtual VectorLine Line => line;

	public static Texture TelemetryTexture
	{
		get
		{
			if (telemetryTexture != null)
			{
				return telemetryTexture;
			}
			telemetryTexture = Resources.Load<Texture>("Telemetry/TelemetryTexture");
			return telemetryTexture;
		}
	}

	public static Material TelemetryMaterial
	{
		get
		{
			if (telemetryMaterial != null)
			{
				return telemetryMaterial;
			}
			telemetryMaterial = Resources.Load<Material>("Telemetry/TelemetryMaterial");
			return telemetryMaterial;
		}
	}

	public virtual void Awake()
	{
		if (Instance != null && Instance != this)
		{
			UnityEngine.Object.Destroy(Instance);
		}
		Instance = this;
		lineMaterial = TelemetryMaterial;
		lineTexture = TelemetryTexture;
		LowColorBrightnessFactor = GameSettings.COMMNET_LOWCOLOR_BRIGHTNESSFACTOR;
	}

	public virtual void OnMapFocusChange(MapObject obj)
	{
		this.obj = obj;
		if (obj == null)
		{
			SetDisplayVessel();
		}
		else if (obj.type == MapObject.ObjectType.Vessel)
		{
			SetDisplayVessel(obj.vessel);
		}
	}

	public virtual void Start()
	{
		GameEvents.OnMapFocusChange.Add(OnMapFocusChange);
		points = new List<Vector3>(4);
		refreshLines = true;
		switch (HighLogic.LoadedScene)
		{
		default:
			UnityEngine.Object.Destroy(this);
			break;
		case GameScenes.TRACKSTATION:
			Show();
			break;
		case GameScenes.FLIGHT:
			GameEvents.OnMapEntered.Add(Show);
			GameEvents.OnMapExited.Add(Hide);
			break;
		}
	}

	public virtual void OnDestroy()
	{
		GameEvents.OnMapFocusChange.Remove(OnMapFocusChange);
		GameEvents.OnMapEntered.Remove(Show);
		GameEvents.OnMapExited.Remove(Hide);
		TimingManager.LateUpdateRemove(TimingManager.TimingStage.BetterLateThanNever, UpdateDisplay);
		if (line != null)
		{
			VectorLine.Destroy(ref line);
		}
		if (Instance == this)
		{
			Instance = null;
		}
	}

	public virtual void UpdateDisplay()
	{
		if (FlightGlobals.ActiveVessel == null)
		{
			useTSBehavior = true;
		}
		else
		{
			useTSBehavior = false;
			vessel = FlightGlobals.ActiveVessel;
		}
		if (vessel == null || vessel.connection == null || vessel.connection.Comm.Net == null)
		{
			useTSBehavior = true;
			if (ModeTrackingStation != 0)
			{
				if (ModeTrackingStation != DisplayMode.Network)
				{
					ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_118264", Localizer.Format(DisplayMode.Network.displayDescription())), 5f);
				}
				ModeTrackingStation = DisplayMode.Network;
			}
		}
		if (CommNetNetwork.Instance == null)
		{
			return;
		}
		if (useTSBehavior)
		{
			Mode = ModeTrackingStation;
		}
		else
		{
			Mode = ModeFlightMap;
		}
		CommNetwork commNet = CommNetNetwork.Instance.CommNet;
		CommNetVessel commNetVessel = null;
		CommNode commNode = null;
		CommPath commPath = null;
		if (vessel != null && vessel.connection != null && vessel.connection.Comm.Net != null)
		{
			commNetVessel = vessel.connection;
			commNode = commNetVessel.Comm;
			commPath = commNetVessel.ControlPath;
		}
		int count = points.Count;
		int num = 0;
		switch (Mode)
		{
		case DisplayMode.None:
			num = 0;
			break;
		case DisplayMode.FirstHop:
			if (commNetVessel.ControlState != VesselControlState.Probe && commNetVessel.ControlState != VesselControlState.Kerbal && !(commPath == null) && commPath.Count != 0)
			{
				commPath.First.GetPoints(points);
				num = 1;
			}
			else
			{
				num = 0;
			}
			break;
		case DisplayMode.Path:
			if (commNetVessel.ControlState != VesselControlState.Probe && commNetVessel.ControlState != VesselControlState.Kerbal && !(commPath == null) && commPath.Count != 0)
			{
				commPath.GetPoints(points);
				num = commPath.Count;
			}
			else
			{
				num = 0;
			}
			break;
		case DisplayMode.VesselLinks:
			num = commNode.Count;
			commNode.GetLinkPoints(points);
			break;
		case DisplayMode.Network:
			_ = vessel == null;
			if (commNet.Links.Count == 0)
			{
				num = 0;
				break;
			}
			commNet.GetLinkPoints(points);
			num = commNet.Links.Count;
			break;
		}
		if (num == 0)
		{
			if (line != null)
			{
				line.active = false;
			}
			points.Clear();
			return;
		}
		if (line != null)
		{
			line.active = true;
		}
		else
		{
			refreshLines = true;
		}
		ScaledSpace.LocalToScaledSpace(points);
		if (refreshLines || MapView.Draw3DLines != draw3dLines || count != points.Count || line == null)
		{
			CreateLine(ref line, points);
			draw3dLines = MapView.Draw3DLines;
			refreshLines = false;
		}
		float num2 = 1f;
		switch (Mode)
		{
		case DisplayMode.FirstHop:
			num2 = Mathf.Pow((float)commPath.First.signalStrength, colorLerpPower);
			if (swapHighLow)
			{
				line.SetColor(Color.Lerp(colorHigh, colorLow, num2), 0);
			}
			else
			{
				line.SetColor(Color.Lerp(colorLow, colorHigh, num2), 0);
			}
			break;
		case DisplayMode.Path:
		{
			int index2 = num;
			while (index2-- > 0)
			{
				num2 = Mathf.Pow((float)commPath[index2].signalStrength, colorLerpPower);
				if (swapHighLow)
				{
					line.SetColor(Color.Lerp(colorHigh, colorLow, num2), index2);
				}
				else
				{
					line.SetColor(Color.Lerp(colorLow, colorHigh, num2), index2);
				}
			}
			break;
		}
		case DisplayMode.VesselLinks:
		{
			Dictionary<CommNode, CommLink>.ValueCollection.Enumerator enumerator = commNode.Values.GetEnumerator();
			int num3 = 0;
			while (enumerator.MoveNext())
			{
				CommLink commLink = enumerator.Current;
				num2 = Mathf.Pow((float)commLink.GetSignalStrength(commLink.a != commNode, commLink.b != commNode), colorLerpPower);
				if (swapHighLow)
				{
					line.SetColor(Color.Lerp(colorHigh, colorLow, num2), num3);
				}
				else
				{
					line.SetColor(Color.Lerp(colorLow, colorHigh, num2), num3);
				}
				num3++;
			}
			break;
		}
		case DisplayMode.Network:
		{
			int index = num;
			while (index-- > 0)
			{
				CommLink commLink = commNet.Links[index];
				num2 = Mathf.Pow((float)commNet.Links[index].GetBestSignal(), colorLerpPower);
				if (swapHighLow)
				{
					line.SetColor(Color.Lerp(colorHigh, colorLow, num2), index);
				}
				else
				{
					line.SetColor(Color.Lerp(colorLow, colorHigh, num2), index);
				}
			}
			break;
		}
		}
		if (draw3dLines)
		{
			line.SetWidth(lineWidth3D);
			line.Draw3D();
		}
		else
		{
			line.SetWidth(lineWidth2D);
			line.Draw();
		}
	}

	public virtual void CreateLine(ref VectorLine l, List<Vector3> points)
	{
		if (l != null)
		{
			VectorLine.Destroy(ref l);
		}
		l = new VectorLine("CommNetUIVectorLine", points, lineWidth2D, LineType.Discrete);
		l.rectTransform.gameObject.layer = 31;
		l.material = lineMaterial;
		l.texture = lineTexture;
		l.smoothColor = smoothColor;
		l.UpdateImmediate = true;
	}

	public virtual void Show()
	{
		isShown = true;
		if (line != null)
		{
			line.active = true;
		}
		TimingManager.LateUpdateAdd(TimingManager.TimingStage.BetterLateThanNever, UpdateDisplay);
	}

	public virtual void Hide()
	{
		isShown = false;
		if (line != null)
		{
			line.active = false;
		}
		TimingManager.LateUpdateRemove(TimingManager.TimingStage.BetterLateThanNever, UpdateDisplay);
	}

	[ContextMenu("UpdateLine")]
	public virtual void UpdateLine()
	{
		CreateLine(ref line, points);
	}

	public virtual void SetDisplayVessel(Vessel v = null)
	{
		if (v == null)
		{
			vessel = FlightGlobals.ActiveVessel;
		}
		else
		{
			vessel = v;
		}
	}

	public virtual void NextMode()
	{
		SwitchMode(1);
	}

	public virtual void PreviousMode()
	{
		SwitchMode(-1);
	}

	public virtual void SwitchMode(int step)
	{
		DisplayMode newMode = (DisplayMode)((int)(Mode + step + ModeCount) % ModeCount);
		if (useTSBehavior)
		{
			ClampAndSetMode(ref ModeTrackingStation, newMode);
		}
		else
		{
			ClampAndSetMode(ref ModeFlightMap, newMode);
		}
		points.Clear();
		ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_118530", Localizer.Format(Mode.displayDescription())), 5f);
	}

	public virtual void ClampAndSetMode(ref DisplayMode curMode, DisplayMode newMode)
	{
		if (vessel == null || vessel.connection == null || vessel.connection.Comm.Net == null)
		{
			newMode = ((curMode == DisplayMode.None) ? DisplayMode.Network : DisplayMode.None);
		}
		Mode = (curMode = newMode);
	}

	public virtual void ResetMode()
	{
		Mode = DisplayMode.None;
		if (FlightGlobals.ActiveVessel == null)
		{
			ModeTrackingStation = Mode;
		}
		else
		{
			ModeFlightMap = Mode;
		}
		points.Clear();
		ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_118264", Localizer.Format(Mode.displayDescription())), 5f);
	}
}
