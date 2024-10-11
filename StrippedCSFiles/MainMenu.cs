using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	private class PlayerProfileInfo
	{
		public int vesselCount;

		public int UT;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public PlayerProfileInfo()
		{
			throw null;
		}
	}

	public TextProButton3D startBtn;

	public TextProButton3D commBtn;

	public TextProButton3D spaceportBtn;

	public TextProButton3D quitBtn;

	public TextProButton3D settingBtn;

	public TextProButton3D creditsBtn;

	public TextProButton3D newGameBtn;

	public TextProButton3D continueBtn;

	public TextProButton3D trainingBtn;

	public TextProButton3D scenariosBtn;

	public TextProButton3D playESAMissionsBtn;

	public TextProButton3D playMissionsBtn;

	public TextProButton3D missionBuilderBtn;

	public TextProButton3D buyMakingHistoryBtn;

	public TextProButton3D buySerenityBtn;

	public TextProButton3D merchandiseBtn;

	private TextProButton3D lastButtonSelected;

	[SerializeField]
	private MakingHistoryAboutDialog mhAboutDialogPrefab;

	[SerializeField]
	private SerenityAboutDialog serenityAboutDialogPrefab;

	[SerializeField]
	private TMP_Text playMissionsNewBadge;

	[SerializeField]
	private TMP_Text playMissionsNewESABadge;

	[SerializeField]
	private TMP_Text missionBuilderNewBadge;

	public TextProButton3D backBtn;

	public UISkinDefSO guiSkinDef;

	public Button unitTestsBtn;

	[SerializeField]
	private PrivacyDialog phDialogPrefab;

	[SerializeField]
	private WhatsNewDialog wnDialogPrefab;

	public Texture careerIcon;

	public Texture scienceSandboxIcon;

	public Texture sandboxIcon;

	public Texture scenarioIcon;

	public Texture tutorialIcon;

	public MainMenuEnvLogic envLogic;

	public string KSPsiteURL;

	public string SpaceportURL;

	public string DefaultFlagURL;

	public string makingHistoryExpansionURL;

	public string serenityExpansionURL;

	public string merchandiseURL;

	private bool hasUsedArrowsOnce;

	[SerializeField]
	private List<TextProButton3D> stageOneBtn;

	[SerializeField]
	private List<TextProButton3D> stageTwoBtn;

	private Rect menuWindowRect;

	private string menuWindowTitle;

	private static bool mhAboutDialogDisplayed;

	private static bool serenityAboutDialogDisplayed;

	private static bool expansionLoadErrorDisplayed;

	private static bool mainMenuVisited;

	[SerializeField]
	private List<string> invalidNames;

	private static string pName;

	private static string fURL;

	private static Game.Modes newGameMode;

	private static GameParameters newGameParameters;

	private FlagBrowserGUIButton flagBrowserGUIButton;

	private PopupDialog createGameDialog;

	public static bool MainMenuVisited
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	private static string flagURL
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MainMenu()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static MainMenu()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnApplicationFocus(bool focus)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void runUnitTests()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void startGame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void NewGame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private PopupDialog CreateNewGameDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private GameParameters UpdatedGameParameters(GameParameters pars)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ConfirmNewGame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateAndStartGame(string name, Game.Modes mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateAndStartGame(string name, Game.Modes mode, GameScenes startScene, EditorFacility editorFacility)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OverWriteNewGame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DeleteDirectory(string targetDir)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CancelOverwriteNewGame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CancelNewGame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnOpenFlagBrowser()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFlagSelect(FlagBrowser.FlagEntry selected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFlagCancel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OpenDifficultyOptions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDifficultyOptionsDismiss(GameParameters pars, bool changed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadGame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLoadDialogFinished(string save)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnLoadDialogPipelineFail(KSPUpgradePipeline.UpgradeFailOption opt, ConfigNode node, string save)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnLoadDialogPipelineFinished(ConfigNode node, string save)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void StartTraining()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void StartScenario()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnScenarioLoadDismiss(ScenarioLoadDialog.ScenarioSaveInfo save)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnScenarioLoadPipelineFailed(KSPUpgradePipeline.UpgradeFailOption opt, ConfigNode node, ScenarioLoadDialog.ScenarioSaveInfo save)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnScenarioLoadPipelineFinished(ConfigNode node, ScenarioLoadDialog.ScenarioSaveInfo save)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PlayBaseMissions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PlayMissions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnMissionLoadDismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MissionEditor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuyExpansionPack()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuyExpansionPack2()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void cancelStartGame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void goToKSPSite()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void goToSpacePort()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GoToMerchandisingSite()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void quit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResetInputLockOnQuit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void settingsKSP()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void credits()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnInputLocksModified(GameEvents.FromToAction<ControlTypes, ControlTypes> ctrls)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void lockEverything()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void unlockEverything()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void HighlightFirstItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResetNavigationOnStageChange()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MenuNavHandler(MenuNavInput input)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void HighlightNextItem(int dir)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SelectHighlithedItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void MouseIsHovering(bool b)
	{
		throw null;
	}
}
