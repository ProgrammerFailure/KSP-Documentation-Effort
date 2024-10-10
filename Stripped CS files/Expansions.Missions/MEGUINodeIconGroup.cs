using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions;

public class MEGUINodeIconGroup : MonoBehaviour
{
	public new string name;

	public TextMeshProUGUI title;

	public RectTransform containerChildren;

	public Button buttonCollapse;

	public Sprite spriteCollapseOn;

	public Sprite spriteCollapseOff;

	[SerializeField]
	public Image headerImage;

	public void Awake()
	{
		title.text = name;
		buttonCollapse.onClick.AddListener(OnButtonCollapsePressed);
	}

	public void OnDestroy()
	{
		buttonCollapse.onClick.RemoveListener(OnButtonCollapsePressed);
	}

	public static MEGUINodeIconGroup Create(string name, string displayName, string htmlColorCode = "")
	{
		try
		{
			MEGUINodeIconGroup component = UnityEngine.Object.Instantiate(MissionsUtils.MEPrefab("_UI5/Screens/MissionEditorScreen/Prefabs/MEGUINodeIconGroup.prefab")).GetComponent<MEGUINodeIconGroup>();
			component.name = name;
			component.title.text = displayName;
			if (!string.IsNullOrEmpty(htmlColorCode))
			{
				component.SetHeaderColor(htmlColorCode);
			}
			return component;
		}
		catch (Exception ex)
		{
			Debug.LogWarningFormat("[MEGUINodeIconGroup]: Failed to create MEGUINodeIconGroup {0}. Error - {1}", name, ex.Message);
		}
		return null;
	}

	public void CollapseGroup()
	{
		containerChildren.gameObject.SetActive(value: false);
		((Image)buttonCollapse.targetGraphic).sprite = spriteCollapseOn;
	}

	public void OnButtonCollapsePressed()
	{
		containerChildren.gameObject.SetActive(!containerChildren.gameObject.activeSelf);
		((Image)buttonCollapse.targetGraphic).sprite = (containerChildren.gameObject.activeSelf ? spriteCollapseOff : spriteCollapseOn);
	}

	public void SetHeaderColor(Color newColor)
	{
		if (headerImage != null)
		{
			headerImage.color = newColor;
		}
	}

	public void SetHeaderColor(string htmlColorCode)
	{
		try
		{
			ColorUtility.TryParseHtmlString(htmlColorCode, out var color);
			SetHeaderColor(color);
		}
		catch (Exception)
		{
			Debug.LogErrorFormat("Unable to parse color for {0}", name);
		}
	}
}
