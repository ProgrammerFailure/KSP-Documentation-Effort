using System;
using System.Runtime.CompilerServices;
using KSP.UI;

[Serializable]
public abstract class TransferDataTopDataBase : AppUI_Data
{
	public Callback dataChangedCallback;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TransferDataTopDataBase(Callback dataChangedCallback)
	{
		throw null;
	}
}
