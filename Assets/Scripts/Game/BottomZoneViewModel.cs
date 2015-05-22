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
	public GUIStyle turnsTextStyle ;
	public GUIStyle characterNameTextStyle ;
	public GUIStyle characterButtonTextStyle ;
	public GUIStyle characterLabelTextStyle ;
	public int nbTurns ;

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
		this.turnsTextStyle = new GUIStyle();
		this.characterNameTextStyle = new GUIStyle();
		this.characterButtonTextStyle = new GUIStyle();
		this.characterLabelTextStyle = new GUIStyle();
		this.nbTurns = 0 ;
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
		this.turnsTextStyle = styles[5];
		this.characterNameTextStyle = styles[6];
		this.characterButtonTextStyle = styles[7];
		this.characterLabelTextStyle = styles[8];

		this.resize(h);
	}

	public void resize (int h)
	{
		this.nameTextStyle.fontSize = h * 25/1000;
		this.messageTextStyle.fontSize = h * 15/1000;
		this.buttonTextStyle.fontSize = h * 20/1000;
		this.turnsTextStyle.fontSize = h * 20/1000;
	}
}


