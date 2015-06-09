using UnityEngine ;

public class TileViewModel
{
	public Vector3 scale ;
	public Vector3 position ;
	public Texture2D background ;
	public Texture2D border;
	public bool isPotentialTarget = false;
	public bool toDisplayHalo = false ;
	public Rect haloRect ;
	public Texture2D halo ;
	public GUIStyle haloStyle ;
	public GUIStyle trapStyle ;
	public bool toDisplayTrap = false ;
	public Texture2D trap ;

	public TileViewModel()
	{
		this.background = null;
		this.border = null;
		this.scale = new Vector3(0, 0, 0);
		this.position = new Vector3(0, 0, 0);
		this.haloStyle = new GUIStyle();
		this.trapStyle = new GUIStyle();
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

