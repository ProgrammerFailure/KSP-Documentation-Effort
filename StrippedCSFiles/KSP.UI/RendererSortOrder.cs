using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI;

[RequireComponent(typeof(Renderer))]
public class RendererSortOrder : MonoBehaviour
{
	[SerializeField]
	protected string layerName;

	[SerializeField]
	protected int sortingOrder;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RendererSortOrder()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Set")]
	public void Set()
	{
		throw null;
	}
}
