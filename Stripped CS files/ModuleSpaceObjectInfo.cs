using System;
using System.Collections.Generic;

public class ModuleSpaceObjectInfo : PartModule
{
	public class ResourceData
	{
		public string Name;

		public float Mass;

		public float Purity;
	}

	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_462423")]
	public string displayMass = "???";

	[KSPField(isPersistant = true)]
	public string massThreshold = "0";

	[KSPField(isPersistant = true)]
	public string currentMass = "0";

	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001746")]
	public string resources = "???";

	public float totalResourceMass;

	public double totalResourcePercent;

	public virtual double currentMassVal
	{
		get
		{
			double result = 0.0;
			double.TryParse(currentMass, out result);
			return result;
		}
		set
		{
			currentMass = value.ToString("G17");
		}
	}

	public virtual double massThresholdVal
	{
		get
		{
			double result = 0.0;
			double.TryParse(massThreshold, out result);
			return result;
		}
		set
		{
			massThreshold = value.ToString("G17");
		}
	}

	public void SetupSpaceObjectResources(List<ModuleSpaceObjectResource> resInfoList)
	{
		if (!HighLogic.LoadedSceneIsFlight)
		{
			return;
		}
		KSPRandom kSPRandom = new KSPRandom();
		List<ResourceData> list = new List<ResourceData>();
		int count = resInfoList.Count;
		for (int i = 0; i < count; i++)
		{
			ModuleSpaceObjectResource moduleSpaceObjectResource = resInfoList[i];
			if (kSPRandom.Next(100) < moduleSpaceObjectResource.presenceChance)
			{
				double minValue = (double)moduleSpaceObjectResource.lowRange * 0.01;
				double maxValue = (double)moduleSpaceObjectResource.highRange * 0.01;
				double num = kSPRandom.NextDouble(minValue, maxValue);
				if (totalResourcePercent + num > 1.0)
				{
					num = 1.0 - totalResourcePercent;
				}
				totalResourcePercent += num;
				ResourceData resourceData = new ResourceData
				{
					Name = moduleSpaceObjectResource.resourceName,
					Mass = (float)(currentMassVal * num)
				};
				resourceData.Purity = (float)kSPRandom.Next(moduleSpaceObjectResource.lowRange, moduleSpaceObjectResource.highRange) * 0.01f;
				list.Add(resourceData);
				totalResourceMass += resourceData.Mass;
			}
		}
		int count2 = list.Count;
		for (int j = 0; j < count; j++)
		{
			ModuleSpaceObjectResource moduleSpaceObjectResource2 = resInfoList[j];
			for (int k = 0; k < count2; k++)
			{
				ResourceData resourceData2 = list[k];
				if (resourceData2.Name == moduleSpaceObjectResource2.resourceName)
				{
					float val = resourceData2.Mass / totalResourceMass;
					moduleSpaceObjectResource2.abundance = Math.Max(0.01f, val) * resourceData2.Purity;
					moduleSpaceObjectResource2.displayAbundance = moduleSpaceObjectResource2.abundance;
				}
			}
		}
		if ((double)totalResourceMass > currentMassVal)
		{
			currentMassVal = totalResourceMass * 1.01f;
		}
		massThresholdVal = currentMassVal - (double)totalResourceMass;
	}
}
