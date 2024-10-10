using System;
using UnityEngine;

public class UI_EnumAttribute : PropertyAttribute
{
	public readonly Type objectType;

	public readonly int bitoffset;

	public UI_EnumAttribute(Type objectType, int bitoffset)
	{
		this.objectType = objectType;
		this.bitoffset = bitoffset;
	}
}
