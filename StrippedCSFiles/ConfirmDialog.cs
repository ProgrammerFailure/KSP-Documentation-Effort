using System.Runtime.CompilerServices;
using UnityEngine;

internal class ConfirmDialog
{
	public UISkinDef skin;

	public Callback confirmed;

	public Callback abort;

	public Rect dialogRect;

	public string windowTitle;

	public string message;

	public string confirmButtonText;

	public string abortButtonText;

	public Callback DrawCustomContent;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfirmDialog(Callback confirmCallback, Callback abortCallback, UISkinDef UISkinDef = null)
	{
		throw null;
	}
}
