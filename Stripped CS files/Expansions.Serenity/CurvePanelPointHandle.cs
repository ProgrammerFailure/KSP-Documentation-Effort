using ns2;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Expansions.Serenity;

public class CurvePanelPointHandle : MonoBehaviour, IDragHandler, IEventSystemHandler
{
	public CurvePanelPoint point;

	public RectTransform rotationHandle;

	public CurvePanelPoint.TangentTypes tangent;

	[SerializeField]
	public Vector2 mouseScreenPos = Vector2.zero;

	[SerializeField]
	public Vector2 pointScreenPos = Vector2.zero;

	[SerializeField]
	public float tangentVal;

	public Vector3 handleRotation;

	public void OnDrag(PointerEventData eventData)
	{
		mouseScreenPos = eventData.position;
		pointScreenPos = RectTransformUtility.WorldToScreenPoint(UIMasterController.Instance.uiCamera, point.gameObject.transform.position);
		if (tangent == CurvePanelPoint.TangentTypes.In)
		{
			mouseScreenPos.x = Mathf.Min(mouseScreenPos.x, pointScreenPos.x - 0.01f);
		}
		else
		{
			mouseScreenPos.x = Mathf.Max(mouseScreenPos.x, pointScreenPos.x + 0.01f);
		}
		tangentVal = (mouseScreenPos.y - pointScreenPos.y) / (mouseScreenPos.x - pointScreenPos.x);
		handleRotation = rotationHandle.localEulerAngles;
		handleRotation.z = Mathf.Atan(tangentVal) * 57.29578f;
		rotationHandle.localEulerAngles = handleRotation;
		point.OnTangentDrag(tangent, tangentVal);
	}
}
