using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class MiniSettings : MonoBehaviour
{
	private Callback OnDismissCallback;

	private Vector2 scrollPos;

	private Rect windowRect;

	private UISkinDef skin;

	private PopupDialog popupDialog;

	private AudioFXSettings audioSettings;

	private VideoSettings videoSettings;

	private GameplaySettingsScreen gameSettings;

	private AlarmClockSettingsUI alarmSettings;

	private MenuNavigation menuNav;

	private ScrollRect scrollRect;

	private RectTransform content;

	private GameObject tempMiniSettingsObj;

	private Transform explicitNavParentcache;

	private List<Selectable> explicitNavSelectables;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MiniSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MiniSettings Create(Callback onDismiss)
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
	private DialogGUIBase[] drawWindow()
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
	private void ApplySettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Dismiss()
	{
		throw null;
	}
}
