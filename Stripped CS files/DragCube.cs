using System;
using UnityEngine;

[Serializable]
public class DragCube
{
	public enum DragFace
	{
		const_0,
		const_1,
		const_2,
		const_3,
		const_4,
		const_5
	}

	[SerializeField]
	public float[] area = new float[6];

	[SerializeField]
	public float[] drag = new float[6];

	[SerializeField]
	public float[] depth = new float[6];

	[SerializeField]
	public float[] dragModifiers = new float[6];

	[SerializeField]
	public Vector3 center;

	[SerializeField]
	public Vector3 size;

	[SerializeField]
	public string name = "";

	[SerializeField]
	public float weight = 1f;

	public float[] Area => area;

	public float[] Drag => drag;

	public float[] Depth => depth;

	public float[] DragModifiers => dragModifiers;

	public Vector3 Center
	{
		get
		{
			return center;
		}
		set
		{
			center = value;
		}
	}

	public Vector3 Size
	{
		get
		{
			return size;
		}
		set
		{
			size = value;
		}
	}

	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
		}
	}

	public float Weight
	{
		get
		{
			return weight;
		}
		set
		{
			weight = value;
		}
	}

	public DragCube()
	{
	}

	public DragCube(string name)
	{
		this.name = name;
	}

	public bool Load(string[] data)
	{
		if (data.Length != 25 && data.Length != 24)
		{
			Debug.LogError("DragCube: Config string does not have correct number of values");
			return false;
		}
		bool flag = false;
		if (data.Length == 12)
		{
			flag = false;
			name = "";
		}
		else
		{
			flag = true;
			name = data[0];
		}
		float result = 0f;
		int num = 0;
		int num2 = (flag ? 1 : 0);
		while (true)
		{
			if (num < 6)
			{
				if (!float.TryParse(data[num2], out result))
				{
					break;
				}
				area[num] = result;
				if (float.TryParse(data[num2 + 1], out result))
				{
					drag[num] = result;
					if (float.TryParse(data[num2 + 2], out result))
					{
						depth[num] = result;
						num++;
						num2 += 3;
						continue;
					}
					Debug.LogError("DragCube: Cannot load, depth value " + num + "'" + data[num2 + 2] + "' cannot be parsed");
					return false;
				}
				Debug.LogError("DragCube: Cannot load, drag value " + num + "'" + data[num2 + 1] + "' cannot be parsed");
				return false;
			}
			center = default(Vector3);
			size = default(Vector3);
			if (float.TryParse(data[num2], out result))
			{
				center.x = result;
				if (float.TryParse(data[num2 + 1], out result))
				{
					center.y = result;
					if (float.TryParse(data[num2 + 2], out result))
					{
						center.z = result;
						if (float.TryParse(data[num2 + 3], out result))
						{
							size.x = result;
							if (float.TryParse(data[num2 + 4], out result))
							{
								size.y = result;
								if (float.TryParse(data[num2 + 5], out result))
								{
									size.z = result;
									return true;
								}
								Debug.LogError("DragCube: Cannot load, bounds size z value '" + data[num2 + 5] + "' cannot be parsed");
								return false;
							}
							Debug.LogError("DragCube: Cannot load, bounds size y value '" + data[num2 + 4] + "' cannot be parsed");
							return false;
						}
						Debug.LogError("DragCube: Cannot load, bounds size x value '" + data[num2 + 3] + "' cannot be parsed");
						return false;
					}
					Debug.LogError("DragCube: Cannot load, bounds z value '" + data[num2 + 2] + "' cannot be parsed");
					return false;
				}
				Debug.LogError("DragCube: Cannot load, bounds y value '" + data[num2 + 1] + "' cannot be parsed");
				return false;
			}
			Debug.LogError("DragCube: Cannot load, bounds x value '" + data[num2] + "' cannot be parsed");
			return false;
		}
		Debug.LogError("DragCube: Cannot load, area value " + num + "'" + data[num2] + "' cannot be parsed");
		return false;
	}

	public string SaveToString()
	{
		string text = string.Empty;
		if (name != string.Empty)
		{
			text += name;
		}
		for (int i = 0; i < 6; i++)
		{
			if (text != string.Empty)
			{
				text += ", ";
			}
			text = text + area[i].ToString("G4") + "," + drag[i].ToString("G4") + "," + depth[i].ToString("G4");
		}
		text = text + ", " + center.x.ToString("G4") + "," + center.y.ToString("G4") + "," + center.z.ToString("G4");
		return text + ", " + size.x.ToString("G4") + "," + size.y.ToString("G4") + "," + size.z.ToString("G4");
	}
}
