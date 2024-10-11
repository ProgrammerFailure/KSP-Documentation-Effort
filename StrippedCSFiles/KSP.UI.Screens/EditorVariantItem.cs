using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class EditorVariantItem : MonoBehaviour
{
	public TextMeshProUGUI title;

	public TextMeshProUGUI description;

	public TextMeshProUGUI partcount;

	public Image imagePrimaryColor;

	public Image imageSecomdaryColor;

	public Button btnSetVariantTheme;

	public Button btnApplyToVessel;

	private AvailableVariantTheme variantTheme;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EditorVariantItem()
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
	public void Create(EditorPartList partList, AvailableVariantTheme variant)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetColor(string hexPrimaryColor, string hexSecondaryColor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseInput_SetTheme()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseInput_ApplyTheme()
	{
		throw null;
	}
}
