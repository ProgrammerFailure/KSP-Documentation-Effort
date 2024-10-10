using ns2;
using TMPro;

namespace ns11;

public class EditorActionGroup_Base : UISelectableGridLayoutGroupItem
{
	public EditorActionGroupType _type;

	public TextMeshProUGUI groupName;

	public EditorActionGroupType type
	{
		get
		{
			return _type;
		}
		set
		{
			_type = value;
		}
	}
}
