using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI;

[AddComponentMenu("KSP/Graphic Raycaster")]
[RequireComponent(typeof(Canvas))]
public class KSPGraphicRaycaster : GraphicRaycaster
{
	[SerializeField]
	private List<string> inputLockMask;

	private ulong lockMask;

	private bool lockMaskDirty;

	public List<string> InputLockMask
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ulong LockMask
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
	public KSPGraphicRaycaster()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateLockMask()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
	{
		throw null;
	}
}
