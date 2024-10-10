using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Expansions.Serenity;

public class CurvePanelPoint : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	public enum TangentTypes
	{
		In,
		Out
	}

	[SerializeField]
	public Button DeleteButton;

	[SerializeField]
	public Transform selectedStuff;

	[SerializeField]
	public Image pointImage;

	[SerializeField]
	public Color normalColor = Color.white;

	[SerializeField]
	public Color selectedColor = Color.white;

	[SerializeField]
	public bool noTangents;

	[SerializeField]
	public bool noValue;

	public bool unselectable;

	[SerializeField]
	public Transform leftHandleRotater;

	[SerializeField]
	public Transform rightHandleRotater;

	[SerializeField]
	public CanvasGroup leftHandleCanvasGroup;

	[SerializeField]
	public CanvasGroup rightHandleCanvasGroup;

	public Vector3 leftHandleRotation;

	public Vector3 rightHandleRotation;

	public int pointIndex;

	public int pointsCount;

	public Keyframe editingKeyframe;

	public Keyframe Keyframe { get; set; }

	public CurvePanel Panel { get; set; }

	public bool Selected { get; set; }

	public void Setup(CurvePanel panel, Keyframe keyframe)
	{
		Panel = panel;
		Keyframe = keyframe;
		editingKeyframe = keyframe;
		pointImage.color = normalColor;
		noValue = panel.noValues;
	}

	public void Awake()
	{
		if (!ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			Object.Destroy(base.gameObject);
			return;
		}
		DeleteButton.onClick.AddListener(OnDeleteClick);
		selectedStuff.gameObject.SetActive(value: false);
	}

	public void OnDestroy()
	{
		DeleteButton.onClick.RemoveListener(OnDeleteClick);
	}

	public void OnDeleteClick()
	{
		Delete();
	}

	public void Select(bool hideDelete = false)
	{
		if (!unselectable)
		{
			Selected = true;
			pointImage.color = selectedColor;
			selectedStuff.gameObject.SetActive(value: true);
			pointIndex = Panel.GetIndexOfPoint(this);
			pointsCount = Panel.GetCurvePointsCount();
			_ = Panel.YAxisMax;
			_ = Panel.YAxisMin;
			DeleteButton.gameObject.SetActive(!hideDelete && (Panel.noEndPoints || (pointIndex != 0 && pointIndex < pointsCount - 1)));
			if (!noTangents)
			{
				leftHandleCanvasGroup.alpha = ((pointIndex != 0) ? 1 : 0);
				rightHandleCanvasGroup.alpha = ((pointIndex < pointsCount - 1) ? 1 : 0);
				SetTangentHandles();
			}
		}
	}

	public void Deselect()
	{
		Selected = false;
		pointImage.color = normalColor;
		selectedStuff.gameObject.SetActive(value: false);
	}

	public void HideDelete()
	{
		DeleteButton.gameObject.SetActive(value: false);
	}

	public void ShowDelete()
	{
		DeleteButton.gameObject.SetActive(value: true);
	}

	public void SetTangentHandles()
	{
		if (!noTangents)
		{
			leftHandleRotation = leftHandleRotater.gameObject.transform.localEulerAngles;
			leftHandleRotation.z = Mathf.Atan(Keyframe.inTangent / Panel.PanelValueRatio * Panel.PanelRectRatio) * 57.29578f;
			leftHandleRotater.gameObject.transform.localEulerAngles = leftHandleRotation;
			rightHandleRotation = rightHandleRotater.gameObject.transform.localEulerAngles;
			rightHandleRotation.z = Mathf.Atan(Keyframe.outTangent / Panel.PanelValueRatio * Panel.PanelRectRatio) * 57.29578f;
			rightHandleRotater.gameObject.transform.localEulerAngles = rightHandleRotation;
		}
	}

	public void SetKeyFrame(float time, float value)
	{
		editingKeyframe.time = time;
		editingKeyframe.value = value;
		Keyframe = editingKeyframe;
	}

	public void SetKeyFrameTangents(float inTangent, float outTangent)
	{
		editingKeyframe.inTangent = inTangent;
		editingKeyframe.outTangent = outTangent;
		Keyframe = editingKeyframe;
	}

	public void SetKeyFrameTangent(TangentTypes tangentType, float tangentValue)
	{
		if (tangentType == TangentTypes.In)
		{
			editingKeyframe.inTangent = tangentValue;
		}
		else
		{
			editingKeyframe.outTangent = tangentValue;
		}
		Keyframe = editingKeyframe;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!Panel.Editable)
		{
			return;
		}
		if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
		{
			if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
			{
				Panel.MousePointClick(this, withShift: false, withCtrl: false);
			}
			else
			{
				Panel.MousePointClick(this, withShift: false, withCtrl: true);
			}
		}
		else
		{
			Panel.MousePointClick(this, withShift: true, withCtrl: false);
		}
	}

	public void Delete()
	{
		Panel.DeletePoint(this);
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (Panel.Editable)
		{
			SetTangentHandles();
			Panel.BeginDragPoint(this, eventData);
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (Panel.Editable)
		{
			Panel.EndDragPoint(this, eventData);
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (Panel.Editable)
		{
			Panel.OnDragPoint(this, eventData);
		}
	}

	public void OnTangentDrag(TangentTypes tangentType, float tanValue)
	{
		Panel.OnTangentDrag(this, tangentType, tanValue);
	}
}
