using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens.Flight;

public class NavBall : MonoBehaviour
{
	public Transform navBall;

	public Transform progradeVector;

	public Transform retrogradeVector;

	public Transform normalVector;

	public Transform antiNormalVector;

	public Transform radialInVector;

	public Transform radialOutVector;

	public Transform progradeWaypoint;

	public Transform retrogradeWaypoint;

	public TextMeshProUGUI headingText;

	public Image sideGaugeGee;

	public Image sideGaugeThrottle;

	private Vector3 rotationOffset;

	private Vector3 displayVelocity;

	private float displaySpeed;

	private Vector3 displayVelDir;

	private bool initialHeadingSet;

	[SerializeField]
	private float vectorUnitScale;

	[SerializeField]
	private float vectorUnitCutoff;

	[SerializeField]
	private float vectorVelocityThreshold;

	private Vector3 wCoM;

	private Vector3 obtVel;

	private Vector3 cbPos;

	private Vector3 normal;

	private Vector3 radial;

	public Transform target
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	public Quaternion attitudeGymbal
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	public Quaternion relativeGymbal
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	public Quaternion offsetGymbal
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	[SerializeField]
	public float VectorUnitScale
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[SerializeField]
	public float VectorUnitCutoff
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[SerializeField]
	public float VectorVelocityThreshold
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public NavBall()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetVectorAlphaTint(Transform vector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DrawOrbitalCues(bool drawCondition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetWaypoint(Transform target)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearWaypoint()
	{
		throw null;
	}
}
