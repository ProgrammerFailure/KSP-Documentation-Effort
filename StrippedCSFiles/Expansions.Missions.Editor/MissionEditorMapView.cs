using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class MissionEditorMapView : MapView
{
	public OrbitGizmo orbitGizmoPrefab;

	protected new static MissionEditorMapView fetch
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public static OrbitGizmo OrbitGizmoPrefab
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionEditorMapView()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected internal override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Camera CreateMapFXCamera(int depthOffset, float nearPlane, float farPlane)
	{
		throw null;
	}
}
