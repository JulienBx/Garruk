using UnityEngine ;

public class TileViewModel
{
	public Vector3 scale ;
	public Vector3 position ;
	public Texture2D background ;

	public TileViewModel ()
	{
		this.background = null;
		this.scale = new Vector3(0,0,0);
		this.position = new Vector3(0,0,0);
	}
}

