using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class KSCPauseMenu : MonoBehaviour
{
	private enum FilenameCheckResults
	{
		Empty,
		AlreadyExists,
		InvalidCharacters,
		Good
	}

	private UISkinDef windowSkin;

	private Rect windowRect;

	private Callback onDismiss;

	private PopupDialog saveDialog;

	private PopupDialog confirmDialog;

	private LoadGameDialog saveBrowser;

	private MiniSettings miniSettings;

	private PopupDialog dialog;

	private DialogGUIButton button;

	public GameObject dialogObj;

	private TMP_InputField saveDialogInputField;

	private MenuNavigation menuNav;

	private string quicksaveFilename;

	public static KSCPauseMenu Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSCPauseMenu()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static KSCPauseMenu Create(Callback onDismissMenu)
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
	private DialogGUIBase[] drawMissionRevertOptions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onMiniSettingsFinished()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Dismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void InitiateSave()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SaveGame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void spawnSaveDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ConfirmDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSaveDialogDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void doSave(string filename)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSaveDialogDismiss(bool dismissAll)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private FilenameCheckResults CheckFilename(string filename)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void InitiateLoad()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadGame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onLoadDialogDismiss(string path)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void quickLoad(string filename, string folder)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onPipelineFailed(KSPUpgradePipeline.UpgradeFailOption opt, ConfigNode n, string folder, Version originalVersion)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onPipelineFinished(ConfigNode node, string saveName, Version originalVersion)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void BuildButtonList(GameObject obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool MissionModeValid()
	{
		throw null;
	}
}
