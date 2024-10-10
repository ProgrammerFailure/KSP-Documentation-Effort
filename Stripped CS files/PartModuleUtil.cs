public static class PartModuleUtil
{
	public static string PrintResourceRequirements(string caption = "Requires:", params ModuleResource[] reqs)
	{
		return PrintResourceRequirements(showFlowModeDesc: false, caption, reqs);
	}

	public static string PrintResourceRequirements(bool showFlowModeDesc, string caption = "Requires:", params ModuleResource[] reqs)
	{
		return PrintResourceRequirements(showFlowModeDesc, caption, "#99ff00ff", reqs);
	}

	public static string PrintResourceRequirements(string caption = "Requires", string color = "#99ff00ff", params ModuleResource[] reqs)
	{
		return PrintResourceRequirements(showFlowModeDesc: false, caption, "#99ff00ff", reqs);
	}

	public static string PrintResourceRequirements(bool showFlowModeDesc, string caption = "Requires", string color = "#99ff00ff", params ModuleResource[] reqs)
	{
		string text = "";
		if (reqs.Length != 0)
		{
			text = text + "\n<b><color=" + color + ">" + caption + "</color></b>\n";
			foreach (ModuleResource moduleResource in reqs)
			{
				text += moduleResource.PrintRate(showFlowModeDesc);
			}
		}
		return text;
	}

	public static string PrintResourceRequirements(string caption = "Requires:", double mult = 1.0, params ModuleResource[] reqs)
	{
		return PrintResourceRequirements(showFlowModeDesc: false, caption, "#99ff00ff", mult, reqs);
	}

	public static string PrintResourceRequirements(bool showFlowModeDesc, string caption = "Requires:", double mult = 1.0, params ModuleResource[] reqs)
	{
		return PrintResourceRequirements(showFlowModeDesc, caption, "#99ff00ff", mult, reqs);
	}

	public static string PrintResourceRequirements(string caption = "Requires:", string color = "#99ff00ff", double mult = 1.0, params ModuleResource[] reqs)
	{
		return PrintResourceRequirements(showFlowModeDesc: false, caption, color, mult, reqs);
	}

	public static string PrintResourceRequirements(bool showFlowModeDesc, string caption = "Requires", string color = "#99ff00ff", double mult = 1.0, params ModuleResource[] reqs)
	{
		string text = "";
		if (reqs.Length != 0)
		{
			text = text + "\n<b><color=" + color + ">" + caption + "</color></b>\n";
			foreach (ModuleResource moduleResource in reqs)
			{
				text += moduleResource.PrintRate(showFlowModeDesc, mult);
			}
		}
		return text;
	}
}
