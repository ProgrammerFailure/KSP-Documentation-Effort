using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class EditorInventoryOnlyIcon : MonoBehaviour
{
	public TextMeshProUGUI StackAmountText;

	public RawImage InventoryItemThumbnail;

	public Image Background;

	public Transform StackParent;

	private int currentStack;

	public float StackSeparation;

	public int MaxStackImages;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EditorInventoryOnlyIcon()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetIconTexture(Texture texture)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetStackAmount(int stackAmount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClearStackImages()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ToggleBackgroundVisibility(bool visible)
	{
		throw null;
	}
}
