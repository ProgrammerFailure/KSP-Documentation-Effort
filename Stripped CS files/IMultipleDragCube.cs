public interface IMultipleDragCube
{
	bool IsMultipleCubesActive { get; }

	string[] GetDragCubeNames();

	void AssumeDragCubePosition(string name);

	bool UsesProceduralDragCubes();
}
