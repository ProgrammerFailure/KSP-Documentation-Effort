using System.Collections.Generic;
using ns11;
using ns9;
using UnityEngine;

public class TutorialEditorAdvanced : TutorialScenario
{
	public string stateName = "welcome";

	public bool complete;

	public string vesselName;

	public Texture2D CoMicon;

	public EditorPartListFilter<AvailablePart> tutorialFilter_none = new EditorPartListFilter<AvailablePart>("TutorialEditor_none", (AvailablePart a) => false);

	public EditorPartListFilter<AvailablePart> tutorialFilterAdv = new EditorPartListFilter<AvailablePart>("TutorialEditorMid", (AvailablePart a) => a.name == "mk1pod" || a.name == "mk1pod.v2" || a.name == "parachuteSingle" || a.name == "Decoupler.1" || a.name == "RCSFuelTank" || a.name == "fuelTankSmallFlat" || a.name == "fuelTank" || a.name == "liquidEngine3" || a.name == "liquidEngine3.v2" || a.name == "RCSBlock" || a.name == "RCSBlock.v2" || a.name == "solarPanels5" || a.name == "batteryPack" || a.name == "liquidEngine2" || a.name == "liquidEngine2.v2" || a.name == "radialDecoupler" || a.name == "solidBooster" || a.name == "solidBooster.v2" || a.name == "noseCone" || a.name == "R8winglet");

	public override void OnAssetSetup()
	{
		instructorPrefabName = "Instructor_Wernher";
		SetDialogRect(new Rect(CalcDialogXRatio(), 0.85f, 420f, 190f));
		CoMicon = GameDatabase.Instance.GetTexture("Squad/Tutorials/EditorCoM", asNormalMap: false);
		base.OnAssetSetup();
	}

	public bool AttachedNum(int n)
	{
		if (EditorLogic.fetch.AreAllPartsConnected())
		{
			return EditorLogic.fetch.CountAllSceneParts(includeSelected: false) == n;
		}
		return false;
	}

	public bool HasShip()
	{
		if (EditorLogic.fetch.ship != null)
		{
			return EditorLogic.RootPart != null;
		}
		return false;
	}

	public bool CheckPod()
	{
		if (HasShip())
		{
			if (!(EditorLogic.RootPart.partInfo.name == "mk1pod"))
			{
				return EditorLogic.RootPart.partInfo.name == "mk1pod.v2";
			}
			return true;
		}
		return false;
	}

	public bool CheckChute()
	{
		if (!CheckPod())
		{
			return false;
		}
		Part attachedPart = EditorLogic.RootPart.FindAttachNode("top").attachedPart;
		if (attachedPart != null)
		{
			return attachedPart.partInfo.name == "parachuteSingle";
		}
		return false;
	}

	public bool CheckChuteStats()
	{
		if (!CheckChute())
		{
			return false;
		}
		ModuleParachute module = EditorLogic.RootPart.FindAttachNode("top").attachedPart.Modules.GetModule<ModuleParachute>();
		if (module == null)
		{
			return false;
		}
		if (module.deployAltitude >= 999f)
		{
			return module.minAirPressureToOpen >= 0.72f;
		}
		return false;
	}

	public bool CheckDecouplerPod()
	{
		if (!CheckChute())
		{
			return false;
		}
		Part attachedPart = EditorLogic.RootPart.FindAttachNode("bottom").attachedPart;
		if (attachedPart != null && attachedPart.partInfo.name == "Decoupler.1")
		{
			return attachedPart.FindAttachNodeByPart(EditorLogic.RootPart).id == "top";
		}
		return false;
	}

	public bool CheckRCSTank()
	{
		if (!CheckChute())
		{
			return false;
		}
		Part attachedPart = EditorLogic.RootPart.FindAttachNode("bottom").attachedPart;
		if (attachedPart != null && attachedPart.partInfo.name == "Decoupler.1" && attachedPart.FindAttachNodeByPart(EditorLogic.RootPart).id == "top")
		{
			Part attachedPart2 = attachedPart.FindAttachNode("bottom").attachedPart;
			if (attachedPart2 != null)
			{
				return attachedPart2.partInfo.name == "RCSFuelTank";
			}
			return false;
		}
		return false;
	}

	public bool CheckRCSTankLoading()
	{
		if (!CheckChute())
		{
			return false;
		}
		Part attachedPart = EditorLogic.RootPart.FindAttachNode("bottom").attachedPart;
		if (attachedPart != null && attachedPart.partInfo.name == "Decoupler.1" && attachedPart.FindAttachNodeByPart(EditorLogic.RootPart).id == "top")
		{
			Part attachedPart2 = attachedPart.FindAttachNode("bottom").attachedPart;
			if (attachedPart2 != null && attachedPart2.partInfo.name == "RCSFuelTank" && attachedPart2.Resources["MonoPropellant"].amount < 105.0)
			{
				return attachedPart2.Resources["MonoPropellant"].amount > 95.0;
			}
			return false;
		}
		return false;
	}

	public bool CheckTanks()
	{
		if (!CheckChute())
		{
			return false;
		}
		Part attachedPart = EditorLogic.RootPart.FindAttachNode("bottom").attachedPart;
		if (!(attachedPart == null) && !(attachedPart.partInfo.name != "Decoupler.1") && !(attachedPart.FindAttachNodeByPart(EditorLogic.RootPart).id != "top"))
		{
			Part attachedPart2 = attachedPart.FindAttachNode("bottom").attachedPart;
			if (!(attachedPart2 == null) && !(attachedPart2.partInfo.name != "RCSFuelTank") && !(attachedPart2.Resources["MonoPropellant"].amount > 102.0) && attachedPart2.Resources["MonoPropellant"].amount >= 90.0)
			{
				if (attachedPart2.children.Count != 1)
				{
					return false;
				}
				Part part = attachedPart2.children[0];
				if (!(part == null) && !(part.partInfo.name != "fuelTankSmallFlat") && attachedPart2.FindAttachNodeByPart(part) != null)
				{
					Part part2 = null;
					int count = part.children.Count;
					do
					{
						if (count-- > 0)
						{
							part2 = part.children[count];
							continue;
						}
						return false;
					}
					while (!(part2.partInfo.name == "fuelTank") || part.FindAttachNodeByPart(part2) == null);
					return true;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public bool CheckEngineUpper()
	{
		if (!CheckChute())
		{
			return false;
		}
		Part attachedPart = EditorLogic.RootPart.FindAttachNode("bottom").attachedPart;
		if (!(attachedPart == null) && !(attachedPart.partInfo.name != "Decoupler.1") && !(attachedPart.FindAttachNodeByPart(EditorLogic.RootPart).id != "top"))
		{
			Part attachedPart2 = attachedPart.FindAttachNode("bottom").attachedPart;
			if (!(attachedPart2 == null) && !(attachedPart2.partInfo.name != "RCSFuelTank") && !(attachedPart2.Resources["MonoPropellant"].amount > 102.0) && attachedPart2.Resources["MonoPropellant"].amount >= 90.0)
			{
				if (attachedPart2.children.Count != 1)
				{
					return false;
				}
				Part part = attachedPart2.children[0];
				if (!(part == null) && !(part.partInfo.name != "fuelTankSmallFlat") && attachedPart2.FindAttachNodeByPart(part) != null)
				{
					Part part2 = null;
					bool flag = true;
					int count = part.children.Count;
					while (count-- > 0)
					{
						part2 = part.children[count];
						if (part2.partInfo.name == "fuelTank" && part.FindAttachNodeByPart(part2) != null)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return false;
					}
					if (part2.children.Count != 1)
					{
						return false;
					}
					Part part3 = part2.children[0];
					if ((part3.partInfo.name != "liquidEngine3" && part3.partInfo.name != "liquidEngine3.v2") || part3.FindAttachNodeByPart(part2).id != "top")
					{
						return false;
					}
					return true;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public bool CheckTanksDry()
	{
		if (!CheckChute())
		{
			return false;
		}
		Part attachedPart = EditorLogic.RootPart.FindAttachNode("bottom").attachedPart;
		if (!(attachedPart == null) && !(attachedPart.partInfo.name != "Decoupler.1") && !(attachedPart.FindAttachNodeByPart(EditorLogic.RootPart).id != "top"))
		{
			Part attachedPart2 = attachedPart.FindAttachNode("bottom").attachedPart;
			if (!(attachedPart2 == null) && !(attachedPart2.partInfo.name != "RCSFuelTank") && !(attachedPart2.Resources["MonoPropellant"].amount > 102.0) && attachedPart2.Resources["MonoPropellant"].amount >= 90.0)
			{
				if (attachedPart2.children.Count != 1)
				{
					return false;
				}
				Part part = attachedPart2.children[0];
				if (!(part == null) && !(part.partInfo.name != "fuelTankSmallFlat") && attachedPart2.FindAttachNodeByPart(part) != null)
				{
					Part part2 = null;
					bool flag = true;
					int count = part.children.Count;
					while (count-- > 0)
					{
						part2 = part.children[count];
						if (part2.partInfo.name == "fuelTank" && part.FindAttachNodeByPart(part2) != null)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return false;
					}
					if (part.Resources["LiquidFuel"].amount + part.Resources["Oxidizer"].amount + part2.Resources["LiquidFuel"].amount + part2.Resources["Oxidizer"].amount > 0.5)
					{
						return false;
					}
					if (part2.children.Count != 1)
					{
						return false;
					}
					Part part3 = part2.children[0];
					if ((part3.partInfo.name != "liquidEngine3" && part3.partInfo.name != "liquidEngine3.v2") || part3.FindAttachNodeByPart(part2).id != "top")
					{
						return false;
					}
					return true;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public bool CheckTanksWet()
	{
		if (!CheckChute())
		{
			return false;
		}
		Part attachedPart = EditorLogic.RootPart.FindAttachNode("bottom").attachedPart;
		if (!(attachedPart == null) && !(attachedPart.partInfo.name != "Decoupler.1") && !(attachedPart.FindAttachNodeByPart(EditorLogic.RootPart).id != "top"))
		{
			Part attachedPart2 = attachedPart.FindAttachNode("bottom").attachedPart;
			if (!(attachedPart2 == null) && !(attachedPart2.partInfo.name != "RCSFuelTank") && !(attachedPart2.Resources["MonoPropellant"].amount > 102.0) && attachedPart2.Resources["MonoPropellant"].amount >= 90.0)
			{
				if (attachedPart2.children.Count != 1)
				{
					return false;
				}
				Part part = attachedPart2.children[0];
				if (!(part == null) && !(part.partInfo.name != "fuelTankSmallFlat") && attachedPart2.FindAttachNodeByPart(part) != null)
				{
					Part part2 = null;
					bool flag = true;
					int count = part.children.Count;
					while (count-- > 0)
					{
						part2 = part.children[count];
						if (part2.partInfo.name == "fuelTank" && part.FindAttachNodeByPart(part2) != null)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return false;
					}
					if (part.Resources["LiquidFuel"].amount + part.Resources["Oxidizer"].amount + part2.Resources["LiquidFuel"].amount + part2.Resources["Oxidizer"].amount < 499.0)
					{
						return false;
					}
					if (part2.children.Count != 1)
					{
						return false;
					}
					Part part3 = part2.children[0];
					if ((part3.partInfo.name != "liquidEngine3" && part3.partInfo.name != "liquidEngine3.v2") || part3.FindAttachNodeByPart(part2).id != "top")
					{
						return false;
					}
					return true;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public bool CheckRCSThrusters()
	{
		if (!CheckChute())
		{
			return false;
		}
		Part attachedPart = EditorLogic.RootPart.FindAttachNode("bottom").attachedPart;
		if (!(attachedPart == null) && !(attachedPart.partInfo.name != "Decoupler.1") && !(attachedPart.FindAttachNodeByPart(EditorLogic.RootPart).id != "top"))
		{
			Part attachedPart2 = attachedPart.FindAttachNode("bottom").attachedPart;
			if (!(attachedPart2 == null) && !(attachedPart2.partInfo.name != "RCSFuelTank") && !(attachedPart2.Resources["MonoPropellant"].amount > 102.0) && attachedPart2.Resources["MonoPropellant"].amount >= 90.0)
			{
				int num = 0;
				Part part = null;
				bool flag = true;
				if (attachedPart2.children.Count != 1 && attachedPart2.children.Count != 5)
				{
					return false;
				}
				int count = attachedPart2.children.Count;
				while (count-- > 0)
				{
					Part part2 = attachedPart2.children[count];
					if (part2.partInfo.name == "fuelTankSmallFlat" && attachedPart2.FindAttachNodeByPart(part2) != null)
					{
						part = part2;
						flag = false;
					}
					else if (part2.partInfo.name == "RCSBlock" || part2.partInfo.name == "RCSBlock.v2")
					{
						num++;
					}
				}
				if (flag)
				{
					return false;
				}
				Part part3 = null;
				flag = true;
				int count2 = part.children.Count;
				while (count2-- > 0)
				{
					Part part2 = part.children[count2];
					if (part2.partInfo.name == "fuelTank" && part.FindAttachNodeByPart(part2) != null)
					{
						part3 = part2;
						flag = false;
					}
					else if (part2.partInfo.name == "RCSBlock" || part2.partInfo.name == "RCSBlock.v2")
					{
						num++;
					}
				}
				if (flag)
				{
					return false;
				}
				if (part.Resources["LiquidFuel"].amount + part.Resources["Oxidizer"].amount + part3.Resources["LiquidFuel"].amount + part3.Resources["Oxidizer"].amount < 499.0)
				{
					return false;
				}
				if (part3.children.Count != 1 && part3.children.Count != 5)
				{
					return false;
				}
				bool flag2 = true;
				int count3 = part3.children.Count;
				while (count3-- > 0)
				{
					Part part2 = part3.children[count3];
					if ((part2.partInfo.name == "liquidEngine3" || part2.partInfo.name == "liquidEngine3.v2") && part2.FindAttachNodeByPart(part3).id == "top")
					{
						flag2 = false;
					}
					if (part2.partInfo.name == "RCSBlock" || part2.partInfo.name == "RCSBlock.v2")
					{
						num++;
					}
				}
				if (num != 4 || flag2)
				{
					return false;
				}
				return true;
			}
			return false;
		}
		return false;
	}

	public bool CheckElectric()
	{
		if (!CheckChute())
		{
			return false;
		}
		Part attachedPart = EditorLogic.RootPart.FindAttachNode("bottom").attachedPart;
		if (!(attachedPart == null) && !(attachedPart.partInfo.name != "Decoupler.1") && !(attachedPart.FindAttachNodeByPart(EditorLogic.RootPart).id != "top"))
		{
			Part attachedPart2 = attachedPart.FindAttachNode("bottom").attachedPart;
			if (!(attachedPart2 == null) && !(attachedPart2.partInfo.name != "RCSFuelTank") && !(attachedPart2.Resources["MonoPropellant"].amount > 102.0) && attachedPart2.Resources["MonoPropellant"].amount >= 90.0)
			{
				int num = 0;
				Part part = null;
				bool flag = true;
				if (attachedPart2.children.Count != 1 && attachedPart2.children.Count != 5)
				{
					return false;
				}
				int count = attachedPart2.children.Count;
				while (count-- > 0)
				{
					Part part2 = attachedPart2.children[count];
					if (part2.partInfo.name == "fuelTankSmallFlat" && attachedPart2.FindAttachNodeByPart(part2) != null)
					{
						part = part2;
						flag = false;
					}
					else if (part2.partInfo.name == "RCSBlock" || part2.partInfo.name == "RCSBlock.v2")
					{
						num++;
					}
				}
				if (flag)
				{
					return false;
				}
				Part part3 = null;
				flag = true;
				int count2 = part.children.Count;
				while (count2-- > 0)
				{
					Part part2 = part.children[count2];
					if (part2.partInfo.name == "fuelTank" && part.FindAttachNodeByPart(part2) != null)
					{
						part3 = part2;
						flag = false;
					}
					else if (part2.partInfo.name == "RCSBlock" || part2.partInfo.name == "RCSBlock.v2")
					{
						num++;
					}
				}
				if (flag)
				{
					return false;
				}
				if (part.Resources["LiquidFuel"].amount + part.Resources["Oxidizer"].amount + part3.Resources["LiquidFuel"].amount + part3.Resources["Oxidizer"].amount < 499.0)
				{
					return false;
				}
				if (part3.children.Count != 9 && part3.children.Count != 13)
				{
					return false;
				}
				bool flag2 = true;
				int num2 = 0;
				int num3 = 0;
				int count3 = part3.children.Count;
				while (count3-- > 0)
				{
					Part part2 = part3.children[count3];
					if ((part2.partInfo.name == "liquidEngine3" || part2.partInfo.name == "liquidEngine3.v2") && part2.FindAttachNodeByPart(part3).id == "top")
					{
						flag2 = false;
					}
					if (!(part2.partInfo.name == "RCSBlock") && !(part2.partInfo.name == "RCSBlock.v2"))
					{
						if (part2.partInfo.name == "batteryPack")
						{
							num2++;
						}
						else if (part2.partInfo.name == "solarPanels5")
						{
							num3++;
						}
					}
					else
					{
						num++;
					}
				}
				if (!(num != 4 || flag2) && num3 == 4 && num2 == 4)
				{
					return true;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public bool CheckLowerDec()
	{
		if (!CheckChute())
		{
			return false;
		}
		Part attachedPart = EditorLogic.RootPart.FindAttachNode("bottom").attachedPart;
		if (!(attachedPart == null) && !(attachedPart.partInfo.name != "Decoupler.1") && !(attachedPart.FindAttachNodeByPart(EditorLogic.RootPart).id != "top"))
		{
			Part attachedPart2 = attachedPart.FindAttachNode("bottom").attachedPart;
			if (!(attachedPart2 == null) && !(attachedPart2.partInfo.name != "RCSFuelTank") && !(attachedPart2.Resources["MonoPropellant"].amount > 102.0) && attachedPart2.Resources["MonoPropellant"].amount >= 90.0)
			{
				int num = 0;
				Part part = null;
				bool flag = true;
				if (attachedPart2.children.Count != 1 && attachedPart2.children.Count != 5)
				{
					return false;
				}
				int count = attachedPart2.children.Count;
				while (count-- > 0)
				{
					Part part2 = attachedPart2.children[count];
					if (part2.partInfo.name == "fuelTankSmallFlat" && attachedPart2.FindAttachNodeByPart(part2) != null)
					{
						part = part2;
						flag = false;
					}
					else if (part2.partInfo.name == "RCSBlock" || part2.partInfo.name == "RCSBlock.v2")
					{
						num++;
					}
				}
				if (flag)
				{
					return false;
				}
				Part part3 = null;
				flag = true;
				int count2 = part.children.Count;
				while (count2-- > 0)
				{
					Part part2 = part.children[count2];
					if (part2.partInfo.name == "fuelTank" && part.FindAttachNodeByPart(part2) != null)
					{
						part3 = part2;
						flag = false;
					}
					else if (part2.partInfo.name == "RCSBlock" || part2.partInfo.name == "RCSBlock.v2")
					{
						num++;
					}
				}
				if (flag)
				{
					return false;
				}
				if (part.Resources["LiquidFuel"].amount + part.Resources["Oxidizer"].amount + part3.Resources["LiquidFuel"].amount + part3.Resources["Oxidizer"].amount < 499.0)
				{
					return false;
				}
				if (part3.children.Count != 9 && part3.children.Count != 13)
				{
					return false;
				}
				bool flag2 = true;
				int num2 = 0;
				int num3 = 0;
				Part part4 = null;
				int count3 = part3.children.Count;
				while (count3-- > 0)
				{
					Part part2 = part3.children[count3];
					if ((part2.partInfo.name == "liquidEngine3" || part2.partInfo.name == "liquidEngine3.v2") && part2.FindAttachNodeByPart(part3).id == "top")
					{
						part4 = part2;
						flag2 = false;
					}
					if (!(part2.partInfo.name == "RCSBlock") && !(part2.partInfo.name == "RCSBlock.v2"))
					{
						if (part2.partInfo.name == "batteryPack")
						{
							num2++;
						}
						else if (part2.partInfo.name == "solarPanels5")
						{
							num3++;
						}
					}
					else
					{
						num++;
					}
				}
				if (!(num != 4 || flag2) && num3 == 4 && num2 == 4)
				{
					Part attachedPart3 = part4.FindAttachNode("bottom").attachedPart;
					if (!(attachedPart3 == null) && !(attachedPart3.partInfo.name != "Decoupler.1") && !(attachedPart3.FindAttachNodeByPart(part4).id != "top"))
					{
						return true;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public bool CheckLowerTanks()
	{
		if (!CheckChute())
		{
			return false;
		}
		Part attachedPart = EditorLogic.RootPart.FindAttachNode("bottom").attachedPart;
		if (!(attachedPart == null) && !(attachedPart.partInfo.name != "Decoupler.1") && !(attachedPart.FindAttachNodeByPart(EditorLogic.RootPart).id != "top"))
		{
			Part attachedPart2 = attachedPart.FindAttachNode("bottom").attachedPart;
			if (!(attachedPart2 == null) && !(attachedPart2.partInfo.name != "RCSFuelTank") && !(attachedPart2.Resources["MonoPropellant"].amount > 102.0) && attachedPart2.Resources["MonoPropellant"].amount >= 90.0)
			{
				int num = 0;
				Part part = null;
				bool flag = true;
				if (attachedPart2.children.Count != 1 && attachedPart2.children.Count != 5)
				{
					return false;
				}
				int count = attachedPart2.children.Count;
				while (count-- > 0)
				{
					Part part2 = attachedPart2.children[count];
					if (part2.partInfo.name == "fuelTankSmallFlat" && attachedPart2.FindAttachNodeByPart(part2) != null)
					{
						part = part2;
						flag = false;
					}
					else if (part2.partInfo.name == "RCSBlock" || part2.partInfo.name == "RCSBlock.v2")
					{
						num++;
					}
				}
				if (flag)
				{
					return false;
				}
				Part part3 = null;
				flag = true;
				int count2 = part.children.Count;
				while (count2-- > 0)
				{
					Part part2 = part.children[count2];
					if (part2.partInfo.name == "fuelTank" && part.FindAttachNodeByPart(part2) != null)
					{
						part3 = part2;
						flag = false;
					}
					else if (part2.partInfo.name == "RCSBlock" || part2.partInfo.name == "RCSBlock.v2")
					{
						num++;
					}
				}
				if (flag)
				{
					return false;
				}
				if (part.Resources["LiquidFuel"].amount + part.Resources["Oxidizer"].amount + part3.Resources["LiquidFuel"].amount + part3.Resources["Oxidizer"].amount < 499.0)
				{
					return false;
				}
				if (part3.children.Count != 9 && part3.children.Count != 13)
				{
					return false;
				}
				bool flag2 = true;
				int num2 = 0;
				int num3 = 0;
				Part part4 = null;
				int count3 = part3.children.Count;
				while (count3-- > 0)
				{
					Part part2 = part3.children[count3];
					if ((part2.partInfo.name == "liquidEngine3" || part2.partInfo.name == "liquidEngine3.v2") && part2.FindAttachNodeByPart(part3).id == "top")
					{
						part4 = part2;
						flag2 = false;
					}
					if (!(part2.partInfo.name == "RCSBlock") && !(part2.partInfo.name == "RCSBlock.v2"))
					{
						if (part2.partInfo.name == "batteryPack")
						{
							num2++;
						}
						else if (part2.partInfo.name == "solarPanels5")
						{
							num3++;
						}
					}
					else
					{
						num++;
					}
				}
				if (!(num != 4 || flag2) && num3 == 4 && num2 == 4)
				{
					Part attachedPart3 = part4.FindAttachNode("bottom").attachedPart;
					if (!(attachedPart3 == null) && !(attachedPart3.partInfo.name != "Decoupler.1") && !(attachedPart3.FindAttachNodeByPart(part4).id != "top"))
					{
						if (attachedPart3.children.Count != 1)
						{
							return false;
						}
						Part part5 = attachedPart3.children[0];
						if (attachedPart3.FindAttachNodeByPart(part5) == null)
						{
							return false;
						}
						if (part5.children.Count != 1)
						{
							return false;
						}
						Part part6 = part5.children[0];
						if (part5.FindAttachNodeByPart(part6) == null)
						{
							return false;
						}
						if (!(part5.partInfo.name != "fuelTank") && !(part6.partInfo.name != "fuelTank"))
						{
							return true;
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public bool CheckLowerEngine()
	{
		if (!CheckChute())
		{
			return false;
		}
		Part attachedPart = EditorLogic.RootPart.FindAttachNode("bottom").attachedPart;
		if (!(attachedPart == null) && !(attachedPart.partInfo.name != "Decoupler.1") && !(attachedPart.FindAttachNodeByPart(EditorLogic.RootPart).id != "top"))
		{
			Part attachedPart2 = attachedPart.FindAttachNode("bottom").attachedPart;
			if (!(attachedPart2 == null) && !(attachedPart2.partInfo.name != "RCSFuelTank") && !(attachedPart2.Resources["MonoPropellant"].amount > 102.0) && attachedPart2.Resources["MonoPropellant"].amount >= 90.0)
			{
				int num = 0;
				Part part = null;
				bool flag = true;
				if (attachedPart2.children.Count != 1 && attachedPart2.children.Count != 5)
				{
					return false;
				}
				int count = attachedPart2.children.Count;
				while (count-- > 0)
				{
					Part part2 = attachedPart2.children[count];
					if (part2.partInfo.name == "fuelTankSmallFlat" && attachedPart2.FindAttachNodeByPart(part2) != null)
					{
						part = part2;
						flag = false;
					}
					else if (part2.partInfo.name == "RCSBlock" || part2.partInfo.name == "RCSBlock.v2")
					{
						num++;
					}
				}
				if (flag)
				{
					return false;
				}
				Part part3 = null;
				flag = true;
				int count2 = part.children.Count;
				while (count2-- > 0)
				{
					Part part2 = part.children[count2];
					if (part2.partInfo.name == "fuelTank" && part.FindAttachNodeByPart(part2) != null)
					{
						part3 = part2;
						flag = false;
					}
					else if (part2.partInfo.name == "RCSBlock" || part2.partInfo.name == "RCSBlock.v2")
					{
						num++;
					}
				}
				if (flag)
				{
					return false;
				}
				if (part.Resources["LiquidFuel"].amount + part.Resources["Oxidizer"].amount + part3.Resources["LiquidFuel"].amount + part3.Resources["Oxidizer"].amount < 499.0)
				{
					return false;
				}
				if (part3.children.Count != 9 && part3.children.Count != 13)
				{
					return false;
				}
				bool flag2 = true;
				int num2 = 0;
				int num3 = 0;
				Part part4 = null;
				int count3 = part3.children.Count;
				while (count3-- > 0)
				{
					Part part2 = part3.children[count3];
					if ((part2.partInfo.name == "liquidEngine3" || part2.partInfo.name == "liquidEngine3.v2") && part2.FindAttachNodeByPart(part3).id == "top")
					{
						part4 = part2;
						flag2 = false;
					}
					if (!(part2.partInfo.name == "RCSBlock") && !(part2.partInfo.name == "RCSBlock.v2"))
					{
						if (part2.partInfo.name == "batteryPack")
						{
							num2++;
						}
						else if (part2.partInfo.name == "solarPanels5")
						{
							num3++;
						}
					}
					else
					{
						num++;
					}
				}
				if (!(num != 4 || flag2) && num3 == 4 && num2 == 4)
				{
					Part attachedPart3 = part4.FindAttachNode("bottom").attachedPart;
					if (!(attachedPart3 == null) && !(attachedPart3.partInfo.name != "Decoupler.1") && !(attachedPart3.FindAttachNodeByPart(part4).id != "top"))
					{
						if (attachedPart3.children.Count != 1)
						{
							return false;
						}
						Part part5 = attachedPart3.children[0];
						if (attachedPart3.FindAttachNodeByPart(part5) == null)
						{
							return false;
						}
						if (part5.children.Count != 1)
						{
							return false;
						}
						Part part6 = part5.children[0];
						if (part5.FindAttachNodeByPart(part6) == null)
						{
							return false;
						}
						if (!(part5.partInfo.name != "fuelTank") && !(part6.partInfo.name != "fuelTank"))
						{
							flag2 = true;
							int count4 = part6.children.Count;
							while (count4-- > 0)
							{
								Part part2 = part6.children[count4];
								if ((part2.partInfo.name == "liquidEngine2" || part2.partInfo.name == "liquidEngine2.v2") && part2.FindAttachNodeByPart(part6).id == "top")
								{
									flag2 = false;
								}
							}
							if (flag2)
							{
								return false;
							}
							return true;
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public bool CheckLowerEngineStats()
	{
		if (!CheckChute())
		{
			return false;
		}
		Part attachedPart = EditorLogic.RootPart.FindAttachNode("bottom").attachedPart;
		if (!(attachedPart == null) && !(attachedPart.partInfo.name != "Decoupler.1") && !(attachedPart.FindAttachNodeByPart(EditorLogic.RootPart).id != "top"))
		{
			Part attachedPart2 = attachedPart.FindAttachNode("bottom").attachedPart;
			if (!(attachedPart2 == null) && !(attachedPart2.partInfo.name != "RCSFuelTank") && !(attachedPart2.Resources["MonoPropellant"].amount > 102.0) && attachedPart2.Resources["MonoPropellant"].amount >= 90.0)
			{
				int num = 0;
				Part part = null;
				bool flag = true;
				if (attachedPart2.children.Count != 1 && attachedPart2.children.Count != 5)
				{
					return false;
				}
				int count = attachedPart2.children.Count;
				while (count-- > 0)
				{
					Part part2 = attachedPart2.children[count];
					if (part2.partInfo.name == "fuelTankSmallFlat" && attachedPart2.FindAttachNodeByPart(part2) != null)
					{
						part = part2;
						flag = false;
					}
					else if (part2.partInfo.name == "RCSBlock" || part2.partInfo.name == "RCSBlock.v2")
					{
						num++;
					}
				}
				if (flag)
				{
					return false;
				}
				Part part3 = null;
				flag = true;
				int count2 = part.children.Count;
				while (count2-- > 0)
				{
					Part part2 = part.children[count2];
					if (part2.partInfo.name == "fuelTank" && part.FindAttachNodeByPart(part2) != null)
					{
						part3 = part2;
						flag = false;
					}
					else if (part2.partInfo.name == "RCSBlock" || part2.partInfo.name == "RCSBlock.v2")
					{
						num++;
					}
				}
				if (flag)
				{
					return false;
				}
				if (part.Resources["LiquidFuel"].amount + part.Resources["Oxidizer"].amount + part3.Resources["LiquidFuel"].amount + part3.Resources["Oxidizer"].amount < 499.0)
				{
					return false;
				}
				if (part3.children.Count != 9 && part3.children.Count != 13)
				{
					return false;
				}
				bool flag2 = true;
				int num2 = 0;
				int num3 = 0;
				Part part4 = null;
				int count3 = part3.children.Count;
				while (count3-- > 0)
				{
					Part part2 = part3.children[count3];
					if ((part2.partInfo.name == "liquidEngine3" || part2.partInfo.name == "liquidEngine3.v2") && part2.FindAttachNodeByPart(part3).id == "top")
					{
						part4 = part2;
						flag2 = false;
					}
					if (!(part2.partInfo.name == "RCSBlock") && !(part2.partInfo.name == "RCSBlock.v2"))
					{
						if (part2.partInfo.name == "batteryPack")
						{
							num2++;
						}
						else if (part2.partInfo.name == "solarPanels5")
						{
							num3++;
						}
					}
					else
					{
						num++;
					}
				}
				if (!(num != 4 || flag2) && num3 == 4 && num2 == 4)
				{
					Part attachedPart3 = part4.FindAttachNode("bottom").attachedPart;
					if (!(attachedPart3 == null) && !(attachedPart3.partInfo.name != "Decoupler.1") && !(attachedPart3.FindAttachNodeByPart(part4).id != "top"))
					{
						if (attachedPart3.children.Count != 1)
						{
							return false;
						}
						Part part5 = attachedPart3.children[0];
						if (attachedPart3.FindAttachNodeByPart(part5) == null)
						{
							return false;
						}
						if (part5.children.Count != 1)
						{
							return false;
						}
						Part part6 = part5.children[0];
						if (part5.FindAttachNodeByPart(part6) == null)
						{
							return false;
						}
						if (!(part5.partInfo.name != "fuelTank") && !(part6.partInfo.name != "fuelTank"))
						{
							Part part7 = null;
							flag2 = true;
							int count4 = part6.children.Count;
							while (count4-- > 0)
							{
								Part part2 = part6.children[count4];
								if ((part2.partInfo.name == "liquidEngine2" || part2.partInfo.name == "liquidEngine2.v2") && part2.FindAttachNodeByPart(part6).id == "top")
								{
									flag2 = false;
									part7 = part2;
								}
							}
							if (flag2)
							{
								return false;
							}
							ModuleEngines module = part7.Modules.GetModule<ModuleEngines>();
							if (!(module == null) && !(module.thrustPercentage > 65.7f) && module.thrustPercentage >= 64.2f)
							{
								return true;
							}
							return false;
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public bool CheckRadialDecs()
	{
		if (!CheckChute())
		{
			return false;
		}
		Part attachedPart = EditorLogic.RootPart.FindAttachNode("bottom").attachedPart;
		if (!(attachedPart == null) && !(attachedPart.partInfo.name != "Decoupler.1") && !(attachedPart.FindAttachNodeByPart(EditorLogic.RootPart).id != "top"))
		{
			Part attachedPart2 = attachedPart.FindAttachNode("bottom").attachedPart;
			if (!(attachedPart2 == null) && !(attachedPart2.partInfo.name != "RCSFuelTank") && !(attachedPart2.Resources["MonoPropellant"].amount > 102.0) && attachedPart2.Resources["MonoPropellant"].amount >= 90.0)
			{
				int num = 0;
				Part part = null;
				bool flag = true;
				if (attachedPart2.children.Count != 1 && attachedPart2.children.Count != 5)
				{
					return false;
				}
				int count = attachedPart2.children.Count;
				while (count-- > 0)
				{
					Part part2 = attachedPart2.children[count];
					if (part2.partInfo.name == "fuelTankSmallFlat" && attachedPart2.FindAttachNodeByPart(part2) != null)
					{
						part = part2;
						flag = false;
					}
					else if (part2.partInfo.name == "RCSBlock" || part2.partInfo.name == "RCSBlock.v2")
					{
						num++;
					}
				}
				if (flag)
				{
					return false;
				}
				Part part3 = null;
				flag = true;
				int count2 = part.children.Count;
				while (count2-- > 0)
				{
					Part part2 = part.children[count2];
					if (part2.partInfo.name == "fuelTank" && part.FindAttachNodeByPart(part2) != null)
					{
						part3 = part2;
						flag = false;
					}
					else if (part2.partInfo.name == "RCSBlock" || part2.partInfo.name == "RCSBlock.v2")
					{
						num++;
					}
				}
				if (flag)
				{
					return false;
				}
				if (part.Resources["LiquidFuel"].amount + part.Resources["Oxidizer"].amount + part3.Resources["LiquidFuel"].amount + part3.Resources["Oxidizer"].amount < 499.0)
				{
					return false;
				}
				if (part3.children.Count != 9 && part3.children.Count != 13)
				{
					return false;
				}
				bool flag2 = true;
				int num2 = 0;
				int num3 = 0;
				Part part4 = null;
				int count3 = part3.children.Count;
				while (count3-- > 0)
				{
					Part part2 = part3.children[count3];
					if ((part2.partInfo.name == "liquidEngine3" || part2.partInfo.name == "liquidEngine3.v2") && part2.FindAttachNodeByPart(part3).id == "top")
					{
						part4 = part2;
						flag2 = false;
					}
					if (!(part2.partInfo.name == "RCSBlock") && !(part2.partInfo.name == "RCSBlock.v2"))
					{
						if (part2.partInfo.name == "batteryPack")
						{
							num2++;
						}
						else if (part2.partInfo.name == "solarPanels5")
						{
							num3++;
						}
					}
					else
					{
						num++;
					}
				}
				if (!(num != 4 || flag2) && num3 == 4 && num2 == 4)
				{
					Part attachedPart3 = part4.FindAttachNode("bottom").attachedPart;
					if (!(attachedPart3 == null) && !(attachedPart3.partInfo.name != "Decoupler.1") && !(attachedPart3.FindAttachNodeByPart(part4).id != "top"))
					{
						if (attachedPart3.children.Count != 1)
						{
							return false;
						}
						Part part5 = attachedPart3.children[0];
						if (attachedPart3.FindAttachNodeByPart(part5) == null)
						{
							return false;
						}
						if (part5.children.Count != 1)
						{
							return false;
						}
						Part part6 = part5.children[0];
						if (part5.FindAttachNodeByPart(part6) == null)
						{
							return false;
						}
						if (!(part5.partInfo.name != "fuelTank") && !(part6.partInfo.name != "fuelTank"))
						{
							Part part7 = null;
							flag2 = true;
							int num4 = 0;
							int count4 = part6.children.Count;
							while (count4-- > 0)
							{
								Part part2 = part6.children[count4];
								if ((part2.partInfo.name == "liquidEngine2" || part2.partInfo.name == "liquidEngine2.v2") && part2.FindAttachNodeByPart(part6).id == "top")
								{
									flag2 = false;
									part7 = part2;
								}
								else if (part2.partInfo.name == "radialDecoupler")
								{
									num4++;
								}
							}
							if (flag2)
							{
								return false;
							}
							ModuleEngines module = part7.Modules.GetModule<ModuleEngines>();
							if (!(module == null) && !(module.thrustPercentage > 65.7f) && module.thrustPercentage >= 64.2f)
							{
								if (num4 != 2)
								{
									return false;
								}
								return true;
							}
							return false;
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public bool CheckSolids()
	{
		if (!CheckChute())
		{
			return false;
		}
		Part attachedPart = EditorLogic.RootPart.FindAttachNode("bottom").attachedPart;
		if (!(attachedPart == null) && !(attachedPart.partInfo.name != "Decoupler.1") && !(attachedPart.FindAttachNodeByPart(EditorLogic.RootPart).id != "top"))
		{
			Part attachedPart2 = attachedPart.FindAttachNode("bottom").attachedPart;
			if (!(attachedPart2 == null) && !(attachedPart2.partInfo.name != "RCSFuelTank") && !(attachedPart2.Resources["MonoPropellant"].amount > 102.0) && attachedPart2.Resources["MonoPropellant"].amount >= 90.0)
			{
				int num = 0;
				Part part = null;
				bool flag = true;
				if (attachedPart2.children.Count != 1 && attachedPart2.children.Count != 5)
				{
					return false;
				}
				int count = attachedPart2.children.Count;
				while (count-- > 0)
				{
					Part part2 = attachedPart2.children[count];
					if (part2.partInfo.name == "fuelTankSmallFlat" && attachedPart2.FindAttachNodeByPart(part2) != null)
					{
						part = part2;
						flag = false;
					}
					else if (part2.partInfo.name == "RCSBlock" || part2.partInfo.name == "RCSBlock.v2")
					{
						num++;
					}
				}
				if (flag)
				{
					return false;
				}
				Part part3 = null;
				flag = true;
				int count2 = part.children.Count;
				while (count2-- > 0)
				{
					Part part2 = part.children[count2];
					if (part2.partInfo.name == "fuelTank" && part.FindAttachNodeByPart(part2) != null)
					{
						part3 = part2;
						flag = false;
					}
					else if (part2.partInfo.name == "RCSBlock" || part2.partInfo.name == "RCSBlock.v2")
					{
						num++;
					}
				}
				if (flag)
				{
					return false;
				}
				if (part.Resources["LiquidFuel"].amount + part.Resources["Oxidizer"].amount + part3.Resources["LiquidFuel"].amount + part3.Resources["Oxidizer"].amount < 499.0)
				{
					return false;
				}
				if (part3.children.Count != 9 && part3.children.Count != 13)
				{
					return false;
				}
				bool flag2 = true;
				int num2 = 0;
				int num3 = 0;
				Part part4 = null;
				int count3 = part3.children.Count;
				while (count3-- > 0)
				{
					Part part2 = part3.children[count3];
					if ((part2.partInfo.name == "liquidEngine3" || part2.partInfo.name == "liquidEngine3.v2") && part2.FindAttachNodeByPart(part3).id == "top")
					{
						part4 = part2;
						flag2 = false;
					}
					if (!(part2.partInfo.name == "RCSBlock") && !(part2.partInfo.name == "RCSBlock.v2"))
					{
						if (part2.partInfo.name == "batteryPack")
						{
							num2++;
						}
						else if (part2.partInfo.name == "solarPanels5")
						{
							num3++;
						}
					}
					else
					{
						num++;
					}
				}
				if (!(num != 4 || flag2) && num3 == 4 && num2 == 4)
				{
					Part attachedPart3 = part4.FindAttachNode("bottom").attachedPart;
					if (!(attachedPart3 == null) && !(attachedPart3.partInfo.name != "Decoupler.1") && !(attachedPart3.FindAttachNodeByPart(part4).id != "top"))
					{
						if (attachedPart3.children.Count != 1)
						{
							return false;
						}
						Part part5 = attachedPart3.children[0];
						if (attachedPart3.FindAttachNodeByPart(part5) == null)
						{
							return false;
						}
						if (part5.children.Count != 1)
						{
							return false;
						}
						Part part6 = part5.children[0];
						if (part5.FindAttachNodeByPart(part6) == null)
						{
							return false;
						}
						if (!(part5.partInfo.name != "fuelTank") && !(part6.partInfo.name != "fuelTank"))
						{
							Part part7 = null;
							flag2 = true;
							List<Part> list = new List<Part>();
							int count4 = part6.children.Count;
							while (count4-- > 0)
							{
								Part part2 = part6.children[count4];
								if ((part2.partInfo.name == "liquidEngine2" || part2.partInfo.name == "liquidEngine2.v2") && part2.FindAttachNodeByPart(part6).id == "top")
								{
									flag2 = false;
									part7 = part2;
								}
								else if (part2.partInfo.name == "radialDecoupler")
								{
									list.Add(part2);
								}
							}
							if (flag2)
							{
								return false;
							}
							ModuleEngines module = part7.Modules.GetModule<ModuleEngines>();
							if (!(module == null) && !(module.thrustPercentage > 65.7f) && module.thrustPercentage >= 64.2f)
							{
								if (list.Count != 2)
								{
									return false;
								}
								List<Part> list2 = new List<Part>();
								int count5 = list.Count;
								while (true)
								{
									Part part2;
									if (count5-- > 0)
									{
										part2 = list[count5];
										if (part2.children.Count != 1)
										{
											break;
										}
										list2.Add(part2.children[0]);
										continue;
									}
									if (list2.Count != 2)
									{
										return false;
									}
									int count6 = list2.Count;
									do
									{
										if (count6-- > 0)
										{
											part2 = list2[count6];
											continue;
										}
										return true;
									}
									while (!(part2.partInfo.name != "solidBooster") || !(part2.partInfo.name != "solidBooster.v2"));
									return false;
								}
								return false;
							}
							return false;
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public bool CheckSolidsStats()
	{
		if (!CheckChute())
		{
			return false;
		}
		Part attachedPart = EditorLogic.RootPart.FindAttachNode("bottom").attachedPart;
		if (!(attachedPart == null) && !(attachedPart.partInfo.name != "Decoupler.1") && !(attachedPart.FindAttachNodeByPart(EditorLogic.RootPart).id != "top"))
		{
			Part attachedPart2 = attachedPart.FindAttachNode("bottom").attachedPart;
			if (!(attachedPart2 == null) && !(attachedPart2.partInfo.name != "RCSFuelTank") && !(attachedPart2.Resources["MonoPropellant"].amount > 102.0) && attachedPart2.Resources["MonoPropellant"].amount >= 90.0)
			{
				int num = 0;
				Part part = null;
				bool flag = true;
				if (attachedPart2.children.Count != 1 && attachedPart2.children.Count != 5)
				{
					return false;
				}
				int count = attachedPart2.children.Count;
				while (count-- > 0)
				{
					Part part2 = attachedPart2.children[count];
					if (part2.partInfo.name == "fuelTankSmallFlat" && attachedPart2.FindAttachNodeByPart(part2) != null)
					{
						part = part2;
						flag = false;
					}
					else if (part2.partInfo.name == "RCSBlock" || part2.partInfo.name == "RCSBlock.v2")
					{
						num++;
					}
				}
				if (flag)
				{
					return false;
				}
				Part part3 = null;
				flag = true;
				int count2 = part.children.Count;
				while (count2-- > 0)
				{
					Part part2 = part.children[count2];
					if (part2.partInfo.name == "fuelTank" && part.FindAttachNodeByPart(part2) != null)
					{
						part3 = part2;
						flag = false;
					}
					else if (part2.partInfo.name == "RCSBlock" || part2.partInfo.name == "RCSBlock.v2")
					{
						num++;
					}
				}
				if (flag)
				{
					return false;
				}
				if (part.Resources["LiquidFuel"].amount + part.Resources["Oxidizer"].amount + part3.Resources["LiquidFuel"].amount + part3.Resources["Oxidizer"].amount < 499.0)
				{
					return false;
				}
				if (part3.children.Count != 9 && part3.children.Count != 13)
				{
					return false;
				}
				bool flag2 = true;
				int num2 = 0;
				int num3 = 0;
				Part part4 = null;
				int count3 = part3.children.Count;
				while (count3-- > 0)
				{
					Part part2 = part3.children[count3];
					if ((part2.partInfo.name == "liquidEngine3" || part2.partInfo.name == "liquidEngine3.v2") && part2.FindAttachNodeByPart(part3).id == "top")
					{
						part4 = part2;
						flag2 = false;
					}
					if (!(part2.partInfo.name == "RCSBlock") && !(part2.partInfo.name == "RCSBlock.v2"))
					{
						if (part2.partInfo.name == "batteryPack")
						{
							num2++;
						}
						else if (part2.partInfo.name == "solarPanels5")
						{
							num3++;
						}
					}
					else
					{
						num++;
					}
				}
				if (!(num != 4 || flag2) && num3 == 4 && num2 == 4)
				{
					Part attachedPart3 = part4.FindAttachNode("bottom").attachedPart;
					if (!(attachedPart3 == null) && !(attachedPart3.partInfo.name != "Decoupler.1") && !(attachedPart3.FindAttachNodeByPart(part4).id != "top"))
					{
						if (attachedPart3.children.Count != 1)
						{
							return false;
						}
						Part part5 = attachedPart3.children[0];
						if (attachedPart3.FindAttachNodeByPart(part5) == null)
						{
							return false;
						}
						if (part5.children.Count != 1)
						{
							return false;
						}
						Part part6 = part5.children[0];
						if (part5.FindAttachNodeByPart(part6) == null)
						{
							return false;
						}
						if (!(part5.partInfo.name != "fuelTank") && !(part6.partInfo.name != "fuelTank"))
						{
							Part part7 = null;
							flag2 = true;
							List<Part> list = new List<Part>();
							int count4 = part6.children.Count;
							while (count4-- > 0)
							{
								Part part2 = part6.children[count4];
								if ((part2.partInfo.name == "liquidEngine2" || part2.partInfo.name == "liquidEngine2.v2") && part2.FindAttachNodeByPart(part6).id == "top")
								{
									flag2 = false;
									part7 = part2;
								}
								else if (part2.partInfo.name == "radialDecoupler")
								{
									list.Add(part2);
								}
							}
							if (flag2)
							{
								return false;
							}
							ModuleEngines module = part7.Modules.GetModule<ModuleEngines>();
							if (!(module == null) && !(module.thrustPercentage > 65.7f) && module.thrustPercentage >= 64.2f)
							{
								if (list.Count != 2)
								{
									return false;
								}
								List<Part> list2 = new List<Part>();
								int count5 = list.Count;
								while (true)
								{
									if (count5-- > 0)
									{
										Part part2 = list[count5];
										if (part2.children.Count != 1)
										{
											break;
										}
										list2.Add(part2.children[0]);
										continue;
									}
									if (list2.Count != 2)
									{
										return false;
									}
									int count6 = list2.Count;
									ModuleEngines module2;
									do
									{
										if (count6-- > 0)
										{
											Part part2 = list2[count6];
											if (!(part2.partInfo.name != "solidBooster") || !(part2.partInfo.name != "solidBooster.v2"))
											{
												module2 = part2.Modules.GetModule<ModuleEngines>();
												continue;
											}
											return false;
										}
										return true;
									}
									while (!(module2 == null) && !(module2.thrustPercentage < 49.4f) && module2.thrustPercentage <= 50.7f);
									return false;
								}
								return false;
							}
							return false;
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public bool CheckNosecones()
	{
		if (!CheckChute())
		{
			return false;
		}
		Part attachedPart = EditorLogic.RootPart.FindAttachNode("bottom").attachedPart;
		if (!(attachedPart == null) && !(attachedPart.partInfo.name != "Decoupler.1") && !(attachedPart.FindAttachNodeByPart(EditorLogic.RootPart).id != "top"))
		{
			Part attachedPart2 = attachedPart.FindAttachNode("bottom").attachedPart;
			if (!(attachedPart2 == null) && !(attachedPart2.partInfo.name != "RCSFuelTank") && !(attachedPart2.Resources["MonoPropellant"].amount > 102.0) && attachedPart2.Resources["MonoPropellant"].amount >= 90.0)
			{
				int num = 0;
				Part part = null;
				bool flag = true;
				if (attachedPart2.children.Count != 1 && attachedPart2.children.Count != 5)
				{
					return false;
				}
				int count = attachedPart2.children.Count;
				while (count-- > 0)
				{
					Part part2 = attachedPart2.children[count];
					if (part2.partInfo.name == "fuelTankSmallFlat" && attachedPart2.FindAttachNodeByPart(part2) != null)
					{
						part = part2;
						flag = false;
					}
					else if (part2.partInfo.name == "RCSBlock" || part2.partInfo.name == "RCSBlock.v2")
					{
						num++;
					}
				}
				if (flag)
				{
					return false;
				}
				Part part3 = null;
				flag = true;
				int count2 = part.children.Count;
				while (count2-- > 0)
				{
					Part part2 = part.children[count2];
					if (part2.partInfo.name == "fuelTank" && part.FindAttachNodeByPart(part2) != null)
					{
						part3 = part2;
						flag = false;
					}
					else if (part2.partInfo.name == "RCSBlock" || part2.partInfo.name == "RCSBlock.v2")
					{
						num++;
					}
				}
				if (flag)
				{
					return false;
				}
				if (part.Resources["LiquidFuel"].amount + part.Resources["Oxidizer"].amount + part3.Resources["LiquidFuel"].amount + part3.Resources["Oxidizer"].amount < 499.0)
				{
					return false;
				}
				if (part3.children.Count != 9 && part3.children.Count != 13)
				{
					return false;
				}
				bool flag2 = true;
				int num2 = 0;
				int num3 = 0;
				Part part4 = null;
				int count3 = part3.children.Count;
				while (count3-- > 0)
				{
					Part part2 = part3.children[count3];
					if ((part2.partInfo.name == "liquidEngine3" || part2.partInfo.name == "liquidEngine3.v2") && part2.FindAttachNodeByPart(part3).id == "top")
					{
						part4 = part2;
						flag2 = false;
					}
					if (!(part2.partInfo.name == "RCSBlock") && !(part2.partInfo.name == "RCSBlock.v2"))
					{
						if (part2.partInfo.name == "batteryPack")
						{
							num2++;
						}
						else if (part2.partInfo.name == "solarPanels5")
						{
							num3++;
						}
					}
					else
					{
						num++;
					}
				}
				if (!(num != 4 || flag2) && num3 == 4 && num2 == 4)
				{
					Part attachedPart3 = part4.FindAttachNode("bottom").attachedPart;
					if (!(attachedPart3 == null) && !(attachedPart3.partInfo.name != "Decoupler.1") && !(attachedPart3.FindAttachNodeByPart(part4).id != "top"))
					{
						if (attachedPart3.children.Count != 1)
						{
							return false;
						}
						Part part5 = attachedPart3.children[0];
						if (attachedPart3.FindAttachNodeByPart(part5) == null)
						{
							return false;
						}
						if (part5.children.Count != 1)
						{
							return false;
						}
						Part part6 = part5.children[0];
						if (part5.FindAttachNodeByPart(part6) == null)
						{
							return false;
						}
						if (!(part5.partInfo.name != "fuelTank") && !(part6.partInfo.name != "fuelTank"))
						{
							Part part7 = null;
							flag2 = true;
							List<Part> list = new List<Part>();
							int count4 = part6.children.Count;
							while (count4-- > 0)
							{
								Part part2 = part6.children[count4];
								if ((part2.partInfo.name == "liquidEngine2" || part2.partInfo.name == "liquidEngine2.v2") && part2.FindAttachNodeByPart(part6).id == "top")
								{
									flag2 = false;
									part7 = part2;
								}
								else if (part2.partInfo.name == "radialDecoupler")
								{
									list.Add(part2);
								}
							}
							if (flag2)
							{
								return false;
							}
							ModuleEngines module = part7.Modules.GetModule<ModuleEngines>();
							if (!(module == null) && !(module.thrustPercentage > 65.7f) && module.thrustPercentage >= 64.2f)
							{
								if (list.Count != 2)
								{
									return false;
								}
								List<Part> list2 = new List<Part>();
								int count5 = list.Count;
								while (true)
								{
									if (count5-- > 0)
									{
										Part part2 = list[count5];
										if (part2.children.Count != 1)
										{
											break;
										}
										list2.Add(part2.children[0]);
										continue;
									}
									if (list2.Count != 2)
									{
										return false;
									}
									int count6 = list2.Count;
									while (true)
									{
										if (count6-- > 0)
										{
											Part part2 = list2[count6];
											if (!(part2.partInfo.name != "solidBooster") || !(part2.partInfo.name != "solidBooster.v2"))
											{
												ModuleEngines module2 = part2.Modules.GetModule<ModuleEngines>();
												if (module2 == null || module2.thrustPercentage < 49.4f || !(module2.thrustPercentage <= 50.7f))
												{
													break;
												}
												if (part2.children.Count != 1 || part2.children[0].partInfo.name != "noseCone")
												{
													return false;
												}
												continue;
											}
											return false;
										}
										return true;
									}
									return false;
								}
								return false;
							}
							return false;
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public bool CheckFins()
	{
		if (!CheckChute())
		{
			return false;
		}
		Part attachedPart = EditorLogic.RootPart.FindAttachNode("bottom").attachedPart;
		if (!(attachedPart == null) && !(attachedPart.partInfo.name != "Decoupler.1") && !(attachedPart.FindAttachNodeByPart(EditorLogic.RootPart).id != "top"))
		{
			Part attachedPart2 = attachedPart.FindAttachNode("bottom").attachedPart;
			if (!(attachedPart2 == null) && !(attachedPart2.partInfo.name != "RCSFuelTank") && !(attachedPart2.Resources["MonoPropellant"].amount > 102.0) && attachedPart2.Resources["MonoPropellant"].amount >= 90.0)
			{
				int num = 0;
				Part part = null;
				bool flag = true;
				if (attachedPart2.children.Count != 1 && attachedPart2.children.Count != 5)
				{
					return false;
				}
				int count = attachedPart2.children.Count;
				while (count-- > 0)
				{
					Part part2 = attachedPart2.children[count];
					if (part2.partInfo.name == "fuelTankSmallFlat" && attachedPart2.FindAttachNodeByPart(part2) != null)
					{
						part = part2;
						flag = false;
					}
					else if (part2.partInfo.name == "RCSBlock" || part2.partInfo.name == "RCSBlock.v2")
					{
						num++;
					}
				}
				if (flag)
				{
					return false;
				}
				Part part3 = null;
				flag = true;
				int count2 = part.children.Count;
				while (count2-- > 0)
				{
					Part part2 = part.children[count2];
					if (part2.partInfo.name == "fuelTank" && part.FindAttachNodeByPart(part2) != null)
					{
						part3 = part2;
						flag = false;
					}
					else if (part2.partInfo.name == "RCSBlock" || part2.partInfo.name == "RCSBlock.v2")
					{
						num++;
					}
				}
				if (flag)
				{
					return false;
				}
				if (part.Resources["LiquidFuel"].amount + part.Resources["Oxidizer"].amount + part3.Resources["LiquidFuel"].amount + part3.Resources["Oxidizer"].amount < 499.0)
				{
					return false;
				}
				if (part3.children.Count != 9 && part3.children.Count != 13)
				{
					return false;
				}
				bool flag2 = true;
				int num2 = 0;
				int num3 = 0;
				Part part4 = null;
				int count3 = part3.children.Count;
				while (count3-- > 0)
				{
					Part part2 = part3.children[count3];
					if ((part2.partInfo.name == "liquidEngine3" || part2.partInfo.name == "liquidEngine3.v2") && part2.FindAttachNodeByPart(part3).id == "top")
					{
						part4 = part2;
						flag2 = false;
					}
					if (!(part2.partInfo.name == "RCSBlock") && !(part2.partInfo.name == "RCSBlock.v2"))
					{
						if (part2.partInfo.name == "batteryPack")
						{
							num2++;
						}
						else if (part2.partInfo.name == "solarPanels5")
						{
							num3++;
						}
					}
					else
					{
						num++;
					}
				}
				if (!(num != 4 || flag2) && num3 == 4 && num2 == 4)
				{
					Part attachedPart3 = part4.FindAttachNode("bottom").attachedPart;
					if (!(attachedPart3 == null) && !(attachedPart3.partInfo.name != "Decoupler.1") && !(attachedPart3.FindAttachNodeByPart(part4).id != "top"))
					{
						if (attachedPart3.children.Count != 1)
						{
							return false;
						}
						Part part5 = attachedPart3.children[0];
						if (attachedPart3.FindAttachNodeByPart(part5) == null)
						{
							return false;
						}
						if (part5.children.Count != 1)
						{
							return false;
						}
						Part part6 = part5.children[0];
						if (part5.FindAttachNodeByPart(part6) == null)
						{
							return false;
						}
						if (!(part5.partInfo.name != "fuelTank") && !(part6.partInfo.name != "fuelTank"))
						{
							Part part7 = null;
							flag2 = true;
							List<Part> list = new List<Part>();
							int num4 = 0;
							int count4 = part6.children.Count;
							while (count4-- > 0)
							{
								Part part2 = part6.children[count4];
								if ((part2.partInfo.name == "liquidEngine2" || part2.partInfo.name == "liquidEngine2.v2") && part2.FindAttachNodeByPart(part6).id == "top")
								{
									flag2 = false;
									part7 = part2;
								}
								else if (part2.partInfo.name == "radialDecoupler")
								{
									list.Add(part2);
								}
								else if (part2.partInfo.name == "R8winglet")
								{
									num4++;
								}
							}
							if (!flag2 && num4 == 4)
							{
								ModuleEngines module = part7.Modules.GetModule<ModuleEngines>();
								if (!(module == null) && !(module.thrustPercentage > 65.7f) && module.thrustPercentage >= 64.2f)
								{
									if (list.Count != 2)
									{
										return false;
									}
									List<Part> list2 = new List<Part>();
									int count5 = list.Count;
									while (true)
									{
										if (count5-- > 0)
										{
											Part part2 = list[count5];
											if (part2.children.Count != 1)
											{
												break;
											}
											list2.Add(part2.children[0]);
											continue;
										}
										if (list2.Count != 2)
										{
											return false;
										}
										int count6 = list2.Count;
										while (true)
										{
											if (count6-- > 0)
											{
												Part part2 = list2[count6];
												if (!(part2.partInfo.name != "solidBooster") || !(part2.partInfo.name != "solidBooster.v2"))
												{
													ModuleEngines module2 = part2.Modules.GetModule<ModuleEngines>();
													if (module2 == null || module2.thrustPercentage < 49.4f || !(module2.thrustPercentage <= 50.7f))
													{
														break;
													}
													if (part2.children.Count != 1 || part2.children[0].partInfo.name != "noseCone")
													{
														return false;
													}
													continue;
												}
												return false;
											}
											return true;
										}
										return false;
									}
									return false;
								}
								return false;
							}
							return false;
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public bool CheckT45Staging()
	{
		if (!CheckChute())
		{
			return false;
		}
		Part attachedPart = EditorLogic.RootPart.FindAttachNode("bottom").attachedPart;
		if (!(attachedPart == null) && !(attachedPart.partInfo.name != "Decoupler.1") && !(attachedPart.FindAttachNodeByPart(EditorLogic.RootPart).id != "top"))
		{
			Part attachedPart2 = attachedPart.FindAttachNode("bottom").attachedPart;
			if (!(attachedPart2 == null) && !(attachedPart2.partInfo.name != "RCSFuelTank") && !(attachedPart2.Resources["MonoPropellant"].amount > 102.0) && attachedPart2.Resources["MonoPropellant"].amount >= 90.0)
			{
				int num = 0;
				Part part = null;
				bool flag = true;
				if (attachedPart2.children.Count != 1 && attachedPart2.children.Count != 5)
				{
					return false;
				}
				int count = attachedPart2.children.Count;
				while (count-- > 0)
				{
					Part part2 = attachedPart2.children[count];
					if (part2.partInfo.name == "fuelTankSmallFlat" && attachedPart2.FindAttachNodeByPart(part2) != null)
					{
						part = part2;
						flag = false;
					}
					else if (part2.partInfo.name == "RCSBlock" || part2.partInfo.name == "RCSBlock.v2")
					{
						num++;
					}
				}
				if (flag)
				{
					return false;
				}
				Part part3 = null;
				flag = true;
				int count2 = part.children.Count;
				while (count2-- > 0)
				{
					Part part2 = part.children[count2];
					if (part2.partInfo.name == "fuelTank" && part.FindAttachNodeByPart(part2) != null)
					{
						part3 = part2;
						flag = false;
					}
					else if (part2.partInfo.name == "RCSBlock" || part2.partInfo.name == "RCSBlock.v2")
					{
						num++;
					}
				}
				if (flag)
				{
					return false;
				}
				if (part.Resources["LiquidFuel"].amount + part.Resources["Oxidizer"].amount + part3.Resources["LiquidFuel"].amount + part3.Resources["Oxidizer"].amount < 499.0)
				{
					return false;
				}
				if (part3.children.Count != 9 && part3.children.Count != 13)
				{
					return false;
				}
				bool flag2 = true;
				int num2 = 0;
				int num3 = 0;
				Part part4 = null;
				int count3 = part3.children.Count;
				while (count3-- > 0)
				{
					Part part2 = part3.children[count3];
					if ((part2.partInfo.name == "liquidEngine3" || part2.partInfo.name == "liquidEngine3.v2") && part2.FindAttachNodeByPart(part3).id == "top")
					{
						part4 = part2;
						flag2 = false;
					}
					if (!(part2.partInfo.name == "RCSBlock") && !(part2.partInfo.name == "RCSBlock.v2"))
					{
						if (part2.partInfo.name == "batteryPack")
						{
							num2++;
						}
						else if (part2.partInfo.name == "solarPanels5")
						{
							num3++;
						}
					}
					else
					{
						num++;
					}
				}
				if (!(num != 4 || flag2) && num3 == 4 && num2 == 4)
				{
					Part attachedPart3 = part4.FindAttachNode("bottom").attachedPart;
					if (!(attachedPart3 == null) && !(attachedPart3.partInfo.name != "Decoupler.1") && !(attachedPart3.FindAttachNodeByPart(part4).id != "top"))
					{
						if (attachedPart3.children.Count != 1)
						{
							return false;
						}
						Part part5 = attachedPart3.children[0];
						if (attachedPart3.FindAttachNodeByPart(part5) == null)
						{
							return false;
						}
						if (part5.children.Count != 1)
						{
							return false;
						}
						Part part6 = part5.children[0];
						if (part5.FindAttachNodeByPart(part6) == null)
						{
							return false;
						}
						if (!(part5.partInfo.name != "fuelTank") && !(part6.partInfo.name != "fuelTank"))
						{
							Part part7 = null;
							flag2 = true;
							List<Part> list = new List<Part>();
							int num4 = 0;
							int count4 = part6.children.Count;
							while (count4-- > 0)
							{
								Part part2 = part6.children[count4];
								if ((part2.partInfo.name == "liquidEngine2" || part2.partInfo.name == "liquidEngine2.v2") && part2.FindAttachNodeByPart(part6).id == "top")
								{
									flag2 = false;
									part7 = part2;
								}
								else if (part2.partInfo.name == "radialDecoupler")
								{
									list.Add(part2);
								}
								else if (part2.partInfo.name == "R8winglet")
								{
									num4++;
								}
							}
							if (!flag2 && num4 == 4)
							{
								ModuleEngines module = part7.Modules.GetModule<ModuleEngines>();
								if (!(module == null) && !(module.thrustPercentage > 65.7f) && module.thrustPercentage >= 64.2f)
								{
									if (list.Count != 2)
									{
										return false;
									}
									List<Part> list2 = new List<Part>();
									int count5 = list.Count;
									while (true)
									{
										if (count5-- > 0)
										{
											Part part2 = list[count5];
											if (part2.children.Count != 1)
											{
												break;
											}
											list2.Add(part2.children[0]);
											continue;
										}
										if (list2.Count != 2)
										{
											return false;
										}
										int count6 = list2.Count;
										while (true)
										{
											if (count6-- > 0)
											{
												Part part2 = list2[count6];
												if (!(part2.partInfo.name != "solidBooster") || !(part2.partInfo.name != "solidBooster.v2"))
												{
													if (part2.inverseStage != part7.inverseStage || part2.inverseStage <= list[0].inverseStage)
													{
														break;
													}
													ModuleEngines module2 = part2.Modules.GetModule<ModuleEngines>();
													if (!(module2 == null) && !(module2.thrustPercentage < 49.4f) && module2.thrustPercentage <= 50.7f)
													{
														if (part2.children.Count != 1 || part2.children[0].partInfo.name != "noseCone")
														{
															return false;
														}
														continue;
													}
													return false;
												}
												return false;
											}
											return true;
										}
										return false;
									}
									return false;
								}
								return false;
							}
							return false;
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public bool CheckAbort()
	{
		if (!CheckChute())
		{
			return false;
		}
		Part attachedPart = EditorLogic.RootPart.FindAttachNode("bottom").attachedPart;
		if (!(attachedPart == null) && !(attachedPart.partInfo.name != "Decoupler.1") && !(attachedPart.FindAttachNodeByPart(EditorLogic.RootPart).id != "top"))
		{
			Part attachedPart2 = attachedPart.FindAttachNode("bottom").attachedPart;
			if (!(attachedPart2 == null) && !(attachedPart2.partInfo.name != "RCSFuelTank") && !(attachedPart2.Resources["MonoPropellant"].amount > 102.0) && attachedPart2.Resources["MonoPropellant"].amount >= 90.0)
			{
				int num = 0;
				Part part = null;
				bool flag = true;
				if (attachedPart2.children.Count != 1 && attachedPart2.children.Count != 5)
				{
					return false;
				}
				int count = attachedPart2.children.Count;
				while (count-- > 0)
				{
					Part part2 = attachedPart2.children[count];
					if (part2.partInfo.name == "fuelTankSmallFlat" && attachedPart2.FindAttachNodeByPart(part2) != null)
					{
						part = part2;
						flag = false;
					}
					else if (part2.partInfo.name == "RCSBlock" || part2.partInfo.name == "RCSBlock.v2")
					{
						num++;
					}
				}
				if (flag)
				{
					return false;
				}
				Part part3 = null;
				flag = true;
				int count2 = part.children.Count;
				while (count2-- > 0)
				{
					Part part2 = part.children[count2];
					if (part2.partInfo.name == "fuelTank" && part.FindAttachNodeByPart(part2) != null)
					{
						part3 = part2;
						flag = false;
					}
					else if (part2.partInfo.name == "RCSBlock" || part2.partInfo.name == "RCSBlock.v2")
					{
						num++;
					}
				}
				if (flag)
				{
					return false;
				}
				if (part.Resources["LiquidFuel"].amount + part.Resources["Oxidizer"].amount + part3.Resources["LiquidFuel"].amount + part3.Resources["Oxidizer"].amount < 499.0)
				{
					return false;
				}
				if (part3.children.Count != 9 && part3.children.Count != 13)
				{
					return false;
				}
				bool flag2 = true;
				int num2 = 0;
				int num3 = 0;
				Part part4 = null;
				int count3 = part3.children.Count;
				while (count3-- > 0)
				{
					Part part2 = part3.children[count3];
					if ((part2.partInfo.name == "liquidEngine3" || part2.partInfo.name == "liquidEngine3.v2") && part2.FindAttachNodeByPart(part3).id == "top")
					{
						part4 = part2;
						flag2 = false;
					}
					if (!(part2.partInfo.name == "RCSBlock") && !(part2.partInfo.name == "RCSBlock.v2"))
					{
						if (part2.partInfo.name == "batteryPack")
						{
							num2++;
						}
						else if (part2.partInfo.name == "solarPanels5")
						{
							num3++;
						}
					}
					else
					{
						num++;
					}
				}
				if (!(num != 4 || flag2) && num3 == 4 && num2 == 4)
				{
					Part attachedPart3 = part4.FindAttachNode("bottom").attachedPart;
					if (!(attachedPart3 == null) && !(attachedPart3.partInfo.name != "Decoupler.1") && !(attachedPart3.FindAttachNodeByPart(part4).id != "top"))
					{
						if (attachedPart3.children.Count != 1)
						{
							return false;
						}
						Part part5 = attachedPart3.children[0];
						if (attachedPart3.FindAttachNodeByPart(part5) == null)
						{
							return false;
						}
						if (part5.children.Count != 1)
						{
							return false;
						}
						Part part6 = part5.children[0];
						if (part5.FindAttachNodeByPart(part6) == null)
						{
							return false;
						}
						if (!(part5.partInfo.name != "fuelTank") && !(part6.partInfo.name != "fuelTank"))
						{
							Part part7 = null;
							flag2 = true;
							List<Part> list = new List<Part>();
							int num4 = 0;
							int count4 = part6.children.Count;
							while (count4-- > 0)
							{
								Part part2 = part6.children[count4];
								if ((part2.partInfo.name == "liquidEngine2" || part2.partInfo.name == "liquidEngine2.v2") && part2.FindAttachNodeByPart(part6).id == "top")
								{
									flag2 = false;
									part7 = part2;
								}
								else if (part2.partInfo.name == "radialDecoupler")
								{
									list.Add(part2);
								}
								else if (part2.partInfo.name == "R8winglet")
								{
									num4++;
								}
							}
							if (!flag2 && num4 == 4)
							{
								ModuleEngines module = part7.Modules.GetModule<ModuleEngines>();
								if (!(module == null) && !(module.thrustPercentage > 65.7f) && module.thrustPercentage >= 64.2f)
								{
									if (list.Count != 2)
									{
										return false;
									}
									List<Part> list2 = new List<Part>();
									int count5 = list.Count;
									while (true)
									{
										if (count5-- > 0)
										{
											Part part2 = list[count5];
											if (part2.children.Count != 1)
											{
												break;
											}
											list2.Add(part2.children[0]);
											continue;
										}
										if (list2.Count != 2)
										{
											return false;
										}
										int count6 = list2.Count;
										while (true)
										{
											if (count6-- > 0)
											{
												Part part2 = list2[count6];
												if (!(part2.partInfo.name != "solidBooster") || !(part2.partInfo.name != "solidBooster.v2"))
												{
													if (part2.inverseStage != part7.inverseStage || part2.inverseStage <= list[0].inverseStage)
													{
														break;
													}
													ModuleEngines module2 = part2.Modules.GetModule<ModuleEngines>();
													if (!(module2 == null) && !(module2.thrustPercentage < 49.4f) && module2.thrustPercentage <= 50.7f)
													{
														if (part2.children.Count != 1 || part2.children[0].partInfo.name != "noseCone")
														{
															return false;
														}
														continue;
													}
													return false;
												}
												return false;
											}
											List<BaseAction> list3 = BaseAction.CreateActionList(EditorLogic.fetch.ship.parts, KSPActionGroup.Abort, 0, overrideDefault: false, include: true);
											if (list3.Count != 5)
											{
												return false;
											}
											bool flag3 = true;
											bool flag4 = true;
											bool flag5 = true;
											int num5 = 0;
											int count7 = list3.Count;
											while (count7-- > 0)
											{
												BaseAction baseAction = list3[count7];
												string text = baseAction.listParent.part.partInfo.name;
												if (baseAction.name == "DecoupleAction")
												{
													if (text == "Decoupler.1")
													{
														flag3 = false;
													}
													else if (text == "radialDecoupler")
													{
														num5++;
													}
												}
												else if (baseAction.name == "ShutdownAction")
												{
													switch (text)
													{
													case "liquidEngine3":
													case "liquidEngine3.v2":
														flag4 = false;
														break;
													case "liquidEngine2":
													case "liquidEngine2.v2":
														flag5 = false;
														break;
													}
												}
											}
											if (!(flag3 || flag4 || flag5) && num5 == 2)
											{
												return true;
											}
											return false;
										}
										return false;
									}
									return false;
								}
								return false;
							}
							return false;
						}
						return false;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public override void OnTutorialSetup()
	{
		if (complete)
		{
			CloseTutorialWindow();
			return;
		}
		TutorialPage tutorialPage = new TutorialPage("welcome");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_312688");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			HighLogic.CurrentGame.Parameters.SpaceCenter.CanGoInVAB = true;
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_312696"), expandW: false, expandH: true));
		tutorialPage.SetAdvanceCondition((KFSMState _003Cp0_003E) => HighLogic.LoadedScene == GameScenes.EDITOR);
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("overview");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_312709");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			EditorDriver.fetch.SetInputLockFromGameParameters();
			EditorPartList.Instance.ExcludeFilters.AddFilter(tutorialFilterAdv);
			EditorPartList.Instance.Refresh();
			InputLockManager.SetControlLock(ControlTypes.EDITOR_ROOT_REFLOW, "tutorialEditor_Reroot");
			InputLockManager.SetControlLock(ControlTypes.EDITOR_MODE_SWITCH, "tutorialEditor_Modes");
			InputLockManager.SetControlLock(ControlTypes.EDITOR_LAUNCH, "tutorialEditor_Launch");
			InputLockManager.SetControlLock(ControlTypes.EDITOR_ICON_HOVER | ControlTypes.EDITOR_ICON_PICK | ControlTypes.EDITOR_SAVE | ControlTypes.EDITOR_LOAD | ControlTypes.EDITOR_NEW | ControlTypes.EDITOR_PAD_PICK_PLACE | ControlTypes.EDITOR_PAD_PICK_COPY | ControlTypes.EDITOR_SYM_SNAP_UI | ControlTypes.EDITOR_EDIT_STAGES | ControlTypes.EDITOR_UNDO_REDO, "tutorialEditor_Most");
			if (EditorLogic.fetch != null)
			{
				EditorLogic.fetch.disallowSave = true;
			}
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_312730"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_312731"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("undoRedo");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_312744");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_idle_lookAround);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_312752", tutorialControlColorString, (Application.platform == RuntimePlatform.OSXPlayer) ? Localizer.Format("#autoLOC_7003403") : Localizer.Format("#autoLOC_5030004"), tutorialControlColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_312753"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("grabPod");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_312766");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			InputLockManager.RemoveControlLock("tutorialEditor_Most");
			if (EditorLogic.fetch != null)
			{
				EditorLogic.fetch.disallowSave = false;
			}
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_312777", tutorialControlColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_312778"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => CheckPod() && AttachedNum(1), dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("addChute");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_312794");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_thumbUp);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_312803"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_312804"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => CheckChute() && AttachedNum(2), dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("chuteParams");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_312820");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_312828", tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_312829"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => CheckChuteStats() && AttachedNum(2), dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("overviewUpper");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_312845");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_nodA);
			InputLockManager.SetControlLock(ControlTypes.EDITOR_ICON_HOVER | ControlTypes.EDITOR_ICON_PICK | ControlTypes.EDITOR_SAVE | ControlTypes.EDITOR_LOAD | ControlTypes.EDITOR_NEW | ControlTypes.EDITOR_PAD_PICK_PLACE | ControlTypes.EDITOR_PAD_PICK_COPY | ControlTypes.EDITOR_SYM_SNAP_UI | ControlTypes.EDITOR_EDIT_STAGES | ControlTypes.EDITOR_UNDO_REDO, "tutorialEditor_Most");
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_312855"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_312856"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("addDecoupler");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_312869");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_nodB);
			InputLockManager.RemoveControlLock("tutorialEditor_Most");
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_312879", tutorialControlColorString, GameSettings.Editor_pitchDown.name, tutorialControlColorString, GameSettings.Editor_yawLeft.name, tutorialControlColorString, GameSettings.Editor_pitchUp.name, tutorialControlColorString, GameSettings.Editor_yawRight.name), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_312880"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => CheckDecouplerPod() && AttachedNum(3), dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("addRCSTank");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_312896");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_312904", tutorialHighlightColorString, tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_312905"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => CheckRCSTank() && AttachedNum(4), dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("tweakRCSTank");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_312921");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_312929", tutorialControlColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_312930"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => CheckRCSTankLoading() && AttachedNum(4), dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("addT100");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_312946");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_thumbUp);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_312954", tutorialHighlightColorString, tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_312955"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => CheckTanks() && AttachedNum(6), dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("add909");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_312971");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_312979", tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_312980"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => CheckEngineUpper() && AttachedNum(7), dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("turnOnCoM");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_312996");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_thumbsUp);
			InputLockManager.SetControlLock(ControlTypes.EDITOR_ICON_HOVER | ControlTypes.EDITOR_ICON_PICK | ControlTypes.EDITOR_SAVE | ControlTypes.EDITOR_LOAD | ControlTypes.EDITOR_NEW | ControlTypes.EDITOR_PAD_PICK_PLACE | ControlTypes.EDITOR_PAD_PICK_COPY | ControlTypes.EDITOR_SYM_SNAP_UI | ControlTypes.EDITOR_EDIT_STAGES | ControlTypes.EDITOR_UNDO_REDO, "tutorialEditor_Most");
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_313006", tutorialControlColorString), expandW: false, expandH: true), new DialogGUIHorizontalLayout(), new DialogGUIImage(new Vector2(92f, 32f), Vector2.zero, Color.white, CoMicon), new DialogGUILayoutEnd(), new DialogGUIButton(Localizer.Format("#autoLOC_313010"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => EditorVesselOverlays.fetch == null || EditorVesselOverlays.fetch.CoMmarker.gameObject.activeInHierarchy, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("moveCoM");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_313026");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			InputLockManager.RemoveControlLock("tutorialEditor_Most");
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_313036", tutorialControlColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_313037"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => CheckTanksDry() && AttachedNum(7), dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("refillTank");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_313053");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_idle_lookAround);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_313061"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_313062"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => CheckTanksWet() && AttachedNum(7), dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("placeRCS");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_313078");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			InputLockManager.SetControlLock(ControlTypes.EDITOR_ICON_HOVER | ControlTypes.EDITOR_ICON_PICK | ControlTypes.EDITOR_SAVE | ControlTypes.EDITOR_LOAD | ControlTypes.EDITOR_NEW | ControlTypes.EDITOR_PAD_PICK_PLACE | ControlTypes.EDITOR_PAD_PICK_COPY | ControlTypes.EDITOR_SYM_SNAP_UI | ControlTypes.EDITOR_EDIT_STAGES | ControlTypes.EDITOR_UNDO_REDO, "tutorialEditor_Most");
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_313088"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_313089"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => CheckTanksWet() && (AttachedNum(7) || AttachedNum(11)), dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("placeRCS2");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_313105");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			InputLockManager.RemoveControlLock("tutorialEditor_Most");
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_313115", tutorialControlColorString, GameSettings.Editor_toggleAngleSnap.name, tutorialHighlightColorString, tutorialHighlightColorString, tutorialControlColorString, GameSettings.Editor_resetRotation.name), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_313116"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => CheckRCSThrusters() && AttachedNum(11) && (EditorVesselOverlays.fetch == null || !EditorVesselOverlays.fetch.CoMmarker.gameObject.activeInHierarchy), dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("placeElec");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_313132");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_idle_sigh);
			InputLockManager.SetControlLock(ControlTypes.EDITOR_ICON_HOVER | ControlTypes.EDITOR_ICON_PICK | ControlTypes.EDITOR_SAVE | ControlTypes.EDITOR_LOAD | ControlTypes.EDITOR_NEW | ControlTypes.EDITOR_PAD_PICK_PLACE | ControlTypes.EDITOR_PAD_PICK_COPY | ControlTypes.EDITOR_SYM_SNAP_UI | ControlTypes.EDITOR_EDIT_STAGES | ControlTypes.EDITOR_UNDO_REDO, "tutorialEditor_Most");
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_313143"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_313144"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("placeElec2");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_313157");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			InputLockManager.RemoveControlLock("tutorialEditor_Most");
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_313166", tutorialHighlightColorString, tutorialHighlightColorString, tutorialControlColorString, GameSettings.Editor_pitchDown.name, tutorialControlColorString, GameSettings.Editor_yawLeft.name, tutorialControlColorString, GameSettings.Editor_pitchUp.name, tutorialControlColorString, GameSettings.Editor_yawRight.name, tutorialControlColorString, GameSettings.Editor_rollLeft.name, tutorialControlColorString, GameSettings.Editor_rollRight.name, tutorialHighlightColorString, tutorialControlColorString, GameSettings.Editor_yawLeft.name, tutorialControlColorString, tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_313167"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => CheckElectric() && AttachedNum(19), dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("summaryUpper");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_313183");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_thumbUp);
			InputLockManager.SetControlLock(ControlTypes.EDITOR_ICON_HOVER | ControlTypes.EDITOR_ICON_PICK | ControlTypes.EDITOR_SAVE | ControlTypes.EDITOR_LOAD | ControlTypes.EDITOR_NEW | ControlTypes.EDITOR_PAD_PICK_PLACE | ControlTypes.EDITOR_PAD_PICK_COPY | ControlTypes.EDITOR_SYM_SNAP_UI | ControlTypes.EDITOR_EDIT_STAGES | ControlTypes.EDITOR_UNDO_REDO, "tutorialEditor_Most");
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_313194"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_313195"), delegate
		{
			Tutorial.GoToNextPage();
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("addDecouplerLower");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_313208");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			InputLockManager.RemoveControlLock("tutorialEditor_Most");
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_313218", tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_313219"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => CheckLowerDec() && AttachedNum(20), dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("addLowerTanks");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_313235");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_nodB);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_313243", tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_313244"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => CheckLowerTanks() && AttachedNum(22), dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("addT45");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_313260");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_313268", tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_313269"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => CheckLowerEngine() && AttachedNum(23), dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("tweakT45");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_313285");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_313293", tutorialControlColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_313294"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => CheckLowerEngineStats() && AttachedNum(23), dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("addRadials");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_313310");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_nodA);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_313318", tutorialHighlightColorString, tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_313319"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => CheckRadialDecs() && AttachedNum(25), dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("addRT10s");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_313335");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_313343", tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_313344"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => CheckSolids() && AttachedNum(27), dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("tweakRT10s");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_313359");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_313367", tutorialControlColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_313368"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => CheckSolidsStats() && AttachedNum(27), dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("addNosecones");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_313384");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_313392", tutorialHighlightColorString, tutorialHighlightColorString, tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_313393"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => CheckNosecones() && AttachedNum(29), dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("addFins");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_313409");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_313417", tutorialHighlightColorString, tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_313418"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => CheckFins() && AttachedNum(33), dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("changeStaging");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_313434");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_thumbUp);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_313442"), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_313443"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => CheckT45Staging() && AttachedNum(33), dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("rotateVessel");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_313459");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_313467", tutorialControlColorString, GameSettings.Editor_rollLeft.name, tutorialControlColorString, GameSettings.Editor_rollRight.name), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_313468"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => CheckT45Staging() && AttachedNum(33) && (EditorLogic.RootPart.transform.right.z > 0.99f || EditorLogic.RootPart.transform.right.z < -0.99f), dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("actionEditor");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_313484");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			InputLockManager.RemoveControlLock("tutorialEditor_Modes");
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_313493", tutorialControlColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_313494"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => CheckT45Staging() && AttachedNum(33) && EditorLogic.fetch.editorScreen == EditorScreen.Actions, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("actionGroups");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_313510");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_313518", tutorialControlColorString, GameSettings.AbortActionGroup.name, tutorialControlColorString, tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_313519"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => CheckT45Staging() && AttachedNum(33) && EditorLogic.fetch.editorScreen == EditorScreen.Actions && EditorActionGroups.Instance.SelectedGroup == KSPActionGroup.Abort, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("actionAbort");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_313535");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_nodB);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_313543", tutorialControlColorString, tutorialHighlightColorString, tutorialControlColorString, tutorialHighlightColorString, tutorialControlColorString, tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_313544"), delegate
		{
			Tutorial.GoToNextPage();
		}, () => CheckAbort() && AttachedNum(33), dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		tutorialPage = new TutorialPage("summary");
		tutorialPage.windowTitle = Localizer.Format("#autoLOC_313560");
		tutorialPage.OnEnter = delegate
		{
			instructor.StopRepeatingEmote();
			instructor.PlayEmote(instructor.anim_true_thumbsUp);
		};
		tutorialPage.dialog = new MultiOptionDialog(tutorialPage.name, "", "", null, dRect, new DialogGUIVerticalLayout(sw: true), new DialogGUILabel(Localizer.Format("#autoLOC_313568", tutorialHighlightColorString), expandW: false, expandH: true), new DialogGUIButton(Localizer.Format("#autoLOC_313569"), delegate
		{
			CompleteTutorial();
		}, dismissOnSelect: true));
		Tutorial.AddPage(tutorialPage);
		if (!HighLogic.LoadedSceneIsEditor)
		{
			stateName = "welcome";
		}
		else
		{
			stateName = "overview";
		}
		Tutorial.StartTutorial(stateName);
	}

	public override void OnSave(ConfigNode node)
	{
		stateName = GetCurrentStateName();
		if (stateName != null)
		{
			node.AddValue("statename", stateName);
		}
		node.AddValue("complete", complete);
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("statename"))
		{
			stateName = node.GetValue("statename");
		}
		if (node.HasValue("complete"))
		{
			complete = bool.Parse(node.GetValue("complete"));
		}
	}

	public override void OnOnDestroy()
	{
		if (EditorPartList.Instance != null)
		{
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilter_none);
			EditorPartList.Instance.ExcludeFilters.RemoveFilter(tutorialFilterAdv);
			try
			{
				EditorPartList.Instance.Refresh();
			}
			catch
			{
			}
		}
		InputLockManager.RemoveControlLock("tutorialEditor_Stages");
		InputLockManager.RemoveControlLock("tutorialEditor_Name");
		InputLockManager.RemoveControlLock("tutorialEditor_Reroot");
		InputLockManager.RemoveControlLock("tutorialEditor_Modes");
		InputLockManager.RemoveControlLock("tutorialEditor_Copy");
		InputLockManager.RemoveControlLock("tutorialEditor_Save");
		if (EditorLogic.fetch != null)
		{
			EditorLogic.fetch.disallowSave = false;
		}
		InputLockManager.RemoveControlLock("tutorialEditor_PickPart");
		InputLockManager.RemoveControlLock("tutorialEditor_PickIcon");
		InputLockManager.RemoveControlLock("tutorialEditor_Most");
		InputLockManager.RemoveControlLock("tutorialEditor_Launch");
	}
}
