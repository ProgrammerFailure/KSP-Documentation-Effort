using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using Contracts;
using FinePrint.Contracts.Parameters;
using ns9;

namespace FinePrint.Utilities;

public class StringUtilities
{
	public static string cacheAutoLOC_289926;

	public static string cacheAutoLOC_289929;

	public static string cacheAutoLOC_289933;

	public static string cacheAutoLOC_289969;

	public static string cacheAutoLOC_289916;

	public static string cacheAutoLOC_290048;

	public static string cacheAutoLOC_290050;

	public static string cacheAutoLOC_290052;

	public static string cacheAutoLOC_290054;

	public static string cacheAutoLOC_290056;

	public static string cacheAutoLOC_290058;

	public static string cacheAutoLOC_290060;

	public static string cacheAutoLOC_290062;

	public static string cacheAutoLOC_7001031;

	public static string cacheAutoLOC_7001032;

	public static string cacheAutoLOC_7001033;

	public static string cacheAutoLOC_6001220;

	public static List<string> greekMap = new List<string>();

	public static List<string> alphaNumeric = new List<string>();

	public static List<string> uniqueSites = new List<string>();

	public static List<string> genericPrefixes = new List<string>();

	public static List<string> developerPrefixes = new List<string>();

	public static List<string> abstractPrefixes = new List<string>();

	public static List<string> abstractSuffixes = new List<string>();

	public static List<string> planetSuffixes = new List<string>();

	public static List<string> landSuffixes = new List<string>();

	public static List<string> homeSuffixes = new List<string>();

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_289926 = Localizer.Format("#autoLOC_289926");
		cacheAutoLOC_289929 = Localizer.Format("#autoLOC_289929");
		cacheAutoLOC_289933 = Localizer.Format("#autoLOC_289933");
		cacheAutoLOC_289969 = Localizer.Format("#autoLOC_289969");
		cacheAutoLOC_289916 = Localizer.Format("#autoLOC_289916");
		cacheAutoLOC_290048 = Localizer.Format("#autoLOC_290048");
		cacheAutoLOC_290050 = Localizer.Format("#autoLOC_290050");
		cacheAutoLOC_290052 = Localizer.Format("#autoLOC_290052");
		cacheAutoLOC_290054 = Localizer.Format("#autoLOC_290054");
		cacheAutoLOC_290056 = Localizer.Format("#autoLOC_290056");
		cacheAutoLOC_290058 = Localizer.Format("#autoLOC_290058");
		cacheAutoLOC_290060 = Localizer.Format("#autoLOC_290060");
		cacheAutoLOC_290062 = Localizer.Format("#autoLOC_290062");
		cacheAutoLOC_7001031 = Localizer.Format("#autoLOC_7001031");
		cacheAutoLOC_7001032 = Localizer.Format("#autoLOC_7001032");
		cacheAutoLOC_7001033 = Localizer.Format("#autoLOC_7001033");
		cacheAutoLOC_6001220 = Localizer.Format("#autoLOC_6001220");
	}

	public static void LoadSiteGenerationInfo()
	{
		ConfigNode configNode = ConfigNode.LoadFromTextAssetResource("TextAssets/" + GetSiteGeneratorInfoFileName());
		greekMap = FillListWithElements(configNode, "GreekMap");
		alphaNumeric = FillListWithElements(configNode, "Alphanumeric");
		uniqueSites = FillListWithElements(configNode, "UniqueSites");
		genericPrefixes = FillListWithElements(configNode, "GenericPrefixes");
		developerPrefixes = FillListWithElements(configNode, "DeveloperPrefixes");
		abstractPrefixes = FillListWithElements(configNode, "AbstractPrefixes");
		abstractSuffixes = FillListWithElements(configNode, "AbstractSuffixes");
		planetSuffixes = FillListWithElements(configNode, "planetSuffixes");
		landSuffixes = FillListWithElements(configNode, "LandSuffixes");
		homeSuffixes = FillListWithElements(configNode, "HomeSuffixes");
		configNode.ClearData();
	}

	public static string GetSiteGeneratorInfoFileName()
	{
		string languageIdFromFile = Localizer.GetLanguageIdFromFile();
		return "SiteGenerator/" + languageIdFromFile + "/SiteGeneratorInfo";
	}

	public static List<string> FillListWithElements(ConfigNode node, string nodeName)
	{
		ConfigNode node2 = new ConfigNode();
		if (node.TryGetNode(nodeName, ref node2))
		{
			return node2.GetValuesList("Element");
		}
		return new List<string>();
	}

	public static string GenerateSiteName(int seed, CelestialBody body, bool landLocked, bool allowNamed = true)
	{
		KSPRandom kSPRandom = new KSPRandom(seed);
		if (kSPRandom.Next(1000) == 123)
		{
			return uniqueSites[kSPRandom.Next(uniqueSites.Count)];
		}
		string text3;
		if (allowNamed && kSPRandom.Next(100) < ((body == Planetarium.fetch.Home) ? 50 : 25))
		{
			string text = NamedSitePrefix(kSPRandom);
			string text2 = NamedSiteSuffix(body, landLocked, kSPRandom);
			text3 = Localizer.Format("#autoLOC_7001300", text, text2);
			if (text3.StartsWith("#"))
			{
				text3 = Localizer.Format("<<g:2,1>>", text, text2);
			}
		}
		else
		{
			text3 = genericPrefixes[kSPRandom.Next(genericPrefixes.Count)] + " " + AlphaNumericDesignation(seed);
		}
		return text3;
	}

	public static string NamedSitePrefix(KSPRandom generator = null)
	{
		if (generator == null)
		{
			generator = new KSPRandom();
		}
		ProtoCrewMember.Gender gender = ((!SystemUtilities.CoinFlip(generator)) ? ProtoCrewMember.Gender.Female : ProtoCrewMember.Gender.Male);
		if (SystemUtilities.CoinFlip(generator))
		{
			string input = ShortKerbalName(CrewGenerator.GetRandomName(gender, generator));
			return input.LocalizeName(gender);
		}
		if (generator.Next(100) < 10)
		{
			return developerPrefixes[generator.Next(developerPrefixes.Count)];
		}
		return abstractPrefixes[generator.Next(abstractPrefixes.Count)];
	}

	public static string NamedSiteSuffix(CelestialBody body, bool landLocked, Random generator = null)
	{
		if (generator == null)
		{
			generator = new Random();
		}
		List<List<string>> list = new List<List<string>> { abstractSuffixes };
		if (!body.hasSolidSurface)
		{
			return SystemUtilities.RandomSplitChoice(list, generator);
		}
		list.Add(planetSuffixes);
		if (body.ocean && !landLocked)
		{
			return SystemUtilities.RandomSplitChoice(list, generator);
		}
		list.Add(landSuffixes);
		if (body == Planetarium.fetch.Home)
		{
			list.Add(homeSuffixes);
		}
		return SystemUtilities.RandomSplitChoice(list, generator);
	}

	public static string AlphaNumericDesignation(int seed)
	{
		string text = string.Empty;
		KSPRandom kSPRandom = new KSPRandom(seed);
		bool flag = false;
		int num = kSPRandom.Next(4, 7);
		for (int i = 1; i <= num; i++)
		{
			if (kSPRandom.Next(101) > 70 && i != 1 && i != num && !flag)
			{
				text += "-";
				flag = true;
			}
			else
			{
				text += alphaNumeric[kSPRandom.Next(alphaNumeric.Count)];
			}
		}
		return text;
	}

	public static string IntegerToGreek(int x)
	{
		return greekMap[Math.Min(Math.Max(x, 0), 23)];
	}

	public static string ShortName(string verbose)
	{
		return verbose.Substring(verbose.LastIndexOf(".") + 1);
	}

	public static string PossessiveString(string thing)
	{
		thing = ((thing.EndsWith("s") || thing.EndsWith("ch") || thing.EndsWith("z")) ? (thing + "'") : (thing + "'s"));
		return thing;
	}

	public static string ThisThisAndThat(List<string> stringList, string connector = "#autoLOC_6002373", string conjugation = "#autoLOC_6002374")
	{
		if (stringList.Count == 0)
		{
			return "(NO ELEMENTS)";
		}
		if (stringList.Count == 1)
		{
			return stringList[0];
		}
		StringBuilder stringBuilder = StringBuilderCache.Acquire();
		connector = Localizer.Format(connector);
		conjugation = Localizer.Format(conjugation);
		for (int i = 0; i < stringList.Count - 2; i++)
		{
			stringBuilder.Append(Localizer.Format(stringList[i]));
			stringBuilder.Append(connector);
		}
		stringBuilder.Append(Localizer.Format(stringList[stringList.Count - 2]));
		stringBuilder.Append(conjugation);
		stringBuilder.Append(Localizer.Format(stringList[stringList.Count - 1]));
		return stringBuilder.ToStringAndRelease();
	}

	public static string ShortKerbalName(string fullKerbalName)
	{
		return CrewGenerator.RemoveLastName(fullKerbalName);
	}

	public static string TitleCase(string str)
	{
		return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str);
	}

	public static bool TryParseGeneric<T>(string input, out T value)
	{
		TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
		if (converter != null && converter.IsValid(input))
		{
			value = (T)converter.ConvertFromString(input);
			return true;
		}
		value = default(T);
		return false;
	}

	public static string PackDelimitedString<T>(List<T> items, char delimiter = '|')
	{
		if (items != null && items.Count > 0)
		{
			return string.Join(delimiter.ToString(), Array.ConvertAll(items.ToArray(), (T s) => s.ToString()));
		}
		return string.Empty;
	}

	public static List<T> UnpackDelimitedString<T>(string items, char delimiter = '|')
	{
		List<T> list = new List<T>();
		if (string.IsNullOrEmpty(items))
		{
			return list;
		}
		string[] array = items.Split(delimiter);
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			if (TryParseGeneric<T>(array[i], out var value))
			{
				list.Add(value);
			}
		}
		return list;
	}

	public static KeyValuePair<T1, T2> UnpackDelimitedPair<T1, T2>(string items, char delimiter = '|')
	{
		if (string.IsNullOrEmpty(items))
		{
			return default(KeyValuePair<T1, T2>);
		}
		string[] array = items.Split(delimiter);
		T1 value = default(T1);
		T2 value2 = default(T2);
		if (array.Length != 0)
		{
			TryParseGeneric<T1>(array[0], out value);
		}
		if (array.Length > 1)
		{
			TryParseGeneric<T2>(array[1], out value2);
		}
		return new KeyValuePair<T1, T2>(value, value2);
	}

	public static string ValueAndUnits(RecordTrackType trackType, double trackValue)
	{
		double num = trackValue;
		string text = cacheAutoLOC_289916;
		switch (trackType)
		{
		case RecordTrackType.SPEED:
			text = cacheAutoLOC_289933;
			break;
		case RecordTrackType.ALTITUDE:
		case RecordTrackType.DEPTH:
		case RecordTrackType.DISTANCE:
			if (num >= 10000.0)
			{
				num /= 1000.0;
				text = cacheAutoLOC_289926;
			}
			else
			{
				text = cacheAutoLOC_289929;
			}
			break;
		}
		return Convert.ToDecimal(num).ToString("#,###.#") + text;
	}

	public static GrammaticalGender KerbalGrammaticalGender(ProtoCrewMember pcm)
	{
		return pcm.gender switch
		{
			ProtoCrewMember.Gender.Female => GrammaticalGender.FEMININE, 
			ProtoCrewMember.Gender.Male => GrammaticalGender.MASCULINE, 
			_ => GrammaticalGender.NEUTRAL, 
		};
	}

	public static string AppropriatePronoun(PronounCasing casing, GrammaticalGender gender, bool postFix = false)
	{
		string text = cacheAutoLOC_289969;
		switch (gender)
		{
		default:
			switch (casing)
			{
			case PronounCasing.SUBJECTIVE:
				text = "they";
				break;
			case PronounCasing.OBJECTIVE:
				text = "them";
				break;
			case PronounCasing.POSSESSIVE:
				text = "their";
				break;
			}
			break;
		case GrammaticalGender.FEMININE:
			switch (casing)
			{
			case PronounCasing.SUBJECTIVE:
				text = "she";
				break;
			case PronounCasing.OBJECTIVE:
				text = "her";
				break;
			case PronounCasing.POSSESSIVE:
				text = "her";
				break;
			}
			break;
		case GrammaticalGender.MASCULINE:
			switch (casing)
			{
			case PronounCasing.SUBJECTIVE:
				text = "he";
				break;
			case PronounCasing.OBJECTIVE:
				text = "him";
				break;
			case PronounCasing.POSSESSIVE:
				text = "his";
				break;
			}
			break;
		}
		if (casing == PronounCasing.POSSESSIVE && postFix && text[text.Length - 1] != 's')
		{
			text += "s";
		}
		return text;
	}

	public static string CardinalDirectionBetween(double originLatitude, double originLongitude, double destinationLatitude, double destinationLongitude)
	{
		originLatitude *= Math.PI / 180.0;
		originLongitude *= Math.PI / 180.0;
		destinationLatitude *= Math.PI / 180.0;
		destinationLongitude *= Math.PI / 180.0;
		double num = destinationLongitude - originLongitude;
		double y = Math.Sin(num) * Math.Cos(destinationLatitude);
		double x = Math.Cos(originLatitude) * Math.Sin(destinationLatitude) - Math.Sin(originLatitude) * Math.Cos(destinationLatitude) * Math.Cos(num);
		return (int)Math.Round((Math.Atan2(y, x) * (180.0 / Math.PI) + 360.0) % 360.0 / 45.0) switch
		{
			1 => cacheAutoLOC_290048, 
			2 => cacheAutoLOC_290050, 
			3 => cacheAutoLOC_290052, 
			4 => cacheAutoLOC_290054, 
			5 => cacheAutoLOC_290056, 
			6 => cacheAutoLOC_290058, 
			7 => cacheAutoLOC_290060, 
			_ => cacheAutoLOC_290062, 
		};
	}

	public static bool IsVowel(char c)
	{
		char[] array = new char[6] { 'a', 'e', 'i', 'o', 'u', 'y' };
		c = char.ToLower(c);
		int num = array.Length;
		do
		{
			if (num-- <= 0)
			{
				return false;
			}
		}
		while (array[num] != c);
		return true;
	}

	public static float CalculateReadDuration(string s)
	{
		float num = 200f;
		float num2 = 5f;
		return (float)s.Length / num2 / num * 60f;
	}

	public static List<string> FormattedCurrencies(float funds, float science, float reputation, bool symbols = false, bool verbose = false, TransactionReasons reason = TransactionReasons.None, CurrencyModifierQuery.TextStyling style = CurrencyModifierQuery.TextStyling.OnGUI)
	{
		if (HighLogic.CurrentGame.Mode != Game.Modes.CAREER && HighLogic.CurrentGame.Mode != Game.Modes.SCIENCE_SANDBOX)
		{
			return new List<string>();
		}
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		string text = string.Empty;
		string text2 = string.Empty;
		string text3 = string.Empty;
		string text4 = string.Empty;
		string text5 = string.Empty;
		if (reason != 0)
		{
			CurrencyModifierQuery currencyModifierQuery = CurrencyModifierQuery.RunQuery(reason, funds, science, reputation);
			num = currencyModifierQuery.GetEffectDelta(Currency.Funds);
			num2 = currencyModifierQuery.GetEffectDelta(Currency.Science);
			num3 = currencyModifierQuery.GetEffectDelta(Currency.Reputation);
			if (verbose)
			{
				if (num != 0f)
				{
					text = currencyModifierQuery.GetEffectDeltaText(Currency.Funds, "N0", style);
				}
				if (num2 != 0f)
				{
					text2 = currencyModifierQuery.GetEffectDeltaText(Currency.Science, "N0", style);
				}
				if (num3 != 0f)
				{
					text3 = currencyModifierQuery.GetEffectDeltaText(Currency.Reputation, "N0", style);
				}
			}
		}
		switch (style)
		{
		case CurrencyModifierQuery.TextStyling.OnGUI:
		case CurrencyModifierQuery.TextStyling.OnGUI_LessIsGood:
			text4 = "<color=#>";
			text5 = "</color>";
			break;
		case CurrencyModifierQuery.TextStyling.EzGUIRichText:
		case CurrencyModifierQuery.TextStyling.EzGUIRichText_LessIsGood:
			text4 = "<#>";
			text5 = "</>";
			break;
		}
		List<string> list = new List<string>();
		if (Funding.Instance != null)
		{
			string text6 = (funds + num).ToString("N0");
			bool flag = verbose && !num.ToString("N0").Equals("0");
			if (!text6.Equals("0"))
			{
				list.Add(text4.Replace("#", "#b4d455") + (symbols ? "<sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>  " : string.Empty) + text6 + (flag ? (" " + text) : string.Empty) + (symbols ? " " : (" " + cacheAutoLOC_7001031 + " ")) + text5);
			}
			else if (flag)
			{
				list.Add(text4.Replace("#", "#b4d455") + (symbols ? "<sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>  " : string.Empty) + text + (symbols ? " " : (" " + cacheAutoLOC_7001031 + " ")) + text5);
			}
		}
		if (ResearchAndDevelopment.Instance != null)
		{
			string text6 = (science + num2).ToString("N0");
			bool flag = verbose && !num2.ToString("N0").Equals("0");
			if (!text6.Equals("0"))
			{
				list.Add(text4.Replace("#", "#6dcff6") + (symbols ? "<sprite=\"CurrencySpriteAsset\" name=\"Science\" tint=1> " : string.Empty) + text6 + (flag ? (" " + text2) : string.Empty) + (symbols ? " " : (" " + cacheAutoLOC_7001032 + " ")) + text5);
			}
			else if (flag)
			{
				list.Add(text4.Replace("#", "#6dcff6") + (symbols ? "<sprite=\"CurrencySpriteAsset\" name=\"Science\" tint=1> " : string.Empty) + text2 + (symbols ? " " : (" " + cacheAutoLOC_7001032 + " ")) + text5);
			}
		}
		if (Reputation.Instance != null)
		{
			string text6 = (reputation + num3).ToString("N0");
			bool flag = verbose && !num3.ToString("N0").Equals("0");
			if (!text6.Equals("0"))
			{
				list.Add(text4.Replace("#", "#e0d503") + (symbols ? "<sprite=\"CurrencySpriteAsset\" name=\"Reputation\" tint=1> " : string.Empty) + text6 + (flag ? (" " + text3) : string.Empty) + (symbols ? " " : (" " + cacheAutoLOC_7001033 + " ")) + text5);
			}
			else if (flag)
			{
				list.Add(text4.Replace("#", "#e0d503") + (symbols ? "<sprite=\"CurrencySpriteAsset\" name=\"Reputation\" tint=1> " : string.Empty) + text3 + (symbols ? " " : (" " + cacheAutoLOC_7001033 + " ")) + text5);
			}
		}
		return list;
	}

	public static string SpecificVesselName(Contract contract)
	{
		SpecificVesselParameter parameter = contract.GetParameter<SpecificVesselParameter>();
		if (parameter != null && parameter.targetVesselName != null)
		{
			return parameter.targetVesselName;
		}
		return cacheAutoLOC_6001220;
	}
}
