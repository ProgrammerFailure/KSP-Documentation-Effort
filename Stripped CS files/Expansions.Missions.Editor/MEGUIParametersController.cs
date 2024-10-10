using System;
using System.Collections.Generic;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class MEGUIParametersController : MonoBehaviour
{
	public GameObject defaultCompoundParameter;

	public List<MEGUIParameter> parameterPrefabs;

	public Dictionary<Type, MEGUIParameter> fieldControlTypes;

	public static MEGUIParametersController Instance { get; set; }

	public void Awake()
	{
		if (Instance != null)
		{
			Debug.Log("[MissionSystem]: Can only have one MEGUIParametersController in the scene!");
			UnityEngine.Object.DestroyImmediate(base.gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	public void Start()
	{
		SetupParameterControls();
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void SetupParameterControls()
	{
		fieldControlTypes = new Dictionary<Type, MEGUIParameter>();
		int count = parameterPrefabs.Count;
		while (count-- > 0)
		{
			MEGUIParameter mEGUIParameter = parameterPrefabs[count];
			if (!(mEGUIParameter != null))
			{
				continue;
			}
			MEGUI_Control[] array = (MEGUI_Control[])mEGUIParameter.GetType().GetCustomAttributes(typeof(MEGUI_Control), inherit: false);
			if (array != null && array.Length != 0)
			{
				Type type = array[0].GetType();
				if (fieldControlTypes.ContainsKey(type))
				{
					Debug.LogError("[MissionSystem]: ParameterPrefab '" + mEGUIParameter.name + "' has same MEGUI_Control type as '" + fieldControlTypes[type].name + "'");
					parameterPrefabs.RemoveAt(count);
				}
				else
				{
					fieldControlTypes.Add(type, parameterPrefabs[count]);
				}
			}
			else
			{
				Debug.LogError("[MissionSystem]: ParameterPrefab '" + mEGUIParameter.name + "' has no MEGUI_Control defined");
				parameterPrefabs.RemoveAt(count);
			}
		}
	}

	public MEGUIParameter GetControl(Type uiControlType)
	{
		MEGUIParameter result = null;
		if (fieldControlTypes.ContainsKey(uiControlType))
		{
			result = fieldControlTypes[uiControlType];
		}
		else
		{
			Debug.LogError("[MissionSystem]: ParameterPrefab for control type '" + uiControlType.Name + "' not found.");
		}
		return result;
	}
}
