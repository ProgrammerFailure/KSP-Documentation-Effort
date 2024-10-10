using ns23;
using UnityEngine;

public interface ISiteNode
{
	string GetName();

	void UpdateNodeCaption(MapNode mn, MapNode.CaptionData data);

	Transform GetWorldPos();
}
