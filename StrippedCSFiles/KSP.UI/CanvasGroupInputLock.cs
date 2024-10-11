using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupInputLock : MonoBehaviour
{
	private static bool disableAllLocking;

	[SerializeField]
	private List<string> inputLockMask;

	private List<string> defaultMask;

	private ulong lockMask;

	private bool lockMaskDirty;

	private CanvasGroup canvasGroup;

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
	public CanvasGroupInputLock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static CanvasGroupInputLock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateLockMask()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitializeDefaultMask()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetInputMaskToDefault()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetInputMask(List<string> newMask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnInputLocksModified(GameEvents.FromToAction<ControlTypes, ControlTypes> fromTo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string ToBits(ControlTypes lockMask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string ToBits(ulong lockMask)
	{
		throw null;
	}
}
