using UnityEngine;

namespace ns13;

public class PartIcon
{
	public GameObject icon;

	public Renderer[] renderers;

	public GameObject Icon
	{
		get
		{
			return icon;
		}
		set
		{
			icon = value;
			if (icon != null)
			{
				renderers = icon.GetComponentsInChildren<Renderer>();
			}
		}
	}

	public Renderer[] Renderers => renderers;

	public PartIcon(GameObject newPartIcon)
	{
		Icon = newPartIcon;
	}

	public void DestroyIcon()
	{
		if (icon != null)
		{
			icon.DestroyGameObject();
		}
		renderers = null;
	}
}
