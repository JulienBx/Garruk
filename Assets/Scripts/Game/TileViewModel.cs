using UnityEngine ;

public class TileViewModel
{
	public Vector3 scale ;
	public Vector3 position ;
	public Texture2D background ;
	public Texture2D border;
	public bool isPotentialTarget = false;

	public TileViewModel()
	{
		this.background = null;
		this.border = null;
		this.scale = new Vector3(0, 0, 0);
		this.position = new Vector3(0, 0, 0);
	}

	public void raiseTile()
	{
		this.position.z -= 0.02f;
	}

	public void lowerTile()
	{
		this.position.z += 0.02f;
	}
}

