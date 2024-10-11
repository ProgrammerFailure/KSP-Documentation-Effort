using System.Runtime.CompilerServices;

[KSPScenario(ScenarioCreationOptions.AddToNewGames, new GameScenes[]
{
	GameScenes.SPACECENTER,
	GameScenes.EDITOR,
	GameScenes.TRACKSTATION
})]
public class ScenarioNewGameIntro : TutorialScenario
{
	public bool kscComplete;

	public bool editorComplete;

	public bool tsComplete;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScenarioNewGameIntro()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnOnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnAssetSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnTutorialSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onLeavingScene(GameScenes scn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void KSCtutorialSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EditorTutorialSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void TSTutorialSetup()
	{
		throw null;
	}
}
