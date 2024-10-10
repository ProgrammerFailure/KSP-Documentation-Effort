using System;
using UnityEngine;

namespace Expansions.Missions.Flow;

public class MEFlowUIGroup_Or : MonoBehaviour
{
	[SerializeField]
	public Transform ChildHolder;

	public static UnityEngine.Object groupPrefab;

	public MEFlowOrBlock orBlock;

	public MEFlowParser parser;

	public static MEFlowUIGroup_Or Create(MEFlowOrBlock orBlock, MEFlowParser parser)
	{
		GameObject obj = (GameObject)UnityEngine.Object.Instantiate(groupPrefab);
		obj.transform.localPosition = Vector3.zero;
		MEFlowUIGroup_Or component = obj.GetComponent<MEFlowUIGroup_Or>();
		component.orBlock = orBlock;
		orBlock.OnUpdateFlowUI = (Callback<MEFlowParser>)Delegate.Combine(orBlock.OnUpdateFlowUI, new Callback<MEFlowParser>(component.OnUpdateFlowUI));
		component.parser = parser;
		return component;
	}

	public void OnDestroy()
	{
		MEFlowOrBlock mEFlowOrBlock = orBlock;
		mEFlowOrBlock.OnUpdateFlowUI = (Callback<MEFlowParser>)Delegate.Remove(mEFlowOrBlock.OnUpdateFlowUI, new Callback<MEFlowParser>(OnUpdateFlowUI));
	}

	public void OnUpdateFlowUI(MEFlowParser parser)
	{
		bool num = parser.showNonObjectives || orBlock.HasObjectives;
		bool flag = !parser.showEvents && !orBlock.HasVisibleChildren;
		bool flag2 = num && !flag;
		if (base.gameObject.activeSelf != flag2)
		{
			base.gameObject.SetActive(flag2);
		}
	}
}
