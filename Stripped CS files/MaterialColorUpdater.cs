using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MaterialColorUpdater
{
	public List<Renderer> renderers = new List<Renderer>();

	public int propertyID;

	public Color setColor = Color.black;

	public MaterialPropertyBlock mpb;

	public MaterialColorUpdater(Transform t, int propertyID, Part part)
	{
		if (!(t == null))
		{
			this.propertyID = propertyID;
			mpb = part.mpb;
			mpb.SetColor(propertyID, Color.black);
			CreateRendererList(t, propertyID, part);
		}
	}

	public MaterialColorUpdater(Transform t, int propertyID)
	{
		if (!(t == null))
		{
			this.propertyID = propertyID;
			mpb = new MaterialPropertyBlock();
			mpb.SetColor(propertyID, Color.black);
			CreateRendererList(t, propertyID, null);
		}
	}

	public void CreateRendererList(Transform t, int propertyID, Part part)
	{
		renderers = new List<Renderer>();
		Renderer[] array;
		if (part != null)
		{
			array = part.FindModelComponents<Renderer>().ToArray();
			if (array.Length == 0)
			{
				array = t.GetComponentsInChildren<Renderer>();
			}
		}
		else
		{
			array = t.GetComponentsInChildren<Renderer>();
		}
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			Renderer item = array[i];
			for (int j = 0; j < array[i].materials.Length; j++)
			{
				if (array[i].materials[j].HasProperty(propertyID))
				{
					renderers.Add(item);
					break;
				}
			}
		}
	}

	public void Update(Color color, bool force = false)
	{
		if (renderers == null || (!force && color == setColor))
		{
			return;
		}
		setColor = color;
		mpb.SetColor(propertyID, setColor);
		int count = renderers.Count;
		while (count-- > 0)
		{
			if (renderers[count] == null)
			{
				renderers.RemoveAt(count);
			}
			else
			{
				renderers[count].SetPropertyBlock(mpb);
			}
		}
	}
}
