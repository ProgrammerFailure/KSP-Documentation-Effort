using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns2;

public class UIHoverPanel : PointerEnterExitHandler
{
	public Image backgroundImage;

	public Sprite backgroundNormal;

	public Sprite backgroundHover;

	public List<GameObject> normalObjects = new List<GameObject>();

	public List<GameObject> hoverObjects = new List<GameObject>();

	public bool hoverEnabled = true;

	public void Awake()
	{
		OnPointerExit(null);
	}

	public void DisableHover()
	{
		hoverEnabled = false;
		PointerExit();
	}

	public void EnableHover()
	{
		hoverEnabled = true;
	}

	public override void OnPointerEnter(PointerEventData data)
	{
		if (hoverEnabled)
		{
			PointerEnter();
			base.OnPointerEnter(data);
		}
	}

	public void PointerEnter()
	{
		backgroundImage.sprite = backgroundHover;
		SetStateObjects(normalObjects, active: false);
		SetStateObjects(hoverObjects, active: true);
	}

	public override void OnPointerExit(PointerEventData data)
	{
		if (hoverEnabled)
		{
			PointerExit();
			base.OnPointerExit(data);
		}
	}

	public void PointerExit()
	{
		backgroundImage.sprite = backgroundNormal;
		SetStateObjects(normalObjects, active: true);
		SetStateObjects(hoverObjects, active: false);
	}

	public void SetStateObjects(List<GameObject> objects, bool active)
	{
		int count = objects.Count;
		while (count-- > 0)
		{
			objects[count].SetActive(active);
		}
	}
}
