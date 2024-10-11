using System.Runtime.CompilerServices;

public class ModuleAblator : PartModule
{
	[KSPField]
	public string ablativeResource;

	[KSPField]
	public double lossExp;

	[KSPField]
	public double lossConst;

	[KSPField]
	public double pyrolysisLossFactor;

	[KSPField]
	public double ablationTempThresh;

	[KSPField]
	public double reentryConductivity;

	[KSPField]
	public bool useNode;

	[KSPField]
	public string nodeName;

	[KSPField]
	public float charAlpha;

	[KSPField]
	public float charMax;

	[KSPField]
	public float charMin;

	[KSPField]
	public bool useChar;

	[KSPField]
	public string charModuleName;

	[KSPField]
	public string outputResource;

	[KSPField]
	public double outputMult;

	[KSPField]
	public double infoTemp;

	[KSPField]
	public bool usekg;

	[KSPField]
	public string unitsName;

	[KSPField(isPersistant = true)]
	public double nominalAmountRecip;

	protected bool useAblator;

	protected bool useOutput;

	protected int ablativeID;

	protected int outputID;

	protected double ablativeAmount;

	protected double ablativeMaxAmount;

	protected ResourceFlowMode ablativeFlowMode;

	protected ResourceFlowMode outputFlowMode;

	protected double pyrolysisLoss;

	protected double origConductivity;

	protected int downDir;

	protected double density;

	[KSPField(guiFormat = "N5", guiActive = false, guiName = "#autoLOC_6001866", guiUnits = "#autoLOC_7001416")]
	public double loss;

	[KSPField(guiFormat = "N2", guiActive = false, guiName = "#autoLOC_6001867", guiUnits = "#autoLOC_7001417")]
	public double flux;

	protected BaseField lossField;

	protected BaseField fluxField;

	protected AttachNode occNode;

	protected IScalarModule charModule;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleAblator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void UpdateColor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}
}
