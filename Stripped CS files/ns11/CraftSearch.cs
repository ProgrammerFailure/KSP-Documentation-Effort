using System;
using System.Collections;
using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns11;

public class CraftSearch : MonoBehaviour
{
	public delegate void HasFiltered(bool filtered);

	public static CraftSearch Instance;

	[Tooltip("The search input field")]
	[Header("Inspector assigned")]
	public TMP_InputField searchField;

	[SerializeField]
	[Tooltip("The display area for the vehicles")]
	public GameObject CraftlistArea;

	[Tooltip("(seconds) Delay in checks for keystrokes")]
	[SerializeField]
	public float searchKeystrokeDelay = 0.25f;

	public HasFiltered hasFiltered;

	[NonSerialized]
	public const string SEARCH_FIELD_CONTROLLOCK_ID = "CraftSearchFieldTextInput";

	public Image searchFieldBackground;

	public PointerClickHandler searchFieldClickHandler;

	public float searchTimer;

	public Coroutine searchRoutine;

	public string previousSearch;

	public static CraftBrowserDialog craftBrowserDialog;

	public static GameObject craftBrowserContent;

	public bool IsDifferentSearch => previousSearch != searchField.text;

	public void Awake()
	{
		Instance = this;
		if (craftBrowserDialog == null)
		{
			craftBrowserDialog = Instance.GetComponent<CraftBrowserDialog>();
			craftBrowserContent = CraftlistArea.gameObject;
		}
		searchField = GetComponentInChildren<TMP_InputField>();
		searchFieldBackground = searchField.GetComponent<Image>();
		searchField.onValueChanged.AddListener(SearchField_OnValueChange);
		searchField.onEndEdit.AddListener(SearchField_OnEndEdit);
		searchFieldBackground = searchField.GetComponent<Image>();
		searchFieldClickHandler = searchField.gameObject.AddComponent<PointerClickHandler>();
		searchFieldClickHandler.onPointerClick.AddListener(SearchField_OnClick);
	}

	public IEnumerator Start()
	{
		yield return null;
		searchField.gameObject.SetActive(value: false);
		searchField.gameObject.SetActive(value: true);
	}

	public void OnDestroy()
	{
		StopSearch();
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void OnDisable()
	{
		searchField.text = string.Empty;
		StopSearch();
	}

	public void StopSearch(bool isPaused = false)
	{
		if (searchRoutine != null)
		{
			StopCoroutine(searchRoutine);
			searchRoutine = null;
		}
		if (!isPaused)
		{
			searchFieldBackground.color = Color.white;
			previousSearch = string.Empty;
		}
	}

	public void SearchField_OnEndEdit(string s)
	{
		InputLockManager.RemoveControlLock("CraftSearchFieldTextInput");
	}

	public void SearchField_OnValueChange(string s)
	{
		SearchStart();
	}

	public void SearchField_OnClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			InputLockManager.SetControlLock(ControlTypes.KEYBOARDINPUT, "CraftSearchFieldTextInput");
			SearchStart();
		}
	}

	public virtual void SearchStart()
	{
		if (searchRoutine != null)
		{
			StopSearch();
		}
		searchTimer = Time.realtimeSinceStartup;
		if (searchRoutine == null && base.gameObject.activeInHierarchy)
		{
			searchFieldBackground.color = Color.green;
			searchRoutine = StartCoroutine(SearchRoutine());
		}
	}

	public IEnumerator SearchRoutine()
	{
		while (searchTimer + searchKeystrokeDelay > Time.realtimeSinceStartup)
		{
			yield return null;
		}
		bool filtered = false;
		string searchTerm = searchField.text;
		for (int i = craftBrowserContent.transform.childCount; i > 0; i--)
		{
			int index = i - 1;
			Transform child = craftBrowserContent.transform.GetChild(index);
			CraftEntry component = child.GetComponent<CraftEntry>();
			if (!(component == null))
			{
				bool flag = CraftMatchesSearch(component, searchTerm);
				child.gameObject.SetActive(flag);
				if (!flag && !filtered)
				{
					filtered = true;
				}
				yield return null;
			}
		}
		if (hasFiltered != null)
		{
			hasFiltered(filtered);
		}
		if (string.IsNullOrWhiteSpace(searchTerm) && IsDifferentSearch)
		{
			StopSearch();
		}
		previousSearch = searchTerm;
	}

	public bool CraftMatchesSearch(CraftEntry craft, string searchTerm)
	{
		if (string.IsNullOrWhiteSpace(searchTerm))
		{
			return true;
		}
		string text = Localizer.Format(craft.craftProfileInfo?.description);
		if (craft.name.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) <= -1)
		{
			return text.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) > -1;
		}
		return true;
	}
}
