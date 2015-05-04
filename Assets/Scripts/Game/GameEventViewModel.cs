using UnityEngine;

public class GameEventViewModel
{
	public Vector3 position ;
	public Vector3 ScreenPosition ;
	public Quaternion rotation ;
	public Vector3 scale ;
	public Rect infoRect ;
	
	public GUIStyle backgroundStyle;
	public GUIStyle nameTextStyle;

	public string characterName;
	public string action;

	public Texture art;

	public GameEventViewModel()
	{
		this.position = new Vector3(0, 0, 0);
		this.ScreenPosition = new Vector3(0, 0, 0);
		this.rotation = Quaternion.Euler(0, 0, 0);
		this.scale = new Vector3(0, 0, 0);
		this.infoRect = new Rect(0, 0, 0, 0);
		
		this.characterName = "";
		this.action = "";
		
		this.backgroundStyle = new GUIStyle();
		this.nameTextStyle = new GUIStyle();
	}
}
