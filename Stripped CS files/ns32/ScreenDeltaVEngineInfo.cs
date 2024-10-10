using ns9;
using TMPro;
using UnityEngine;

namespace ns32;

public class ScreenDeltaVEngineInfo : MonoBehaviour
{
	[SerializeField]
	public TextMeshProUGUI engineName;

	[SerializeField]
	public TextMeshProUGUI engineType;

	[SerializeField]
	public TextMeshProUGUI maxThrustNumber;

	[SerializeField]
	public TextMeshProUGUI maxThrustType;

	[SerializeField]
	public TextMeshProUGUI ispNumber;

	[SerializeField]
	public TextMeshProUGUI ispType;

	[SerializeField]
	public TextMeshProUGUI maxBurnTimeNumber;

	public DeltaVEngineInfo engineInfo;

	public void UpdateData(DeltaVEngineInfo engineInfo, CalcType type, string typeDesc, int stage)
	{
		this.engineInfo = engineInfo;
		engineName.text = engineInfo.engine.part.partInfo.title;
		engineType.text = engineInfo.engine.GetEngineType().ToString();
		switch (type)
		{
		default:
			maxThrustNumber.text = Localizer.Format("#autoLOC_8002207", engineInfo.thrustVac.ToString("N2"));
			maxThrustType.text = type.displayDescription();
			ispNumber.text = engineInfo.ispVac.ToString("N2") + Localizer.Format("#autoLOC_6002317");
			ispType.text = typeDesc;
			maxBurnTimeNumber.text = KSPUtil.dateTimeFormatter.PrintTimeCompact(engineInfo.GetFuelTimeAtThrottle(stage), explicitPositive: false);
			break;
		case CalcType.const_1:
			maxThrustNumber.text = Localizer.Format("#autoLOC_8002207", engineInfo.thrustASL.ToString("N2"));
			maxThrustType.text = type.displayDescription();
			ispNumber.text = engineInfo.ispASL.ToString("N2") + Localizer.Format("#autoLOC_6002317");
			ispType.text = typeDesc;
			maxBurnTimeNumber.text = KSPUtil.dateTimeFormatter.PrintTimeCompact(engineInfo.GetFuelTimeAtThrottle(stage), explicitPositive: false);
			break;
		case CalcType.Actual:
			maxThrustNumber.text = Localizer.Format("#autoLOC_8002207", engineInfo.thrustActual.ToString("N2"));
			maxThrustType.text = type.displayDescription();
			ispNumber.text = engineInfo.ispActual.ToString("N2") + Localizer.Format("#autoLOC_6002317");
			ispType.text = typeDesc;
			maxBurnTimeNumber.text = KSPUtil.dateTimeFormatter.PrintTimeCompact(engineInfo.GetFuelTimeAtActiveThrottle(stage), explicitPositive: false);
			break;
		}
	}
}
