using System;
using ns2;

[Serializable]
public abstract class TransferDataTopDataBase : AppUI_Data
{
	public Callback dataChangedCallback;

	public TransferDataTopDataBase(Callback dataChangedCallback)
	{
		this.dataChangedCallback = dataChangedCallback;
	}
}
