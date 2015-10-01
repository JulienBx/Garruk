using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CupProgressionController : MonoBehaviour
{
	
	public Color[] gaugeColors;
	public GameObject roundObject;

	private GameObject[] rounds;
	private float roundXScale;
	private float blockHeight;
	private float blockYOrigin;
	
	
	void Awake()
	{
		this.rounds=new GameObject[0];
	}
	public void resize(Rect parentBlock)
	{
		float pixelPerUnit = 108f;
		float roundWidth = 59f;
		float roundWorldWidth = roundWidth / pixelPerUnit;
		float maxXScale = 14;
		this.roundXScale = (parentBlock.width-2f) / roundWorldWidth;
		if(this.roundXScale>maxXScale)
		{
			this.roundXScale=maxXScale;
		}
		this.blockHeight = parentBlock.height;
		this.blockYOrigin = parentBlock.y;

		for(int i=0;i<rounds.Length;i++)
		{
			Vector3 roundScale = rounds[i].transform.FindChild ("Background").transform.localScale;
			roundScale.x = roundXScale;
			rounds[i].transform.FindChild ("Background").transform.localScale=roundScale;
		}
		this.gameObject.transform.localPosition = new Vector3 (parentBlock.x, parentBlock.y, 0f);
		
	}
	public void drawCup(Cup c, bool hasWon)
	{
		float pixelPerUnit = 108f;
		float roundHeight = 59f;
		float maxRoundYScale = 1.5f;
		float maxPoliceScale = 1f;
		float roundWorldMaxHeight = (this.blockHeight - 3f) / c.NbRounds;

		float roundYScale = (roundWorldMaxHeight * pixelPerUnit) / roundHeight;
		if(roundYScale>maxRoundYScale)
		{
			roundYScale=maxRoundYScale;
		}
		float policeScale = roundYScale;
		if(roundYScale>maxPoliceScale)
		{
			policeScale=maxPoliceScale;
		}

		float roundWorldHeight = roundYScale * (roundHeight / pixelPerUnit);

		int remainingGames = c.NbRounds - c.GamesPlayed;
		if(remainingGames!=0)
		{
			gameObject.transform.FindChild ("RemainingRounds").GetComponent<TextMeshPro>().text=remainingGames.ToString()+" tours restants";
		}
		else
		{
			gameObject.transform.FindChild ("RemainingRounds").GetComponent<TextMeshPro>().text="Fin de la coupe";
		}
		gameObject.transform.FindChild ("RemainingRounds").localScale = new Vector3 (policeScale, policeScale, policeScale);
		gameObject.transform.FindChild("RemainingRounds").position=new Vector3(0f,this.blockYOrigin+0.15f+((float)c.NbRounds/2f)*(roundWorldHeight+0.1f)+0.25f*(roundWorldHeight+0.1f),0f);

		if(c.NbWins==c.NbRounds)
		{
			gameObject.transform.FindChild ("Status").GetComponent<TextMeshPro>().text="Statut actuel : vainqueur";
		}
		else if(c.GamesPlayed==0)
		{
			gameObject.transform.FindChild ("Status").GetComponent<TextMeshPro>().text="Statut actuel : non démarré";
		}
		else if(hasWon)
		{
			gameObject.transform.FindChild ("Status").GetComponent<TextMeshPro>().text="Statut actuel : qualifié";
		}
		else
		{
			gameObject.transform.FindChild ("Status").GetComponent<TextMeshPro>().text="Statut actuel : éliminé";
		}
		gameObject.transform.FindChild ("Status").localScale = new Vector3 (policeScale, policeScale, policeScale);
		gameObject.transform.FindChild("Status").position=new Vector3(0f,this.blockYOrigin+0.15f-((float)c.NbRounds/2f)*(roundWorldHeight+0.1f)-0.25f*(roundWorldHeight+0.1f),0f);

		this.rounds=new GameObject[c.NbRounds];
		for(int i =0;i<c.NbRounds;i++)
		{
			this.rounds[i] = Instantiate(this.roundObject) as GameObject;
			this.rounds[i].gameObject.transform.parent=this.gameObject.transform;
			this.rounds[i].transform.position=new Vector3(0f,this.blockYOrigin+0.15f+((float)c.NbRounds/2f)*(roundWorldHeight+0.1f)-(i+0.5f)*(roundWorldHeight+0.1f),0f);
			this.rounds[i].transform.localScale=new Vector3(1f,1f,1f);
			this.rounds[i].transform.FindChild("Background").localScale=new Vector3(this.roundXScale,roundYScale,1f);
			this.rounds[i].transform.FindChild("Title").localScale=new Vector3(policeScale,policeScale,policeScale);

			if((c.NbRounds-i)>c.GamesPlayed)
			{
				this.rounds[i].transform.FindChild("Background").GetComponent<SpriteRenderer>().color=this.gaugeColors[0];
			}
			else if((c.NbRounds-i)==c.GamesPlayed)
			{
				if(hasWon)
				{
					this.rounds[i].transform.FindChild("Background").GetComponent<SpriteRenderer>().color=this.gaugeColors[1];
				}
				else
				{
					this.rounds[i].transform.FindChild("Background").GetComponent<SpriteRenderer>().color=this.gaugeColors[2];
				}
			}
			else
			{
				this.rounds[i].transform.FindChild("Background").GetComponent<SpriteRenderer>().color=this.gaugeColors[1];
			}
			string roundName="";
			if(i==0)
			{
				roundName="Finale";
			}
			else if(i==1)
			{
				roundName="1/2 finale";
			}
			else if(i==2)
			{
				roundName="1/4 de finale";
			}
			else if(i==3)
			{
				roundName="1/8ème de finale";
			}
			else if(i==4)
			{
				roundName="1/16ème de finale";
			}
			else if(i==5)
			{
				roundName="1/32ème de finale";
			}
			else if(i==6)
			{
				roundName="1er tour préliminaire";
			}
			else if(i==7)
			{
				roundName="2ème tour préliminaire";
			}
			else if(i==8)
			{
				roundName="3ème tour préliminaire";
			}
			this.rounds[i].transform.FindChild("Title").GetComponent<TextMeshPro>().text=roundName;
		}

	}
}


