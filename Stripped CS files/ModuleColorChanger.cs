using System.Collections.Generic;
using ns9;
using UnityEngine;

public class ModuleColorChanger : PartModule, IScalarModule
{
	[KSPField]
	public string moduleID = "colorChanger";

	[KSPField]
	public string shaderProperty = "";

	public int shaderPropertyInt;

	[KSPField]
	public FloatCurve redCurve;

	[KSPField]
	public FloatCurve greenCurve;

	[KSPField]
	public FloatCurve blueCurve;

	[KSPField]
	public FloatCurve alphaCurve;

	[KSPField]
	public float animRate = 1f;

	[KSPField(isPersistant = true)]
	public bool animState;

	[KSPField]
	public bool useRate = true;

	[KSPField]
	public bool toggleInEditor = true;

	[KSPField]
	public bool toggleInFlight = true;

	[KSPField]
	public bool toggleUnfocused;

	[KSPField]
	public bool toggleAction;

	[KSPField]
	public float unfocusedRange = 5f;

	[KSPField]
	public string toggleName = "Toggle Color";

	[KSPField]
	public string eventOnName = string.Empty;

	[KSPField]
	public string eventOffName = string.Empty;

	[KSPField]
	public KSPActionGroup defaultActionGroup;

	[KSPField]
	public bool useMaterialsList;

	[KSPField]
	public string materialsNames = "";

	[KSPField]
	public bool alphaOnly;

	[KSPField]
	public float redColor;

	[KSPField]
	public float blueColor;

	[KSPField]
	public float greenColor;

	public List<string> includedRenderers = new List<string>();

	public List<string> excludedRenderers = new List<string>();

	public float currentRateState;

	public Color color;

	public List<Renderer> renderers;

	public bool isValid;

	public EventData<float, float> OnMove = new EventData<float, float>("OnMove");

	public EventData<float> OnStopped = new EventData<float>("OnStop");

	public static string cacheAutoLOC_6003030;

	public float CurrentRateState => currentRateState;

	public string ScalarModuleID => moduleID;

	public float GetScalar
	{
		get
		{
			if (!useRate)
			{
				if (!animState)
				{
					return 0f;
				}
				return 1f;
			}
			return currentRateState;
		}
	}

	public bool CanMove => true;

	public EventData<float, float> OnMoving => OnMove;

	public EventData<float> OnStop => OnStopped;

	[KSPEvent(unfocusedRange = 5f, guiActiveUnfocused = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001829")]
	public virtual void ToggleEvent()
	{
		animState = !animState;
		if (!useRate)
		{
			SetState(animState);
		}
		SetEventName();
	}

	[KSPAction("Toggle Color", KSPActionGroup.REPLACEWITHDEFAULT)]
	public virtual void ToggleAction(KSPActionParam param)
	{
		if (param.type != 0 && (param.type != KSPActionType.Toggle || animState))
		{
			animState = true;
		}
		else
		{
			animState = false;
		}
		ToggleEvent();
	}

	public override void OnAwake()
	{
		base.OnAwake();
		BaseAction baseAction = base.Actions["ToggleAction"];
		if (baseAction.actionGroup == KSPActionGroup.REPLACEWITHDEFAULT)
		{
			baseAction.actionGroup = defaultActionGroup;
		}
		if (baseAction.defaultActionGroup == KSPActionGroup.REPLACEWITHDEFAULT)
		{
			baseAction.defaultActionGroup = defaultActionGroup;
		}
		isValid = false;
		if (redCurve == null)
		{
			redCurve = new FloatCurve();
		}
		if (greenCurve == null)
		{
			greenCurve = new FloatCurve();
		}
		if (blueCurve == null)
		{
			blueCurve = new FloatCurve();
		}
		if (alphaCurve == null)
		{
			alphaCurve = new FloatCurve();
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("includedRenderer"))
		{
			includedRenderers.AddRange(node.GetValues("includedRenderer"));
		}
		if (node.HasValue("excludedRenderer"))
		{
			excludedRenderers.AddRange(node.GetValues("excludedRenderer"));
		}
		if (excludedRenderers.Count > 0 && includedRenderers.Count > 0)
		{
			Debug.Log("[ModuleColorChanger] You can not have both includedRenderer and excludedRenderer. Using excludedRenderer only");
			includedRenderers.Clear();
		}
	}

	public void EditRenderers()
	{
		if (excludedRenderers.Count > 0)
		{
			int count = renderers.Count;
			while (count-- > 0)
			{
				Renderer renderer = renderers[count];
				if (excludedRenderers.Contains(renderer.name))
				{
					renderers.RemoveAt(count);
				}
			}
		}
		if (includedRenderers.Count <= 0)
		{
			return;
		}
		int count2 = renderers.Count;
		while (count2-- > 0)
		{
			Renderer renderer2 = renderers[count2];
			if (!includedRenderers.Contains(renderer2.name))
			{
				renderers.RemoveAt(count2);
			}
		}
	}

	public void ProcessMaterialsList()
	{
		string[] array = materialsNames.Split(',');
		if (array.Length == 0)
		{
			return;
		}
		int count = renderers.Count;
		while (count-- > 0)
		{
			Material[] materials = renderers[count].materials;
			bool flag = false;
			for (int i = 0; i < materials.Length; i++)
			{
				for (int j = 0; j < array.Length; j++)
				{
					if (materials[i].name.Contains(array[j]))
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				renderers.RemoveAt(count);
			}
		}
	}

	public override void OnStart(StartState state)
	{
		BaseEvent baseEvent = base.Events["ToggleEvent"];
		BaseAction baseAction = base.Actions["ToggleAction"];
		baseEvent.guiActive = toggleInFlight;
		baseEvent.guiActiveEditor = toggleInEditor;
		baseEvent.guiActiveUnfocused = toggleUnfocused;
		baseEvent.unfocusedRange = unfocusedRange;
		string text2 = (baseEvent.guiName = toggleName);
		baseAction.guiName = text2;
		baseAction.active = toggleAction;
	}

	public virtual void Start()
	{
		isValid = true;
		if (string.IsNullOrEmpty(shaderProperty))
		{
			isValid = false;
		}
		else
		{
			shaderPropertyInt = Shader.PropertyToID(shaderProperty);
		}
		currentRateState = (animState ? 1f : 0f);
		renderers = base.part.FindModelComponents<Renderer>();
		if (renderers.Count == 0)
		{
			isValid = false;
		}
		else
		{
			if (useMaterialsList)
			{
				ProcessMaterialsList();
			}
			EditRenderers();
		}
		SetEventName();
	}

	public virtual void FixedUpdate()
	{
		if (!useRate || !isValid)
		{
			return;
		}
		if (animState)
		{
			if (currentRateState < 1f)
			{
				currentRateState += animRate * TimeWarp.fixedDeltaTime;
			}
			if (currentRateState > 1f)
			{
				currentRateState = 1f;
			}
		}
		else
		{
			if (currentRateState > 0f)
			{
				currentRateState -= animRate * TimeWarp.fixedDeltaTime;
			}
			if (currentRateState < 0f)
			{
				currentRateState = 0f;
			}
		}
	}

	public virtual void Update()
	{
		if ((!HighLogic.LoadedSceneIsEditor || !base.part.frozen) && useRate && isValid)
		{
			SetState(currentRateState);
		}
	}

	public void SetState(bool val)
	{
		currentRateState = (val ? 1f : 0f);
		SetState(currentRateState);
	}

	public void SetState(float val)
	{
		if (isValid)
		{
			if (alphaOnly)
			{
				color.r = redColor;
				color.g = greenColor;
				color.b = blueColor;
			}
			else
			{
				color.r = redCurve.Evaluate(val);
				color.g = greenCurve.Evaluate(val);
				color.b = blueCurve.Evaluate(val);
			}
			color.a = alphaCurve.Evaluate(val);
			UpdateColor(base.part.mpb);
		}
	}

	public void SetState(Color val)
	{
		if (isValid)
		{
			color = val;
			UpdateColor(base.part.mpb);
		}
	}

	public void UpdateColor(MaterialPropertyBlock mpb)
	{
		mpb.SetColor(shaderPropertyInt, color);
		int count = renderers.Count;
		while (count-- > 0)
		{
			if (renderers[count] == null)
			{
				renderers.RemoveAt(count);
			}
			else
			{
				renderers[count].SetPropertyBlock(mpb);
			}
		}
	}

	public void SetEventName()
	{
		if (animState)
		{
			if (!string.IsNullOrEmpty(eventOffName))
			{
				base.Events["ToggleEvent"].guiName = eventOffName;
			}
		}
		else if (!string.IsNullOrEmpty(eventOnName))
		{
			base.Events["ToggleEvent"].guiName = eventOnName;
		}
	}

	public void SetScalar(float t)
	{
		if (!useRate)
		{
			t = Mathf.Clamp01(t);
			currentRateState = 0f;
			animState = false;
		}
		else if (t >= 0.5f)
		{
			animState = true;
		}
		else
		{
			animState = false;
		}
		SetState(t);
		SetEventName();
	}

	public void SetUIRead(bool state)
	{
	}

	public void SetUIWrite(bool state)
	{
		BaseEvent baseEvent = base.Events["ToggleEvent"];
		BaseAction baseAction = base.Actions["ToggleAction"];
		baseEvent.guiActive = toggleInFlight && state;
		baseEvent.guiActiveEditor = toggleInEditor && state;
		baseEvent.guiActiveUnfocused = toggleUnfocused && state;
		baseAction.active = toggleAction && state;
	}

	public bool IsMoving()
	{
		if (useRate && currentRateState != 0f)
		{
			return currentRateState != 1f;
		}
		return false;
	}

	public override string GetModuleDisplayName()
	{
		return cacheAutoLOC_6003030;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_6003030 = Localizer.Format("#autoLoc_6003030");
	}
}
