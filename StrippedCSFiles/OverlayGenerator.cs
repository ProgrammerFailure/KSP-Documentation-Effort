using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class OverlayGenerator : MonoBehaviour
{
	private List<PartResourceDefinition> _resourceList;

	private PartResourceDefinition _displayResource;

	private CelestialBody _displayBody;

	private KSPRandom _rng;

	private static readonly Dictionary<int, Texture2D> textureDict;

	private static readonly Dictionary<int, Color32[]> resetDict;

	private static OverlayGenerator instance;

	public PartResourceDefinition DisplayResource
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public bool IsActive
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	public MapDisplayTypes DisplayMode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	public int Cutoff
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	public HarvestTypes DisplayResourceType
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	public int OverlayStyle
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	public List<PartResourceDefinition> ResourceList
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsMapView
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	public CelestialBody DisplayBody
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	private KSPRandom RNG
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static OverlayGenerator Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public OverlayGenerator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static OverlayGenerator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Texture2D GetTexture(int width, int height)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResetRNG()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnExitMapView()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnterMapView()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGameLoaded(ConfigNode data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMapFocusChange(MapObject target)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector2 GenerateScanPoint(float lon, float lat)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Texture2D CreateTexture(int height, bool checkForLock)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Color GetColor(float point, Color startColor, Color endColor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AverageLine(Texture2D tex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AverageDots(Texture2D tex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Interpolate(Texture2D tex, bool fuzzyEdges, int xOff, int yOff, int step, bool isEmpty = false, bool xOnly = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<PartResourceDefinition> LoadResourceList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetLerp(int lerpStep)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GenerateOverlay(bool checkForLock)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearDisplay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetMaxAbundance(PartResourceDefinition resource, int planetID, HarvestTypes harvestType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetMinAbundance(PartResourceDefinition resource, int planetID, HarvestTypes harvestType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetMaxAbundance()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetMinAbundance()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetPoint(Vector3 radial, bool checkForLock, float maxAbundance, float minAbundance, bool bypassCache = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Color FetchOverlayColor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Color InvertColor(Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FetchNextResource()
	{
		throw null;
	}
}
