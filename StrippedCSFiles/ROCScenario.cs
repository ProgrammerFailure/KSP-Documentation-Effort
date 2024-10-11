using System.Collections.Generic;
using System.Runtime.CompilerServices;

[KSPScenario((ScenarioCreationOptions)1066, new GameScenes[] { GameScenes.FLIGHT })]
public class ROCScenario : ScenarioModule
{
	public List<ROC> currentROCs;

	public Dictionary<string, ROC> destroyedROCs;

	public static ROCScenario Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ROCScenario()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}
}
