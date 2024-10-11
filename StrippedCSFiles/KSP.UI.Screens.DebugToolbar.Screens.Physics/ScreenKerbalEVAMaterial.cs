using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens.DebugToolbar.Screens.Physics;

public class ScreenKerbalEVAMaterial : MonoBehaviour
{
	public TextMeshProUGUI headerText;

	public Button applyChanges;

	private float dynamicFriction;

	public float dynamicFrictionMin;

	public float dynamicFrictionMax;

	public Slider dynamicFrictionSlider;

	public TextMeshProUGUI dynamicFrictionText;

	public TextMeshProUGUI dynamicFrictionCurrentValueText;

	private float staticFriction;

	public float staticFrictionMin;

	public float staticFrictionMax;

	public Slider staticFrictionSlider;

	public TextMeshProUGUI staticFrictionText;

	public TextMeshProUGUI staticFrictionCurrentValueText;

	private float bounciness;

	public float bouncinessMin;

	public float bouncinessMax;

	public Slider bouncinessSlider;

	public TextMeshProUGUI bouncinessText;

	public TextMeshProUGUI bouncinessCurrentValueText;

	private PhysicMaterialCombine frictionCombine;

	public TextMeshProUGUI frictionCombineText;

	public TextMeshProUGUI currentFrictionCombineText;

	public Button frictionAverageButton;

	public Button frictionMinimumButton;

	public Button frictionMultiplyButton;

	public Button frictionMaximumButton;

	private PhysicMaterialCombine bounceCombine;

	public TextMeshProUGUI bounceCombineText;

	public TextMeshProUGUI currentBounceCombineText;

	public Button bounceAverageButton;

	public Button bounceMinimumButton;

	public Button bounceMultiplyButton;

	public Button bounceMaximumButton;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScreenKerbalEVAMaterial()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddListeners()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SlidersUpdated(float arg)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateValuesFromPhysiscsGlobals()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateFrictionCombine(PhysicMaterialCombine newValue, TextMeshProUGUI currentText)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateBounceCombine(PhysicMaterialCombine newValue, TextMeshProUGUI currentText)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ApplyChanges()
	{
		throw null;
	}
}
