using ns9;

public class ModuleDeployableAntenna : ModuleDeployablePart
{
	public override bool CanMove
	{
		get
		{
			if (!base.CanMove)
			{
				return false;
			}
			if (HighLogic.LoadedSceneIsFlight)
			{
				return !ShouldBreakFromPressure();
			}
			return true;
		}
	}

	public override float MinAoAForQCheck => 0.5f;

	public override void OnLoad(ConfigNode node)
	{
		base.OnLoad(node);
		subPartName = Localizer.Format("#autoLOC_234195");
		partType = Localizer.Format("#autoLOC_234196");
	}

	public override void Extend()
	{
		if (HighLogic.LoadedSceneIsFlight && ShouldBreakFromPressure())
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_234215", partType), 5f, ScreenMessageStyle.UPPER_LEFT);
		}
		else
		{
			base.Extend();
		}
	}

	public override void OnInventoryModeDisable()
	{
		base.OnInventoryModeDisable();
		if (base.part.protoPartSnapshot != null)
		{
			ProtoPartModuleSnapshot protoPartModuleSnapshot = base.part.protoPartSnapshot.FindModule("ModuleDeployableAntenna");
			if (protoPartModuleSnapshot != null && protoPartModuleSnapshot.moduleValues != null)
			{
				protoPartModuleSnapshot.moduleValues.SetValue("deployState", deployState.ToString());
			}
		}
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLoc_6003035");
	}
}
