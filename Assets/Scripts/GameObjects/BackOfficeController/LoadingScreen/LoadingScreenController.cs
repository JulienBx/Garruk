using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

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

	private Vector3[] myNameStartPosition;
	private float[] myNameCurrentPosition;
	private Vector3[] myNameEndPosition;

	private Vector3[] hisNameStartPosition;
	private float[] hisNameCurrentPosition;
	private Vector3[] hisNameEndPosition;

	private bool toAnimatePreMatchLoadingScreen;

	void Awake()
	{
		this.speed = 300.0f;
		this.angle = 0f;
	}
	void Update()
	{
		this.angle = this.angle + this.speed * Time.deltaTime;
		this.target = Quaternion.Euler (0f, 0f, this.angle);
		this.gameObject.transform.FindChild("loadingCircle").transform.rotation = target;
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
		this.toAnimatePreMatchLoadingScreen=true;
	}
	private void initializePreMatchLoadingScreen()
	{
		myCards=new GameObject[ApplicationModel.nbCardsByDeck];
		hisCards=new GameObject[ApplicationModel.nbCardsByDeck];
		for(int i=0;i<ApplicationModel.nbCardsByDeck;i++)
		{
			this.myCards[i]=this.gameObject.transform.FindChild("myCard"+i).gameObject;
			this.hisCards[i]=this.gameObject.transform.FindChild("hisCard"+i).gameObject;
			this.myNameCurrentPosition[i]=0f;
			this.hisNameCurrentPosition[i]=0f;
		}
		this.myName= this.gameObject.transform.FindChild("myName").gameObject;
		this.hisName=this.gameObject.transform.FindChild("hisName").gameObject;
		this.vs=this.gameObject.transform.FindChild("VS").gameObject;
	}
	public void resize()
	{
		for(int i=0;i<ApplicationModel.nbCardsByDeck;i++)
		{
			this.myCards[i].transform.localScale=ApplicationDesignRules.flipObjectScale;
			this.hisCards[i].transform.localScale=ApplicationDesignRules.flipObjectScale;
			float verticalGap=0.25f;
			float horizontalGap = 0.2f;
			float firstLineGap=1f;
			if(ApplicationDesignRules.isMobileScreen)
			{

			}
			else
			{
				this.myNameStartPosition[i]=new Vector3(-ApplicationDesignRules.worldWidth/2f-ApplicationDesignRules.flipObjectWorldSize.x/2f-i*(horizontalGap), ApplicationDesignRules.worldHeight/2f-firstLineGap-ApplicationDesignRules.flipObjectWorldSize.y-i*(ApplicationDesignRules.flipObjectWorldSize.y),0f);
				this.hisNameStartPosition[i]=new Vector3(ApplicationDesignRules.worldWidth/2f+ApplicationDesignRules.flipObjectWorldSize.x/2f+i*(horizontalGap), -ApplicationDesignRules.worldHeight/2f+firstLineGap+ApplicationDesignRules.flipObjectWorldSize.y+i*(ApplicationDesignRules.flipObjectWorldSize.y),0f);
				this.myNameEndPosition[i]=new Vector3(-ApplicationDesignRules.worldWidth/2f+ApplicationDesignRules.flipObjectWorldSize.x/2f-i*(horizontalGap), this.myNameStartPosition[i].y,0f);
				this.hisNameEndPosition[i]=new Vector3(ApplicationDesignRules.worldWidth/2f-ApplicationDesignRules.flipObjectWorldSize.x/2f+i*(horizontalGap), this.hisNameStartPosition[i].y,0f);
			}
		}
	}
}


