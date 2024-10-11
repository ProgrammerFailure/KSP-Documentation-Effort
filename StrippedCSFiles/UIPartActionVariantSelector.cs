using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[UI_VariantSelector]
public class UIPartActionVariantSelector : UIPartActionFieldItem
{
	public GameObject prefabVariantButton;

	public Button buttonPrevious;

	public Button buttonNext;

	public ScrollRect scrollMain;

	public TextMeshProUGUI variantName;

	private int fieldValue;

	private UI_VariantSelector variantSelector;

	private List<UIPartActionVariantButton> buttonList;

	private List<int> lockedButtonIndexes;

	private bool hadSurfaceAttachedParts;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionVariantSelector()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Setup(UIPartActionWindow window, Part part, PartModule partModule, UI_Scene scene, UI_Control control, BaseField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorPartEvent(ConstructionEventType evt, Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void UpdateItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RefreshVariantButtons()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int GetSurfaceAttachedPartCount()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionVariantButton AddVariantButton(int entryIndex, string hexPrimaryColor, string hexSecondaryColor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int GetFieldValue()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnButtonPressed(int newIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetButtonOverText(int entryIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetButtonOverText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnButtonNext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnButtonPrev()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool SelectVariant(int newIndex)
	{
		throw null;
	}
}
