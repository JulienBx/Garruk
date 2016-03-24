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
		this.skills [3] = new PistoBoost();
		this.skills [4] = new PistoSape();
		this.skills [5] = new PistoLest();
		this.skills [6] = new Vitamines();
		this.skills [7] = new Antibiotique();
		this.skills [8] = new Kunai();
		this.skills [9] = new Furtivite();
		this.skills [10] = new Assassinat();
		this.skills [11] = new Blesser();
		this.skills [12] = new Combo();
		this.skills [13] = new Electropiege();
		this.skills [14] = new Pisteur();
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
		this.skills [33] = new GameSkill();
		this.skills [34] = new GameSkill();
		this.skills [35] = new GameSkill();
		this.skills [36] = new GameSkill();
		this.skills [37] = new GameSkill();
		this.skills [38] = new GameSkill();
		this.skills [39] = new Renfoderme();
		this.skills [40] = new GameSkill();
		this.skills [41] = new GameSkill();
		this.skills [42] = new GameSkill();
		this.skills [43] = new GameSkill();
		this.skills [44] = new GameSkill();
		this.skills [45] = new GameSkill();
		this.skills [46] = new GameSkill();
		this.skills [47] = new GameSkill();
		this.skills [48] = new GameSkill();
		this.skills [49] = new GameSkill();
		this.skills [50] = new GameSkill();
		this.skills [51] = new GameSkill();
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
		this.skills [95] = new GameSkill();
		this.skills [96] = new GameSkill();
		this.skills [97] = new GameSkill();
		this.skills [98] = new GameSkill();
		this.skills [99] = new GameSkill();
		this.skills [100] = new GameSkill();
		this.skills [101] = new GameSkill();
		this.skills [102] = new GameSkill();
		this.skills [103] = new GameSkill();
		this.skills [104] = new GameSkill();
		this.skills [105] = new GameSkill();
		this.skills [106] = new GameSkill();
		this.skills [107] = new GameSkill();
		this.skills [108] = new GameSkill();
		this.skills [109] = new GameSkill();
		this.skills [110] = new GameSkill();
		this.skills [111] = new GameSkill();
		this.skills [112] = new GameSkill();
		this.skills [113] = new GameSkill();
		this.skills [114] = new GameSkill();
		this.skills [115] = new GameSkill();
		this.skills [116] = new GameSkill();
		this.skills [117] = new GameSkill();
		this.skills [118] = new GameSkill();
		this.skills [119] = new GameSkill();
		this.skills [120] = new GameSkill();
		this.skills [121] = new GameSkill();
		this.skills [122] = new GameSkill();
		this.skills [123] = new GameSkill();
		this.skills [124] = new GameSkill();
		this.skills [125] = new GameSkill();
		this.skills [126] = new GameSkill();
		this.skills [127] = new GameSkill();
		this.skills [128] = new Cristopower();
		this.skills [129] = new Cristolife();
		this.skills [130] = new GameSkill();
		this.skills [131] = new GameSkill();
		this.skills [132] = new GameSkill();
		this.skills [133] = new GameSkill();
		this.skills [134] = new GameSkill();
		this.skills [135] = new GameSkill();
		this.skills [136] = new GameSkill();
		this.skills [137] = new GameSkill();
		this.skills [138] = new GameSkill();
		this.skills [139] = new GameSkill();
		this.skills [140] = new GameSkill();
		this.skills [141] = new GameSkill();
		this.skills [142] = new GameSkill();
		this.skills [143] = new GameSkill();
		this.skills [144] = new GameSkill();
		this.skills [145] = new GameSkill();

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

