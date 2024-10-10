using System;
using UnityEngine;

public class ModuleSeeThroughObject : PartModule
{
	[KSPField]
	public string transformName = "";

	[KSPField]
	public string shaderName = "KSP/Specular (Transparent)";

	[KSPField]
	public float screenRadius = 1f;

	[KSPField]
	public float proximityBias = 1.4f;

	[KSPField]
	public float minOpacity = 0.4f;

	[KSPField]
	public int leadModuleIndex = -1;

	[KSPField]
	public float leadModuleTgtValue = 1f;

	[KSPField]
	public float leadModuleTgtGain = 2f;

	[NonSerialized]
	public MeshRenderer[] mrs;

	[NonSerialized]
	public Transform trf;

	public bool setup;

	public float opacity;

	public float screenHeightRecip = 1f;

	[NonSerialized]
	public Shader seeThroughShader;

	[NonSerialized]
	public IScalarModule leadModule;

	public override void OnStart(StartState state)
	{
		setup = false;
		if (!HighLogic.LoadedSceneIsEditor)
		{
			return;
		}
		trf = base.part.FindModelTransform(transformName);
		if (trf == null)
		{
			Debug.LogError("[ModuleSeeThroughObject]: No Transform exists in part " + base.part.partName + " called " + transformName, base.gameObject);
			return;
		}
		mrs = trf.GetComponentsInChildren<MeshRenderer>(includeInactive: true);
		if (mrs == null)
		{
			Debug.LogError("[ModuleSeeThroughObject]: No MeshRenderer components found in " + transformName + " on part " + base.part.partName, base.gameObject);
			return;
		}
		if (leadModuleIndex != -1)
		{
			leadModule = base.part.Modules[leadModuleIndex] as IScalarModule;
			if (leadModule == null)
			{
				Debug.LogError("[ModuleSeeThroughObject]: Module at index " + leadModuleIndex + " is not an IScalarModule on part " + base.part.partName, base.gameObject);
				return;
			}
		}
		seeThroughShader = Shader.Find(shaderName);
		if (seeThroughShader != null)
		{
			int num = mrs.Length;
			while (num-- > 0)
			{
				mrs[num].material.shader = seeThroughShader;
			}
			opacity = 1f;
			if (leadModule != null)
			{
				SetOpacity(Mathf.Clamp01(opacity + Mathf.Abs(leadModuleTgtValue - leadModule.GetScalar) * leadModuleTgtGain));
			}
			else
			{
				SetOpacity(opacity);
			}
			screenHeightRecip = 1f / (float)Screen.height;
			setup = true;
		}
		else
		{
			Debug.LogError("[ModuleSeeThroughObject]: No shader called " + shaderName + " found for part " + base.part.partName + ".", base.gameObject);
		}
	}

	public void LateUpdate()
	{
		if (!(EditorLogic.fetch == null) && setup && EditorLogic.fetch.ship.Contains(base.part))
		{
			MouseFadeUpdate();
		}
	}

	public void MouseFadeUpdate()
	{
		float cursorProximity = GetCursorProximity(Input.mousePosition, screenRadius, trf, Camera.main);
		cursorProximity = Mathf.Pow(Mathf.Clamp01(cursorProximity), proximityBias);
		opacity = Mathf.Max(1f - cursorProximity, minOpacity);
		if (leadModule != null)
		{
			SetOpacity(Mathf.Clamp01(opacity + Mathf.Abs(leadModuleTgtValue - leadModule.GetScalar) * leadModuleTgtGain));
		}
		else
		{
			SetOpacity(opacity);
		}
	}

	public float GetCursorProximity(Vector3 cursorPosition, float range, Transform trf, Camera referenceCamera)
	{
		float num = Mathf.Tan(referenceCamera.fieldOfView * 0.5f * ((float)Math.PI / 180f)) * (base.part.partTransform.transform.position - referenceCamera.transform.position).sqrMagnitude;
		float num2 = range * range / num;
		cursorPosition *= screenHeightRecip;
		Vector3 vector = referenceCamera.WorldToScreenPoint(trf.position) * screenHeightRecip;
		Vector3 vector2 = cursorPosition - vector;
		float sqrMagnitude = Vector3.ProjectOnPlane(vector2, Vector3.forward).sqrMagnitude;
		return Mathf.Clamp01(1f - sqrMagnitude / num2);
	}

	public void SetOpacity(float o)
	{
		opacity = o;
		int num = mrs.Length;
		while (num-- > 0)
		{
			mrs[num].material.SetFloat(PropertyIDs._Opacity, o);
		}
	}
}
