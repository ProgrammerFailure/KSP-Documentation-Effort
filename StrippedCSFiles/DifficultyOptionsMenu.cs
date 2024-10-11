using System.Runtime.CompilerServices;
using UnityEngine;

public class DifficultyOptionsMenu : MonoBehaviour
{
	private class SortedVerticalLayout
	{
		public int order;

		public DialogGUIVerticalLayout layout;

		public string LocalizedTitle;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public SortedVerticalLayout(DialogGUIVerticalLayout layout, int order, string LocalizedTitle)
		{
			throw null;
		}
	}

	private GameParameters gPars;

	private GameParameters backupPars;

	private Game.Modes gameMode;

	private Callback<GameParameters, bool> OnDismiss;

	private Rect dialogRect;

	private float width;

	private float height;

	private UISkinDef skin;

	private bool isNewGame;

	private static GameObject minisettingsSource;

	private string currentSection;

	public string CurrentSection
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DifficultyOptionsMenu()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static DifficultyOptionsMenu Create(Game.Modes mode, GameParameters initialParams, bool newGame, Callback<GameParameters, bool> OnDismiss)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static DifficultyOptionsMenu Create(Game.Modes mode, GameParameters initialParams, bool newGame, Callback<GameParameters, bool> OnDismiss, GameObject miniSettings)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private PopupDialog CreateDifficultWindow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Dismiss(bool commitChanges)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetPreset(GameParameters.Preset p)
	{
		throw null;
	}
}
