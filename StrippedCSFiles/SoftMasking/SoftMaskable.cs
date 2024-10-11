using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SoftMasking;

[ExecuteInEditMode]
[AddComponentMenu("")]
[DisallowMultipleComponent]
public class SoftMaskable : UIBehaviour, IMaterialModifier
{
	private ISoftMask _mask;

	private Graphic _graphic;

	private Material _replacement;

	private bool _affectedByMask;

	private bool _destroyed;

	private static List<ISoftMask> s_softMasks;

	private static List<Canvas> s_canvases;

	public bool shaderIsNotSupported
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

	public bool isMaskingEnabled
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ISoftMask mask
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		private set
		{
			throw null;
		}
	}

	private Graphic graphic
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	private Material replacement
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SoftMaskable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static SoftMaskable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Material GetModifiedMaterial(Material baseMaterial)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Invalidate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void MaskMightChanged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnTransformParentChanged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnCanvasHierarchyChanged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTransformChildrenChanged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RequestChildTransformUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool FindMaskOrDie()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static ISoftMask NearestMask(Transform transform, out bool processedByThisMask, bool enabledOnly = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static ISoftMask GetISoftMask(Transform current, bool shouldBeEnabled = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool IsOverridingSortingCanvas(Transform transform)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static T GetComponent<T>(Component component, List<T> cachedList) where T : class
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetShaderNotSupported(Material material)
	{
		throw null;
	}
}
