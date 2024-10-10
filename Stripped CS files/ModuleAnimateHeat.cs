public class ModuleAnimateHeat : ModuleAnimationSetter
{
	public double draperPoint = 798.0;

	[KSPField]
	public double lerpMin = double.NaN;

	[KSPField]
	public double lerpOffset = double.NaN;

	[KSPField]
	public double lerpMax = double.NaN;

	[KSPField]
	public double lerpScalar = 1.0;

	public double lerpDivRecip = 1.0;

	[KSPField]
	public bool useSkinTemp;

	public override void OnLoad(ConfigNode node)
	{
		string value = node.GetValue("ThermalAnim");
		if (!string.IsNullOrEmpty(value))
		{
			animName = value;
		}
		base.OnLoad(node);
	}

	public override void OnStart(StartState state)
	{
		HeatEffectStartup();
		base.OnStart(state);
	}

	public new void Update()
	{
		UpdateHeatEffect();
		UpdateAnim();
	}

	public void HeatEffectStartup()
	{
		if (double.IsNaN(lerpMin))
		{
			lerpMin = 0.0;
		}
		if (double.IsNaN(lerpOffset))
		{
			lerpOffset = 0.0 - draperPoint;
		}
		if (double.IsNaN(lerpMax))
		{
			if (useSkinTemp)
			{
				lerpMax = base.part.skinMaxTemp;
			}
			else
			{
				lerpMax = base.part.maxTemp;
			}
		}
		lerpScalar = 1.0;
		lerpDivRecip = 1.0 / (lerpMax - lerpMin + lerpOffset);
	}

	public void UpdateHeatEffect()
	{
		double num = ((!useSkinTemp) ? base.part.temperature : base.part.skinTemperature);
		if (double.IsNaN(num))
		{
			num = 0.0;
		}
		double num2 = (num + lerpOffset) * lerpScalar;
		num2 = ((num2 >= lerpMax) ? 1.0 : ((!(num2 <= lerpMin)) ? (num2 * lerpDivRecip) : 0.0));
		SetScalar((float)num2);
	}
}
