using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TMPro;

[Serializable]
public class TMP_TextInfo
{
	private static Vector2 k_InfinityVectorPositive;

	private static Vector2 k_InfinityVectorNegative;

	public TMP_Text textComponent;

	public int characterCount;

	public int spriteCount;

	public int spaceCount;

	public int wordCount;

	public int linkCount;

	public int lineCount;

	public int pageCount;

	public int materialCount;

	public TMP_CharacterInfo[] characterInfo;

	public TMP_WordInfo[] wordInfo;

	public TMP_LinkInfo[] linkInfo;

	public TMP_LineInfo[] lineInfo;

	public TMP_PageInfo[] pageInfo;

	public TMP_MeshInfo[] meshInfo;

	private TMP_MeshInfo[] m_CachedMeshInfo;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TMP_TextInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TMP_TextInfo(TMP_Text textComponent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static TMP_TextInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Clear()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearMeshInfo(bool updateMesh)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearAllMeshInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetVertexLayout(bool isVolumetric)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearUnusedVertices(MaterialReference[] materials)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearLineInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TMP_MeshInfo[] CopyMeshInfoVertexData()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Resize<T>(ref T[] array, int size)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Resize<T>(ref T[] array, int size, bool isBlockAllocated)
	{
		throw null;
	}
}
