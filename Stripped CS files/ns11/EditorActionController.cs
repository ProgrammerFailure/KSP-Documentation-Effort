using Expansions.Serenity;
using ns9;
using UnityEngine;

namespace ns11;

public class EditorActionController : EditorActionGroup_Base
{
	public uint PartId { get; set; }

	public ModuleRoboticController controller { get; set; }

	public void Setup(string groupName, bool contains)
	{
		base.groupName.text = Localizer.Format(groupName);
		if (contains)
		{
			base.groupName.color = Color.yellow;
		}
		else
		{
			base.groupName.color = Color.white;
		}
	}

	public void Setup(ModuleRoboticController controller, bool contains)
	{
		PartId = controller.PartPersistentId;
		this.controller = controller;
		_type = EditorActionGroupType.Controller;
		Setup(controller.displayName, contains);
	}
}
