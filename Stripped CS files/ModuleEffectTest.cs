using UnityEngine;

public class ModuleEffectTest : PartModule
{
	[KSPField(isPersistant = true)]
	public string fireEffectName = "TestFire";

	[KSPField(isPersistant = true)]
	public string powerEffectName = "TestStrength";

	[KSPField(isPersistant = true, guiActive = true)]
	public float effectStrength;

	[KSPField(isPersistant = true)]
	public float effectStrengthSpeed = 4f;

	[KSPField(isPersistant = true)]
	public float effectTarget;

	[KSPEvent(guiActive = true, guiName = "#autoLOC_502068")]
	public void ToggleAction()
	{
		if (effectTarget == 1f)
		{
			effectTarget = 0f;
		}
		else
		{
			effectTarget = 1f;
		}
	}

	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001871")]
	public void FireAction()
	{
		base.part.Effects.Event(fireEffectName, -1);
	}

	public override void OnStart(StartState state)
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			base.part.Effects.Event(powerEffectName, effectStrength, -1);
		}
	}

	public void Update()
	{
		if (HighLogic.LoadedSceneIsFlight && effectStrength != effectTarget)
		{
			effectStrength = Mathf.Clamp01(effectStrength + Mathf.Sign(effectTarget - effectStrength) * (1f / effectStrengthSpeed * Time.deltaTime));
			base.part.Effects.Event(powerEffectName, effectStrength, -1);
		}
	}
}
