using UnityEngine;
using System.Collections.Generic;

public class Antibiotique : GameSkill
{
	public Antibiotique()
	{
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameView.instance.getGC().initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAllButMeModifiersTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		if (GameView.instance.getIsMine(GameView.instance.getGC().getCurrentPlayingCard())){
			GameView.instance.hideTargets();
		}
		
		int target = targetsPCC[0];
		
		int successChances = base.skill.ManaCost;
		
		if (Random.Range(1,101) > GameView.instance.getCard(target).GetMagicalEsquive())
		{                             
			if (Random.Range(1,101) <= successChances)
			{ 
				GameView.instance.getGC().addTarget(target,1);
			}
			else{
				GameView.instance.getGC().addTarget(target,2);
			}
		}
		else{
			GameView.instance.getGC().addTarget(target,0);
		}
		
		if (base.card.isGenerous()){
			if (Random.Range(1,101) <= base.card.getPassiveManacost()){
				List<int> allys = GameView.instance.getEveryoneButMe();
				if(allys.Count>1){
					allys.Remove(target);
					target = allys[Random.Range(0,allys.Count)];
					
					if (Random.Range(1,101) <= successChances)
					{
						if (Random.Range(1,101) > GameView.instance.getCard(target).GetMagicalEsquive())
						{
							GameView.instance.getGC().addTarget(target,4);
						}
						else{
							GameView.instance.getGC().addTarget(target,5);
						}
					}
					else{
						GameView.instance.getGC().addTarget(target,3);
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
				text = "Echec";
				GameView.instance.displaySkillEffect(target, text, 4);
				receiversTexts.Add (text);
			}
			else if (base.results[i]==3){
				text = "Bonus 'Généreux'\nEsquive";
				GameView.instance.displaySkillEffect(target, text, 4);
				receiversTexts.Add (text);
			}
			else if (base.results[i]==5){
				text = "Bonus 'Généreux'\nEchec";
				GameView.instance.displaySkillEffect(target, text, 4);
				receiversTexts.Add (text);
			}
			else{
				GameView.instance.getCard(target).emptyModifiers();
				if(target!=GameView.instance.getGC().getCurrentPlayingCard()){
					GameView.instance.show(target, true);
				}
				else{
					GameView.instance.show(target, false);
				}
				
				if(base.results[i]==4){
					text = "Bonus Généreux\n";
				}
				else{
					text="";
				}
				
				text+="Effets dissipés";
				
				receiversTexts.Add (text);
				
				GameView.instance.displaySkillEffect(target, text, 5);
			}	
		}
		if(!GameView.instance.getIsMine(GameView.instance.getGC().getCurrentPlayingCard())){
			GameView.instance.setSkillPopUp("lance <b>Antibiotique</b>...", base.card, receivers, receiversTexts);
		}
	}
	
	public override string isLaunchable()
	{
		return GameView.instance.canLaunchAllButMeModifiersTargets();
	}
	
	public override string getTargetText(int id, Card targetCard){
		
		int amount = base.skill.ManaCost;
		int attack = base.card.GetAttack();
		string text;
		
		text = "Dissipe les effets\n";
		
		
		int probaEsquive = targetCard.GetMagicalEsquive();
		int probaHit = Mathf.Max(0,(amount*(100-probaEsquive)/100)) ;
		
		text += "HIT% : "+probaHit;
		
		return text ;
	}
}
