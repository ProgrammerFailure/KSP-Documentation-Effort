using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI;

public class UIDragAndDropController : MonoBehaviour
{
	public static UIDragAndDropController Instance;

	public RectTransform dragPlane;

	public static RectTransform mainTransform;

	public static UIList dragFromList;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIDragAndDropController()
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
	public static bool Register(RectTransform draggedTransform, RectTransform mainTransform, bool reparentOnDrag)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Unregister(RectTransform rt)
	{
		throw null;
	}
}
