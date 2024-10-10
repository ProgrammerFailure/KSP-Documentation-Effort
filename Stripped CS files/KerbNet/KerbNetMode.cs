using System;
using ns17;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace KerbNet;

public abstract class KerbNetMode
{
	public string name = "ModeName";

	public string displayName = "ModeName";

	public Sprite buttonSprite;

	public ColorBlock buttonTintBlock;

	public bool doCoordinatePass;

	public string localCoordinateInfoLabel = "N/A";

	public bool doTerrainContourPass;

	public float terrainContourThreshold = 1f;

	public bool doAnomaliesPass;

	public bool doCustomPass;

	public UnityAction customButtonCallback;

	public string customButtonCaption = "Custom";

	public string customButtonTooltip = string.Empty;

	public KSPRandom backgroundGenerator;

	public KSPRandom foregroundGenerator;

	public static ColorHSV _hsv;

	public static ColorHSV hsv
	{
		get
		{
			if (_hsv == null)
			{
				_hsv = new ColorHSV(1f, 1f, 1f, 1f);
			}
			return _hsv;
		}
	}

	public KerbNetMode()
	{
	}

	public virtual bool AutoGenerateMode()
	{
		return true;
	}

	public void Init()
	{
		Color modeColorTint = GetModeColorTint();
		hsv.FromColor(modeColorTint);
		buttonTintBlock.colorMultiplier = 1f;
		buttonTintBlock.fadeDuration = 0.1f;
		hsv.a = 1f;
		hsv.v = 1f;
		buttonTintBlock.normalColor = hsv.ToColor();
		hsv.v = 0.9607f;
		buttonTintBlock.highlightedColor = hsv.ToColor();
		hsv.v = 0.7843f;
		buttonTintBlock.pressedColor = hsv.ToColor();
		buttonTintBlock.selectedColor = hsv.ToColor();
		buttonTintBlock.disabledColor = buttonTintBlock.pressedColor.smethod_0(0.5f);
		OnInit();
	}

	public abstract void OnInit();

	public virtual void OnActivated()
	{
	}

	public virtual void OnDeactivated()
	{
	}

	public virtual bool isModeActive(Vessel vessel)
	{
		return true;
	}

	public virtual string GetErrorState()
	{
		return null;
	}

	public virtual string GetModeCaption()
	{
		return string.Empty;
	}

	public virtual Color GetModeColorTint()
	{
		return Color.white;
	}

	public ColorBlock GetModeColorTintBlock()
	{
		return buttonTintBlock;
	}

	public void Precache(Vessel vessel)
	{
		int num = ((KerbNetDialog.Instance != null) ? KerbNetDialog.Instance.Seed : 0);
		backgroundGenerator = new KSPRandom(num + 1);
		foregroundGenerator = new KSPRandom(num + 2);
		OnPrecache(vessel);
	}

	public virtual void OnPrecache(Vessel vessel)
	{
	}

	public virtual Color GetCoordinateColor(Vessel vessel, double latitude, double longitude)
	{
		return Color.black;
	}

	public virtual void GetTerrainContourColors(Vessel vessel, out Color lowColor, out Color highColor)
	{
		lowColor = Color.black;
		highColor = Color.white;
	}

	public virtual Color GetBackgroundColor(int x, int y)
	{
		Vessel vessel = ((KerbNetDialog.Instance != null) ? KerbNetDialog.Instance.DisplayVessel : null);
		if (!(vessel == null) && !(vessel.mainBody == null) && GameSettings.KERBNET_BACKGROUND_FLUFF)
		{
			if (vessel.altitude < vessel.mainBody.minOrbitalDistance - vessel.mainBody.Radius)
			{
				float num = 0.05f + KerbNetDialog.NormalizedDistanceFromCenter(x, y) * 0.2f;
				float num2 = (float)backgroundGenerator.NextDouble() * 0.1f - 0.05f;
				hsv.h = 0f;
				hsv.s = 0f;
				hsv.v = num + num2;
				hsv.a = 1f;
			}
			else
			{
				if (backgroundGenerator.NextDouble() > 0.005)
				{
					return Color.black;
				}
				hsv.h = Convert.ToSingle(backgroundGenerator.NextDouble());
				hsv.s = Convert.ToSingle(backgroundGenerator.NextDouble()) * 0.2f;
				hsv.v = 1f;
				hsv.a = 1f;
			}
			return hsv.ToColor();
		}
		return Color.black;
	}

	public virtual void InterpolateMainTexture(Texture2D tex)
	{
		OverlayGenerator.Instance.Interpolate(tex, fuzzyEdges: false, 2, 2, 2);
		OverlayGenerator.Instance.Interpolate(tex, fuzzyEdges: false, 0, 2, 2);
		OverlayGenerator.Instance.Interpolate(tex, fuzzyEdges: false, 2, 0, 2);
		OverlayGenerator.Instance.Interpolate(tex, fuzzyEdges: false, 1, 1, 1);
		OverlayGenerator.Instance.Interpolate(tex, fuzzyEdges: false, 0, 1, 1);
		OverlayGenerator.Instance.Interpolate(tex, fuzzyEdges: false, 1, 0, 1);
	}

	public virtual void InterpolateContourTexture(Texture2D tex)
	{
		OverlayGenerator.Instance.Interpolate(tex, fuzzyEdges: false, 2, 2, 2);
		OverlayGenerator.Instance.Interpolate(tex, fuzzyEdges: false, 0, 2, 2);
		OverlayGenerator.Instance.Interpolate(tex, fuzzyEdges: false, 2, 0, 2);
	}

	public virtual string LocalCoordinateInfo(Vessel vessel, double centerLatitude, double centerLongitude, double waypointLatitude, double waypointLongitude, bool waypointInSpace)
	{
		return "N/A";
	}

	public virtual void CustomPass(Texture2D tex)
	{
	}
}
