using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogGUIRadialLayout : DialogGUILayoutBase
{
	public float radius;

	public float increment;

	public float startangle;

	public Image[] backgrounds;

	public int selectedIndex;

	public static float minMag = 0.3f;

	public DialogGUIRadialLayout(float radius, float increment, float startangle, params DialogGUIBase[] items)
		: base(items)
	{
		this.radius = radius;
		if (increment == -1f)
		{
			this.increment = 360f / (float)items.Length;
		}
		else
		{
			this.increment = increment;
		}
		this.startangle = startangle;
	}

	public override void Resize()
	{
		base.Resize();
		float num = startangle;
		Vector2 vector = Vector2.up * radius;
		foreach (DialogGUIBase child in children)
		{
			child.uiItem.transform.localPosition = vector;
			Quaternion localRotation = child.uiItem.transform.localRotation;
			child.uiItem.transform.RotateAround(uiItem.transform.position, Vector3.back, num);
			child.uiItem.transform.localRotation = localRotation;
			num += increment;
		}
	}

	public override GameObject Create(ref Stack<Transform> layouts, UISkinDef skin)
	{
		uiItem = new GameObject("RadialLayout");
		RectTransform rectTransform = uiItem.AddComponent<RectTransform>();
		uiItem.SetActive(value: true);
		uiItem.transform.SetParent(layouts.Peek(), worldPositionStays: false);
		rectTransform.localPosition = Vector3.zero;
		rectTransform.localScale = Vector3.one;
		rectTransform.sizeDelta = new Vector2(radius * 2f + 18f, radius * 2f + 18f);
		backgrounds = new Image[children.Count];
		for (int i = 0; i < children.Count; i++)
		{
			GameObject gameObject = new GameObject("Background");
			gameObject.transform.SetParent(uiItem.transform, worldPositionStays: false);
			rectTransform = gameObject.GetComponent<RectTransform>();
			if (rectTransform == null)
			{
				rectTransform = gameObject.AddComponent<RectTransform>();
			}
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.localPosition = Vector3.zero;
			rectTransform.localScale = Vector3.one;
			rectTransform.sizeDelta = Vector2.zero;
			rectTransform.Rotate(Vector3.back, startangle + increment * (float)i - increment * 0.5f - 0.25f);
			Image image = gameObject.AddComponent<Image>();
			image.sprite = skin.window.normal.background;
			image.type = Image.Type.Filled;
			image.fillMethod = Image.FillMethod.Radial360;
			image.fillAmount = increment / 360f + 0.0013888889f;
			image.fillCenter = true;
			image.fillOrigin = 2;
			image.color = new Color(16f / 85f, 16f / 85f, 16f / 85f, 1f);
			backgrounds[i] = image;
		}
		return base.Create(ref layouts, skin);
	}

	public int SelectedItemAtAngle(float angle)
	{
		int num = -1;
		if (startangle <= angle && startangle + increment * (float)children.Count >= angle)
		{
			num = Mathf.FloorToInt((angle - startangle) / increment);
			if (num >= backgrounds.Length)
			{
				num = 0;
			}
			if (num < 0)
			{
				num = backgrounds.Length - 1;
			}
			backgrounds[selectedIndex].color = new Color(16f / 85f, 16f / 85f, 16f / 85f, 1f);
			selectedIndex = num;
			backgrounds[selectedIndex].color = XKCDColors.Blue;
			EventSystem.current.SetSelectedGameObject(children[num].uiItem);
		}
		return num;
	}

	public override void Update()
	{
		base.Update();
		if (!OptionInteractableCondition() || !OptionEnabledCondition())
		{
			return;
		}
		Vector2 vector = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
		if (vector.magnitude > minMag)
		{
			float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f + 90f;
			if (num < 0f)
			{
				num = 360f + num;
			}
			SelectedItemAtAngle(num);
		}
	}
}
