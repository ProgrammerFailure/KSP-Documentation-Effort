using UnityEngine;
using UnityEngine.UI;

public class EVAConstructionToolsUI : MonoBehaviour
{
	[SerializeField]
	public Toggle placeButton;

	[SerializeField]
	public Toggle moveButton;

	[SerializeField]
	public Toggle rotateButton;

	[SerializeField]
	public Button coordButton;

	public ConstructionMode constructionMode;

	public void Awake()
	{
		ToggleButtonListeners(toggle: true);
	}

	public void Update()
	{
		if ((!(EVAConstructionModeController.Instance != null) || EVAConstructionModeController.Instance.IsOpen || !DeltaVApp.AnyTextFieldHasFocus()) && EVAConstructionModeController.Instance.panelMode == EVAConstructionModeController.PanelMode.Construction)
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
		}
	}

	public void OnDestroy()
	{
		ToggleButtonListeners(toggle: false);
	}

	public void ToggleButtonListeners(bool toggle)
	{
		if (toggle)
		{
			placeButton.onValueChanged.AddListener(onPlaceButtonInput);
			moveButton.onValueChanged.AddListener(onMoveButtonInput);
			rotateButton.onValueChanged.AddListener(onRotateButtonInput);
		}
		else
		{
			placeButton.onValueChanged.RemoveListener(onPlaceButtonInput);
			moveButton.onValueChanged.RemoveListener(onMoveButtonInput);
			rotateButton.onValueChanged.RemoveListener(onRotateButtonInput);
		}
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

	public void SetMode(ConstructionMode mode, bool updateUI = true)
	{
		if (!(EVAConstructionModeController.Instance == null) && !(EVAConstructionModeController.Instance.evaEditor == null))
		{
			if (mode != 0 && InputLockManager.IsLocked(ControlTypes.EDITOR_GIZMO_TOOLS))
			{
				OnChangeModeBlocked();
				return;
			}
			if (constructionMode == ConstructionMode.Place && (mode == ConstructionMode.Move || mode == ConstructionMode.Rotate) && EVAConstructionModeController.Instance.evaEditor.SelectedPart != null)
			{
				EVAConstructionModeController.Instance.evaEditor.PlayAudioClip(EVAConstructionModeController.Instance.evaEditor.cannotPlaceClip);
				OnChangeModeBlocked();
				return;
			}
			if (constructionMode != mode)
			{
				constructionMode = mode;
				GameEvents.OnEVAConstructionModeChanged.Fire(constructionMode);
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
					break;
				}
			}
		}
		else
		{
			OnChangeModeBlocked();
		}
	}

	public void OnChangeModeBlocked()
	{
		if (placeButton != null)
		{
			placeButton.isOn = constructionMode == ConstructionMode.Place;
		}
		if (moveButton != null)
		{
			moveButton.isOn = constructionMode == ConstructionMode.Move;
		}
		if (rotateButton != null)
		{
			rotateButton.isOn = constructionMode == ConstructionMode.Rotate;
		}
	}

	public void ShowModeTools()
	{
		placeButton.gameObject.SetActive(value: true);
		moveButton.gameObject.SetActive(value: true);
		rotateButton.gameObject.SetActive(value: true);
		coordButton.gameObject.SetActive(value: true);
	}

	public void HideModeTools()
	{
		placeButton.gameObject.SetActive(value: false);
		moveButton.gameObject.SetActive(value: false);
		rotateButton.gameObject.SetActive(value: false);
		coordButton.gameObject.SetActive(value: false);
	}
}
