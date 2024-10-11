using System.Runtime.CompilerServices;
using KSP.UI;
using KSP.UI.Screens;
using UnityEngine;

public class DeltaVApp : UIApp
{
	internal class UsageStats
	{
		public int body;

		public int situation;

		public int atmosphereSlider;

		public int atmosphereText;

		public int displayFields;

		internal bool UsageFound
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public UsageStats()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void Reset()
		{
			throw null;
		}
	}

	public static DeltaVApp Instance;

	public static bool Ready;

	[SerializeField]
	private GenericAppFrame appFramePrefab;

	private GenericAppFrame appFrame;

	[SerializeField]
	private DeltaVAppSituation situationPrefab;

	private DeltaVAppSituation situation;

	[SerializeField]
	private DeltaVAppStageInfo stageInfoPrefab;

	private DeltaVAppStageInfo stageInfo;

	private RectTransform appRectTransform;

	[SerializeField]
	private int appWidth;

	private int appHeight;

	[SerializeField]
	private int appHeightSituation;

	[SerializeField]
	private int appHeightStageInfo;

	[SerializeField]
	private int appStageInfoLineHeight;

	private bool initialHeightSet;

	internal UsageStats usage;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DeltaVApp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static DeltaVApp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnAppInitialized()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnAppDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override bool OnAppAboutToStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void DisplayApp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void HideApp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override ApplicationLauncher.AppScenes GetAppScenes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override Vector3 GetAppScreenPos(Vector3 defaultAnchorPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool AnyTextFieldHasFocus()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TextFieldHasFocus()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneSwitch(GameEvents.FromToAction<GameScenes, GameScenes> scn)
	{
		throw null;
	}
}
