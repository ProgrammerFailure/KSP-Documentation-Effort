using System;
using System.Collections;
using System.Collections.Generic;
using ns11;
using ns2;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VesselSpawnSearch : MonoBehaviour
{
	public delegate void HasFiltered(bool filtered);

	[Header("Inspector assigned")]
	[Tooltip("The input field for search terms")]
	[SerializeField]
	public TMP_InputField searchField;

	[SerializeField]
	[Tooltip("The content area where the searchable widgets appear")]
	public RectTransform scrollListContent;

	[SerializeField]
	[Tooltip("(seconds) Delay in checks for keystrokes")]
	public float searchKeystrokeDelay = 0.25f;

	[SerializeField]
	[Tooltip("The full tab background for when the Steam tab is visible")]
	public RectTransform fullTabBackground;

	[SerializeField]
	[Tooltip("The toggle for the stock craft / player craft list")]
	public Toggle tabVesselList;

	[SerializeField]
	[Tooltip("Items the search filter will ignore")]
	public List<GameObject> filterIgnoreItems;

	public HasFiltered hasFiltered;

	[NonSerialized]
	public const string gameSearchFieldControlLockId = "GameSearchFieldTextInput";

	public Image searchFieldBackground;

	public PointerClickHandler searchFieldClickHandler;

	public float searchTimer;

	public Coroutine searchRoutine;

	public string previousSearch;

	public bool HasSearchText
	{
		get
		{
			if (searchField != null)
			{
				return !string.IsNullOrEmpty(searchField.text);
			}
			return false;
		}
	}

	public void Awake()
	{
		searchField = GetComponentInChildren<TMP_InputField>();
		searchFieldBackground = searchField.GetComponent<Image>();
		searchField.onValueChanged.AddListener(SearchField_OnValueChange);
		searchField.onEndEdit.AddListener(SearchField_OnEndEdit);
		searchFieldBackground = searchField.GetComponent<Image>();
		searchFieldClickHandler = searchField.gameObject.AddComponent<PointerClickHandler>();
		searchFieldClickHandler.onPointerClick.AddListener(SearchField_OnClick);
		tabVesselList.onValueChanged.AddListener(delegate(bool data)
		{
			if (data)
			{
				SearchRestart();
			}
		});
	}

	public void OnDisable()
	{
		SearchStop();
	}

	public void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape) && !string.IsNullOrEmpty(searchField.text))
		{
			searchField.text = string.Empty;
		}
		AdjustSearchInputSize();
	}

	public void OnDestroy()
	{
		SearchStop();
	}

	public void SearchField_OnEndEdit(string s)
	{
		InputLockManager.RemoveControlLock("GameSearchFieldTextInput");
	}

	public void SearchField_OnValueChange(string s)
	{
		SearchStart();
	}

	public void SearchField_OnClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			InputLockManager.SetControlLock(ControlTypes.KEYBOARDINPUT, "GameSearchFieldTextInput");
			SearchStart();
		}
	}

	public void SteamCraftListReceived()
	{
		SearchRestart();
	}

	public void SearchRestart()
	{
		SearchStop();
		SearchStart();
	}

	public virtual void SearchStart()
	{
		searchTimer = Time.realtimeSinceStartup;
		if (searchRoutine == null && base.gameObject.activeInHierarchy)
		{
			searchRoutine = StartCoroutine(SearchRoutine());
		}
	}

	public IEnumerator SearchRoutine()
	{
		while (searchTimer + searchKeystrokeDelay > Time.realtimeSinceStartup)
		{
			yield return null;
		}
		string text = searchField.text;
		bool flag = previousSearch != null && previousSearch.Equals(text, StringComparison.OrdinalIgnoreCase);
		bool flag2 = string.IsNullOrEmpty(text);
		previousSearch = text;
		bool flag3 = false;
		for (int num = scrollListContent.transform.childCount; num > 0; num--)
		{
			int index = num - 1;
			Transform child = scrollListContent.transform.GetChild(index);
			if (!(child == null) && !filterIgnoreItems.Contains(child.gameObject))
			{
				bool flag4 = ItemMatchesSearch(child, text);
				child.gameObject.SetActive(flag4);
				if (!flag4 && !flag3)
				{
					flag3 = true;
				}
			}
		}
		if (hasFiltered != null)
		{
			hasFiltered(flag3);
		}
		if (flag || flag2)
		{
			SearchStop();
			yield break;
		}
		searchFieldBackground.color = Color.green;
		searchRoutine = null;
	}

	public bool ItemMatchesSearch(Transform item, string searchTerm)
	{
		if (string.IsNullOrWhiteSpace(searchTerm))
		{
			return true;
		}
		VesselListItem component = item.GetComponent<VesselListItem>();
		CraftEntry component2 = item.GetComponent<CraftEntry>();
		if (component == null && component2 == null)
		{
			return true;
		}
		string text = string.Empty;
		if (component != null)
		{
			text = component.vesselName.text;
		}
		if (component2 != null)
		{
			text = component2.craftName;
		}
		return text.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) > -1;
	}

	public void SearchStop()
	{
		searchFieldBackground.color = Color.white;
		searchRoutine = null;
		previousSearch = null;
	}

	public void AdjustSearchInputSize()
	{
		if (!(fullTabBackground == null) && !(searchField == null))
		{
			LayoutElement component = searchField.GetComponent<LayoutElement>();
			if (!(component == null))
			{
				component.enabled = fullTabBackground.gameObject.activeInHierarchy;
			}
		}
	}
}
