using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MyCardsScript : MonoBehaviour
{
	public GameObject CardObject;														// Prefab de la carte vide
	public string URL_get_cards_by_user; 												// L'url de récupération des cartes du joueur sur le serveur
	public string URL_get_cards_type_list;												// L'url de récupération des cardtype du joueur sur le serveur
	public string URL_get_skills_list;													// L'url de récupération des compétences du joueur sur le serveur
	public string URL_get_cards_informations_by_card;									// L'url de récupération des informations de base d'une carte sur le serveur
	public string URL_get_cards_skills_by_card;											// L'url de récupération des compétences d'une carte sur le serveur
	public string cardId;																// Id d'une carte, utilisée pour les requêtes sur le serveur 
	public string[] cards_type_list_entries;											// Liste des cardtype envoyée par le serveur 
	public string[] cards_type_list_data;												// Variable pour stocker 1 ligne de la table CardType
	public string[] cards_information_by_card_entries;									// Liste des informations d'une carte envoyée par le serveur
	public string[] skills_list_entries;												// Liste des compétences envoyée par le serveur
	public string[] skills_list_data;													// Variable pour stocker 1 ligne de la table Skills
	public string[] cards_by_user_entries;												// Liste des compétences d'une carte envoyée par le serveur
	public string[] cards_skills_by_card_entries;

	// ----------------  Variables pour les filtres et l'autocomplétion

	public GameObject[] filtersCardsType = new GameObject[9];							// Variables correspondantes aux filtres CardsType
	public List<string> filters = new List<string> ();									// Liste contenant à un instant t les valeurs des filtres CardsType
	public string skillToFilter;														// Variable contenant le nom de la compétence sur laquelle filtrer
	public List<string> skills = new List<string>();									// Liste contenant à un instant t les compétences proposées par l'autocomplétion
	public GameObject AutoCompletion = null;											// Variable correspondante au panneau d'Autocomplétion
	public GameObject filterSkills = null;												// Variable correspondante au champs de saisie de la compétence
	public GameObject button = null;													// Variable correspondante à la proposition d'autocomplétion cliquée par l'utilisateur
	public GameObject[] Proposition = new GameObject[4];								// Variables correspondantes aux propositions issues de l'autocomplétion
	

	void Start() 
	{

		for(int i=0;i<4;i++){															// On masque les éléments de l'auto complétion
			Proposition[i]= GameObject.Find ("Proposition"+i);							// On masque les panneaux correspondants aux propositions
			Proposition[i].SetActive(false);							
		}

		AutoCompletion = GameObject.Find("AutoCompletion");								// On masque le panneau d'autocomplétion
		AutoCompletion.SetActive(false);
	}


	void Awake()
	{
		StartCoroutine(Retrieve_skills_list());											// Appel de la coroutine pour récupérer la liste des compétences
		StartCoroutine(Retrieve_cards_type_list());										// Appel de la coroutine pour récupérer la liste des cardtype
		StartCoroutine(Retrieve_cards_by_user()); 										// Appel de la coroutine pour récupérer les cartes de l'utilisateur sur le serveur distant	
		
	}

	// ----------------  Fonctions pour les filtres CardType


	public void initializeFilters(){													// Initialisation des filtres à partir de la liste récupérée sur le serveur

		for (int i=0; i<10; i++) {														// Boucle sur l'ensemble des filtres
			
			
			string[] cards_type_list_data = cards_type_list_entries[i].Split('\\');		// Récupération du nom du cardTYpe à partir de son ID
			filtersCardsType[i] = GameObject.Find("cardsTypeLabel" + i);				// Modification du filtre
			var textFilterCardsType = filtersCardsType[i].GetComponent<Text> ();
			textFilterCardsType.text = cards_type_list_data[1];
		}
	}


	public void changeOnFilterCardsType(string filterNumber) {							// Fonction de mise à jour de la liste des cardType sur lesquelles filtrer

		bool has = filters.Any(e => e == filterNumber);									// Après avoir détecté une mise à jour sur le filtre, on met à jour la liste
		if(!has){																		// Soit on supprime si l'élément existe déjà, soit on l'ajoute si il n'existe pas
			filters.Add (filterNumber);
		}										
		else {
			filters.Remove(filterNumber);
		}
		cleanScreen();																	// Comme les filtres ont été mis à jour, on relance l'affichage des cartes
		StartCoroutine(calcul ());														// On lance le calcul après avoir supprimé les cartes de l'écran							
	}	



	// ----------------  Fonctions pour effacer toutes les cartes
		

	void cleanScreen(){
		for (int i = 0; i < cards_by_user_entries.Length - 1; i++) {         			// Il ne peut pas y avoir plus de cartes affichés sur le nombre de cartes possédées par l'utilisateur

			Destroy(GameObject.Find("Card" + i + ""));	
			}																			// Boucle sur le nombre de cartes de l'utilisateur pour supprimer les cartes affichées
		}


	// ----------------  Fonction d'affichage des cartes

	IEnumerator calcul()																// Fonction qui permet d'afficher toutes les cartes à l'initialisation et en fonction des filtres actifs
	{

		skills.Clear();																	// Remise à zéro de la liste des compétences, car on la met à jour en fonction des cartes possédées par l'utilisateur

		int x=0;												  						// initialisation des coordonnées des cartes
		float y=2.5f;
		int k = 0;
		for(int i = 0 ; i < cards_by_user_entries.Length - 1 ; i++)         			// On parcourt toutes les cartes de l'utilisateur
		{
			cardId = cards_by_user_entries[i];											// On stocke dans une variable l'ID de la carte analysée

			yield return StartCoroutine(Retrieve_cards_information_by_card());			// On récupère les informations de la carte
			yield return StartCoroutine(Retrieve_cards_skills_by_card());				// On récupère les compétences de la carte


			bool skillsFilterSucess=false;												// On crée une variable pour détecter si la carte correspond aux critères du filtre compétence


			for(int j = 0 ; (j < cards_skills_by_card_entries.Length-1)
			    && cards_skills_by_card_entries[j] !=""  ; j++) {        				// Boucle sur l'ensemble des compétences de la carte

			
			int idSkill = System.Convert.ToInt32(cards_skills_by_card_entries[j]);		// On compare une à une les compétences de la carte pour vérifier si l'une d'entre elle correspond à la compétence sur laquelle filtrer
			string[] skills_list_data = skills_list_entries[idSkill].Split('\\');
			
				if(skillToFilter==skills_list_data[1].ToLower ()){						// Si c'est le cas on passe la variable de succès à True
				skillsFilterSucess=true;
			}
			
				bool has = skills.Any(e => e == skills_list_data[1]);					// On met à jour la liste des compétences disponibles
							
			if (!has){
					skills.Add (skills_list_data[1]);									// On teste si la compétence est déjà dans la liste et on ajuste le cas échéant
					}
			}

			bool cardTypeFilterSucess = true;											// On créé une variable pour détecter si la carte correspond aux critères des filtres cardType

			string[] cards_information_by_card_data = 
				cards_information_by_card_entries[0].Split('\\');						// On récupère le cardType de la carte

			if (filters.Any()) {
			cardTypeFilterSucess = 
					filters.Any(e => e == cards_information_by_card_data[4]);			// On vérifie si le cardType de la carte est dans la liste des filtres actifs
			}

			bool toFilter = false;														// Si ce n'est pas le cas la carte ne peut être filtré


			if((cardTypeFilterSucess || !filters.Any()) &&								// Tests pour savoir si la carte correspond aux critères compétences et cardType
			   (skillsFilterSucess || skillToFilter=="") ){								// Si tous les filtres sont désactivés on peut aussi afficher la carte
				toFilter=true;
			}


			if (toFilter)																// La carte peut être affichée
			{

				x= k % 5;																// On calcule les coordonnées de la prochaine carte
				if (k % 5 == 0 && k>0) 
				{
					y=y-2.5f;
					x=0;
				}
																						// On récupère les informations de la carte

				int cardArt = System.Convert.ToInt32(cards_information_by_card_data[1]); 	
				string cardTitle = cards_information_by_card_data[2]; 					
				int cardLife = System.Convert.ToInt32(cards_information_by_card_data[3]);	
				
				Card card = new Card(cardTitle, cardLife, cardArt);						// On fabrique et on affiche la carte
				
				GameObject instance = 
					Instantiate(CardObject) as GameObject;            					// On charge une instance du prefab Card
				instance.transform.localScale = 
					new Vector3(0.15f, 0.02f, 0.20f);               					 // On change ses attributs d'échelle ...                                                                    
				instance.transform.localPosition = 
					new Vector3(-5 + (2 * x), y, 0);                					// ..., de positionnement ...
				instance.GetComponent<GameCard>().Card = card;        					// ... et la carte qu'elle représente
				instance.GetComponent<GameCard>().ShowFace();        					// On affiche la carte
				instance.gameObject.name = "Card" + k + "";								// On nomme la carte
				k++;
			}
		}

	}


	// ----------------  Fonctions pour l'auto complétion
	
	
	public void autoCompletion(string skill) {											// Script qui s'exécute lorsque l'on modifie l'interface de saisie d'une compétence
		
		skill = skill.ToLower ();														// On convertit en minuscule le texte saisi											
		
		AutoCompletion.SetActive(false);												// On masque le panneau de l'auto complétion
		if (skill.Length > 0) {
			List<string> found = skills.FindAll
				( w => w.ToLowerInvariant().StartsWith(skill) );						// On cherche une correspondance dans la liste des compétences possédées par l'utilisateur
			if (found.Count > 0 && found[0] != skill){
				AutoCompletion.SetActive(true);											// Si une correspondance est trouvée on affiche le panneau
				for(int i=0;i<4;i++){													// On met en forme les propositions d'auto complétion
					if (i<=found.Count-1){
						Proposition[i].SetActive(true);
						var texteProposition = Proposition[i].GetComponent<Text>();
						texteProposition.text = found[i];
					}
					else{
						Proposition[i].SetActive(false);								// On masque les propositions selon le nombre de correspondances trouvées
					}
				}
				var tailleAutoCompletion = 												// On modifie le panneau d'auto complétion
					AutoCompletion.GetComponent<RectTransform>();
				tailleAutoCompletion.sizeDelta =										// La taille
					new Vector2(160,(found.Count)*25);
				tailleAutoCompletion.transform.localPosition =							// et la position
					new Vector2(0,-15-(found.Count)*12.5f);
			}
		}
		
	}
	
	
	public void autoCompletionClick(string buttonName)									// Fonction qui s'exécute lorsque l'on clique sur une proposition
	{
		StopAllCoroutines ();															// On stoppe les autres fonctions (en fait le déclenchement de cette fonciton, exécute la fonction endTyping (cette instruction sert à la stopper)
		cleanScreen();																	// On efface les cartes affichées
		filterSkills = GameObject.Find("filterSkills");									// On récupère le texte cliqué par l'utilisateur
		var skill = filterSkills.GetComponent<InputField> ();
		button = GameObject.Find(buttonName);											// On met à jour le champs de saisie
		var texteProposition = button.GetComponent<Text> ();
		skill.text = texteProposition.text;
		endTyping ();																	// On lance endTyping
	}
	
	
	public void endTyping()																// Fonction qui s'exécute lorsque l'on termine la saisie d'une compétence (ou lorsque l'on clique sur une proposition)
	{
		filterSkills = GameObject.Find("filterSkills");									// On enregistre dans une variable la compétence
		var skill = filterSkills.GetComponent<InputField> ();							
		skillToFilter = skill.text.ToLower ();											// On convertit en minuscule la variable
		cleanScreen();																	// On efface les cartes affichées
		StartCoroutine(calcul ());														// On lance l'affichage des cartes
	}



	// ----------------  Récupération de la liste des cartes de l'utilisateur 
	
	IEnumerator Retrieve_cards_by_user() {
		WWWForm form = new WWWForm(); 													// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 							// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 										// Pseudo de l'utilisateur connecté
		
		WWW w = new WWW(URL_get_cards_by_user, form); 									// On envoie le formulaire à l'url sur le serveur 
		yield return w; 																// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			print(w.error); 															// donne l'erreur eventuelle
		} 
		else 
		{
			print(w.text); 																// donne le retour
			cards_by_user_entries = w.text.Split('\n'); 								// Chaque ligne du serveur correspond à une carte
			StartCoroutine(calcul ());													// On lance l'affichage des cartes
			
		}
	}
	
	// ----------------  Récupération de la liste des CardType 
	
	IEnumerator Retrieve_cards_type_list() {
		WWWForm form = new WWWForm(); 													// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 							// hashcode de sécurité, doit etre identique à celui sur le serveur
		
		WWW w = new WWW(URL_get_cards_type_list, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 																// On attend la rponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			print(w.error); 															// donne l'erreur eventuelle
		} 
		else 
		{
			print(w.text); 																// donne le retour
			cards_type_list_entries = w.text.Split('\n'); 								// Chaque ligne du serveur correspond à une carte
			
		}
		initializeFilters();															// On lance l'initialisation des filtres
	}
	
	// ----------------  Récupération de la liste des compétences
	
	IEnumerator Retrieve_skills_list() {
		WWWForm form = new WWWForm(); 													// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 							// hashcode de sécurité, doit etre identique à celui sur le serveur
		
		WWW w = new WWW(URL_get_skills_list, form); 									// On envoie le formulaire à l'url sur le serveur 
		yield return w; 																// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			print(w.error); 															// donne l'erreur eventuelle
		} 
		else 
		{
			print(w.text); 																// donne le retour
			skills_list_entries = w.text.Split('\n'); 									// Chaque ligne du serveur correspond à une carte
			
		}
	}

	// ----------------  Récupération des informations d'une carte

	IEnumerator Retrieve_cards_information_by_card() {
		WWWForm form_informations = new WWWForm(); 										// Création de la connexion
		form_informations.AddField("myform_hash", ApplicationModel.hash); 				// hashcode de sécurité, doit etre identique à celui sur le serveur
		form_informations.AddField("myform_idcard", cardId);
		
		WWW w_informations = 
			new WWW(URL_get_cards_informations_by_card, form_informations); 			// On envoie le formulaire à l'url sur le serveur 
		yield return w_informations; 													// On attend la réponse du serveur, le jeu est donc en attente
		if (w_informations.error != null) 
		{
			print(w_informations.error); 												// donne l'erreur eventuelle
		} 
		else 
		{
			print(w_informations.text); 												// donne le retour
			cards_information_by_card_entries = w_informations.text.Split('\n'); 		// Chaque ligne du serveur correspond à une carte												// Fonction d'affichage des cartes
		}
	}

	// ----------------  Récupération des compétences d'une carte
	
	IEnumerator Retrieve_cards_skills_by_card() {
		WWWForm form_skills = new WWWForm(); 											// Création de la connexion
		form_skills.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form_skills.AddField("myform_idcard", cardId);
		
		WWW w_skills = new WWW(URL_get_cards_skills_by_card, form_skills); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w_skills; 															// On attend la réponse du serveur, le jeu est donc en attente
		if (w_skills.error != null) 
		{
			print(w_skills.error); 														// donne l'erreur eventuelle
		} 
		else 
		{
			print(w_skills.text); 														// donne le retour
			cards_skills_by_card_entries = w_skills.text.Split('\n'); 					// Chaque ligne du serveur correspond à une carte												// Fonction d'affichage des cartes
			
		}

	}

	

}