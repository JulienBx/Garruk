using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class LoadingScreenController : MonoBehaviour
{
	
	private float angle;
	private float speed;
	private Quaternion target;

	private bool isPreMatchScreen;

	private float preMatchScreenSpeed;
	private float preMatchScreenRatio;

	private GameObject[] myCards;
	private GameObject[] hisCards;

	private GameObject myName;
	private GameObject hisName;
	private GameObject vs;

	private Vector3[] myCardsStartPosition;
	private float[] myCardsCurrentPosition;
	private bool[] toAnimateMyCards;
	private Vector3[] myCardsEndPosition;

	private Vector3[] hisCardsStartPosition;
	private float[] hisCardsCurrentPosition;
	private bool[] toAnimateHisCards;
	private Vector3[] hisCardsEndPosition;

	private Vector3 myNameStartPosition;
	private float myNameCurrentPosition;
	private bool toAnimateMyName;
	private Vector3 myNameEndPosition;

	private Vector3 hisNameStartPosition;
	private float hisNameCurrentPosition;
	private bool toAnimateHisName;
	private Vector3 hisNameEndPosition;

	private bool toAnimatePreMatchLoadingScreen;
	private bool toRewindPreMatchLoadingScreen;
	private bool toShowLoading;
	private bool toLoadScene;

	void Awake()
	{
		this.speed = 300.0f;
		this.preMatchScreenSpeed=3f;
		this.preMatchScreenRatio = 0.3f;

		this.angle = 0f;
		this.toShowLoading=true;
	}
	void Update()
	{
		if(this.toAnimatePreMatchLoadingScreen)
		{
			bool isOver=true;

			if(this.toAnimateMyName)
			{
				this.myNameCurrentPosition=this.myNameCurrentPosition+this.preMatchScreenSpeed*Time.deltaTime;
				if(this.myNameCurrentPosition>this.preMatchScreenRatio && !this.toAnimateMyCards.Contains(true))
				{
					this.toAnimateMyCards[0]=true;
				}
				if(this.myNameCurrentPosition>1f)
				{
					this.myNameCurrentPosition=1f;
					this.toAnimateMyName=false;
				}
				else
				{
					isOver=false;
				}
				Vector3 myNamePosition=this.myName.transform.localPosition;
				myNamePosition.x=this.myNameStartPosition.x + (this.myNameEndPosition.x-this.myNameStartPosition.x)*this.myNameCurrentPosition;
				this.myName.transform.localPosition=myNamePosition;
			}
			if(this.toAnimateHisName)
			{
				this.hisNameCurrentPosition=this.hisNameCurrentPosition+this.preMatchScreenSpeed*Time.deltaTime;
				if(this.hisNameCurrentPosition>this.preMatchScreenRatio && !this.toAnimateHisCards.Contains(true))
				{
					this.toAnimateHisCards[0]=true;
				}
				if(this.hisNameCurrentPosition>1f)
				{
					this.hisNameCurrentPosition=1f;
					this.toAnimateHisName=false;
				}
				else
				{
					isOver=false;
				}
				Vector3 hisNamePosition=this.hisName.transform.localPosition;
				hisNamePosition.x=this.hisNameStartPosition.x + (this.hisNameEndPosition.x-this.hisNameStartPosition.x)*this.hisNameCurrentPosition;
				this.hisName.transform.localPosition=hisNamePosition;
			}
			for(int i=0;i<ApplicationModel.nbCardsByDeck;i++)
			{
				if(this.toAnimateMyCards[i])
				{
					this.myCardsCurrentPosition[i]=this.myCardsCurrentPosition[i]+this.preMatchScreenSpeed*Time.deltaTime;
					if(this.myCardsCurrentPosition[i]>this.preMatchScreenRatio && i<ApplicationModel.nbCardsByDeck-1 && !this.toAnimateMyCards[i+1])
					{
						this.toAnimateMyCards[i+1]=true;
					}
					if(this.myCardsCurrentPosition[i]>1f)
					{
						this.myCardsCurrentPosition[i]=1f;
						this.toAnimateMyCards[i]=false;
					}
					else
					{
						isOver=false;
					}
					Vector3 myCardPosition=this.myCards[i].transform.localPosition;
					myCardPosition.x=this.myCardsStartPosition[i].x + (this.myCardsEndPosition[i].x-this.myCardsStartPosition[i].x)*this.myCardsCurrentPosition[i];
					this.myCards[i].transform.localPosition=myCardPosition;
				}
				if(this.toAnimateHisCards[i])
				{
					this.hisCardsCurrentPosition[i]=this.hisCardsCurrentPosition[i]+this.preMatchScreenSpeed*Time.deltaTime;
					if(this.hisCardsCurrentPosition[i]>this.preMatchScreenRatio && i<ApplicationModel.nbCardsByDeck-1 && !this.toAnimateHisCards[i+1])
					{
						this.toAnimateHisCards[i+1]=true;
					}
					if(this.hisCardsCurrentPosition[i]>1f)
					{
						this.hisCardsCurrentPosition[i]=1f;
						this.toAnimateHisCards[i]=false;
					}
					else
					{
						isOver=false;
					}
					Vector3 hisCardPosition=this.hisCards[i].transform.localPosition;
					hisCardPosition.x=this.hisCardsStartPosition[i].x + (this.hisCardsEndPosition[i].x-this.hisCardsStartPosition[i].x)*this.hisCardsCurrentPosition[i];
					this.hisCards[i].transform.localPosition=hisCardPosition;
				}
			}
			if(isOver)
			{
				this.toAnimatePreMatchLoadingScreen=false;
				this.vs.transform.localPosition=new Vector3(0f,0f,0f);
				this.vs.SetActive(true);
				StartCoroutine(this.toRewind());
			}
		}
		if(this.toRewindPreMatchLoadingScreen)
		{
			bool isOver=true;

			for(int i=0;i<ApplicationModel.nbCardsByDeck;i++)
			{
				if(this.toAnimateMyCards[ApplicationModel.nbCardsByDeck-1-i])
				{
					this.myCardsCurrentPosition[ApplicationModel.nbCardsByDeck-1-i]=this.myCardsCurrentPosition[ApplicationModel.nbCardsByDeck-1-i]-this.preMatchScreenSpeed*Time.deltaTime;
					if(this.myCardsCurrentPosition[ApplicationModel.nbCardsByDeck-1-i]<(1-this.preMatchScreenRatio) && ApplicationModel.nbCardsByDeck-1-i>0 && !this.toAnimateMyCards[ApplicationModel.nbCardsByDeck-2-i])
					{
						this.toAnimateMyCards[ApplicationModel.nbCardsByDeck-2-i]=true;
					}
					else if(this.myCardsCurrentPosition[ApplicationModel.nbCardsByDeck-1-i]<(1-this.preMatchScreenRatio) && ApplicationModel.nbCardsByDeck-1-i==0 && !this.toAnimateMyName)
					{
						this.toAnimateMyName=true;
					}
					if(this.myCardsCurrentPosition[ApplicationModel.nbCardsByDeck-1-i]<0f)
					{
						this.myCardsCurrentPosition[ApplicationModel.nbCardsByDeck-1-i]=0f;
						this.toAnimateMyCards[ApplicationModel.nbCardsByDeck-1-i]=false;
					}
					else
					{
						isOver=false;
					}
					Vector3 myCardPosition=this.myCards[ApplicationModel.nbCardsByDeck-1-i].transform.localPosition;
					myCardPosition.x=this.myCardsStartPosition[ApplicationModel.nbCardsByDeck-1-i].x + (this.myCardsEndPosition[ApplicationModel.nbCardsByDeck-1-i].x-this.myCardsStartPosition[ApplicationModel.nbCardsByDeck-1-i].x)*this.myCardsCurrentPosition[ApplicationModel.nbCardsByDeck-1-i];
					this.myCards[ApplicationModel.nbCardsByDeck-1-i].transform.localPosition=myCardPosition;
				}
				if(this.toAnimateHisCards[ApplicationModel.nbCardsByDeck-1-i])
				{
					this.hisCardsCurrentPosition[ApplicationModel.nbCardsByDeck-1-i]=this.hisCardsCurrentPosition[ApplicationModel.nbCardsByDeck-1-i]-this.preMatchScreenSpeed*Time.deltaTime;
					if(this.hisCardsCurrentPosition[ApplicationModel.nbCardsByDeck-1-i]<(1-this.preMatchScreenRatio) && ApplicationModel.nbCardsByDeck-1-i>0 && !this.toAnimateHisCards[ApplicationModel.nbCardsByDeck-2-i])
					{
						this.toAnimateHisCards[ApplicationModel.nbCardsByDeck-2-i]=true;
					}
					else if(this.hisCardsCurrentPosition[ApplicationModel.nbCardsByDeck-1-i]<(1-this.preMatchScreenRatio) && ApplicationModel.nbCardsByDeck-1-i==0 && !this.toAnimateHisName)
					{
						this.toAnimateHisName=true;
					}
					if(this.hisCardsCurrentPosition[ApplicationModel.nbCardsByDeck-1-i]<0f)
					{
						this.hisCardsCurrentPosition[ApplicationModel.nbCardsByDeck-1-i]=0f;
						this.toAnimateHisCards[ApplicationModel.nbCardsByDeck-1-i]=false;
					}
					else
					{
						isOver=false;
					}
					Vector3 myCardPosition=this.hisCards[ApplicationModel.nbCardsByDeck-1-i].transform.localPosition;
					myCardPosition.x=this.hisCardsStartPosition[ApplicationModel.nbCardsByDeck-1-i].x + (this.hisCardsEndPosition[ApplicationModel.nbCardsByDeck-1-i].x-this.hisCardsStartPosition[ApplicationModel.nbCardsByDeck-1-i].x)*this.hisCardsCurrentPosition[ApplicationModel.nbCardsByDeck-1-i];
					this.hisCards[ApplicationModel.nbCardsByDeck-1-i].transform.localPosition=myCardPosition;
				}
			}
			if(this.toAnimateMyName)
			{
				this.myNameCurrentPosition=this.myNameCurrentPosition-this.preMatchScreenSpeed*Time.deltaTime;
				if(this.myNameCurrentPosition<0f)
				{
					this.myNameCurrentPosition=0f;
					this.toAnimateMyName=false;
				}
				else
				{
					isOver=false;
				}
				Vector3 myNamePosition=this.myName.transform.localPosition;
				myNamePosition.x=this.myNameStartPosition.x + (this.myNameEndPosition.x-this.myNameStartPosition.x)*this.myNameCurrentPosition;
				this.myName.transform.localPosition=myNamePosition;
			}
			if(this.toAnimateHisName)
			{
				this.hisNameCurrentPosition=this.hisNameCurrentPosition-this.preMatchScreenSpeed*Time.deltaTime;
				if(this.hisNameCurrentPosition<0f)
				{
					this.hisNameCurrentPosition=0f;
					this.toAnimateHisName=false;
				}
				else
				{
					isOver=false;
				}
				Vector3 hisNamePosition=this.hisName.transform.localPosition;
				hisNamePosition.x=this.hisNameStartPosition.x + (this.hisNameEndPosition.x-this.hisNameStartPosition.x)*this.hisNameCurrentPosition;
				this.hisName.transform.localPosition=hisNamePosition;
			}
			if(isOver)
			{
				this.toRewindPreMatchLoadingScreen = false ;
				this.toLoadScene=true;
			}
		}
		if(this.toShowLoading)
		{
			this.angle = this.angle + this.speed * Time.deltaTime;
			this.target = Quaternion.Euler (0f,this.angle, 0f);
			this.gameObject.transform.FindChild("loadingCircle").transform.rotation = target;
		}
		if(this.toLoadScene)
		{
			if(BackOfficeController.instance.photon.async.progress>=0.9)
			{
				this.toLoadScene=false;
				BackOfficeController.instance.photon.async.allowSceneActivation = true ;
			}
		}
	}

	public void changeLoadingScreenLabel(string label)
	{
		this.gameObject.transform.FindChild ("title").GetComponent<TextMeshPro> ().text = label;
	}
	public void displayButton(bool value)
	{
		this.gameObject.transform.FindChild ("button").gameObject.SetActive (value);
	}
	public void launchPreMatchLoadingScreen()
	{
		this.initializePreMatchLoadingScreen();
		this.resize();
		this.toAnimatePreMatchLoadingScreen=true;
		this.toShowLoading=false;
	}
	private void initializePreMatchLoadingScreen()
	{
		this.isPreMatchScreen=true;
		this.myCards=new GameObject[ApplicationModel.nbCardsByDeck];
		this.hisCards=new GameObject[ApplicationModel.nbCardsByDeck];
		this.myCardsCurrentPosition=new float[ApplicationModel.nbCardsByDeck];
		this.hisCardsCurrentPosition=new float[ApplicationModel.nbCardsByDeck];
		this.myCardsStartPosition=new Vector3[ApplicationModel.nbCardsByDeck];
		this.hisCardsStartPosition=new Vector3[ApplicationModel.nbCardsByDeck];
		this.myCardsEndPosition=new Vector3[ApplicationModel.nbCardsByDeck];
		this.hisCardsEndPosition=new Vector3[ApplicationModel.nbCardsByDeck];
		this.toAnimateMyCards=new bool[ApplicationModel.nbCardsByDeck];
		this.toAnimateHisCards=new bool[ApplicationModel.nbCardsByDeck];
		for(int i=0;i<ApplicationModel.nbCardsByDeck;i++)
		{
			this.myCards[i]=this.gameObject.transform.FindChild("myCard"+i).gameObject;
			this.myCards[i].transform.FindChild("Avatar").GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnPreMatchScreenAvatar(ApplicationModel.player.MyDeck.cards[i].Skills[0].Id);
			this.myCards[i].SetActive(true);
			this.hisCards[i]=this.gameObject.transform.FindChild("hisCard"+i).gameObject;
			this.hisCards[i].transform.FindChild("Avatar").GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnPreMatchScreenAvatar(ApplicationModel.opponentDeck.cards[i].Skills[0].Id);
			this.hisCards[i].SetActive(true);
			this.myCardsCurrentPosition[i]=0f;
			this.hisCardsCurrentPosition[i]=0f;
		}
		this.toAnimateMyName=true;
		this.toAnimateHisName=true;
		this.myNameCurrentPosition=0f;
		this.hisNameCurrentPosition=0f;
		this.myName= this.gameObject.transform.FindChild("myName").gameObject;
		this.myName.SetActive(true);
		this.hisName=this.gameObject.transform.FindChild("hisName").gameObject;
		this.hisName.SetActive(true);
		this.vs=this.gameObject.transform.FindChild("VS").gameObject;
		this.vs.transform.GetComponent<TextMeshPro>().text=WordingLoadingScreen.getReference(1);
		this.vs.SetActive(false);
		this.gameObject.transform.FindChild("loadingCircle").gameObject.SetActive(false);
		this.gameObject.transform.FindChild("button").gameObject.SetActive(false);
		this.gameObject.transform.FindChild("title").gameObject.SetActive(false);
		this.gameObject.transform.FindChild("background2").GetComponent<SpriteRenderer>().sprite=this.gameObject.transform.parent.GetComponent<BackOfficeRessources>().loadingScreenBackgrounds[1];
		this.myName.GetComponent<TextMeshPro>().text=ApplicationModel.myPlayerName.ToUpper();
		this.hisName.GetComponent<TextMeshPro>().text=ApplicationModel.hisPlayerName.ToUpper();
		SoundController.instance.playSound(17);
	}
	public void resize()
	{
		this.gameObject.transform.position = new Vector3 (ApplicationDesignRules.menuPosition.x, ApplicationDesignRules.menuPosition.y, -9f);

		if(this.isPreMatchScreen)
		{
			Vector3 myCardAvatarLocalPosition = new Vector3();
			Vector3 hisCardAvatarLocalPosition = new Vector3();
			for(int i=0;i<ApplicationModel.nbCardsByDeck;i++)
			{
				this.myCards[i].transform.localScale=ApplicationDesignRules.flipObjectScale;
				myCardAvatarLocalPosition = this.myCards[i].transform.FindChild("Avatar").transform.localPosition;
				myCardAvatarLocalPosition.x = 1f/(ApplicationDesignRules.flipObjectScale.x)*((ApplicationDesignRules.flipObjectWorldSize.x/2f)-this.myCards[i].transform.FindChild("Avatar").GetComponent<SpriteRenderer>().bounds.size.x/2f-1.1f);
				this.myCards[i].transform.FindChild("Avatar").transform.localPosition = myCardAvatarLocalPosition;
				this.hisCards[i].transform.localScale=ApplicationDesignRules.flipObjectScale;
				hisCardAvatarLocalPosition = this.hisCards[i].transform.FindChild("Avatar").transform.localPosition;
				hisCardAvatarLocalPosition.x = 1f/(ApplicationDesignRules.flipObjectScale.x)*((-ApplicationDesignRules.flipObjectWorldSize.x/2f)+this.hisCards[i].transform.FindChild("Avatar").GetComponent<SpriteRenderer>().bounds.size.x/2f+1.1f);
				this.hisCards[i].transform.FindChild("Avatar").transform.localPosition = hisCardAvatarLocalPosition;
				this.myName.transform.localScale=ApplicationDesignRules.flipNameScale;
				this.hisName.transform.localScale=ApplicationDesignRules.flipNameScale;
				this.vs.transform.localScale=ApplicationDesignRules.flipNameScale;

				float verticalGap;
				float horizontalGap;
				float firstLineGap;

				if(ApplicationDesignRules.isMobileScreen)
				{
					verticalGap=0.2f;
					horizontalGap = 0.7f;
					firstLineGap=0.8f;
				}
				else
				{
					verticalGap=0.3f;
					horizontalGap = 1.1f;
					firstLineGap=1f;
				}
				this.myNameStartPosition=new Vector3(-2f*ApplicationDesignRules.worldWidth/2f,ApplicationDesignRules.worldHeight/2f-0.4f,0f);
				this.myName.transform.localPosition=this.myNameStartPosition;
				this.myNameEndPosition=new Vector3(0.2f-ApplicationDesignRules.worldWidth/2f,this.myNameStartPosition.y,0f);

				this.hisNameStartPosition=new Vector3(2f*ApplicationDesignRules.worldWidth/2f,-ApplicationDesignRules.worldHeight/2f+0.4f,0f);
				this.hisName.transform.localPosition=this.hisNameStartPosition;
				this.hisNameEndPosition=new Vector3(-0.2f+ApplicationDesignRules.worldWidth/2f,this.hisNameStartPosition.y,0f);

				this.myCardsStartPosition[i]=new Vector3(-ApplicationDesignRules.worldWidth/2f-ApplicationDesignRules.flipObjectWorldSize.x/2f-i*(horizontalGap), ApplicationDesignRules.worldHeight/2f-firstLineGap-ApplicationDesignRules.flipObjectWorldSize.y/2f-i*(ApplicationDesignRules.flipObjectWorldSize.y + verticalGap),0f);
				this.myCards[i].transform.localPosition=this.myCardsStartPosition[i];
				this.hisCardsStartPosition[i]=new Vector3(+ApplicationDesignRules.worldWidth/2f+ApplicationDesignRules.flipObjectWorldSize.x/2f+i*(horizontalGap), -ApplicationDesignRules.worldHeight/2f+firstLineGap+ApplicationDesignRules.flipObjectWorldSize.y/2f+i*(ApplicationDesignRules.flipObjectWorldSize.y + verticalGap),0f);
				this.hisCards[i].transform.localPosition=this.hisCardsStartPosition[i];
				this.myCardsEndPosition[i]=new Vector3(-0.1f-ApplicationDesignRules.worldWidth/2f+ApplicationDesignRules.flipObjectWorldSize.x/2f-i*(horizontalGap), this.myCardsStartPosition[i].y,0f);
				this.hisCardsEndPosition[i]=new Vector3(0.1f+ApplicationDesignRules.worldWidth/2f-ApplicationDesignRules.flipObjectWorldSize.x/2f+i*(horizontalGap), this.hisCardsStartPosition[i].y,0f);

				Vector3 myCardPosition=this.myCards[i].transform.localPosition;
				myCardPosition.x=this.myCardsStartPosition[i].x + (this.myCardsEndPosition[i].x-this.myCardsStartPosition[i].x)*this.myCardsCurrentPosition[i];
				this.myCards[i].transform.localPosition=myCardPosition;

				Vector3 hisCardPosition=this.hisCards[i].transform.localPosition;
				hisCardPosition.x=this.hisCardsStartPosition[i].x + (this.hisCardsEndPosition[i].x-this.hisCardsStartPosition[i].x)*this.hisCardsCurrentPosition[i];
				this.hisCards[i].transform.localPosition=hisCardPosition;
			}
		}
	}
	private IEnumerator toRewind()
	{
		yield return new WaitForSeconds(2.5f);
		this.vs.SetActive(false);
		this.toAnimateHisCards[ApplicationModel.nbCardsByDeck-1]=true;
		this.toAnimateMyCards[ApplicationModel.nbCardsByDeck-1]=true;
		this.toRewindPreMatchLoadingScreen=true;
		SoundController.instance.playSound(18);
	}
}


