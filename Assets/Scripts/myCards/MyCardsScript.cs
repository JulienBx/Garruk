using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class MyCardsScript : MonoBehaviour
{
	public GameObject CardObject;												 
	string[] skillsList;
	string[] cardTypeList;
	public IList<Card> cards ;
	public IList<int> cardsToBeFiltered ;
	public bool[] cardsToBeDisplayed ;
	
	GameObject[] filtres ;
	private bool[] togglesCurrentStates;
	private string valueSkill = null ;
	int counterFilters ;
	IList<string> matchValues;
	public Texture2D textureAutoc ;
	private string filtreAutoC ;
	
	bool dataIsLoaded = false ;
	
	private IEnumerator Start() 
	{
		matchValues = new List<string> ();
		dataIsLoaded = false;
		yield return StartCoroutine ("getCards");
		
		togglesCurrentStates = new bool[this.cardTypeList.Length - 1];
		counterFilters = 0;
		for (int i=0; i<this.cardTypeList.Length-1; i++) {
			togglesCurrentStates[i] = false ;
		}
		valueSkill = "";
		displayCards ();

		dataIsLoaded = true;
	}
	
	void OnGUI () {
		if (dataIsLoaded) {
			bool toggle;
			int i;
			string s;
			string[] cardTypeData = new string[this.cardTypeList.Length-1];
			GameObject cadreFiltres = GameObject.Find("filterCardsType");
			GUI.Label (new Rect (cadreFiltres.transform.position.x-cadreFiltres.GetComponent<RectTransform>().rect.width/2, cadreFiltres.transform.position.y-cadreFiltres.GetComponent<RectTransform>().rect.height/2, 200, 20), "FILTRER PAR CLASSE");
			for (i=0; i<this.cardTypeList.Length-1; i++) {		
				cardTypeData = cardTypeList [i].Split ('\\');
				toggle = GUI.Toggle (new Rect (cadreFiltres.transform.position.x-cadreFiltres.GetComponent<RectTransform>().rect.width/2, cadreFiltres.transform.position.y+ 20 + i * 20-cadreFiltres.GetComponent<RectTransform>().rect.height/2, 150, 20), togglesCurrentStates[i], cardTypeData [1]);
				if (toggle != togglesCurrentStates[i]){
					togglesCurrentStates[i]=toggle ;
					if (toggle){
						this.addCardTypes(i);
						counterFilters++;
					}
					else{
						this.removeCardTypes(i);
						counterFilters--;
					}
				}
			}
			
			GUI.Label (new Rect (cadreFiltres.transform.position.x-cadreFiltres.GetComponent<RectTransform>().rect.width/2, cadreFiltres.transform.position.y+ 40 + i * 20-cadreFiltres.GetComponent<RectTransform>().rect.height/2, 150, 20), "FILTRER PAR SKILL");
			s = GUI.TextField (new Rect (cadreFiltres.transform.position.x-cadreFiltres.GetComponent<RectTransform>().rect.width/2, cadreFiltres.transform.position.y+ 60 + i * 20-cadreFiltres.GetComponent<RectTransform>().rect.height/2, 150, 20), valueSkill);
			if (s!=valueSkill){
				if (s.Length>0){
					StartCoroutine(displaySkills (s));
					valueSkill = s.ToLower () ;
				}
				else{
					StartCoroutine(displaySkills (s));
					valueSkill = "";
				}
				cleanScreen();
				filterCards ();
				displayCards ();
			}
			GUIStyle myStyle = new GUIStyle();
			myStyle.normal.textColor = Color.blue;
			myStyle.fontSize = 12 ;
			myStyle.normal.background = this.textureAutoc ;
			for (int j=0; j<matchValues.Count(); j++) {
				//GUI.Button (new Rect (cadreFiltres.transform.position.x-cadreFiltres.GetComponent<RectTransform>().rect.width/2+10, cadreFiltres.transform.position.y+ 80 + i * 20 + j*10-cadreFiltres.GetComponent<RectTransform>().rect.height/2, 150, 10), matchValues[j], myStyle);
				if(GUI.Button (new Rect (cadreFiltres.transform.position.x-cadreFiltres.GetComponent<RectTransform>().rect.width/2+10, cadreFiltres.transform.position.y+ 80 + i * 20 + j*15-cadreFiltres.GetComponent<RectTransform>().rect.height/2, 150, 14), matchValues[j], myStyle)){
					valueSkill=matchValues[j].ToLower ();
					StartCoroutine(displaySkills (s));
					cleanScreen();
					filterCards ();
					displayCards ();	
				}
			}
		}
	}
	
	private IEnumerator getCards() {
		string[] cardsIDS = null;
		//string[] cardInformation = null;
		
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		
		WWW w = new WWW("http://54.77.118.214/GarrukServer/get_mycardspage_data.php", form); 				// On envoie le formulaire à l'url sur le serveur 
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
					this.cards.Add(new Card(cardInformation[1], System.Convert.ToInt32(cardInformation[2]),System.Convert.ToInt32(cardInformation[7]),System.Convert.ToInt32(cardInformation[9])));	
					this.cards[j].Skills = new List<Skill>();
					this.cardsToBeFiltered.Add(j);
					j++;
				}
			}
			else {
				this.cards.Add(new Card(cardInformation[1], System.Convert.ToInt32(cardInformation[2]),System.Convert.ToInt32(cardInformation[7]),System.Convert.ToInt32(cardInformation[9])));	
				this.cards[j].Skills = new List<Skill>();
				this.cardsToBeFiltered.Add(j);
				j++ ;
			}
			if (cardInformation[10].Length>0){
				//print ("id : "+cardInformation[10]);
				this.cards[j-1].Skills.Add(new Skill (skillsList[System.Convert.ToInt32(cardInformation[10])]));
			}
		}
		this.cardsToBeDisplayed = new bool[j];
		for (int i = 0; i < j; i++){
			this.cardsToBeDisplayed [i] = true;
		}
	}

	public void displayCards()																// Fonction qui permet d'afficher toutes les cartes à l'initialisation et en fonction des filtres actifs
	{
		int x=0;												  						// initialisation des coordonnées des cartes
		float y=2.5f;
		int k = 0;
		
		for(int i = 0 ; i < this.cardsToBeFiltered.Count ; i++)         			// On parcourt toutes les cartes de l'utilisateu
		{
			if (this.cardsToBeDisplayed[this.cardsToBeFiltered[i]]){
				x= k % 5;																// On calcule les coordonnées de la prochaine carte
				if (k % 5 == 0 && k>0) 
				{
					y=y-2.5f;
					x=0;
				}
				// On récupère les informations de la carte
				GameObject instance = Instantiate(CardObject) as GameObject;            					// On charge une instance du prefab Card
				instance.transform.localScale = new Vector3(0.15f, 0.02f, 0.20f);               					 // On change ses attributs d'échelle ...                                                                    
				instance.transform.localPosition = new Vector3(-5 + (2 * x), y, 0);                					// ..., de positionnement ...
				instance.GetComponent<GameCard>().Card = cards[cardsToBeFiltered[i]];        					// ... et la carte qu'elle représente
				instance.GetComponent<GameCard>().ShowFace();        					// On affiche la carte
				instance.gameObject.name = "Card" + k + "";	
				k++;
			}
		}
		
	}
	
	public void filterCards() {			
		if (valueSkill.Length < 1) {
			for (int i = 0; i < this.cardsToBeFiltered.Count; i++) {
				cardsToBeDisplayed [cardsToBeFiltered [i]] = true;
			}
		} 
		else {
			for (int i = 0; i < this.cardsToBeFiltered.Count; i++) {
				if (cards [cardsToBeFiltered [i]].hasSkill (valueSkill)) {
					cardsToBeDisplayed [cardsToBeFiltered [i]] = true;
				} 
				else {
					cardsToBeDisplayed [cardsToBeFiltered [i]] = false;
					//print ("Je passe a false " + cardsToBeDisplayed [cardsToBeFiltered [i]]);
				}
			}	
		}
	}
	
	public void addCardTypes(int a) {			
		cleanScreen();
		if (counterFilters==0){
			this.clearCards();
		}
		for (int i = 0; i < cards.Count; i++) {
			if (cards[i].IdClass==a){
				cardsToBeFiltered.Add(i);
			}
		}
		if (valueSkill.Length>0){
			filterCards ();
		}
		
		displayCards ();							
	}	
	
	public void removeCardTypes(int a) {
		cleanScreen();
		if (counterFilters == 1) {
			this.clearCards ();
			for (int j = 0; j < cards.Count ; j++) {
				cardsToBeFiltered.Add (j);
			}
		}
		else {
			for (int j = 0; j < cards.Count ; j++) {
				if (cards [j].IdClass == a) {
					cardsToBeDisplayed [cardsToBeFiltered [j]] = true;
					cardsToBeFiltered.Remove (j);
				}
			}
		}
		if (valueSkill.Length>0){
			filterCards ();
		}
		displayCards ();												// On lance le calcul après avoir supprimé les cartes de l'écran							
	}
	
	public void clearCards() {
		cardsToBeFiltered.Clear ();
	}
	
	public void cleanScreen(){
		for (int i = 0; i < cardsToBeFiltered.Count(); i++) {         			// Il ne peut pas y avoir plus de cartes affichés sur le nombre de cartes possédées par l'utilisateur
			Destroy(GameObject.Find("Card" + i + ""));	
		}	
	}
	
	private IEnumerator displaySkills(string a){
		if (a == "") {
			this.matchValues = new List<string> ();	
		} 
		else {
			this.matchValues = new List<string> ();
			for (int i = 0; i < skillsList.Length; i++) {  
				if (skillsList [i].ToLower ().Contains (a)) {
					matchValues.Add (skillsList [i]);
				}
			}
		}
		yield break;
	}
}