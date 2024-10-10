namespace VehiclePhysics;

public class DataBus
{
	public int[][] m_data;

	public int[][] bus => m_data;

	public DataBus()
	{
		m_data = new int[3][];
		m_data[0] = new int[10];
		m_data[1] = new int[22];
		m_data[2] = new int[10];
	}

	public int Get(int idChannel, int idValue)
	{
		return m_data[idChannel][idValue];
	}

	public void Set(int idChannel, int idValue, int value)
	{
		m_data[idChannel][idValue] = value;
	}

	public int[] Get(int idChannel)
	{
		return m_data[idChannel];
	}
}
