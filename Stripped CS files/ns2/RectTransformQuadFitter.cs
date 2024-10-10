using System.Collections;
using UnityEngine;

namespace ns2;

public class RectTransformQuadFitter : MonoBehaviour
{
	public RectTransform rectTransform;

	public Transform quadTransform;

	public bool updateDaemonRunning;

	public void Awake()
	{
		StartCoroutine(UpdateDaemon());
	}

	public IEnumerator UpdateDaemon()
	{
		yield return null;
		yield return null;
		if (updateDaemonRunning)
		{
			yield break;
		}
		updateDaemonRunning = true;
		while ((bool)this)
		{
			if (rectTransform.hasChanged)
			{
				rectTransform.hasChanged = false;
				quadTransform.localScale = new Vector3(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y, 1f);
			}
			yield return null;
		}
	}
}
