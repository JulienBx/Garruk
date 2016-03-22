using UnityEngine;

public class GameSkills : MonoBehaviour
{	
	public static GameSkills instance;
	GameSkill[] skills ;
	
	void Awake()
	{
		instance = this;
		
		this.skills = new GameSkill[96];
		this.skills [0] = new Attack();
		this.skills [1] = new Pass();
		this.skills [2] = new PistoSoin();
		this.skills [3] = new Fortifiant();
		this.skills [4] = new Relaxant();
		this.skills [5] = new PistoLest();
		this.skills [6] = new Vitamines();
		this.skills [7] = new Antibiotique();
		this.skills [8] = new Kunai();
		this.skills [9] = new Furtivite();
		this.skills [10] = new Assassinat();
		this.skills [11] = new Blesser();
		this.skills [12] = new Combo();
		this.skills [13] = new Electropiege();
		this.skills [14] = new Agilite();
		this.skills [15] = new Guerilla();
		this.skills [16] = new Berserk();
		this.skills [17] = new Attaque360();
		this.skills [18] = new Frenesie();
		this.skills [19] = new Criderage();
		this.skills [20] = new Terreur();
		this.skills [21] = new Cannibale();
		this.skills [22] = new Laser();
		this.skills [23] = new Grenade();
		this.skills [24] = new Bombardier();
		this.skills [25] = new Visee();
		this.skills [26] = new GrosCalibre();
		this.skills [27] = new LanceFlammes();
		this.skills [28] = new Implosion();
		this.skills [29] = new Protection();
		this.skills [30] = new Mitraillette();
		this.skills [31] = new PerfoTir();
		this.skills [32] = new GameSkill();
		this.skills [33] = new Reparation();
		this.skills [34] = new RobotSpecialise();
		this.skills [35] = new Virus();
		this.skills [36] = new GameSkill();
		this.skills [37] = new GameSkill();
		this.skills [38] = new GameSkill();
		this.skills [39] = new Renfoderme();
		this.skills [40] = new TempleSacre();
		this.skills [41] = new ForetDeLianes();
		this.skills [42] = new SablesMouvants();
		this.skills [43] = new GameSkill();
		this.skills [44] = new FontaineDeJouvence();
		this.skills [45] = new Loup();
		this.skills [46] = new Resurrection();
		this.skills [47] = new Vampire();
		this.skills [48] = new Grizzly();
		this.skills [49] = new GameSkill();
		this.skills [50] = new Guerison();
		this.skills [51] = new EauBenite();
		this.skills [52] = new GameSkill();
		this.skills [53] = new GameSkill();
		this.skills [54] = new GameSkill();
		this.skills [55] = new GameSkill();
		this.skills [56] = new Steroide();
		this.skills [57] = new Senilite();
		this.skills [58] = new Telepiege();
		this.skills [59] = new CoupPrecis();
		this.skills [60] = new GameSkill();
		this.skills [61] = new GameSkill();
		this.skills [62] = new GameSkill();
		this.skills [63] = new Massue();
		this.skills [64] = new Piegeur();
		this.skills [65] = new GameSkill();
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
		this.skills [91] = new Lance();
		this.skills [92] = new Desequilibre();
		this.skills [93] = new Furie();
		this.skills [94] = new Poison();
	}

	public GameSkill getSkill(int i)
	{
		return this.skills[i];
	}
	
	public GameSkill getCurrentGameSkill()
	{
		if(GameView.instance.runningSkill!=-1){
			return this.skills[GameView.instance.runningSkill];
		}
		else{
			print ("Pas de running skill");
			return null;
		}
	}
}

