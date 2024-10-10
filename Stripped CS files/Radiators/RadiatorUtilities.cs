using System;

namespace Radiators;

public class RadiatorUtilities
{
	public static RadiatorData GetThermalData(Part part)
	{
		RadiatorData result = default(RadiatorData);
		result.Energy = part.thermalMass * part.temperature;
		result.MaxEnergy = part.thermalMass * part.maxTemp * part.radiatorMax;
		result.EnergyCap = part.thermalMass * part.maxTemp * part.radiatorHeadroom;
		result.Part = part;
		return result;
	}

	public static void TransferEnergy(RadiatorData src, RadiatorData tgt)
	{
		double val = tgt.EnergyCap - tgt.Energy;
		double val2 = src.Energy - src.MaxEnergy;
		double num = Math.Min(val, val2);
		src.Part.thermalInternalFlux -= num;
		tgt.Part.thermalInternalFlux += num;
	}
}
