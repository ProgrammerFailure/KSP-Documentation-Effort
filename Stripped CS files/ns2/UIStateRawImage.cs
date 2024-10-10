using System;
using UnityEngine;
using UnityEngine.UI;

namespace ns2;

public class UIStateRawImage : MonoBehaviour
{
	[Serializable]
	public class ImageState
	{
		public string name;

		public Texture texture;
	}

	public RawImage image;

	public ImageState[] states = new ImageState[0];

	public string startState = "";

	public void Reset()
	{
		image = GetComponent<RawImage>();
	}

	public void Awake()
	{
		SetState(startState);
	}

	public void SetState(int index)
	{
		if (index >= 0 && index < states.Length && image != null)
		{
			image.texture = states[index].texture;
		}
	}

	public void SetState(string name)
	{
		int num = states.Length;
		while (num-- > 0)
		{
			if (states[num].name == name)
			{
				SetState(num);
			}
		}
	}
}
