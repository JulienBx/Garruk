using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameCard_experience : MonoBehaviour 
{

	public Texture[] xplevels; 	

	GameObject Parent;
	float minScaleYGauge =0.05f; 
	float maxScaleYGauge =0.27f;
	public GameObject addXpGO;
	bool toMove=false;
	GameObject animation =null;
	Vector3 destination;
	float scaleSpeed=0.5f;
	float startGaugeScale;
	float startPercentage;
	float currentPercentage;
	float endPercentage;
	int startLevel;
	int currentLevel;
	int endLevel;
	float nbIteration;
	float animationRatio;
	

	void Awake () 
	{
		Parent = transform.parent.gameObject;
		Parent = Parent.transform.parent.gameObject;	
	}

	void Update ()
	{

		if (Card.xpDone) {

			Card.xpDone = false;

			endLevel=Parent.GetComponent<GameCard> ().Card.ExperienceLevel;
			endPercentage=0.01f*(float)Parent.GetComponent<GameCard> ().Card.percentageToNextXpLevel();

			currentLevel=startLevel;
			currentPercentage=startPercentage;

			if(endLevel!=10)
			nbIteration=endLevel+endPercentage-startLevel-startPercentage;
			else
			nbIteration=10-startLevel-startPercentage;

			animation = Instantiate(addXpGO) as GameObject;
			animation.transform.parent = Parent.transform;
			
			animation.transform.localScale=new Vector3(0f,0f,1f);
			animation.transform.position = new Vector3(Parent.transform.position.x,Parent.transform.position.y,Parent.transform.position.z-0.3f);

			scaleSpeed=0.5f;


			toMove=true;
		}

		if(toMove){

			float increase = scaleSpeed * Time.deltaTime;


			if (startLevel!=endLevel || startPercentage!=endPercentage){


				if(startPercentage==1f){
					startLevel=startLevel+1;
					if (startLevel!=10)
						startPercentage=0;
					for (int i = 0 ; i < 6 ; i++)
					{
						transform.FindChild ("ExperienceGauge")
							.renderer.materials[i].mainTexture = xplevels [startLevel];
					}
					transform.FindChild("Level").GetComponent<TextMesh> ().text = "Lvl"+startLevel ;
				}
				else{
					startPercentage=startPercentage+increase;
					if (startPercentage>1f){
						startPercentage=1f;
					}
					if(startLevel==endLevel){
						if(startPercentage>endPercentage)
							startPercentage=endPercentage;
					}

				}

				transform.FindChild ("ExperienceGauge")
					.localScale = new Vector3 (transform.FindChild ("ExperienceGauge").localScale.x,
					                           0.06f + startPercentage * 0.27f,
					                           transform.FindChild ("ExperienceGauge").localScale.z);
				
				transform.FindChild ("Percentage").GetComponent<TextMesh> ().text = Mathf.CeilToInt(100f*(startPercentage)) + "%";
				
				transform.FindChild ("ExperienceGauge")
					.localPosition = new Vector3 (transform.FindChild ("ExperienceGauge").localPosition.x,
					                              transform.FindChild ("ExperienceGauge").localScale.y / 2 + 0.02f,
					                              transform.FindChild ("ExperienceGauge").localPosition.z);
				
				transform.FindChild ("Percentage")
					.localPosition = new Vector3 (transform.FindChild ("Percentage").localPosition.x,
					                              transform.FindChild ("ExperienceGauge").localScale.y - 0.01f,
					                              transform.FindChild ("Percentage").localPosition.z);
			
				animationRatio=(currentLevel+currentPercentage-startLevel-startPercentage)/nbIteration;

				animation.transform.localScale = new Vector3(animationRatio,
															animationRatio,
															animation.transform.localScale.z);

			}

			else{	
				Destroy (animation);
				Parent.GetComponent<GameCard> ().ShowFace();
				setXpLevel();
				toMove=false;
			}
		}

	}


	public void addXp(int xp, int price){


		Parent.GetComponent<GameCard> ().Card.getCardXpLevel ();
		startLevel = Parent.GetComponent<GameCard> ().Card.ExperienceLevel;
		startPercentage = 0.01f*(float)Parent.GetComponent<GameCard> ().Card.percentageToNextXpLevel();

		StartCoroutine(Parent.GetComponent<GameCard>().Card.addXp(xp,price));
	}
	


	public void setXpLevel(){

		int cardXp = Parent.GetComponent<GameCard> ().Card.Experience;

		Parent.GetComponent<GameCard> ().Card.getCardXpLevel ();
		int level = Parent.GetComponent<GameCard> ().Card.ExperienceLevel;
	
		for (int i = 0 ; i < 6 ; i++)
		{
			transform.FindChild ("ExperienceGauge")
				.renderer.materials[i].mainTexture = xplevels [level];
		}
				
		transform.FindChild("Level").GetComponent<TextMesh> ().text = "Lvl"+level ;

		int percentage = Parent.GetComponent<GameCard> ().Card.percentageToNextXpLevel ();

		transform.FindChild ("Percentage").GetComponent<TextMesh> ().text = percentage + "%";

		transform.FindChild ("ExperienceGauge")
			.localScale = new Vector3 (transform.FindChild ("ExperienceGauge").localScale.x,
			                           0.06f + 0.01f * percentage * 0.27f,
			                           transform.FindChild ("ExperienceGauge").localScale.z);

		transform.FindChild ("ExperienceGauge")
			.localPosition = new Vector3 (transform.FindChild ("ExperienceGauge").localPosition.x,
			                                transform.FindChild ("ExperienceGauge").localScale.y / 2 + 0.02f,
			                                transform.FindChild ("ExperienceGauge").localPosition.z);


		transform.FindChild ("Percentage")
			.localPosition = new Vector3 (transform.FindChild ("Percentage").localPosition.x,
			                                transform.FindChild ("ExperienceGauge").localScale.y - 0.01f,
			                                transform.FindChild ("Percentage").localPosition.z);


	}

}
	
