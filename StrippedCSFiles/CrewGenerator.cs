using System;
using System.Runtime.CompilerServices;

public class CrewGenerator
{
	private static string[] kerbalNamesMale;

	private static string[] kerbalNamesFemale;

	private static int specialNameChance;

	private static string crewLastName;

	private static string[] specialNamesMale;

	private static string[] specialNamesFemale;

	internal static string[] KerbalNamesMale
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	internal static string[] KerbalNamesFemale
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	internal static string[] SpecialNamesMale
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	internal static string[] SpecialNamesFemale
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CrewGenerator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static CrewGenerator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Initialize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ProtoCrewMember RandomCrewMemberPrototype(ProtoCrewMember.KerbalType type = ProtoCrewMember.KerbalType.Crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetRandomName(ProtoCrewMember.Gender g, Random generator = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetFullName(string name, string lastName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool LoadNameArray(ref string[] nameArray, string filePrefix, ProtoCrewMember.Gender gender)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static string GetFileName(string filePrefix, ProtoCrewMember.Gender gender)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetLastName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string RemoveLastName(string fullKerbalName)
	{
		throw null;
	}
}
