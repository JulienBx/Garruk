using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class newMyGameController : MonoBehaviour
{
	public static newMyGameController instance;

	public GameObject cardObject;
	private GameObject menu;
	private GameObject deckBoard;
	private GameObject cardsBoard;
	private GameObject filters;
	private GameObject[] deckCards;
	private GameObject[] cards;
	private GameObject[] cursors;
	
	private int widthScreen;
	private int heightScreen;

	private bool isSearchingSkill;
	private string skillSearched;

	private float minPowerVal=0;
	private float maxPowerVal=100;
	private float minPowerLimit=0;
	private float maxPowerLimit=100;
	private float minLifeVal = 0;
	private float maxLifeVal = 100;
	private float minLifeLimit=0;
	private float maxLifeLimit=100;
	private float minAttackVal=0;
	private float maxAttackVal=100;
	private float minAttackLimit=0;
	private float maxAttackLimit=100;
	private float minQuicknessVal=0;
	private float maxQuicknessVal=100;
	private float minQuicknessLimit=0;
	private float maxQuicknessLimit=100;
	private float oldMinPowerVal=0;
	private float oldMaxPowerVal=100;
	private float oldMinLifeVal=0;
	private float oldMaxLifeVal=100;
	private float oldMinAttackVal=0;
	private float oldMaxAttackVal=100;
	private float oldMinQuicknessVal=0;
	private float oldMaxQuicknessVal=100;

	
	void Awake()
	{
		this.widthScreen = Screen.width;
		this.heightScreen = Screen.height;
		this.cards = new GameObject[0];
	}
	void Start()
	{
		instance = this;
		menu = GameObject.Find ("newMenu");
		menu.GetComponent<newMenuController> ().setCurrentPage (1);
		this.deckBoard = GameObject.Find ("deckBoard");
		this.cardsBoard = GameObject.Find ("cardsBoard");
		this.filters = GameObject.Find ("myGameFilters");
		this.deckCards=new GameObject[4];
		for (int i=0;i<4;i++)
		{
			this.deckCards[i]=GameObject.Find("deckCard"+i);
		}
		this.cursors=new GameObject[8];
		for (int i=0;i<8;i++)
		{
			this.cursors[i]=GameObject.Find("Cursor"+i);
		}
		this.resize ();
	}
	void Update()
	{	
		if (Screen.width != this.widthScreen || Screen.height != this.heightScreen) 
		{
			this.resize();

		}
		if(isSearchingSkill)
		{
			if(!Input.GetKey(KeyCode.Delete))
			{
				foreach (char c in Input.inputString) 
				{
					if(c==(char)KeyCode.Backspace && this.skillSearched.Length>0)
					{
						this.skillSearched = this.skillSearched.Remove(this.skillSearched.Length - 1);
						this.filters.transform.FindChild("skillSearch").FindChild ("SearchBar").FindChild("Text").GetComponent<TextMesh>().text = this.skillSearched;
						if(this.skillSearched.Length==0)
						{
							this.isSearchingSkill=false;
							this.filters.transform.FindChild("skillSearch").FindChild ("SearchBar").FindChild("Text").GetComponent<TextMesh>().text ="Rechercher";
						}
					}
					else if (c == "\b"[0])
					{
						if (skillSearched.Length != 0)
						{
							skillSearched= skillSearched.Substring(0, skillSearched.Length - 1);
						}
					}
					else
					{
						if (c == "\n"[0] || c == "\r"[0])
						{

						}
						else if(this.skillSearched.Length<12)
						{
							skillSearched += c;
							this.filters.transform.FindChild("skillSearch").FindChild ("SearchBar").FindChild("Text").GetComponent<TextMesh>().text = this.skillSearched;
						}
					}
				}
			}
		}
	}
	public void resize()
	{
		this.cleanCards ();
		this.widthScreen=Screen.width;
		this.heightScreen=Screen.height;
		float worldHeight = 2f*Camera.main.GetComponent<Camera>().orthographicSize;
		float worldWidth = ((float)Screen.width/(float)Screen.height) * worldHeight;
		float screenRatio = (float)this.widthScreen / (float)this.heightScreen;

		menu.GetComponent<newMenuController> ().resizeMeunObject (worldHeight,worldWidth);

		deckBoard.GetComponent<deckBoardController>().resize(screenRatio);

		this.deckBoard.transform.FindChild("deckList").FindChild("renameDeckButton").FindChild("Title").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.deckBoard.transform.FindChild("deckList").FindChild("renameDeckButton").FindChild("Title").GetComponent<TextMesh>().text = "Renommer";
		this.deckBoard.transform.FindChild("deckList").FindChild("deleteDeckButton").FindChild("Title").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.deckBoard.transform.FindChild("deckList").FindChild("deleteDeckButton").FindChild("Title").GetComponent<TextMesh>().text = "Supprimer";
		this.deckBoard.transform.FindChild("deckList").FindChild("newDeckButton").FindChild("Title").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.deckBoard.transform.FindChild("deckList").FindChild("newDeckButton").FindChild("Title").GetComponent<TextMesh>().text = "Nouveau deck";


		float cardsBoardUpMargin = (worldHeight/2f-deckBoard.transform.position.y)*2f-0.15f;
		float cardsBoardDownMargin = 0.35f;
		float cardsBoardLeftMargin = 2.9f;
		float cardsBoardRightMargin = 2.9f;
		float cardsBoardHeight = worldHeight - cardsBoardUpMargin-cardsBoardDownMargin;
		float cardsBoardWidth = worldWidth-cardsBoardLeftMargin-cardsBoardRightMargin;
		Vector2 cardsBoardOrigin = new Vector3 (-worldWidth/2f+cardsBoardLeftMargin+cardsBoardWidth/2f, -worldHeight / 2f + cardsBoardDownMargin + cardsBoardHeight / 2,0);

		this.cardsBoard.GetComponent<CardsBoardController> ().resize(cardsBoardWidth,cardsBoardHeight,cardsBoardOrigin);

		GameObject deckBoardTitle = GameObject.Find ("deckBoardTitle");
		deckBoardTitle.transform.FindChild("Title").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		float deckBoardTitlePosX = this.deckBoard.transform.FindChild ("deckList").position.x;
		float deckBoardTitlePosY = this.deckBoard.transform.FindChild ("Card0").FindChild ("stars").position.y;
		deckBoardTitle.transform.position = new Vector3 (deckBoardTitlePosX+0.3f, deckBoardTitlePosY, 0);

		float originalScale = 0.7417971f;
		float newScale=1-this.deckBoard.transform.localScale.x;
		newScale=originalScale-originalScale*newScale;

		for(int i=0;i<4;i++)
		{
			this.deckCards[i].transform.position=this.deckBoard.transform.FindChild("Card"+i).FindChild("cardHalo").position;
			this.deckCards[i].transform.localScale=new Vector3(newScale,newScale,newScale);
		}

		float pixelPerUnit = 108f;
		float cardWidth = 194f;
		float cardHeight = 271f;
		float cardWorldWidth = (cardWidth / pixelPerUnit) * newScale;
		float cardWorldHeight = (cardHeight / pixelPerUnit) * newScale;

		int cardsPerLine = Mathf.FloorToInt ((cardsBoardWidth - 0.5f) / cardWorldWidth);
		int nbLines = Mathf.FloorToInt ((cardsBoardHeight - 0.5f) / cardWorldHeight);

		float gapWidth = (cardsBoardWidth - (cardsPerLine * cardWorldWidth)) / (cardsPerLine + 1);
		float gapHeight = (cardsBoardHeight - (nbLines * cardWorldHeight)) / (nbLines + 1);
		float cardBoardStartX = cardsBoardOrigin.x - cardsBoardWidth / 2f-cardWorldWidth/2f;
		float cardBoardStartY = cardsBoardOrigin.y + cardsBoardHeight / 2f+cardWorldHeight/2f;

		this.cards=new GameObject[(cardsPerLine)*(nbLines)];

		for(int j=0;j<nbLines;j++)
		{
			for(int i =0;i<cardsPerLine;i++)
			{
				this.cards[j*(cardsPerLine)+i] = Instantiate(this.cardObject) as GameObject;
				this.cards[j*(cardsPerLine)+i].transform.localScale= new Vector3(newScale,newScale,newScale);
				this.cards[j*(cardsPerLine)+i].transform.position=new Vector3(cardBoardStartX+(i+1)*(gapWidth+cardWorldWidth),
				                                                                cardBoardStartY-(j+1)*(gapHeight+cardWorldHeight),
				                                                                0);
				this.cards[j*(cardsPerLine)+i].transform.name="Card"+(j*(cardsPerLine)+i);
			}
		}

		this.filters.transform.position = new Vector3 (worldWidth/2f - 1.4f, 0f, 0f);
		this.filters.transform.FindChild("Title").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("Title").GetComponent<TextMesh>().text = "Filtres";
		this.filters.transform.FindChild ("onSaleFilters").FindChild("Toggle0").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild ("onSaleFilters").FindChild("Toggle0").GetComponent<TextMesh>().text = "Cartes mises en vente";
		this.filters.transform.FindChild ("onSaleFilters").FindChild("Toggle1").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild ("onSaleFilters").FindChild("Toggle1").GetComponent<TextMesh>().text = "Cartes non mises en vente";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Title").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Title").GetComponent<TextMesh>().text = "Classes";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle2").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle2").GetComponent<TextMesh>().text = "Enchanteur";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle3").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle3").GetComponent<TextMesh>().text = "Roublard";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle4").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle4").GetComponent<TextMesh>().text = "Berserk";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle5").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle5").GetComponent<TextMesh>().text = "Artificier";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle6").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle6").GetComponent<TextMesh>().text = "Mentaliste";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle7").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle7").GetComponent<TextMesh>().text = "Androide";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle8").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle8").GetComponent<TextMesh>().text = "Metamorphe";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle9").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle9").GetComponent<TextMesh>().text = "Pretre";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle10").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle10").GetComponent<TextMesh>().text = "Animiste";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle11").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("cardTypeFilters").FindChild("Toggle11").GetComponent<TextMesh>().text = "Geomancien";
		this.filters.transform.FindChild("skillSearch").FindChild("Title").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("skillSearch").FindChild("Title").GetComponent<TextMesh>().text = "Compétences";
		this.filters.transform.FindChild("skillSearch").FindChild ("SearchBar").FindChild("Text").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("skillSearch").FindChild ("SearchBar").FindChild("Text").GetComponent<TextMesh>().text = "Rechercher";
		this.filters.transform.FindChild("valueFilter").FindChild ("Title").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("valueFilter").FindChild ("Title").GetComponent<TextMesh>().text = "Puissance";
		this.filters.transform.FindChild("valueFilter").FindChild ("MinValue").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("valueFilter").FindChild ("MinValue").GetComponent<TextMesh>().text = this.minPowerVal.ToString();
		this.filters.transform.FindChild("valueFilter").FindChild ("MaxValue").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("valueFilter").FindChild ("MaxValue").GetComponent<TextMesh>().text = this.maxPowerVal.ToString();
		this.filters.transform.FindChild("lifeFilter").FindChild ("Title").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("lifeFilter").FindChild ("Title").GetComponent<TextMesh>().text = "Vie";
		this.filters.transform.FindChild("lifeFilter").FindChild ("MinValue").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("lifeFilter").FindChild ("MinValue").GetComponent<TextMesh>().text = this.minLifeVal.ToString();
		this.filters.transform.FindChild("lifeFilter").FindChild ("MaxValue").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("lifeFilter").FindChild ("MaxValue").GetComponent<TextMesh>().text = this.maxLifeVal.ToString();
		this.filters.transform.FindChild("attackFilter").FindChild ("Title").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("attackFilter").FindChild ("Title").GetComponent<TextMesh>().text = "Attaque";
		this.filters.transform.FindChild("attackFilter").FindChild ("MinValue").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("attackFilter").FindChild ("MinValue").GetComponent<TextMesh>().text = this.minAttackVal.ToString();
		this.filters.transform.FindChild("attackFilter").FindChild ("MaxValue").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("attackFilter").FindChild ("MaxValue").GetComponent<TextMesh>().text = this.maxAttackVal.ToString();
		this.filters.transform.FindChild("quicknessFilter").FindChild ("Title").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("quicknessFilter").FindChild ("Title").GetComponent<TextMesh>().text = "Vitesse";
		this.filters.transform.FindChild("quicknessFilter").FindChild ("MinValue").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("quicknessFilter").FindChild ("MinValue").GetComponent<TextMesh>().text = this.minQuicknessVal.ToString();
		this.filters.transform.FindChild("quicknessFilter").FindChild ("MaxValue").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.filters.transform.FindChild("quicknessFilter").FindChild ("MaxValue").GetComponent<TextMesh>().text = this.maxQuicknessVal.ToString();

	}
	public void cleanCards()
	{
		for (int i=0;i<this.cards.Length;i++)
		{
			Destroy (this.cards[i]);
		}
	}
	public void searchingSkill()
	{
		this.isSearchingSkill = true;
		this.skillSearched = "";
		this.filters.transform.FindChild("skillSearch").FindChild ("SearchBar").FindChild("Text").GetComponent<TextMesh>().text = this.skillSearched;
	}
	public void moveCursor(float mousePositionX, int cursorId)
	{
		Vector3 cursorPosition = this.cursors [cursorId].transform.position;
		float sliderPositionX = this.cursors [cursorId].transform.parent.gameObject.transform.position.x;
		float cursorSizeX = (78f / 108f) * 0.3f;
		float offset = mousePositionX-cursorPosition.x;
		cursorPosition.x = cursorPosition.x + offset;
		int secondCursorId;
		float secondCursorPositionX;
		float distance;
		if(cursorId%2==0)
		{
			secondCursorId=cursorId+1;
			secondCursorPositionX = this.cursors [secondCursorId].transform.position.x;
			if(cursorPosition.x>secondCursorPositionX-cursorSizeX)
			{
				cursorPosition.x=secondCursorPositionX-cursorSizeX;
			}
			else if(cursorPosition.x<-0.975f+sliderPositionX)
			{
				cursorPosition.x=-0.975f+sliderPositionX;
			}
			distance = cursorPosition.x -(-0.975f+sliderPositionX)-0.01f;
		}
		else
		{
			secondCursorId=cursorId-1;
			secondCursorPositionX = this.cursors [secondCursorId].transform.position.x;
			if(cursorPosition.x>0.975f+sliderPositionX)
			{
				cursorPosition.x=0.975f+sliderPositionX;
			}
			else if(cursorPosition.x<secondCursorPositionX+cursorSizeX)
			{
				cursorPosition.x=secondCursorPositionX+cursorSizeX;
			}
			distance = (0.975f+sliderPositionX)-cursorPosition.x+0.01f;
		}
		this.cursors [cursorId].transform.position = cursorPosition;
		float maxDistance = 2 * 0.975f-cursorSizeX;
		float ratio = distance / maxDistance;
		switch (cursorId) 
		{
		case 0:
			minPowerVal=minPowerLimit+Mathf.CeilToInt(ratio*(maxPowerLimit-minPowerLimit));
			if(minPowerVal!=oldMinPowerVal)
			{
				this.filters.transform.FindChild("valueFilter").FindChild ("MinValue").GetComponent<TextMesh>().text = minPowerVal.ToString();
				this.oldMinPowerVal=this.minPowerVal;
			}
			break;
		case 1:
			maxPowerVal=maxPowerLimit-Mathf.FloorToInt(ratio*(maxPowerLimit-minPowerLimit));
			if(maxPowerVal!=oldMaxPowerVal)
			{
				this.filters.transform.FindChild("valueFilter").FindChild ("MaxValue").GetComponent<TextMesh>().text = maxPowerVal.ToString();
				this.oldMaxPowerVal=this.maxPowerVal;
			}
			break;
		case 2:
			minLifeVal=minLifeLimit+Mathf.CeilToInt(ratio*(maxLifeLimit-minLifeLimit));
			if(minLifeVal!=oldMinLifeVal)
			{
				this.filters.transform.FindChild("lifeFilter").FindChild ("MinValue").GetComponent<TextMesh>().text = minLifeVal.ToString();
				this.oldMinLifeVal=this.minLifeVal;
			}
			break;
		case 3:
			maxLifeVal=maxLifeLimit-Mathf.FloorToInt(ratio*(maxLifeLimit-minLifeLimit));
			if(maxLifeVal!=oldMaxLifeVal)
			{
				this.filters.transform.FindChild("lifeFilter").FindChild ("MaxValue").GetComponent<TextMesh>().text = maxLifeVal.ToString();
				this.oldMaxLifeVal=this.maxLifeVal;
			}
			break;
		case 4:
			minAttackVal=minAttackLimit+Mathf.CeilToInt(ratio*(maxAttackLimit-minAttackLimit));
			if(minAttackVal!=oldMinAttackVal)
			{
				this.filters.transform.FindChild("attackFilter").FindChild ("MinValue").GetComponent<TextMesh>().text = minAttackVal.ToString();
				this.oldMinAttackVal=this.minAttackVal;
			}
			break;
		case 5:
			maxAttackVal=maxAttackLimit-Mathf.FloorToInt(ratio*(maxAttackLimit-minAttackLimit));
			if(maxAttackVal!=oldMaxAttackVal)
			{
				this.filters.transform.FindChild("attackFilter").FindChild ("MaxValue").GetComponent<TextMesh>().text = maxAttackVal.ToString();
				this.oldMaxAttackVal=this.maxAttackVal;
			}
			break;
		case 6:
			minQuicknessVal=minQuicknessLimit+Mathf.CeilToInt(ratio*(maxQuicknessLimit-minQuicknessLimit));
			if(minQuicknessVal!=oldMinQuicknessVal)
			{
				this.filters.transform.FindChild("quicknessFilter").FindChild ("MinValue").GetComponent<TextMesh>().text = minQuicknessVal.ToString();
				this.oldMinQuicknessVal=this.minQuicknessVal;
			}
			break;
		case 7:
			maxQuicknessVal=maxQuicknessLimit-Mathf.FloorToInt(ratio*(maxQuicknessLimit-minQuicknessLimit));
			if(maxQuicknessVal!=oldMaxQuicknessVal)
			{
				this.filters.transform.FindChild("quicknessFilter").FindChild ("MaxValue").GetComponent<TextMesh>().text = maxQuicknessVal.ToString();
				this.oldMaxQuicknessVal=this.maxQuicknessVal;
			}
			break;
		}
	}
}