using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class CheckpointEntry : MonoBehaviour
{
	public string fullFilePath;

	public Button btnRemove;

	public GameObject warningMarker;

	private Callback<CheckpointEntry> onSelected;

	private Callback<CheckpointEntry> onRemove;

	public bool isValid;

	public bool isCheckpointDirty;

	[SerializeField]
	private Toggle tgtCtrl;

	[SerializeField]
	private TextMeshProUGUI title1;

	[SerializeField]
	private TextMeshProUGUI title2;

	[SerializeField]
	private TextMeshProUGUI fieldMsg;

	public Toggle Toggle
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Game CheckpointGame
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	public Mission CheckpointMission
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	internal LoadGameDialog.PlayerProfileInfo CheckpointMeta
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CheckpointEntry()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CheckpointEntry Create(string checkpointFile, Callback<CheckpointEntry> onSelected, Callback<CheckpointEntry> onRemove)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Init(string checkpointFile)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Validate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Game GetSaveGame(string checkpointFile)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal LoadGameDialog.PlayerProfileInfo GetSaveGameMeta(string checkpointFile)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void LoadMission()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected ConfigNode GetMissionNode(Game checkpointGame)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnValueChanged(bool st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnButtonRemove()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Terminate()
	{
		throw null;
	}
}
