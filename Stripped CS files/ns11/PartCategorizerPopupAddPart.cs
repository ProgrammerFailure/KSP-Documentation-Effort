using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns11;

public class PartCategorizerPopupAddPart : MonoBehaviour
{
	public delegate bool CriteriaAccept(string partName, PartCategorizer.Category category, out string reason);

	public class MiniCategory
	{
		public PartCategorizer.Category category;

		public PartCategorizerButton buttonCopy;

		public MiniCategory(PartCategorizer.Category category, bool main)
		{
			this.category = category;
			buttonCopy = PartCategorizer.InstantiatePartCategorizerButton(category.button.categoryName, category.button.categorydisplayName, category.button.icon, PartCategorizer.Instance.colorCategory, PartCategorizer.Instance.colorIcons);
			if (main)
			{
				GameObject gameObject = buttonCopy.activeButton.gameObject;
				Object.DestroyImmediate(gameObject.GetComponent<UIRadioButton>());
				Object.DestroyImmediate(gameObject.GetComponent<Button>());
			}
			buttonCopy.activeButton.unselectable = false;
			buttonCopy.activeButton.onTrueBtn.AddListener(OnIconSelect);
			buttonCopy.DisableDividers();
			buttonCopy.SetRadioGroup(237);
		}

		public void OnIconSelect(UIRadioButton button, UIRadioButton.CallType callType, PointerEventData data)
		{
			Instance.selectedCategory = category;
		}
	}

	public TextMeshProUGUI title;

	public Button btnClose;

	public Button btnAccept;

	public UIList scrollList;

	public Font font;

	public Callback<string, PartCategorizer.Category> onAccept = delegate
	{
	};

	public CriteriaAccept onAcceptCriteria = delegate(string partName, PartCategorizer.Category category, out string reason)
	{
		reason = "";
		return false;
	};

	public bool _started;

	public string partName;

	public PartCategorizer.Category selectedCategory;

	public static PartCategorizerPopupAddPart Instance { get; set; }

	public bool Showing { get; set; }

	public void Awake()
	{
		Instance = this;
		btnClose.onClick.AddListener(MouseinputClose);
		btnAccept.onClick.AddListener(MouseinputAccept);
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void Show(string partName, string partTitle, Callback<string, PartCategorizer.Category> onAccept, CriteriaAccept onAcceptCriteria, bool forceListRebuild)
	{
		this.partName = partName;
		Showing = true;
		base.gameObject.SetActive(value: true);
		title.text = Localizer.Format("#autoLOC_900551") + " : " + partTitle;
		this.onAccept = onAccept;
		this.onAcceptCriteria = onAcceptCriteria;
		if (!_started)
		{
			_started = true;
		}
		if (forceListRebuild)
		{
			CreateIconList();
		}
		else
		{
			UpdateIconList();
		}
	}

	public void Hide()
	{
		Showing = false;
		base.gameObject.SetActive(value: false);
	}

	public void MouseinputClose()
	{
		Hide();
	}

	public void MouseinputAccept()
	{
		string reason = "";
		if (onAcceptCriteria(partName, selectedCategory, out reason))
		{
			onAccept(partName, selectedCategory);
			Hide();
		}
		else
		{
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("MouseInputAccept", reason, Localizer.Format("#autoLOC_6002480"), HighLogic.UISkin, new DialogGUIButton(Localizer.Format("#autoLOC_417274"), delegate
			{
			})), persistAcrossScenes: false, HighLogic.UISkin);
		}
	}

	public void CreateIconList()
	{
		scrollList.Clear(destroyElements: true);
		UIListItem uIListItem = new GameObject("IconList").AddComponent<UIListItem>();
		uIListItem.gameObject.AddComponent<RectTransform>().pivot = new Vector2(0f, 1f);
		LayoutElement layoutElement = uIListItem.gameObject.AddComponent<LayoutElement>();
		int num = 39;
		int num2 = 36;
		int num3 = 17;
		int num4 = 4;
		int num5 = 4;
		int i = 1;
		for (int count = PartCategorizer.Instance.categories.Count; i < count; i++)
		{
			PartCategorizer.Category category = PartCategorizer.Instance.categories[i];
			if (category.displayType != EditorPartList.State.VariantsList)
			{
				Text text = new GameObject("text_category").AddComponent<Text>();
				text.font = font;
				text.rectTransform.pivot = new Vector2(0f, 1f);
				text.rectTransform.anchorMin = new Vector2(0f, 1f);
				text.rectTransform.anchorMax = new Vector2(0f, 1f);
				text.text = category.button.displayCategoryName;
				text.gameObject.transform.SetParent(uIListItem.transform);
				text.transform.localPosition = new Vector3(num4, -num5, 0f);
				num5 += num3;
				MiniCategory miniCategory = new MiniCategory(category, main: true);
				miniCategory.buttonCopy.transform.SetParent(uIListItem.transform, worldPositionStays: false);
				miniCategory.buttonCopy.transform.localPosition = new Vector3(num4, -num5, 0f);
				int j = 0;
				for (int count2 = PartCategorizer.Instance.categories[i].subcategories.Count; j < count2; j++)
				{
					PartCategorizer.Category category2 = PartCategorizer.Instance.categories[i].subcategories[j];
					MiniCategory miniCategory2 = new MiniCategory(category2, main: false);
					miniCategory2.buttonCopy.gameObject.transform.SetParent(uIListItem.transform);
					miniCategory2.buttonCopy.transform.localPosition = new Vector3(num4 + num, -num5, 0f);
					Text text2 = new GameObject("text_subCategory").AddComponent<Text>();
					text2.font = font;
					text2.rectTransform.pivot = new Vector2(0f, 1f);
					text2.rectTransform.anchorMin = new Vector2(0f, 1f);
					text2.rectTransform.anchorMax = new Vector2(0f, 1f);
					text2.text = category2.button.displayCategoryName;
					text2.transform.SetParent(uIListItem.transform, worldPositionStays: false);
					text2.transform.localPosition = new Vector3(num4 + num * 2 + 2, -num5 - 8, 0f);
					num5 += num2;
				}
			}
		}
		layoutElement.preferredHeight = num5 + num2;
		scrollList.AddItem(uIListItem);
	}

	public void UpdateIconList()
	{
	}
}
