using FinePrint.Utilities;

public static class LingoonaGrammarExtensions
{
	public enum Gender
	{
		Male,
		Female,
		Neutral
	}

	public static string LocalizeNoun(this string input, Gender gender, bool isProperNoun)
	{
		input = input.LocalizeRemoveGender();
		string text = "^";
		switch (gender)
		{
		case Gender.Male:
			text += "m";
			break;
		case Gender.Female:
			text += "f";
			break;
		case Gender.Neutral:
			text += "n";
			break;
		}
		if (isProperNoun)
		{
			text = text.ToUpper();
		}
		return input + text;
	}

	public static string LocalizeRemoveGender(this string input)
	{
		int num = input.LastIndexOf('^');
		if (num != -1)
		{
			input = input.Substring(0, num);
		}
		return input;
	}

	public static string LocalizeName(this string input, Gender gender)
	{
		return input.LocalizeNoun(gender, isProperNoun: true);
	}

	public static string LocalizeName(this string input, ProtoCrewMember.Gender gender)
	{
		Gender gender2 = Gender.Neutral;
		if (gender == ProtoCrewMember.Gender.Male)
		{
			gender2 = Gender.Male;
		}
		if (gender == ProtoCrewMember.Gender.Female)
		{
			gender2 = Gender.Female;
		}
		return input.LocalizeNoun(gender2, isProperNoun: true);
	}

	public static string LocalizeName(this string input, GrammaticalGender gender)
	{
		Gender gender2 = Gender.Neutral;
		if (gender == GrammaticalGender.MASCULINE)
		{
			gender2 = Gender.Male;
		}
		if (gender == GrammaticalGender.FEMININE)
		{
			gender2 = Gender.Female;
		}
		return input.LocalizeNoun(gender2, isProperNoun: true);
	}

	public static string LocalizeNameMale(this string input)
	{
		return input.LocalizeName(Gender.Male);
	}

	public static string LocalizeNameFemale(this string input)
	{
		return input.LocalizeName(Gender.Female);
	}

	public static string LocalizeNameNeutral(this string input)
	{
		return input.LocalizeName(Gender.Neutral);
	}

	public static string LocalizeCommonNoun(this string input, Gender gender)
	{
		return input.LocalizeNoun(gender, isProperNoun: false);
	}

	public static string LocalizeCommonNounMale(this string input)
	{
		return input.LocalizeCommonNoun(Gender.Male);
	}

	public static string LocalizeCommonNounFemale(this string input)
	{
		return input.LocalizeCommonNoun(Gender.Female);
	}

	public static string LocalizeCommonNounNeutral(this string input)
	{
		return input.LocalizeCommonNoun(Gender.Neutral);
	}
}
