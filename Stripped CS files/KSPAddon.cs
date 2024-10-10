using System;

[AttributeUsage(AttributeTargets.Class)]
public class KSPAddon : Attribute
{
	public enum Startup
	{
		FlightEditorAndKSC = -6,
		AllGameScenes = -5,
		FlightAndEditor = -4,
		FlightAndKSC = -3,
		Instantly = -2,
		EveryScene = -1,
		EditorAny = 6,
		MainMenu = 2,
		Settings = 3,
		SpaceCentre = 5,
		Credits = 4,
		EditorVAB = 6,
		EditorSPH = 6,
		Flight = 7,
		TrackingStation = 8,
		PSystemSpawn = 9
	}

	public Startup startup;

	public bool once;

	public KSPAddon(Startup startup, bool once)
	{
		this.startup = startup;
		this.once = once;
	}
}
