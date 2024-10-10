using System;
using System.Collections;
using System.Collections.Generic;
using ns2;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns11;

public class RDTechTreeSearchBar : MonoBehaviour
{
	public enum MatchType
	{
		NONE,
		EQUALS_ONLY,
		TERM_STARTS_WITH_TAG,
		TERM_ENDS_WITH_TAG,
		TAG_STARTS_WITH_TERM,
		TAG_ENDS_WITH_TERM,
		EITHER_STARTS_WITH_EITHER,
		EITHER_ENDS_WITH_EITHER,
		TERM_CONTAINS_TAG,
		TAG_CONTAINS_TERM,
		EITHER_CONTAINS_EITHER
	}

	public static RDTechTreeSearchBar Instance;

	public float searchTimer;

	public Coroutine searchRoutine;

	public float searchKeystrokeDelay = 0.25f;

	public Image searchFieldBackground;

	public PointerClickHandler searchFieldClickHandler;

	public List<RDNode> treeNodes = new List<RDNode>();

	public TMP_InputField searchField;

	public static Color searchSelectionNodeColor;

	public static Color searchSelectionPartColor;

	public Button clearButton;

	public void Awake()
	{
		if (Instance != null)
		{
			base.gameObject.DestroyGameObject();
			return;
		}
		Instance = this;
		searchFieldBackground = searchField.GetComponent<Image>();
		searchFieldClickHandler = searchField.gameObject.AddComponent<PointerClickHandler>();
		searchField.onValueChanged.AddListener(SearchField_OnValueChange);
		clearButton.onClick.AddListener(ClearSearchBox);
	}

	public void Start()
	{
		treeNodes = RDController.Instance.nodes;
	}

	public virtual void SearchField_OnValueChange(string s)
	{
		if (searchField.text != string.Empty)
		{
			SearchStart();
		}
		else
		{
			SearchStop();
		}
	}

	public virtual void SearchField_OnClick(PointerEventData eventData)
	{
		SearchStart();
	}

	public virtual void SearchStart()
	{
		searchTimer = Time.realtimeSinceStartup;
		if (searchRoutine == null)
		{
			searchRoutine = StartCoroutine(SearchRoutine());
		}
	}

	public virtual IEnumerator SearchRoutine()
	{
		while (searchTimer + searchKeystrokeDelay > Time.realtimeSinceStartup)
		{
			yield return null;
		}
		searchFieldBackground.color = Color.green;
		searchRoutine = null;
		SearchForNode(searchField.text);
	}

	public virtual void SearchStop()
	{
		DeselectSearchResults();
		searchFieldBackground.color = Color.white;
		searchRoutine = null;
	}

	public void SearchForNode(string name)
	{
		string[] terms = SearchTagSplit(name);
		for (int i = 0; i < treeNodes.Count; i++)
		{
			int num = 0;
			List<AvailablePart> partsAssigned = treeNodes[i].tech.partsAssigned;
			for (int j = 0; j < partsAssigned.Count; j++)
			{
				if (partsAssigned[j].name.Contains(name) || PartMatchesSearch(partsAssigned[j], terms))
				{
					num++;
				}
			}
			if (num > 0)
			{
				treeNodes[i].graphics.searchHighlight.enabled = true;
				treeNodes[i].graphics.searchHighlight.color = searchSelectionNodeColor;
			}
			else
			{
				treeNodes[i].graphics.searchHighlight.enabled = false;
			}
		}
		SelectPartIcons();
	}

	public void DeselectSearchResults()
	{
		for (int i = 0; i < treeNodes.Count; i++)
		{
			treeNodes[i].graphics.searchHighlight.enabled = false;
		}
		List<RDPartListItem> listItems = RDController.Instance.partList.listItems;
		for (int j = 0; j < listItems.Count; j++)
		{
			listItems[j].searchHighlight.enabled = false;
		}
	}

	public void SelectPartIcons()
	{
		string[] terms = SearchTagSplit(searchField.text);
		List<RDPartListItem> listItems = RDController.Instance.partList.listItems;
		if (!(searchField.text != string.Empty))
		{
			return;
		}
		for (int i = 0; i < listItems.Count; i++)
		{
			if (listItems[i].myPart.name.Contains(searchField.text) || PartMatchesSearch(listItems[i].myPart, terms))
			{
				listItems[i].searchHighlight.enabled = true;
				listItems[i].searchHighlight.color = searchSelectionPartColor;
			}
			if (!listItems[i].myPart.name.Contains(searchField.text) && !PartMatchesSearch(listItems[i].myPart, terms))
			{
				listItems[i].searchHighlight.enabled = false;
			}
		}
	}

	public MatchType TagMatchType(ref string tag)
	{
		char c = '_';
		if (tag.Length > 0)
		{
			c = tag[0];
		}
		switch (c)
		{
		case '<':
			tag = tag.Substring(1);
			return MatchType.TERM_CONTAINS_TAG;
		case '>':
			tag = tag.Substring(1);
			return MatchType.TAG_CONTAINS_TERM;
		case '?':
			tag = tag.Substring(1);
			return MatchType.EQUALS_ONLY;
		case ')':
			tag = tag.Substring(1);
			return MatchType.EITHER_ENDS_WITH_EITHER;
		case '(':
			tag = tag.Substring(1);
			return MatchType.EITHER_STARTS_WITH_EITHER;
		case ']':
			tag = tag.Substring(1);
			return MatchType.TERM_ENDS_WITH_TAG;
		case '[':
			tag = tag.Substring(1);
			return MatchType.TERM_STARTS_WITH_TAG;
		default:
			return MatchType.EITHER_CONTAINS_EITHER;
		case '}':
			tag = tag.Substring(1);
			return MatchType.TAG_ENDS_WITH_TERM;
		case '{':
			tag = tag.Substring(1);
			return MatchType.TAG_STARTS_WITH_TERM;
		}
	}

	public bool TermMatchesTag(string term, string tag)
	{
		switch (TagMatchType(ref tag))
		{
		default:
			if (!term.Contains(tag))
			{
				return tag.Contains(term);
			}
			return true;
		case MatchType.EQUALS_ONLY:
			return term.Equals(tag);
		case MatchType.TERM_STARTS_WITH_TAG:
			return term.StartsWith(tag);
		case MatchType.TERM_ENDS_WITH_TAG:
			return term.EndsWith(tag);
		case MatchType.TAG_STARTS_WITH_TERM:
			return tag.StartsWith(term);
		case MatchType.TAG_ENDS_WITH_TERM:
			return tag.EndsWith(term);
		case MatchType.EITHER_STARTS_WITH_EITHER:
			if (!term.StartsWith(tag))
			{
				return tag.StartsWith(term);
			}
			return true;
		case MatchType.EITHER_ENDS_WITH_EITHER:
			if (!term.EndsWith(tag))
			{
				return tag.EndsWith(term);
			}
			return true;
		case MatchType.TERM_CONTAINS_TAG:
			return term.Contains(tag);
		case MatchType.TAG_CONTAINS_TERM:
			return tag.Contains(term);
		}
	}

	public static string[] SearchTagSplit(string terms)
	{
		return terms.Trim().ToLowerInvariant().Split(new char[5] { ' ', '|', ';', ',', '-' }, StringSplitOptions.RemoveEmptyEntries);
	}

	public bool PartMatchesSearch(AvailablePart part, string[] terms)
	{
		if (part.category == PartCategories.none)
		{
			return false;
		}
		string[] array = SearchTagSplit(part.tags);
		int num = terms.Length - 1;
		while (true)
		{
			if (num >= 0)
			{
				string text = terms[num];
				int num2 = Math.Min(text.Length, 3);
				bool flag = false;
				int num3 = array.Length - 1;
				while (num3 >= 0)
				{
					string text2 = array[num3];
					if (text2.Length < num2 || !TermMatchesTag(text, text2))
					{
						num3--;
						continue;
					}
					flag = true;
					break;
				}
				if (!flag)
				{
					break;
				}
				num--;
				continue;
			}
			return true;
		}
		return false;
	}

	public void ClearSearchBox()
	{
		searchField.text = string.Empty;
		DeselectSearchResults();
	}
}
