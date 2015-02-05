using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MyCardsScript : MonoBehaviour
{
	public GameObject CardObject;												 
	string[] skillsList;
	string[] cardTypeList;
	Card[] cards ;
	public IList<int> cardsToBeDisplayed ;
	GameObject[] filtres ;
	private bool[] togglesCurrentStates;
	private string valueSkill ;
	int counterFilters ;


	bool dataIsLoaded = false ;

	private IEnumerator Start() 
	{
		dataIsLoaded = false;
		yield return StartCoroutine("getSkillsList");
		yield return StartCoroutine("getCardTypeList");

		//préparation du tableau de filtres
		togglesCurrentStates = new bool[this.cardTypeList.Length - 1];
		counterFilters = 0;
		for (int i=0; i<this.cardTypeList.Length-1; i++) {
			togglesCurrentStates[i] = false ;
		}
		valueSkill = "";
		
		yield return StartCoroutine ("getCards");
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
					print ("Filtre sur :" + i);
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
				if (valueSkill.Length>1){
					IList<string> matchValues = displaySkills (s);
					for (int j=0; j<matchValues.Count(); j++) {

						GUI.Label (new Rect (cadreFiltres.transform.position.x-cadreFiltres.GetComponent<RectTransform>().rect.width/2+10, cadreFiltres.transform.position.y+ 80 + i * 20 + j*20-cadreFiltres.GetComponent<RectTransform>().rect.height/2, 150, 20), matchValues[j]);
					}
				}
				valueSkill = s ;
			}
		}
	}

	private IEnumerator getSkillsList(){
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		
		WWW w = new WWW("http://54.77.118.214/GarrukServer/get_skills_list.php", form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
		{
			print (w.error); 										// donne l'erreur eventuelle
		} else {
			//			print (w.text);
			this.skillsList = w.text.Split('\n');
		}
	}

	private IEnumerator getCardTypeList(){
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		
		WWW w = new WWW("http://54.77.118.214/GarrukServer/get_cards_type_list.php", form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
		{
			print (w.error); 										// donne l'erreur eventuelle
		} else {
			//			print (w.text);
			this.cardTypeList = w.text.Split('\n');
		}
	}

	private IEnumerator getCards() {
		string[] cardsIDS = null;
		string[] cardInformation = null;

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		
		WWW w = new WWW("http://54.77.118.214/GarrukServer/get_cards_by_user.php", form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
		{
			print (w.error); 										// donne l'erreur eventuelle
		} else {
			//print (w.text);
			cardsIDS = w.text.Split('\n');
		}
		
		this.cards = new Card[cardsIDS.Length];
		this.cardsToBeDisplayed = new List<int>();

		for(int i = 0 ; i < cardsIDS.Length-1 ; i++)         			// On parcourt toutes les cartes de l'utilisateur
		{
			form = new WWWForm(); 											// Création de la connexion
			form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
			form.AddField("myform_nick", ApplicationModel.username);
			form.AddField ("myform_idcard", cardsIDS[i]);
			
			w = new WWW("http://54.77.118.214/GarrukServer/get_cards_informations_by_card.php", form); 				// On envoie le formulaire à l'url sur le serveur 
			yield return w;
			if (w.error != null) 
			{
				print (w.error); 										// donne l'erreur eventuelle
			} else {
//				print (w.text);
				cardInformation = w.text.Split('\\');
			}
			this.cards[i] = new Card(cardInformation[2], System.Convert.ToInt32(cardInformation[3]), System.Convert.ToInt32(cardInformation[1]),System.Convert.ToInt32(cardInformation[4]));	

			form = new WWWForm(); 											// Création de la connexion
			form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
			form.AddField("myform_nick", ApplicationModel.username);
			form.AddField ("myform_idcard", cardsIDS[i]);
			
			w = new WWW("http://54.77.118.214/GarrukServer/get_cards_skills_by_card.php", form); 				// On envoie le formulaire à l'url sur le serveur 
			yield return w;
			if (w.error != null) 
			{
				print (w.error); 										// donne l'erreur eventuelle
			} else {
				//print (w.text);
				cardInformation = w.text.Split('\\');
			}
			this.cards[i].Skills = new List<Skill>();
			foreach(string ci in cardInformation)
			{
				this.cards[i].Skills.Add(new Skill(ci));
			}
			this.cardsToBeDisplayed.Add(i);
		}
	}

	public void displayCards()																// Fonction qui permet d'afficher toutes les cartes à l'initialisation et en fonction des filtres actifs
	{
		int x=0;												  						// initialisation des coordonnées des cartes
		float y=2.5f;
		int k = 0;

		for(int i = 0 ; i < this.cardsToBeDisplayed.Count ; i++)         			// On parcourt toutes les cartes de l'utilisateur
		{
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
			instance.GetComponent<GameCard>().Card = cards[cardsToBeDisplayed[i]];        					// ... et la carte qu'elle représente
			instance.GetComponent<GameCard>().ShowFace();        					// On affiche la carte
			instance.gameObject.name = "Card" + k + "";	
			k++;
		}

	}

	public void addCardTypes(int a) {			
		cleanScreen();
		if (counterFilters==0){
			this.clearCards();
		}
		for (int i = 0; i < cards.Length - 1; i++) {
			if (cards[i].IdClass==a){
				cardsToBeDisplayed.Add(i);

			}
		}

		displayCards ();							
	}	

	public void removeCardTypes(int a) {
		cleanScreen();
		if (counterFilters == 1) {
			this.clearCards ();
			for (int j = 0; j < cards.Length - 1; j++) {
				cardsToBeDisplayed.Add (j);
			}
		}
		else {
			for (int j = 0; j < cards.Length - 1; j++) {
				if (cards [j].IdClass == a) {
					cardsToBeDisplayed.Remove (j);
				}
			}
		}
		displayCards ();												// On lance le calcul après avoir supprimé les cartes de l'écran							
	}

	public void clearCards() {
		cardsToBeDisplayed.Clear ();
	}

	public void cleanScreen(){
		for (int i = 0; i < cardsToBeDisplayed.Count(); i++) {         			// Il ne peut pas y avoir plus de cartes affichés sur le nombre de cartes possédées par l'utilisateur
			Destroy(GameObject.Find("Card" + i + ""));	
		}	
	}

	public IList<string> displaySkills(string a){
		IList<string> matchValues = new List<string> ();
		for (int i = 0; i < skillsList.Length; i++) {  
			if(skillsList[i].ToLower().Contains(a)){
				print ("Match avec "+skillsList[i]);
				matchValues.Add(skillsList[i]);
			}
		}
		return matchValues;
	}

	void testType(){
//		bool cardTypeFilterSucess = true;
//		bool toFilter;
//		bool skillsFilterSucess = true;
//		if (filters.Any ()) {
//						cardTypeFilterSucess = filters.Any (e => e == cards_information_by_card_data [4]);			// On vérifie si le cardType de la carte est dans la liste des filtres actifs
//	}

//		if((cardTypeFilterSucess || !filters.Any()) &&								// Tests pour savoir si la carte correspond aux critères compétences et cardType
//		   (skillsFilterSucess || skillToFilter=="") ){								// Si tous les filtres sont désactivés on peut aussi afficher la carte
			//toFilter=true;
//		}
		}


	// ----------------  Fonction d'affichage des cartes




	// ----------------  Fonctions pour l'auto complétion
	
	
	public void autoCompletion(string skill) {											// Script qui s'exécute lorsque l'on modifie l'interface de saisie d'une compétence
		
		skill = skill.ToLower ();														// On convertit en minuscule le texte saisi											
		
//		AutoCompletion.SetActive(false);												// On masque le panneau de l'auto complétion
//		if (skill.Length > 0) {
//			List<string> found = skills.FindAll
//				( w => w.ToLowerInvariant().StartsWith(skill) );						// On cherche une correspondance dans la liste des compétences possédées par l'utilisateur
//			if (found.Count > 0 && found[0] != skill){
//				AutoCompletion.SetActive(true);											// Si une correspondance est trouvée on affiche le panneau
//				for(int i=0;i<4;i++){													// On met en forme les propositions d'auto complétion
//					if (i<=found.Count-1){
//						Proposition[i].SetActive(true);
//						var texteProposition = Proposition[i].GetComponent<Text>();
//						texteProposition.text = found[i];
//					}
//					else{
//						Proposition[i].SetActive(false);								// On masque les propositions selon le nombre de correspondances trouvées
//					}
//				}
//				var tailleAutoCompletion = 												// On modifie le panneau d'auto complétion
//					AutoCompletion.GetComponent<RectTransform>();
//				tailleAutoCompletion.sizeDelta =										// La taille
//					new Vector2(160,(found.Count)*25);
//				tailleAutoCompletion.transform.localPosition =							// et la position
//					new Vector2(0,-15-(found.Count)*12.5f);
//			}
//		}
		
	}
	
	
	public void autoCompletionClick(string buttonName)									// Fonction qui s'exécute lorsque l'on clique sur une proposition
	{
//		StopAllCoroutines ();															// On stoppe les autres fonctions (en fait le déclenchement de cette fonciton, exécute la fonction endTyping (cette instruction sert à la stopper)
//		cleanScreen();																	// On efface les cartes affichées
//		filterSkills = GameObject.Find("filterSkills");									// On récupère le texte cliqué par l'utilisateur
//		var skill = filterSkills.GetComponent<InputField> ();
//		button = GameObject.Find(buttonName);											// On met à jour le champs de saisie
//		var texteProposition = button.GetComponent<Text> ();
//		skill.text = texteProposition.text;
//		endTyping ();																	// On lance endTyping
	}
	
	
	public void endTyping()																// Fonction qui s'exécute lorsque l'on termine la saisie d'une compétence (ou lorsque l'on clique sur une proposition)
	{
//		filterSkills = GameObject.Find("filterSkills");									// On enregistre dans une variable la compétence
//		var skill = filterSkills.GetComponent<InputField> ();							
//		skillToFilter = skill.text.ToLower ();											// On convertit en minuscule la variable
//		cleanScreen();																	// On efface les cartes affichées
//		//StartCoroutine(calcul ());														// On lance l'affichage des cartes
	}
}