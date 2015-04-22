using UnityEngine;

public class BottomZoneViewModel
{
	public string userName ;
	public Texture2D userPicture ;
	public string message ;
	public bool displayStartButton ;
	public GUIStyle backgroundStyle ;
	public GUIStyle nameTextStyle ;
	public GUIStyle imageStyle ;
	public GUIStyle messageTextStyle ;
	public GUIStyle buttonTextStyle ;

	public BottomZoneViewModel ()
	{
		this.userName = "";
		this.userPicture = null;
		this.displayStartButton = false ;
		this.message = "En attente des informations du joueur 1" ;
		this.backgroundStyle = new GUIStyle();
		this.nameTextStyle = new GUIStyle();
		this.imageStyle = new GUIStyle();
		this.messageTextStyle = new GUIStyle();
		this.buttonTextStyle = new GUIStyle();
	}

	public void setValues (User u, GUIStyle[] styles, int h)
	{
		this.userName = u.Username ;
		this.userPicture = u.texture ;
		this.displayStartButton = true ;
		this.message = "Positionnez vos héros sur le champ de bataille" ;
		this.backgroundStyle = styles[0];
		this.nameTextStyle = styles[1];
		this.imageStyle = styles[2];
		this.messageTextStyle = styles[3];
		this.buttonTextStyle = styles[4];

		this.resize(h);
	}

	public void resize (int h)
	{
		this.nameTextStyle.fontSize = h * 25/1000;
		this.messageTextStyle.fontSize = h * 20/1000;
		this.buttonTextStyle.fontSize = h * 25/1000;
	}
}


