using System;
using ns9;

public class ModuleAblator : PartModule
{
	[KSPField]
	public string ablativeResource = "";

	[KSPField]
	public double lossExp;

	[KSPField]
	public double lossConst = 1.0;

	[KSPField]
	public double pyrolysisLossFactor = 10.0;

	[KSPField]
	public double ablationTempThresh = 600.0;

	[KSPField]
	public double reentryConductivity = 0.01;

	[KSPField]
	public bool useNode;

	[KSPField]
	public string nodeName = "bottom";

	[KSPField]
	public float charAlpha = 0.8f;

	[KSPField]
	public float charMax = 0.85f;

	[KSPField]
	public float charMin = 0.1f;

	[KSPField]
	public bool useChar = true;

	[KSPField]
	public string charModuleName = "shieldChar";

	[KSPField]
	public string outputResource = "";

	[KSPField]
	public double outputMult = 1.0;

	[KSPField]
	public double infoTemp = 2000.0;

	[KSPField]
	public bool usekg;

	[KSPField]
	public string unitsName = "#autoLOC_289916";

	[KSPField(isPersistant = true)]
	public double nominalAmountRecip = -1.0;

	public bool useAblator = true;

	public bool useOutput;

	public int ablativeID;

	public int outputID;

	public double ablativeAmount;

	public double ablativeMaxAmount;

	public ResourceFlowMode ablativeFlowMode;

	public ResourceFlowMode outputFlowMode;

	public double pyrolysisLoss;

	public double origConductivity;

	public int downDir = 3;

	public double density = 1.0;

	[KSPField(guiFormat = "N5", guiActive = false, guiName = "#autoLOC_6001866", guiUnits = "#autoLOC_7001416")]
	public double loss;

	[KSPField(guiFormat = "N2", guiActive = false, guiName = "#autoLOC_6001867", guiUnits = "#autoLOC_7001417")]
	public double flux;

	public BaseField lossField;

	public BaseField fluxField;

	public AttachNode occNode;

	public IScalarModule charModule;

	public override void OnAwake()
	{
		base.OnAwake();
	}

	public void Start()
	{
		if (useChar)
		{
			charModule = base.part.Modules.GetScalarModule(charModuleName);
			if (charModule == null)
			{
				useChar = false;
			}
		}
		lossField = base.Fields["loss"];
		fluxField = base.Fields["flux"];
		if (useNode)
		{
			occNode = base.part.FindAttachNode(nodeName);
		}
		useOutput = false;
		useAblator = false;
		if (!HighLogic.LoadedSceneIsFlight)
		{
			return;
		}
		if (!string.IsNullOrEmpty(ablativeResource))
		{
			PartResourceDefinition definition;
			if (lossExp < 0.0 && moduleIsEnabled && (definition = PartResourceLibrary.Instance.GetDefinition(ablativeResource)) != null)
			{
				pyrolysisLoss = pyrolysisLossFactor * (double)definition.specificHeatCapacity;
				density = definition.density;
				useAblator = true;
				ablativeID = definition.id;
				ablativeFlowMode = definition.resourceFlowMode;
				base.part.GetConnectedResourceTotals(ablativeID, definition.resourceFlowMode, out var amount, out var _);
				if (amount > 0.0)
				{
					if (nominalAmountRecip < 0.0)
					{
						nominalAmountRecip = 1.0 / amount;
					}
					ablativeAmount = amount;
				}
				else
				{
					double num = 1.0;
					nominalAmountRecip = 1.0;
					ablativeAmount = num;
				}
				if (!string.IsNullOrEmpty(outputResource) && (definition = PartResourceLibrary.Instance.GetDefinition(outputResource)) != null)
				{
					outputID = definition.id;
					outputFlowMode = definition.resourceFlowMode;
					useOutput = true;
				}
			}
		}
		else
		{
			nominalAmountRecip = 1.0;
		}
		origConductivity = base.part.heatConductivity;
	}

	public void FixedUpdate()
	{
		if (!HighLogic.LoadedSceneIsFlight)
		{
			return;
		}
		flux = 0.0;
		loss = 0.0;
		if ((!useNode) ? ((double)base.part.DragCubes.AreaOccluded[downDir] <= 0.1 * (double)base.part.DragCubes.WeightedArea[downDir]) : (occNode == null || ((occNode.attachedPart != null) ? true : false)))
		{
			base.part.heatConductivity = origConductivity;
		}
		else
		{
			base.part.heatConductivity = reentryConductivity;
		}
		if (!useAblator || !moduleIsEnabled || !(base.part.skinTemperature > ablationTempThresh))
		{
			return;
		}
		base.part.GetConnectedResourceTotals(ablativeID, ablativeFlowMode, out ablativeAmount, out ablativeMaxAmount);
		if (!(ablativeAmount > 0.0))
		{
			return;
		}
		loss = lossConst * Math.Exp(lossExp / base.part.skinTemperature);
		if (!(loss > 0.0))
		{
			return;
		}
		loss *= ablativeMaxAmount;
		double num = loss * (double)TimeWarp.fixedDeltaTime;
		if (num > ablativeAmount)
		{
			loss = ablativeAmount / (double)TimeWarp.fixedDeltaTime;
			base.part.RequestResource(ablativeID, num, ablativeFlowMode);
			if (useOutput)
			{
				base.part.RequestResource(outputID, (0.0 - ablativeAmount) * outputMult, outputFlowMode);
			}
		}
		else
		{
			base.part.RequestResource(ablativeID, num, ablativeFlowMode);
			if (useOutput)
			{
				base.part.RequestResource(outputID, (0.0 - num) * outputMult, outputFlowMode);
			}
		}
		loss *= density;
		flux = pyrolysisLoss * loss;
		base.part.AddExposedThermalFlux(0.0 - flux);
		loss *= 1000.0;
	}

	public void Update()
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			lossField.guiActive = PhysicsGlobals.ThermalDataDisplay && moduleIsEnabled;
			fluxField.guiActive = PhysicsGlobals.ThermalDataDisplay && moduleIsEnabled;
			if (useChar && useAblator)
			{
				UpdateColor();
			}
		}
	}

	public void UpdateColor()
	{
		float num = (float)(ablativeAmount * nominalAmountRecip);
		if (num > 1f)
		{
			num = 1f;
		}
		float num2 = charMax - charMin;
		float scalar = charMin + num2 * num;
		charModule.SetScalar(scalar);
	}

	public override string GetInfo()
	{
		string text = string.Empty;
		PartResourceDefinition definition;
		if (!string.IsNullOrEmpty(ablativeResource) && (definition = PartResourceLibrary.Instance.GetDefinition(ablativeResource)) != null && lossExp < 0.0)
		{
			double num = lossConst * Math.Exp(lossExp / infoTemp);
			if (base.part.Resources.Contains(ablativeResource))
			{
				num *= base.part.Resources.Get(ablativeResource).maxAmount;
			}
			text += Localizer.Format("#autoLOC_243615", definition.displayName);
			text += Localizer.Format("#autoLOC_6001041", infoTemp, (usekg ? (num * (double)definition.density * 1000.0) : num).ToString("N1"), unitsName);
			text += Localizer.Format("#autoLOC_6001042", KSPUtil.PrintSI(num * (double)definition.density * (double)definition.specificHeatCapacity * pyrolysisLossFactor, ""));
			if (useOutput && (definition = PartResourceLibrary.Instance.GetDefinition(outputResource)) != null)
			{
				text = text + "\n" + Localizer.Format("#autoLOC_243619", ((usekg ? (num * (double)definition.density * 1000.0) : num) * outputMult).ToString("N1") + " " + unitsName, outputResource) + "\n";
			}
		}
		if (!moduleIsEnabled)
		{
			text += Localizer.Format("#autoLOC_243623");
		}
		return text;
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLoc_6003023");
	}
}
