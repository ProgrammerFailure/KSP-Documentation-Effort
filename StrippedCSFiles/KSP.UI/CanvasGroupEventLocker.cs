using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupEventLocker : MonoBehaviour
{
	private CanvasGroup canvasGroup;

	[SerializeField]
	private bool lockWhileKSPediaOpen;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CanvasGroupEventLocker()
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
	private void Lock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Unlock()
	{
		throw null;
	}
}
