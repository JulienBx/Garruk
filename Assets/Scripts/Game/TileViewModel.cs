using UnityEngine ;
using System.Collections.Generic;

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
	public GUIStyle descritionIconStyle ;
	public GUIStyle titleStyle ;
	public GUIStyle descriptionStyle ;
	public GUIStyle additionnalInfoStyle ;
	public List<string> haloTexts ;
	public List<GUIStyle> haloStyles ;
	public GUIStyle trapStyle ;
	public bool toDisplayTrap = false ;
	public Texture2D trap ;
	
	public string title ;
	public string description ; 
	public string additionnalInfo ;

	public bool toDisplayIcon = false;

	public GUIStyle iconStyle;
	public Rect iconRect;

	public Texture2D icon;
	public bool isZoneEffect;
	public bool toDisplayDescriptionIcon = false ;

	public TileViewModel()
	{
		this.background = null;
		this.border = null;
		this.scale = new Vector3(0, 0, 0);
		this.position = new Vector3(0, 0, 0);
		this.haloStyle = new GUIStyle();
		this.trapStyle = new GUIStyle();
		this.titleStyle = new GUIStyle();
		this.descriptionStyle = new GUIStyle();
		this.additionnalInfoStyle = new GUIStyle();
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

