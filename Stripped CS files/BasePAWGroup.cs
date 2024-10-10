public class BasePAWGroup
{
	public string name;

	public string displayName;

	public bool startCollapsed;

	public BasePAWGroup(string name, string displayName, bool startCollapsed)
	{
		this.name = name;
		this.displayName = displayName;
		this.startCollapsed = startCollapsed;
	}

	public BasePAWGroup()
	{
		name = "";
		displayName = "";
		startCollapsed = false;
	}
}
