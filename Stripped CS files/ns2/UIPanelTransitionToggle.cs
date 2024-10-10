using UnityEngine;

namespace ns2;

public class UIPanelTransitionToggle : UIPanelTransition
{
	[SerializeField]
	public int expandedIndex;

	[SerializeField]
	public int collapsedIndex = 1;

	public bool expanded => base.StateIndex == expandedIndex;

	public bool collapsed => base.StateIndex == collapsedIndex;

	public void OnValidate()
	{
		if (states.Length > 2)
		{
			Debug.LogError("[UIPanelTransitionToggle]: Invalid Setup on " + base.name + "! Toggle type panels can only have 2 states!", base.gameObject);
		}
	}

	public void Expand()
	{
		Transition(expandedIndex);
	}

	public void Collapse()
	{
		Transition(collapsedIndex);
	}

	public void ExpandImmediate()
	{
		TransitionImmediate(expandedIndex);
	}

	public void CollapseImmediate()
	{
		TransitionImmediate(collapsedIndex);
	}
}
