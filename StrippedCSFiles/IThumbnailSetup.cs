using UnityEngine;

public interface IThumbnailSetup
{
	void AssumeSnapshotPosition(GameObject icon, ProtoPartSnapshot protoPart);

	string ThumbSuffix(ProtoPartSnapshot protoPart);
}
