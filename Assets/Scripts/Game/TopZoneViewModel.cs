using UnityEngine;

public class TopZoneViewModel
{
	public string userName ;
	public Texture2D userPicture ;
	public string message ;
	public bool toDisplayGreenStatus ;
	public bool toDisplayRedStatus ;
	public string status ;
	public GUIStyle backgroundStyle ;
	public GUIStyle nameTextStyle ;
	public GUIStyle imageStyle ;
	public GUIStyle messageTextStyle ;
	public GUIStyle redStatusTextStyle ;
	public GUIStyle greenStatusTextStyle ;

	public TopZoneViewModel ()
	{
		this.userName = "";
		this.userPicture = null;
		this.message = "En attente des informations du joueur 2" ;
		this.toDisplayGreenStatus = false ;
		this.toDisplayRedStatus = false ;
		this.status = "";
		this.backgroundStyle = new GUIStyle();
		this.nameTextStyle = new GUIStyle();
		this.imageStyle = new GUIStyle();
		this.messageTextStyle = new GUIStyle();
		this.redStatusTextStyle = new GUIStyle();
		this.greenStatusTextStyle = new GUIStyle();
	}

	public void setValues (User u, GUIStyle[] styles, int h)
	{
		this.userName = u.Username ;
		this.userPicture = u.texture ;
		this.message = "Positionne ses héros sur le champ de bataille..." ;
		this.toDisplayGreenStatus = false ;
		this.toDisplayRedStatus = true ;
		this.status = "En cours...";
		this.backgroundStyle = styles[0];
		this.nameTextStyle = styles[1];
		this.imageStyle = styles[2];
		this.messageTextStyle = styles[3];
		this.redStatusTextStyle = styles[4];
		this.greenStatusTextStyle = styles[5];

		this.resize(h);
	}

	public void resize (int h)
	{
		this.nameTextStyle.fontSize = h * 25/1000;
		this.messageTextStyle.fontSize = h * 20/1000;
		this.redStatusTextStyle.fontSize = h * 25/1000;
		this.greenStatusTextStyle.fontSize = h * 25/1000;
	}
}


