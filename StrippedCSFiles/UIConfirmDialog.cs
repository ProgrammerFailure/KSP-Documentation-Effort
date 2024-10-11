using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIConfirmDialog : MonoBehaviour
{
	[SerializeField]
	[Header("UI Components")]
	protected TextMeshProUGUI textHeader;

	[SerializeField]
	protected TextMeshProUGUI textDescription;

	[SerializeField]
	protected TextMeshProUGUI textCancel;

	[SerializeField]
	private Button buttonCancel;

	[SerializeField]
	protected TextMeshProUGUI textConfirmation;

	[SerializeField]
	private Button buttonConfirmation;

	[SerializeField]
	private TextMeshProUGUI textDontShowAgain;

	[SerializeField]
	private Toggle toggleDontShowAgain;

	public bool modal;

	private CanvasGroup canvasGroup;

	protected Callback<bool> onOk;

	private Callback<bool> onCancel;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIConfirmDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static UIConfirmDialog Spawn(string title, string message, Callback<bool> onOk, Callback<bool> onCancel, bool showCancelBtn = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static UIConfirmDialog Spawn(string title, string message, Callback<bool> onOk, bool showCancelBtn = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static UIConfirmDialog Spawn(string title, string message, Callback<bool> onOk, bool showDontShowAgain, string textDontShowAgain)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static UIConfirmDialog Spawn(string title, string message, string textCancel, string textOK, string textDontShowAgain, Callback<bool> onOk, Callback<bool> onCancel, bool showCancelBtn = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnCancel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CloseDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnLeavingScene(GameScenes scn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}
}
