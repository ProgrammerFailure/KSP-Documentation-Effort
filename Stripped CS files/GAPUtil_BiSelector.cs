using Expansions.Missions;
using Expansions.Missions.Editor;
using ns11;
using ns12;
using ns5;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GAPUtil_BiSelector : MonoBehaviour
{
	public Button leftButton;

	public Button rightButton;

	public TextMeshProUGUI titleText;

	public TextMeshProUGUI footerText;

	public GameObject containerSelector;

	public GameObject containerLeftButtons;

	public GameObject containerFooter;

	public Transform sideBarTransform;

	public Sprite sideBarDividerSprite;

	public Sprite sideBarDividerEndSprite;

	public DictionaryValueList<string, TrackingStationObjectButton> sideBarControls;

	public DictionaryValueList<string, MEPartCategoryButton> sideBarGAPControls;

	public Image lastDivider;

	public void SetTitleText(string newText)
	{
		titleText.text = newText;
	}

	public void SetFooterText(string newText)
	{
		footerText.text = newText;
	}

	public virtual void Awake()
	{
		RectTransform component = GetComponent<RectTransform>();
		component.localPosition = Vector3.zero;
		component.localScale = Vector3.one;
		component.offsetMin = new Vector2(0f, 0f);
		component.offsetMax = new Vector2(0f, 0f);
		sideBarControls = new DictionaryValueList<string, TrackingStationObjectButton>();
		sideBarGAPControls = new DictionaryValueList<string, MEPartCategoryButton>();
	}

	public TrackingStationObjectButton AddSidebarButton(string id, string icon, string toolTip, bool startState = false, int count = 0)
	{
		if (!sideBarControls.ContainsKey(id))
		{
			TrackingStationObjectButton component = Object.Instantiate(MissionsUtils.MEPrefab("Prefabs/GAPSidebarButton.prefab"), sideBarTransform).GetComponent<TrackingStationObjectButton>();
			Icon icon2 = MissionEditorLogic.Instance.actionPane.gapIconLoader.GetIcon(icon);
			if (icon2.simple)
			{
				component.spriteTrue = Sprite.Create(icon2.iconNormal as Texture2D, new Rect(0f, 0f, icon2.iconNormal.width, icon2.iconNormal.height), new Vector2(0.5f, 0.5f));
				component.spriteFalse = Sprite.Create(icon2.iconNormal as Texture2D, new Rect(0f, 0f, icon2.iconNormal.width, icon2.iconNormal.height), new Vector2(0.5f, 0.5f));
			}
			else
			{
				component.spriteTrue = Sprite.Create(icon2.iconSelected as Texture2D, new Rect(0f, 0f, icon2.iconSelected.width, icon2.iconSelected.height), new Vector2(0.5f, 0.5f));
				component.spriteFalse = Sprite.Create(icon2.iconNormal as Texture2D, new Rect(0f, 0f, icon2.iconNormal.width, icon2.iconNormal.height), new Vector2(0.5f, 0.5f));
			}
			component.GetComponent<TooltipController_Text>().textString = toolTip;
			component.SetState(startState);
			component.SetCount(count);
			sideBarControls.Add(id, component);
			AddDivider();
			return component;
		}
		return sideBarControls[id];
	}

	public MEPartCategoryButton AddSidebarGAPButton(string id, string icon, string toolTip, bool startState = false, int count = 0)
	{
		if (!sideBarGAPControls.ContainsKey(id))
		{
			MEPartCategoryButton component = Object.Instantiate(MissionsUtils.MEPrefab("Prefabs/GAPScrollableSidebarButton.prefab"), sideBarTransform).GetComponent<MEPartCategoryButton>();
			Icon icon2 = MissionEditorLogic.Instance.actionPane.gapIconLoader.GetIcon(icon);
			if (icon2.simple)
			{
				component.spriteTrue = Sprite.Create(icon2.iconNormal as Texture2D, new Rect(0f, 0f, icon2.iconNormal.width, icon2.iconNormal.height), new Vector2(0.5f, 0.5f));
				component.spriteFalse = Sprite.Create(icon2.iconNormal as Texture2D, new Rect(0f, 0f, icon2.iconNormal.width, icon2.iconNormal.height), new Vector2(0.5f, 0.5f));
			}
			else
			{
				component.spriteTrue = Sprite.Create(icon2.iconSelected as Texture2D, new Rect(0f, 0f, icon2.iconSelected.width, icon2.iconSelected.height), new Vector2(0.5f, 0.5f));
				component.spriteFalse = Sprite.Create(icon2.iconNormal as Texture2D, new Rect(0f, 0f, icon2.iconNormal.width, icon2.iconNormal.height), new Vector2(0.5f, 0.5f));
			}
			component.GetComponent<TooltipController_Text>().textString = toolTip;
			component.SetState(startState);
			component.SetCount(count);
			sideBarGAPControls.Add(id, component);
			AddDivider();
			return component;
		}
		return sideBarGAPControls[id];
	}

	public TrackingStationObjectButton GetSidebarButton(string id)
	{
		if (sideBarControls.ContainsKey(id))
		{
			return sideBarControls[id];
		}
		return null;
	}

	public MEPartCategoryButton GetSidebarGapButton(string id)
	{
		if (sideBarGAPControls.ContainsKey(id))
		{
			return sideBarGAPControls[id];
		}
		return null;
	}

	public void ClearSidebar()
	{
		int childCount = sideBarTransform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Object.Destroy(sideBarTransform.GetChild(i).gameObject);
		}
		lastDivider = null;
		sideBarControls.Clear();
		sideBarGAPControls.Clear();
	}

	public void AddDivider()
	{
		if (lastDivider != null)
		{
			lastDivider.sprite = sideBarDividerSprite;
		}
		Image component = new GameObject("divider", typeof(Image)).GetComponent<Image>();
		component.transform.SetParent(sideBarTransform, worldPositionStays: false);
		component.sprite = sideBarDividerEndSprite;
		lastDivider = component;
	}
}
