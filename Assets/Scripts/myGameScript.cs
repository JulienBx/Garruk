using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class myGameScript : MonoBehaviour {
	private string URLGetDecks = "http://54.77.118.214/GarrukServer/get_decks_by_user.php";
	private string URLGetCardsByDeck = "http://localhost/GarrukServer/get_cards_by_deck.php";
	private string URLGetMyCardsPage = "http://54.77.118.214/GarrukServer/get_mycardspage_data.php";


	private IList<Deck> myDecks;
	private bool isDeckEnabled = false ;
	public Texture2D backButton ;
	public Texture2D backCard ;
	public Texture2D backActivatedButton ;
	GameObject myDecksPanel ;
	GUIStyle[] myDecksGuiStyle;
	private int chosenDeck = 0;
	private int chosenIdDeck ;

//	public GameObject CardObject;												 
	string[] skillsList;
	string[] cardTypeList;
	public IList<Card> cards ;
	public IList<int> cardsToBeFiltered ;
	public bool[] cardsToBeDisplayed ;

//	GameObject[] filtres ;
//	private bool[] togglesCurrentStates;
//	private string valueSkill = null ;
//	int counterFilters ;
//	IList<string> matchValues;
//	public Texture2D textureAutoc ;
//	private string filtreAutoC ;
//	
//	bool dataIsLoaded = false ;
//	
//	public Rect[] dragFilters = new Rect[2];
//	
//	private Vector2 currentDrag = new Vector2();

	// Update is called once per frame
	void Update () {
		
	}

	void Start() {
		StartCoroutine(getCards()); 
		StartCoroutine(RetrieveDecks()); 
	}
	
	IEnumerator RetrieveDecks() {
		myDecks = new List<Deck> ();
		myDecksPanel = GameObject.Find("MyDecks");

		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		
		WWW w = new WWW(URLGetDecks, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			print(w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			//print(w.text); 											// donne le retour
			
			string[] decksInformation = w.text.Split('\n'); 
			string[] deckInformation;
			myDecksGuiStyle = new GUIStyle[decksInformation.Length - 1];

			for(int i = 0 ; i < decksInformation.Length - 1 ; i++) 		// On boucle sur les attributs d'un deck
			{
				myDecksGuiStyle[i] = new GUIStyle();
				myDecksGuiStyle[i].margin=new RectOffset(0,0,0,0);
				myDecksGuiStyle[i].alignment= TextAnchor.MiddleCenter;
				myDecksGuiStyle[i].fontSize=14;
				if (i>0){
					myDecksGuiStyle[i].normal.background=backButton;
				}
				else{
					myDecksGuiStyle[i].normal.background=backActivatedButton;
				}

				deckInformation = decksInformation[i].Split('\\'); 	// On découpe les attributs du deck qu'on place dans un tableau
				
				int deckId = System.Convert.ToInt32(deckInformation[0]); 	// Ici, on récupère l'id en base
				string deckName = deckInformation[1]; 						// Le nom du deck
				int deckNbC = System.Convert.ToInt32(deckInformation[2]);	// le nombre de cartes
				myDecks.Add(new Deck(deckId, deckName, deckNbC));

				if (i==0){
					chosenIdDeck = deckId;
				}
			}
		}
		isDeckEnabled = true ;
	}
	
	void displayDeck(){
	
	}

	void OnGUI()
	{
		if (isDeckEnabled) {


			GUILayout.BeginArea(new Rect(0,0.1f*Screen.height+5,Screen.width * 0.2f,0.9f*Screen.height));
			{
				GUILayout.BeginVertical(); // also can put width in here
				{
					for(int i = 0 ; i < myDecks.Count ; i++){	
						if (GUILayout.Button(myDecks[i].Name+" ("+myDecks[i].NbCards+")",myDecksGuiStyle[i], GUILayout.Height(50))) // also can put width here
						{
							if (chosenDeck != i){
								myDecksGuiStyle[chosenDeck].normal.background=backButton;
								chosenDeck = i;
								myDecksGuiStyle[i].normal.background=backActivatedButton;
								chosenIdDeck=myDecks[i].Id;
							}
							StartCoroutine(RetrieveCardsFromDeck());
						}
					}
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndArea();
		}
	}

	private IEnumerator RetrieveCardsFromDeck()
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_deck", this.chosenIdDeck); // id du deck courant
		
		WWW w = new WWW(URLGetCardsByDeck, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			print(w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			print(w.text); 											// donne le retour
			
			string[] cardDeckEntries = w.text.Split('\n'); 				// Chaque ligne du serveur correspond à une carte
			
			for(int i = 1 ; i < cardDeckEntries.Length - 1 ; i++) 		// On boucle sur les attributs d'une carte
			{
				GameObject card = GameObject.Find ("Card"+i);
				int j = 0 ;
				int id = -1 ;
				while(id==-1){

					if (cards[j].Id==System.Convert.ToInt32(cardDeckEntries[i])){
						id = j;
					}
					j++;
				}
				GameCard gCard = card.GetComponent<GameCard>();		// On récupère l'instance de la GameCard
				gCard.Card = cards[id];											// On défini la carte qu'elle représente
				gCard.ShowFace();	
			}
		}
	}

	private IEnumerator getCards() {
		string[] cardsIDS = null;

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		
		WWW w = new WWW(URLGetMyCardsPage, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
		{
			print (w.error); 										// donne l'erreur eventuelle
		} else {
			//print (w.text);
			
			// cardsIDS = w.text.Split('\n');
			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			cardsIDS=data[2].Split ('\n');
			this.skillsList = data[1].Split('\n');
			this.cardTypeList = data[0].Split('\n');
		}
		
		this.cards = new List<Card>();
		int j = 0;
		this.cardsToBeFiltered = new List<int>();
		
		for(int i = 1 ; i < cardsIDS.Length-1 ; i++)         			// On parcourt toutes les cartes de l'utilisateur
		{
			string[] cardInformation=cardsIDS[i].Split(new string[] { "\\" }, System.StringSplitOptions.None);
			//print (cardInformation[1]);
			if(i!=1){
				if(!cardsIDS[i].StartsWith(cardsIDS[i-1].Substring(0, 4))){
					this.cards.Add(new Card(System.Convert.ToInt32(cardInformation[0]), // id
					                        cardInformation[1], // title
					                        System.Convert.ToInt32(cardInformation[2]), // life
					                        System.Convert.ToInt32(cardInformation[3]), // attack
					                        System.Convert.ToInt32(cardInformation[4]), // speed
					                        System.Convert.ToInt32(cardInformation[5]), // move
					                        System.Convert.ToInt32(cardInformation[6]), // artindex
					                        System.Convert.ToInt32(cardInformation[7]), // idclass
					                        this.cardTypeList[System.Convert.ToInt32(cardInformation[7])], // titleclass
					                        System.Convert.ToInt32(cardInformation[8]), // lifelevel
					                        System.Convert.ToInt32(cardInformation[9]), // movelevel
					                        System.Convert.ToInt32(cardInformation[10]), // speedlevel
					                        System.Convert.ToInt32(cardInformation[11]))); // attacklevel;
					this.cards[j].Skills = new List<Skill>();
					this.cardsToBeFiltered.Add(j);
					j++;
				}
			}
			else {
				this.cards.Add(new Card(System.Convert.ToInt32(cardInformation[0]), // id
				                        cardInformation[1], // title
				                        System.Convert.ToInt32(cardInformation[2]), // life
				                        System.Convert.ToInt32(cardInformation[3]), // attack
				                        System.Convert.ToInt32(cardInformation[4]), // speed
				                        System.Convert.ToInt32(cardInformation[5]), // move
				                        System.Convert.ToInt32(cardInformation[6]), // artindex
				                        System.Convert.ToInt32(cardInformation[7]), // idclass
				                        this.cardTypeList[System.Convert.ToInt32(cardInformation[7])], // titleclass
				                        System.Convert.ToInt32(cardInformation[8]), // lifelevel
				                        System.Convert.ToInt32(cardInformation[9]), // movelevel
				                        System.Convert.ToInt32(cardInformation[10]), // speedlevel
				                        System.Convert.ToInt32(cardInformation[11]))); // attacklevel
				this.cards[j].Skills = new List<Skill>();
				this.cardsToBeFiltered.Add(j);
				j++ ;
			}
			if (cardInformation[12].Length>0){
				this.cards[j-1].Skills.Add(new Skill (skillsList[System.Convert.ToInt32(cardInformation[12])], //skillName
				                                      System.Convert.ToInt32(cardInformation[12]), // idskill
				                                      System.Convert.ToInt32(cardInformation[13]), // isactivated
				                                      System.Convert.ToInt32(cardInformation[14]), // level
				                                      System.Convert.ToInt32(cardInformation[15]), // power
				                                      System.Convert.ToInt32(cardInformation[16]))); // costmana
			}
		}
		this.cardsToBeDisplayed = new bool[j];
		for (int i = 0; i < j; i++){
			this.cardsToBeDisplayed [i] = true;
		}
	}
}
