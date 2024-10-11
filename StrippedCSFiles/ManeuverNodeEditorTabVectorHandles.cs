using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManeuverNodeEditorTabVectorHandles : ManeuverNodeEditorTab
{
	[SerializeField]
	private Slider precisionSlider;

	[SerializeField]
	private TextMeshProUGUI sliderTimeDVString;

	[SerializeField]
	private Button prevOrbitButton;

	[SerializeField]
	private Button nextOrbitButton;

	[SerializeField]
	private Button progradeVectorHandle;

	[SerializeField]
	private Button retrogradeVectorHandle;

	[SerializeField]
	private Button normalVectorHandle;

	[SerializeField]
	private Button antiNormalVectorHandle;

	[SerializeField]
	private Button radialInVectorHandle;

	[SerializeField]
	private Button radialOutVectorHandle;

	[SerializeField]
	private Button timeStepUp;

	[SerializeField]
	private Button timeStepDown;

	private double baseVectorStepValue;

	private double baseTimeStepValue;

	private double vectorPullAmount;

	private int exponent;

	private float multiplier;

	private static string cacheAutoLOC_6002317;

	private static string cacheAutoLOC_7001415;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ManeuverNodeEditorTabVectorHandles()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void SetInitialValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void UpdateUIElements()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetHandlesSensitivity()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPrecisionValueChanged(float newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void NextOrbitLoop()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PrevOrbitLoop()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void TimeStepUp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void TimeStepDown()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ProgradeStepUp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RetrogradeStepUp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void NormalStepUp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AntiNormalStepUp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RadialInStepUp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RadialOutStepUp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CacheLocalStrings()
	{
		throw null;
	}
}
