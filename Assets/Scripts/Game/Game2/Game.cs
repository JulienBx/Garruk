using System;
using UnityEngine;

public class Game : MonoBehaviour
{
	public static Game instance;
	public Board board ;
	public Gamecards gamecards ;

	GameObject myHoveredCard ;
	GameObject hisHoveredCard ;
	GameObject startButton ;
	GameObject interlude ;
	GameObject passZone ;

	float realWidth;

	void Awake(){
		this.board = new Board();
		this.gamecards = new Gamecards();
		SoundController.instance.playMusic(new int[]{4,5,6});

        this.myHoveredCard = GameObject.Find("MyHoveredPlayingCard");
		this.hisHoveredCard = GameObject.Find("HisHoveredPlayingCard");
		this.startButton = GameObject.Find("SB");
		this.interlude = GameObject.Find("Interlude");
		this.passZone = GameObject.Find("PassZone");

		/*
		this.skillZone = GameObject.Find("SkillZone");
		this.choicePopUp = GameObject.Find("PopUpChoice");
		this.timeline = GameObject.Find("Timeline").GetComponent<TimelineController>();
		this.hisTimer = GameObject.Find("HisTime").GetComponent<TimerController>();
		this.myTimer = GameObject.Find("MyTime").GetComponent<TimerController>();
		this.timerFront = GameObject.Find("TimerFront");
		this.validationSkill = GameObject.Find("ValidationAutoSkill");
		this.validationSkill.GetComponent<SkillValidationController>().show(false);
		this.gameTutoController = GameObject.Find("HelpController").GetComponent<GameTutoController>();
		this.SB.GetComponent<StartButtonController>().show(false);
        this.background=GameObject.Find("GameBackground");

		this.runningSkill=-1;
		this.isGameskillOK = true ;
		this.createBackground();
		this.targets = new List<Tile>();
		this.skillEffects = new List<Tile>();
		this.anims = new List<Tile>();
		this.deads = new List<int>();
		this.orderCards = new List<int>();
		this.hasLaunchedQGH = false ;
		*/
	}

	public LeftSlider getMyHoveredCard(){
		return this.myHoveredCard.GetComponent<LeftSlider>();
	}

	public RightSlider getHisHoveredCard(){
		return this.hisHoveredCard.GetComponent<RightSlider>();
	}

	public void resize(){
		int width = Screen.width ;
		int height = Screen.height ;

		float realwidth = 10f*width/height;

		this.getMyHoveredCard().putToStartPosition(realwidth);
		this.getHisHoveredCard().putToStartPosition(realwidth);
	}
}

