public class ModuleAnalysisResource : PartModule
{
	[KSPField(isPersistant = true)]
	public float abundance;

	[KSPField(isPersistant = true)]
	public string resourceName = "";

	[KSPField(isPersistant = true)]
	public float displayAbundance;

	[KSPField(guiActive = false, guiActiveEditor = false)]
	public string status = "Unknown";

	public BaseField statusFld;

	public override void OnStart(StartState state)
	{
		statusFld = base.Fields["status"];
		statusFld.guiName = resourceName;
	}

	public void Update()
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			SetupAnalysis();
		}
	}

	public void SetupAnalysis()
	{
		if ((double)abundance > 1E-09)
		{
			statusFld.guiActive = true;
			status = $"{displayAbundance * 100f:0.0000}%";
		}
		else
		{
			base.Fields["status"].guiActive = false;
		}
	}
}
