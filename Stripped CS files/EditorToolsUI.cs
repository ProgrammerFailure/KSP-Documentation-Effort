using Expansions.Serenity;
using ns11;
using ns2;
using UnityEngine;
using UnityEngine.UI;

public class EditorToolsUI : MonoBehaviour
{
	[SerializeField]
	public Toggle placeButton;

	[SerializeField]
	public Toggle moveButton;

	[SerializeField]
	public Toggle rotateButton;

	[SerializeField]
	public Toggle rootButton;

	public Color placeBtnColor0;

	public Color moveBtnColor0;

	public Color rotateBtnColor0;

	public Color rootBtnColor0;

	public ConstructionMode constructionMode;

	public UIPanelTransition panelTransition;

	public void OnEditorRestart()
	{
		if (EditorDriver.StartupBehaviour == EditorDriver.StartupBehaviours.START_CLEAN && base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: false);
		}
	}

	public void OnEditorLoad(ShipConstruct ct, CraftBrowserDialog.LoadType loadType)
	{
		if (loadType == CraftBrowserDialog.LoadType.Normal && !base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: true);
			SetMode(ConstructionMode.Place);
		}
	}

	public void Start()
	{
		GameEvents.onEditorRestart.Add(OnEditorRestart);
		GameEvents.onEditorLoad.Add(OnEditorLoad);
		placeButton.onValueChanged.AddListener(onPlaceButtonInput);
		moveButton.onValueChanged.AddListener(onMoveButtonInput);
		rotateButton.onValueChanged.AddListener(onRotateButtonInput);
		rootButton.onValueChanged.AddListener(onRootButtonInput);
		GameEvents.onEditorScreenChange.Add(OnEditorScreenChange);
		GameEvents.onEditorPartEvent.Add(onEditorPartEvent);
		OnEditorRestart();
	}

	public void OnDestroy()
	{
		GameEvents.onEditorRestart.Remove(OnEditorRestart);
		GameEvents.onEditorLoad.Remove(OnEditorLoad);
		GameEvents.onEditorScreenChange.Remove(OnEditorScreenChange);
		GameEvents.onEditorPartEvent.Remove(onEditorPartEvent);
	}

	public void onPlaceButtonInput(bool b)
	{
		if (b && placeButton.interactable)
		{
			SetMode(ConstructionMode.Place, updateUI: false);
		}
	}

	public void onMoveButtonInput(bool b)
	{
		if (b && moveButton.interactable)
		{
			SetMode(ConstructionMode.Move, updateUI: false);
		}
	}

	public void onRotateButtonInput(bool b)
	{
		if (b && rotateButton.interactable)
		{
			SetMode(ConstructionMode.Rotate, updateUI: false);
		}
	}

	public void onRootButtonInput(bool b)
	{
		if (b && rootButton.interactable)
		{
			SetMode(ConstructionMode.Root, updateUI: false);
		}
	}

	public void Update()
	{
		if (!EditorLogic.fetch.NameOrDescriptionFocused() && !DeltaVApp.AnyTextFieldHasFocus() && !RoboticControllerManager.AnyWindowTextFieldHasFocus())
		{
			if (GameSettings.Editor_modePlace.GetKeyDown())
			{
				SetMode(ConstructionMode.Place);
			}
			if (GameSettings.Editor_modeOffset.GetKeyDown())
			{
				SetMode(ConstructionMode.Move);
			}
			if (GameSettings.Editor_modeRotate.GetKeyDown())
			{
				SetMode(ConstructionMode.Rotate);
			}
			if (GameSettings.Editor_modeRoot.GetKeyDown())
			{
				SetMode(ConstructionMode.Root);
			}
		}
	}

	public void SetMode(ConstructionMode mode, bool updateUI = true)
	{
		switch (mode)
		{
		case ConstructionMode.Root:
			if (InputLockManager.IsLocked(ControlTypes.EDITOR_ROOT_REFLOW))
			{
				return;
			}
			break;
		default:
			if (InputLockManager.IsLocked(ControlTypes.EDITOR_GIZMO_TOOLS))
			{
				return;
			}
			break;
		case ConstructionMode.Place:
			break;
		}
		if ((mode == ConstructionMode.Move || mode == ConstructionMode.Rotate) && EditorLogic.SelectedPart != null && !EditorLogic.fetch.ship.Contains(EditorLogic.SelectedPart))
		{
			EditorLogic.fetch.GetComponent<AudioSource>().PlayOneShot(EditorLogic.fetch.cannotPlaceClip);
			return;
		}
		if (constructionMode != mode)
		{
			constructionMode = mode;
			GameEvents.onEditorConstructionModeChange.Fire(constructionMode);
		}
		if (updateUI)
		{
			switch (mode)
			{
			case ConstructionMode.Place:
				placeButton.isOn = true;
				break;
			case ConstructionMode.Move:
				moveButton.isOn = true;
				break;
			case ConstructionMode.Rotate:
				rotateButton.isOn = true;
				break;
			case ConstructionMode.Root:
				rootButton.isOn = true;
				break;
			}
		}
	}

	public void OnEditorScreenChange(EditorScreen scr)
	{
		switch (scr)
		{
		case EditorScreen.Parts:
			if (!base.gameObject.activeSelf)
			{
				panelTransition.TransitionImmediate("cargoOut");
				EditorLogic.fetch.StartCoroutine(CallbackUtil.DelayedCallback(0.5f, delegate
				{
					base.gameObject.SetActive(value: true);
				}));
			}
			else
			{
				panelTransition.Transition("cargoOut");
			}
			break;
		default:
			if (base.gameObject.activeSelf)
			{
				panelTransition.TransitionImmediate("cargoOut");
				base.gameObject.SetActive(value: false);
			}
			break;
		case EditorScreen.Cargo:
			if (!base.gameObject.activeSelf)
			{
				panelTransition.TransitionImmediate("cargoIn");
				EditorLogic.fetch.StartCoroutine(CallbackUtil.DelayedCallback(0.5f, delegate
				{
					base.gameObject.SetActive(value: true);
				}));
			}
			else
			{
				panelTransition.Transition("cargoIn");
			}
			break;
		}
	}

	public void onEditorPartEvent(ConstructionEventType evt, Part part)
	{
		if (EditorLogic.fetch.editorScreen == EditorScreen.Cargo)
		{
			return;
		}
		switch (evt)
		{
		case ConstructionEventType.PartCreated:
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(value: true);
			}
			SetMode(ConstructionMode.Place);
			break;
		case ConstructionEventType.PartPicked:
			SetMode(ConstructionMode.Place);
			break;
		case ConstructionEventType.PartDeleted:
			if (EditorLogic.RootPart == null)
			{
				if (base.gameObject.activeSelf)
				{
					base.gameObject.SetActive(value: false);
				}
			}
			else
			{
				SetMode(ConstructionMode.Place);
			}
			break;
		case ConstructionEventType.PartCopied:
			SetMode(ConstructionMode.Place);
			break;
		case ConstructionEventType.PartRootSelected:
			SetMode(ConstructionMode.Place);
			break;
		case ConstructionEventType.Unknown:
		case ConstructionEventType.PartDropped:
		case ConstructionEventType.PartDragging:
		case ConstructionEventType.PartAttached:
		case ConstructionEventType.PartDetached:
			break;
		}
	}
}
