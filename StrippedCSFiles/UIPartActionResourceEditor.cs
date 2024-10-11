using System.Runtime.CompilerServices;
using KSP.UI;
using KSP.UI.TooltipTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPartActionResourceEditor : UIPartActionResourceItem
{
	public TextMeshProUGUI resourceName;

	public TextMeshProUGUI resourceAmnt;

	public TextMeshProUGUI resourceMax;

	public Slider slider;

	public GameObject sliderContainer;

	public GameObject numericContainer;

	public TextMeshProUGUI fieldNameNumeric;

	public TMP_InputField inputField;

	public UIButtonToggle flowBtn;

	public static float StepIncrement;

	private int displayNameLimit;

	private TooltipController_Text tooltip;

	private string displayText;

	private bool bypassSliderRounding;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionResourceEditor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static UIPartActionResourceEditor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Setup(UIPartActionWindow window, Part part, UI_Scene scene, UI_Control control, PartResource resource)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsItemValid()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void UpdateItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSliderChanged(float obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSliderChangeProcess()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FlowBtnToggled()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetButtonState(bool state, bool forceButton)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFieldInput(string input)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleNumericSlider(bool numeric)
	{
		throw null;
	}
}
