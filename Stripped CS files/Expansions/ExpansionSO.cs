using System;
using System.Collections.Generic;
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

		public BundleInfo(string name, string fileSignature)
		{
			this.name = name;
			this.fileSignature = fileSignature;
		}

		public BundleInfo()
		{
			name = null;
			fileSignature = null;
		}
	}

	[SerializeField]
	public string _displayName;

	[SerializeField]
	public string _folderName;

	[SerializeField]
	public string _version;

	[SerializeField]
	public string _kspVersion;

	[SerializeField]
	public ProtoCrewMember.KerbalSuit[] _kerbalSuits;

	[SerializeField]
	[HideInInspector]
	public List<BundleInfo> _bundles;

	public string DisplayName => _displayName;

	public string FolderName => _folderName;

	public string Version => _version;

	public string KSPVersion => _kspVersion;

	public ProtoCrewMember.KerbalSuit[] KerbalSuits => _kerbalSuits;

	public List<BundleInfo> Bundles => _bundles;

	public virtual void CreateExpansion(string name, string folderName, string expansionVersion, string kspVersion, List<BundleInfo> bundeInfoList, ProtoCrewMember.KerbalSuit[] kerbalSuits)
	{
		_displayName = name;
		_folderName = folderName;
		_version = expansionVersion;
		_kspVersion = kspVersion;
		_bundles = bundeInfoList;
		_kerbalSuits = kerbalSuits;
	}

	public virtual void SetBundleInfoList(List<BundleInfo> bundleInfoList)
	{
		_bundles = bundleInfoList;
	}
}
