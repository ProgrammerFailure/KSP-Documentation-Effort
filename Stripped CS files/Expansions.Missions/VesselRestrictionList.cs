using System;
using System.Collections.Generic;

namespace Expansions.Missions;

public class VesselRestrictionList : DynamicModuleList
{
	public static List<Type> vesselRestrictionTypes;

	public List<VesselRestriction> ActiveRestrictions
	{
		get
		{
			List<VesselRestriction> list = new List<VesselRestriction>();
			for (int i = 0; i < activeModules.Count; i++)
			{
				list.Add(activeModules[i] as VesselRestriction);
			}
			return list;
		}
	}

	public VesselRestrictionList(MENode node)
		: base(node)
	{
	}

	public override List<Type> GetSupportedTypes()
	{
		if (vesselRestrictionTypes == null)
		{
			Type[] types = typeof(VesselRestriction).Assembly.GetTypes();
			Type typeFromHandle = typeof(VesselRestriction);
			vesselRestrictionTypes = new List<Type>();
			for (int i = 0; i < types.Length; i++)
			{
				if (types[i].IsSubclassOf(typeFromHandle))
				{
					VesselRestrictionOptions vesselRestrictionOptions = null;
					object[] customAttributes = types[i].GetCustomAttributes(typeof(VesselRestrictionOptions), inherit: false);
					if (customAttributes != null && customAttributes.Length != 0)
					{
						vesselRestrictionOptions = customAttributes[0] as VesselRestrictionOptions;
					}
					if (vesselRestrictionOptions == null || vesselRestrictionOptions.listedInSAP)
					{
						vesselRestrictionTypes.Add(types[i]);
					}
				}
			}
		}
		return vesselRestrictionTypes;
	}

	public void StartAppEvents()
	{
		List<VesselRestriction> activeRestrictions = ActiveRestrictions;
		for (int i = 0; i < activeRestrictions.Count; i++)
		{
			activeRestrictions[i].SuscribeToEvents();
		}
	}

	public void ClearAppEvents()
	{
		List<VesselRestriction> activeRestrictions = ActiveRestrictions;
		for (int i = 0; i < activeRestrictions.Count; i++)
		{
			activeRestrictions[i].ClearEvents();
		}
	}
}
