using System;
using System.Collections;
using System.Collections.Generic;
using ns2;
using ns5;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns11;

public class BasePartCategorizer : MonoBehaviour
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

	public UIList scrollListSub;

	[SerializeField]
	public GameObject iconLoaderPrefab;

	[NonSerialized]
	public IconLoader iconLoader;

	public TMP_InputField searchField;

	public float searchKeystrokeDelay = 0.25f;

	public Image searchFieldBackground;

	public PointerClickHandler searchFieldClickHandler;

	[NonSerialized]
	public Color colorFilterFunction = new Color(0.604f, 0.784f, 0.878f, 1f);

	[NonSerialized]
	public Color colorIcons = new Color(1f, 1f, 1f, 0.7f);

	public float searchTimer;

	public Coroutine searchRoutine;

	public EditorPartListFilter<AvailablePart> filterPods = new EditorPartListFilter<AvailablePart>("Function_Pods", (AvailablePart p) => p.category == PartCategories.Pods);

	public EditorPartListFilter<AvailablePart> filterEngine = new EditorPartListFilter<AvailablePart>("Function_Engine", (AvailablePart p) => p.category == PartCategories.Engine || (p.category == PartCategories.Propulsion && p.moduleInfos.Exists((AvailablePart.ModuleInfo q) => q.moduleName == "Engine")));

	public EditorPartListFilter<AvailablePart> filterFuelTank = new EditorPartListFilter<AvailablePart>("Function_FuelTank", (AvailablePart p) => p.category == PartCategories.FuelTank || (p.category == PartCategories.Propulsion && !p.moduleInfos.Exists((AvailablePart.ModuleInfo q) => q.moduleName == "Engine")));

	public EditorPartListFilter<AvailablePart> filterControl = new EditorPartListFilter<AvailablePart>("Function_Control", (AvailablePart p) => p.category == PartCategories.Control);

	public EditorPartListFilter<AvailablePart> filterStructural = new EditorPartListFilter<AvailablePart>("Function_Structural", (AvailablePart p) => p.category == PartCategories.Structural);

	public EditorPartListFilter<AvailablePart> filterCoupling = new EditorPartListFilter<AvailablePart>("Function_Coupling", (AvailablePart p) => p.category == PartCategories.Coupling);

	public EditorPartListFilter<AvailablePart> filterPayload = new EditorPartListFilter<AvailablePart>("Function_Payload", (AvailablePart p) => p.category == PartCategories.Payload);

	public EditorPartListFilter<AvailablePart> filterAero = new EditorPartListFilter<AvailablePart>("Function_Aero", (AvailablePart p) => p.category == PartCategories.Aero);

	public EditorPartListFilter<AvailablePart> filterGround = new EditorPartListFilter<AvailablePart>("Function_Ground", (AvailablePart p) => p.category == PartCategories.Ground);

	public EditorPartListFilter<AvailablePart> filterThermal = new EditorPartListFilter<AvailablePart>("Function_Thermal", (AvailablePart p) => p.category == PartCategories.Thermal);

	public EditorPartListFilter<AvailablePart> filterElectrical = new EditorPartListFilter<AvailablePart>("Function_Electrical", (AvailablePart p) => p.category == PartCategories.Electrical);

	public EditorPartListFilter<AvailablePart> filterCommunication = new EditorPartListFilter<AvailablePart>("Function_Communication", (AvailablePart p) => p.category == PartCategories.Communication);

	public EditorPartListFilter<AvailablePart> filterScience = new EditorPartListFilter<AvailablePart>("Function_Science", (AvailablePart p) => p.category == PartCategories.Science);

	public EditorPartListFilter<AvailablePart> filterCargo = new EditorPartListFilter<AvailablePart>("Function_Cargo", (AvailablePart p) => p.category == PartCategories.Cargo);

	public EditorPartListFilter<AvailablePart> filterRobotics = new EditorPartListFilter<AvailablePart>("Function_Robotics", (AvailablePart p) => p.category == PartCategories.Robotics);

	public EditorPartListFilter<AvailablePart> filterUtility = new EditorPartListFilter<AvailablePart>("Function_Utility", (AvailablePart p) => p.category == PartCategories.Utility);

	public static string[] size0Tags = new string[6] { "0.625", ")mini", "small", "tiny", "little", "micro" };

	public static string[] size1Tags = new string[6] { "1.25", "FL-T", "regular", "standard", "average", "medium" };

	public static string[] size1p5Tags = new string[6] { "1.875", "FL-TX", "regular", "standard", "average", "medium" };

	public static string[] size2Tags = new string[5] { "2.5", "huge", "jumbo", "large", "big" };

	public static string[] size3Tags = new string[5] { "3.75", "enormous", "massive", "gigantic", "giant" };

	public static string[] size4Tags = new string[5] { "5", "enormous", "massive", "gigantic", "giant" };

	public static string[] srfTags = new string[2] { "surface", "attach" };

	public static string[] xfeedTags = new string[2] { "cross", "feed" };

	public static string[] mannedTags = new string[3] { "(crew", "(mann", "kerbal" };

	public static string[] unmannedTags = new string[2] { "(uncrew", "(unmann" };

	public static string[] radialTag = new string[1] { "radial" };

	public static string[] cargoTag = new string[1] { "cargo" };

	public virtual void SearchField_OnEndEdit(string s)
	{
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
		string[] searchTerms = SearchTagSplit(searchField.text);
		Func<AvailablePart, bool> criteria = (AvailablePart p) => PartMatchesSearch(p, searchTerms);
		SearchFilterResult(new EditorPartListFilter<AvailablePart>("SearchFilter_", criteria));
		searchFieldBackground.color = Color.green;
		searchRoutine = null;
	}

	public virtual void SearchStop()
	{
		searchFieldBackground.color = Color.white;
		SearchFilterResult(null);
		searchRoutine = null;
	}

	public virtual void SearchFilterResult(EditorPartListFilter<AvailablePart> filter)
	{
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

	public void Awake()
	{
		GameEvents.onLanguageSwitched.Add(LoadAutoTags);
	}

	public void LoadAutoTags()
	{
		size0Tags = Localizer.Format("#autoLOC_8003192").Split(' ');
		size1Tags = Localizer.Format("#autoLOC_8003193").Split(' ');
		size1p5Tags = Localizer.Format("#autoLOC_8003194").Split(' ');
		size2Tags = Localizer.Format("#autoLOC_8003195").Split(' ');
		size3Tags = Localizer.Format("#autoLOC_8003196").Split(' ');
		size4Tags = Localizer.Format("#autoLOC_8003197").Split(' ');
		srfTags = Localizer.Format("#autoLOC_8003198").Split(' ');
		xfeedTags = Localizer.Format("#autoLOC_8003199").Split(' ');
		mannedTags = Localizer.Format("#autoLOC_8003200").Split(' ');
		unmannedTags = Localizer.Format("#autoLOC_8003201").Split(' ');
		radialTag = Localizer.Format("#autoLOC_8003202").Split(' ');
		cargoTag = Localizer.Format("#autoLOC_8003411").Split(' ');
	}

	public static string GeneratePartAutoTags(AvailablePart p)
	{
		if (p == null)
		{
			return string.Empty;
		}
		List<string> list = new List<string>();
		string[] array = SearchTagSplit(p.title);
		for (int num = array.Length - 1; num >= 0; num--)
		{
			string text = array[num];
			text = ((text.Length > 3) ? ("(" + text) : ("?" + text));
			list.Add(text);
		}
		string[] array2 = SearchTagSplit(p.manufacturer + " " + p.category);
		for (int num2 = array2.Length - 1; num2 >= 0; num2--)
		{
			list.Add("?" + array2[num2]);
		}
		if (p.bulkheadProfiles != null)
		{
			string[] array3 = p.bulkheadProfiles.Split(',');
			for (int num3 = array3.Length - 1; num3 >= 0; num3--)
			{
				switch (array3[num3].Trim())
				{
				case "size0":
					list.AddRange(size0Tags);
					break;
				case "size4":
					list.AddRange(size4Tags);
					break;
				case "size1p5":
					list.AddRange(size1p5Tags);
					break;
				case "size2":
					list.AddRange(size2Tags);
					break;
				case "size1":
					list.AddRange(size1Tags);
					break;
				case "srf":
					list.AddRange(srfTags);
					if (array3.Length == 1)
					{
						list.AddRange(radialTag);
					}
					break;
				case "size3":
					list.AddRange(size3Tags);
					break;
				}
			}
		}
		Part partPrefab = p.partPrefab;
		if (partPrefab == null)
		{
			return string.Join(" ", list.ToArray());
		}
		if (partPrefab.fuelCrossFeed)
		{
			list.AddRange(xfeedTags);
		}
		if (partPrefab.vesselType > VesselType.Debris)
		{
			list.AddRange((partPrefab.CrewCapacity > 0) ? mannedTags : unmannedTags);
		}
		if (partPrefab.FindModuleImplementing<ModuleCargoPart>() != null)
		{
			list.AddRange(cargoTag);
		}
		return string.Join(" ", list.ToArray());
	}
}
