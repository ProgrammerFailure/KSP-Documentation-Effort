using System.Collections.Generic;
using Expansions.Missions.Actions;
using ns11;
using ns2;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Runtime;

public class MessageNodeDialog : MonoBehaviour
{
	public bool modal;

	public CanvasGroup canvasGroup;

	[SerializeField]
	public TextMeshProUGUI headerText;

	[SerializeField]
	public RawImage instructorPortrait;

	[SerializeField]
	public TextMeshProUGUI instructorText;

	[SerializeField]
	public TextMeshProUGUI nodeText;

	[SerializeField]
	public Button continueButton;

	[SerializeField]
	public TextMeshProUGUI continueBtnText;

	[SerializeField]
	public RectTransform dialogRect;

	[SerializeField]
	public LayoutElement textSectionLayoutElement;

	[SerializeField]
	public Object strategy_AvatarLights;

	public Callback continueCallback;

	public KerbalInstructorBase instructor;

	public GameObject mainlight;

	public GameObject backlight;

	public string instructorPrefabName = "";

	public string instructorName = "";

	public int instructorPortraitSize = 128;

	public RenderTexture instructorTexture;

	public GameObject avatarLights;

	public bool lightsOn;

	public bool autoGrowDialogHeight;

	public bool autoGrowPending;

	public static Object messsageNodeDialogPrefab;

	public static GameObject lightInScene;

	public static List<int> instructorPositions;

	public int instructorPosition;

	public bool autoClose;

	public int autoCloseTime;

	public double timeCloseDisplay;

	public AudioSource instructorAudio;

	public static MessageNodeDialog Spawn(string headerText, string nodeText, string instructorPrefabName, string continueBtnText, Callback continueCallback = null, int textAreaSize = 135, ActionDialogMessage.DialogMessageArea msgArea = ActionDialogMessage.DialogMessageArea.Center, bool autoClose = false, int autoCloseTimeout = 20, bool autoGrowDialogHeight = false, string instructorName = "")
	{
		messsageNodeDialogPrefab = MissionsUtils.MEPrefab("_UI5/Dialogs/MessageNodeDialog/prefabs/MessageNodeDialog.prefab");
		if (messsageNodeDialogPrefab != null)
		{
			MessageNodeDialog component = ((GameObject)Object.Instantiate(messsageNodeDialogPrefab)).GetComponent<MessageNodeDialog>();
			component.gameObject.transform.SetParent(PopupDialogController.PopupDialogCanvas.transform, worldPositionStays: true);
			RectTransform component2 = PopupDialogController.PopupDialogCanvas.gameObject.GetComponent<RectTransform>();
			Vector3 zero = Vector3.zero;
			switch (msgArea)
			{
			case ActionDialogMessage.DialogMessageArea.Left:
				zero.x = component2.rect.xMin + 250f;
				break;
			case ActionDialogMessage.DialogMessageArea.Center:
				zero.x = 0f;
				break;
			case ActionDialogMessage.DialogMessageArea.Right:
				zero.x = component2.rect.xMax - 250f;
				break;
			}
			component.gameObject.transform.localPosition = zero;
			component.headerText.text = headerText;
			component.nodeText.text = nodeText;
			component.continueBtnText.text = continueBtnText;
			component.continueCallback = continueCallback;
			component.instructorPrefabName = instructorPrefabName;
			component.instructorName = instructorName;
			component.autoGrowDialogHeight = autoGrowDialogHeight;
			if (autoGrowDialogHeight)
			{
				component.autoGrowPending = true;
			}
			textAreaSize = (int)Mathf.Clamp(textAreaSize, 135f, 300f);
			component.textSectionLayoutElement.minHeight = textAreaSize;
			component.autoClose = autoClose;
			component.autoCloseTime = autoCloseTimeout;
			return component;
		}
		return null;
	}

	public void Awake()
	{
		continueButton.onClick.AddListener(onClickContinueBtn);
		GameEvents.onGameSceneLoadRequested.Add(OnSceneChange);
	}

	public void Start()
	{
		AssignInstructorPosition();
		GameObject gameObject = null;
		if (!string.IsNullOrEmpty(instructorPrefabName))
		{
			gameObject = MissionsUtils.MEPrefab("Prefabs/" + instructorPrefabName + ".prefab");
		}
		if (gameObject != null)
		{
			GameObject gameObject2 = Object.Instantiate(gameObject);
			instructor = gameObject2.GetComponent<KerbalInstructorBase>();
			instructor.gameObject.SetLayerRecursive(LayerMask.NameToLayer("KerbalInstructors"));
			instructor.instructorCamera.gameObject.SetActive(value: true);
			if (!string.IsNullOrEmpty(instructorName))
			{
				instructor.CharacterName = instructorName;
			}
			Vector3 position = new Vector3(3f * (float)instructorPosition, 0f, 0f);
			gameObject2.transform.position = position;
			if (instructorTexture != null)
			{
				instructorTexture.DiscardContents();
				instructorTexture.Release();
				Object.Destroy(instructorTexture);
				instructorTexture = null;
			}
			instructorTexture = new RenderTexture(instructorPortraitSize, instructorPortraitSize, 24);
			instructor.SetupCamera(instructorTexture);
			instructorPortrait.texture = instructorTexture;
			instructorText.text = instructor.CharacterName;
			instructorAudio = instructor.GetComponent<AudioSource>();
			GameObject child = instructor.gameObject.GetChild("backdrop");
			if (child != null)
			{
				child.gameObject.SetActive(value: true);
			}
			mainlight = instructor.gameObject.GetChild("mainlight");
			if (mainlight != null)
			{
				mainlight.SetActive(value: true);
				backlight = instructor.gameObject.GetChild("backlight");
				if (backlight != null)
				{
					backlight.SetActive(value: true);
				}
				if (mainlight.GetComponent<Light>().isActiveAndEnabled)
				{
					lightsOn = true;
				}
				if (lightInScene == null)
				{
					lightInScene = mainlight;
				}
			}
			if (mainlight == null)
			{
				avatarLights = (GameObject)Object.Instantiate(strategy_AvatarLights);
				avatarLights.transform.SetParent(instructor.transform);
				avatarLights.transform.localPosition = Vector3.zero;
				if (avatarLights.gameObject.GetChild("mainlight").GetComponent<Light>().isActiveAndEnabled)
				{
					lightsOn = true;
				}
				if (lightInScene == null)
				{
					lightInScene = avatarLights;
				}
			}
		}
		else
		{
			instructorPortrait.gameObject.SetActive(value: false);
			instructorText.gameObject.SetActive(value: false);
		}
		canvasGroup = GetComponent<CanvasGroup>();
		if (modal)
		{
			UIMasterController.Instance.RegisterModalDialog(canvasGroup);
		}
		else
		{
			UIMasterController.Instance.RegisterNonModalDialog(canvasGroup);
		}
		if (autoClose)
		{
			timeCloseDisplay = Planetarium.GetUniversalTime() + (double)autoCloseTime;
		}
	}

	public void OnSceneChange(GameScenes scenes)
	{
		CloseDialog();
	}

	public void OnDestroy()
	{
		if (instructor != null)
		{
			instructor.ClearCamera();
			if (instructor.gameObject != null)
			{
				Object.Destroy(instructor.gameObject);
			}
		}
		continueButton.onClick.RemoveListener(onClickContinueBtn);
		GameEvents.onGameSceneLoadRequested.Remove(OnSceneChange);
		if (UIMasterController.Instance != null)
		{
			if (modal)
			{
				UIMasterController.Instance.UnregisterModalDialog(canvasGroup);
			}
			else
			{
				UIMasterController.Instance.UnregisterNonModalDialog(canvasGroup);
			}
		}
		ClearInstructorPosition();
		CloseDialog();
	}

	public void Update()
	{
		if (instructor != null)
		{
			if (instructorAudio != null)
			{
				instructorAudio.panStereo = Mathf.Lerp(-1f, 1f, Mathf.InverseLerp(0f, Screen.width, dialogRect.rect.x + dialogRect.rect.width * 0.5f));
			}
			if (mainlight != null)
			{
				if (lightInScene != null && lightInScene != mainlight)
				{
					mainlight.SetActive(value: false);
					if (backlight != null)
					{
						backlight.SetActive(value: false);
					}
					lightsOn = false;
					return;
				}
				if (!HighLogic.LoadedSceneIsEditor && !(MissionControl.Instance != null) && !(Administration.Instance != null))
				{
					if (!lightsOn)
					{
						mainlight.SetActive(value: true);
						if (backlight != null)
						{
							backlight.SetActive(value: true);
						}
						lightsOn = true;
						if (lightInScene == null)
						{
							lightInScene = mainlight;
						}
					}
				}
				else
				{
					mainlight.SetActive(value: false);
					if (backlight != null)
					{
						backlight.SetActive(value: false);
					}
					lightsOn = false;
				}
			}
			else if (avatarLights != null)
			{
				if (lightInScene != null && lightInScene != avatarLights)
				{
					if (lightsOn)
					{
						avatarLights.SetActive(value: false);
						lightsOn = false;
					}
					return;
				}
				if (lightsOn && (HighLogic.LoadedSceneIsEditor || MissionControl.Instance != null || Administration.Instance != null))
				{
					avatarLights.SetActive(value: false);
					lightsOn = false;
				}
				else if (!lightsOn && !HighLogic.LoadedSceneIsEditor && MissionControl.Instance == null && Administration.Instance == null)
				{
					avatarLights.SetActive(value: true);
					lightsOn = true;
					if (lightInScene == null)
					{
						lightInScene = mainlight;
					}
				}
			}
		}
		if (autoClose && Planetarium.GetUniversalTime() > timeCloseDisplay)
		{
			onClickContinueBtn();
		}
		if (autoGrowDialogHeight)
		{
			if (autoGrowPending && !nodeText.havePropertiesChanged)
			{
				textSectionLayoutElement.minHeight = Mathf.Clamp(nodeText.preferredHeight + 5f, 135f, 300f);
				autoGrowPending = false;
			}
			else if (!autoGrowPending && nodeText.havePropertiesChanged)
			{
				autoGrowPending = true;
			}
		}
	}

	public void onClickContinueBtn()
	{
		if (continueCallback != null)
		{
			continueCallback();
		}
		CloseDialog();
	}

	public void CloseDialog()
	{
		Object.Destroy(base.gameObject);
	}

	public void AssignInstructorPosition()
	{
		if (instructorPositions == null)
		{
			instructorPositions = new List<int>();
		}
		int num = 0;
		while (instructorPosition < 1)
		{
			num++;
			if (instructorPositions.IndexOf(num) < 0)
			{
				instructorPosition = num;
				instructorPositions.Add(instructorPosition);
			}
			if (num > 1000)
			{
				break;
			}
		}
	}

	public void ClearInstructorPosition()
	{
		if (instructorPositions.IndexOf(instructorPosition) > -1)
		{
			instructorPositions.Remove(instructorPosition);
		}
	}
}
