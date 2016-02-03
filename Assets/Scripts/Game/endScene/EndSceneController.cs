using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;

public class EndSceneController : MonoBehaviour 
{
	public static EndSceneController instance;

	public GameObject endGamePanelObject;
	public GameObject cardObject;
	public GameObject nextLevelPopUpObject;

	private GameObject endGamePanel;
	private GameObject nextLevelPopUp;
	private GameObject[] cards;

	private bool toUpdateCredits;
	private bool toStartExperienceUpdate;
	private bool areCreditsUpdated;

	private float updateSpeed;
	private float updateRatio;

	private int earnXp;
	private int earnCredits;

	private int endCredits;
	private int currentCredits;
	private int startCredits;

	private int xpDrawn;

	public int collectionPointsEarned;
	public int newCollectionRanking;
	public int idCardTypeUnlocked;
	public string titleCardTypeUnlocked;
	public List<Skill> skillsUnlocked;

	private IList<int> idCardsToNextLevel;

	private bool isNextLevelPopUpDisplayed;

	private string urlAddXpToDeck = ApplicationModel.host + "add_xp_to_deck.php";
	private string urlUpgradeCardAttribute = ApplicationModel.host + "upgrade_card_attribute.php";
	

	void Awake()
	{
		instance = this;
		this.updateSpeed = 1.5f;
		this.updateRatio = 0;
		this.toUpdateCredits = false;
		this.toStartExperienceUpdate = false;
		this.areCreditsUpdated = false;
		this.idCardsToNextLevel = new List<int> ();
	}
	void Update () 
	{
		if (this.toUpdateCredits)
		{
			this.updateRatio = this.updateRatio + this.updateSpeed * Time.deltaTime;
			if(this.updateRatio>=1)
			{
				this.currentCredits=this.endCredits;
				this.toUpdateCredits=false;
				this.areCreditsUpdated=true;
			}
			else
			{
				this.currentCredits=this.startCredits+(int)(this.updateRatio*this.earnCredits);
			}
			this.endGamePanel.transform.FindChild("Credits").GetComponent<TextMeshPro>().text="Vous avez gagné + "+ this.earnCredits+ " crédits ("+this.currentCredits+" crédits)";
		}
		if(this.areCreditsUpdated && this.toStartExperienceUpdate)
		{
			for(int i=0;i<this.cards.Length;i++)
			{
				this.cards[i].GetComponent<NewCardEndSceneController>().animateExperience();
			}
			this.toStartExperienceUpdate=false;
		}
	}
	public void displayEndScene(bool hasWon)
	{
		this.retrieveBonus (hasWon);
		this.endGamePanel = Instantiate(endGamePanelObject) as GameObject;
		this.endGamePanel.transform.FindChild ("Button").gameObject.SetActive (false); 
		this.endGamePanel.transform.position = new Vector3 (0f, 0f, -8f);
		this.cards=new GameObject[ApplicationModel.nbCardsByDeck];
		Deck myDeck = GameView.instance.getMyDeck();
		
		for(int i=0;i<ApplicationModel.nbCardsByDeck;i++)
		{
			cards[i]=Instantiate(cardObject) as GameObject;
			cards[i].transform.position=new Vector3(-4.5f+i*3f,0f,-8f);
			cards[i].AddComponent<NewCardEndSceneController>();
			cards[i].GetComponent<NewCardController>().c = myDeck.cards[i];
			cards[i].GetComponent<NewCardEndSceneController>().show();
			cards[i].GetComponent<NewCardEndSceneController>().changeLayer(11,"UIA");
			cards[i].GetComponent<NewCardEndSceneController>().setId(i);
			cards[i].transform.localScale=new Vector3(0.3108f,0.3108f,0.3108f);
		}

		if(hasWon)
		{
			this.endGamePanel.transform.FindChild("Title").GetComponent<TextMeshPro>().text="BRAVO !";
			ApplicationModel.player.HasWonLastGame=true;
		}
		else
		{
			this.endGamePanel.transform.FindChild("Title").GetComponent<TextMeshPro>().text="DOMMAGE !";
			ApplicationModel.player.HasWonLastGame=false;
		}

		StartCoroutine (this.drawCredits());
		StartCoroutine (this.addXp ());
	}
	
	private void retrieveBonus(bool hasWon)
	{
		switch(ApplicationModel.player.ChosenGameType)
		{
		case 0:
			if(hasWon)
			{
				this.earnXp=ApplicationModel.player.CurrentFriendlyGame.EarnXp_W;
				this.earnCredits=ApplicationModel.player.CurrentFriendlyGame.EarnCredits_W;
			}
			else
			{
				this.earnXp=ApplicationModel.player.CurrentFriendlyGame.EarnXp_L;
				this.earnCredits=ApplicationModel.player.CurrentFriendlyGame.EarnCredits_L;
			}
			break;
		case 1:
			if(hasWon)
			{
				this.earnXp=ApplicationModel.player.CurrentDivision.EarnXp_W;
				this.earnCredits=ApplicationModel.player.CurrentDivision.EarnCredits_W;
			}
			else
			{
				this.earnXp=ApplicationModel.player.CurrentDivision.EarnXp_L;
				this.earnCredits=ApplicationModel.player.CurrentDivision.EarnCredits_L;
			}
			break;
		case 2:
			if(hasWon)
			{
				this.earnXp=ApplicationModel.player.CurrentCup.EarnXp_W;
				this.earnCredits=ApplicationModel.player.CurrentCup.EarnCredits_W;
			}
			else
			{
				this.earnXp=ApplicationModel.player.CurrentCup.EarnXp_L;
				this.earnCredits=ApplicationModel.player.CurrentCup.EarnCredits_L;
			}
			break;
		}
	}
	public IEnumerator drawCredits()
	{
		yield return StartCoroutine(ApplicationModel.player.addMoney(earnCredits));
		this.endCredits = ApplicationModel.player.Money;
		this.startCredits = ApplicationModel.player.Money - this.earnCredits;
		this.toUpdateCredits = true;
	}
	public IEnumerator addXp()
	{
		this.skillsUnlocked = new List<Skill> ();
		this.titleCardTypeUnlocked = "";
		string idCards = "";
		Deck myDeck = GameView.instance.getMyDeck();
		
		for (int i=0; i<ApplicationModel.nbCardsByDeck; i++)
		{
			idCards = idCards + myDeck.cards [i].Id.ToString() + "SEPARATOR";
		}
		
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_idcard", idCards);
		form.AddField("myform_xp", this.earnXp);
		form.AddField("myform_nick", ApplicationModel.player.Username); 
		form.AddField ("myform_nbcardsbydeck", ApplicationModel.nbCardsByDeck);
		
		WWW w = new WWW(urlAddXpToDeck, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		
		if (w.error != null)
		{
			Debug.Log(w.error); 											// donne l'erreur eventuelle
		}
		else
		{
			string [] cardsData = w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			this.collectionPointsEarned=System.Convert.ToInt32(cardsData[cardsData.Length-2]);
			this.newCollectionRanking=System.Convert.ToInt32(cardsData[cardsData.Length-1]);
			for(int i=0;i<ApplicationModel.nbCardsByDeck;i++)
			{
				string [] experienceData = cardsData[i].Split(new string[] {"#EXPERIENCEDATA#"},System.StringSplitOptions.None);
				myDeck.cards[i].parseCard(experienceData[0]);
				myDeck.cards[i].GetNewSkill=System.Convert.ToBoolean(System.Convert.ToInt32(experienceData[1]));
				//this.Cards[i].NewSkills=new List<Skill>();
				if(myDeck.cards[i].GetNewSkill)
				{
					for(int j=0;j<myDeck.cards[i].Skills.Count;j++)
					{
						if(myDeck.cards[i].Skills[myDeck.cards[i].Skills.Count-j-1].IsActivated==1)
						{
							if(System.Convert.ToBoolean(System.Convert.ToInt32(experienceData[2])))
							{
								this.skillsUnlocked.Add (myDeck.cards[i].Skills[myDeck.cards[i].Skills.Count-j-1]);
								myDeck.cards[i].Skills[myDeck.cards[i].Skills.Count-j-1].IsNew=true;
							}
							break;
						}
					}
				}
			}
		}
		this.toStartExperienceUpdate = true;
		this.xpDrawn = 0;
	}
	public void incrementXpDrawn(bool hasChangedLevel, int id)
	{
		this.xpDrawn++;
		if(hasChangedLevel)
		{
			this.idCardsToNextLevel.Add (id);
		}
		if(xpDrawn==this.cards.Length)
		{
			if(this.idCardsToNextLevel.Count>0)
			{
				this.displayNextLevelPopUp(this.idCardsToNextLevel[0]);
			}
			else
			{
				this.showEndButton();	
			}
		}
	}
	public void quitEndSceneHandler()
	{
		//GameController.instance.disconnect ();
		ApplicationModel.player.ToLaunchEndGameSequence=true;
		if(ApplicationModel.player.ChosenGameType==1 || ApplicationModel.player.ChosenGameType==2)
		{
			Application.LoadLevel("NewLobby");
		}
		else
		{
			Application.LoadLevel("NewHomePage");
		}
	}
	public Vector3 getQuitButtonPosition()
	{
		return this.endGamePanel.transform.FindChild ("Button").gameObject.transform.position;
	}
	public void showEndButton()
	{
		this.endGamePanel.transform.FindChild("Button").gameObject.SetActive(true);
	}
	public void displayNextLevelPopUp(int indexCard)
	{
		Deck myDeck = GameView.instance.getMyDeck();
		
		this.nextLevelPopUp = Instantiate(this.nextLevelPopUpObject) as GameObject;
		this.nextLevelPopUp.transform.parent=this.endGamePanel.transform;
		this.nextLevelPopUp.transform.localPosition = new Vector3 (0, 0, -1f);
		this.nextLevelPopUp.AddComponent<NextLevelPopUpControllerEndSceneGame> ();
		this.nextLevelPopUp.transform.GetComponent<NextLevelPopUpController> ().initialize (myDeck.cards[indexCard]);
		this.isNextLevelPopUpDisplayed=true;
	}
	public void hideNextLevelPopUp()
	{
		this.idCardsToNextLevel.RemoveAt(0);
		Destroy (this.nextLevelPopUp);
		this.isNextLevelPopUpDisplayed=false;
		if(this.idCardsToNextLevel.Count>0)
		{
			this.displayNextLevelPopUp(this.idCardsToNextLevel[0]);
		}
		else
		{
			this.showEndButton();
			for(int i=0;i<this.cards.Length;i++)
			{
				this.cards[i].GetComponent<NewCardEndSceneController>().endUpdatingCardToNextLevel();
			}
			if(this.collectionPointsEarned>0)
			{
				this.endGamePanel.transform.FindChild("Credits").GetComponent<TextMeshPro>().text =this.endGamePanel.transform.FindChild("Credits").GetComponent<TextMeshPro>().text+"\n et "+this.collectionPointsEarned+" points de collection (classement : "+this.newCollectionRanking+")";
			}
			if(this.titleCardTypeUnlocked!="")
			{
				this.endGamePanel.transform.FindChild("NewCardType").GetComponent<TextMeshPro>().text="Bravo ! Vous venez d'acquérir la classe : "+this.titleCardTypeUnlocked;
			}
			if(this.skillsUnlocked.Count>0)
			{
				this.endGamePanel.transform.FindChild("Skills").GetComponent<TextMeshPro>().text="Vous débloquez : ";
				for(int i =0;i<this.skillsUnlocked.Count;i++)
				{
					this.endGamePanel.transform.FindChild("Skills").GetComponent<TextMeshPro>().text=this.endGamePanel.transform.FindChild("Skills").GetComponent<TextMeshPro>().text+"\n- "+this.skillsUnlocked[i].Name;
				}
			}
		}
	}
	public IEnumerator upgradeCardAttribute(int attributeToUpgrade, int newPower, int newLevel)
	{
		GameView.instance.displayLoadingScreen ();
		Deck myDeck = GameView.instance.getMyDeck();
		
		WWWForm form = new WWWForm(); 								
		form.AddField("myform_hash", ApplicationModel.hash); 		
		form.AddField("myform_idcard", myDeck.cards[this.idCardsToNextLevel[0]].Id.ToString());
		form.AddField("myform_nick", ApplicationModel.player.Username);
		form.AddField ("myform_attribute", attributeToUpgrade);
		form.AddField ("myform_newpower", newPower);
		form.AddField ("myform_newlevel", newLevel);
		
		WWW w = new WWW(urlUpgradeCardAttribute, form); 								
		yield return w; 											
		
		if (w.error == null)
		{
			if (!w.text.Contains("#ERROR#"))
			{
				string [] cardData = w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
				string [] experienceData = cardData[0].Split(new string[] {"#EXPERIENCEDATA#"},System.StringSplitOptions.None);
				myDeck.cards[this.idCardsToNextLevel[0]].parseCard(experienceData[0]);
				this.titleCardTypeUnlocked=experienceData[1];
				this.idCardTypeUnlocked=System.Convert.ToInt32(experienceData[2]);
				this.cards[this.idCardsToNextLevel[0]].GetComponent<NewCardController>().caracteristicUpgraded=System.Convert.ToInt32(experienceData[3]);
				this.cards[this.idCardsToNextLevel[0]].GetComponent<NewCardController>().caracteristicIncrease=System.Convert.ToInt32(experienceData[4]);
				this.collectionPointsEarned=this.collectionPointsEarned+ System.Convert.ToInt32(cardData [1]);
				this.newCollectionRanking=System.Convert.ToInt32(cardData[2]);
				this.hideNextLevelPopUp();
			}
		}
		GameView.instance.hideLoadingScreen ();
	}
}

