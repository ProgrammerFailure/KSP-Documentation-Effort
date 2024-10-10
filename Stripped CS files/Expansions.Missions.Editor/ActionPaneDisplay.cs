using System;
using ns12;
using ns2;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class ActionPaneDisplay : Selectable, IPointerClickHandler, IEventSystemHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	[Serializable]
	public class RaycastEvent : UnityEvent<RaycastHit?>
	{
	}

	[Serializable]
	public class DragEvent : UnityEvent<PointerEventData.InputButton, Vector2>
	{
	}

	[Serializable]
	public class MouseOverEvent : UnityEvent<Vector2>
	{
	}

	[HideInInspector]
	public RectTransform rectTransform;

	[HideInInspector]
	public RawImage displayImage;

	[SerializeField]
	public Camera displayCamera;

	public RenderTexture displayTexture;

	public float hitDistance;

	public Ray ray;

	public RaycastHit hit;

	public int layerMask;

	public Vector2 cameraPoint;

	public float aspect;

	public bool isSelected;

	public bool isDragging;

	public bool isMouseOver;

	public RectMask2D displayMask;

	public DictionaryValueList<string, MonoBehaviour> toolbarControls;

	public RaycastEvent DisplayClick;

	public RaycastEvent DisplayClickUp;

	public DragEvent DisplayDrag;

	public RaycastEvent DisplayDragEnd;

	public MouseOverEvent MouseOver;

	public RenderTexture DisplayTexture => displayTexture;

	public override void Awake()
	{
		rectTransform = base.transform as RectTransform;
		displayTexture = new RenderTexture(1280, 720, 24, RenderTextureFormat.Default);
		displayMask = GetComponent<RectMask2D>();
		if (displayMask == null)
		{
			displayMask = base.gameObject.AddComponent<RectMask2D>();
		}
		aspect = (float)Screen.width / (float)Screen.height;
		hitDistance = 1000f;
		if (DisplayDrag == null)
		{
			DisplayDrag = new DragEvent();
		}
		if (DisplayDragEnd == null)
		{
			DisplayDragEnd = new RaycastEvent();
		}
		if (DisplayClick == null)
		{
			DisplayClick = new RaycastEvent();
		}
		if (DisplayClickUp == null)
		{
			DisplayClickUp = new RaycastEvent();
		}
		if (MouseOver == null)
		{
			MouseOver = new MouseOverEvent();
		}
		toolbarControls = new DictionaryValueList<string, MonoBehaviour>();
	}

	public virtual void Update()
	{
		if (isMouseOver && !MissionEditorLogic.Instance.isMouseOverGAPScroll && GetMousePointOnCamera(Input.mousePosition, UIMasterController.Instance.mainCanvas.worldCamera, ref cameraPoint))
		{
			OnMouseOver(cameraPoint);
			MouseOver.Invoke(cameraPoint);
		}
		UpdateDisplayArea();
	}

	public virtual void Setup(Camera displayCamera, int layerMask)
	{
		this.displayCamera = displayCamera;
		this.layerMask = layerMask;
		if (displayTexture == null)
		{
			displayTexture = new RenderTexture(1280, 720, 24, RenderTextureFormat.Default);
		}
		this.displayCamera.targetTexture = displayTexture;
		if (displayImage == null)
		{
			displayImage = new GameObject("Display", typeof(RawImage)).GetComponent<RawImage>();
			displayImage.rectTransform.SetParent(rectTransform, worldPositionStays: false);
		}
		displayImage.texture = displayTexture;
		UpdateDisplayArea();
	}

	public void UpdateDisplayArea()
	{
		if (!(displayImage == null))
		{
			displayImage.rectTransform.sizeDelta = new Vector2(aspect * rectTransform.rect.height, rectTransform.rect.height);
			displayImage.rectTransform.anchoredPosition = Vector2.zero;
		}
	}

	public virtual void Clean()
	{
	}

	public virtual void Destroy()
	{
		Clean();
		ClearToolbar();
		UnityEngine.Object.Destroy(this);
	}

	public bool GetMousePointOnCamera(Vector2 mousePosition, Camera canvasCamera, ref Vector2 point)
	{
		if (displayImage != null && RectTransformUtility.ScreenPointToLocalPointInRectangle(displayImage.rectTransform, mousePosition, canvasCamera, out var localPoint))
		{
			point.x = Mathf.InverseLerp(displayImage.rectTransform.rect.x, displayImage.rectTransform.rect.xMax, localPoint.x) * (float)displayCamera.pixelWidth;
			point.y = Mathf.InverseLerp(displayImage.rectTransform.rect.y, displayImage.rectTransform.rect.yMax, localPoint.y) * (float)displayCamera.pixelHeight;
			return true;
		}
		return false;
	}

	public bool Raycast(Vector3 cameraPoint, out RaycastHit hit, int layerMask = -1)
	{
		ray = displayCamera.ScreenPointToRay(cameraPoint);
		return Physics.Raycast(ray, out hit, hitDistance, (layerMask == -1) ? this.layerMask : layerMask);
	}

	public Button AddToolbarButton(string id, string icon, string toolTip)
	{
		return AddObject<Button>(id, "Prefabs/GAPButtonPrefab.prefab", icon, toolTip, toolbarControls, base.transform.parent.Find("DisplayHeader/LeftAnchor"));
	}

	public Toggle AddToolbarToggle(string id, string icon, string toolTip, bool startState = false)
	{
		Toggle toggle = AddObject<Toggle>(id, "Prefabs/GAPTogglePrefab.prefab", icon, toolTip, toolbarControls, base.transform.parent.Find("DisplayHeader/LeftAnchor"));
		if (toggle != null)
		{
			toggle.isOn = startState;
			return toggle;
		}
		return null;
	}

	public Button GetToolbarButton(string id)
	{
		if (toolbarControls.ContainsKey(id))
		{
			return toolbarControls[id] as Button;
		}
		return null;
	}

	public void ClearToolbarEvents()
	{
		for (int i = 0; i < toolbarControls.Count; i++)
		{
			MonoBehaviour monoBehaviour = toolbarControls.At(i);
			if (monoBehaviour is Button)
			{
				(monoBehaviour as Button).onClick.RemoveAllListeners();
			}
			else if (monoBehaviour is Toggle)
			{
				(monoBehaviour as Toggle).onValueChanged.RemoveAllListeners();
			}
		}
	}

	public void ClearToolbar()
	{
		for (int i = 0; i < toolbarControls.Count; i++)
		{
			UnityEngine.Object.Destroy(toolbarControls.At(i).gameObject);
		}
		toolbarControls.Clear();
	}

	public T AddObject<T>(string id, string prefab, string icon, string toolTip, DictionaryValueList<string, MonoBehaviour> controls, Transform parent) where T : MonoBehaviour
	{
		if (!controls.ContainsKey(id))
		{
			T component = UnityEngine.Object.Instantiate(MissionsUtils.MEPrefab(prefab), parent).GetComponent<T>();
			component.GetComponentInChildren<RawImage>().texture = MissionEditorLogic.Instance.actionPane.gapIconLoader.GetIcon(icon).iconNormal;
			component.GetComponent<TooltipController_Text>().textString = toolTip;
			controls.Add(id, component);
			return component;
		}
		return controls[id] as T;
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		if (eventData.button != 0 || !GetMousePointOnCamera(eventData.position, eventData.pressEventCamera, ref cameraPoint))
		{
			return;
		}
		ray = displayCamera.ScreenPointToRay(cameraPoint);
		if (isDragging)
		{
			if (Physics.Raycast(ray, out hit, hitDistance, layerMask))
			{
				OnDisplayDragEnd(hit);
				DisplayDragEnd.Invoke(hit);
			}
			else
			{
				OnDisplayDragEnd(null);
				DisplayDragEnd.Invoke(null);
			}
		}
		else if (Physics.Raycast(ray, out hit, hitDistance, layerMask))
		{
			OnDisplayClickUp(hit);
			DisplayClickUp.Invoke(hit);
		}
		else
		{
			OnDisplayClickUp(null);
			DisplayClickUp.Invoke(null);
		}
	}

	public virtual void OnPointerClick(PointerEventData eventData)
	{
		Select();
		if (eventData.button == PointerEventData.InputButton.Left && !isDragging && GetMousePointOnCamera(eventData.position, eventData.pressEventCamera, ref cameraPoint))
		{
			ray = displayCamera.ScreenPointToRay(cameraPoint);
			if (Physics.Raycast(ray, out hit, hitDistance, layerMask))
			{
				Debug.DrawRay(ray.origin, ray.direction * hitDistance, Color.green, 1f);
				OnDisplayClick(hit);
				DisplayClick.Invoke(hit);
			}
			else
			{
				Debug.DrawRay(ray.origin, ray.direction * hitDistance, Color.red, 1f);
				OnDisplayClick(null);
				DisplayClick.Invoke(null);
			}
		}
	}

	public virtual void OnBeginDrag(PointerEventData eventData)
	{
		isDragging = true;
	}

	public virtual void OnEndDrag(PointerEventData eventData)
	{
		isDragging = false;
	}

	public virtual void OnDrag(PointerEventData eventData)
	{
		OnDisplayDrag(eventData.button, eventData.delta);
		DisplayDrag.Invoke(eventData.button, eventData.delta);
	}

	public override void OnSelect(BaseEventData eventData)
	{
		base.OnSelect(eventData);
		isSelected = true;
	}

	public override void OnDeselect(BaseEventData eventData)
	{
		base.OnDeselect(eventData);
		isSelected = false;
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		base.OnPointerEnter(eventData);
		isMouseOver = true;
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		base.OnPointerExit(eventData);
		isMouseOver = false;
	}

	public virtual void OnDisplayDrag(PointerEventData.InputButton button, Vector2 delta)
	{
	}

	public virtual void OnDisplayDragEnd(RaycastHit? hit)
	{
	}

	public virtual void OnDisplayClick(RaycastHit? hit)
	{
	}

	public virtual void OnDisplayClickUp(RaycastHit? hit)
	{
	}

	public virtual void OnMouseOver(Vector2 position)
	{
	}
}
