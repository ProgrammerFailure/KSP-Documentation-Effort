using System.IO;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI;

public class DialogUISelectFile : MonoBehaviour
{
	public delegate void OnFileSelected(FileInfo file);

	public DialogUISelectFileItem itemPrefab;

	public TextMeshProUGUI textTitle;

	public TextMeshProUGUI textDescription;

	public RectTransform layout;

	public Button buttonAccept;

	public Button buttonCancel;

	public string filePath;

	public string fileSearchPattern;

	public OnFileSelected onFileSelected;

	private FileInfo selectedItem;

	private FileInfo[] files;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogUISelectFile()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(string title, string description, string filePath, string fileSearchPattern, OnFileSelected onFileSelected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateFiles()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Destroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAccept()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCancel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SelectItem(int index)
	{
		throw null;
	}
}
