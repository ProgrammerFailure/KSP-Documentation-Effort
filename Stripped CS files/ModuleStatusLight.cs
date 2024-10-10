using UnityEngine;

public class ModuleStatusLight : PartModule
{
	[KSPField]
	public string lightObjName = "";

	[KSPField]
	public string lightMeshRendererName = "";

	[KSPField]
	public string lightMatPropertyName = "TintColor";

	[KSPField]
	public string colorOn = "";

	[KSPField]
	public string colorOff = "";

	public Light lightObj;

	public Material lightMat;

	public Color cOn;

	public Color cOff;

	public bool IsOn { get; set; }

	public override void OnStart(StartState state)
	{
		if (!string.IsNullOrEmpty(lightObjName))
		{
			Light light = base.part.FindModelComponent<Light>(lightObjName);
			if (light != null)
			{
				lightObj = light;
			}
			else
			{
				Debug.LogError("[ModuleStatusLight]: No Light object called " + lightObjName + " found in " + base.part.partName + "!", base.gameObject);
			}
		}
		if (!string.IsNullOrEmpty(lightMeshRendererName))
		{
			MeshRenderer meshRenderer = base.part.FindModelComponent<MeshRenderer>(lightMeshRendererName);
			if (meshRenderer != null)
			{
				lightMat = meshRenderer.material;
			}
			else
			{
				Debug.LogError("[ModuleStatusLight]: No MeshRenderer object called " + lightMeshRendererName + " found in " + base.part.partName + "!", base.gameObject);
			}
		}
		if (!string.IsNullOrEmpty(colorOn) && colorOn.StartsWith("#"))
		{
			cOn = XKCDColors.ColorTranslator.FromHtml(colorOn);
		}
		if (!string.IsNullOrEmpty(colorOff) && colorOff.StartsWith("#"))
		{
			cOff = XKCDColors.ColorTranslator.FromHtml(colorOff);
		}
		SetStatus(status: false);
	}

	public override void OnInventoryModeDisable()
	{
		if (lightObj == null && !string.IsNullOrEmpty(lightObjName))
		{
			Light light = base.part.FindModelComponent<Light>(lightObjName);
			if (light != null)
			{
				lightObj = light;
			}
		}
		if (lightObj != null)
		{
			lightObj.enabled = false;
		}
	}

	public void SetStatus(bool status)
	{
		if (lightObj != null)
		{
			lightObj.color = (status ? cOn : cOff);
		}
		if (lightMat != null)
		{
			lightMat.SetColor(lightMatPropertyName, status ? cOn : cOff);
		}
		IsOn = status;
	}
}
