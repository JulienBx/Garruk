using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class SkillButtonC : MonoBehaviour
{
	public int id ;
	bool launchable ;
	CardC card ;
	SkillC skillC ;
	Skill skill ;
	List<TileM> targets;
	Vector3 initialPosition ;
	string skillText;

	float timerMove;
	float moveTime = 0.25f;
	bool moving;

	Vector3 startPosition, endPosition;

	void Awake()
	{
		this.show(false);
		this.showDescription(false);
		this.launchable = false ;

		if(this.id==0){
			this.initialPosition = new Vector3(-2.5f, -4.4f, 0f);
		}
		else if(this.id==1){
			this.initialPosition = new Vector3(-1.5f, -4.4f, 0f);
		}
		else if(this.id==2){
			this.initialPosition = new Vector3(-0.5f, -4.4f, 0f);
		}
		else if(this.id==3){
			this.initialPosition = new Vector3(0.5f, -4.4f, 0f);
		}
	}

	public int getSkillID(){
		return this.skillC.id;
	}

	public SkillC getSkillC(){
		return this.skillC;
	}

	public Skill getSkill(){
		return this.skill;
	}

	public void show(bool b){
		gameObject.transform.GetComponent<SpriteRenderer>().enabled = b ;
		this.showCollider(b);
		this.showTitle(b);
	}

	public void showCollider(bool b){
		gameObject.transform.GetComponent<BoxCollider>().enabled = b ;
	}

	public void showTitle(bool b){
		gameObject.transform.FindChild("Text").GetComponent<MeshRenderer>().enabled = b ;
	}

	public void showDescription(bool b){
		gameObject.transform.FindChild("DescriptionZone").GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<MeshRenderer>().enabled = b ;
		gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled = b ;
	}

	public void size(Vector3 position){
		gameObject.transform.localPosition = position;
	}

	public void setCard(CardC c){
		this.card = c ;
		if(this.id==0){
			this.skillC = Game.instance.getSkills().skills[0];
		}
		else{
			this.skillC = Game.instance.getSkills().skills[this.card.getCardM().getSkill(this.id).Id];
			this.skill = c.getCardM().getSkill(this.id);
		}
		if(this.id==0){
			gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().text = WordingSkills.getName(0) ;
			gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().text = WordingSkills.getName(0) ;
			gameObject.GetComponent<SpriteRenderer>().sprite = Game.instance.getSkillSprite(0);
		}
		else{
			gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().text = WordingSkills.getName(this.skillC.id) ;
			gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().text = WordingSkills.getName(this.skillC.id) ;
			gameObject.GetComponent<SpriteRenderer>().sprite = Game.instance.getSkillSprite(this.skillC.id);
		}
	}

	public void forbid(){
		this.grey();
		this.launchable = false ;
		gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = WordingGame.getText(71) ;
	}

	public void OnMouseEnter()
	{
		if(Game.instance.getDraggingSBID()==-1){
			if(this.launchable){
				this.blue();
				Game.instance.getBoard().startTargets(this.targets);
			}
			else{
				this.red();
			}
			this.showDescription(true);
		}
	}

	public void OnMouseExit()
	{
		if(Game.instance.getDraggingSBID()==-1){
			Game.instance.getBoard().stopTargets(this.targets);
			this.showDescription(false);
			this.reinit();
		}
	}

	public void OnMouseDown()
	{
		if(this.launchable && Game.instance.getDraggingSBID()==-1){
			Game.instance.hitSkillButton(this.id);
		}
	}

	public void OnMouseUp()
	{
		if(Game.instance.getDraggingSBID()!=-1){
			TileM tile = Game.instance.getBoard().getMouseTile();

			if(tile.x>=0 && tile.x<Game.instance.getBoard().getBoardWidth() && tile.y>=0 && tile.y<Game.instance.getBoard().getBoardHeight()){
				Game.instance.dropSBOnTile(tile.x,tile.y);
			}
			else{
				Game.instance.dropSBOutsideBoard();
			}
		}
	}

	public void grey(){
		gameObject.GetComponent<SpriteRenderer>().color = new Color(80f/255f, 80f/255f, 80f/255f, 255f/255f) ;
		gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color(80f/255f, 80f/255f, 80f/255f, 255f/255f) ;
		gameObject.transform.FindChild("DescriptionZone").GetComponent<SpriteRenderer>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
		gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
	}

	public void red(){
		gameObject.GetComponent<SpriteRenderer>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
		gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
		gameObject.transform.FindChild("DescriptionZone").GetComponent<SpriteRenderer>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
		gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
	}

	public void blue(){
		gameObject.GetComponent<SpriteRenderer>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
		gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
		gameObject.transform.FindChild("DescriptionZone").GetComponent<SpriteRenderer>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
		gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
	}

	public void reinit(){
		if(this.launchable){
			this.white();
		}
		else{
			this.grey();
		}
	}

	public void white(){
		gameObject.GetComponent<SpriteRenderer>().color = new Color(255f/255f, 255f/255f, 255f/255f, 255f/255f) ;
		gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color(255f/255f, 255f/255f, 255f/255f, 255f/255f) ;
		gameObject.transform.FindChild("DescriptionZone").GetComponent<SpriteRenderer>().color = new Color(255f/255f, 255f/255f, 255f/255f, 255f/255f) ;
		gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().color = new Color(255f/255f, 255f/255f, 255f/255f, 255f/255f) ;
	}

	public void update(){
		if(this.id==0){
			this.targets = Game.instance.getSkills().skills[0].getTargetTiles(this.card);
		}
		else{
			this.targets = Game.instance.getSkills().skills[this.card.getCardM().getSkill(this.id).Id].getTargetTiles(this.card);
		}

		if(this.targets.Count==0){
			this.grey();
			this.launchable = false ;
			gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = Game.instance.getSkills().skills[this.skillC.id].getEmptyTargetText();
		}
		else{
			this.white();
			this.launchable = true ;
			if(this.id==0){
				this.skillText = this.card.getAttackText();
			}
			else{
				this.skillText = this.card.getSkillText(this.skillC.id);
			}
			this.setSkillText(this.skillText);
		}
	}

	public void getTargetText(int targetId){
		string s = this.skillC.getSkillText(targetId);
		this.setSkillText(s);
	}

	public void setSkillText(string s){
		gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = s;
	}

	public void reinitSkillText(){
		gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = this.skillText;
	}

	public void setPosition(Vector3 p){
		gameObject.transform.localPosition = new Vector3(p.x, p.y, -0.5f);
	}

	public void startMove(Vector3 s, Vector3 e){
		this.startPosition = new Vector3(s.x, s.y, -0.5f) ;
		this.endPosition = new Vector3(e.x, e.y, -0.5f) ;
		this.showDescription(false);
		this.timerMove = 0f;
		this.moving = true;
	}

	public Vector3 getPosition(){
		return gameObject.transform.localPosition;
	}

	public Vector3 getInitialPosition(){
		return this.initialPosition;
	}

	public void addMoveTime(float f){
		this.timerMove+=f;
		Vector3 p = new Vector3(0f, 0f, -0.5f);
		p.x = this.startPosition.x+(this.endPosition.x-this.startPosition.x)*(Mathf.Sqrt(Mathf.Min(1,this.timerMove/this.moveTime)));
		p.y = this.startPosition.y+(this.endPosition.y-this.startPosition.y)*(Mathf.Sqrt(Mathf.Min(1,this.timerMove/this.moveTime)));
		this.setPosition(p);
		if(this.timerMove>this.moveTime){
			this.moving=false;
			this.showTitle(true);
			this.showCollider(true);
		}
	}

	public bool isMoving(){
		return this.moving;
	}

	public List<TileM> getTargets(){
		return this.targets;
	}
}

