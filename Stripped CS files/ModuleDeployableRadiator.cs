using ns9;

public class ModuleDeployableRadiator : ModuleDeployablePart
{
	public static string cacheAutoLOC_7000030;

	public override void OnLoad(ConfigNode node)
	{
		base.OnLoad(node);
		subPartName = Localizer.Format("#autoLOC_234369");
		partType = Localizer.Format("#autoLOC_232117");
	}

	public override void OnInventoryModeDisable()
	{
		base.OnInventoryModeDisable();
		if (base.part.protoPartSnapshot != null)
		{
			ProtoPartModuleSnapshot protoPartModuleSnapshot = base.part.protoPartSnapshot.FindModule("ModuleDeployableRadiator");
			if (protoPartModuleSnapshot != null && protoPartModuleSnapshot.moduleValues != null)
			{
				protoPartModuleSnapshot.moduleValues.SetValue("deployState", deployState.ToString());
			}
		}
	}

	public override string GetModuleDisplayName()
	{
		return cacheAutoLOC_7000030;
	}

	public new static void CacheLocalStrings()
	{
		cacheAutoLOC_7000030 = Localizer.Format("#autoLOC_7000030");
	}
}
