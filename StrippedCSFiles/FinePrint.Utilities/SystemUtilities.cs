using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Contracts;
using UnityEngine;

namespace FinePrint.Utilities;

public class SystemUtilities
{
	public const int frameSuccessDelay = 5;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SystemUtilities()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Texture2D LoadTexture(string textureName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Color RandomColor(int seed = 0, float alpha = 1f, float saturation = 1f, float brightness = 0.5f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool TryConvert<T>(string input, out T value, ref string error)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static PreBuiltCraftDefinition ValidateLoadCraftNode(string url, string className, bool brokenParts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static PreBuiltCraftDefinition ValidateLoadCraftNode(string url, string className, bool brokenParts, bool bypassTechCheck)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void LoadNode<T>(ConfigNode node, string className, string valueName, ref T value, T defaultValue, bool logging = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void LoadNodeList<T>(ConfigNode node, string className, string valueName, ref List<T> list)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SaveNodeList<T>(ConfigNode node, string listName, List<T> list)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CondenseNodeList(ConfigNode node, string valueName, string listName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void LoadNodePair<T1, T2>(ConfigNode node, string className, string valueName, ref KeyValuePair<T1, T2> pair)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool FlightIsReady(bool checkVessel = false, CelestialBody targetBody = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool FlightIsReady(Contract.State currentState, Contract.State targetState, bool checkVessel = false, CelestialBody targetBody = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool CoinFlip(System.Random generator = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static uint HashNumber(uint x)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int SuperSeed(Contract c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ProcessSideRequests(Contract contract, ConfigNode contractNode, CelestialBody targetBody, string vesselName, ref float fundsMultiplier, ref float scienceMultiplier, ref float reputationMultiplier, Vessel vessel = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ExpungeKerbal(ProtoCrewMember pcm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ExpungeVessel(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool WithinDeviation(double v1, double v2, double deviation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool WithinDeviationByValue(double v1, double v2, double deviation, double fullValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double MeasureDeviation(double v1, double v2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double MeasureDeviationByValue(double v1, double v2, double fullValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static double GetDeviationAsFraction(double deviation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double WithinDeviationAccuracy(double v1, double v2, double deviation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double WithinDeviationByReferenceAccuracy(double v1, double v2, double reference, double deviation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ShuffleList<T>(ref List<T> list, System.Random generator = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T RandomSplitChoice<T>(List<List<T>> listOfLists, System.Random generator = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool CheckTouristRecoveryContractKerbals(string kerbalName)
	{
		throw null;
	}
}
