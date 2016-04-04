using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadingScreenController : MonoBehaviour
{
	
	private float angle;
	private float speed;
	private Quaternion target;

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

	private bool toAnimatePreMatchLoadingScreen;
	private bool toShowLoading;

	void Awake()
	{
		this.speed = 300.0f;
		this.angle = 0f;
		this.toShowLoading=true;
	}
	void Update()
	{
		if(this.toAnimatePreMatchLoadingScreen)
		{
			bool isOver=true;
			for(int i=0;i<ApplicationModel.nbCardsByDeck;i++)
			{
				if(this.toAnimateMyCards[i])
				{
					this.myCardsCurrentPosition[i]=this.myCardsCurrentPosition[i]+3f*Time.deltaTime;
					if(this.myCardsCurrentPosition[i]>0.3f && i<ApplicationModel.nbCardsByDeck-1 && !this.toAnimateMyCards[i+1])
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
					this.hisCardsCurrentPosition[i]=this.hisCardsCurrentPosition[i]+3f*Time.deltaTime;
					if(this.hisCardsCurrentPosition[i]>0.3f && i<ApplicationModel.nbCardsByDeck-1 && !this.toAnimateHisCards[i+1])
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
				SceneManager.LoadScene("Game");
			}
		}
		if(this.toShowLoading)
		{
			this.angle = this.angle + this.speed * Time.deltaTime;
			this.target = Quaternion.Euler (0f, 0f, this.angle);
			this.gameObject.transform.FindChild("loadingCircle").transform.rotation = target;
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
			this.myCards[i].SetActive(true);
			this.hisCards[i]=this.gameObject.transform.FindChild("hisCard"+i).gameObject;
			this.hisCards[i].SetActive(true);
			this.myCardsCurrentPosition[i]=0f;
			this.hisCardsCurrentPosition[i]=0f;
		}
		this.toAnimateMyCards[0]=true;
		this.toAnimateHisCards[0]=true;
		this.myName= this.gameObject.transform.FindChild("myName").gameObject;
		this.hisName=this.gameObject.transform.FindChild("hisName").gameObject;
		this.vs=this.gameObject.transform.FindChild("VS").gameObject;
		this.gameObject.transform.FindChild("loadingCircle").gameObject.SetActive(false);
		this.gameObject.transform.FindChild("button").gameObject.SetActive(false);
		this.gameObject.transform.FindChild("title").gameObject.SetActive(false);
		this.gameObject.transform.FindChild("background2").GetComponent<SpriteRenderer>().sprite=this.gameObject.transform.parent.GetComponent<BackOfficeRessources>().loadingScreenBackgrounds[1];
	}
	public void resize()
	{
		for(int i=0;i<ApplicationModel.nbCardsByDeck;i++)
		{
			this.myCards[i].transform.localScale=ApplicationDesignRules.flipObjectScale;
			this.hisCards[i].transform.localScale=ApplicationDesignRules.flipObjectScale;
			float verticalGap=0.3f;
			float horizontalGap = 1.1f;
			float firstLineGap=1f;
			if(ApplicationDesignRules.isMobileScreen)
			{

			}
			else
			{
				this.myCardsStartPosition[i]=new Vector3(-ApplicationDesignRules.worldWidth/2f-ApplicationDesignRules.flipObjectWorldSize.x/2f-i*(horizontalGap), ApplicationDesignRules.worldHeight/2f-firstLineGap-ApplicationDesignRules.flipObjectWorldSize.y/2f-i*(ApplicationDesignRules.flipObjectWorldSize.y + verticalGap),0f);
				this.myCards[i].transform.localPosition=this.myCardsStartPosition[i];
				this.hisCardsStartPosition[i]=new Vector3(ApplicationDesignRules.worldWidth/2f+ApplicationDesignRules.flipObjectWorldSize.x/2f+i*(horizontalGap), -ApplicationDesignRules.worldHeight/2f+firstLineGap+ApplicationDesignRules.flipObjectWorldSize.y/2f+i*(ApplicationDesignRules.flipObjectWorldSize.y + verticalGap),0f);
				this.hisCards[i].transform.localPosition=this.hisCardsStartPosition[i];
				this.myCardsEndPosition[i]=new Vector3(-ApplicationDesignRules.worldWidth/2f+ApplicationDesignRules.flipObjectWorldSize.x/2f-i*(horizontalGap), this.myCardsStartPosition[i].y,0f);
				this.hisCardsEndPosition[i]=new Vector3(ApplicationDesignRules.worldWidth/2f-ApplicationDesignRules.flipObjectWorldSize.x/2f+i*(horizontalGap), this.hisCardsStartPosition[i].y,0f);
			}
		}
	}
}


