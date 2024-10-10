using ns9;

public class ProtoPartResourceSnapshot
{
	public ConfigNode resourceValues;

	public string resourceName;

	public PartResource resourceRef;

	public double amount;

	public double maxAmount;

	public bool flowState;

	public PartResourceDefinition definition { get; set; }

	public ProtoPartResourceSnapshot(PartResource resource)
	{
		resourceRef = resource;
		resourceName = resource.info.name;
		resourceValues = new ConfigNode("RESOURCE");
		resource.Save(resourceValues);
		amount = resource.amount;
		maxAmount = resource.maxAmount;
		flowState = resource.flowState;
		definition = PartResourceLibrary.Instance.GetDefinition(resourceName);
	}

	public ProtoPartResourceSnapshot(ConfigNode node)
	{
		resourceValues = new ConfigNode("RESOURCE");
		node.CopyTo(resourceValues);
		string text = null;
		if ((text = node.GetValue("name")) != null)
		{
			resourceName = text;
		}
		if ((text = node.GetValue("amount")) != null)
		{
			amount = double.Parse(text);
		}
		if ((text = node.GetValue("maxAmount")) != null)
		{
			maxAmount = double.Parse(text);
		}
		if ((text = node.GetValue("flowState")) != null)
		{
			flowState = bool.Parse(text);
		}
		if (!string.IsNullOrEmpty(resourceName))
		{
			definition = PartResourceLibrary.Instance.GetDefinition(resourceName);
		}
	}

	public void Save(ConfigNode node)
	{
		resourceValues.CopyTo(node);
		node.SetValue("flowState", flowState, createIfNotFound: true);
		node.SetValue("amount", amount, createIfNotFound: true);
		node.SetValue("maxAmount", maxAmount, createIfNotFound: true);
	}

	public void UpdateConfigNodeAmounts()
	{
		resourceValues.SetValue("amount", amount, createIfNotFound: true);
		resourceValues.SetValue("maxAmount", maxAmount, createIfNotFound: true);
	}

	public void Load(Part hostPart)
	{
		resourceValues.SetValue("flowState", flowState, createIfNotFound: true);
		resourceValues.SetValue("amount", amount, createIfNotFound: true);
		resourceValues.SetValue("maxAmount", maxAmount, createIfNotFound: true);
		hostPart.SetResource(resourceValues);
	}

	public AvailablePart.ResourceInfo GetCurrentResourceInfo()
	{
		AvailablePart.ResourceInfo resourceInfo = new AvailablePart.ResourceInfo();
		resourceInfo.resourceName = resourceName;
		if (resourceRef != null)
		{
			resourceInfo.displayName = resourceRef.info.displayName.LocalizeRemoveGender();
			resourceInfo.info = Localizer.Format("#autoLOC_166269", amount.ToString("F1") + " / " + maxAmount.ToString("F1")) + Localizer.Format("#autoLOC_166270", (amount * (double)resourceRef.info.density).ToString("F2") + Localizer.Format("#autoLOC_7001407"));
		}
		else
		{
			PartResourceDefinition partResourceDefinition = PartResourceLibrary.Instance?.GetDefinition(resourceName);
			resourceInfo.displayName = resourceInfo.resourceName;
			resourceInfo.info = Localizer.Format("#autoLOC_166269", amount.ToString("F1") + " / " + maxAmount.ToString("F1"));
			if (partResourceDefinition != null)
			{
				resourceInfo.info += Localizer.Format("#autoLOC_166270", (amount * (double)partResourceDefinition.density).ToString("F2") + Localizer.Format("#autoLOC_7001407"));
			}
		}
		return resourceInfo;
	}
}
