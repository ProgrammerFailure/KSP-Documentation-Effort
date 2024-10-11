using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	public bool display;

	public string guiSkinName;

	public string guiMiniSkinName;

	private UISkinDef guiSkin;

	public Color backgroundColor;

	private static PauseMenu fetch;

	private MiniSettings miniSettings;

	private MiniKeyBindings miniKeyBindings;

	private DialogGUIHorizontalLayout dialogObj;

	private Rect dialogRect;

	public static ClearToSaveStatus canSaveAndExit;

	private PopupDialog confirmOptionDialog;

	private GameScenes sceneToLeaveTo;

	private PopupDialog dialog;

	private bool loadBrowserShown;

	private bool showExitToMainConfirmation;

	private PopupDialog dialogExitToMainConfirmation;

	private bool exitToMainConfirmationShown;

	public static bool isOpen
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool exists
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PauseMenu()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Display()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Close()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private DialogGUIBase[] draw()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static DialogGUIBase[] drawMissionRestartOptions(PopupDialog dialog)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private DialogGUIBase[] drawRevertOptions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void drawStockRevertOptions(PopupDialog dialog, List<DialogGUIBase> options)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void drawMissionsRevertOptions(PopupDialog dialog, List<DialogGUIBase> options)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private DialogGUIBase[] drawExitWithoutSaveOptions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private DialogGUIBase[] drawExitScenarioOptions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static DialogGUIBase[] drawExitTestMissionOptions(PopupDialog dialogMenu)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ShowExitToMainConfirmation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SubmitExitToMainMenuConfirmation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CancelExitToMainMenuConfirmation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void saveAndExit(GameScenes sceneToLoad, Game stateToSave)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void miniSettingsFinished()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool MissionModeValid()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void BuildButtonList(GameObject obj)
	{
		throw null;
	}
}
