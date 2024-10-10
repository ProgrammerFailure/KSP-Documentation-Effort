using UnityEngine;

public class UIListClamper : MonoBehaviour
{
	public RectTransform anchorList;

	public RectTransform anchorPlaceholders;

	public int listMaxSize = -1;

	public RectTransform prefabPlaceholder;

	public RectTransform placeholder;

	public float timer;

	public bool update;

	public void Awake()
	{
		placeholder = Object.Instantiate(prefabPlaceholder);
		placeholder.SetParent(anchorList, worldPositionStays: false);
		placeholder.gameObject.SetActive(value: false);
		timer = Time.realtimeSinceStartup;
	}

	public void OnRectTransformDimensionsChange()
	{
		if (timer != Time.realtimeSinceStartup)
		{
			timer = Time.realtimeSinceStartup;
			update = true;
		}
	}

	public void FixedUpdate()
	{
		if (update)
		{
			Clamp();
			update = false;
		}
	}

	public void Clamp()
	{
		if (listMaxSize == -1)
		{
			return;
		}
		if (anchorList.childCount - 1 > listMaxSize)
		{
			placeholder.gameObject.SetActive(value: true);
			for (int num = anchorList.childCount - 1; num > listMaxSize; num--)
			{
				anchorList.GetChild(anchorList.childCount - 1).SetParent(anchorPlaceholders, worldPositionStays: false);
			}
		}
		else
		{
			int num2 = Mathf.Clamp(Mathf.Clamp(listMaxSize - (anchorList.childCount - 1), 0, listMaxSize), 0, anchorPlaceholders.childCount);
			for (int i = 0; i < num2; i++)
			{
				anchorPlaceholders.GetChild(0).SetParent(anchorList, worldPositionStays: false);
			}
			placeholder.gameObject.SetActive(anchorPlaceholders.childCount > 0);
		}
	}
}
