using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ns25;

public class ScreenDatabaseList : MonoBehaviour
{
	public TextMeshQueue textMeshQueue;

	public ScrollRect scrollRect;

	public void Awake()
	{
		LoadDatabaseListItems();
	}

	public void OnEnable()
	{
		StartCoroutine(ScrollUp());
	}

	public IEnumerator ScrollUp()
	{
		yield return null;
		if (scrollRect != null)
		{
			scrollRect.verticalNormalizedPosition = 1f;
		}
	}

	public virtual void LoadDatabaseListItems()
	{
		for (int i = 0; i <= 100; i++)
		{
			textMeshQueue.AddLine("DatabaseList is not properly set up.");
		}
	}
}
