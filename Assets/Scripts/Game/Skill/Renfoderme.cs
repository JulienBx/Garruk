using UnityEngine;
using System.Collections.Generic;

public class Renfoderme : GameSkill
{
	public Renfoderme()
	{
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameView.instance.getGC().initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAllysButMeTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		if (GameView.instance.getIsMine(GameView.instance.getGC().getCurrentPlayingCard())){
			GameView.instance.hideTargets();
		}
		
		int target = targetsPCC[0];
		
		if (Random.Range(1,101) > GameView.instance.getCard(target).GetMagicalEsquive())
		{                             
			GameView.instance.getGC().addTarget(target,1);
		}
		else{
			GameView.instance.getGC().addTarget(target,0);
		}
		
		if (base.card.isGenerous()){
			if (Random.Range(1,101) <= base.card.getPassiveManacost()){
				List<int> allys = GameView.instance.getAllys();
				for (int i = 0 ; i < allys.Count ; i++){
					Debug.Log("Allys "+allys[i]);
				}
				if(allys.Count>1){
					allys.Remove(target);
					for (int i = 0 ; i < allys.Count ; i++){
						Debug.Log("Allys2 "+allys[i]);
					}
					
					target = allys[Random.Range(0,allys.Count)];
					
					if (Random.Range(1,101) > GameView.instance.getCard(target).GetMagicalEsquive())
					{
						GameView.instance.getGC().addTarget(target,3);
					}
					else{
						GameView.instance.getGC().addTarget(target,2);
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
				text = "Bonus 'Généreux'\nEsquive";
				GameView.instance.displaySkillEffect(target, text, 4);
				receiversTexts.Add (text);
			}
			else{
				amount = base.skill.ManaCost;
				if(base.results[i]==3){
					text = "Bonus Généreux\n";
				}
				else{
					text="";
				}
				
				text+="Renforcé";
				receiversTexts.Add (text);
				
				GameView.instance.getGC().addCardModifier(target, amount, ModifierType.Type_Bouclier, ModifierStat.Stat_No, -1, 10, "Bouclier", "Dommages subis : -"+amount+"%", "Permanent");
				
				GameView.instance.displaySkillEffect(target, text, 5);
			}	
		}
		if(!GameView.instance.getIsMine(GameView.instance.getGC().getCurrentPlayingCard())){
			GameView.instance.setSkillPopUp("lance <b>Renfoderme</b>...", base.card, receivers, receiversTexts);
		}
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAllysButMeTargets();
	}
	
	public override string getTargetText(int i, Card targetCard){
		
		string text = "Ajoute un bouclier\n";
		
		int probaEsquive = targetCard.GetMagicalEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "HIT% : "+probaHit;
		
		return text ;
	}
}
