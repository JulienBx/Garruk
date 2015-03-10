using UnityEngine;

/**
 ** Classe permettant de stocker certaines informations lors des chargements de niveaux
*/
public class ApplicationModel : MonoBehaviour
{
	static public string host = "http://54.77.118.214/GarrukServer/";	// Adresse du serveur
	static public string dev = "http://localhost/GarrukServer/";		// Adresse du serveur
	static public string username = "julien";							// Pseudo de l'utilisateur connecté
	static public string hash = "J8xy9Uz4"; 						// Clé secrète
	static public Deck selectedDeck;								// Deck à afficher
	static public string photonSettings = "0.2";                    // identifiant utilisé par Photon A mettre à jour à chaque nouvelle version
	static public string profileChosen = "";
	static public bool toDeconnect = false;
}