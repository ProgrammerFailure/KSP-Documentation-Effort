using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Contracts;

public static class TextGen
{
	private static Dictionary<string, ListDictionary<string, string>> texts;

	private static bool setup;

	private static string currentContractType;

	private static string currentAgency;

	private static string currentTopic;

	private static string currentSubject;

	[MethodImpl(MethodImplOptions.NoInlining)]
	static TextGen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Setup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GenerateBackStories(string contractType, string agency, string topic, string subject, int seed, bool allowGenericIntroduction, bool allowGenericProblem, bool allowGenericConclusion)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static string GetSentence(string sentenceType, bool allowGeneric)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static string ProcessTextReplacements(string src, bool wipeReplacements)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static string ReplaceCharAt(string src, char repl, int index)
	{
		throw null;
	}
}
