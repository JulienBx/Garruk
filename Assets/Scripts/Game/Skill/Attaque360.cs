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
		Tile t = GameView.instance.getPlayingCardTile(GameController.instance.getCurrentPlayingCard());
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
						GameController.instance.addTarget(tempInt,1);
					}
					else{
						GameController.instance.addTarget(tempInt,0);
					}
					
					if (base.card.isGiant()){
						if (Random.Range(1,101) <= base.card.getPassiveManacost()){
							List<Tile> opponents = GameView.instance.getOpponentImmediateNeighbours(GameView.instance.getPlayingCardTile(tempInt));
							if(opponents.Count>1){
								int ran = Random.Range(0,opponents.Count);
								tempInt = GameView.instance.getTileCharacterID(opponents[ran].x, opponents[ran].y) ;
								
								if (Random.Range(1,101) > GameView.instance.getCard(tempInt).GetMagicalEsquive())
								{
									GameController.instance.addTarget(tempInt,3);
								}
								else{
									GameController.instance.addTarget(tempInt,2);
								}
							}
						}
					}
				}
			}
			i++;
		}
		
		GameController.instance.play();
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
				amount = (int)Mathf.Min(targetCard.GetLife(),Mathf.Floor((base.card.GetAttack()*(50f+5f*base.skill.Level)/100f)*(1f-(targetCard.GetBouclier()/100f))));
				if (base.card.isLache()){
					if(GameController.instance.getIsFirstPlayer()==GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
						if(GameView.instance.getPlayingCardTile(target).y==GameView.instance.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()).y-1){
							amount = (100+base.card.getPassiveManacost())*amount/100;
						}
					}
					else{
						if(GameView.instance.getPlayingCardTile(target).y-1==GameView.instance.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()).y){
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
				
				GameController.instance.addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
				
				GameView.instance.displaySkillEffect(target, text, 5);
			}	
		}
		GameView.instance.setSkillPopUp("lance <b>Attaque Circulaire</b>...", base.card, receivers, receiversTexts);
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAdjacentOpponents();
	}
}
