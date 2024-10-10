using UnityEngine;

namespace ns14;

public class InvertSortOrder : MonoBehaviour
{
	[ContextMenu("Invert Sort Order")]
	public void InvertSortingOrder()
	{
		RectTransform component = base.gameObject.GetComponent<RectTransform>();
		RecurseLevel(component);
	}

	public void RecurseLevel(RectTransform lvlRoot)
	{
		Transform transform = null;
		int childCount = lvlRoot.childCount;
		for (int i = 0; i < childCount; i++)
		{
			transform = lvlRoot.GetChild(i);
			if (transform is RectTransform)
			{
				transform.SetSiblingIndex(childCount - 1 - i);
				if (transform.childCount >= 1)
				{
					RecurseLevel(transform as RectTransform);
				}
			}
		}
	}
}
