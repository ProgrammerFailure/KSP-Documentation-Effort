using UnityEngine;

namespace ns2;

public class Tooltip : MonoBehaviour
{
	public RectTransform rectTransform;

	public RectTransform RectTransform => rectTransform;

	public void Awake()
	{
		rectTransform = base.transform as RectTransform;
	}
}
