using ns9;
using TMPro;
using UnityEngine;

namespace ns32;

public class ScreenDeltaVStageInfo : MonoBehaviour
{
	[SerializeField]
	public TextMeshProUGUI stageNumber;

	[SerializeField]
	public TextMeshProUGUI partCountNumber;

	[SerializeField]
	public TextMeshProUGUI partCountDecoupledNumber;

	[SerializeField]
	public TextMeshProUGUI startMassNumber;

	[SerializeField]
	public TextMeshProUGUI dryMassNumber;

	[SerializeField]
	public TextMeshProUGUI fuelMassNumber;

	[SerializeField]
	public TextMeshProUGUI endMassNumber;

	[SerializeField]
	public TextMeshProUGUI ispNumber;

	[SerializeField]
	public TextMeshProUGUI thrustNumber;

	[SerializeField]
	public TextMeshProUGUI twrNumber;

	[SerializeField]
	public TextMeshProUGUI deltavNumber;

	[SerializeField]
	public TextMeshProUGUI burnTimeNumber;

	[SerializeField]
	public TextMeshProUGUI payloadText;

	public DeltaVStageInfo stageInfo;

	public void UpdateData(DeltaVStageInfo stageInfo, CalcType type)
	{
		this.stageInfo = stageInfo;
		stageNumber.text = stageInfo.stage.ToString();
		partCountNumber.text = stageInfo.PartsActiveInStage().ToString();
		partCountDecoupledNumber.text = stageInfo.PartsDecoupledInStage().ToString();
		startMassNumber.text = stageInfo.startMass.ToString("N3");
		dryMassNumber.text = stageInfo.dryMass.ToString("N3");
		fuelMassNumber.text = stageInfo.fuelMass.ToString("N3");
		endMassNumber.text = stageInfo.endMass.ToString("N3");
		burnTimeNumber.text = KSPUtil.dateTimeFormatter.PrintTimeCompact(stageInfo.stageBurnTime, explicitPositive: false);
		payloadText.enabled = stageInfo.payloadStage;
		switch (type)
		{
		default:
			ispNumber.text = stageInfo.ispVac.ToString("N2") + Localizer.Format("#autoLOC_6002317");
			thrustNumber.text = Localizer.Format("#autoLOC_8002207", stageInfo.thrustVac.ToString("N2"));
			twrNumber.text = stageInfo.TWRVac.ToString("N2");
			deltavNumber.text = stageInfo.deltaVinVac.ToString("N2");
			break;
		case CalcType.const_1:
			ispNumber.text = stageInfo.ispASL.ToString("N2") + Localizer.Format("#autoLOC_6002317");
			thrustNumber.text = Localizer.Format("#autoLOC_8002207", stageInfo.thrustASL.ToString("N2"));
			twrNumber.text = stageInfo.TWRASL.ToString("N2");
			deltavNumber.text = stageInfo.deltaVatASL.ToString("N2");
			break;
		case CalcType.Actual:
			ispNumber.text = stageInfo.ispActual.ToString("N2") + Localizer.Format("#autoLOC_6002317");
			thrustNumber.text = Localizer.Format("#autoLOC_8002207", stageInfo.thrustActual.ToString("N2"));
			twrNumber.text = stageInfo.TWRActual.ToString("N2");
			deltavNumber.text = stageInfo.deltaVActual.ToString("N2");
			break;
		}
	}
}
