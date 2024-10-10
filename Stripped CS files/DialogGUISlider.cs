using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogGUISlider : DialogGUIBase
{
	public float min;

	public float max;

	public bool wholeNumbers = true;

	public UnityAction<float> actionCallback = delegate
	{
	};

	public Func<float> setValue = () => 0f;

	public Slider slider;

	public DialogGUISlider(Func<float> setValue, float min, float max, bool wholeNumbers, float width, float height, UnityAction<float> setCallback)
	{
		actionCallback = setCallback;
		this.min = min;
		this.max = max;
		this.wholeNumbers = wholeNumbers;
		this.setValue = setValue;
		size = new Vector2(width, height);
	}

	public override GameObject Create(ref Stack<Transform> layouts, UISkinDef skin)
	{
		uiItem = UnityEngine.Object.Instantiate(UISkinManager.GetPrefab("UISliderPrefab"));
		uiItem.SetActive(value: true);
		uiItem.transform.SetParent(layouts.Peek(), worldPositionStays: false);
		SetupTransformAndLayout();
		slider = uiItem.GetComponent<Slider>();
		slider.direction = Slider.Direction.LeftToRight;
		slider.maxValue = max;
		slider.minValue = min;
		slider.wholeNumbers = wholeNumbers;
		slider.value = setValue();
		slider.onValueChanged.AddListener(actionCallback);
		uiItem.GetChild("Background").GetComponent<Image>().sprite = skin.horizontalSlider.normal.background;
		slider.targetGraphic.GetComponent<Image>().sprite = skin.horizontalSliderThumb.normal.background;
		return base.Create(ref layouts, skin);
	}

	public override void Update()
	{
		base.Update();
		if (slider != null)
		{
			slider.value = setValue();
		}
	}
}
