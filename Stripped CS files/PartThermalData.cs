using System.Collections.Generic;
using UnityEngine;

public class PartThermalData
{
	public static PartThermalDataIntComparer intComparer = new PartThermalDataIntComparer();

	public static PartThermalDataSkinComparer skinComparer = new PartThermalDataSkinComparer();

	public Part part;

	public int thermalLinkCount;

	public double bodyAreaMultiplier = 1.0;

	public double sunAreaMultiplier = 1.0;

	public double convectionAreaMultiplier = 1.0;

	public double convectionTempMultiplier = 1.0;

	public double convectionCoeffMultiplier = 1.0;

	public List<ThermalLink> thermalLinks = new List<ThermalLink>();

	public List<IAnalyticOverheatModule> overheatModules = new List<IAnalyticOverheatModule>();

	public List<IAnalyticPreview> analyticPreviewModules = new List<IAnalyticPreview>();

	public IAnalyticTemperatureModifier analyticModifier;

	public OcclusionData convectionData;

	public OcclusionData sunData;

	public OcclusionData bodyData;

	public double previousTemperature;

	public double previousSkinTemperature;

	public double previousSkinUnexposedTemperature;

	public bool exposed;

	public double radAreaRecip = 1.0;

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

	public PartThermalData(Part part)
	{
		this.part = part;
		thermalLinkCount = 0;
		thermalLinks = new List<ThermalLink>();
		convectionData = new OcclusionData(this);
		sunData = new OcclusionData(this);
		bodyData = new OcclusionData(this);
		int count = part.Modules.Count;
		for (int i = 0; i < count; i++)
		{
			PartModule partModule = part.Modules[i];
			if (partModule is IAnalyticOverheatModule)
			{
				overheatModules.Add(partModule as IAnalyticOverheatModule);
			}
			if (partModule is IAnalyticPreview)
			{
				analyticPreviewModules.Add(partModule as IAnalyticPreview);
			}
			if (partModule is IAnalyticTemperatureModifier)
			{
				if (analyticModifier == null)
				{
					analyticModifier = partModule as IAnalyticTemperatureModifier;
				}
				else
				{
					Debug.LogWarning("Analytic: Part " + part.name + " has multiple IAnalyticTemperatureModifier!");
				}
			}
			if (partModule is KerbalEVA)
			{
				isEVA = true;
			}
		}
	}

	public double GetUnifiedSkinTemp()
	{
		if (exposed)
		{
			if (GameSettings.FI_LOG_TEMP_ERROR && double.IsNaN(part.skinExposedAreaFrac))
			{
				Debug.LogError("[FlightIntegrator]: For part " + part.name + ", frac NaN");
			}
			if (GameSettings.FI_LOG_TEMP_ERROR && double.IsNaN(part.skinTemperature))
			{
				Debug.LogError("[FlightIntegrator]: For part " + part.name + ", skintemp NaN");
			}
			if (GameSettings.FI_LOG_TEMP_ERROR && double.IsNaN(part.skinUnexposedTemperature))
			{
				Debug.LogError("[FlightIntegrator]: For part " + part.name + ", ueSkintemp NaN");
			}
			return part.skinExposedAreaFrac * part.skinTemperature + (1.0 - part.skinExposedAreaFrac) * part.skinUnexposedTemperature;
		}
		if (GameSettings.FI_LOG_TEMP_ERROR && double.IsNaN(part.skinTemperature))
		{
			Debug.LogError("[FlightIntegrator]: For part " + part.name + ", skintemp NaN");
		}
		return part.skinTemperature;
	}
}
