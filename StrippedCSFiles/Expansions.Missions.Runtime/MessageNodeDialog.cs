using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Actions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Runtime;

public class MessageNodeDialog : MonoBehaviour
{
	public bool modal;

	private CanvasGroup canvasGroup;

	[SerializeField]
	private TextMeshProUGUI headerText;

	[SerializeField]
	private RawImage instructorPortrait;

	[SerializeField]
	private TextMeshProUGUI instructorText;

	[SerializeField]
	private TextMeshProUGUI nodeText;

	[SerializeField]
	private Button continueButton;

	[SerializeField]
	private TextMeshProUGUI continueBtnText;

	[SerializeField]
	private RectTransform dialogRect;

	[SerializeField]
	private LayoutElement textSectionLayoutElement;

	[SerializeField]
	private Object strategy_AvatarLights;

	private Callback continueCallback;

	private KerbalInstructorBase instructor;

	private GameObject mainlight;

	private GameObject backlight;

	private string instructorPrefabName;

	private string instructorName;

	private int instructorPortraitSize;

	private RenderTexture instructorTexture;

	private GameObject avatarLights;

	private bool lightsOn;

	private bool autoGrowDialogHeight;

	private bool autoGrowPending;

	private static Object messsageNodeDialogPrefab;

	private static GameObject lightInScene;

	private static List<int> instructorPositions;

	private int instructorPosition;

	private bool autoClose;

	private int autoCloseTime;

	private double timeCloseDisplay;

	private AudioSource instructorAudio;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MessageNodeDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MessageNodeDialog Spawn(string headerText, string nodeText, string instructorPrefabName, string continueBtnText, Callback continueCallback = null, int textAreaSize = 135, ActionDialogMessage.DialogMessageArea msgArea = ActionDialogMessage.DialogMessageArea.Center, bool autoClose = false, int autoCloseTimeout = 20, bool autoGrowDialogHeight = false, string instructorName = "")
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
	private void OnSceneChange(GameScenes scenes)
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
	private void onClickContinueBtn()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CloseDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AssignInstructorPosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClearInstructorPosition()
	{
		throw null;
	}
}
