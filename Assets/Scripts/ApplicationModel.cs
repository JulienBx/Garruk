using UnityEngine;

/**
 ** Classe permettant de stocker certaines informations lors des chargements de niveaux
*/
public class ApplicationModel : MonoBehaviour
{
	static public string host = "http://54.77.118.214/GarrukServer/";			// Adresse du serveur
	static public string username = "";								// Pseudo de l'utilisateur connecté
	static public string hash = "J8xy9Uz4"; 						// Clé secrète
	static public Deck selectedDeck;								// Deck à afficher
}