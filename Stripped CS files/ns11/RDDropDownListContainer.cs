using System;
using ns2;
using UnityEngine;

namespace ns11;

public class RDDropDownListContainer : MonoBehaviour
{
	public UIList scrollList;

	public RDDropDownList dropDownListPrefab;

	[NonSerialized]
	public RDDropDownList[] lists;

	public void Initialize(params RDDropDownList[] lists)
	{
		this.lists = lists;
		scrollList.Clear(destroyElements: true);
		int num = lists.Length;
		for (int i = 0; i < num; i++)
		{
			RDDropDownList rDDropDownList = lists[i];
			rDDropDownList.Open(force: true, callback: false);
			scrollList.AddItem(rDDropDownList.GetComponent<UIListItem>());
		}
	}
}
