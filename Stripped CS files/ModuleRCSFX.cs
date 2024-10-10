using ns9;

public class ModuleRCSFX : ModuleRCS
{
	[KSPField]
	public string runningEffectName = "running";

	public static string cacheAutoLOC_7000028;

	public override void SetupFX()
	{
		base.part.Effect(runningEffectName, 0f);
	}

	public override void UpdatePowerFX(bool running, int idx, float power)
	{
		base.part.Effect(runningEffectName, running ? power : 0f, idx);
	}

	public override void DeactivatePowerFX()
	{
		base.part.Effect(runningEffectName, 0f);
	}

	public override void DeactivateFX()
	{
		base.part.Effect(runningEffectName, 0f);
	}

	public override void OnAwake()
	{
		base.OnAwake();
		base.part.Effect(runningEffectName, 0f);
	}

	public override string GetModuleDisplayName()
	{
		return cacheAutoLOC_7000028;
	}

	public new static void CacheLocalStrings()
	{
		cacheAutoLOC_7000028 = Localizer.Format("#autoLOC_7000028");
	}
}
