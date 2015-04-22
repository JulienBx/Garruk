using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyGameViewModel 
{
	public GUIStyle monLoaderStyle;

	public IList<Deck> myDecks;
	public IList<Card> cards;
	public IList<int> cardsToBeDisplayed;
	public IList<int> cardsIds;
	public IList<int> deckCardsIds;

	public GameObject[] displayedCards;
	public GameObject[] displayedDeckCards;
	public string[] skillsList;
	public string[] cardTypeList;
	public bool[] togglesCurrentStates;

	public GameObject cardFocused;
	public Vector2 scrollPosition;
	public bool areDecksRetrieved;
	public bool isLoadedCards;
	public bool isLoadedDeck;
	public bool soldCard;
	public bool toReloadAll;
	public bool areCreatedDeckCards;
	public bool isSellingCard;
	public bool isUpgradingCard;
	public bool isMarketingCard; 
	public bool isRenamingCard;
	public bool toReload;
	public bool destroyAll;
	public bool displayDecks;
	public bool isCreatedDeckCards;
	public bool isCreatedCards;
	public bool destroySellingCardWindow;
	public bool destroyUpgradingCardWindow;
	public bool destroyRenamingCardWindow;
	public bool destroyFocus;
	public bool isEscDown;
	public bool isChangingPrice;
	public bool displayLoader;
	public bool isUpEscape;
	public bool isDisplayedCards;
	public bool isSkillToDisplay;
	public bool isSkillChosen;
	public bool isBeingDragged;
	public bool confirmSuppress;
	public bool displayCreationDeckWindow;
	public bool isMarketed;
	public int idFocused;
	public int nbCardsPerRow;
	public int cardId;
	public string valueSkill;
	public string textMarket;
	public string tempPrice; 
	public bool enVente;

	public Texture2D[] textures;
	public Texture2D backButton;
	public Texture2D backActivatedButton;

	public MyGameViewModel()
	{
		areDecksRetrieved               = false;
		isLoadedCards                   = false;
		isLoadedDeck                    = false;
		soldCard                        = false;
		toReloadAll                     = false;
		areCreatedDeckCards             = false;
		isSellingCard                   = false;
		isUpgradingCard                 = false;
		isMarketingCard                 = false; 
		isRenamingCard                  = false;
		toReload                        = false;
		destroyAll                      = false;
		displayDecks                    = false;
		isCreatedDeckCards              = false;
		isCreatedCards                  = false;
		destroySellingCardWindow        = false;
		destroyUpgradingCardWindow      = false;
		destroyRenamingCardWindow       = false;
		destroyFocus                    = false;
		isEscDown                       = false;
		isDisplayedCards                = true;
		isSkillToDisplay                = false;
		isSkillChosen                   = false;
		isSkillToDisplay                = false;
		isSkillChosen                   = false;
		isBeingDragged                  = false;
		displayCreationDeckWindow       = false;
		enVente                         = false;
		nbCardsPerRow                   = 0;
		valueSkill                      = "";
		scrollPosition                  = new Vector2(0, 0);
	}

	public void initTextures()
	{
		backButton                      = textures[0];
		backActivatedButton             = textures[1];
	}
}
