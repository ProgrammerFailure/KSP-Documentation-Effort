using ns2;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns18;

public class BuildingPickerItem : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	public Text buildingName;

	public SpaceCenterBuilding building;

	public Material lineMaterial;

	public Color lineColor;

	public float lineWidth;

	public float lineCornerRadius;

	public UIWorldPointer pointer;

	[SerializeField]
	public Vector2 uiPointerAnchor = new Vector2(1f, 0.5f);

	[SerializeField]
	public ButtonSpritesMgr btnSpriteMgr;

	public bool isHovering;

	public void Awake()
	{
		GameEvents.onGameUnpause.Add(OnGameUnpause);
	}

	public void OnDestroy()
	{
		GameEvents.onGameUnpause.Remove(OnGameUnpause);
	}

	public void Setup(SpaceCenterBuilding building, ButtonSpritesMgr.ButtonSprites spriteSet)
	{
		this.building = building;
		btnSpriteMgr.SetSpriteSet(spriteSet);
		building.OnClick.Add(OnBuildingClick);
		building.OnInViewChange.Add(OnBuildingInView);
	}

	public void OnPointerEnter(PointerEventData data)
	{
		isHovering = true;
		if (building.InView)
		{
			CreatePointer();
		}
		building.HighLightBuilding(mouseOverIcon: true);
	}

	public void CreatePointer()
	{
		if (pointer == null)
		{
			pointer = UIWorldPointer.Create((RectTransform)base.transform, building.BuildingTransform, FlightCamera.fetch.mainCamera, lineMaterial);
			pointer.transform.SetParent(base.transform);
			pointer.lineColor = lineColor;
			pointer.lineWidth = lineWidth;
			pointer.chamferDistance = lineCornerRadius;
			pointer.uiSnapType = UIWorldPointer.UISnapType.Anchor;
			pointer.uiSnapAnchor = uiPointerAnchor;
		}
	}

	public void DestroyPointer()
	{
		if (pointer != null)
		{
			Object.Destroy(pointer.gameObject);
		}
	}

	public void OnPointerExit(PointerEventData data)
	{
		isHovering = false;
		if (pointer != null)
		{
			Object.Destroy(pointer.gameObject);
		}
		building.HighLightBuilding(mouseOverIcon: false);
	}

	public void OnGameUnpause()
	{
		RectTransform rectTransform = base.transform as RectTransform;
		bool flag;
		if ((flag = RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, Input.mousePosition, UIMainCamera.Camera, out var worldPoint) && rectTransform.Contains(worldPoint)) && !isHovering)
		{
			OnPointerEnter(null);
		}
		else if (!flag && isHovering)
		{
			OnPointerExit(null);
		}
	}

	public void OnPointerClick(PointerEventData data)
	{
		isHovering = false;
		if (data.button == PointerEventData.InputButton.Left)
		{
			building.OnLeftClick();
		}
		else if (data.button == PointerEventData.InputButton.Right)
		{
			building.OnRightClick();
		}
	}

	public void OnBuildingClick(bool leftClick)
	{
		isHovering = false;
		DestroyPointer();
	}

	public void OnBuildingInView(bool inView)
	{
		if (!inView)
		{
			DestroyPointer();
		}
		else if (isHovering)
		{
			CreatePointer();
		}
	}
}
