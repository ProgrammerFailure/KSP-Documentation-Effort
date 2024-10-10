using System.Collections.Generic;
using ns12;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

public class KerbalListItem : MonoBehaviour
{
	public TextMeshProUGUI title;

	public Image border;

	public RawImage kerbalImage;

	public TooltipController_TitleAndText tooltip;

	public GameObject avatar;

	public RenderTexture avatarRT;

	public void Initialize(string headName, string headDesc, GameObject avatarPrefab)
	{
		title.text = headName;
		tooltip.titleString = headName.Replace('\n', ' ');
		tooltip.textString = headDesc;
		if (avatarPrefab != null)
		{
			avatar = Object.Instantiate(avatarPrefab);
			avatar.transform.SetParent(kerbalImage.transform);
			avatar.transform.localPosition = new Vector3(0f, 0f, 150f);
			List<Light> list = new List<Light>(avatar.GetComponentsInChildren<Light>());
			int count = list.Count;
			while (count-- > 0)
			{
				Object.Destroy(list[count]);
			}
			Camera componentInChildren = avatar.GetComponentInChildren<Camera>();
			if (componentInChildren != null)
			{
				avatarRT = new RenderTexture(256, 256, 8, RenderTextureFormat.Default);
				componentInChildren.aspect = 1f;
				componentInChildren.targetTexture = avatarRT;
				kerbalImage.texture = avatarRT;
			}
		}
		else
		{
			kerbalImage.material = null;
		}
	}

	public void OnDestroy()
	{
		if (avatar != null)
		{
			Object.Destroy(avatar);
		}
		if (avatarRT != null)
		{
			Object.Destroy(avatarRT);
		}
	}
}
