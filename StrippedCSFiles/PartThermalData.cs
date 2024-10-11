using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class PartThermalData
{
	public static PartThermalDataIntComparer intComparer;

	public static PartThermalDataSkinComparer skinComparer;

	public Part part;

	public int thermalLinkCount;

	public double bodyAreaMultiplier;

	public double sunAreaMultiplier;

	public double convectionAreaMultiplier;

	public double convectionTempMultiplier;

	public double convectionCoeffMultiplier;

	public List<ThermalLink> thermalLinks;

	public List<IAnalyticOverheatModule> overheatModules;

	public List<IAnalyticPreview> analyticPreviewModules;

	public IAnalyticTemperatureModifier analyticModifier;

	public OcclusionData convectionData;

	public OcclusionData sunData;

	public OcclusionData bodyData;

	public double previousTemperature;

	public double previousSkinTemperature;

	public double previousSkinUnexposedTemperature;

	public bool exposed;

	public double radAreaRecip;

	public double convectionArea;

	public double intConductionFlux;

	public double skinConductionFlux;

	public double localIntConduction;

	public double localSkinConduction;

	public double skinSkinConductionFlux;

	public double skinInteralConductionFlux;

	public double unexpSkinInternalConductionFlux;

	public double radiationFlux;

	public double unexpRadiationFlux;

	public double convectionFlux;

	public bool isEVA;

	public double postShockExtTemp;

	public double finalCoeff;

	public double partPseudoRe;

	public double emissScalar;

	public double absorbScalar;

	public double absEmissRatio;

	public double sunFlux;

	public double bodyFlux;

	public double expFlux;

	public double unexpFlux;

	public double brtUnexposed;

	public double brtExposed;

	public int sCount;

	public int realSCount;

	public double sDivisor;

	public double conductionMult;

	public double skinSkinTransfer;

	public double unifiedTemp;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartThermalData(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static PartThermalData()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetUnifiedSkinTemp()
	{
		throw null;
	}
}
