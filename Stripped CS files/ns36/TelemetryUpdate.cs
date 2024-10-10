using System;
using CommNet;
using ns12;
using ns9;
using UnityEngine;
using UnityEngine.UI;

namespace ns36;

public class TelemetryUpdate : MonoBehaviour
{
	public CommNetUIModeButton modeButton;

	public Sprite NOSIG;

	public Sprite NOEP;

	public Sprite sprite_0;

	public Sprite sprite_1;

	public Sprite sprite_2;

	public Sprite EP0;

	public Sprite EP1;

	public Sprite EP2;

	public Sprite CK1;

	public Sprite CK2;

	public Sprite CK3;

	public Sprite CP1;

	public Sprite CP2;

	public Sprite CP3;

	public Sprite SS0;

	public Sprite SS1;

	public Sprite SS2;

	public Sprite SS3;

	public Sprite SS4;

	public Image arrow_icon;

	public Image firstHop_icon;

	public Image lastHop_icon;

	public Image control_icon;

	public Image signal_icon;

	public TooltipController_Text firstHop_tooltip;

	public TooltipController_Text arrow_tooltip;

	public TooltipController_Text lastHop_tooltip;

	public TooltipController_Text control_tooltip;

	public TooltipController_SignalStrength signal_tooltip;

	public static string cacheAutoLOC_461342;

	public static string cacheAutoLOC_461346;

	public static string cacheAutoLOC_461350;

	public static string cacheAutoLOC_461354;

	public static string cacheAutoLOC_461358;

	public static string cacheAutoLOC_461362;

	public static string cacheAutoLOC_461366;

	public static string cacheAutoLOC_461381;

	public static string cacheAutoLOC_461382;

	public static string cacheAutoLOC_461402;

	public static string cacheAutoLOC_461407;

	public static string cacheAutoLOC_461418;

	public static string cacheAutoLOC_461422;

	public static string cacheAutoLOC_461426;

	public static string cacheAutoLOC_461436;

	public static string cacheAutoLOC_461442;

	public static string cacheAutoLOC_461446;

	public static TelemetryUpdate Instance { get; set; }

	public virtual void Start()
	{
		if (!CommNetScenario.CommNetEnabled)
		{
			base.gameObject.SetActive(value: false);
		}
	}

	public virtual void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			HideModeButton();
		}
		else
		{
			UnityEngine.Object.DestroyImmediate(this);
		}
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
		if (MapView.MapIsEnabled)
		{
			ShowModeButton();
		}
		else
		{
			HideModeButton();
		}
		Vessel activeVessel = FlightGlobals.ActiveVessel;
		if (activeVessel == null)
		{
			return;
		}
		if (activeVessel.isEVA)
		{
			SetEVAUI(activeVessel);
			return;
		}
		if (activeVessel.connection == null)
		{
			ClearGui();
			return;
		}
		Sprite sprite = sprite_0;
		Sprite sprite2 = sprite_0;
		Sprite sprite3 = sprite_0;
		Sprite sprite4 = sprite_0;
		Sprite nOSIG = sprite_0;
		if (Time.timeSinceLevelLoad > 1f)
		{
			switch (activeVessel.connection.ControlState)
			{
			case VesselControlState.ProbePartial:
				sprite = CP2;
				control_tooltip.textString = cacheAutoLOC_461362;
				break;
			case VesselControlState.None:
				sprite = sprite_0;
				control_tooltip.textString = cacheAutoLOC_461342;
				break;
			case VesselControlState.Probe:
				sprite = CP1;
				control_tooltip.textString = cacheAutoLOC_461358;
				break;
			case VesselControlState.Kerbal:
				sprite = CK1;
				control_tooltip.textString = cacheAutoLOC_461346;
				break;
			case VesselControlState.KerbalFull:
				sprite = CK3;
				control_tooltip.textString = cacheAutoLOC_461354;
				break;
			case VesselControlState.ProbeFull:
				sprite = CP3;
				control_tooltip.textString = cacheAutoLOC_461366;
				break;
			case VesselControlState.KerbalPartial:
				sprite = CK2;
				control_tooltip.textString = cacheAutoLOC_461350;
				break;
			}
			if (activeVessel.connection.ControlState != VesselControlState.Kerbal && activeVessel.connection.ControlState != VesselControlState.Probe && activeVessel.connection.ControlState != 0)
			{
				switch (activeVessel.connection.Signal)
				{
				case SignalStrength.None:
					sprite4 = SS0;
					nOSIG = NOSIG;
					sprite3 = NOEP;
					arrow_tooltip.textString = cacheAutoLOC_461381;
					lastHop_tooltip.textString = cacheAutoLOC_461382;
					break;
				case SignalStrength.Red:
					sprite4 = SS1;
					break;
				case SignalStrength.Orange:
					sprite4 = SS2;
					break;
				case SignalStrength.Yellow:
					sprite4 = SS3;
					break;
				case SignalStrength.Green:
					sprite4 = SS4;
					break;
				}
				if (activeVessel.connection.Signal != 0)
				{
					if (activeVessel.Connection.CanScience)
					{
						arrow_tooltip.textString = cacheAutoLOC_461402;
						nOSIG = sprite_1;
					}
					else
					{
						arrow_tooltip.textString = cacheAutoLOC_461407;
						nOSIG = sprite_2;
					}
				}
				if (activeVessel.connection.IsConnected)
				{
					switch (activeVessel.connection.ControlPath.Last.hopType)
					{
					case HopType.Relay:
						sprite3 = EP0;
						lastHop_tooltip.textString = cacheAutoLOC_461418;
						break;
					case HopType.ControlPoint:
						sprite3 = EP1;
						lastHop_tooltip.textString = cacheAutoLOC_461422;
						break;
					case HopType.Home:
						sprite3 = EP2;
						lastHop_tooltip.textString = cacheAutoLOC_461426;
						break;
					}
					if (activeVessel.connection.ControlPath.Last.hopType != activeVessel.connection.ControlPath.First.hopType)
					{
						switch (activeVessel.connection.ControlPath.First.hopType)
						{
						case HopType.Relay:
							sprite2 = EP0;
							firstHop_tooltip.textString = cacheAutoLOC_461436;
							break;
						case HopType.ControlPoint:
							sprite2 = EP1;
							firstHop_tooltip.textString = cacheAutoLOC_461442;
							break;
						case HopType.Home:
							sprite2 = EP2;
							firstHop_tooltip.textString = cacheAutoLOC_461446;
							break;
						}
						firstHop_tooltip.textString += $" ({Math.Ceiling(activeVessel.connection.ControlPath.First.signalStrength * 100.0) * 0.01:0%})";
					}
				}
			}
		}
		SetIcon(arrow_icon, nOSIG);
		SetIcon(firstHop_icon, sprite2);
		SetIcon(lastHop_icon, sprite3);
		SetIcon(control_icon, sprite);
		SetIcon(signal_icon, sprite4);
	}

	public virtual void ClearGui()
	{
		SetIcon(arrow_icon, sprite_0);
		SetIcon(firstHop_icon, sprite_0);
		SetIcon(lastHop_icon, sprite_0);
		SetIcon(control_icon, sprite_0);
		SetIcon(signal_icon, sprite_0);
	}

	public virtual void SetIcon(Image image, Sprite sprite, bool disableOnBlank = true)
	{
		if (sprite == sprite_0)
		{
			if (disableOnBlank && image.gameObject.activeSelf)
			{
				image.gameObject.SetActive(value: false);
			}
			return;
		}
		if (disableOnBlank && !image.gameObject.activeSelf)
		{
			image.gameObject.SetActive(value: true);
		}
		image.sprite = sprite;
	}

	public virtual void SetEVAUI(Vessel v)
	{
		SetIcon(arrow_icon, sprite_0);
		SetIcon(firstHop_icon, sprite_0);
		SetIcon(lastHop_icon, sprite_0);
		SetIcon(control_icon, CK3);
		SetIcon(signal_icon, sprite_0);
		control_tooltip.textString = Localizer.Format("#autoLOC_445419");
	}

	public virtual void ShowModeButton()
	{
		if (!modeButton.gameObject.activeSelf)
		{
			modeButton.gameObject.SetActive(value: true);
		}
	}

	public virtual void HideModeButton()
	{
		if (modeButton.gameObject.activeSelf)
		{
			modeButton.gameObject.SetActive(value: false);
		}
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_461342 = Localizer.Format("#autoLOC_461342");
		cacheAutoLOC_461346 = Localizer.Format("#autoLOC_461346");
		cacheAutoLOC_461350 = Localizer.Format("#autoLOC_461350");
		cacheAutoLOC_461354 = Localizer.Format("#autoLOC_461354");
		cacheAutoLOC_461358 = Localizer.Format("#autoLOC_461358");
		cacheAutoLOC_461362 = Localizer.Format("#autoLOC_461362");
		cacheAutoLOC_461366 = Localizer.Format("#autoLOC_461366");
		cacheAutoLOC_461381 = Localizer.Format("#autoLOC_461381");
		cacheAutoLOC_461382 = Localizer.Format("#autoLOC_461382");
		cacheAutoLOC_461402 = Localizer.Format("#autoLOC_461402");
		cacheAutoLOC_461407 = Localizer.Format("#autoLOC_461407");
		cacheAutoLOC_461418 = Localizer.Format("#autoLOC_461418");
		cacheAutoLOC_461422 = Localizer.Format("#autoLOC_461422");
		cacheAutoLOC_461426 = Localizer.Format("#autoLOC_461426");
		cacheAutoLOC_461436 = Localizer.Format("#autoLOC_461436");
		cacheAutoLOC_461442 = Localizer.Format("#autoLOC_461442");
		cacheAutoLOC_461446 = Localizer.Format("#autoLOC_461446");
	}
}
