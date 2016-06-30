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
	GameObject passButton ;
	GameObject[] skillsButton ;
	GameObject popUpChoice ;
	GameObject timeline ;
	GameObject myTimer ;
	GameObject hisTimer ;
	GameObject frontTimer ;
	GameObject popUpValidation ;
	GameObject forfeitButton ;
	GameObject myPlayerName ;
	GameObject hisPlayerName ;
	GameObject cancelButton ;

	float realWidth;
	float tileScale;
	bool isMobile ;

	void Awake(){
		this.board = new Board();
		this.gamecards = new Gamecards();
		SoundController.instance.playMusic(new int[]{4,5,6});

        this.myHoveredCard = GameObject.Find("MyHoveredPlayingCard");
		this.hisHoveredCard = GameObject.Find("HisHoveredPlayingCard");
		this.startButton = GameObject.Find("SB");
		this.interlude = GameObject.Find("Interlude");
		this.passButton = GameObject.Find("PassZone");
		this.skillsButton = new GameObject[4];
		this.skillsButton[0] = GameObject.Find("SkillButton1");
		this.skillsButton[1] = GameObject.Find("SkillButton2");
		this.skillsButton[2] = GameObject.Find("SkillButton3");
		this.skillsButton[3] = GameObject.Find("SkillButton4");
		this.popUpChoice = GameObject.Find("PopUpChoice");
		this.timeline = GameObject.Find("Timeline");
		this.myTimer = GameObject.Find("MyTimer");
		this.hisTimer = GameObject.Find("HisTimer");
		this.frontTimer = GameObject.Find("FrontTimer");
		this.popUpValidation = GameObject.Find("PopUpValidation");
		this.forfeitButton = GameObject.Find("ForfeitButton");
		this.myPlayerName = GameObject.Find("MyPlayerName");
		this.hisPlayerName = GameObject.Find("HisPlayerName");
		this.cancelButton = GameObject.Find("CancelButton");

		GameObject.Find("Logo").transform.position = new Vector3(0f, 2.5f+2*tileScale, 0f);
		GameObject.Find("Logo").GetComponent<SpriteRenderer>().enabled = false;

		if(this.isFirstPlayer()){
			GameRPC.instance.createBackground();
		}
	}

	public void createBackground(GameObject verticalBorderModel, GameObject horizontalBorderModel)
	{
		for (int i = 0; i < this.board.getBoardWidth()+1; i++)
		{
			this.board.addVerticalBorder(i, (GameObject)Instantiate(verticalBorderModel));
		}
		for (int i = 0; i < this.board.getBoardHeight()+1; i++)
		{
			this.board.addHorizontalBorder(i, (GameObject)Instantiate(horizontalBorderModel));
		}
		
		this.resize();
	}

	public CancelButtonC getCancelButton(){
		return this.cancelButton.GetComponent<CancelButtonC>();
	}

	public StartButtonC getStartButton(){
		return this.startButton.GetComponent<StartButtonC>();
	}

	public MyTimerC getMyTimer(){
		return this.myTimer.GetComponent<MyTimerC>();
	}

	public HisTimerC getHisTimer(){
		return this.hisTimer.GetComponent<HisTimerC>();
	}

	public ForfeitC getForfeitButton(){
		return this.forfeitButton.GetComponent<ForfeitC>();
	}

	public LeftSlider getMyHoveredCard(){
		return this.myHoveredCard.GetComponent<LeftSlider>();
	}

	public RightSlider getHisHoveredCard(){
		return this.hisHoveredCard.GetComponent<RightSlider>();
	}

	public PassButtonC getPassButton(){
		return this.passButton.GetComponent<PassButtonC>();
	}

	public SkillButtonC getSkillButton(int i){
		return this.skillsButton[i].GetComponent<SkillButtonC>();
	}

	public TimelineC getTimeline(){
		return this.timeline.GetComponent<TimelineC>();
	}

	public bool isFirstPlayer(){
		return ApplicationModel.player.IsFirstPlayer;
	}

	public void resize(){
		int width = Screen.width ;
		int height = Screen.height ;

		float realwidth = 10f*width/height;

		this.getMyHoveredCard().putToStartPosition(realwidth);
		this.getHisHoveredCard().putToStartPosition(realwidth);
		this.isMobile = (width<height);

		this.getMyHoveredCard().activateCollider(this.isMobile);
		this.getHisHoveredCard().activateCollider(this.isMobile);
		this.tileScale = Mathf.Min (realwidth/6.05f, 8f/this.board.getBoardHeight());

		for (int i = 0; i < this.board.getBoardHeight()+1; i++){
			this.board.sizeHorizontalBorder(i, new Vector3(0,(-4*tileScale)+tileScale*i,-1f), new Vector3(1f,0.5f,1f));
		}
		
		for (int i = 0; i < this.board.getBoardWidth()+1; i++){
			this.board.sizeVerticalBorder(i, new Vector3((-this.board.getBoardWidth()/2f+i)*this.tileScale, 0f, -1f), new Vector3(0.5f,tileScale,1f));
		}
		
		if(this.isMobile){
			this.getForfeitButton().size(new Vector3(-realwidth/2f+0.25f, 4.75f, 0f), new Vector3(0.4f, 0.4f, 0.4f));
			this.getMyTimer().size(new Vector3(-realwidth/2f+0.02f, 4*tileScale+0.25f, 0f));
			this.getHisTimer().size(new Vector3(realwidth/2f-0.52f, 4*tileScale+0.25f, 0f));
			GameObject.Find("MyPlayerName").GetComponent<MeshRenderer>().enabled = false;
			GameObject.Find("HisPlayerName").GetComponent<MeshRenderer>().enabled = false;
		}
		else{
			this.getForfeitButton().size(new Vector3(-realwidth/2f+0.5f, 4.5f, 0f), new Vector3(0.8f, 0.8f, 0.8f));
			this.getMyTimer().size(new Vector3(-2.98f, 4*tileScale+0.25f, 0f));
			this.getHisTimer().size(new Vector3(2.48f, 4*tileScale+0.25f, 0f));
			GameObject.Find("MyPlayerName").transform.localPosition = new Vector3(-realwidth/2f+1f, 4.5f, 0f);
			GameObject.Find("HisPlayerName").transform.localPosition = new Vector3(realwidth/2f, 4.5f, 0f);
		}

		GameObject.Find("Logo").transform.position = new Vector3(0f, 2.5f+2*tileScale, 0f);
		this.getStartButton().size(new Vector3(0f, 2.5f+2*tileScale, 0f));

		this.getPassButton().size(new Vector3(Mathf.Min(2.2f, 0.5f*realwidth-0.8f), -2.4f-2*tileScale, 0f));
		this.getSkillButton(0).size(new Vector3(-2.5f*this.tileScale, -2.4f-2*tileScale, 0f));
		this.getSkillButton(1).size(new Vector3(-1.5f*this.tileScale, -2.4f-2*tileScale, 0f));
		this.getSkillButton(2).size(new Vector3(-0.5f*this.tileScale, -2.4f-2*tileScale, 0f));
		this.getSkillButton(3).size(new Vector3(0.5f*this.tileScale, -2.4f-2*tileScale, 0f));

		this.getCancelButton().size
		if(this.isMobile){
			tempTransform = this.skillZone.transform.FindChild("CancelZone");
			position = tempTransform.position ;
			position.x = 2.5f * (realwidth/6f);
			tempTransform.position = position;

			tempTransform = this.skillZone.transform.FindChild("CancelZone").FindChild("Text");
			tempTransform.GetComponent<TextContainer>().width = 3.5f*(realwidth/6f) ;
		}

		this.interlude.GetComponent<InterludeController>().resize(realwidth);

		if(this.areTilesLoaded){
			for(int i = 0 ; i < boardWidth ; i++){
				for(int j = 0 ; j < boardHeight ; j++){
					this.getTileController(i,j).resize();
				}
			}
			for(int i = 0 ; i < this.nbCards ; i++){
				this.getPlayingCardController(i).resize();
			}
		}

	}
}