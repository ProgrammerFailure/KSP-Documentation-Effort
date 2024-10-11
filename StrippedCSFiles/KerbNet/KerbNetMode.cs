using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace KerbNet;

public abstract class KerbNetMode
{
	public string name;

	public string displayName;

	public Sprite buttonSprite;

	private ColorBlock buttonTintBlock;

	public bool doCoordinatePass;

	public string localCoordinateInfoLabel;

	public bool doTerrainContourPass;

	public float terrainContourThreshold;

	public bool doAnomaliesPass;

	public bool doCustomPass;

	public UnityAction customButtonCallback;

	public string customButtonCaption;

	public string customButtonTooltip;

	protected KSPRandom backgroundGenerator;

	protected KSPRandom foregroundGenerator;

	private static ColorHSV _hsv;

	protected static ColorHSV hsv
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected KerbNetMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool AutoGenerateMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Init()
	{
		throw null;
	}

	public abstract void OnInit();

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnActivated()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnDeactivated()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool isModeActive(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual string GetErrorState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual string GetModeCaption()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Color GetModeColorTint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ColorBlock GetModeColorTintBlock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Precache(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnPrecache(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Color GetCoordinateColor(Vessel vessel, double latitude, double longitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void GetTerrainContourColors(Vessel vessel, out Color lowColor, out Color highColor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Color GetBackgroundColor(int x, int y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void InterpolateMainTexture(Texture2D tex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void InterpolateContourTexture(Texture2D tex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual string LocalCoordinateInfo(Vessel vessel, double centerLatitude, double centerLongitude, double waypointLatitude, double waypointLongitude, bool waypointInSpace)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void CustomPass(Texture2D tex)
	{
		throw null;
	}
}
