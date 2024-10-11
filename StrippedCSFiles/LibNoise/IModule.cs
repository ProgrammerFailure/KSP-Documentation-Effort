using UnityEngine;

namespace LibNoise;

public interface IModule
{
	double GetValue(double x, double y, double z);

	double GetValue(Vector3d coordinate);

	double GetValue(Vector3 coordinate);
}
