using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
///<summary>
///<para>Scene:All</para>
///<para>Object:GlobalVariables</para>
///<para>Description: Skripta koja sluzi za pamcenje svih globalnih promenjivih</para>
///</summary>

public class GlobalVariables : MonoBehaviour {

	static GlobalVariables instance;
	public static int numberOfStart = 0;
	public static string applicationID ="com.top.burger.chef.restaurant.game";
	public static int coins = 0; // broj coina
	public static bool tutorialPassed = false;
	public static bool happyTime = false; // bool promenjiva koja regulise da li je happytime aktivan na tekucem na nivo-u
	public static bool tipMaster = false; // bool promenjiva koja regulise da li je tipMaster aktivan na tekucem na nivo-u
	public static bool hintArrows = false; // bool promenjiva koja regulise da li je hintArrows na ni aktivan na tekucem na nivo-u
	public static int numberOfWorldSelected = 1; // koji svet je trenutno odabran //1 - prvi svet, 2 - drugi svet, 0 - treci svet
	public static int numberOfWorldUnlocked = 1; // koliko svetova je otkljucano 1 - 3
	public static int numberOfLvl = 1;  // koliko nivoa je ukupno otkljucano 1 - 180, 60 po svetu
	public static int numberOfUnlockedIngredients = 5; // ukupan broj otkljucanih sastojaka na nivou 5-14
	public static int numberOfUnlockedSideFoods = 0; // otkljucan broj priloga 1-4
	public static int currentStars = 0, currentStarsWorld1 = 0, currentStarsWorld2 = 0, currentStarsWorld3 = 0;
	public static int SceneToLoad = 1; // 0 - MainScene, 1 - GamePlay, 2 - TimeAttack, 3 - Championship,
	public static int currentLevel = 0;
	public static int GameplayMode = 1; // 0 - Chef Championship; 1 - Career; 2 - TimeAttack;
//	List<string[]> Worlds = new List<string[]>();
	public string[] world1Stars, world2Stars, world3Stars;
	public static int maxLevelWorld1 = 0, maxLevelWorld2 = 0, maxLevelWorld3 = 0;
	public static bool removeAds = false;
	public static bool doubleCoins = false;
	public static bool startShowned = false;
	string[] allLevels;
	string pom;
	public static GlobalVariables Instance
	{
		get
		{
			if(instance==null)
				instance = GameObject.FindObjectOfType(typeof(GlobalVariables)) as GlobalVariables;
			return instance;
		}
	}

	// Use this for initialization
	void Start () {

		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		if (PlayerPrefs.HasKey ("coins")) 
		{
			coins = PlayerPrefs.GetInt ("coins");
			int tutPassed = PlayerPrefs.GetInt("tutorialPassed");
			if(tutPassed==1)
				tutorialPassed = true;
			numberOfWorldUnlocked = PlayerPrefs.GetInt ("numberOfWorldUnlocked",0);
			numberOfLvl = PlayerPrefs.GetInt ("numberOfLvl");
			numberOfUnlockedIngredients = PlayerPrefs.GetInt ("numberOfUnlockedIngredients");
			numberOfUnlockedSideFoods = PlayerPrefs.GetInt ("numberOfUnlockedSideFoods");
			maxLevelWorld1 = PlayerPrefs.GetInt ("maxLevelWorld1");
			maxLevelWorld2 = PlayerPrefs.GetInt ("maxLevelWorld2");
			maxLevelWorld3 = PlayerPrefs.GetInt ("maxLevelWorld3");

		}
		else 
		{
			PlayerPrefs.SetInt("tutorialPassed",0);
			PlayerPrefs.SetInt ("coins",coins);
			PlayerPrefs.SetInt ("numberOfWorldUnlocked",numberOfWorldUnlocked);
			PlayerPrefs.SetInt ("numberOfLvl",numberOfLvl);
			PlayerPrefs.SetInt ("numberOfUnlockedIngredients",numberOfUnlockedIngredients);
			PlayerPrefs.SetInt ("numberOfUnlockedSideFoods",numberOfUnlockedSideFoods);
			PlayerPrefs.SetInt ("maxLevelWorld1 ",maxLevelWorld1);
			PlayerPrefs.SetInt ("maxLevelWorld2 ",maxLevelWorld2);
			PlayerPrefs.SetInt ("maxLevelWorld3 ",maxLevelWorld3);
			PlayerPrefs.Save();
		}


		if(!PlayerPrefs.HasKey("AllLevels")) //ukoliko prvi put pokrece igru, nivo#brojZvezdica, brojZvezdica: -1 - zakljucan, 0 - otkljucan ali nista nije osvojeno, 1 - 1 zvezdica, 2 - 2 zvezdice, 3 - 3 zvezdice
		{
			InitializeLevels();
		}
		else
		{
			ReadLevels();
		}

		if(PlayerPrefs.HasKey("ChefPlayed"))
		{
			int ChefPlayed = PlayerPrefs.GetInt("ChefPlayed");

			
		}

//		GetStars("test1%test2%test3");
		instance = this;
		transform.name = "GlobalVariables";
		DontDestroyOnLoad(gameObject);
	}

	public void GetStars(string msg)
	{
		string[] values;
		values = msg.Split('%');
		Debug.Log("Ima ih ".Colored(Colors.magenta)+values.Length);

	}

	public void InitializeLevels()
	{
//		pom = "3";
//		for(int j=0;j<3;j++)
//		{
//
//			for(int i=1;i<60;i++)
//			{
//				pom+="!3";
//			}
//			if(j<2)
//			{
//				pom+="%3";
//			}
//			
//		}
		pom = "0";
		for(int j=0;j<3;j++)
		{
			
			for(int i=1;i<60;i++)
			{
				pom+="!-1";
			}
			if(j<2)
			{
				pom+="%0";
			}
			
		}


		PlayerPrefs.SetString("AllLevels",pom);
		PlayerPrefs.Save();
		ReadLevels();
	}

	public void ReadLevels()
	{
		allLevels = new string[3];
		allLevels =  PlayerPrefs.GetString("AllLevels").Split('%');
		string temp1 = allLevels[0];
		string temp2 = allLevels[1];
		string temp3 = allLevels[2];
		currentStars = 0;
		currentStarsWorld1 = 0;
		currentStarsWorld2 = 0;
		currentStarsWorld3 = 0;
		world1Stars = new string[60];
		world2Stars = new string[60];
		world3Stars = new string[60];
		world1Stars = temp1.Split('!');
		world2Stars = temp2.Split('!');
		world3Stars = temp3.Split('!');
		for(int i=0;i<60;i++)
		{
			if(world1Stars[i]!="-1")
			{
				maxLevelWorld1 = i;
				currentStarsWorld1 += int.Parse(world1Stars[i]);
			}
			if(world2Stars[i]!="-1")
			{
				maxLevelWorld2 = i;
				currentStarsWorld2 += int.Parse(world2Stars[i]);
			}
			if(world3Stars[i]!="-1")
			{
				maxLevelWorld3 = i;
				currentStarsWorld3  += int.Parse(world3Stars[i]);
			}
			PlayerPrefs.SetInt ("maxLevelWorld1 ",maxLevelWorld1);
			PlayerPrefs.SetInt ("maxLevelWorld2 ",maxLevelWorld2);
			PlayerPrefs.SetInt ("maxLevelWorld3 ",maxLevelWorld3);
			currentStars = currentStarsWorld1 + currentStarsWorld2 + currentStarsWorld3;
//			currentStars = 333;
			if(currentStars>=310)
			{
				numberOfWorldUnlocked = 3;

			}
			else if(currentStars>=150)
			{
				numberOfWorldUnlocked = 2;
			}
			PlayerPrefs.SetInt ("numberOfWorldUnlocked",numberOfWorldUnlocked);
			PlayerPrefs.Save();
//			Debug.Log("Prvi svet nivo "+(i+1)+", broj zvezdica "+world1Stars[i]);
//			Debug.Log("Drugi svet nivo "+(i+1)+", broj zvezdica "+world2Stars[i]);
//			Debug.Log("Treci svet nivo "+(i+1)+", broj zvezdica "+world3Stars[i]);
		}


		Debug.Log("Ukupno ima zvezdica " +currentStars);

	}

	public void ChangeStarsOnWorld(int numberOfWorldSelected, int newStars)
	{
		switch(numberOfWorldSelected)
		{
		case 1:
			if(int.Parse(world1Stars[currentLevel-1])<=newStars)
			{
				world1Stars[currentLevel-1] = newStars.ToString();

			}
			Debug.Log("currentLevel "+currentLevel+" maxLevelWorld1 "+maxLevelWorld1);
			if(currentLevel<60)
			{
				if(world1Stars[currentLevel].Equals("-1"))
				{
					maxLevelWorld1 = currentLevel;
					world1Stars[currentLevel] = "0";
				}
			}

			if(currentLevel==60)
			{
				GameObject.Find("WinMenu").transform.FindChild("AnimationHolder/WinHolder/AnimationHolder/ButtonsHolder/ButtonNext").gameObject.SetActive(false);
				GameObject.Find("WinMenu").transform.FindChild("AnimationHolder/WinHolder/AnimationHolder/ButtonsHolder/ButtonRestartBG").gameObject.SetActive(false);
				GameObject.Find("WinMenu").transform.FindChild("AnimationHolder/WinHolder/AnimationHolder/ButtonsHolder/ButtonRestart").localPosition = new Vector3(208,-2.6f,0);

			}
				
			break;
		case 2:
			if(int.Parse(world2Stars[currentLevel-1])<=newStars)
			{
				world2Stars[currentLevel-1] = newStars.ToString();
			}
			if(currentLevel<60)
			{
				if(world2Stars[currentLevel].Equals("-1"))
				{
					maxLevelWorld2 = currentLevel;
					world2Stars[currentLevel] = "0";
				}
			}

			if(currentLevel==60)
			{
				GameObject.Find("WinMenu").transform.FindChild("AnimationHolder/WinHolder/AnimationHolder/ButtonsHolder/ButtonNext").gameObject.SetActive(false);
				GameObject.Find("WinMenu").transform.FindChild("AnimationHolder/WinHolder/AnimationHolder/ButtonsHolder/ButtonRestartBG").gameObject.SetActive(false);
				GameObject.Find("WinMenu").transform.FindChild("AnimationHolder/WinHolder/AnimationHolder/ButtonsHolder/ButtonRestart").localPosition = new Vector3(208,-2.6f,0);
				
			}
			break;
		case 0:
			if(int.Parse(world3Stars[currentLevel-1])<=newStars)
			{
				world3Stars[currentLevel-1] = newStars.ToString();
			}
			if(currentLevel<60)
			{
				if(world3Stars[currentLevel].Equals("-1"))
				{
					maxLevelWorld3 = currentLevel;
					world3Stars[currentLevel] = "0";
				}
			}

			if(currentLevel==60)
			{
				GameObject.Find("WinMenu").transform.FindChild("AnimationHolder/WinHolder/AnimationHolder/ButtonsHolder/ButtonNext").gameObject.SetActive(false);
				GameObject.Find("WinMenu").transform.FindChild("AnimationHolder/WinHolder/AnimationHolder/ButtonsHolder/ButtonRestartBG").gameObject.SetActive(false);
				GameObject.Find("WinMenu").transform.FindChild("AnimationHolder/WinHolder/AnimationHolder/ButtonsHolder/ButtonRestart").localPosition = new Vector3(208,-2.6f,0);
				
			}
			break;
		}
		string test = null;

		for(int i=0;i<60;i++)
		{
			if(i==59)
			{
				test +=world1Stars[i];
			}
			else
			{
				test +=world1Stars[i]+"!";
			}

		}
		test+="%";
		for(int i=0;i<60;i++)
		{
			if(i==59)
			{
				test +=world2Stars[i];
			}
			else
			{
				test +=world2Stars[i]+"!";
			}
		}
		test+="%";
		for(int i=0;i<60;i++)
		{
			if(i==59)
			{
				test +=world3Stars[i];
			}
			else
			{
				test +=world3Stars[i]+"!";
			}
		}
		PlayerPrefs.SetString("AllLevels",test);
		PlayerPrefs.Save();
	}

	public IEnumerator moneyCounter(int kolicina, Text moneyText)
	{
		Debug.Log("MoneyCounter " + kolicina);
		GlobalVariables.coins+=kolicina;
		PlayerPrefs.SetInt("coins", GlobalVariables.coins);
		PlayerPrefs.Save();
//		if(kolicina>0)
//		{
//			SoundManager.Instance.Play_Coins();
//		}
		
		int current = int.Parse(moneyText.text);
		int suma = current + kolicina;
		int korak = (suma - current)/10;
		if(korak>=0 && korak<1)
		{
			korak = 1;
		}
		while(current != suma)
		{
			current += korak;
			moneyText.text = current.ToString();
			
			yield return new WaitForSeconds(0.07f);
		}
		moneyText.text = GlobalVariables.coins.ToString();
	}

	void OnApplicationQuit() {
		Debug.Log("OnApplicationQuit".Colored(Colors.red));
		string timeQuitString = DateTime.Now.ToString();
		PlayerPrefs.SetFloat("ChefTimer", Timer.counterTime);



		PlayerPrefs.SetString("VremeQuit", timeQuitString);
		PlayerPrefs.Save();
		
		//Pokreni Notifikaciju za DailyReward na 24h
	}
}
