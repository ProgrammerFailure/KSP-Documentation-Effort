using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AngleRenderEject : MonoBehaviour
{
	private bool _isBecomingVisible;

	private bool _isBecomingVisible_LinesDone;

	private bool _isBecomingVisible_ArcDone;

	private bool _isBecomingVisible_VesselVectDone;

	private bool _isHiding;

	private DateTime StartDrawing;

	internal Vector3d vectPosWorldPivot;

	internal Vector3d vectPosWorldOrigin;

	internal Vector3d vectPosWorldOrbitLabel;

	internal Vector3d vectPosWorldEnd;

	private Vector3d vectPosPivotWorking;

	private Vector3d vectPosEndWorking;

	private GameObject objLineStart;

	private GameObject objLineStartArrow1;

	private GameObject objLineStartArrow2;

	private GameObject objLineEnd;

	private GameObject objLineArc;

	private GameObject objLineVesselVect;

	private GameObject objLineVesselVectArrow1;

	private GameObject objLineVesselVectArrow2;

	private LineRenderer lineStart;

	private LineRenderer lineStartArrow1;

	private LineRenderer lineStartArrow2;

	private LineRenderer lineVesselVect;

	private LineRenderer lineVesselVectArrow1;

	private LineRenderer lineVesselVectArrow2;

	private LineRenderer lineEnd;

	internal LineRenderer lineArc;

	internal PlanetariumCamera cam;

	internal int ArcPoints;

	internal int StartWidth;

	internal int EndWidth;

	private GUIStyle styleLabelEnd;

	private GUIStyle styleLabelTarget;

	private double time;

	public bool isDrawing
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

	public bool isVisible
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool isAngleVisible
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

	public bool isBecomingVisible
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsBecomingInvisible
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

	public CelestialBody bodyOrigin
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

	public Orbit VesselOrbit
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

	public double AngleTargetValue
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

	public bool DrawToRetrograde
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AngleRenderEject()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private LineRenderer InitLine(GameObject objToAttach, Color lineColor, int VertexCount, int InitialWidth, Material linesMaterial)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DrawAngle(CelestialBody bodyOrigin, double angleTarget, bool ToRetrograde, double time)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HideAngle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static double ClampDegrees360(double angle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static double ClampDegrees180(double angle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPreCull()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DrawArc(LineRenderer line, Vector3d vectStart, double Angle, double StartLength, double EndLength)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DrawLine(LineRenderer line, Vector3d pointStart, Vector3d pointEnd)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DrawLineArrow(LineRenderer line1, LineRenderer line2, Vector3d pointStart, Vector3d pointEnd, Vector3d vectPlaneNormal, double ArrowArmLength)
	{
		throw null;
	}
}
