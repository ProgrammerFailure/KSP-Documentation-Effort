using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ns11;

public class RDTestSceneLoader : MonoBehaviour
{
	public static RDTestSceneLoader Instance;

	public List<AvailablePart> partsList;

	public GameObject partIconPlaceholder;

	public PartResourceLibrary ResourcesLibrary;

	public List<ScienceSubject> subjects;

	public string sfsFilePath = "saves/default/persistent.sfs";

	public static Dictionary<string, ScienceExperiment> experiments;

	public string scienceDefsPath = "GameData/Squad/Resources/ScienceDefs.cfg";

	public static List<AvailablePart> LoadedPartsList => Instance.partsList;

	public void Awake()
	{
		Instance = this;
		ResourcesLibrary = base.gameObject.AddComponent<PartResourceLibrary>();
		if (File.Exists(KSPUtil.ApplicationRootPath + scienceDefsPath))
		{
			ConfigNode[] nodes = ConfigNode.Load(KSPUtil.ApplicationRootPath + "GameData/Squad/Resources/ResourcesGeneric.cfg").GetNodes("RESOURCE_DEFINITION");
			int num = nodes.Length;
			for (int i = 0; i < num; i++)
			{
				ResourcesLibrary.resourceDefinitions.Add(nodes[i]);
			}
		}
		LoadPartDefs();
		subjects = new List<ScienceSubject>();
		if (File.Exists(KSPUtil.ApplicationRootPath + sfsFilePath))
		{
			ConfigNode node = ConfigNode.Load(KSPUtil.ApplicationRootPath + sfsFilePath).GetNode("GAME");
			if (node != null)
			{
				ConfigNode[] nodes2 = node.GetNodes("SCENARIO");
				if (nodes2 == null || nodes2.Length == 0)
				{
					return;
				}
				ConfigNode configNode = null;
				int j = 0;
				for (int num2 = nodes2.Length; j < num2; j++)
				{
					ConfigNode configNode2 = nodes2[j];
					if (configNode2.GetNodes("Science").Length != 0)
					{
						configNode = configNode2;
						ConfigNode[] nodes3 = configNode.GetNodes("Science");
						int num3 = 0;
						int num4 = nodes3.Length;
						while (num3 < num4)
						{
							subjects.Add(new ScienceSubject(nodes3[num3]));
							j++;
						}
						break;
					}
				}
				if (configNode == null)
				{
					Debug.LogError("[TestLoader]: No R&D Node found on given sfs file");
					return;
				}
				Debug.Log("[TestLoader]: Loaded " + subjects.Count + " subjects!");
			}
			else
			{
				Debug.LogError("[TestLoader]: No GAME Node found on given sfs file");
			}
		}
		else
		{
			Debug.LogError("[TestLoader]: " + sfsFilePath + " does not exist.");
		}
		experiments = new Dictionary<string, ScienceExperiment>();
		if (File.Exists(KSPUtil.ApplicationRootPath + scienceDefsPath))
		{
			ConfigNode[] nodes4 = ConfigNode.Load(KSPUtil.ApplicationRootPath + scienceDefsPath).GetNodes("EXPERIMENT_DEFINITION");
			int num5 = nodes4.Length;
			for (int k = 0; k < num5; k++)
			{
				ScienceExperiment scienceExperiment = new ScienceExperiment();
				scienceExperiment.Load(nodes4[k]);
				experiments.Add(scienceExperiment.id, scienceExperiment);
				Debug.Log("[TestLoader]: Added Experiment Definition: " + scienceExperiment.id, base.gameObject);
			}
		}
		else
		{
			Debug.LogError("[TestLoader]: ScienceDefs file not found at " + sfsFilePath);
		}
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void LoadPartDefs()
	{
		partsList = new List<AvailablePart>();
		FileInfo[] files = new DirectoryInfo(KSPUtil.ApplicationRootPath + "/GameData").GetFiles("*.cfg", SearchOption.AllDirectories);
		Debug.Log(files.Length + " files found");
		int num = files.Length;
		for (int i = 0; i < num; i++)
		{
			FileInfo fileInfo = files[i];
			ConfigNode configNode = ConfigNode.Load(fileInfo.FullName);
			if (configNode.HasNode("PART"))
			{
				ConfigNode node = configNode.GetNode("PART");
				AvailablePart availablePart = new AvailablePart();
				availablePart.name = node.GetValue("name");
				availablePart.title = node.GetValue("title");
				availablePart.description = node.GetValue("description");
				if (!string.IsNullOrEmpty(availablePart.description))
				{
					availablePart.description = availablePart.description.Replace("\\n", "\n");
				}
				availablePart.manufacturer = node.GetValue("manufacturer");
				availablePart.partConfig = node;
				availablePart.configFileFullName = fileInfo.FullName;
				availablePart.category = (PartCategories)Enum.Parse(typeof(PartCategories), node.GetValue("category"));
				if (node.HasValue("TechRequired"))
				{
					availablePart.TechRequired = node.GetValue("TechRequired");
				}
				if (node.HasValue("entryCost"))
				{
					availablePart.SetEntryCost(int.Parse(node.GetValue("entryCost")));
				}
				if (node.HasValue("cost"))
				{
					availablePart.cost = float.Parse(node.GetValue("cost"));
					ShipConstruction.SanitizePartCosts(availablePart, node);
				}
				GameObject gameObject = UnityEngine.Object.Instantiate(partIconPlaceholder);
				gameObject.name = availablePart.name + " icon";
				gameObject.transform.parent = base.transform;
				gameObject.SetActive(value: false);
				availablePart.iconPrefab = gameObject;
				availablePart.iconScale = UnityEngine.Random.Range(0f, 1f);
				availablePart.moduleInfo = "";
				ConfigNode[] nodes = node.GetNodes("MODULE");
				int num2 = nodes.Length;
				for (int j = 0; j < num2; j++)
				{
					ConfigNode configNode2 = nodes[j];
					availablePart.moduleInfo = availablePart.moduleInfo + "<b>" + configNode2.GetValue("name") + "</b>\nWe also assume module infos take up\non average about 4 lines\ntodisplay their data.\nIt can get pretty long.\n";
					AvailablePart.ModuleInfo moduleInfo = new AvailablePart.ModuleInfo();
					moduleInfo.moduleName = KSPUtil.PrintModuleName(configNode2.GetValue("name"));
					moduleInfo.info = "Some <b>test</b> values\nGet <i>Displayed Here</i>\nin <color=orange>multiple lines</color>\nand rich-text formatted";
					availablePart.moduleInfos.Add(moduleInfo);
				}
				availablePart.resourceInfo = "";
				ConfigNode[] nodes2 = node.GetNodes("RESOURCE");
				num2 = nodes2.Length;
				for (int k = 0; k < num2; k++)
				{
					ConfigNode configNode2 = nodes2[k];
					PartResourceDefinition definition = ResourcesLibrary.GetDefinition(configNode2.GetValue("name"));
					availablePart.resourceInfo = availablePart.resourceInfo + "<b>" + definition.name + "</b>\nResource Amounts,\nResource Density\nIt all adds up to one massive tooltip.\n";
					float num3 = float.Parse(configNode2.GetValue("amount"));
					float num4 = float.Parse(configNode2.GetValue("maxAmount"));
					float num5 = definition.unitCost * num3;
					AvailablePart.ResourceInfo resourceInfo = new AvailablePart.ResourceInfo();
					resourceInfo.resourceName = configNode2.GetValue("name");
					resourceInfo.displayName = (configNode2.HasValue("displayName") ? configNode2.GetValue("displayName").LocalizeRemoveGender() : KSPUtil.PrintModuleName(configNode2.GetValue("name")));
					resourceInfo.info = "Amount: " + num3.ToString("F1") + "/" + num4.ToString("F1") + "\nCost: \\F " + num5.ToString("0.00");
					availablePart.resourceInfos.Add(resourceInfo);
				}
				partsList.Add(availablePart);
			}
		}
	}

	public static ScienceExperiment GetExperiment(string experimentID)
	{
		if (experiments == null)
		{
			Debug.LogError("[TestLoader]: experiment definitions not loaded... make sure the ScienceDefs filepath is set right.");
			return null;
		}
		if (experiments.ContainsKey(experimentID))
		{
			return experiments[experimentID];
		}
		Debug.LogError("[TestLoader]: No Experiment definition found with id " + experimentID);
		return null;
	}

	public static string GetResults(string subjectID)
	{
		if (Instance != null)
		{
			string[] array = subjectID.Split('@');
			string experimentID = array[0];
			string text = array[1];
			ScienceExperiment experiment = GetExperiment(experimentID);
			string text2 = "";
			if (experiment.Results.ContainsKey(text))
			{
				return experiment.Results[text];
			}
			List<string> list = new List<string>();
			Dictionary<string, string>.KeyCollection.Enumerator enumerator = experiment.Results.Keys.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				if (text.Contains(current.TrimEnd('*')))
				{
					list.Add(current);
				}
			}
			if (list.Count > 0)
			{
				return experiment.Results[list[UnityEngine.Random.Range(0, list.Count)]];
			}
			if (experiment.Results.ContainsKey("default"))
			{
				return experiment.Results["default"];
			}
			return experiment.experimentTitle + " Data Acquired";
		}
		return "In Sandbox Mode, there is no Science to be done.";
	}

	public static List<string> GetExperimentIDs()
	{
		return new List<string>(experiments.Keys);
	}

	public static List<string> GetSituationTagsDescriptions()
	{
		List<string> list = new List<string>();
		ExperimentSituations[] array = (ExperimentSituations[])Enum.GetValues(typeof(ExperimentSituations));
		for (int i = 0; i < array.Length; i++)
		{
			string item = array[i].Description();
			list.Add(item);
		}
		return list;
	}

	public static List<string> GetSituationTags()
	{
		return new List<string>(Enum.GetNames(typeof(ExperimentSituations)));
	}

	public static List<string> GetBiomeTagsLocalized(CelestialBody cb, bool includeMiniBiomes)
	{
		List<string> list = new List<string>();
		if (!(cb.BiomeMap == null) && cb.BiomeMap.Attributes != null && cb.BiomeMap.Attributes.Length != 0)
		{
			int num = cb.BiomeMap.Attributes.Length;
			for (int i = 0; i < num; i++)
			{
				list.Add(cb.BiomeMap.Attributes[i].displayname.Replace(" ", string.Empty));
			}
			if (includeMiniBiomes)
			{
				list.AddRange(GetMiniBiomeTagsLocalized(cb));
			}
			return list;
		}
		return list;
	}

	public static List<string> GetMiniBiomeTagsLocalized(CelestialBody cb)
	{
		List<string> list = new List<string>();
		if (cb.MiniBiomes != null && cb.MiniBiomes.Length != 0)
		{
			int num = cb.MiniBiomes.Length;
			for (int i = 0; i < num; i++)
			{
				string getDisplayName = cb.MiniBiomes[i].GetDisplayName;
				bool flag = false;
				for (int j = 0; j < list.Count; j++)
				{
					if (getDisplayName == list[j])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(getDisplayName);
				}
			}
			return list;
		}
		return list;
	}

	public static List<string> GetBiomeTags(CelestialBody cb, bool includeMiniBiomes)
	{
		List<string> list = new List<string>();
		if (!(cb.BiomeMap == null) && cb.BiomeMap.Attributes != null && cb.BiomeMap.Attributes.Length != 0)
		{
			int num = cb.BiomeMap.Attributes.Length;
			for (int i = 0; i < num; i++)
			{
				list.Add(cb.BiomeMap.Attributes[i].name.Replace(" ", ""));
			}
			if (includeMiniBiomes)
			{
				list.AddRange(GetMiniBiomeTags(cb));
			}
			return list;
		}
		return list;
	}

	public static List<string> GetMiniBiomeTags(CelestialBody cb)
	{
		List<string> list = new List<string>();
		if (cb.MiniBiomes != null && cb.MiniBiomes.Length != 0)
		{
			int num = cb.MiniBiomes.Length;
			for (int i = 0; i < num; i++)
			{
				string tagKeyID = cb.MiniBiomes[i].TagKeyID;
				bool flag = false;
				for (int j = 0; j < list.Count; j++)
				{
					if (tagKeyID == list[j])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(tagKeyID);
				}
			}
			return list;
		}
		return list;
	}
}
