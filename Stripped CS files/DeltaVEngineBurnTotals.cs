using System.Collections.Generic;

public class DeltaVEngineBurnTotals
{
	public double maxTotalBurnTime;

	public double setFuelFlowTotalBurnTime;

	public double currentFuelFlowTotalBurnTime;

	public List<DeltaVPropellantInfo> propellantInfo;

	public DeltaVEngineBurnTotals()
	{
		propellantInfo = new List<DeltaVPropellantInfo>();
		maxTotalBurnTime = 0.0;
		setFuelFlowTotalBurnTime = 0.0;
		currentFuelFlowTotalBurnTime = 0.0;
	}
}
