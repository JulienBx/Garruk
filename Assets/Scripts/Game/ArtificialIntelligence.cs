using System.Collections.Generic;

public class ArtificialIntelligence
{
	public int aggressiveFactor ;
	public int trapFactor ;
	public int soutienFactor ;
	List<Tile> emplacements ; 

	public ArtificialIntelligence ()
	{
		this.aggressiveFactor = UnityEngine.Random.Range(1,4);
		this.aggressiveFactor = UnityEngine.Random.Range(1,4);
		this.aggressiveFactor = UnityEngine.Random.Range(1,4);
	}

	public void play(){
		
	}

	public void updateDestinations(){
		this.emplacements = GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getDestinations();
		this.emplacements.Add(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public void getActionScore(){
		Tile tempTile ;
		int actionScore = 0 ;
		int bestActionScore = 0 ; 
		int bestImmoActionScore = 0 ; 
		GameSkill gs ;
		List<Skill> skills = GameView.instance.getCurrentCard().Skills; 
		for (int i = 0 ; i < emplacements.Count ; i++){
			tempTile = emplacements[i];
			for(int j = 0 ; j < skills.Count ; j++){
				gs = GameSkills.instance.getSkill(skills[j].Id);
				if (gs.isLaunchable(emplacements[i]).Length<3){
					
				}
			}
		}
	}

}

