using UnityEngine;
using UnityEngine.UI;

namespace ns11;

public class VesselIconSprite : MonoBehaviour
{
	[SerializeField]
	public Image image;

	[SerializeField]
	public Sprite[] vesselTypeIcons;

	public VesselType type;

	public VesselType vesselType => type;

	public void SetType(VesselType t)
	{
		if (t != type)
		{
			type = t;
			if ((int)t > vesselTypeIcons.Length - 1)
			{
				image.sprite = vesselTypeIcons[2];
			}
			else
			{
				image.sprite = vesselTypeIcons[(int)t];
			}
		}
	}
}
