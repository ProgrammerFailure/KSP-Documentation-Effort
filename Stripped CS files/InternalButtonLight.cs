using UnityEngine;

public class InternalButtonLight : InternalModule
{
	[KSPField]
	public string buttonName = "button";

	[KSPField]
	public bool defaultValue;

	[KSPField]
	public string lightName = "light";

	[KSPField]
	public Color lightColor = Color.white;

	[KSPField]
	public float lightIntensityOn = 1f;

	[KSPField]
	public float lightIntensityOff;

	[KSPField]
	public bool useButtonColor;

	[KSPField]
	public Color buttonColorOn = Color.green;

	[KSPField]
	public Color buttonColorOff = Color.black;

	public Collider buttonObject;

	public Light[] lightObjects;

	public bool lightsOn;

	public override void OnAwake()
	{
		lightsOn = defaultValue;
		if (buttonObject == null)
		{
			buttonObject = internalProp.FindModelTransform(buttonName).GetComponent<Collider>();
		}
		if (lightObjects == null)
		{
			lightObjects = internalProp.FindModelComponents<Light>(lightName);
			Light[] array = lightObjects;
			foreach (Light obj in array)
			{
				obj.color = lightColor;
				obj.enabled = lightsOn;
			}
		}
		if (buttonObject != null && lightObjects != null)
		{
			InternalButton.Create(buttonObject.gameObject).OnTap(Button_OnTap);
		}
	}

	public void Button_OnTap()
	{
		SetLight(!lightsOn);
	}

	public void SetLight(bool on)
	{
		lightsOn = on;
		if (lightsOn)
		{
			int i = 0;
			for (int num = lightObjects.Length; i < num; i++)
			{
				Light obj = lightObjects[i];
				obj.enabled = true;
				obj.intensity = lightIntensityOn;
			}
			if (useButtonColor)
			{
				GetComponent<Renderer>().material.SetColor(PropertyIDs._EmissiveColor, buttonColorOn);
			}
			return;
		}
		if (lightIntensityOff <= 0f)
		{
			int j = 0;
			for (int num2 = lightObjects.Length; j < num2; j++)
			{
				lightObjects[j].enabled = false;
			}
		}
		else
		{
			int k = 0;
			for (int num3 = lightObjects.Length; k < num3; k++)
			{
				Light obj2 = lightObjects[k];
				obj2.enabled = true;
				obj2.intensity = lightIntensityOff;
			}
		}
		if (useButtonColor)
		{
			GetComponent<Renderer>().material.SetColor(PropertyIDs._EmissiveColor, buttonColorOff);
		}
	}
}
