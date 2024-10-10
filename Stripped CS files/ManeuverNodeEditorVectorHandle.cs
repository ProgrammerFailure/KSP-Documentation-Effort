using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ManeuverNodeEditorVectorHandle : MonoBehaviour, IDragHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
	public Color highlightedColor;

	public Button handleButton;

	public RectTransform handleButton_RT;

	public Vector3 handleScale;

	public NavBallVector vectorType;

	public double vectorModifyRate;

	public double dragAccelerationLimit = 100.0;

	public ManeuverNodeEditorManager editorComponent;

	public Vector2 directorVector;

	public Color originalColor;

	public bool positiveDirection = true;

	public double modifyAmount;

	public bool drag;

	public bool hover;

	public double currentDragAcceleration;

	public float dragAngle;

	public Vector2 originalMousePos;

	public Vector2 mouseDiferential;

	public WaitForSeconds waitAndReleasePointer;

	[SerializeField]
	public Image dragLine;

	public void Start()
	{
		if ((bool)FlightUIModeController.Instance)
		{
			editorComponent = FlightUIModeController.Instance.manNodeHandleEditor.GetComponent<ManeuverNodeEditorManager>();
		}
		currentDragAcceleration = 0.0;
		originalMousePos = default(Vector2);
		mouseDiferential = default(Vector2);
		waitAndReleasePointer = new WaitForSeconds(1f);
		handleButton_RT = handleButton.GetComponent<RectTransform>();
		if (handleButton_RT != null)
		{
			handleScale = handleButton_RT.localScale;
		}
		else
		{
			handleScale = Vector3.one;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			handleButton_RT.localScale = handleScale * 1.6f;
			originalColor = handleButton.targetGraphic.color;
			handleButton.targetGraphic.color = highlightedColor;
			if (dragLine != null)
			{
				dragLine.color = XKCDColors.AquaBlue;
			}
			drag = true;
			currentDragAcceleration = 0.0;
			originalMousePos.x = Input.mousePosition.x;
			originalMousePos.y = Input.mousePosition.y;
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			handleButton_RT.localScale *= 0.625f;
			handleButton.targetGraphic.color = originalColor;
			if (dragLine != null)
			{
				dragLine.color = Color.white;
			}
			drag = false;
			currentDragAcceleration = 0.0;
			originalMousePos = Vector2.zero;
			if (vectorType == NavBallVector.const_3)
			{
				editorComponent.usage.utHandle++;
			}
			else
			{
				editorComponent.usage.vectorHandle++;
			}
			StartCoroutine("MouseUpCooldown");
		}
	}

	public IEnumerator MouseUpCooldown()
	{
		yield return waitAndReleasePointer;
		if (!editorComponent.MouseWithinTool)
		{
			editorComponent.SetMouseOverGizmo(state: false);
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (currentDragAcceleration < dragAccelerationLimit)
		{
			currentDragAcceleration += mouseDiferential.magnitude;
		}
	}

	public void OnPointerEnter(PointerEventData pointerEventData)
	{
		hover = true;
		editorComponent.SetMouseOverGizmo(state: true);
	}

	public void OnPointerExit(PointerEventData pointerEventData)
	{
		hover = false;
	}

	public void Update()
	{
		if (!drag && hover)
		{
			if (GameSettings.AXIS_MOUSEWHEEL.GetAxis() != 0f)
			{
				modifyAmount = vectorModifyRate * (double)GameSettings.AXIS_MOUSEWHEEL.GetAxis();
				switch (vectorType)
				{
				case NavBallVector.PROGRADE:
					modifyAmount += editorComponent.SelectedManeuverNode.DeltaV.z;
					break;
				case NavBallVector.NORMAL:
					modifyAmount += editorComponent.SelectedManeuverNode.DeltaV.y;
					break;
				case NavBallVector.RADIAL:
					modifyAmount += editorComponent.SelectedManeuverNode.DeltaV.x;
					break;
				case NavBallVector.const_3:
					modifyAmount += editorComponent.SelectedManeuverNode.double_0;
					break;
				}
				if (vectorType == NavBallVector.const_3)
				{
					editorComponent.usage.utHandle++;
				}
				else
				{
					editorComponent.usage.vectorHandle++;
				}
				editorComponent.ModifyBurnVector(vectorType, modifyAmount);
			}
		}
		else
		{
			if (!drag)
			{
				return;
			}
			editorComponent.SetMouseOverGizmo(state: true);
			mouseDiferential.x = Input.mousePosition.x - originalMousePos.x;
			mouseDiferential.y = Input.mousePosition.y - originalMousePos.y;
			dragAngle = Vector2.Dot(mouseDiferential.normalized, directorVector.normalized);
			if (!(Mathf.Abs(dragAngle) < 0.1f))
			{
				if (dragAngle < 0f)
				{
					positiveDirection = false;
				}
				else
				{
					positiveDirection = true;
				}
				modifyAmount = (positiveDirection ? (vectorModifyRate * 0.05 * currentDragAcceleration) : (vectorModifyRate * 0.05 * currentDragAcceleration * -1.0));
				switch (vectorType)
				{
				case NavBallVector.PROGRADE:
					modifyAmount += editorComponent.SelectedManeuverNode.DeltaV.z;
					break;
				case NavBallVector.NORMAL:
					modifyAmount += editorComponent.SelectedManeuverNode.DeltaV.y;
					break;
				case NavBallVector.RADIAL:
					modifyAmount += editorComponent.SelectedManeuverNode.DeltaV.x;
					break;
				case NavBallVector.const_3:
					modifyAmount += editorComponent.SelectedManeuverNode.double_0;
					break;
				}
				editorComponent.ModifyBurnVector(vectorType, modifyAmount);
			}
		}
	}
}
