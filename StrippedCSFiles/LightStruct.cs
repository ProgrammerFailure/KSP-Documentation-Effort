using System;
using System.Runtime.CompilerServices;
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
	public string flareRendererName;

	public Transform lightTransform;

	private Renderer flareRenderer;

	private Light light;

	private MeshRenderer lightMeshRenderer;

	private Color currentColor;

	private bool disableLightColorPicker;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LightStruct()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LightStruct(string lightName, float lightR, float lightG, float lightB, string flareRendererName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FindLightComponents(Part part, bool disableColorPicker)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FixedUpdateLightsStatus(bool isOn, float time, float fdt, float resourceFraction, float targetLight, float currentLight, float lightBrightenSpeed, float lightDimSpeed, bool castLight, Color lightColor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateLightStatusOnEditor(bool isOn, Color lightColor)
	{
		throw null;
	}
}
