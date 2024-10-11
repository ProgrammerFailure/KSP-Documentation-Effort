using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions;

public class ExpansionSO : ScriptableObject
{
	[Serializable]
	public class BundleInfo
	{
		[HideInInspector]
		public string name;

		[HideInInspector]
		public string fileSignature;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public BundleInfo(string name, string fileSignature)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public BundleInfo()
		{
			throw null;
		}
	}

	[SerializeField]
	protected string _displayName;

	[SerializeField]
	protected string _folderName;

	[SerializeField]
	protected string _version;

	[SerializeField]
	protected string _kspVersion;

	[SerializeField]
	protected ProtoCrewMember.KerbalSuit[] _kerbalSuits;

	[SerializeField]
	[HideInInspector]
	protected List<BundleInfo> _bundles;

	public string DisplayName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string FolderName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string Version
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string KSPVersion
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ProtoCrewMember.KerbalSuit[] KerbalSuits
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<BundleInfo> Bundles
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ExpansionSO()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void CreateExpansion(string name, string folderName, string expansionVersion, string kspVersion, List<BundleInfo> bundeInfoList, ProtoCrewMember.KerbalSuit[] kerbalSuits)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetBundleInfoList(List<BundleInfo> bundleInfoList)
	{
		throw null;
	}
}
