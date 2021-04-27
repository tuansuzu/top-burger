using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif

///<summary>
///<para>Scene:GamePlay</para>
///<para>Object:LevelGenerator</para>
///<para>Description: GamePlay funkcionalnost </para>
///</summary>

public class LevelGenerator : MonoBehaviour {

	int maxNumberOfCharachters;
	public static bool videoChampAvailable = false;
	public static bool videoChampWatched = false;
	public Sprite[] WorldOneSprites;
	public Sprite[] WorldTwoSprites;
	public Sprite[] WorldThreeSprites;
	public Sprite[] IngredientsSprites;
	public Sprite[] IngredientWorld1ReferenceSprites;
	public Sprite[] IngredientWorld2ReferenceSprites;
	public Sprite[] IngredientWorld3ReferenceSprites;
	public Sprite[] IngredientOnTableWorld1ReferenceSprites;
	public Sprite[] IngredientOnTableWorld2ReferenceSprites;
	public Sprite[] IngredientOnTableWorld3ReferenceSprites;
	public Sprite empty;
	public static bool timeFreezeActive = false;
	public static bool winGame = false;
	public static int currentCharacter = -1, lastCharacter = 99;
	public static int score = 0, tipsEarningsFinal, mealEarningFinal;
	public Sprite[] SideFoodSprites;
	public Sprite[] SideFoodSpritesWorld1;
	public Sprite[] SideFoodSpritesWorld2;
	public Sprite[] SideFoodSpritesWorld3;
	public Sprite[] UnlockedSpritesWorld1,UnlockedSpritesWorld2,UnlockedSpritesWorld3;
	public GameObject TipsPrefab;
	public static bool gameActive = false;
	public static bool gameStarted = false;
	public static bool customerActive = false;
	public static bool hintsPowerActive = false, tipsMasterPowerActive =false, happyTimePowerActive = false;
	GameObject hintHolder, sideFoodHintsHolder;
	int hintPowerPrice = 1000, tipsMasterPowerPrice = 1500, happyTimePowerPrice = 800;
	public static int numberOfTips = 0;
	Text titleUnlock, messageUnlock; // unlock pop-up title and msg
	Image imageUnlock; // unlock pop-up image of unlocked item
	Vector3[] tipsPositions = new []{new Vector3(-39f,38f,0f),new Vector3(42f,38f,0f),new Vector3(52f,-1,0f),new Vector3(-47f,6f,0f)};
	bool firstOrder = true;
	int mealPrice = 0;
	public static bool isMadCustomer = false;
	public static int numberOfCoinsTaken = 0;
	Text CoinsNumberText;
	GameObject DeliveryHolder, MainMealOrder, SideFoodOrder, DeliverySideFoodHolder, FoodDeliveryHolder, CorrectHolder, CharacterChangerObject;
	Sprite CorrectSprite, IncorectSprite;
	int lastNumberOfSideFoodOrders = -1, lastNumberofMainMealOrders = -1, numberofMainMealOrders, numberOfOrders, maxNumberOfSideFoodOrders,numberOfSideFoodOrders, numberOfIngredientsForMainMeal, currentIndexOfIngredient; //numberOfOrders-koliko ima narudzbina 1-5(glavno jelo i sideFood, numberOfIngredientsForMainMeal - koliko sastojaka ima tekuca naruzbina, currentIndexOfIngredient - trenutni index
	int maxNumberOfIngredientsPerOrder; // max broj sastojaka u orderu za trenutni nivo, ne racunajuci donji i gornji hleb
	int lastIngredient = 99; // poslednji sastojak u trenutnoj narudzbini, na pocetku i kada resetujemo postavljamo ga na 99
	List<int> ingredientsList=new List<int>();
	List<int> sideFoodOrders=new List<int>();
	List<int> sideFoodOrdersBackUp=new List<int>();
	List<int> allPosiblleSideFoodOrders=new List<int>();
	List<int> championshipallMealsOrder=new List<int>();
	int currentSideOrder = 0;
	public static bool tutorial = false;
	public static bool tipsTutorial = true;
	Animator Character;
	bool sideFoodChecked = false, mainFoodFoodChecked = false;
	int championshipMainOrderIndex, championshipSideOrderIndex;
	bool championshipEndOrder = false;
	string[] girlNames = new string[]{"Emma","Sophia","Madison","Zoey","Aubrey","Arianna","Hailey","Peyton","Alexis","Mackenzie","Kylie","Elena", "Rachel", "Vanessa", "Jessica"};
	string[] boyNames = new string[]{"Dylan","James","David","Liam","Daniel","Carter","Kevin","Brandon","Tyler","Justin","Steven","Patrick","Alan","Bradley","Derek"};
	public static bool timeVideoWatched = false;
	int character1RandomCurrent = -1, character2RandomCurrent = -1, character3RandomCurrent = -1, character4RandomCurrent = -1, character5RandomCurrent = -1, character6RandomCurrent = -1, character7RandomCurrent = -1;
	int character1RandomLast = 99, character2RandomLast = 99, character3RandomLast = 99, character4RandomLast = 99, character5RandomLast = 99, character6RandomLast = 99, character7RandomLast = 99;
	// Use this for initialization


	void Awake()
	{
		Debug.Log("SVET JE "+GlobalVariables.numberOfWorldSelected);
		timeVideoWatched = false;

		AdsManager.Instance.IsVideoRewardAvailable();
		tipsEarningsFinal = 0;
		mealEarningFinal = 0;
		gameStarted = false;
		if(GlobalVariables.numberOfWorldSelected==1)
		{
			if(GlobalVariables.currentLevel==1 && !GlobalVariables.tutorialPassed)
			{
				tipsTutorial = false;
				tutorial=true;
			}
			else
			{
				tipsTutorial = true;
				tutorial=false;
			}


			GameObject.Find("MainMenu/AnimationHolder/BG").GetComponent<Image>().sprite = WorldOneSprites[0];
			GameObject.Find("MainMenu/AnimationHolder/BG/BGBlur").GetComponent<Image>().sprite = WorldOneSprites[1];
			GameObject.Find("RestorantHolder").GetComponent<Image>().sprite = WorldOneSprites[2];
			GameObject.Find("Tray").GetComponent<Image>().sprite = WorldOneSprites[3];
			GameObject.Find("Plate").GetComponent<Image>().sprite = WorldOneSprites[4];
			GameObject.Find("LeftLegImage").GetComponent<Image>().sprite = WorldOneSprites[5];
			GameObject.Find("RightLegImage").GetComponent<Image>().sprite = WorldOneSprites[5];
			GameObject.Find("TableHolderImage").GetComponent<Image>().sprite = WorldOneSprites[6];
			GameObject.Find("BurgerPosterImage").GetComponent<Image>().sprite = WorldOneSprites[7];

			for(int i =0;i<14;i++)
			{
				GameObject.Find("IngredientsHolder").transform.GetChild(i).GetComponent<Image>().sprite = WorldOneSprites[8];
			}

			for(int i =0;i<8;i++)
			{
				GameObject.Find ("TableHolder").transform.GetChild(i).GetComponent<Image>().sprite = WorldOneSprites[9];
			}

			for(int i =0;i<4;i++)
			{
				GameObject.Find("SideFoodsHolder").transform.GetChild(i).GetComponent<Image>().sprite = WorldOneSprites[8];
				SideFoodSprites[i] = SideFoodSpritesWorld1[i];
				GameObject.Find("SideFoodsHolder").transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = SideFoodSpritesWorld1[i];
			}

			for(int i =0;i<14;i++)
			{
				IngredientsSprites[i] = IngredientWorld1ReferenceSprites[i];
			}

			for(int i =0;i<14;i++)
			{
				GameObject.Find("IngredientsHolder").transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = IngredientOnTableWorld1ReferenceSprites[i];
			}
		}
		else if(GlobalVariables.numberOfWorldSelected==2)
		{
			GameObject.Find("MainMenu/AnimationHolder/BG").GetComponent<Image>().sprite = WorldTwoSprites[0];
			GameObject.Find("MainMenu/AnimationHolder/BG/BGBlur").GetComponent<Image>().sprite = WorldTwoSprites[1];
			GameObject.Find("RestorantHolder").GetComponent<Image>().sprite = WorldTwoSprites[2];
			GameObject.Find("Tray").GetComponent<Image>().sprite = WorldTwoSprites[3];
			GameObject.Find("Plate").GetComponent<Image>().sprite = WorldTwoSprites[4];
			GameObject.Find("LeftLegImage").GetComponent<Image>().sprite = WorldTwoSprites[5];
			GameObject.Find("RightLegImage").GetComponent<Image>().sprite = WorldTwoSprites[5];
			GameObject.Find("TableHolderImage").GetComponent<Image>().sprite = WorldTwoSprites[6];
			GameObject.Find("BurgerPosterImage").GetComponent<Image>().sprite = WorldTwoSprites[7];
			
			for(int i =0;i<14;i++)
			{
				GameObject.Find("IngredientsHolder").transform.GetChild(i).GetComponent<Image>().sprite = WorldTwoSprites[8];
			}
			
			for(int i =0;i<9;i++)
			{
				GameObject.Find("TableHolder").transform.GetChild(i).GetComponent<Image>().sprite = WorldTwoSprites[9];
			}
			
			for(int i =0;i<4;i++)
			{
				GameObject.Find("SideFoodsHolder").transform.GetChild(i).GetComponent<Image>().sprite = WorldTwoSprites[8];
				SideFoodSprites[i] = SideFoodSpritesWorld2[i];
				GameObject.Find("SideFoodsHolder").transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = SideFoodSpritesWorld2[i];
			}

			for(int i =0;i<14;i++)
			{
				IngredientsSprites[i] = IngredientWorld2ReferenceSprites[i];
				GameObject.Find("IngredientsHolder").transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = IngredientWorld2ReferenceSprites[i];
			}

			for(int i =0;i<14;i++)
			{
				GameObject.Find("IngredientsHolder").transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = IngredientOnTableWorld2ReferenceSprites[i];
			}
		}
		else if(GlobalVariables.numberOfWorldSelected==0)
		{

			GameObject.Find("MainMenu/AnimationHolder/BG").GetComponent<Image>().sprite = WorldThreeSprites[0];
			GameObject.Find("MainMenu/AnimationHolder/BG/BGBlur").GetComponent<Image>().sprite = WorldThreeSprites[1];
			GameObject.Find("RestorantHolder").GetComponent<Image>().sprite = WorldThreeSprites[2];
			GameObject.Find("Tray").GetComponent<Image>().sprite = WorldThreeSprites[3];
			GameObject.Find("Plate").GetComponent<Image>().sprite = WorldThreeSprites[4];
			GameObject.Find("LeftLegImage").GetComponent<Image>().sprite = WorldThreeSprites[5];
			GameObject.Find("RightLegImage").GetComponent<Image>().sprite = WorldThreeSprites[5];
			GameObject.Find("TableHolderImage").GetComponent<Image>().sprite = WorldThreeSprites[6];
			GameObject.Find("BurgerPosterImage").GetComponent<Image>().sprite = WorldThreeSprites[7];
			
			for(int i =0;i<14;i++)
			{
				GameObject.Find("IngredientsHolder").transform.GetChild(i).GetComponent<Image>().sprite = WorldThreeSprites[8];
			}
			
			for(int i =0;i<9;i++)
			{
				GameObject.Find("TableHolder").transform.GetChild(i).GetComponent<Image>().sprite = WorldThreeSprites[9];
			}
			
			for(int i =0;i<4;i++)
			{
				GameObject.Find("SideFoodsHolder").transform.GetChild(i).GetComponent<Image>().sprite = WorldThreeSprites[8];
				SideFoodSprites[i] = SideFoodSpritesWorld3[i];
				GameObject.Find("SideFoodsHolder").transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = SideFoodSpritesWorld3[i];
			}

			for(int i =0;i<14;i++)
			{
				IngredientsSprites[i] = IngredientWorld3ReferenceSprites[i];
				GameObject.Find("IngredientsHolder").transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = IngredientWorld3ReferenceSprites[i];
			}

			for(int i =0;i<14;i++)
			{
				GameObject.Find("IngredientsHolder").transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = IngredientOnTableWorld3ReferenceSprites[i];
			}
		}
	}

	void Start () {
		// 0 - Chef Championship; 1 - Career; 2 - TimeAttack;
		if(GlobalVariables.GameplayMode==0)
		{
			Debug.Log("Chef Championship MODE".Colored(Colors.cyan));
			GameObject.Find("PowerUpsHolder").SetActive(false);
//			GameObject.Find("CustomerOrderHolder").SetActive(false); //NATIVE AD MRK

			GameObject.Find("FoodDeliveryHolder").GetComponent<Animator>().enabled = false;
			GameObject.Find("FoodDeliveryHolder").transform.localScale = Vector3.one;
			GameObject.Find("FoodDeliveryHolder").GetComponent<RectTransform>().anchoredPosition = new Vector2(-190,236);
//			GameObject.Find("FacebookNativeAdChampionship").GetComponent<FacebookNativeAd>().LoadAd();
//			GameObject.Find("FoodDeliveryHolder").transform.position = new Vector3(0, GameObject.Find("FoodDeliveryHolder").transform.position.y, 0); //NATIVE AD MRK
			GameObject.Find("LevelTimerHolder").SetActive(false);
			GameObject.Find("HeaderHolder/GoalHolder").SetActive(false);
			GameObject.Find("MainMenu/AnimationHolder/HeaderHolder/CoinHolderlHolder").SetActive(false);
			GlobalVariables.numberOfUnlockedIngredients = 14;
			GlobalVariables.numberOfUnlockedSideFoods = 4;
		}
		else if(GlobalVariables.GameplayMode==1)
		{
			GameObject.Find("ChampionshipHolder").SetActive(false);
//			Debug.Log("Career MODE".Colored(Colors.cyan));
			//deo za podesavanje maximalnog broja namirnica po jelu zavisno od nivoa
			if(GlobalVariables.currentLevel<10)
			{
				maxNumberOfIngredientsPerOrder=5;
			}
			else if(GlobalVariables.currentLevel<20)
			{
				maxNumberOfIngredientsPerOrder=6;
			}
			else if(GlobalVariables.currentLevel<30)
			{
				maxNumberOfIngredientsPerOrder=7;
			}
			else if(GlobalVariables.currentLevel<40)
			{
				maxNumberOfIngredientsPerOrder=8;
			}
			else if(GlobalVariables.currentLevel<50)
			{
				maxNumberOfIngredientsPerOrder=9;
			}
			else
			{
				maxNumberOfIngredientsPerOrder=10;
			}

			//deo za podesavanje maximalnog broja karaktera zavisno od nivoa
			if(GlobalVariables.currentLevel<5)
			{
				maxNumberOfCharachters = 2;
			}
			else if(GlobalVariables.currentLevel<15)
			{
				maxNumberOfCharachters = 3;
			}
			else if(GlobalVariables.currentLevel<25)
			{
				maxNumberOfCharachters = 4;
			}
			else if(GlobalVariables.currentLevel<35)
			{
				maxNumberOfCharachters = 5;
			}
			else if(GlobalVariables.currentLevel<45)
			{
				maxNumberOfCharachters = 6;
			}
			else
			{
				maxNumberOfCharachters = 7;
			}

			//deo za podesavanje broja otkljucanih sastojaka zavisno od nivoa
			if(GlobalVariables.currentLevel<10)
			{
				GlobalVariables.numberOfUnlockedIngredients = 5;
			}
			else if(GlobalVariables.currentLevel<14)
			{
				GlobalVariables.numberOfUnlockedIngredients = 6;
			}
			else if(GlobalVariables.currentLevel<18)
			{
				GlobalVariables.numberOfUnlockedIngredients = 7;
			}
			else if(GlobalVariables.currentLevel<22)
			{
				GlobalVariables.numberOfUnlockedIngredients = 8;
			}
			else if(GlobalVariables.currentLevel<26)
			{
				GlobalVariables.numberOfUnlockedIngredients = 9;
			}
			else if(GlobalVariables.currentLevel<30)
			{
				GlobalVariables.numberOfUnlockedIngredients = 10;
			}
			else if(GlobalVariables.currentLevel<34)
			{
				GlobalVariables.numberOfUnlockedIngredients = 11;
			}
			else if(GlobalVariables.currentLevel<38)
			{
				GlobalVariables.numberOfUnlockedIngredients = 12;
			}
			else if(GlobalVariables.currentLevel<42)
			{
				GlobalVariables.numberOfUnlockedIngredients = 13;
			}
			else
			{
				GlobalVariables.numberOfUnlockedIngredients = 14;
			}

			//deo za podesavanje broja otkljucanih side food zavisno od nivoa
			if(GlobalVariables.currentLevel<6)
			{
				GlobalVariables.numberOfUnlockedSideFoods = 0;
			}
			else if(GlobalVariables.currentLevel<46)
			{
				GlobalVariables.numberOfUnlockedSideFoods = 1;
			}
			else if(GlobalVariables.currentLevel<50)
			{
				GlobalVariables.numberOfUnlockedSideFoods = 2;
			}
			else if(GlobalVariables.currentLevel<54)
			{
				GlobalVariables.numberOfUnlockedSideFoods = 3;
			}
			else
			{
				GlobalVariables.numberOfUnlockedSideFoods = 4;
			}
		}
		else if(GlobalVariables.GameplayMode==2)
		{
			gameStarted = true;
			GameObject.Find("ChampionshipHolder").SetActive(false);
//			Debug.Log("TimeAttack MODE".Colored(Colors.cyan));
			GameObject.Find("CustomerTimer").SetActive(false);
			GlobalVariables.numberOfUnlockedIngredients = 14;
			GlobalVariables.numberOfUnlockedSideFoods = 4;
			maxNumberOfIngredientsPerOrder=10;
		}

		currentCharacter = -1;
		hintHolder = GameObject.Find("IngredientsHolder");
		sideFoodHintsHolder = GameObject.Find("SideFoodsHolder");

		GameObject.Find("Canvas/PopUps").transform.FindChild("LoadingHolder/LoadingHolder/AnimationHolder").GetComponent<Animator>().Play("LoadingGameplayDeparting");
		if(GlobalVariables.GameplayMode!=0)
		{
			CoinsNumberText = GameObject.Find("CoinsText").GetComponent<Text>();
			GameObject.Find("NumberOfCoinsGoal").GetComponent<Text>().text = GlobalVariables.coins.ToString();
			CoinsNumberText.text = GlobalVariables.coins.ToString();
		}
		titleUnlock = GameObject.Find("Canvas").transform.FindChild("PopUps/PopUpUnlocked/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>();
		messageUnlock = GameObject.Find("Canvas").transform.FindChild("PopUps/PopUpUnlocked/AnimationHolder/Body/ContentHolder/TextBG/TextMessage").GetComponent<Text>();
		imageUnlock = GameObject.Find("Canvas").transform.FindChild("PopUps/PopUpUnlocked/AnimationHolder/Body/ContentHolder/ItemBG/Item").GetComponent<Image>();
		SetGoalPopUp();
		Character = GameObject.Find("CharacterHolder/AnimationHolder").GetComponent<Animator>();
		DeliveryHolder = GameObject.Find("DeliveryHolder");
		if(GlobalVariables.GameplayMode!=0)
		{
			MainMealOrder = GameObject.Find("MainMealOrder");
			SideFoodOrder = GameObject.Find("SideFoodOrder");
		}
		DeliverySideFoodHolder = GameObject.Find("DeliverySideFoodHolder");
		FoodDeliveryHolder = GameObject.Find("FoodDeliveryHolder");
		CorrectHolder = GameObject.Find("CorrectHolder");
		CorrectSprite = GameObject.Find("ReferenceImage/CorrectImage").GetComponent<Image>().sprite;
		IncorectSprite = GameObject.Find("ReferenceImage/IncorrectImage").GetComponent<Image>().sprite;
		CharacterChangerObject = GameObject.Find("CharacterHolder");

		maxNumberOfSideFoodOrders = GlobalVariables.numberOfUnlockedSideFoods;
		
	
		if(maxNumberOfSideFoodOrders>0)
		{
			//			Debug.Log("Vise "+maxNumberOfSideFoodOrders);
			
			for(int i=0; i<maxNumberOfSideFoodOrders;i++)
			{
				int numberOfSideFoodHolder = i+1;
				sideFoodHintsHolder.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
				sideFoodHintsHolder.transform.GetChild(i).GetChild(0).GetComponent<Image>().color = Color.white;
//				sideFoodHintsHolder.transform.GetChild(i).GetChild(0).GetComponent<Button>().interactable = true;
				sideFoodHintsHolder.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
			}
		}
		
		for(int i=0;i<GlobalVariables.numberOfUnlockedIngredients;i++)
		{
			hintHolder.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
			hintHolder.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
			hintHolder.transform.GetChild(i).GetChild(0).GetComponent<Image>().color = Color.white;
//			hintHolder.transform.GetChild(i).GetChild(0).GetComponent<Button>().interactable = true;
			
		}
		//HINT 7, Tip 31, Happy 47
//		if(PlayerPrefs.HasKey("BoostHints"))
//		{
//			GameObject.Find("PopUps/PopUpGoal/AnimationHolder/Body/ContentHolder").transform.FindChild("PowerslHolder").transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.white;
//			GameObject.Find("PopUps/PopUpGoal/AnimationHolder/Body/ContentHolder").transform.FindChild("PowerslHolder").transform.GetChild(0).GetChild(2).GetComponent<Text>().color = Color.white;
//			GameObject.Find("PopUps/PopUpGoal/AnimationHolder/Body/ContentHolder").transform.FindChild("PowerslHolder").transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
//			GameObject.Find("PopUps/PopUpGoal/AnimationHolder/Body/ContentHolder").transform.FindChild("PowerslHolder").transform.GetChild(0).GetChild(0).GetComponent<Button>().interactable = true;
//		}
//		else
//		{
			if(GlobalVariables.currentLevel>7)
			{
				GameObject.Find("PopUps/PopUpGoal/AnimationHolder/Body/ContentHolder").transform.FindChild("PowerslHolder").transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.white;
				GameObject.Find("PopUps/PopUpGoal/AnimationHolder/Body/ContentHolder").transform.FindChild("PowerslHolder").transform.GetChild(0).GetChild(2).GetComponent<Text>().color = Color.white;
				GameObject.Find("PopUps/PopUpGoal/AnimationHolder/Body/ContentHolder").transform.FindChild("PowerslHolder").transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
				GameObject.Find("PopUps/PopUpGoal/AnimationHolder/Body/ContentHolder").transform.FindChild("PowerslHolder").transform.GetChild(0).GetChild(0).GetComponent<Button>().interactable = true;
				PlayerPrefs.SetInt("BoostHints",1);
				PlayerPrefs.Save();
			}
//		}

//		if(PlayerPrefs.HasKey("BoostMagnet"))
//		{
//			GameObject.Find("PopUps/PopUpGoal/AnimationHolder/Body/ContentHolder").transform.FindChild("PowerslHolder").transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.white;
//			GameObject.Find("PopUps/PopUpGoal/AnimationHolder/Body/ContentHolder").transform.FindChild("PowerslHolder").transform.GetChild(1).GetChild(2).GetComponent<Text>().color = Color.white;
//			GameObject.Find("PopUps/PopUpGoal/AnimationHolder/Body/ContentHolder").transform.FindChild("PowerslHolder").transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
//			GameObject.Find("PopUps/PopUpGoal/AnimationHolder/Body/ContentHolder").transform.FindChild("PowerslHolder").transform.GetChild(1).GetChild(0).GetComponent<Button>().interactable = true;
//		}
//		else
//		{
			if(GlobalVariables.currentLevel>31)
			{
				GameObject.Find("PopUps/PopUpGoal/AnimationHolder/Body/ContentHolder").transform.FindChild("PowerslHolder").transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.white;
				GameObject.Find("PopUps/PopUpGoal/AnimationHolder/Body/ContentHolder").transform.FindChild("PowerslHolder").transform.GetChild(1).GetChild(2).GetComponent<Text>().color = Color.white;
				GameObject.Find("PopUps/PopUpGoal/AnimationHolder/Body/ContentHolder").transform.FindChild("PowerslHolder").transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
				GameObject.Find("PopUps/PopUpGoal/AnimationHolder/Body/ContentHolder").transform.FindChild("PowerslHolder").transform.GetChild(1).GetChild(0).GetComponent<Button>().interactable = true;
				PlayerPrefs.SetInt("BoostMagnet",1);
				PlayerPrefs.Save();
			}
//		}

//		if(PlayerPrefs.HasKey("BoostHappines"))
//		{
//			GameObject.Find("PopUps/PopUpGoal/AnimationHolder/Body/ContentHolder").transform.FindChild("PowerslHolder").transform.GetChild(2).GetChild(0).GetComponent<Image>().color = Color.white;
//			GameObject.Find("PopUps/PopUpGoal/AnimationHolder/Body/ContentHolder").transform.FindChild("PowerslHolder").transform.GetChild(2).GetChild(2).GetComponent<Text>().color = Color.white;
//			GameObject.Find("PopUps/PopUpGoal/AnimationHolder/Body/ContentHolder").transform.FindChild("PowerslHolder").transform.GetChild(2).GetChild(3).gameObject.SetActive(false);
//			GameObject.Find("PopUps/PopUpGoal/AnimationHolder/Body/ContentHolder").transform.FindChild("PowerslHolder").transform.GetChild(2).GetChild(0).GetComponent<Button>().interactable = true;
//		}
//		else
//		{
			if(GlobalVariables.currentLevel>47)
			{
				GameObject.Find("PopUps/PopUpGoal/AnimationHolder/Body/ContentHolder").transform.FindChild("PowerslHolder").transform.GetChild(2).GetChild(0).GetComponent<Image>().color = Color.white;
				GameObject.Find("PopUps/PopUpGoal/AnimationHolder/Body/ContentHolder").transform.FindChild("PowerslHolder").transform.GetChild(2).GetChild(2).GetComponent<Text>().color = Color.white;
				GameObject.Find("PopUps/PopUpGoal/AnimationHolder/Body/ContentHolder").transform.FindChild("PowerslHolder").transform.GetChild(2).GetChild(3).gameObject.SetActive(false);
				GameObject.Find("PopUps/PopUpGoal/AnimationHolder/Body/ContentHolder").transform.FindChild("PowerslHolder").transform.GetChild(2).GetChild(0).GetComponent<Button>().interactable = true;
				PlayerPrefs.SetInt("BoostHappines",1);
				PlayerPrefs.Save();
			}
//		}



		if(GlobalVariables.GameplayMode!=0)
		{
//			if(PlayerPrefs.HasKey("PowerUpFreezeTime"))
//			{
//				GameObject.Find("StopTimeImage").GetComponent<Image>().color = Color.white;
//				GameObject.Find("StopTimeImage").GetComponent<Button>().interactable = true;
//				GameObject.Find("LockedStopTime").SetActive(false);
//			}
//			else
//			{
				if(GlobalVariables.currentLevel>10)
				{
					GameObject.Find("StopTimeImage").GetComponent<Image>().color = Color.white;
					GameObject.Find("StopTimeImage").GetComponent<Button>().interactable = true;
					GameObject.Find("LockedStopTime").SetActive(false);
					PlayerPrefs.SetInt("PowerUpFreezeTime",1);
					PlayerPrefs.Save();
				}
//			}


//			if(PlayerPrefs.HasKey("PowerUpAddTime"))
//			{
//				GameObject.Find("AddTimeImage").GetComponent<Image>().color = Color.white;
//				GameObject.Find("AddTimeImage").GetComponent<Button>().interactable = true;
//				GameObject.Find("LockedAddTime").SetActive(false);
//			}
//			else
//			{
				if(GlobalVariables.currentLevel>20)
				{
					GameObject.Find("AddTimeImage").GetComponent<Image>().color = Color.white;
					GameObject.Find("AddTimeImage").GetComponent<Button>().interactable = true;
					GameObject.Find("LockedAddTime").SetActive(false);
					PlayerPrefs.SetInt("PowerUpAddTime",1);
					PlayerPrefs.Save();
				}
//			}
		}

	}
	

	IEnumerator CleanTips(Transform tips, float time)
	{
		yield return new WaitForSeconds(time);

			if(tips.GetChild(0).GetChild(2).gameObject.activeSelf)
			{
				tips.GetComponent<Animator>().Play("TipsCollected");
			}
			else
			{
				tips.GetComponent<Animator>().Play("TipsCollectedShort");
			}
//			tips.GetComponent<Animator>().Play("TipsCollected");
			SoundManager.Instance.Play_TipsCollect();
			if(numberOfTips>0)
				numberOfTips--;




//		Debug.Log("smanjen, ukupno ih ima "+numberOfTips);
		
	}



	/// <summary>
	/// F-ja koja generise novi order
	/// </summary>
	public void GenerateNewOrder()
	{
		EnableInteraction();
		GameObject.Find("Plate").GetComponent<Animator>().Play("IgredientDeparting");
		if(GlobalVariables.GameplayMode==1)
		{
			do
			{
				currentCharacter = UnityEngine.Random.Range(0,maxNumberOfCharachters);
			}
			while(currentCharacter==lastCharacter);
			
			lastCharacter=currentCharacter;
		}

		ingredientsList.Clear();
		sideFoodOrders.Clear();
		sideFoodOrdersBackUp.Clear();
		allPosiblleSideFoodOrders.Clear();
		numberOfSideFoodOrders = 0;
		numberofMainMealOrders=0;
		ResetSideFood();
		ResetMainFood();

		for(int i=0;i<10;i++)
		{
			MainMealOrder.transform.GetChild(i).GetComponent<Image>().sprite = empty;
			DeliveryHolder.transform.GetChild(i).GetComponent<Image>().sprite = empty;
		}
//		if(!firstOrder && LevelGenerator.gameActive)
//		{
//			if(GlobalVariables.GameplayMode==1)
//			{
//				Debug.Log("NOVA MUSTERIJA".Colored(Colors.lime)+" teswte"+LevelGenerator.gameActive);
		Invoke("NewCharacter",0.33f);
//			}
//		}


		mealEarningFinal+=mealPrice;
		mealPrice = 0;


		currentSideOrder = 0;

		for(int i=0; i<maxNumberOfSideFoodOrders;i++)
		{
			allPosiblleSideFoodOrders.Add(i+1);
//			Debug.Log("allPosiblleSideFoodOrders "+allPosiblleSideFoodOrders[i]);
		}



	

		for(int i=0;i<4;i++)
		{
			SideFoodOrder.transform.GetChild(i).GetComponent<Image>().sprite = empty;
		}
		
		currentIndexOfIngredient=0;

		switch(maxNumberOfSideFoodOrders)
		{
		case 0:
			numberOfSideFoodOrders = 0;
			break;
		case 1:
			numberOfSideFoodOrders = UnityEngine.Random.Range (0, maxNumberOfSideFoodOrders+1);
			break;
		case 2:
			numberOfSideFoodOrders = UnityEngine.Random.Range (0, maxNumberOfSideFoodOrders+1);
			break;
		case 3:
			numberOfSideFoodOrders = UnityEngine.Random.Range (0, maxNumberOfSideFoodOrders+1);
			break;
		case 4:
			numberOfSideFoodOrders = UnityEngine.Random.Range (0, maxNumberOfSideFoodOrders+1);
			break;
		}

		if(firstOrder)
		{
			firstOrder = false;
			numberOfSideFoodOrders = 0;
		}


		if(numberOfSideFoodOrders>0)
		{
			numberofMainMealOrders = UnityEngine.Random.Range(0,2);
			numberOfOrders = UnityEngine.Random.Range (0, numberOfSideFoodOrders);
		}
		else
		{
			numberOfOrders = 1;
			numberofMainMealOrders = 1;
		}

		if(lastNumberofMainMealOrders==0 && numberofMainMealOrders ==0)
			numberofMainMealOrders = 1;

		lastNumberofMainMealOrders=numberofMainMealOrders;
//		Debug.Log("numberOfOrders "+numberOfOrders);

		if(numberofMainMealOrders==1)
		{
				MainMealOrded();
		}
		else
		{
			mainFoodFoodChecked = true;
//			Debug.Log("Promenjen je mainFoodFoodChecked ".Colored(Colors.blue)+mainFoodFoodChecked);
		}

		if(numberOfSideFoodOrders>0)
		{
			SideMealOrder(numberOfSideFoodOrders);
		}
		else
		{
			sideFoodChecked = true;
//			Debug.Log("Promenjen je sideFoodChecked ".Colored(Colors.blue)+sideFoodChecked);
		}

//		Debug.Log("Generisan je novi order, mainMeal " + numberofMainMealOrders + " , sideOrderNumber "+numberOfSideFoodOrders);
//		Debug.Log("mainFoodFoodChecked "+mainFoodFoodChecked+" a sideFoodChecked "+sideFoodChecked);



	}

	void NewCharacter()
	{
		Debug.Log("NewCharacter".Colored(Colors.red));
//		currentCharacter = 5;
		switch(currentCharacter)
		{
		case 0:
			do
			{
				character1RandomCurrent = UnityEngine.Random.Range(0,3);
			}
			while(character1RandomCurrent==character1RandomLast);
			character1RandomLast = character1RandomCurrent;
			CharacterChangerObject.GetComponent<CharacterChanger>().ChangeCharacter(character1RandomCurrent);
			break;
		case 1:
			do
			{
				character2RandomCurrent = UnityEngine.Random.Range(3,6);
			}
			while(character2RandomCurrent==character2RandomLast);
			character2RandomLast = character2RandomCurrent;
			CharacterChangerObject.GetComponent<CharacterChanger>().ChangeCharacter(character2RandomCurrent);
			break;
		case 2:
			do
			{
				character3RandomCurrent = UnityEngine.Random.Range(6,9);
			}
			while(character3RandomCurrent==character3RandomLast);
			character3RandomLast = character3RandomCurrent;
			CharacterChangerObject.GetComponent<CharacterChanger>().ChangeCharacter(character3RandomCurrent);
			break;
		case 3:
			do
			{
				character4RandomCurrent = UnityEngine.Random.Range(9,12);
			}
			while(character4RandomCurrent==character4RandomLast);
			character4RandomLast = character4RandomCurrent;
			CharacterChangerObject.GetComponent<CharacterChanger>().ChangeCharacter(character4RandomCurrent);
			break;
		case 4:
			do
			{
				character5RandomCurrent = UnityEngine.Random.Range(12,15);
			}
			while(character5RandomCurrent==character5RandomLast);
			character5RandomLast = character5RandomCurrent;
			CharacterChangerObject.GetComponent<CharacterChanger>().ChangeCharacter(character5RandomCurrent);
			break;
		case 5:
			do
			{
				character6RandomCurrent = UnityEngine.Random.Range(15,18);
			}
			while(character6RandomCurrent==character6RandomLast);
			character6RandomLast = character6RandomCurrent;
			CharacterChangerObject.GetComponent<CharacterChanger>().ChangeCharacter(character6RandomCurrent);
			break;
		case 6:
			do
			{
				character7RandomCurrent = UnityEngine.Random.Range(18,21);
			}
			while(character7RandomCurrent==character7RandomLast);
			character7RandomLast = character7RandomCurrent;
			CharacterChangerObject.GetComponent<CharacterChanger>().ChangeCharacter(character7RandomCurrent);
			break;
		}
//		CharacterChangerObject.GetComponent<CharacterChanger>().ChangeCharacter(currentCharacter);
	}

	void MainMealOrded()
	{
		numberOfIngredientsForMainMeal = UnityEngine.Random.Range(3, maxNumberOfIngredientsPerOrder+1);
		
//		Debug.Log("numberOfIngredientsForMainMeal "+GlobalVariables.numberOfUnlockedIngredients+"/"+numberOfIngredientsForMainMeal);
		
		
		for(int i=0;i<numberOfIngredientsForMainMeal;i++)
		{
			int ingredient=0;
			if(ingredientsList.Count==0)
			{
				ingredientsList.Add(1);
				lastIngredient=1;
//				Debug.Log("Postavlja prvi");
			}
			else if(ingredientsList.Count==numberOfIngredientsForMainMeal-1)
			{
				ingredientsList.Add(2);
//				Debug.Log("Postavlja poslednji");
			}
			else if(ingredientsList.Count==numberOfIngredientsForMainMeal-2)
			{
				do
				{
					ingredient = UnityEngine.Random.Range(3,GlobalVariables.numberOfUnlockedIngredients+1);
//					Debug.Log("ingredient "+ingredient);
				}
				while(lastIngredient==ingredient || ingredient==2);
				
				lastIngredient=ingredient;
//				Debug.Log("Postavlja izmedju");
				ingredientsList.Add(ingredient);
			}
			else
			{
				do
				{
					ingredient = UnityEngine.Random.Range(1,GlobalVariables.numberOfUnlockedIngredients+1);
//					Debug.Log("ingredient "+ingredient);
				}
				while(lastIngredient==ingredient || ingredient==2);
				
				lastIngredient=ingredient;
//				Debug.Log("Postavlja izmedju");
				ingredientsList.Add(ingredient);
			}
			
			MainMealOrder.transform.GetChild(i).GetComponent<Image>().sprite = IngredientsSprites[ingredientsList[i]-1];
//			Debug.Log("Sastojak broj "+i+" je : "+ingredientsList[i]+" a sprite je "+IngredientsSprites[ingredientsList[i]-1].name);
		}

		for(int i=0;i<ingredientsList.Count;i++)
		{
			Debug.Log("Ingredient List ".Colored(Colors.olive)+ingredientsList[i]);
		}
		if(numberofMainMealOrders==1)
		{
			if(hintsPowerActive)
			{
				hintHolder.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
			}
			else if(tutorial)
			{
				hintHolder.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
			}
		}
		else
		{
			Debug.Log("numberofMainMealOrders je 0 ".Colored(Colors.olive)+"00000000000000000000000000 gasi tips za main");
		}
		lastIngredient=99;
	}

	/// <summary>
	/// F-ja koja za odredjeni kliknuti sastojak proverava da li je on sledeci u narudzbenici ili nije, i zavisno od toga korisniku prikazuje takvo stanje
	/// </summary>
	/// <param name="indexOfIngredient">vrednost sastojaka 1-14/param>
	public void ClickedIngredient(int indexOfIngredient)
	{
		if(GlobalVariables.GameplayMode==0)
		{
			if(!championshipEndOrder)
			{

				if(championshipMainOrderIndex==0)
				{
					if(indexOfIngredient == 1)
					{
						championshipallMealsOrder.Add(indexOfIngredient);
						GameObject.Find("TrayHolder/Plate").GetComponent<Animator>().Play("IgredientArrival");
						championshipMainOrderIndex = 1;
						SoundManager.Instance.Play_Ingredient();
						DeliveryHolder.transform.GetChild(championshipMainOrderIndex-1).GetComponent<Image>().sprite = IngredientsSprites[indexOfIngredient-1];
						DeliveryHolder.transform.GetChild(championshipMainOrderIndex-1).GetComponent<Animator>().Play("IgredientArrival");
					}
					else
					{
						SoundManager.Instance.Play_WrongIngredient();
						hintHolder.transform.GetChild(0).GetComponent<Animator>().Play("IngredientChampionshipWrongIdle");
					}
				}
				else if(championshipMainOrderIndex==9)
				{
					if(indexOfIngredient==2)
					{
						championshipallMealsOrder.Add(indexOfIngredient);
						championshipEndOrder = true;
						championshipMainOrderIndex = 10;
						SoundManager.Instance.Play_Ingredient();
						DeliveryHolder.transform.GetChild(championshipMainOrderIndex-1).GetComponent<Image>().sprite = IngredientsSprites[indexOfIngredient-1];
						DeliveryHolder.transform.GetChild(championshipMainOrderIndex-1).GetComponent<Animator>().Play("IgredientArrival");
					}
					else
					{
						SoundManager.Instance.Play_WrongIngredient();
						hintHolder.transform.GetChild(1).GetComponent<Animator>().Play("IngredientChampionshipWrongIdle");
					}
				}
				else
				{
					championshipallMealsOrder.Add(indexOfIngredient);
					if(indexOfIngredient!=2)
					{
						championshipMainOrderIndex ++;
						DeliveryHolder.transform.GetChild(championshipMainOrderIndex-1).GetComponent<Image>().sprite = IngredientsSprites[indexOfIngredient-1];
						DeliveryHolder.transform.GetChild(championshipMainOrderIndex-1).GetComponent<Animator>().Play("IgredientArrival");
						SoundManager.Instance.Play_Ingredient();
					}
					else
					{
						championshipEndOrder = true;
						SoundManager.Instance.Play_Ingredient();
						championshipMainOrderIndex ++;
						DeliveryHolder.transform.GetChild(championshipMainOrderIndex-1).GetComponent<Image>().sprite = IngredientsSprites[indexOfIngredient-1];
						DeliveryHolder.transform.GetChild(championshipMainOrderIndex-1).GetComponent<Animator>().Play("IgredientArrival");
					}
				}
			}
			else
			{
//				Debug.Log("Stavljen je poklopac ne moze vise");
				GameObject.Find("SubmitHolder").GetComponent<Animator>().Play("SubmitPulsing");
			}


		}
		else
		{
			if(!mainFoodFoodChecked )
			{
				

//				Debug.Log("Trt 1 " + ingredientsList.Count+ " a redni broj je "+currentIndexOfIngredient);
//				Debug.Log("ingredientsList "+ingredientsList[currentIndexOfIngredient]+" indexOfIngredient "+indexOfIngredient);
				if(ingredientsList[currentIndexOfIngredient]==indexOfIngredient)
				{
					SoundManager.Instance.Play_Ingredient();
					if(currentIndexOfIngredient==0)
					{
						GameObject.Find("Plate").GetComponent<Animator>().Play("IgredientArrival");
					}
					
					DeliveryHolder.transform.GetChild(currentIndexOfIngredient).GetComponent<Image>().sprite = IngredientsSprites[ingredientsList[currentIndexOfIngredient]-1];
					DeliveryHolder.transform.GetChild(currentIndexOfIngredient).GetComponent<Animator>().Play("IgredientArrival");
					currentIndexOfIngredient++;
					mealPrice++;
					if(currentIndexOfIngredient==ingredientsList.Count)
					{
						mainFoodFoodChecked = true;
						
						hintHolder.transform.GetChild(ingredientsList[currentIndexOfIngredient-1]-1).GetChild(0).GetChild(0).gameObject.SetActive(false);
//						Debug.Log("Promenjen je mainFoodFoodChecked ".Colored(Colors.blue)+mainFoodFoodChecked);
						if(sideFoodChecked)
						{
							
							CorrectHolder.transform.GetChild(1).GetComponent<Image>().sprite = CorrectSprite;
							CorrectHolder.GetComponent<Animator>().Play("CorrectArrival");
							AddPointsToScore(mealPrice);
						
							if(GlobalVariables.GameplayMode==1)
							{
								
								GameObject.Find("CustomerTimerBar").GetComponent<CustomerTimer>().StopCustomerTimer();
								SoundManager.Instance.Play_CustomerHappyDeparting();
								SoundManager.Instance.Stop_CustomerMad();
								Character.Play("CharacterHappyDeparting");
								GameObject.Find("CustomerTimerBar").GetComponent<CustomerTimer>().ResetCustomerTimer();
								CreateTip();
							}
							else if(GlobalVariables.GameplayMode==2)
							{
								//							AddPointsToScore(1);
								GameObject.Find("LevelTimer").GetComponent<LevelTimer>().AddOrRemoveTime(2);
							}
							if(GlobalVariables.GameplayMode!=0)
							{
								DisableInteraction();
								Invoke("GenerateNewOrder",1f);
							}
							FoodDeliveryHolder.GetComponent<Animator>().Play("TrayDeparting");
							
						}
						
					}
					else
					{
//						Debug.Log("EVE GA".Colored(Colors.lime));
						if(currentIndexOfIngredient<ingredientsList.Count)
						{
							if(hintsPowerActive)
							{
								hintHolder.transform.GetChild(ingredientsList[currentIndexOfIngredient-1]-1).GetChild(0).GetChild(0).gameObject.SetActive(false);
								hintHolder.transform.GetChild(ingredientsList[currentIndexOfIngredient]-1).GetChild(0).GetChild(0).gameObject.SetActive(true);
							}
							else if(tutorial)
							{
								hintHolder.transform.GetChild(ingredientsList[currentIndexOfIngredient-1]-1).GetChild(0).GetChild(0).gameObject.SetActive(false);
								hintHolder.transform.GetChild(ingredientsList[currentIndexOfIngredient]-1).GetChild(0).GetChild(0).gameObject.SetActive(true);
							}
						}
					}
					
				}
				else
				{
					if(GlobalVariables.GameplayMode==2)
					{
//						Debug.Log("ODUZMI VREME");
						GameObject.Find("LevelTimer").GetComponent<LevelTimer>().AddOrRemoveTime(-2);
					}
					mealPrice = 0;
					CorrectHolder.transform.GetChild(1).GetComponent<Image>().sprite = IncorectSprite;
					SoundManager.Instance.Play_WrongIngredient();
					CorrectHolder.GetComponent<Animator>().Play("CorrectArrival");
					if(numberofMainMealOrders==1)
					{
						hintHolder.transform.GetChild(ingredientsList[currentIndexOfIngredient]-1).GetChild(0).GetChild(0).gameObject.SetActive(false);
						ResetMainFood();
					}
					if(numberOfSideFoodOrders>0)
					{
						
						currentSideOrder = 0;
						ResetSideFood();
					}
				}
			}
			else
			{
				if(GlobalVariables.GameplayMode==2)
				{
//					Debug.Log("ODUZMI VREME");
					GameObject.Find("LevelTimer").GetComponent<LevelTimer>().AddOrRemoveTime(-2);
				}
//				Debug.Log("EVE GA".Colored(Colors.lime));
				mealPrice = 0;
				CorrectHolder.transform.GetChild(1).GetComponent<Image>().sprite = IncorectSprite;
				SoundManager.Instance.Play_WrongIngredient();
				CorrectHolder.GetComponent<Animator>().Play("CorrectArrival");
				
				if(numberofMainMealOrders==1)
				{
					if(currentIndexOfIngredient<ingredientsList.Count)
					{
						hintHolder.transform.GetChild(ingredientsList[currentIndexOfIngredient]-1).GetChild(0).GetChild(0).gameObject.SetActive(false);
					}
					ResetMainFood();
				}
				
				if(numberOfSideFoodOrders>0)
				{
					currentSideOrder = 0;
					ResetSideFood();
				}
			}
		}

	}

	void SideMealOrder(int numberOfSideFoodOrders)
	{
		for(int i=0;i<numberOfSideFoodOrders;i++)
		{
			int sideFoodNumber = UnityEngine.Random.Range(0,allPosiblleSideFoodOrders.Count);
//			Debug.Log("SideFoodNumber "+allPosiblleSideFoodOrders[sideFoodNumber]);
			sideFoodOrders.Add(allPosiblleSideFoodOrders[sideFoodNumber]);
			allPosiblleSideFoodOrders.RemoveAt(sideFoodNumber);
			
			
//			Debug.Log("SIDE FOOD ORDER ".Colored(Colors.green)+sideFoodOrders[i]);
//			sideFoodOrders.Add(sideFoodNumber);
			SideFoodOrder.transform.GetChild(sideFoodOrders[i]-1).GetComponent<Image>().sprite = SideFoodSprites[sideFoodOrders[i]-1];
		}
		sideFoodOrdersBackUp = new List<int> (sideFoodOrders);

//		for(int i=0;i<numberOfSideFoodOrders;i++)
//		{
//			Debug.Log("SIDE FOOD ORDER ".Colored(Colors.green)+sideFoodOrders[i]);
//			Debug.Log("SIDE FOOD ORDER BACK UP ".Colored(Colors.green)+sideFoodOrdersBackUp[i]);
//		}

		if(numberOfSideFoodOrders>0)
		{
			if(hintsPowerActive)
			{
				sideFoodHintsHolder.transform.GetChild(sideFoodOrders[0]-1).GetChild(0).GetChild(0).gameObject.SetActive(true);
			}
			else if(tutorial)
			{
				sideFoodHintsHolder.transform.GetChild(sideFoodOrders[0]-1).GetChild(0).GetChild(0).gameObject.SetActive(true);
			}
		}
	}

	void CreateTip()
	{
		if(!tipsTutorial)
		{
			LevelGenerator.gameActive = false;
			GameObject.Find("LevelTimer").GetComponent<LevelTimer>().StopTimerDecrease();
			GameObject.Find("ClockHolder").GetComponent<Animator>().speed = 0;
			GameObject.Find("TipsTutorialHolder").GetComponent<Animator>().Play("TipsTutorialArriving");
		}
		GameObject tips = Instantiate(TipsPrefab) as GameObject;
		int tipAmount = CharacterParser.ListOfCharacters[currentCharacter].maxTip;
		Debug.Log("tipAmount "+tipAmount);
		tips.GetComponent<TipsCollect>().tipsAmount = tipAmount;
		numberOfTips++;
//		tips.transform.parent = GameObject.Find("TablePart6").transform;
		if(tipsMasterPowerActive)
		{
			tips.GetComponent<Button>().interactable = false;
		}
		tips.transform.position = tipsPositions[numberOfTips-1];
		
		tips.transform.SetParent(GameObject.Find("Canvas/MainMenu/AnimationHolder/RestorantHolder/Tips").transform,false);





		if(!tipsTutorial)
		{
			GameObject tipsArrow = Instantiate (Resources.Load ("ArrowTips")) as GameObject;
			tipsArrow.transform.position = Vector3.zero;
			tipsArrow.transform.SetParent(tips.transform,false);
			tipsArrow.name = "ArrowTips";
		}
		if(tipsMasterPowerActive)
		{
			StartCoroutine(CleanTips(tips.transform.GetChild(0),1));
		}
//		else
//		{
//			if(tipsTutorial)
//			{
//				StartCoroutine(CleanTips(tips.transform.GetChild(0),5));
//			}
//		}


		
		if(tipAmount<11)
		{
			tips.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
			tips.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
			tips.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
		}
		else if(tipAmount<21)
		{
			tips.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
			tips.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
		}
		else if(tipAmount<26)
		{
			tips.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
		}
		else
			Debug.Log("Kreiraj tip nesto");

		tips.transform.GetChild(0).GetComponent<Animator>().Play("TipsArriving");
	}



	public void ClickedSideFood(int indexOfSideFood)
	{
		//championshipSideOrderIndex
		if(GlobalVariables.GameplayMode==0)
		{
			int newValue = indexOfSideFood+20;
			if(!championshipallMealsOrder.Contains(newValue))
			{
				championshipallMealsOrder.Add(newValue);
				championshipSideOrderIndex++;
				DeliverySideFoodHolder.transform.GetChild(indexOfSideFood).GetComponent<Image>().sprite = SideFoodSprites[indexOfSideFood];
				DeliverySideFoodHolder.transform.GetChild(indexOfSideFood).GetComponent<Animator>().Play("IgredientArrival");
				SoundManager.Instance.Play_SideFood();
			}
		}
		else
		{
			if(sideFoodOrders.Contains(indexOfSideFood+1))
			{
				//			Debug.Log("Taj ga ima side food order "+sideFoodOrders.FindIndex(x=>x==indexOfSideFood));
				sideFoodOrders.RemoveAt(sideFoodOrders.FindIndex(x=>x==indexOfSideFood+1));
				//			SideFoodOrder.transform.GetChild(indexOfSideFood).GetChild(0).gameObject.SetActive(true);
				DeliverySideFoodHolder.transform.GetChild(indexOfSideFood).GetComponent<Image>().sprite = SideFoodSprites[indexOfSideFood];
				DeliverySideFoodHolder.transform.GetChild(indexOfSideFood).GetComponent<Animator>().Play("IgredientArrival");
				SoundManager.Instance.Play_Ingredient();
//				Debug.Log("GASI SIDE FOOD HINT NA POZICIJI "+indexOfSideFood);
				sideFoodHintsHolder.transform.GetChild(indexOfSideFood).GetChild(0).GetChild(0).gameObject.SetActive(false);
				currentSideOrder++;
				mealPrice++;
				if(currentSideOrder == numberOfSideFoodOrders)
				{
					sideFoodChecked = true;
					//				Debug.Log("Promenjen je sideFoodChecked ".Colored(Colors.blue)+sideFoodChecked);
					if(mainFoodFoodChecked)
					{
						
						CorrectHolder.transform.GetChild(1).GetComponent<Image>().sprite = CorrectSprite;
						CorrectHolder.GetComponent<Animator>().Play("CorrectArrival");
						AddPointsToScore(mealPrice);
						if(GlobalVariables.GameplayMode==1)
						{
							
							GameObject.Find("CustomerTimerBar").GetComponent<CustomerTimer>().StopCustomerTimer();
							SoundManager.Instance.Stop_CustomerMad();
							SoundManager.Instance.Play_CustomerHappyDeparting();
							Character.Play("CharacterHappyDeparting");
							GameObject.Find("CustomerTimerBar").GetComponent<CustomerTimer>().ResetCustomerTimer();
							CreateTip();
						}
						else if(GlobalVariables.GameplayMode==2)
						{
							//						AddPointsToScore(1);
							GameObject.Find("LevelTimer").GetComponent<LevelTimer>().AddOrRemoveTime(2);
						}
						if(GlobalVariables.GameplayMode!=0)
						{
							DisableInteraction();
							Invoke("GenerateNewOrder",1f);
						}
						FoodDeliveryHolder.GetComponent<Animator>().Play("TrayDeparting");
						
					}
					
				}
				else
				{
					if(hintsPowerActive)
					{
//						sideFoodHintsHolder.transform.GetChild(sideFoodOrders[0]).GetChild(0).GetChild(0).gameObject.SetActive(false);
						sideFoodHintsHolder.transform.GetChild(sideFoodOrders[0]-1).GetChild(0).GetChild(0).gameObject.SetActive(true);
					}
					else if(tutorial)
					{
						sideFoodHintsHolder.transform.GetChild(sideFoodOrders[0]-1).GetChild(0).GetChild(0).gameObject.SetActive(true);
					}
				}
				//			sideFoodOrders.RemoveAt(sideFoodOrders.FindIndex(x=>x==indexOfSideFood));
			}
			else
			{
				if(GlobalVariables.GameplayMode==2)
				{
//					Debug.Log("ODUZMI VREME");
					GameObject.Find("LevelTimer").GetComponent<LevelTimer>().AddOrRemoveTime(-2);
				}
				CorrectHolder.transform.GetChild(1).GetComponent<Image>().sprite = IncorectSprite;
				SoundManager.Instance.Play_WrongIngredient();
				CorrectHolder.GetComponent<Animator>().Play("CorrectArrival");
				SoundManager.Instance.Play_WrongIngredient();
				mealPrice = 0;
				//			Debug.Log("Nije taj side food order");
				currentSideOrder = 0;
				if(numberofMainMealOrders==1)
				{
//					hintHolder.transform.GetChild(ingredientsList[currentIndexOfIngredient]-1).GetChild(0).GetChild(0).gameObject.SetActive(false);
					ResetMainFood();
				}
				if(numberOfSideFoodOrders>0)
				{
					ResetSideFood();
				}
			}
		}



	}

	void ResetSideFood()
	{
		Debug.Log("ResetSideFood".Colored(Colors.red));
		sideFoodChecked = false;
		sideFoodOrders = new List<int> (sideFoodOrdersBackUp);

		for(int i=0;i<4;i++)
		{
			//			SideFoodOrder.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
			DeliverySideFoodHolder.transform.GetChild(i).GetComponent<Image>().sprite = empty;
			DeliverySideFoodHolder.transform.GetChild(i).GetComponent<Animator>().Play("New State");
		}

		if(hintsPowerActive)
		{
			if(numberOfSideFoodOrders>0)
			{
					sideFoodHintsHolder.transform.GetChild(sideFoodOrders[0]-1).GetChild(0).GetChild(0).gameObject.SetActive(true);
			}
			else
			{
				for(int i=0;i<4;i++)
				{
					sideFoodHintsHolder.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(false);
				}
			}
		}
		else if(tutorial)
		{
			if(numberOfSideFoodOrders>0)
			{
				sideFoodHintsHolder.transform.GetChild(sideFoodOrders[0]-1).GetChild(0).GetChild(0).gameObject.SetActive(true);
			}
			else
			{
				for(int i=0;i<4;i++)
				{
					sideFoodHintsHolder.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(false);
				}
			}
		}
	}

	void ResetMainFood()
	{
			Debug.Log("ResetMainFood".Colored(Colors.red));
			mainFoodFoodChecked = false;
			
//			Debug.Log("Promenjen je mainFoodFoodChecked ".Colored(Colors.blue)+mainFoodFoodChecked);
			for(int i=0;i<10;i++)
			{
				DeliveryHolder.transform.GetChild(i).GetComponent<Image>().sprite = empty;
				DeliveryHolder.transform.GetChild(i).GetComponent<Animator>().Play("New State");
			}
			currentIndexOfIngredient=0;
		if(hintsPowerActive)
		{
			if(numberofMainMealOrders==1)
			{
				Debug.Log("ResetMainFood numberofMainMealOrders==1".Colored(Colors.red));
				hintHolder.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
			}
			else
			{
				Debug.Log("ResetMainFood numberofMainMealOrders==0".Colored(Colors.red));
				for(int i=0;i<14;i++)
				{
					hintHolder.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(false);
				}
			}
		}
		else if(tutorial)
		{
			if(numberofMainMealOrders==1)
			{
				
				hintHolder.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
			}
			else
			{
				for(int i=0;i<14;i++)
				{
					hintHolder.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(false);
				}
			}
		}
			
	}

	#if UNITY_EDITOR
	public static void ClearLog()
	{
		Assembly assembly = Assembly.GetAssembly(typeof(UnityEditor.ActiveEditorTracker));
		System.Type type = assembly.GetType("UnityEditorInternal.LogEntries");
		MethodInfo method = type.GetMethod("Clear");
		method.Invoke(new object(), null);
	}
	#endif

	public void FoodDeliveryStart()
	{
		FoodDeliveryHolder.GetComponent<Animator>().Play("TrayArriving");
	}

	public void SetUnlockMessage(string title, string message, Sprite image)
	{
//		Debug.Log("OTKLJUCAJ ELEMENT".Colored(Colors.red));
		titleUnlock.text = title;
		messageUnlock.text = message;
		imageUnlock.sprite = image;
	}

	public void AddPointsToScore(int amount)
	{
		Debug.Log("POZIVA SE AddPointsToScore ".Colored(Colors.lightblue)+amount);
		if(GlobalVariables.GameplayMode==1)
		{
			if(score<LevelsParser.ListOfLevels[GlobalVariables.currentLevel-1].levelGoal && (score+amount)>=LevelsParser.ListOfLevels[GlobalVariables.currentLevel-1].levelGoal)
				GameObject.Find("GoalHolder").GetComponent<Animator>().Play("GoalReach");
		}

		score +=amount;
		if(GlobalVariables.doubleCoins)
		{
//			Debug.Log("Duplo od "+amount+" je "+(amount*2));
			amount *=2;

		}
		else
		{
//			Debug.Log("Nema duplo od "+amount+" a bilo bi "+(amount*2));
		}
		GlobalVariables.coins +=amount;
		GameObject.Find("CoinsText").GetComponent<Text>().text = GlobalVariables.coins.ToString();
		PlayerPrefs.SetInt("coins", GlobalVariables.coins);
		PlayerPrefs.Save();
		SoundManager.Instance.Play_Coins();
//		StartCoroutine(GlobalVariables.Instance.moneyCounter(amount , GameObject.Find("CoinsText").GetComponent<Text>()));
		if(GlobalVariables.GameplayMode==1)
		{
			if(score>=LevelsParser.ListOfLevels[GlobalVariables.currentLevel-1].levelGoal)
			{
				GameObject.Find("MainGoalText").GetComponent<Text>().text = "<color=green>"+score+"</color>"+"/"+LevelsParser.ListOfLevels[GlobalVariables.currentLevel-1].levelGoal.ToString();
			}
			else
			{
				GameObject.Find("MainGoalText").GetComponent<Text>().text =score+"/"+LevelsParser.ListOfLevels[GlobalVariables.currentLevel-1].levelGoal.ToString();
			}
			
		}
		else if(GlobalVariables.GameplayMode==2)
		{
			GameObject.Find("MainGoalText").GetComponent<Text>().text =score.ToString();
		}
	}

	public void SetGoalPopUp()
	{

//		if(GlobalVariables.GameplayMode==0)
//		{
//			Debug.Log("Chef Championship MODE GOAL PODESAVANJE".Colored(Colors.cyan));
//			GameObject.Find("TextHeaderGoalLevel").GetComponent<Text>().text = "Chef Championship";
//			GameObject.Find("MainGoalText").GetComponent<Text>().text = "0";
//		}
		/*else*/ if(GlobalVariables.GameplayMode==1)
		{
			GameObject.Find("TextHeaderGoalLevel").GetComponent<Text>().text = "LEVEL "+GlobalVariables.currentLevel.ToString();
			GameObject.Find("StartGoalText").GetComponent<Text>().text =LevelsParser.ListOfLevels[GlobalVariables.currentLevel-1].levelGoal.ToString();
			GameObject.Find("MainGoalText").GetComponent<Text>().text ="0/"+LevelsParser.ListOfLevels[GlobalVariables.currentLevel-1].levelGoal.ToString();

			if(LevelsParser.ListOfLevels[GlobalVariables.currentLevel-1].levelUnlockedItem!=0)
			{
				if(GlobalVariables.numberOfWorldSelected == 1)
				{
					SetUnlockMessage(LevelsParser.ListOfLevels[GlobalVariables.currentLevel-1].levelUnlockedTitleWorld1, LevelsParser.ListOfLevels[GlobalVariables.currentLevel-1].levelUnlockedMessageWorld1, UnlockedSpritesWorld1[LevelsParser.ListOfLevels[GlobalVariables.currentLevel-1].levelUnlockedItem-1]);
				}
				else if(GlobalVariables.numberOfWorldSelected == 2)
				{
					SetUnlockMessage(LevelsParser.ListOfLevels[GlobalVariables.currentLevel-1].levelUnlockedTitleWorld2, LevelsParser.ListOfLevels[GlobalVariables.currentLevel-1].levelUnlockedMessageWorld2, UnlockedSpritesWorld2[LevelsParser.ListOfLevels[GlobalVariables.currentLevel-1].levelUnlockedItem-1]);
				}
				else if(GlobalVariables.numberOfWorldSelected == 0)
				{
					SetUnlockMessage(LevelsParser.ListOfLevels[GlobalVariables.currentLevel-1].levelUnlockedTitleWorld3, LevelsParser.ListOfLevels[GlobalVariables.currentLevel-1].levelUnlockedMessageWorld3, UnlockedSpritesWorld3[LevelsParser.ListOfLevels[GlobalVariables.currentLevel-1].levelUnlockedItem-1]);
				}
			}

//			Debug.Log("Unlock je :".Colored(Colors.red)+LevelsParser.ListOfLevels[GlobalVariables.currentLevel-1].levelUnlockedItem+" idemoooooo".Colored(Colors.blue));

		}
		else if(GlobalVariables.GameplayMode==2)
		{
//			Debug.Log("TimeAttack MODE  GOAL PODESAVANJE".Colored(Colors.cyan));
			GameObject.Find("TextHeaderGoalLevel").GetComponent<Text>().text = "Time Attack";
			GameObject.Find("MainGoalText").GetComponent<Text>().text = "0";
		}
//		Debug.Log("SVET JE "+GlobalVariables.numberOfWorldSelected);
	}
	public void StartGameChange()
	{
		gameStarted = true;
	}

	public void StartGame()
	{
		StartCoroutine("StartGameCoorutine");
	}
	
	IEnumerator StartGameCoorutine()
	{

		if(GlobalVariables.GameplayMode!=0)
		{
			GenerateNewOrder();
		}
		yield return new WaitForSeconds(1.5f);
		if(GlobalVariables.GameplayMode!=0)
		{
			GameObject.Find("ClockHolder").GetComponent<Animator>().Play("ClockIdle");
		}

			LevelGenerator.gameActive = true;
		Invoke ("FoodDeliveryStart",0.5f);

		if(GlobalVariables.GameplayMode==1)
		{
			Character.Play("CharacterIArriving");
			LevelGenerator.customerActive = true;
			GameObject.Find("CustomerTimerBar").GetComponent<CustomerTimer>().StartCustomerTimer();



		}
		Debug.Log ("maxNumberOfSideFoodOrders " + maxNumberOfSideFoodOrders);
		if(maxNumberOfSideFoodOrders>0)
		{
			//			Debug.Log("Vise "+maxNumberOfSideFoodOrders);
			
			for(int i=0; i<maxNumberOfSideFoodOrders;i++)
			{
				//					int numberOfSideFoodHolder = i+1;
				//					sideFoodHintsHolder.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
				//					sideFoodHintsHolder.transform.GetChild(i).GetChild(0).GetComponent<Image>().color = Color.white;
				sideFoodHintsHolder.transform.GetChild(i).GetChild(0).GetComponent<Button>().interactable = true;
				//					sideFoodHintsHolder.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
			}
		}
		Debug.Log ("GlobalVariables.numberOfUnlockedIngredients " + GlobalVariables.numberOfUnlockedIngredients);
		for(int i=0;i<GlobalVariables.numberOfUnlockedIngredients;i++)
		{
			//				hintHolder.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
			//				hintHolder.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
			//				hintHolder.transform.GetChild(i).GetChild(0).GetComponent<Image>().color = Color.white;
			hintHolder.transform.GetChild(i).GetChild(0).GetComponent<Button>().interactable = true;
			
		}

		if(GlobalVariables.GameplayMode!=0)
		{
			GameObject.Find("LevelTimer").GetComponent<LevelTimer>().StartLevelTimer();
		}

		ActivatePauseButton();
	}

	void ActivatePauseButton()
	{
		GameObject.Find("ButtonPause").GetComponent<Button>().interactable = true;
	}

	public void ActivateHintPowerUp()
	{
		if(!hintsPowerActive)
		{
			if(hintPowerPrice<=GlobalVariables.coins)
			{

				SoundManager.Instance.Play_Boost();
				BuyStartPowerUp(-hintPowerPrice);
				hintsPowerActive = true;
				GameObject.Find("PowerslHolder").transform.GetChild(0).GetChild(4).gameObject.SetActive(true);

				#if UNITY_ANDROID
				try
				{
					using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) 
					{
						using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) 
						{
							obj_Activity.Call("FlurryLevelStartBoostUsed","HintPower");
						}
					} 
					return;
				}
				catch
				{
					Debug.Log("Error Contacting Android - HintPower! ");
					return;
				}
				#endif
			}
			else
			{
				SoundManager.Instance.Play_NoMoney();
				GameObject.Find("CoinsHolder").GetComponent<Animator>().Play("NotEnoughCoins");
			}

		}

	}

	public void ActivateTipMasterPowerUp()
	{
		if(!tipsMasterPowerActive)
		{
			if(tipsMasterPowerPrice<=GlobalVariables.coins)
			{

				SoundManager.Instance.Play_Boost();
				BuyStartPowerUp(-tipsMasterPowerPrice);
				tipsMasterPowerActive = true;
				GameObject.Find("PowerslHolder").transform.GetChild(1).GetChild(4).gameObject.SetActive(true);

				#if UNITY_ANDROID
				try
				{
					using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) 
					{
						using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) 
						{
							obj_Activity.Call("FlurryLevelStartBoostUsed","TipMasterPower");
						}
					} 
					return;
				}
				catch
				{
					Debug.Log("Error Contacting Android - TipMasterPower! ");
					return;
				}
				#endif
			}
			else
			{
				SoundManager.Instance.Play_NoMoney();
				GameObject.Find("CoinsHolder").GetComponent<Animator>().Play("NotEnoughCoins");
			}

		}
	}

	public void ActivateHappyTimePowerUp()
	{
		if(!happyTimePowerActive)
		{
			if(happyTimePowerPrice<=GlobalVariables.coins)
			{

				SoundManager.Instance.Play_Boost();
				BuyStartPowerUp(-happyTimePowerPrice);
				happyTimePowerActive = true;
				GameObject.Find("CustomerTimer").GetComponent<Animator>().Play("CustomerTimerIdle");
				GameObject.Find("PowerslHolder").transform.GetChild(2).GetChild(4).gameObject.SetActive(true);

				#if UNITY_ANDROID
				try
				{
					using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) 
					{
						using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) 
						{
							obj_Activity.Call("FlurryLevelStartBoostUsed","HappyTimePower");
						}
					} 
					return;
				}
				catch
				{
					Debug.Log("Error Contacting Android - HappyTimePower! ");
					return;
				}
				#endif
			}
			else
			{
				SoundManager.Instance.Play_NoMoney();
				GameObject.Find("CoinsHolder").GetComponent<Animator>().Play("NotEnoughCoins");
			}

		}
	}

	public void BuyStartPowerUp(int numberOfCoins)
	{
		StartCoroutine(GlobalVariables.Instance.moneyCounter(numberOfCoins, GameObject.Find("NumberOfCoinsGoal").GetComponent<Text>()));

		CoinsNumberText.text = GlobalVariables.coins.ToString();
	}

	public void StartLoadingSceneFromGameplay(string msg)
	{
		if(winGame)
			GameObject.Find("WinHolder").transform.FindChild("AnimationHolder/SparklesMain").gameObject.SetActive(false);
		score = 0;
		winGame=false;
		AnimEvents.startShowned = false;
		hintsPowerActive = false;
		tipsMasterPowerActive =false;
		happyTimePowerActive = false;
		GameObject.Find("HeadBG").transform.FindChild("ParticlesHolder/SmokeParticleRight").gameObject.SetActive(false);
		GameObject.Find("HeadBG").transform.FindChild("ParticlesHolder/SmokeParticleLeft").gameObject.SetActive(false);
		if(GlobalVariables.GameplayMode!=0)
		{
			GameObject.Find("ClockHolder").transform.FindChild("AnimationHolder/SmokeHolder/SmokeParticleMidle").gameObject.SetActive(false);
		}
		for(int i=0;i<14;i++)
		{
			GameObject.Find("IngredientsHolder").transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(false);
		}
		
		for(int i=0;i<4;i++)
		{
			GameObject.Find("SideFoodsHolder").transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(false);
		}
		SoundManager.Instance.Play_LoadingArrival();
		switch(msg.ToLower())
		{
//			SceneToLoad = 1; // 0 - MainScene, 1 - GamePlay
		case "restart":
//			Debug.Log("RESTART JE".Colored(Colors.fuchsia));
			break;
		case "home":
			GlobalVariables.numberOfWorldSelected = 1;
			LevelGenerator.videoChampWatched = false;
			SoundManager.Instance.Stop_GameplayMusic();
			GlobalVariables.SceneToLoad = 0;
			GlobalVariables.GameplayMode = 1;
//			Debug.Log("HOME JE".Colored(Colors.fuchsia));
			break;
		case "next":
			GlobalVariables.currentLevel++;
//			Debug.Log("NEXT JE".Colored(Colors.fuchsia));
			break;
		}
		LevelGenerator.gameActive = false;
		isMadCustomer = false;
		GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("LoadingHolder").gameObject);
		GameObject.Find("Canvas/PopUps").transform.FindChild("LoadingHolder/LoadingHolder/AnimationHolder").GetComponent<Animator>().Play("LoadingGameplayArriving");
	}

	public void SubmitMeal()
	{
		AdsManager.Instance.IsVideoRewardAvailable();
		if(championshipallMealsOrder.Count>0)
		{
			if(videoChampAvailable)
			{
				GameObject.Find("Canvas/PopUps").transform.FindChild("ChefChampionshipMenu/AnimationHolder/WatchVideoHolder").gameObject.SetActive(true);
			}
			else
			{
				GameObject.Find("Canvas/PopUps").transform.FindChild("ChefChampionshipMenu/AnimationHolder/WaitingForJudgesHolder").gameObject.SetActive(true);
				StartCoroutine("WaitForJudges");
			}
			GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("ChefChampionshipMenu").gameObject);
			GenerateLeaderBoardFood(videoChampWatched);
		}
		else
		{
			hintHolder.transform.GetChild(0).GetComponent<Animator>().Play("IngredientChampionshipWrongIdle");
		}
	}

	IEnumerator WaitForJudges()
	{
		yield return new WaitForSeconds(0.5f);
//		Debug.Log("Upali se bre");
		GameObject.Find("Canvas/PopUps").transform.FindChild("ChefChampionshipMenu/AnimationHolder/WaitingForJudgesHolder").GetComponent<Animator>().Play("WaitingForJudgesArriving");
		yield return new WaitForSeconds(5f);
		GameObject.Find("Canvas/PopUps").transform.FindChild("ChefChampionshipMenu/AnimationHolder/WaitingForJudgesHolder").GetComponent<Animator>().Play("WaitingForJudgesDeparting");

	}

	public void UndoMeal()
	{
		if(championshipallMealsOrder.Count>0)
		{
			StartCoroutine("UndoMealCoorutine");
		}

	}

	IEnumerator UndoMealCoorutine()
	{
		Debug.Log("Trenutno je "+championshipallMealsOrder[championshipallMealsOrder.Count-1]);
		if(championshipallMealsOrder[championshipallMealsOrder.Count-1]<15)
		{
			if(championshipEndOrder)
				championshipEndOrder=false;
			DeliveryHolder.transform.GetChild(championshipMainOrderIndex-1).GetComponent<Animator>().Play("IgredientDeparting");
			yield return new WaitForSeconds(0.15f);
			DeliveryHolder.transform.GetChild(championshipMainOrderIndex-1).GetComponent<Image>().sprite = empty;
			championshipallMealsOrder.RemoveAt(championshipallMealsOrder.Count-1);
			championshipMainOrderIndex--;
		}
		else
		{
			switch(championshipallMealsOrder[championshipallMealsOrder.Count-1])
			{
			case 20:
				DeliverySideFoodHolder.transform.GetChild(0).GetComponent<Animator>().Play("IgredientDeparting");
				yield return new WaitForSeconds(0.15f);
				DeliverySideFoodHolder.transform.GetChild(0).GetComponent<Image>().sprite = empty;
				championshipallMealsOrder.RemoveAt(championshipallMealsOrder.Count-1);
				championshipSideOrderIndex--;
				break;
			case 21:
				DeliverySideFoodHolder.transform.GetChild(1).GetComponent<Animator>().Play("IgredientDeparting");
				yield return new WaitForSeconds(0.15f);
				DeliverySideFoodHolder.transform.GetChild(1).GetComponent<Image>().sprite = empty;
				championshipallMealsOrder.RemoveAt(championshipallMealsOrder.Count-1);
				championshipSideOrderIndex--;
				break;
			case 22:
				DeliverySideFoodHolder.transform.GetChild(2).GetComponent<Animator>().Play("IgredientDeparting");
				yield return new WaitForSeconds(0.15f);
				DeliverySideFoodHolder.transform.GetChild(2).GetComponent<Image>().sprite = empty;
				championshipallMealsOrder.RemoveAt(championshipallMealsOrder.Count-1);
				championshipSideOrderIndex--;
				break;
			case 23:
				DeliverySideFoodHolder.transform.GetChild(3).GetComponent<Animator>().Play("IgredientDeparting");
				yield return new WaitForSeconds(0.15f);
				DeliverySideFoodHolder.transform.GetChild(3).GetComponent<Image>().sprite = empty;
				championshipallMealsOrder.RemoveAt(championshipallMealsOrder.Count-1);
				championshipSideOrderIndex--;
				break;
			}

		}

	}

	public void CloseWatchVideoReview()
	{
		GameObject.Find("ChefChampionshipMenu/AnimationHolder/WatchVideoHolder").GetComponent<Animator>().Play("WatchVideoDeparting");
		GameObject.Find("Canvas/PopUps").transform.FindChild("ChefChampionshipMenu/AnimationHolder/WaitingForJudgesHolder").gameObject.SetActive(true);
		StartCoroutine("WaitForJudges");
	}

	public void GenerateLeaderBoardFood(bool videoWatched)
	{
		float rndNum = UnityEngine.Random.Range(0f,1f);
		float percFirstPlace = 0.60f;
		float percSecondPlace = 0.85f;
		if(videoWatched)
		{
			percFirstPlace = 0.85f;
			percSecondPlace = 0.98f;
		}

		if(rndNum<=percFirstPlace)
		{
//			Debug.Log("PRVO MESTO "+rndNum);
			for(int i=0;i<10;i++)
			{
				GameObject.Find("FirstPlaceHolder/FoodHolder/DeliveryHolderPlace").transform.GetChild(i).GetComponent<Image>().sprite = DeliveryHolder.transform.GetChild(i).GetComponent<Image>().sprite;
//				Debug.Log("Ime mu je "+DeliveryHolder.transform.GetChild(i).GetComponent<Image>().sprite.name);
			}
			for(int j=0;j<4;j++)
			{
				GameObject.Find("FirstPlaceHolder/FoodHolder/DeliverySideFoodHolderPlace").transform.GetChild(j).GetComponent<Image>().sprite = DeliverySideFoodHolder.transform.GetChild(j).GetComponent<Image>().sprite;
			}
			GlobalVariables.coins += 500;
			GameObject.Find("Name1").GetComponent<Text>().text = "YOU";
			GameObject.Find("Name2").GetComponent<Text>().text = GetRandomName();
			GameObject.Find("Name3").GetComponent<Text>().text = GetRandomName();

			int rndScdNumIngr = UnityEngine.Random.Range(5,9);
			int rndThrdNumIngr = UnityEngine.Random.Range(5,9);

			int rndScdNumSideIngr = UnityEngine.Random.Range(0,5);
			int rndThrdNumSideIngr = UnityEngine.Random.Range(0,5);
			GameObject.Find("SecondPlaceHolder/FoodHolder/DeliveryHolderPlace").transform.GetChild(0).GetComponent<Image>().sprite = IngredientsSprites[0];
			GameObject.Find("ThirdPlaceHolder/FoodHolder/DeliveryHolderPlace").transform.GetChild(0).GetComponent<Image>().sprite = IngredientsSprites[0];
			for(int i=1;i<rndScdNumIngr;i++)
			{
				GameObject.Find("SecondPlaceHolder/FoodHolder/DeliveryHolderPlace").transform.GetChild(i).GetComponent<Image>().sprite = IngredientsSprites[UnityEngine.Random.Range(2,11)];
			}
			GameObject.Find("SecondPlaceHolder/FoodHolder/DeliveryHolderPlace").transform.GetChild(rndScdNumIngr).GetComponent<Image>().sprite = IngredientsSprites[1];
			for(int j=0;j<rndScdNumSideIngr;j++)
			{
				GameObject.Find("SecondPlaceHolder/FoodHolder/DeliverySideFoodHolderPlace").transform.GetChild(j).GetComponent<Image>().sprite = SideFoodSprites[j];
			}

			for(int i=1;i<rndThrdNumIngr;i++)
			{
				GameObject.Find("ThirdPlaceHolder/FoodHolder/DeliveryHolderPlace").transform.GetChild(i).GetComponent<Image>().sprite = IngredientsSprites[UnityEngine.Random.Range(2,11)];
			}
			GameObject.Find("ThirdPlaceHolder/FoodHolder/DeliveryHolderPlace").transform.GetChild(rndThrdNumIngr).GetComponent<Image>().sprite = IngredientsSprites[1];
			for(int j=0;j<rndThrdNumSideIngr;j++)
			{
				GameObject.Find("ThirdPlaceHolder/FoodHolder/DeliverySideFoodHolderPlace").transform.GetChild(j).GetComponent<Image>().sprite = SideFoodSprites[j];
			}
		}
		else if(rndNum<=percSecondPlace)
		{
//			Debug.Log("DRUGO MESTO "+rndNum);
			for(int i=0;i<10;i++)
			{
				GameObject.Find("SecondPlaceHolder/FoodHolder/DeliveryHolderPlace").transform.GetChild(i).GetComponent<Image>().sprite = DeliveryHolder.transform.GetChild(i).GetComponent<Image>().sprite;
//				Debug.Log("Ime mu je "+DeliveryHolder.transform.GetChild(i).GetComponent<Image>().sprite.name);
			}
			for(int j=0;j<4;j++)
			{
				GameObject.Find("SecondPlaceHolder/FoodHolder/DeliverySideFoodHolderPlace").transform.GetChild(j).GetComponent<Image>().sprite = DeliverySideFoodHolder.transform.GetChild(j).GetComponent<Image>().sprite;
			}
			GlobalVariables.coins += 100;
			GameObject.Find("Name1").GetComponent<Text>().text = GetRandomName();
			GameObject.Find("Name2").GetComponent<Text>().text = "YOU";
			GameObject.Find("Name3").GetComponent<Text>().text = GetRandomName();

			int rndFstNumIngr = UnityEngine.Random.Range(5,9);
			int rndThrdNumIngr = UnityEngine.Random.Range(5,9);
			
			int rndFstNumSideIngr = UnityEngine.Random.Range(0,5);
			int rndThrdNumSideIngr = UnityEngine.Random.Range(0,5);
			GameObject.Find("FirstPlaceHolder/FoodHolder/DeliveryHolderPlace").transform.GetChild(0).GetComponent<Image>().sprite = IngredientsSprites[0];
			GameObject.Find("ThirdPlaceHolder/FoodHolder/DeliveryHolderPlace").transform.GetChild(0).GetComponent<Image>().sprite = IngredientsSprites[0];
			for(int i=1;i<rndFstNumIngr;i++)
			{
				GameObject.Find("FirstPlaceHolder/FoodHolder/DeliveryHolderPlace").transform.GetChild(i).GetComponent<Image>().sprite = IngredientsSprites[UnityEngine.Random.Range(2,11)];
			}
			GameObject.Find("FirstPlaceHolder/FoodHolder/DeliveryHolderPlace").transform.GetChild(rndFstNumIngr).GetComponent<Image>().sprite = IngredientsSprites[1];
			for(int j=0;j<rndFstNumSideIngr;j++)
			{
				GameObject.Find("FirstPlaceHolder/FoodHolder/DeliverySideFoodHolderPlace").transform.GetChild(j).GetComponent<Image>().sprite = SideFoodSprites[j];
			}
			
			for(int i=1;i<rndThrdNumIngr;i++)
			{
				GameObject.Find("ThirdPlaceHolder/FoodHolder/DeliveryHolderPlace").transform.GetChild(i).GetComponent<Image>().sprite = IngredientsSprites[UnityEngine.Random.Range(2,11)];
			}
			GameObject.Find("ThirdPlaceHolder/FoodHolder/DeliveryHolderPlace").transform.GetChild(rndThrdNumIngr).GetComponent<Image>().sprite = IngredientsSprites[1];
			for(int j=0;j<rndThrdNumSideIngr;j++)
			{
				GameObject.Find("ThirdPlaceHolder/FoodHolder/DeliverySideFoodHolderPlace").transform.GetChild(j).GetComponent<Image>().sprite = SideFoodSprites[j];
			}
		}
		else
		{
//			Debug.Log("TRECE MESTO "+rndNum);
			for(int i=0;i<10;i++)
			{
				GameObject.Find("ThirdPlaceHolder/FoodHolder/DeliveryHolderPlace").transform.GetChild(i).GetComponent<Image>().sprite = DeliveryHolder.transform.GetChild(i).GetComponent<Image>().sprite;
//				Debug.Log("Ime mu je "+DeliveryHolder.transform.GetChild(i).GetComponent<Image>().sprite.name);
			}
			for(int j=0;j<4;j++)
			{
				GameObject.Find("ThirdPlaceHolder/FoodHolder/DeliverySideFoodHolderPlace").transform.GetChild(j).GetComponent<Image>().sprite = DeliverySideFoodHolder.transform.GetChild(j).GetComponent<Image>().sprite;
			}
			GlobalVariables.coins += 50;
			GameObject.Find("Name1").GetComponent<Text>().text = GetRandomName();
			GameObject.Find("Name2").GetComponent<Text>().text = GetRandomName();
			GameObject.Find("Name3").GetComponent<Text>().text = "YOU";

			int rndFstNumIngr = UnityEngine.Random.Range(5,9);
			int rndScdNumIngr = UnityEngine.Random.Range(5,9);
			
			int rndFstNumSideIngr = UnityEngine.Random.Range(0,5);
			int rndScdNumSideIngr = UnityEngine.Random.Range(0,5);
			GameObject.Find("FirstPlaceHolder/FoodHolder/DeliveryHolderPlace").transform.GetChild(0).GetComponent<Image>().sprite = IngredientsSprites[0];
			GameObject.Find("SecondPlaceHolder/FoodHolder/DeliveryHolderPlace").transform.GetChild(0).GetComponent<Image>().sprite = IngredientsSprites[0];
			for(int i=1;i<rndFstNumIngr;i++)
			{
				GameObject.Find("FirstPlaceHolder/FoodHolder/DeliveryHolderPlace").transform.GetChild(i).GetComponent<Image>().sprite = IngredientsSprites[UnityEngine.Random.Range(2,11)];
			}
			GameObject.Find("FirstPlaceHolder/FoodHolder/DeliveryHolderPlace").transform.GetChild(rndFstNumIngr).GetComponent<Image>().sprite = IngredientsSprites[1];
			for(int j=0;j<rndFstNumSideIngr;j++)
			{
				GameObject.Find("FirstPlaceHolder/FoodHolder/DeliverySideFoodHolderPlace").transform.GetChild(j).GetComponent<Image>().sprite = SideFoodSprites[j];
			}
			
			for(int i=1;i<rndScdNumIngr;i++)
			{
				GameObject.Find("SecondPlaceHolder/FoodHolder/DeliveryHolderPlace").transform.GetChild(i).GetComponent<Image>().sprite = IngredientsSprites[UnityEngine.Random.Range(2,11)];
			}
			GameObject.Find("SecondPlaceHolder/FoodHolder/DeliveryHolderPlace").transform.GetChild(rndScdNumIngr).GetComponent<Image>().sprite = IngredientsSprites[1];
			for(int j=0;j<rndScdNumSideIngr;j++)
			{
				GameObject.Find("SecondPlaceHolder/FoodHolder/DeliverySideFoodHolderPlace").transform.GetChild(j).GetComponent<Image>().sprite = SideFoodSprites[j];
			}

		}
		PlayerPrefs.SetFloat("ChefTimer", 7200);
		string timeQuitString = DateTime.Now.ToString();
		PlayerPrefs.SetInt("ChefPlayed",1);
		PlayerPrefs.SetString("VremeQuit", timeQuitString);
		PlayerPrefs.SetInt("coins", GlobalVariables.coins);
		PlayerPrefs.Save();
//		if(videoWatched)
//		{
//			for(int i=0;i<10;i++)
//			{
//				GameObject.Find("FirstPlaceHolder/FoodHolder/DeliveryHolderPlace").transform.GetChild(i).GetComponent<Image>().sprite = DeliveryHolder.transform.GetChild(i).GetComponent<Image>().sprite;
//			}
//
//		}
//		else
//		{
//
//		}
	}

	string GetRandomName()
	{
		int rndGirlBoy = UnityEngine.Random.Range(0,2);
		int rndNameNum = UnityEngine.Random.Range(0,15);

		if(rndGirlBoy==0)
		{
			return boyNames[rndNameNum];
		}
		else
		{
			return girlNames[rndNameNum];
		}

	}

	public void HideHints()
	{
		StartCoroutine("HideHintsCoorutine");
	}

	IEnumerator HideHintsCoorutine()
	{
		yield return new WaitForSeconds(0.35f);
		hintHolder.SetActive(false);
	}

	public void ActivateHints()
	{
		StartCoroutine("ActivateHintsCoorutine");
	}

	IEnumerator ActivateHintsCoorutine()
	{
		yield return new WaitForSeconds(0.35f);
		hintHolder.SetActive(true);
	}

	void DisableInteraction()
	{
		Debug.Log ("maxNumberOfSideFoodOrders " + maxNumberOfSideFoodOrders);
		if(maxNumberOfSideFoodOrders>0)
		{
			for(int i=0; i<maxNumberOfSideFoodOrders;i++)
			{
				sideFoodHintsHolder.transform.GetChild(i).GetChild(0).GetComponent<Button>().interactable = false;
			}
		}
		Debug.Log ("GlobalVariables.numberOfUnlockedIngredients " + GlobalVariables.numberOfUnlockedIngredients);
		for(int i=0;i<GlobalVariables.numberOfUnlockedIngredients;i++)
		{
			hintHolder.transform.GetChild(i).GetChild(0).GetComponent<Button>().interactable = false;
			
		}
	}

	void EnableInteraction()
	{
		Debug.Log ("maxNumberOfSideFoodOrders " + maxNumberOfSideFoodOrders);
		if(maxNumberOfSideFoodOrders>0)
		{
			for(int i=0; i<maxNumberOfSideFoodOrders;i++)
			{
				sideFoodHintsHolder.transform.GetChild(i).GetChild(0).GetComponent<Button>().interactable = true;
			}
		}
		Debug.Log ("GlobalVariables.numberOfUnlockedIngredients " + GlobalVariables.numberOfUnlockedIngredients);
		for(int i=0;i<GlobalVariables.numberOfUnlockedIngredients;i++)
		{
			hintHolder.transform.GetChild(i).GetChild(0).GetComponent<Button>().interactable = true;
			
		}
	}
}
