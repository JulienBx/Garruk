using UnityEngine;

public class GameSkills : MonoBehaviour
{	
	public static GameSkills instance;
	GameSkill[] skills ;
	Skill currentSkill ;

	void Awake()
	{
		instance = this;
		
		this.skills = new GameSkill[96];
		this.skills [0] = new Attack();
		this.skills [1] = new Pass();
		this.skills [2] = new GameSkill();
		this.skills [3] = new Excitant();
		this.skills [4] = new Calmant();
		this.skills [5] = new Fortifiant();
		this.skills [6] = new Relaxant();
		this.skills [7] = new Lest();
		this.skills [8] = new Adrenaline();
		this.skills [9] = new Antibiotique();
		this.skills [10] = new TirALarc();
		this.skills [11] = new Furtivite();
		this.skills [12] = new Assassinat();
		this.skills [13] = new Estoc();
		this.skills [14] = new Combo();
		this.skills [15] = new Electropiege();
		this.skills [16] = new Agilite();
		this.skills [17] = new CoupeJambes();
		this.skills [18] = new Berserk();
		this.skills [19] = new Attaque360();
		this.skills [20] = new Frenesie();
		this.skills [21] = new Rugissement();
		this.skills [22] = new Terreur();
		this.skills [23] = new Cannibale();
		this.skills [24] = new RayonEnergie();
		this.skills [25] = new BouleEnergie();
		this.skills [26] = new TempeteEnergie();
		this.skills [27] = new EnergieQuantique();
		this.skills [28] = new PuissanceIncontrolable();
		this.skills [29] = new Concentration();
		this.skills [30] = new ImplosionEnergie();
		this.skills [31] = new GameSkill();
		this.skills [32] = new GameSkill();
		this.skills [33] = new GameSkill();
		this.skills [34] = new GameSkill();
		this.skills [35] = new Reparation();
		this.skills [36] = new RobotSpecialise();
		this.skills [37] = new Virus();
		this.skills [38] = new GameSkill();
		this.skills [39] = new GameSkill();
		this.skills [40] = new GameSkill();
		this.skills [41] = new Renfoderme();
		this.skills [42] = new TempleSacre();
		this.skills [43] = new ForetDeLianes();
		this.skills [44] = new SablesMouvants();
		this.skills [45] = new GameSkill();
		this.skills [46] = new FontaineDeJouvence();
		this.skills [47] = new Loup();
		this.skills [48] = new Resurrection();
		this.skills [49] = new Vampire();
		this.skills [50] = new Grizzly();
		this.skills [51] = new AppositionDesMains();
		this.skills [52] = new Guerison();
		this.skills [53] = new EauBenite();
		this.skills [54] = new GameSkill();
		this.skills [55] = new GameSkill();
		this.skills [56] = new GameSkill();
		this.skills [57] = new GameSkill();
		this.skills [58] = new Steroide();
		this.skills [59] = new Senilite();
		this.skills [60] = new PiegeAffaiblissant();
		this.skills [61] = new Somnipiege();
		this.skills [62] = new GameSkill();
		this.skills [63] = new GameSkill();
		this.skills [64] = new GameSkill();
		this.skills [65] = new Massue();
		this.skills [66] = new GameSkill();
		this.skills [67] = new GameSkill();
		this.skills [68] = new GameSkill();
		this.skills [69] = new GameSkill();
		this.skills [70] = new GameSkill();
		this.skills [71] = new GameSkill();
		this.skills [72] = new GameSkill();
		this.skills [73] = new GameSkill();
		this.skills [74] = new GameSkill();
		this.skills [75] = new GameSkill();
		this.skills [76] = new GameSkill();
		this.skills [77] = new GameSkill();
		this.skills [78] = new GameSkill();
		this.skills [79] = new GameSkill();
		this.skills [80] = new GameSkill();
		this.skills [81] = new GameSkill();
		this.skills [82] = new GameSkill();
		this.skills [83] = new GameSkill();
		this.skills [84] = new GameSkill();
		this.skills [85] = new GameSkill();
		this.skills [86] = new GameSkill();
		this.skills [87] = new GameSkill();
		this.skills [88] = new GameSkill();
		this.skills [89] = new GameSkill();
		this.skills [90] = new GameSkill();
		this.skills [91] = new GameSkill();
		this.skills [92] = new GameSkill();
		this.skills [93] = new Lance();
		this.skills [94] = new ToutDonner();
		this.skills [95] = new Furie();
		
	}

	public GameSkill getSkill(int i)
	{
		return this.skills[i];
	}
	
	public GameSkill getCurrentGameSkill()
	{
		return this.skills[this.currentSkill.Id];
	}
	
	public GameSkill getCurrentSkill()
	{
		return this.skills[this.currentSkill.Id];
	}
	
	public int getCurrentSkillId()
	{
		return this.currentSkill.Id;
	}
	
	public void setCurrentSkill(Skill s){
		this.currentSkill = s ;
	}
}

