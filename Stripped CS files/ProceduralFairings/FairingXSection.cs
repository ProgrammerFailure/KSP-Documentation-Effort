using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralFairings;

[Serializable]
public class FairingXSection : IConfigNode, IComparable<FairingXSection>
{
	public float h;

	public float r;

	public bool isLast;

	public bool isCap;

	public bool isValid;

	public Color color;

	[NonSerialized]
	public FairingXSection lerp1;

	[NonSerialized]
	public FairingXSection lerp2;

	public List<FairingPanelFlags> fairingPanelFlags;

	public bool isLerp;

	public bool IsLerp => isLerp;

	public FairingXSection()
	{
		isLerp = false;
	}

	public FairingXSection(bool isCap)
	{
		this.isCap = isCap;
	}

	public FairingXSection(FairingXSection from, FairingXSection to)
	{
		lerp1 = from;
		lerp2 = to;
		isLerp = true;
	}

	public FairingXSection(FairingXSection cloneOf)
	{
		h = cloneOf.h;
		r = cloneOf.r;
		isCap = cloneOf.isCap;
		isLast = cloneOf.isLast;
		isValid = cloneOf.isValid;
		color = cloneOf.color;
		isLerp = cloneOf.isLerp;
		lerp1 = cloneOf.lerp1;
		lerp2 = cloneOf.lerp2;
	}

	public bool AddAttachedFlag(int panelIndex, uint flagPartID)
	{
		if (fairingPanelFlags == null)
		{
			fairingPanelFlags = new List<FairingPanelFlags>();
		}
		bool result = false;
		for (int i = 0; i < fairingPanelFlags.Count; i++)
		{
			if (fairingPanelFlags[i].panelIndex == panelIndex)
			{
				result = true;
				fairingPanelFlags[i].attachedFlagsPartIds.AddUnique(flagPartID);
				break;
			}
		}
		return result;
	}

	public void AddNewFairingPanel(int panelIndex)
	{
		if (this.fairingPanelFlags == null)
		{
			this.fairingPanelFlags = new List<FairingPanelFlags>();
		}
		else
		{
			for (int i = 0; i < this.fairingPanelFlags.Count; i++)
			{
				if (this.fairingPanelFlags[i].panelIndex == panelIndex)
				{
					return;
				}
			}
		}
		FairingPanelFlags fairingPanelFlags = new FairingPanelFlags();
		fairingPanelFlags.panelIndex = panelIndex;
		this.fairingPanelFlags.Add(fairingPanelFlags);
	}

	public int GetPanelIndexFromFlag(uint FlagID, uint placementID)
	{
		if (fairingPanelFlags == null)
		{
			return -1;
		}
		int num = 0;
		while (true)
		{
			if (num < fairingPanelFlags.Count)
			{
				if ((fairingPanelFlags[num].attachedFlagsPartIds != null && fairingPanelFlags[num].attachedFlagsPartIds.Contains(FlagID)) || fairingPanelFlags[num].attachedFlagsPartIds.Contains(placementID))
				{
					break;
				}
				num++;
				continue;
			}
			return -1;
		}
		return fairingPanelFlags[num].panelIndex;
	}

	public void RemoveAttachedFlag(uint FlagID)
	{
		if (fairingPanelFlags != null)
		{
			for (int i = 0; i < fairingPanelFlags.Count; i++)
			{
				fairingPanelFlags[i].attachedFlagsPartIds.Remove(FlagID);
			}
		}
	}

	public void Load(ConfigNode node)
	{
		if (node.HasValue("h"))
		{
			h = float.Parse(node.GetValue("h"));
		}
		if (node.HasValue("r"))
		{
			r = float.Parse(node.GetValue("r"));
		}
		fairingPanelFlags = new List<FairingPanelFlags>();
		ConfigNode[] nodes = node.GetNodes("ATTACHEDFLAG");
		if (nodes == null || nodes.Length == 0)
		{
			return;
		}
		for (int i = 0; i < nodes.Length; i++)
		{
			string value = "";
			int result = 0;
			nodes[i].TryGetValue("panelIndex", ref value);
			if (!int.TryParse(value, out result))
			{
				continue;
			}
			FairingPanelFlags item = new FairingPanelFlags();
			fairingPanelFlags.Add(item);
			fairingPanelFlags[fairingPanelFlags.Count - 1].panelIndex = result;
			string value2 = string.Empty;
			if (!nodes[i].TryGetValue("attachedFlagPartIDs", ref value2))
			{
				continue;
			}
			string[] array = value2.Split(',');
			for (int j = 0; j < array.Length; j++)
			{
				uint result2 = 0u;
				if (uint.TryParse(array[j], out result2))
				{
					fairingPanelFlags[fairingPanelFlags.Count - 1].attachedFlagsPartIds.Add(result2);
				}
			}
		}
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("h", h);
		node.AddValue("r", r);
		if (fairingPanelFlags == null)
		{
			return;
		}
		for (int i = 0; i < fairingPanelFlags.Count; i++)
		{
			ConfigNode configNode = node.AddNode("ATTACHEDFLAG");
			string text = string.Empty;
			bool flag = false;
			for (int j = 0; j < fairingPanelFlags[i].attachedFlagsPartIds.Count; j++)
			{
				if (flag)
				{
					text += ",";
				}
				text += fairingPanelFlags[i].attachedFlagsPartIds[j];
				flag = true;
			}
			configNode.AddValue("attachedFlagPartIDs", text);
			configNode.AddValue("panelIndex", fairingPanelFlags[i].panelIndex);
		}
	}

	public void UpdateLerp(float t, float y)
	{
		if (isLerp)
		{
			h = Mathf.Lerp(lerp1.h, lerp2.h, t);
			r = Mathf.Lerp(lerp1.r, lerp2.r, t) + y;
			color = Color.Lerp(lerp1.color, lerp2.color, t);
		}
	}

	public static float GetSlopeAngle(FairingXSection from, FairingXSection to)
	{
		return Mathf.Atan((to.h - from.h) / (from.r - to.r));
	}

	public static float CircleCast(FairingXSection xs, Vector3 wAxis, Vector3 wPivot, Vector3 wRadial, int nRays, float rLength, int layerMask, out float lVariance, out RaycastHit hit)
	{
		nRays = Mathf.Max(nRays, 2);
		float num = 360f / Mathf.Max(nRays, 2f);
		float num2 = 0f;
		float num3 = float.MaxValue;
		lVariance = 0f;
		hit = default(RaycastHit);
		for (int i = 0; i < nRays; i++)
		{
			Vector3 vector = Quaternion.AngleAxis(num * (float)i, wAxis) * wRadial;
			if (Physics.Raycast(new Ray(wPivot + wAxis * xs.h + vector * rLength, -vector), out hit, rLength, layerMask))
			{
				num2 = Mathf.Max(rLength - hit.distance, num2);
				num3 = Mathf.Min(rLength - hit.distance, num3);
			}
		}
		lVariance = num2 - num3;
		return num2;
	}

	public static bool ConeCast(FairingXSection xsFrom, FairingXSection xsTo, Vector3 wAxis, Vector3 wPivot, Vector3 wRadial, float radiusOffset, int nRays, int layerMask, out float hitLengthScalar, float aOffset = 0f)
	{
		nRays = Mathf.Max(nRays, 2);
		float num = 360f / Mathf.Max(nRays, 2f);
		hitLengthScalar = 1f;
		int num2 = 0;
		float magnitude;
		RaycastHit hitInfo;
		while (true)
		{
			if (num2 < nRays)
			{
				Vector3 vector = Quaternion.AngleAxis(num * (float)num2 + aOffset, wAxis) * wRadial;
				Vector3 vector2 = wPivot + wAxis * xsFrom.h + vector * (xsFrom.r + radiusOffset);
				Vector3 vector3 = wPivot + wAxis * xsTo.h + vector * (xsTo.r + radiusOffset);
				Ray ray = new Ray(vector3, vector2 - vector3);
				magnitude = (vector3 - vector2).magnitude;
				if (Physics.Raycast(ray, out hitInfo, magnitude, layerMask))
				{
					break;
				}
				num2++;
				continue;
			}
			return false;
		}
		hitLengthScalar = hitInfo.distance / magnitude;
		return true;
	}

	public int CompareTo(FairingXSection b)
	{
		return h.CompareTo(b.h);
	}
}
