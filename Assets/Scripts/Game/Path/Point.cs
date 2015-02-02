using System;

public class Point
{
	public int X, Y;
	
	public Point(int x, int y)
	{
		X = x;
		Y = y;
	}

	public override int GetHashCode() 
	{
		return string.Format("{0}-{1}", X, Y).GetHashCode();
	}

	public override bool Equals(object obj) 
	{
		return Equals(obj as Point);
	}

	public bool Equals(Point obj) 
	{
		return obj != null && obj.X == this.X && obj.Y == this.Y;
	}
}