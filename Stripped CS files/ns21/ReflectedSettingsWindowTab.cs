using System.Collections.Generic;
using ns2;
using UnityEngine;

namespace ns21;

public class ReflectedSettingsWindowTab : SettingsControlBase
{
	public UIStateButton tabStateButton;

	public RectTransform layoutGroup;

	public List<GameObject> spawnedObjects = new List<GameObject>();

	public List<GameObject> SpawnedObjects => spawnedObjects;

	public void Start()
	{
		if (tabStateButton != null)
		{
			tabStateButton.onValueChanged.AddListener(ShowTab);
		}
		OnStart();
	}

	public virtual void OnStart()
	{
	}

	public void ShowTab(UIStateButton btn)
	{
		ShowTab(btn.currentState == "Active");
	}

	public void ShowTab(bool state)
	{
		int i = 0;
		for (int count = spawnedObjects.Count; i < count; i++)
		{
			spawnedObjects[i].gameObject.SetActive(state);
		}
	}
}
