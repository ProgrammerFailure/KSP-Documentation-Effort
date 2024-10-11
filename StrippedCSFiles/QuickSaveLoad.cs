using System;
using System.Runtime.CompilerServices;
using KSP.UI;
using TMPro;
using UnityEngine;

public class QuickSaveLoad : MonoBehaviour
{
	public delegate void FinishedSaveLoadDialogCallback(string saveName);

	internal class loadDialogParameters
	{
		internal bool pauseGame;

		internal FinishedSaveLoadDialogCallback onLoadCloseReturn;

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal loadDialogParameters(bool pauseGame, FinishedSaveLoadDialogCallback onLoadCloseReturn)
		{
			throw null;
		}
	}

	private enum FilenameCheckResults
	{
		Empty,
		AlreadyExists,
		InvalidCharacters,
		Good
	}

	public static QuickSaveLoad fetch;

	public bool AutoSaveOnQuickSave;

	public bool holdToLoad;

	public float holdTime;

	private float TatHold;

	private UISkinDef saveBrowserSkin;

	private ScreenMessage screenMessage;

	private DialogUISelectFile loadDialog;

	private PopupDialog saveDialog;

	private PopupDialog confirmDialog;

	private FinishedSaveLoadDialogCallback onSaveAsCloseReturn;

	private bool pauseGame;

	private MenuNavigation menuNav;

	private TMP_InputField inputField;

	private bool blockedSubmitOnStart;

	public string quicksaveFilename;

	private LoadGameDialog loadBrowser;

	private loadDialogParameters loadDialogParams;

	private static string cacheAutoLOC_174575;

	private static string cacheAutoLOC_174610;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public QuickSaveLoad()
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
	private void start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onHoldComplete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void QuickSave()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void quickSave(bool saveAs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static bool QuickSaveClearToSave()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void doSave(string filename, string screenMsg = "#autoLOC_174669")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void quickLoad(string filename, string folder)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onQuickloadPipelineFailed(KSPUpgradePipeline.UpgradeFailOption opt, ConfigNode n, string folder, Version originalVersion)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onQuickloadPipelineFinished(ConfigNode node, string saveName, Version originalVersion)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SpawnSaveAsDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SpawnSaveAsDialog(bool pauseGame, FinishedSaveLoadDialogCallback onSaveAsCloseReturn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ConfirmDialog(bool pauseGame = true, FinishedSaveLoadDialogCallback onSaveAsCloseReturn = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSaveAsClose(string saveName, bool pauseGame = true, FinishedSaveLoadDialogCallback onSaveAsCloseReturn = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SpawnQuickLoadDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SpawnQuickLoadDialog(bool pauseGame, FinishedSaveLoadDialogCallback onLoadCloseReturn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLoadClose(string saveName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string EscapeString(string str)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private FilenameCheckResults CheckFilename(string filename)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
