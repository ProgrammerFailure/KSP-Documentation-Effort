using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Screens;

public class AlarmClockApp : UIApp
{
	internal class UsageStats
	{
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

	public static AlarmClockApp Instance;

	public static bool Ready;

	[SerializeField]
	private AlarmClockUIFrame appFramePrefab;

	private AlarmClockUIFrame appFrame;

	private RectTransform appRectTransform;

	[SerializeField]
	private int appWidth;

	private int appHeight;

	internal UsageStats usage;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AlarmClockApp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static AlarmClockApp()
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
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool AnyTextFieldHasFocus()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static bool AppFrameHasLock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RemoveAppFrameInputLock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TextFieldHasFocus()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool EditAlarm(AlarmTypeBase alarm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneSwitch(GameEvents.FromToAction<GameScenes, GameScenes> scn)
	{
		throw null;
	}
}
