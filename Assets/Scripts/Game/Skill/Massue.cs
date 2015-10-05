using UnityEngine;
using System.Collections.Generic;

public class Massue : GameSkill
{
	public Massue()
	{
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameView.instance.getGC().initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAdjacentOpponentsTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		if (GameView.instance.getIsMine(GameView.instance.getGC().getCurrentPlayingCard())){
			GameView.instance.hideTargets();
		}
		
		int target = targetsPCC[0];
		
		if (Random.Range(1,101) > GameView.instance.getCard(target).GetEsquive())
		{   
			int arg = Random.Range(1,base.skill.ManaCost+1)*GameView.instance.getCard(GameView.instance.getGC().getCurrentPlayingCard()).GetAttack()/100;
			GameView.instance.getGC().addTarget(target,1,arg);
		}
		else{
			GameView.instance.getGC().addTarget(target,0,0);
		}
		
		if (base.card.isGiant()){
			if (Random.Range(1,101) <= base.card.getPassiveManacost()){
				List<Tile> opponents = GameView.instance.getOpponentImmediateNeighbours(GameView.instance.getPlayingCardTile(target));
				if(opponents.Count>1){
					int ran = Random.Range(0,opponents.Count);
					target = GameView.instance.getTileCharacterID(opponents[ran].x, opponents[ran].y) ;
					
					if (Random.Range(1,101) > GameView.instance.getCard(target).GetMagicalEsquive())
					{
						int arg = Random.Range(1,base.skill.ManaCost+1)*GameView.instance.getCard(GameView.instance.getGC().getCurrentPlayingCard()).GetAttack()/100;
						GameView.instance.getGC().addTarget(target,3,arg);
					}
					else{
						GameView.instance.getGC().addTarget(target,2,0);
					}
				}
			}
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
				amount = Mathf.Min (targetCard.GetLife(), base.values[i]);
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
			GameView.instance.setSkillPopUp("lance <b>Massue</b>...", base.card, receivers, receiversTexts);
		}
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAdjacentOpponents();
	}
	
	public override string getTargetText(int id, Card targetCard){
		
		int currentLife = targetCard.GetLife();
		
		string text = "PV : "+currentLife+"->"+(currentLife-1)+"-"+(Mathf.Max(0,currentLife-base.skill.ManaCost))+"\n";
	
		int probaEsquive = targetCard.GetEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "HIT% : "+probaHit;
		
		return text ;
	}
}
