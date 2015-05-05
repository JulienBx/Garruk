using UnityEngine;

public class GameEventViewModel
{
	public Vector3 scale;
	public Rect infoRect;
	public int width;
	public int height;
	
	public GUIStyle backgroundStyle;
	public GUIStyle nameTextStyle;

	public string characterName;
	public string action;

	public Texture art;

	public GameEventViewModel()
	{
		float scaleCoef = Screen.height * 1.4f / 1000f;
		this.scale = new Vector3(scaleCoef, scaleCoef, scaleCoef);
		this.infoRect = new Rect(0, 0, 0, 0);
		
		this.characterName = "";
		this.action = "";
		
		this.backgroundStyle = new GUIStyle();
		this.nameTextStyle = new GUIStyle();
	}
}
