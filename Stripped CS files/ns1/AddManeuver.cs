using ns9;

namespace ns1;

public class AddManeuver : MapContextMenuOption
{
	public PatchedConicRenderer pcr;

	public double double_0;

	public AddManeuver(PatchedConicRenderer pcr, double double_1)
		: base("#autoLOC_465466")
	{
		this.pcr = pcr;
		double_0 = double_1;
	}

	public override void OnSelect()
	{
		if (InputLockManager.IsUnlocked(ControlTypes.MANNODE_ADDEDIT))
		{
			pcr.AddManeuverNode(double_0);
		}
		else
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_465476"), 3f, ScreenMessageStyle.UPPER_CENTER);
		}
	}
}
