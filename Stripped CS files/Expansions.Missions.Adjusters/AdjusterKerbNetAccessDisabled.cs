using ns17;
using ns9;

namespace Expansions.Missions.Adjusters;

public class AdjusterKerbNetAccessDisabled : AdjusterKerbNetAccessBase
{
	public AdjusterKerbNetAccessDisabled()
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100318";
	}

	public AdjusterKerbNetAccessDisabled(MENode node)
		: base(node)
	{
		disableKSPFields = true;
		disableKSPActions = true;
		disableKSPEvents = true;
		canBeRepaired = true;
		guiName = "#autoLOC_8100318";
	}

	public override void Activate()
	{
		base.Activate();
		UpdateStatusMessage(Localizer.Format("#autoLOC_8100319"));
		ModuleKerbNetAccess moduleKerbNetAccess = adjustedModule as ModuleKerbNetAccess;
		if (moduleKerbNetAccess != null && moduleKerbNetAccess.HasThisDialogOpen())
		{
			KerbNetDialog.Close();
		}
	}
}
