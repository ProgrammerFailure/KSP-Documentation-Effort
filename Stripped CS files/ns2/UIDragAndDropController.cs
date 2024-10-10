using UnityEngine;

namespace ns2;

public class UIDragAndDropController : MonoBehaviour
{
	public static UIDragAndDropController Instance;

	public RectTransform dragPlane;

	public static RectTransform mainTransform;

	public static UIList dragFromList;

	public void Awake()
	{
		if (Instance != null)
		{
			base.gameObject.DestroyGameObject();
			return;
		}
		Instance = this;
		dragPlane.gameObject.SetActive(value: true);
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public static bool Register(RectTransform draggedTransform, RectTransform mainTransform, bool reparentOnDrag)
	{
		UIDragAndDropController.mainTransform = mainTransform;
		UIListItem componentInParent = UIDragAndDropController.mainTransform.GetComponentInParent<UIListItem>();
		if (componentInParent != null)
		{
			UIList componentInParent2 = componentInParent.GetComponentInParent<UIList>();
			if (componentInParent2 != null)
			{
				dragFromList = componentInParent2;
			}
		}
		if (reparentOnDrag)
		{
			draggedTransform.SetParent(Instance.dragPlane);
			draggedTransform.transform.localPosition = new Vector3(draggedTransform.transform.localPosition.x, draggedTransform.transform.localPosition.y, 0f);
		}
		return true;
	}

	public static void Unregister(RectTransform rt)
	{
		mainTransform = null;
		dragFromList = null;
		Instance.dragPlane.DetachChildren();
	}
}
