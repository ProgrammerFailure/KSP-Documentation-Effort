using System.Collections.Generic;
using ns9;

public class ModuleAsteroidInfo : ModuleSpaceObjectInfo
{
	public ModuleAsteroid baseMod;

	public List<ModuleSpaceObjectResource> resInfoList;

	public override void OnStart(StartState state)
	{
		baseMod = base.part.Modules.GetModule<ModuleAsteroid>();
		if ((object)baseMod != null)
		{
			baseMod.OnStart(state);
			if (currentMassVal <= 1E-09)
			{
				currentMassVal = base.part.mass;
			}
			if (massThresholdVal <= 1E-09)
			{
				SetupAsteroidResources();
			}
			base.part.force_activate();
			baseMod.SetAsteroidMass((float)currentMassVal);
			base.part.mass = (float)currentMassVal;
		}
	}

	public virtual void SetupAsteroidResources()
	{
		resInfoList = base.part.FindModulesImplementing<ModuleSpaceObjectResource>();
		SetupSpaceObjectResources(resInfoList);
	}

	public virtual void Update()
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			displayMass = $"{currentMassVal:0.00000}t";
			if ((object)baseMod != null)
			{
				baseMod.SetAsteroidMass((float)currentMassVal);
			}
			resources = Localizer.Format("#autoLOC_6001049", (currentMassVal - massThresholdVal).ToString("0.00000"), ((currentMassVal - massThresholdVal) / currentMassVal * 100.0).ToString("0.000"));
		}
	}
}
