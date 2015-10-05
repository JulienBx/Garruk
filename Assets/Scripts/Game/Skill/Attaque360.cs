using UnityEngine;
using System.Collections.Generic;

public class Attaque360 : GameSkill
{
	public Attaque360()
	{
		this.numberOfExpectedTargets = 0 ; 
	}
	
	public override void launch()
	{
		this.resolve(new List<int>());
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		List<Tile> tempTiles;
		Tile t = GameView.instance.getPlayingCardTile(GameView.instance.getGC().getCurrentPlayingCard());
		tempTiles = t.getImmediateNeighbourTiles();
		
		bool isLaunchable = false ;
		int i = 0 ;
		int tempInt ; 
		
		while (!isLaunchable && i<tempTiles.Count){
			t = tempTiles[i];
			tempInt = GameView.instance.getTileCharacterID(t.x, t.y);
			if (tempInt!=-1)
			{
				if (GameView.instance.getPCC(tempInt).canBeTargeted())	
				{
					if (Random.Range(1,101) > GameView.instance.getCard(tempInt).GetEsquive())
					{                             
						GameView.instance.getGC().addTarget(tempInt,1);
					}
					else{
						GameView.instance.getGC().addTarget(tempInt,0);
					}
					
					if (base.card.isGiant()){
						if (Random.Range(1,101) <= base.card.getPassiveManacost()){
							List<Tile> opponents = GameView.instance.getOpponentImmediateNeighbours(GameView.instance.getPlayingCardTile(tempInt));
							if(opponents.Count>1){
								int ran = Random.Range(0,opponents.Count);
								tempInt = GameView.instance.getTileCharacterID(opponents[ran].x, opponents[ran].y) ;
								
								if (Random.Range(1,101) > GameView.instance.getCard(tempInt).GetMagicalEsquive())
								{
									GameView.instance.getGC().addTarget(tempInt,3);
								}
								else{
									GameView.instance.getGC().addTarget(tempInt,2);
								}
							}
						}
					}
				}
			}
			i++;
		}
		
		GameView.instance.getGC().play();
	}
	
	public override void applyOn(){
		Card targetCard ;
		int target ;
		string text ;
		List<Card> receivers =  new List<Card>();
		List<string> receiversTexts =  new List<string>();
		
		int amount ; 
		
		for(int i = 0 ; i < base.targets.Count ; i++){
			target = base.targets[i];
			targetCard = GameView.instance.getCard(target);
			receivers.Add (targetCard);
			if (base.results[i]==0){
				text = "Esquive";
				GameView.instance.displaySkillEffect(target, text, 4);
				receiversTexts.Add (text);
			}
			else if (base.results[i]==2){
				text = "Bonus 'Géant'\nEsquive";
				GameView.instance.displaySkillEffect(target, text, 4);
				receiversTexts.Add (text);
			}
			else{
				amount = Mathf.Min(targetCard.GetLife(),(base.card.GetAttack()*base.skill.ManaCost/100)*(1-(targetCard.GetBouclier()/100)));
				if (base.card.isLache()){
					if(GameView.instance.getGC().getIsFirstPlayer()==GameView.instance.getIsMine(GameView.instance.getGC().getCurrentPlayingCard())){
						if(GameView.instance.getPlayingCardTile(target).y==GameView.instance.getPlayingCardTile(GameView.instance.getGC().getCurrentPlayingCard()).y-1){
							amount = (100+base.card.getPassiveManacost())*amount/100;
						}
					}
					else{
						if(GameView.instance.getPlayingCardTile(target).y-1==GameView.instance.getPlayingCardTile(GameView.instance.getGC().getCurrentPlayingCard()).y){
							amount = (100+base.card.getPassiveManacost())*amount/100;
						}
					}
				}
				if(base.results[i]==3){
					text = "Bonus Géant\n";
				}
				else{
					text="";
				}
				
				text+="-"+amount+" PV";
				
				if(targetCard.GetLife()==amount){
					text+="\nMORT";
				}
				receiversTexts.Add (text);
				
				GameView.instance.getGC().addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
				
				GameView.instance.displaySkillEffect(target, text, 5);
			}	
		}
		if(!GameView.instance.getIsMine(GameView.instance.getGC().getCurrentPlayingCard())){
			GameView.instance.setSkillPopUp("lance <b>Attaque Circulaire</b>...", base.card, receivers, receiversTexts);
		}
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAdjacentOpponents();
	}
}
