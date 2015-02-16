using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class MyCardsScript : MonoBehaviour
{


	public GameObject CardObject;	
	List<string> skillsList = new List<string>();
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

	GameObject hoveredCard = null;
	GameObject oldHoveredCard = null;

	float minLifeVal = 0;
	float minLifeLimit = 0;
	float maxLifeVal = 0;
	float maxLifeLimit = 0;
	float oldMinLifeVal = 0;
	float oldMaxLifeVal = 0;

	float minSpeedVal = 0;
	float minSpeedLimit = 0;
	float maxSpeedVal = 0;
	float maxSpeedLimit = 0;
	float oldMinSpeedVal = 0;
	float oldMaxSpeedVal = 0;

	float minAttackVal = 0;
	float minAttackLimit = 0;
	float maxAttackVal =0;
	float maxAttackLimit = 0;
	float oldMinAttackVal = 0;
	float oldMaxAttackVal = 0;

	float minMoveVal = 0;
	float minMoveLimit = 0;
	float maxMoveVal = 0;
	float maxMoveLimit = 0;
	float oldMinMoveVal = 0;
	float oldMaxMoveVal = 0;

	bool isBeingDragged=false;
	Ray ray;
	RaycastHit hit;

	
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


		maxAttackVal = maxAttackLimit;
		oldMaxAttackVal = maxAttackLimit;
		minAttackVal = minAttackLimit;
		oldMinAttackVal = minAttackLimit;

		maxSpeedVal = maxSpeedLimit;
		oldMaxSpeedVal = maxSpeedLimit;
		minSpeedVal = minSpeedLimit;
		oldMinSpeedVal = minSpeedLimit;

		maxMoveVal = maxMoveLimit;
		oldMaxMoveVal = maxMoveLimit;
		minMoveVal = minMoveLimit;
		oldMinLifeVal = minMoveLimit;

		maxLifeVal = maxLifeLimit;
		oldMaxLifeVal = maxLifeLimit;
		minLifeVal = minLifeLimit;
		oldMinLifeVal = minLifeLimit;

		displayCards ();


		dataIsLoaded = true;

	}

	
	void Update() {

		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit))
		{
			if (hit.collider.name.StartsWith("Card")){
			hoveredCard = GameObject.Find(hit.collider.name);
			onHovering(hoveredCard);
			}

			if (oldHoveredCard != hoveredCard){
				if (oldHoveredCard != null)
				endHovering(oldHoveredCard);
			oldHoveredCard = hoveredCard;
			}


		}
		else
		{
			if (oldHoveredCard != null){
			endHovering(oldHoveredCard);
			oldHoveredCard = hoveredCard;
			}
		}
	}

	void OnGUI () {


		if (Input.GetMouseButtonDown(0))
			isBeingDragged = true;
		
		if (Input.GetMouseButtonUp(0) && isBeingDragged )
			isBeingDragged = false;




		GUILayout.BeginArea(new Rect(0.80f*Screen.width,0.05f*Screen.height,Screen.width * 0.18f,0.95f*Screen.height));
		{
			GUILayout.BeginVertical(); // also can put width in here
			{
				
				GUI.skin = null;
				
				if (dataIsLoaded) {
					bool toggle;
					int i;
					string s;
					string[] cardTypeData = new string[this.cardTypeList.Length - 1];
					GameObject cadreFiltres = GameObject.Find ("filterCardsType");
					GUILayout.Label ("FILTRER PAR CLASSE");
					for (i=0; i<this.cardTypeList.Length-1; i++) {		
						cardTypeData = cardTypeList [i].Split ('\\');
						toggle = GUILayout.Toggle (togglesCurrentStates [i], cardTypeData [0]);
						if (toggle != togglesCurrentStates [i]) {
							togglesCurrentStates [i] = toggle;
							if (toggle) {
								this.addCardTypes (i);
								counterFilters++;
							} else {
								this.removeCardTypes (i);
								counterFilters--;
							}
						}
					}
					
					GUILayout.Space(5);
					
					GUILayout.Label ("FILTRER PAR SKILL");
					s = GUILayout.TextField (valueSkill);
					if (s != valueSkill) {
						if (s.Length > 0) {
							StartCoroutine (displaySkills (s));
							valueSkill = s.ToLower ();
						} else {
							StartCoroutine (displaySkills (s));
							valueSkill = "";
						}
						cleanScreen ();
						filterCards ();
						displayCards ();
					}
					GUIStyle myStyle = new GUIStyle ();
					myStyle.normal.textColor = Color.blue;
					myStyle.fontSize = 12;
					myStyle.normal.background = this.textureAutoc;
					for (int j=0; j<matchValues.Count(); j++) {
						//GUI.Button (matchValues[j], myStyle);
						if (GUILayout.Button (matchValues [j], myStyle,GUILayout.Height(20))) {
							valueSkill = matchValues [j].ToLower ();
							StartCoroutine (displaySkills (s));
							cleanScreen ();
							filterCards ();
							displayCards ();	
						}
					}
					
					GUILayout.Space(5);
					
					GUILayout.Label ("PTS DE VIE");
					GUILayout.BeginHorizontal(); // also can put width in here
					{
						GUILayout.Label ("Min :"+ Mathf.Round(minLifeVal).ToString());
						GUILayout.FlexibleSpace();
						GUILayout.Label ("Max :"+ Mathf.Round(maxLifeVal).ToString());
					}
					GUILayout.EndHorizontal();

					#if UNITY_EDITOR
					EditorGUILayout.MinMaxSlider (ref minLifeVal, ref maxLifeVal, minLifeLimit, maxLifeLimit);
					#endif

					minLifeVal = Mathf.Round (minLifeVal);
					maxLifeVal = Mathf.Round (maxLifeVal);
					
					
					GUILayout.Space(5);
					
					GUILayout.Label ("PTS D'ATTAQUE");
					GUILayout.BeginHorizontal(); // also can put width in here
					{
						GUILayout.Label ("Min :"+ Mathf.Round(minAttackVal).ToString());
						GUILayout.FlexibleSpace();
						GUILayout.Label ("Max :"+ Mathf.Round(maxAttackVal).ToString());
					}
					GUILayout.EndHorizontal();

					#if UNITY_EDITOR
					EditorGUILayout.MinMaxSlider (ref minAttackVal, ref maxAttackVal, minAttackLimit, maxAttackLimit);
					#endif

					minAttackVal = Mathf.Round (minAttackVal);
					maxAttackVal = Mathf.Round (maxAttackVal);
					
					GUILayout.Space(5);
					
					GUILayout.Label ("PTS DE VITESSE");
					GUILayout.BeginHorizontal(); // also can put width in here
					{
						GUILayout.Label ("Min :"+ Mathf.Round(minSpeedVal).ToString());
						GUILayout.FlexibleSpace();
						GUILayout.Label ("Max :"+ Mathf.Round(maxSpeedVal).ToString());
					}
					GUILayout.EndHorizontal();
					
					#if UNITY_EDITOR
					EditorGUILayout.MinMaxSlider (ref minSpeedVal, ref maxSpeedVal, minSpeedLimit, maxSpeedLimit);
					#endif

					minSpeedVal = Mathf.Round (minSpeedVal);
					maxSpeedVal = Mathf.Round (maxSpeedVal);
					
					GUILayout.Space(5);
					
					GUILayout.Label ("PTS DE DEPLACEMENT");
					GUILayout.BeginHorizontal(); // also can put width in here
					{
						GUILayout.Label ("Min :"+ minMoveVal.ToString());
						GUILayout.FlexibleSpace();
						GUILayout.Label ("Max :"+ maxMoveVal.ToString());
					}
					GUILayout.EndHorizontal();
					
					#if UNITY_EDITOR
					EditorGUILayout.MinMaxSlider (ref minMoveVal, ref maxMoveVal, minMoveLimit, maxMoveLimit);
					#endif

					minMoveVal = Mathf.Round (minMoveVal);
					maxMoveVal = Mathf.Round (maxMoveVal);
					
					
					if (!isBeingDragged){
						
						if (oldMaxLifeVal != maxLifeVal 
						    ||oldMinLifeVal != minLifeVal 
						    ||oldMaxMoveVal != maxMoveVal
						    ||oldMinMoveVal != minMoveVal
						    ||oldMaxSpeedVal != maxSpeedVal
						    ||oldMinSpeedVal != minSpeedVal
						    ||oldMaxAttackVal != maxAttackVal
						    ||oldMinAttackVal != minAttackVal) {
							
							oldMaxAttackVal = maxAttackVal;
							oldMinAttackVal = minAttackVal;
							oldMaxSpeedVal = maxSpeedVal;
							oldMinSpeedVal = minSpeedVal;
							oldMaxMoveVal = maxMoveVal;
							oldMinLifeVal = minMoveVal;
							oldMaxLifeVal = maxLifeVal;
							oldMinLifeVal = minLifeVal;
							
							cleanScreen();
							filterCardsByPoints();
							displayCards ();
							
						}
						
					}
					
					
				}
			}
			GUILayout.EndVertical();

		}
		GUILayout.EndArea();
	
	}
	
	 
	private IEnumerator getCards() {
	string[] cardsIDS = null;
	string[] skillsIDS = null;
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
			skillsIDS=data[1].Split ('\n');
			//this.skillsList = data[1].Split('\n');
			this.cardTypeList = data[0].Split('\n');

			for (int i=1 ; i< skillsIDS.Length-1;i++){
				string[] skillsInformation=skillsIDS[i].Split(new string[] { "\\" }, System.StringSplitOptions.None);
				skillsList.Add(skillsInformation[0]);
			}
		
		
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
					                        this.cardTypeList.ElementAt(System.Convert.ToInt32(cardInformation[7])), // titleclass
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
				                        this.cardTypeList.ElementAt(System.Convert.ToInt32(cardInformation[7])), // titleclass
				                        System.Convert.ToInt32(cardInformation[8]), // lifelevel
				                        System.Convert.ToInt32(cardInformation[9]), // movelevel
				                        System.Convert.ToInt32(cardInformation[10]), // speedlevel
				                        System.Convert.ToInt32(cardInformation[11]))); // attacklevel
				this.cards[j].Skills = new List<Skill>();
				this.cardsToBeFiltered.Add(j);
				j++ ;
			}
			if (cardInformation[12].Length>0){
				string[] skillsInformation=skillsIDS[System.Convert.ToInt32(cardInformation[12])].Split(new string[] { "\\" }, System.StringSplitOptions.None);

				int skillForce=Mathf.RoundToInt(System.Convert.ToSingle(skillsInformation[3])*System.Convert.ToInt32(cardInformation[15]));
				if (skillForce <=System.Convert.ToInt32(skillsInformation[2]))
					skillForce = System.Convert.ToInt32(skillsInformation[2]);

				string skillDescription= skillsInformation[1];
				skillDescription=skillDescription.Replace(" X"," " + System.Convert.ToString(skillForce));
				skillDescription=skillDescription.Replace(" X%"," " + System.Convert.ToString(skillForce) + "%");


				this.cards[j-1].Skills.Add(new Skill (skillsInformation[0], //skillName
				                                      System.Convert.ToInt32(cardInformation[12]), // idskill
				                                      System.Convert.ToInt32(cardInformation[13]), // isactivated
				                                      System.Convert.ToInt32(cardInformation[14]), // level
				                                      System.Convert.ToInt32(cardInformation[15]), // power
				                                      System.Convert.ToInt32(cardInformation[16]),
				                                      skillDescription, // description du skill
				                                      skillForce)); // force du skill
				                                     
													
			}
		
		
			if (this.cards[j-1].Attack > maxAttackLimit)
				maxAttackLimit = cards[j-1].Attack;
			if (this.cards[j-1].Attack < minAttackLimit)
				minAttackLimit = cards[j-1].Attack;

			if (this.cards[j-1].Speed > maxSpeedLimit)
				maxSpeedLimit = cards[j-1].Speed;
			if (this.cards[j-1].Speed < minSpeedLimit)
				minSpeedLimit = cards[j-1].Speed;

			if (this.cards[j-1].Move > maxMoveLimit)
				maxMoveLimit = cards[j-1].Move;
			if (this.cards[j-1].Move < minMoveLimit)
				minMoveLimit = cards[j-1].Move;

			if (this.cards[j-1].Life > maxLifeLimit)
				maxLifeLimit = cards[j-1].Life;
			if (this.cards[j-1].Life < minLifeLimit)
				minLifeLimit = cards[j-1].Life;

		
		
		
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
				if (cards [cardsToBeFiltered [i]].hasSkill (valueSkill) 
				    && cards [cardsToBeFiltered [i]].Life >= minLifeVal
				    && cards [cardsToBeFiltered [i]].Life <= maxLifeVal) {
					cardsToBeDisplayed [cardsToBeFiltered [i]] = true;
				} 
				else {
					cardsToBeDisplayed [cardsToBeFiltered [i]] = false;
					//print ("Je passe a false " + cardsToBeDisplayed [cardsToBeFiltered [i]]);
				}
			}	
		}
	}




	public void filterCardsByPoints() {	

		for (int i = 0; i < this.cardsToBeFiltered.Count; i++) {
			if (cards [cardsToBeFiltered [i]].Life >= minLifeVal
			    && cards [cardsToBeFiltered [i]].Life <= maxLifeVal
			    && cards [cardsToBeFiltered [i]].Speed >= minSpeedVal
			    && cards [cardsToBeFiltered [i]].Speed <= maxSpeedVal
			    && cards [cardsToBeFiltered [i]].Move >= minMoveVal
			    && cards [cardsToBeFiltered [i]].Move <= maxMoveVal
			    && cards [cardsToBeFiltered [i]].Attack >= minAttackVal
			    && cards [cardsToBeFiltered [i]].Attack <= maxAttackVal) {

				cardsToBeDisplayed [cardsToBeFiltered [i]] = true;
			} 
			else {
				cardsToBeDisplayed [cardsToBeFiltered [i]] = false;
			}
		}	

	}


	
	public void addCardTypes(int a) {			
		cleanScreen ();
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
		cleanScreen ();
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
			for (int i = 0; i < skillsList.Count; i++) {  
				if (skillsList [i].ToLower ().Contains (a)) {
					matchValues.Add (skillsList [i]);
				}
			}
		}
		yield break;
	}


	public void onHovering (GameObject cardName){

		cardName.transform.localScale = new Vector3(0.30f, 0.02f, 0.40f);
		Vector3 cardPosition = cardName.transform.position; 
		cardName.transform.position = new Vector3 (cardPosition.x, cardPosition.y, -1);
		cardName.GetComponent<GameCard> ().setTextResolution (2f);


	}

	public void endHovering (GameObject cardName){
		
		cardName.transform.localScale = new Vector3(0.15f, 0.02f, 0.20f);
		Vector3 cardPosition = cardName.transform.position; 
		cardName.transform.position = new Vector3 (cardPosition.x, cardPosition.y, 0);
		cardName.GetComponent<GameCard> ().setTextResolution (1f);
	}
	


}