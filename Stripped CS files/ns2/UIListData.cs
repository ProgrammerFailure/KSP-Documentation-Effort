namespace ns2;

public class UIListData<T>
{
	public T data;

	public UIListItem listItem;

	public UIListData(T data, UIListItem listItem)
	{
		this.data = data;
		this.listItem = listItem;
	}
}
