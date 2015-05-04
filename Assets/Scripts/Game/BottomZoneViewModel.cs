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
	public string namePlayingCharacter ;
	public string nameSkill1 ;
	public string nameSkill2 ;
	public string nameSkill3 ;
	public string nameSkill4 ;
	public bool isActivableSkill1 ;
	public bool isActivableSkill2 ;
	public bool isActivableSkill3 ;
	public bool isActivableSkill4 ;

	public bool toDisplayCharacter ;

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
		this.nbTurns = -1 ;
		this.namePlayingCharacter = "" ;
		this.nameSkill1 = "" ;
		this.nameSkill2 = "" ;
		this.nameSkill3 = "" ;
		this.nameSkill4 = "" ; 
		this.toDisplayCharacter = false ;
		this.isActivableSkill1 = false ;
		this.isActivableSkill2 = false ;
		this.isActivableSkill3 = false ;
		this.isActivableSkill4 = false ;

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
		this.messageTextStyle.fontSize = h * 20/1000;
		this.buttonTextStyle.fontSize = h * 25/1000;
		this.turnsTextStyle.fontSize = h * 30/1000;
	}

	public void emptyCharacter(){
		this.toDisplayCharacter = false ;
		this.namePlayingCharacter = "" ;
		this.nameSkill1 = "" ;
		this.nameSkill2 = "" ;
		this.nameSkill3 = "" ;
		this.nameSkill4 = "" ;
		this.isActivableSkill1 = false ;
		this.isActivableSkill2 = false ;
		this.isActivableSkill3 = false ;
		this.isActivableSkill4 = false ;
	}

	public void setCharacter(Card c){
		this.toDisplayCharacter = true ;
		this.namePlayingCharacter = c.Title ;
		int nbSkills = c.Skills.Count;
		if (nbSkills>0){
			this.nameSkill1 = c.Skills[0].Name ;
			this.isActivableSkill1 = (c.Skills[0].ManaCost <= nbTurns);
		}
		if (nbSkills>1){
			this.nameSkill1 = c.Skills[1].Name ;
			this.isActivableSkill1 = (c.Skills[1].ManaCost <= nbTurns);
		}
		if (nbSkills>2){
			this.nameSkill1 = c.Skills[2].Name ;
			this.isActivableSkill1 = (c.Skills[2].ManaCost <= nbTurns);
		}
		if (nbSkills>3){
			this.nameSkill1 = c.Skills[3].Name ;
			this.isActivableSkill1 = (c.Skills[3].ManaCost <= nbTurns);
		}
	}
}


