using System;
using System.Runtime.CompilerServices;
using KSP.UI;

[Serializable]
public class TransferDataSimpleTopData : TransferDataTopDataBase
{
	public class APPUIMemberLabelListItem
	{
		public string column1;

		public string column2;

		public string column3;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public APPUIMemberLabelListItem()
		{
			throw null;
		}
	}

	[AppUI_LabelList(preferredHeight = 500f, showSeparator = true, guiNameVertAlignment = AppUI_Control.VerticalAlignment.Top, showGuiName = true, guiName = "", guiNameHorizAlignment = AppUI_Control.HorizontalAlignment.Left, textAlignment = AppUI_Control.HorizontalAlignment.Right)]
	public APPUIMemberLabelListItem transferListData;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TransferDataSimpleTopData(Callback dataChangedCallback)
	{
		throw null;
	}
}
