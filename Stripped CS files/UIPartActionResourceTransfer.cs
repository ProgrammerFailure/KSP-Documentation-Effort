using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPartActionResourceTransfer : UIPartActionResourceItem
{
	public enum FlowState
	{
		None,
		Out,
		In
	}

	public Button flowInBtn;

	public Button flowOutBtn;

	public Button flowStopBtn;

	[SerializeField]
	public List<UIPartActionResourceTransfer> targets;

	public static bool transferRequiresFullControl;

	public double lastUT;

	public FlowState state;

	public double transferFraction;

	public List<UIPartActionResourceTransfer> Targets
	{
		get
		{
			return targets;
		}
		set
		{
			targets = value;
		}
	}

	public FlowState State => state;

	public void Awake()
	{
		flowInBtn.onClick.AddListener(OnBtnIn);
		flowOutBtn.onClick.AddListener(OnBtnOut);
		flowStopBtn.onClick.AddListener(OnBtnStop);
		targets = new List<UIPartActionResourceTransfer>();
	}

	public void OnDestroy()
	{
		resource = null;
		targets = null;
	}

	public override void Setup(UIPartActionWindow window, Part part, UI_Scene scene, UI_Control control, PartResource resource)
	{
		base.Setup(window, part, scene, control, resource);
		state = FlowState.None;
		base.resource = resource;
		targets.Clear();
		flowInBtn.gameObject.SetActive(value: true);
		flowOutBtn.gameObject.SetActive(value: true);
		flowStopBtn.gameObject.SetActive(value: false);
	}

	public override bool IsItemValid()
	{
		if (!(part == null) && part.State != PartStates.DEAD && resource != null)
		{
			int count = targets.Count;
			do
			{
				if (count-- <= 0)
				{
					if (part.vessel != FlightGlobals.ActiveVessel)
					{
						return false;
					}
					if (window.Display == UIPartActionWindow.DisplayType.ResourceOnly && !UIPartActionController.Instance.resourcesShown.Contains(resource.info.id))
					{
						return false;
					}
					if (!UIPartActionController.Instance.ShowTransfers(resource))
					{
						return false;
					}
					return true;
				}
			}
			while (!(targets[count] == null));
			return false;
		}
		return false;
	}

	public override void UpdateItem()
	{
		double universalTime = Planetarium.GetUniversalTime();
		if (state == FlowState.In)
		{
			bool flag;
			if (!(flag = resource.amount >= resource.maxAmount || transferFraction == 0.0))
			{
				flag = true;
				int count = targets.Count;
				while (count-- > 0)
				{
					if (targets[count].gameObject.activeInHierarchy && targets[count].state != 0)
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				FlowStop();
			}
			else
			{
				double num = resource.maxAmount / 20.0 / transferFraction * Math.Round(universalTime - lastUT, 2);
				int count2 = targets.Count;
				for (int i = 0; i < count2; i++)
				{
					UIPartActionResourceTransfer uIPartActionResourceTransfer = targets[i];
					if (uIPartActionResourceTransfer.gameObject.activeInHierarchy && uIPartActionResourceTransfer.State == FlowState.Out)
					{
						double val = num;
						val = Math.Min(val, uIPartActionResourceTransfer.resource.amount);
						val = Math.Min(val, resource.maxAmount - resource.amount);
						resource.part.TransferResource(resource, val, uIPartActionResourceTransfer.part);
						uIPartActionResourceTransfer.resource.part.TransferResource(uIPartActionResourceTransfer.resource, 0.0 - val, resource.part);
					}
				}
			}
		}
		else if (state == FlowState.Out && resource.amount <= 0.0)
		{
			FlowStop();
		}
		lastUT = universalTime;
	}

	public void FlowIn(double fraction)
	{
		if (base.gameObject.activeInHierarchy && state != FlowState.In)
		{
			state = FlowState.In;
			transferFraction = fraction;
			lastUT = Planetarium.GetUniversalTime();
			flowInBtn.gameObject.SetActive(value: false);
			flowOutBtn.gameObject.SetActive(value: false);
			flowStopBtn.gameObject.SetActive(value: true);
		}
	}

	public void FlowOut()
	{
		if (base.gameObject.activeInHierarchy && state != FlowState.Out)
		{
			state = FlowState.Out;
			flowInBtn.gameObject.SetActive(value: false);
			flowOutBtn.gameObject.SetActive(value: false);
			flowStopBtn.gameObject.SetActive(value: true);
		}
	}

	public void FlowStop()
	{
		if (state != 0)
		{
			state = FlowState.None;
			flowInBtn.gameObject.SetActive(value: true);
			flowOutBtn.gameObject.SetActive(value: true);
			flowStopBtn.gameObject.SetActive(value: false);
		}
	}

	public void OnBtnIn()
	{
		if (!InputLockManager.IsUnlocked(transferRequiresFullControl ? ControlTypes.TWEAKABLES_FULLONLY : ControlTypes.TWEAKABLES_ANYCONTROL))
		{
			return;
		}
		double num = 0.0;
		int count = targets.Count;
		while (count-- > 0)
		{
			if (targets[count].gameObject.activeInHierarchy)
			{
				if (targets[count].resource.amount > 0.0)
				{
					num += 1.0;
				}
				targets[count].FlowOut();
			}
		}
		FlowIn(num);
	}

	public void OnBtnOut()
	{
		if (!InputLockManager.IsUnlocked(transferRequiresFullControl ? ControlTypes.TWEAKABLES_FULLONLY : ControlTypes.TWEAKABLES_ANYCONTROL))
		{
			return;
		}
		FlowOut();
		double num = 0.0;
		int num2 = targets.Count - 1;
		for (int num3 = num2; num3 >= 0; num3--)
		{
			if (targets[num3].gameObject.activeInHierarchy && targets[num3].resource.amount < targets[num3].resource.maxAmount)
			{
				num += 1.0;
			}
		}
		for (int num4 = num2; num4 >= 0; num4--)
		{
			targets[num4].FlowIn(num);
		}
	}

	public void OnBtnStop()
	{
		if (!InputLockManager.IsUnlocked(transferRequiresFullControl ? ControlTypes.TWEAKABLES_FULLONLY : ControlTypes.TWEAKABLES_ANYCONTROL))
		{
			return;
		}
		FlowStop();
		int count = targets.Count;
		while (count-- > 0)
		{
			if (targets[count].gameObject.activeInHierarchy)
			{
				targets[count].FlowStop();
			}
		}
	}
}
