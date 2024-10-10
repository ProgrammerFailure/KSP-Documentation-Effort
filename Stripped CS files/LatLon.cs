using System;
using UnityEngine;

public static class LatLon
{
	public static Vector3d GetRelSurfaceNVector(double lat, double lon)
	{
		lat *= Math.PI / 180.0;
		lon *= Math.PI / 180.0;
		return Planetarium.SphericalVector(lat, lon).xzy;
	}

	public static Vector3d GetSurfaceNVector(Planetarium.CelestialFrame BodyFrame, double lat, double lon)
	{
		lat *= Math.PI / 180.0;
		lon *= Math.PI / 180.0;
		Vector3d r = Planetarium.SphericalVector(lat, lon);
		return BodyFrame.LocalToWorld(r).xzy;
	}

	public static Vector3d GetRelSurfacePosition(Planetarium.CelestialFrame BodyFrame, double Radius, double lat, double lon, double alt)
	{
		return GetSurfaceNVector(BodyFrame, lat, lon) * (Radius + alt);
	}

	public static Vector3d GetRelSurfacePosition(Planetarium.CelestialFrame BodyFrame, double Radius, double lat, double lon, double alt, out Vector3d normal)
	{
		normal = GetSurfaceNVector(BodyFrame, lat, lon);
		return normal * (Radius + alt);
	}

	public static Vector3d GetRelSurfacePosition(Planetarium.CelestialFrame BodyFrame, Vector3d bodyPosition, Vector3d worldPosition)
	{
		return BodyFrame.WorldToLocal((worldPosition - bodyPosition).xzy).xzy;
	}

	public static Vector3d GetWorldSurfacePosition(Planetarium.CelestialFrame BodyFrame, Vector3 bodyPosition, double radius, double lat, double lon, double alt)
	{
		return GetRelSurfacePosition(BodyFrame, radius, lat, lon, alt) + bodyPosition;
	}

	public static double GetLatitude(Planetarium.CelestialFrame BodyFrame, Vector3d bodyPosition, Vector3d pos, bool isRadial = false)
	{
		double num = Math.Asin(BodyFrame.WorldToLocal((isRadial ? pos.normalized : (pos - bodyPosition).normalized).xzy).z) * 57.295780181884766;
		if (double.IsNaN(num))
		{
			num = 0.0;
		}
		return num;
	}

	public static double GetLongitude(Planetarium.CelestialFrame BodyFrame, Vector3d bodyPosition, Vector3d pos, bool isRadial = false)
	{
		Vector3d vector3d = BodyFrame.WorldToLocal((isRadial ? pos.normalized : (pos - bodyPosition).normalized).xzy);
		double num = Math.Atan2(vector3d.y, vector3d.x) * 57.295780181884766;
		if (double.IsNaN(num))
		{
			num = 0.0;
		}
		return num;
	}

	public static Vector2d GetLatitudeAndLongitude(Planetarium.CelestialFrame BodyFrame, Vector3d bodyPosition, Vector3d pos, bool isRadial = false)
	{
		Vector3d vector3d = BodyFrame.WorldToLocal((isRadial ? pos.normalized : (pos - bodyPosition).normalized).xzy);
		double num = Math.Asin(vector3d.z) * 57.295780181884766;
		double num2 = Math.Atan2(vector3d.y, vector3d.x) * 57.295780181884766;
		if (double.IsNaN(num))
		{
			num = 0.0;
		}
		if (double.IsNaN(num2))
		{
			num2 = 0.0;
		}
		return new Vector2d(num, num2);
	}

	public static bool GetImpactLatitudeAndLongitude(double Radius, Vector3d position, Vector3d velocity, out double latitude, out double longitude)
	{
		double num = Vector3d.Dot(position, velocity);
		double num2 = Vector3d.Dot(velocity, velocity);
		double num3 = Vector3d.Dot(position, position);
		double num4 = num / num2;
		double num5 = num4 * num4 - (num3 - Radius * Radius) / num2;
		if (num5 >= 0.0)
		{
			double num6 = 0.0 - num4 - Math.Sqrt(num5);
			Vector3d vector3d = position + velocity * num6;
			latitude = Math.Asin(vector3d.y / vector3d.magnitude) * (180.0 / Math.PI);
			longitude = Math.Atan2(vector3d.z, vector3d.x) * (180.0 / Math.PI);
			return true;
		}
		latitude = 0.0;
		longitude = 0.0;
		return false;
	}

	public static double GetAltitude(Vector3d bodyPosition, Vector3d worldPos, double Radius)
	{
		return (worldPos - bodyPosition).magnitude - Radius;
	}

	public static void GetLatLongAlt(Planetarium.CelestialFrame BodyFrame, Vector3d bodyPosition, double radius, Vector3d worldPos, out double lat, out double lon, out double alt)
	{
		Vector3d vector3d = BodyFrame.WorldToLocal((worldPos - bodyPosition).xzy);
		double magnitude = vector3d.magnitude;
		alt = magnitude - radius;
		vector3d /= magnitude;
		lat = Math.Asin(vector3d.z) * (180.0 / Math.PI);
		lon = Math.Atan2(vector3d.y, vector3d.x) * (180.0 / Math.PI);
		if (double.IsNaN(lat))
		{
			lat = 0.0;
		}
		if (double.IsNaN(lon))
		{
			lon = 0.0;
		}
	}
}
