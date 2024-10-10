using UnityEngine;
using UnityEngine.UI;

namespace ns2;

public class ProgressBar : MonoBehaviour
{
	public Image foregroundImage;

	public float Value
	{
		get
		{
			if (foregroundImage != null)
			{
				return foregroundImage.fillAmount;
			}
			return 0f;
		}
		set
		{
			if (foregroundImage != null)
			{
				foregroundImage.fillAmount = value;
			}
		}
	}

	public void Start()
	{
		Value = 0f;
		foregroundImage = base.gameObject.GetComponent<Image>();
	}
}
