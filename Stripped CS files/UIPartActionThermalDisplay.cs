using ns9;
using TMPro;
using UnityEngine;

[UI_Label]
public class UIPartActionThermalDisplay : UIPartActionItem
{
	public BasePAWGroup pawGroup;

	[SerializeField]
	public TextMeshProUGUI txtThermalMass;

	[SerializeField]
	public TextMeshProUGUI txtTemperature;

	[SerializeField]
	public TextMeshProUGUI txtTemperatureExternal;

	[SerializeField]
	public TextMeshProUGUI txtConductionInternal;

	[SerializeField]
	public TextMeshProUGUI txtConductionExternal;

	[SerializeField]
	public TextMeshProUGUI txtRadiationExternal;

	[SerializeField]
	public TextMeshProUGUI txtGeneration;

	[SerializeField]
	public TextMeshProUGUI txtSkinToInternal;

	[SerializeField]
	public TextMeshProUGUI txtSkinThermalMass;

	public virtual void Setup(UIPartActionWindow window, Part part, UI_Scene scene)
	{
		SetupItem(window, part, null, scene, null);
	}

	public void Awake()
	{
		pawGroup = new BasePAWGroup("Debug", "#autoLOC_8320010", startCollapsed: false);
	}

	public override void UpdateItem()
	{
		txtThermalMass.text = Localizer.Format("#autoLOC_434305", part.thermalMass.ToString("F1"));
		txtSkinThermalMass.text = Localizer.Format("#autoLOC_434306", part.skinThermalMass.ToString("F1"));
		txtTemperature.text = Localizer.Format("#autoLOC_434308", part.temperature.ToString("F1"));
		if (part.skinExposedAreaFrac > 0.0 && part.skinExposedAreaFrac < 1.0)
		{
			txtTemperatureExternal.text = Localizer.Format("#autoLOC_434310", part.skinTemperature.ToString("F1"));
		}
		else
		{
			txtTemperatureExternal.text = Localizer.Format("#autoLOC_434312", part.skinTemperature.ToString("F1"));
		}
		txtConductionInternal.text = Localizer.Format("#autoLOC_434314", part.thermalConductionFlux.ToString("F2"));
		txtConductionExternal.text = Localizer.Format("#autoLOC_434315", part.thermalConvectionFlux.ToString("F2"));
		txtRadiationExternal.text = Localizer.Format("#autoLOC_434316", part.thermalRadiationFlux.ToString("F2"));
		txtGeneration.text = Localizer.Format("#autoLOC_434317", part.thermalInternalFluxPrevious.ToString("F2"));
		txtSkinToInternal.text = Localizer.Format("#autoLOC_434318", part.skinToInternalFlux.ToString("F2"));
	}
}
