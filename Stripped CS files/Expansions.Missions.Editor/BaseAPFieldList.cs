namespace Expansions.Missions.Editor;

public class BaseAPFieldList : BaseFieldList<BaseAPField, MEGUI_Control>
{
	public BaseAPFieldList()
	{
	}

	public BaseAPFieldList(object host)
		: base(host, ignoreUIControl: true)
	{
	}
}
