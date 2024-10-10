using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MEGUIParameterAwardModule : MEGUIParameterDynamicModule
{
	public Image awardIcon;

	public TextMeshProUGUI awardDescription;

	public Toggle moduleEnabledToggle;

	public TMP_InputField awardedPoints;

	public AwardModule awardModule;

	public bool isDirty;

	public override void Setup(string name)
	{
		base.Setup(name);
		moduleEnabledToggle.onValueChanged.AddListener(OnModuleEnabledToggleValueChange);
		awardedPoints.onValueChanged.AddListener(OnAwardedPointsValueChange);
		awardedPoints.onEndEdit.AddListener(OnAwardedPointsEndEdit);
		container.SetActive(value: false);
	}

	public override void Setup(string name, object value, Transform transform)
	{
		awardModule = value as AwardModule;
		if (awardModule != null && awardModule.Definition != null)
		{
			awardIcon.sprite = awardModule.Definition.icon;
			awardDescription.text = awardModule.Definition.description;
			if (moduleEnabledToggle != null)
			{
				moduleEnabledToggle.isOn = awardModule.enabled;
			}
			awardedPoints.text = awardModule.score.ToString();
		}
		base.Setup(name, value, transform);
		if (!(parentParameter != null) || !(parentParameter.FieldValue.node == null))
		{
			return;
		}
		Dictionary<string, MEGUIParameter>.KeyCollection.Enumerator enumerator = subParameters.Keys.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform parent = subParameters[enumerator.Current].pinToggle.transform.parent;
				if (parent != null)
				{
					parent.gameObject.SetActive(value: false);
				}
				Transform transform2 = parent.parent.Find("Separator");
				if (transform2 != null)
				{
					transform2.gameObject.SetActive(value: false);
				}
			}
		}
		finally
		{
			enumerator.Dispose();
		}
	}

	public void SetupAwardModule(AwardModule module)
	{
		Setup("", module, null);
		awardedPoints.onValueChanged.AddListener(OnAwardedPointsValueChange);
		awardedPoints.onEndEdit.AddListener(OnAwardedPointsEndEdit);
	}

	public void OnModuleEnabledToggleValueChange(bool status)
	{
		awardModule.enabled = status;
	}

	public void OnAwardedPointsValueChange(string value)
	{
		isDirty = true;
	}

	public void OnAwardedPointsEndEdit(string value)
	{
		if (isDirty)
		{
			float result = 0f;
			if (float.TryParse(value, out result))
			{
				awardModule.score = result;
			}
		}
	}
}
