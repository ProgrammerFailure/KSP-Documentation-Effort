using UnityEngine.EventSystems;

namespace ns2;

public class UIListDropArea : DropHandler
{
	public UIList dropOnList;

	public bool dropOnListWasSerialized;

	public void Awake()
	{
		if (dropOnList != null)
		{
			dropOnListWasSerialized = true;
		}
	}

	public override void OnDrop(PointerEventData eventData)
	{
		if (UIDragAndDropController.mainTransform == null)
		{
			return;
		}
		UIList componentInParent = dropOnList;
		if (!dropOnListWasSerialized)
		{
			componentInParent = GetComponentInParent<UIList>();
		}
		if (componentInParent == null)
		{
			return;
		}
		UIListItem component = UIDragAndDropController.mainTransform.GetComponent<UIListItem>();
		if (component == null)
		{
			return;
		}
		UIListItem componentInParent2 = GetComponentInParent<UIListItem>();
		if (UIDragAndDropController.dragFromList != null)
		{
			int index = UIDragAndDropController.dragFromList.GetIndex(component);
			if (UIDragAndDropController.dragFromList == componentInParent && index == componentInParent.GetIndex(componentInParent2))
			{
				return;
			}
		}
		if (componentInParent.Contains(componentInParent2))
		{
			int index2 = componentInParent.GetIndex(componentInParent2);
			componentInParent.onDrop.Invoke(UIDragAndDropController.dragFromList, component, index2);
		}
		else
		{
			int index2 = componentInParent.Count;
			componentInParent.onDrop.Invoke(UIDragAndDropController.dragFromList, component, index2);
		}
		base.OnDrop(eventData);
	}
}
