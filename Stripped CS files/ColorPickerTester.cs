using UnityEngine;

public class ColorPickerTester : MonoBehaviour
{
	public Renderer renderer;

	public ColorPicker picker;

	public void Start()
	{
		picker.onValueChanged.AddListener(delegate(Color color)
		{
			renderer.material.color = color;
		});
		picker.onInternalValueChanged.AddListener(delegate(Color color)
		{
			renderer.material.color = color;
		});
		renderer.material.color = picker.CurrentColor;
	}

	public void Update()
	{
	}
}
