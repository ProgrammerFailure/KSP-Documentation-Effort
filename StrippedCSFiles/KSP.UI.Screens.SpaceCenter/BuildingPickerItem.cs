using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI.Screens.SpaceCenter;

public class BuildingPickerItem : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	public Text buildingName;

	private SpaceCenterBuilding building;

	public Material lineMaterial;

	public Color lineColor;

	public float lineWidth;

	public float lineCornerRadius;

	private UIWorldPointer pointer;

	[SerializeField]
	private Vector2 uiPointerAnchor;

	[SerializeField]
	private ButtonSpritesMgr btnSpriteMgr;

	private bool isHovering;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public BuildingPickerItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(SpaceCenterBuilding building, ButtonSpritesMgr.ButtonSprites spriteSet)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerEnter(PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreatePointer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void DestroyPointer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerExit(PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGameUnpause()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerClick(PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnBuildingClick(bool leftClick)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnBuildingInView(bool inView)
	{
		throw null;
	}
}
