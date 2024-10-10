using UnityEngine;

public class FacilityScreenFitter : MonoBehaviour
{
	public RectTransform rtScaleToFit;

	public RectTransform rtScaleThis;

	public void Awake()
	{
		rtScaleThis = base.transform as RectTransform;
	}

	public void Start()
	{
		if (rtScaleThis.rect.width > rtScaleToFit.rect.width)
		{
			float height = rtScaleThis.rect.height;
			float num = rtScaleToFit.rect.width / rtScaleThis.rect.width;
			rtScaleThis.localScale = new Vector3(num, num, 1f);
			rtScaleThis.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height / num);
		}
	}
}
