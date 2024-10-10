using System;
using UnityEngine;

[Serializable]
public class LightStruct
{
	[SerializeField]
	public string lightName;

	[SerializeField]
	public float lightR;

	[SerializeField]
	public float lightG;

	[SerializeField]
	public float lightB;

	[SerializeField]
	public string flareRendererName = "Flare";

	public Transform lightTransform;

	public Renderer flareRenderer;

	public Light light;

	public MeshRenderer lightMeshRenderer;

	public Color currentColor;

	public bool disableLightColorPicker;

	public LightStruct()
	{
	}

	public LightStruct(string lightName, float lightR, float lightG, float lightB, string flareRendererName)
		: this()
	{
		this.lightName = lightName;
		this.lightR = lightR;
		this.lightG = lightG;
		this.lightB = lightB;
		this.flareRendererName = flareRendererName;
	}

	public void Load(ConfigNode node)
	{
		lightName = node.GetValue("lightName");
		lightR = float.Parse(node.GetValue("lightR"));
		lightG = float.Parse(node.GetValue("lightG"));
		lightB = float.Parse(node.GetValue("lightB"));
		flareRendererName = node.GetValue("flareRendererName");
	}

	public void FindLightComponents(Part part, bool disableColorPicker)
	{
		lightTransform = part.FindModelTransform(lightName);
		light = part.FindModelComponent<Light>(lightName);
		flareRenderer = part.FindModelComponent<MeshRenderer>(flareRendererName);
		if (flareRenderer == null)
		{
			Transform transform = part.FindModelTransformByLayer("TransparentFX");
			if (transform != null)
			{
				flareRenderer = transform.GetComponent<MeshRenderer>();
			}
		}
		disableLightColorPicker = disableColorPicker;
	}

	public void FixedUpdateLightsStatus(bool isOn, float time, float fdt, float resourceFraction, float targetLight, float currentLight, float lightBrightenSpeed, float lightDimSpeed, bool castLight, Color lightColor)
	{
		if (isOn)
		{
			if (time / lightBrightenSpeed < 1f)
			{
				if (disableLightColorPicker)
				{
					currentColor = Color.Lerp(Color.black, new Color(lightR, lightG, lightB), time / lightBrightenSpeed);
				}
				else
				{
					currentColor = Color.Lerp(Color.black, lightColor, time / lightBrightenSpeed);
				}
				if (lightMeshRenderer != null)
				{
					lightMeshRenderer.material.SetColor("_EmissiveColor", currentColor);
				}
				if (flareRenderer != null)
				{
					flareRenderer.material.SetColor("_TintColor", Color.Lerp(Color.black, Color.white, time / lightBrightenSpeed));
				}
			}
			targetLight = 1f * resourceFraction;
			currentLight = ((!(currentLight < targetLight)) ? Mathf.Lerp(currentLight, targetLight, lightDimSpeed * fdt) : Mathf.Lerp(currentLight, targetLight, lightBrightenSpeed * fdt));
			light.enabled = castLight;
			light.intensity = currentLight;
			light.color = currentColor;
			return;
		}
		if (time / lightBrightenSpeed < 1f)
		{
			if (disableLightColorPicker)
			{
				currentColor = Color.Lerp(new Color(lightR, lightG, lightB), Color.black, time / lightBrightenSpeed);
			}
			else
			{
				currentColor = Color.Lerp(lightColor, Color.black, time / lightBrightenSpeed);
			}
			if (lightMeshRenderer != null)
			{
				lightMeshRenderer.material.SetColor("_EmissiveColor", currentColor);
			}
			if (flareRenderer != null)
			{
				flareRenderer.material.SetColor("_TintColor", Color.Lerp(Color.white, Color.black, time / lightBrightenSpeed));
			}
		}
		targetLight = 0f;
		currentLight = ((!(currentLight < targetLight)) ? Mathf.Lerp(currentLight, targetLight, lightDimSpeed * fdt) : Mathf.Lerp(currentLight, targetLight, lightBrightenSpeed * fdt));
		light.intensity = currentLight;
		light.color = currentColor;
	}

	public void UpdateLightStatusOnEditor(bool isOn, Color lightColor)
	{
		if (isOn)
		{
			if (lightMeshRenderer != null)
			{
				lightMeshRenderer.material.SetColor("_EmissiveColor", new Color(lightR, lightG, lightB));
			}
			if (flareRenderer != null)
			{
				flareRenderer.material.SetColor("_TintColor", Color.white);
			}
			if (disableLightColorPicker)
			{
				currentColor = new Color(lightR, lightG, lightB);
			}
			else
			{
				currentColor = lightColor;
			}
		}
		else
		{
			if (lightMeshRenderer != null)
			{
				lightMeshRenderer.material.SetColor("_EmissiveColor", Color.black);
			}
			if (flareRenderer != null)
			{
				flareRenderer.material.SetColor("_TintColor", Color.black);
			}
			currentColor = Color.black;
		}
		light.intensity = 1f;
		light.enabled = isOn;
		light.color = currentColor;
	}
}
