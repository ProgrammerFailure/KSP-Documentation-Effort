using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

[UI_Label]
public class UIPartActionThermalDisplay : UIPartActionItem
{
	internal BasePAWGroup pawGroup;

	[SerializeField]
	private TextMeshProUGUI txtThermalMass;

	[SerializeField]
	private TextMeshProUGUI txtTemperature;

	[SerializeField]
	private TextMeshProUGUI txtTemperatureExternal;

	[SerializeField]
	private TextMeshProUGUI txtConductionInternal;

	[SerializeField]
	private TextMeshProUGUI txtConductionExternal;

	[SerializeField]
	private TextMeshProUGUI txtRadiationExternal;

	[SerializeField]
	private TextMeshProUGUI txtGeneration;

	[SerializeField]
	private TextMeshProUGUI txtSkinToInternal;

	[SerializeField]
	private TextMeshProUGUI txtSkinThermalMass;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionThermalDisplay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Setup(UIPartActionWindow window, Part part, UI_Scene scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void UpdateItem()
	{
		throw null;
	}
}
