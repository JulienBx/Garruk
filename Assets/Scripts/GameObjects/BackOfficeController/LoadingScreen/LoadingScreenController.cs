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
//	private void initializePreMatchLoadingScreen()
//	{
//		myCards=new GameObject[ApplicationModel.nbCardsByDeck];
//		hisCards=new GameObject[ApplicationModel.nbCardsByDeck];
//		for(int i=0;i<ApplicationModel.nbCardsByDeck;i++)
//		{
//			this.myCards[i]=this.gameObject.transform.FindChild("myCard"+i);
//			this.hisCards[i]=this.gameObject.transform.FindChild("hisCard"+i);
//		}
//		this.myName= this.gameObject.transform.FindChild("myName");
//		this.hisName=this.gameObject.transform.FindChild("hisName");
//		this.vs=this.gameObject.transform.FindChild("VS");
//	}
//	public void resize()
//	{
//		for(int i=0;i<ApplicationModel.nbCardsByDeck;i++)
//		{
//			this.myCards[i].transform.localScale=ApplicationDesignRules.flipObjectScale;
//			this.hisCards[i].transform.localScale=ApplicationDesignRules.flipObjectScale;
//			if(ApplicationDesignRules.isMobileScreen)
//			{
//
//			}
//			else
//			{
//				//this.myNameStartPosition[i]=new Vector3(ApplicationDesignRules.worldWidth
//			}
//		}
//
//	}
}


