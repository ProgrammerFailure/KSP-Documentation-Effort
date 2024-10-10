using System;
using ns2;

[Serializable]
public class TransferDataSimpleTopData : TransferDataTopDataBase
{
	public class APPUIMemberLabelListItem
	{
		public string column1;

		public string column2;

		public string column3;
	}

	[AppUI_LabelList(preferredHeight = 500f, showSeparator = true, guiNameVertAlignment = AppUI_Control.VerticalAlignment.Top, showGuiName = true, guiName = "", guiNameHorizAlignment = AppUI_Control.HorizontalAlignment.Left, textAlignment = AppUI_Control.HorizontalAlignment.Right)]
	public APPUIMemberLabelListItem transferListData;

	public TransferDataSimpleTopData(Callback dataChangedCallback)
		: base(dataChangedCallback)
	{
		transferListData = new APPUIMemberLabelListItem();
	}
}
