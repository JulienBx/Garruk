using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.IO;
using System.Text;
/**
 ** Classe permettant de stocker certaines informations lors des chargements de niveaux
*/
public class ApplicationModel
{
	static public Player player;
	static public string host;
	static public string hash;
	static public string photonSettings;                    // identifiant utilisé par Photon A mettre à jour à chaque nouvelle version
	static public int nbCardsByDeck;

    static public int currentGameId;

	static public string myPlayerName; // A voir si on le met pas dans player ?
	static public string hisPlayerName; // A voir si on le met pas dans un objet user ?
	static public int hisPlayerID;
    static public int hisRankingPoints;

	static public float volBackOfficeFx;
	static public float volMusic;

	static public float volMaxBackOfficeFx;
	static public float volMaxMusic;

	static public Deck opponentDeck;
	static public string gameRoomId;

	static public float timeOutDelay;

    static public float timeAppModel ;


    static public Packs packs;
    static public DisplayedProducts products;

    static public CardTypes cardTypes;
    static public SkillTypes skillTypes;
    static public Skills skills;
    static public List<int> xpLevels;


	#if (UNITY_EDITOR)
    static public int[] onlineStatus;
	static public string[] onlineCheck;
	#endif


	//static public bool isFirstPlayer; // A REMPLACER PAR ApplicationModel.player.IsFirstPlayer
	//static public bool launchGameTutorial; // A REMPLACER PAR ApplicationModel.player.ToLaunchGameTutorial
	//static public int gameType; // A REMPLACER PAR ApplicationModel.player.ChosenGameType


	static ApplicationModel()
	{
		host = "https://www.techticalwars.com/"; // PROD
		//host = "http://testing.techticalwars.com/";  // RECETTE
		hash = ApplicationSecurity.hash;
		photonSettings = "0.2";
		volMaxBackOfficeFx=1f;
		volMaxMusic=0.3f;
		nbCardsByDeck=4;
		myPlayerName="";
		hisPlayerName="";
		timeOutDelay=10f;
        currentGameId=-1;
		player=new Player();
		packs=new Packs();
		products=new DisplayedProducts();
        cardTypes=new CardTypes();
        skillTypes=new SkillTypes();
        skills=new Skills();
        xpLevels=new List<int>();
        
		#if (UNITY_EDITOR)
		onlineCheck=new string[3];
		onlineCheck[0]="guillaume";
		onlineCheck[1]="julien";
		onlineCheck[2]="yoann";
		onlineStatus=new int[3];
		onlineStatus[0]=0;
		onlineStatus[1]=0;
		onlineStatus[2]=0;
		#endif

	}

    static public string Encrypt(string plainText)
    {
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
 
        byte[] keyBytes = new Rfc2898DeriveBytes(ApplicationSecurity.PasswordHash, Encoding.ASCII.GetBytes(ApplicationSecurity.SaltKey)).GetBytes(256 / 8);
        var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
        var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(ApplicationSecurity.VIKey));
 
        byte[] cipherTextBytes;
 
        using (var memoryStream = new MemoryStream())
        {
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                cipherTextBytes = memoryStream.ToArray();
                cryptoStream.Close();
            }
            memoryStream.Close();
        }
        return Convert.ToBase64String(cipherTextBytes);
    }
    static public string Decrypt(string encryptedText)
    {
        byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
        byte[] keyBytes = new Rfc2898DeriveBytes(ApplicationSecurity.PasswordHash, Encoding.ASCII.GetBytes(ApplicationSecurity.SaltKey)).GetBytes(256 / 8);
        var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };
 
        var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(ApplicationSecurity.VIKey));
        var memoryStream = new MemoryStream(cipherTextBytes);
        var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        byte[] plainTextBytes = new byte[cipherTextBytes.Length];
 
        int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
        memoryStream.Close();
        cryptoStream.Close();
        return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
    }
	static public string ReplaceFirst(string text, string search, string replace)
	{
		int pos = text.IndexOf(search);
		if (pos < 0)
		{
			return text;
		}
		return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
	}
	static public void parseXpLevels(string s)
    {
		string[] array = s.Split (new string[]{"#XPLEVEL#"},System.StringSplitOptions.None);
        for(int i=0;i<array.Length-1;i++)
        {
			xpLevels.Add (System.Convert.ToInt32(array[i]));
        }
    }
}