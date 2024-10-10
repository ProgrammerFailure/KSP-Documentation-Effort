using System.Collections.Generic;
using UnityEngine;

public class DialogGUILayoutEnd : DialogGUIBase
{
	public override GameObject Create(ref Stack<Transform> layouts, UISkinDef skin)
	{
		layouts.Pop();
		return null;
	}
}
