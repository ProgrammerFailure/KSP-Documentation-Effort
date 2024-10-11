using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Screens;

public class RDTestSceneLoader : MonoBehaviour
{
	public static RDTestSceneLoader Instance;

	public List<AvailablePart> partsList;

	public GameObject partIconPlaceholder;

	public PartResourceLibrary ResourcesLibrary;

	public List<ScienceSubject> subjects;

	public string sfsFilePath;

	private static Dictionary<string, ScienceExperiment> experiments;

	public string scienceDefsPath;

	public static List<AvailablePart> LoadedPartsList
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RDTestSceneLoader()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadPartDefs()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ScienceExperiment GetExperiment(string experimentID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetResults(string subjectID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<string> GetExperimentIDs()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<string> GetSituationTagsDescriptions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<string> GetSituationTags()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<string> GetBiomeTagsLocalized(CelestialBody cb, bool includeMiniBiomes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<string> GetMiniBiomeTagsLocalized(CelestialBody cb)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<string> GetBiomeTags(CelestialBody cb, bool includeMiniBiomes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<string> GetMiniBiomeTags(CelestialBody cb)
	{
		throw null;
	}
}
