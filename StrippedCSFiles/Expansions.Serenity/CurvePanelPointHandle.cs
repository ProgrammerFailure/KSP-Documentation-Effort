using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Expansions.Serenity;

public class CurvePanelPointHandle : MonoBehaviour, IDragHandler, IEventSystemHandler
{
	public CurvePanelPoint point;

	public RectTransform rotationHandle;

	public CurvePanelPoint.TangentTypes tangent;

	[SerializeField]
	private Vector2 mouseScreenPos;

	[SerializeField]
	private Vector2 pointScreenPos;

	[SerializeField]
	private float tangentVal;

	private Vector3 handleRotation;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CurvePanelPointHandle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDrag(PointerEventData eventData)
	{
		throw null;
	}
}
