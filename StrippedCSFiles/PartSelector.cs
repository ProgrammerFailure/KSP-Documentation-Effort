using System.Runtime.CompilerServices;
using UnityEngine;

public class PartSelector : MonoBehaviour
{
	private Part lastHoveredPart;

	private Part part;

	private Callback<Part> onSelect;

	private bool hover;

	private Color edgeHighlightColor;

	private Color edgeHoverColor;

	private Color highlightColor;

	private Color hoverColor;

	public Part Host
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartSelector()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static PartSelector Create(Part host, Callback<Part> onSelect, Color highlightColor, Color hoverColor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static PartSelector Create(Part host, Callback<Part> onSelect, Color highlightColor, Color hoverColor, Color edgeHighlightColor, Color edgeHoverColor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Dismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Setup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMouseEntered()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMouseExited()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}
}
