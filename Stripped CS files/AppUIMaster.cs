using System;
using System.Collections.Generic;
using System.Reflection;
using ns2;
using UnityEngine;

public class AppUIMaster : MonoBehaviour
{
	public List<GameObject> controlPrefabs;

	public Dictionary<Type, GameObject> controlPrefabByType;

	public static AppUIMaster Instance { get; set; }

	public void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Debug.LogError("[AppUIMaster]: Instance already exists!", Instance.gameObject);
			UnityEngine.Object.Destroy(Instance);
		}
		Instance = this;
		controlPrefabByType = new Dictionary<Type, GameObject>();
	}

	public void Start()
	{
		SetupRowPrefabByType();
	}

	public void Update()
	{
	}

	public void SetupRowPrefabByType()
	{
		controlPrefabByType = new Dictionary<Type, GameObject>();
		controlPrefabByType = new Dictionary<Type, GameObject>();
		Type[] types = Assembly.GetExecutingAssembly().GetTypes();
		foreach (Type type in types)
		{
			if (!type.IsSubclassOf(typeof(AppUI_Control)))
			{
				continue;
			}
			bool flag = false;
			foreach (GameObject controlPrefab in controlPrefabs)
			{
				MonoBehaviour[] components = controlPrefab.GetComponents<MonoBehaviour>();
				for (int j = 0; j < components.Length; j++)
				{
					foreach (Attribute customAttribute in components[j].GetType().GetCustomAttributes())
					{
						if (customAttribute.GetType() == type)
						{
							controlPrefabByType.Add(type, controlPrefab);
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			if (!flag)
			{
				Debug.LogError("[AppUIMaster] Control Type found, but no prefab found in the list. Type = " + type.Name);
			}
		}
	}

	public static bool TryGetAppUIControlPrefab(AppUI_Control controlAttrib, out GameObject prefab)
	{
		if (Instance == null)
		{
			prefab = null;
			return false;
		}
		return Instance.controlPrefabByType.TryGetValue(controlAttrib.GetType(), out prefab);
	}
}
