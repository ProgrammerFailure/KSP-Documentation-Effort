using System;
using System.Collections;
using ns2;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoadGameSearch : MonoBehaviour
{
	public delegate void HasFiltered(bool filtered);

	[SerializeField]
	[Tooltip("The input field for search terms")]
	[Header("Inspector assigned")]
	public TMP_InputField searchField;

	[SerializeField]
	[Tooltip("The content area where the saved game widgets appear")]
	public RectTransform scrollListContent;

	[SerializeField]
	[Tooltip("(seconds) Delay in checks for keystrokes")]
	public float searchKeystrokeDelay = 0.25f;

	public HasFiltered hasFiltered;

	[NonSerialized]
	public const string gameSearchFieldControlLockId = "GameSearchFieldTextInput";

	public Image searchFieldBackground;

	public PointerClickHandler searchFieldClickHandler;

	public float searchTimer;

	public Coroutine searchRoutine;

	public string previousSearch;

	public void Awake()
	{
		searchField = GetComponentInChildren<TMP_InputField>();
		searchFieldBackground = searchField.GetComponent<Image>();
		searchField.onValueChanged.AddListener(SearchField_OnValueChange);
		searchField.onEndEdit.AddListener(SearchField_OnEndEdit);
		searchFieldBackground = searchField.GetComponent<Image>();
		searchFieldClickHandler = searchField.gameObject.AddComponent<PointerClickHandler>();
		searchFieldClickHandler.onPointerClick.AddListener(SearchField_OnClick);
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
		string searchTerm = searchField.text;
		if (previousSearch != null && previousSearch.Equals(searchTerm, StringComparison.OrdinalIgnoreCase))
		{
			SearchStop();
			yield return null;
		}
		else
		{
			previousSearch = searchTerm;
		}
		bool flag = false;
		for (int num = scrollListContent.transform.childCount; num > 0; num--)
		{
			int index = num - 1;
			Transform child = scrollListContent.transform.GetChild(index);
			if (!(child == null))
			{
				bool flag2 = ItemMatchesSearch(child, searchTerm);
				child.gameObject.SetActive(flag2);
				if (!flag2 && !flag)
				{
					flag = true;
				}
			}
		}
		if (hasFiltered != null)
		{
			hasFiltered(flag);
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
		VerticalLayoutGroup componentInChildren = item.GetComponentInChildren<VerticalLayoutGroup>();
		if (componentInChildren == null)
		{
			return true;
		}
		Transform child = componentInChildren.transform.GetChild(0);
		if (child == null)
		{
			return true;
		}
		TMP_Text component = child.GetComponent<TMP_Text>();
		if (component == null)
		{
			return true;
		}
		return component.text.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) > -1;
	}

	public void SearchStop()
	{
		searchFieldBackground.color = Color.white;
		searchRoutine = null;
	}
}
