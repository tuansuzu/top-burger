using UnityEngine;
using System.Collections;
using UnityEngine.UI;

	/**
  * Scene:All
  * Object:Canvas
  * Description: Skripta zaduzena za hendlovanje(prikaz i sklanjanje svih Menu-ja,njihovo paljenje i gasenje, itd...)
  **/
public class MenuManager : MonoBehaviour 
{
	
	public Menu currentMenu;
	public Menu currentPopUpMenu = null;
	bool canEscape = true;
	Color Siva = new Color(0.60784f,0.60784f,0.60784f);
//	[HideInInspector]
//	public Animator openObject;
	public static int popupType = 1; // MainScene 1 - QuitApp, 2 - Shop->MainEkran, 3 - CrossPromotion->MainEkran, 4 - Settings->MainEkran, 5 - StartInterstitial -> MainEkran , 6 - GameModeSelection->MainEkran, 7 - LevelSelector_GameModeSelection, 8 - SpecialOffer -> MainEkran, 9 - Rate -> MainEkran, 9 - InAppMsg -> Shop
	// GamePlay 1 - , 2 - , 3 - , 4 - , 5 -  , 6 - , 7 - , 8 - , 9 - 
	public GameObject[] disabledObjects;
	GameObject worldSelectHeader, levelSelectHeader, shopHeader;
	GameObject ratePopUp, crossPromotionInterstitial, Levels;
	Text CoinsWorld, CoinsLevel, CoinsShop;
	float timePopUp = 1.2f;
	void Start () 
	{
		GlobalVariables.numberOfStart++;
		if(Application.loadedLevelName=="MainScene")
		{

			popupType = 1;
			SoundManager.Instance.Play_MenuMusic();
			Levels = GameObject.Find("LevelsHolder");
			crossPromotionInterstitial = GameObject.Find("PopUps/PopUpInterstitial");
			ratePopUp = GameObject.Find("PopUps/PopUpRate");

			CoinsWorld = GameObject.Find("CoinlWorldChooserText").GetComponent<Text>();
			CoinsLevel = GameObject.Find("CoinlLevelsChooserText").GetComponent<Text>();
			CoinsShop = GameObject.Find("CoinShopText").GetComponent<Text>();
			CoinsWorld.text = GlobalVariables.coins.ToString();
			CoinsLevel.text = GlobalVariables.coins.ToString();
			CoinsShop.text = GlobalVariables.coins.ToString();
			GameObject.Find("CoinHomeText").GetComponent<Text>().text = GlobalVariables.coins.ToString();
			GlobalVariables.Instance.ReadLevels();
			GameObject.Find("TextStarsCurrentWorldChooser").GetComponent<Text>().text = GlobalVariables.currentStars.ToString();
			GameObject.Find("TextStarsCurrentLevelChooser").GetComponent<Text>().text = GlobalVariables.currentStars.ToString();
			GameObject.Find("WorldsHolder/World1/StarsHolder/NumberHolder/TextStarsCurrent").GetComponent<Text>().text = GlobalVariables.currentStarsWorld1.ToString();
			GameObject.Find("WorldsHolder/World2/StarsHolder/NumberHolder/TextStarsCurrent").GetComponent<Text>().text = GlobalVariables.currentStarsWorld2.ToString();
			GameObject.Find("WorldsHolder/World3/StarsHolder/NumberHolder/TextStarsCurrent").GetComponent<Text>().text = GlobalVariables.currentStarsWorld3.ToString();
			if(GlobalVariables.currentStars>=310)
			{
				GameObject.Find("Canvas").transform.Find("Menus/WorldChooserMenu/WorldChooseHolder/AnimationHolder/WorldsHolder/World3").FindChild("Locked").gameObject.SetActive(false);
				GameObject.Find("Canvas").transform.Find("Menus/WorldChooserMenu/WorldChooseHolder/AnimationHolder/WorldsHolder/World2").FindChild("Locked").gameObject.SetActive(false);
			}
			else if(GlobalVariables.currentStars>=150)
			{
				GameObject.Find("Canvas").transform.Find("Menus/WorldChooserMenu/WorldChooseHolder/AnimationHolder/WorldsHolder/World2").FindChild("Locked").gameObject.SetActive(false);
			}

//			Debug.Log(" maxLevelWorld1 ".Colored(Colors.aqua)+GlobalVariables.maxLevelWorld1.ToString().Colored(Colors.lime));
//			Debug.Log(" maxLevelWorld2 ".Colored(Colors.aqua)+GlobalVariables.maxLevelWorld2.ToString().Colored(Colors.lime));
//			Debug.Log(" maxLevelWorld3 ".Colored(Colors.aqua)+GlobalVariables.maxLevelWorld3.ToString().Colored(Colors.lime));

			worldSelectHeader = GameObject.Find("Canvas/Menus/WorldChooserMenu/WorldChooseHolder/AnimationHolder/HeaderHolder");
			levelSelectHeader = GameObject.Find("Canvas/Menus/LevelChooserMenu/LevelsChooseHolder/AnimationHolder/HeaderHolder");
			shopHeader = GameObject.Find("Canvas/Menus/ShopHolder/AnimationHolder/HeaderHolder");


		}
		else if(Application.loadedLevelName=="GamePlay")
		{
			popupType = 0;
			SoundManager.Instance.Play_GameplayMusic();
		}

		if (disabledObjects!=null) {
			Debug.Log("disabledObjects.Length "+disabledObjects.Length);
			for(int i=0;i<disabledObjects.Length;i++)
				disabledObjects[i].SetActive(false);
		}
		
		if(Application.loadedLevelName!= "MapScene")
			ShowMenu(currentMenu.gameObject);	
		
		if(Application.loadedLevelName=="MainScene")
		{
			
			if(GlobalVariables.numberOfStart>1)
			{
				if(PlayerPrefs.HasKey("alreadyRated"))
				{
					Rate.alreadyRated = PlayerPrefs.GetInt("alreadyRated");
				}
				else
				{
					Rate.alreadyRated = 0;
				}
				
				if(Rate.alreadyRated==0)
				{
					Rate.appStartedNumber = PlayerPrefs.GetInt("appStartedNumber");
					Debug.Log("appStartedNumber "+Rate.appStartedNumber);
					if(Rate.appStartedNumber==2)
					{
						if(!PlayerPrefs.HasKey("DoubleCoins"))
						{
							Invoke("ShowSpecialOffer",timePopUp);
						}
						else
						{
							Invoke("ShowStartInterstitial",timePopUp);
						}
					}
					else if(Rate.appStartedNumber>=6)
					{
						Rate.appStartedNumber=0;
						PlayerPrefs.SetInt("appStartedNumber",Rate.appStartedNumber);
						PlayerPrefs.Save();
						Invoke("ShowRatePopUp",timePopUp);
						
					}
					else
					{
						Invoke("ShowStartInterstitial",timePopUp);
					}
				}
				else
				{
					if(Rate.appStartedNumber==2)
					{
						if(!PlayerPrefs.HasKey("DoubleCoins"))
						{
							Invoke("ShowSpecialOffer",timePopUp);
							Rate.appStartedNumber = 0;
							PlayerPrefs.SetInt("appStartedNumber",Rate.appStartedNumber);
							PlayerPrefs.Save();
						}
						else
						{
							if(Rate.appStartedNumber>3)
							{
								Rate.appStartedNumber=0;
								PlayerPrefs.SetInt("appStartedNumber",Rate.appStartedNumber);
								PlayerPrefs.Save();
							}
							Invoke("ShowStartInterstitial",timePopUp);
						}

					}
					else
					{
						if(Rate.appStartedNumber>3)
						{
							Rate.appStartedNumber=0;
							PlayerPrefs.SetInt("appStartedNumber",Rate.appStartedNumber);
							PlayerPrefs.Save();
						}
						Invoke("ShowStartInterstitial",timePopUp);
					}
				}
			}

		}
		
		
		
		
	}

	IEnumerator ChangeEscapeBool(float time)
	{
		canEscape = false;
		yield return new WaitForSeconds(time);
		canEscape = true;
	}
	// MainScene 1 - QuitApp, 2 - Shop->MainEkran, 3 - CrossPromotion->MainEkran, 4 - Settings->MainEkran, 5 - StartInterstitial -> MainEkran , 6 - GameModeSelection->MainEkran, 7 - LevelSelector_GameModeSelection, 8 - SpecialOffer -> MainEkran, 9 - Rate -> MainEkran, 10 - InAppMsg -> Shop

	void Update()
	{
		if(Input.GetKeyUp(KeyCode.Escape) && canEscape)
		{
			if(Application.loadedLevelName.Equals("MainScene"))
			{
				
				if(popupType == 2)
				{
					StartCoroutine(ChangeEscapeBool(1));
					ClosePopUpMenu(GameObject.Find("Canvas/Menus").transform.FindChild("ShopHolder").gameObject);
//					ShowInterstitial(1);
					
					popupType=1;

				}
				else if(popupType == 3)
				{
					StartCoroutine(ChangeEscapeBool(1));
					ClosePopUpMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("PopUpCrossPromotionOfferWall").gameObject);
					popupType=1;
				}
				else if(popupType == 4)
				{
					StartCoroutine(ChangeEscapeBool(1));
					ClosePopUpMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("PopUpSettings").gameObject);
					popupType=1;
				}
				else if(popupType == 5)
				{
					StartCoroutine(ChangeEscapeBool(1));
					ClosePopUpMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("PopUpInterstitial").gameObject);
					popupType=1;
				}
				else if(popupType == 6)
				{
					StartCoroutine(ChangeEscapeBool(1.5f));
					ClosePopUpMenu(GameObject.Find("Canvas/Menus").transform.FindChild("WorldChooserMenu").gameObject);

					popupType=1;
				}
				else if(popupType == 7)
				{
					StartCoroutine(ChangeEscapeBool(1f));
					ClosePopUpMenu(GameObject.Find("Canvas/Menus").transform.FindChild("LevelChooserMenu").gameObject);
					popupType=6;
				}
				else if(popupType == 8)
				{
					StartCoroutine(ChangeEscapeBool(1));
					ClosePopUpMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("PopUpDoubleCoins").gameObject);
					popupType=1;
				}
				else if(popupType == 9)
				{
					StartCoroutine(ChangeEscapeBool(1));
					ClosePopUpMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("PopUpRate").gameObject);
					popupType=1;
				}
				else if(popupType == 10)
				{
					StartCoroutine(ChangeEscapeBool(1));
					ClosePopUpMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("PopUpMessage").gameObject);
					popupType=2;
				}
			}
			else if(Application.loadedLevelName.Equals("GamePlay"))
			{
				if(GlobalVariables.GameplayMode==0)
				{
					if(popupType == 1)
					{
						StartCoroutine(ChangeEscapeBool(2));
						ShowPopUpMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("PopUpPause").gameObject);
					
						popupType = 2;
					}
					else if(popupType == 2)
					{
						StartCoroutine(ChangeEscapeBool(1.5f));
						popupType = 1;
						ClosePopUpMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("PopUpPause").gameObject);
		
					}
					else if(popupType == 3)
					{
						StartCoroutine(ChangeEscapeBool(1.5f));
						ShowPopUpMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("ShopHolder").gameObject);
						popupType = 1;
					}
					else if(popupType == 4)
					{
						StartCoroutine(ChangeEscapeBool(2));
						GameObject.Find("LevelGenerator").GetComponent<LevelGenerator>().StartLoadingSceneFromGameplay("home");
					}
					else if(popupType == 5)
					{
						StartCoroutine(ChangeEscapeBool(2));
						ClosePopUpMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("PopUpMessage").gameObject);
						popupType=3;
					}
				}
				else if(GlobalVariables.GameplayMode==1)
				{
					if(popupType == 1)
					{
						StartCoroutine(ChangeEscapeBool(1.5f));
						ShowPopUpMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("PopUpPause").gameObject);
						GameObject.Find("LevelTimer").GetComponent<LevelTimer>().PauseGame();
						popupType = 2;
					}
					else if(popupType == 2)
					{
						StartCoroutine(ChangeEscapeBool(1.5f));
						ClosePopUpMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("PopUpPause").gameObject);
						GameObject.Find("LevelTimer").GetComponent<LevelTimer>().SetGameActive();
						popupType = 1;
					}
					else if(popupType == 3)
					{
						StartCoroutine(ChangeEscapeBool(1.5f));
						ShowPopUpMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("ShopHolder").gameObject);
						GameObject.Find("LevelTimer").GetComponent<LevelTimer>().PauseGame();
						popupType = 1;
					}

				}
				else if(GlobalVariables.GameplayMode==2)
				{
					if(popupType == 1)
					{
						StartCoroutine(ChangeEscapeBool(1.5f));
						ShowPopUpMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("PopUpPause").gameObject);
						GameObject.Find("LevelTimer").GetComponent<LevelTimer>().PauseGame();
						popupType = 2;
					}
					else if(popupType == 2)
					{
						StartCoroutine(ChangeEscapeBool(1.5f));
						popupType = 1;
						ClosePopUpMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("PopUpPause").gameObject);
						GameObject.Find("LevelTimer").GetComponent<LevelTimer>().SetGameActive();
					}
					else if(popupType == 3)
					{
						StartCoroutine(ChangeEscapeBool(1.5f));
						ShowPopUpMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("ShopHolder").gameObject);
						GameObject.Find("LevelTimer").GetComponent<LevelTimer>().PauseGame();
						popupType = 1;
					}
				}
			}
		}
	}

	public void ChangePopUpType(int number)
	{
		popupType = number;
	}
	
	/// <summary>
	/// Funkcija koja pali(aktivira) objekat
	/// </summary>
	/// /// <param name="gameObject">Game object koji se prosledjuje i koji treba da se upali</param>
	public void EnableObject(GameObject gameObject)
	{
		
		if (gameObject != null) 
		{
			if (!gameObject.activeSelf) 
			{
				gameObject.SetActive (true);
			}
		}
	}

	/// <summary>
	/// Funkcija koja gasi objekat
	/// </summary>
	/// /// <param name="gameObject">Game object koji se prosledjuje i koji treba da se ugasi</param>
	public void DisableObject(GameObject gameObject)
	{
		
		if (gameObject != null) 
		{
			if (gameObject.activeSelf) 
			{
				gameObject.SetActive (false);
			}
		}
	}
	
	/// <summary>
	/// F-ja koji poziva ucitavanje Scene
	/// </summary>
	/// <param name="levelName">Level name.</param>
	public void LoadScene(string levelName )
	{
		if (levelName != "") {
			try {
				Application.LoadLevel (levelName);
			} catch (System.Exception e) {
				Debug.Log ("Can't load scene: " + e.Message);
			}
		} else {
			Debug.Log ("Can't load scene: Level name to set");
		}
	}
	
	/// <summary>
	/// F-ja koji poziva asihrono ucitavanje Scene
	/// </summary>
	/// <param name="levelName">Level name.</param>
	public void LoadSceneAsync(string levelName )
	{
		if (levelName != "") {
			try {
				Application.LoadLevelAsync (levelName);
			} catch (System.Exception e) {
				Debug.Log ("Can't load scene: " + e.Message);
			}
		} else {
			Debug.Log ("Can't load scene: Level name to set");
		}
	}

	/// <summary>
	/// Funkcija za prikaz Menu-ja koji je pozvan kao Menu
	/// </summary>
	/// /// <param name="menu">Game object koji se prosledjuje i treba da se skloni, mora imati na sebi skriptu Menu.</param>
	public void ShowMenu(GameObject menu)
	{
		if (currentMenu != null)
			currentMenu.IsOpen = false;
		
		menu.gameObject.SetActive (true);
		currentMenu = menu.GetComponent<Menu> ();
		currentMenu.IsOpen = true;
		
	}

	/// <summary>
	/// Funkcija za zatvaranje Menu-ja koji je pozvan kao Meni
	/// </summary>
	/// /// <param name="menu">Game object koji se prosledjuje za prikaz, mora imati na sebi skriptu Menu.</param>
	public void CloseMenu(GameObject menu)
	{
		if (menu != null) 
		{
			menu.GetComponent<Menu> ().IsOpen = false;
			menu.SetActive (false);
		}
	}

	/// <summary>
	/// Funkcija za prikaz Menu-ja koji je pozvan kao PopUp-a
	/// </summary>
	/// /// <param name="menu">Game object koji se prosledjuje za prikaz, mora imati na sebi skriptu Menu.</param>
	public void ShowPopUpMenu(GameObject menu)
	{
		menu.gameObject.SetActive (true);
		currentPopUpMenu = menu.GetComponent<Menu> ();
		currentPopUpMenu.IsOpen = true;
	}

	/// <summary>
	/// Funkcija za zatvaranje Menu-ja koji je pozvan kao PopUp-a, poziva inace coroutine-u, ima delay zbog animacije odlaska Menu-ja
	/// </summary>
	/// /// <param name="menu">Game object koji se prosledjuje i treba da se skloni, mora imati na sebi skriptu Menu.</param>
	public void ClosePopUpMenu(GameObject menu)
	{
		StartCoroutine("HidePopUp",menu);
	}

	/// <summary>
	/// Couorutine-a za zatvaranje Menu-ja koji je pozvan kao PopUp-a
	/// </summary>
	/// /// <param name="menu">Game object koji se prosledjuje, mora imati na sebi skriptu Menu.</param>
	IEnumerator HidePopUp(GameObject menu)
	{
		menu.GetComponent<Menu> ().IsOpen = false;
		yield return new WaitForSeconds(1.2f);

		menu.SetActive (false);
	}

	/// <summary>
	/// Funkcija za prikaz poruke preko Log-a, prilikom klika na dugme
	/// </summary>
	/// /// <param name="message">poruka koju treba prikazati.</param>
	public void ShowMessage(string message)
	{
		Debug.Log(message);
	}

	void ShowSpecialOffer()
	{
		popupType = 8;
		SoundManager.Instance.Play_SpecialOffer();
		ShowPopUpMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("PopUpDoubleCoins").gameObject);
	}

	void ShowRatePopUp()
	{
		popupType = 9;
		GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpMenu(ratePopUp);
	}

	/// <summary>
	/// Funkcija koja podesava naslov dialoga kao i poruku u dialogu i ova f-ja se poziva iz skripte
	/// </summary>
	/// <param name="messageTitleText">naslov koji treba prikazati.</param>
	/// <param name="messageText">custom poruka koju treba prikazati.</param>
	public void ShowPopUpMessage(string messageTitleText, string messageText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text=messageTitleText;
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/ContentHolder/TextBG/TextMessage").GetComponent<Text>().text=messageText;
		ShowPopUpMenu(transform.Find("PopUps/PopUpMessage").gameObject);

	}

	/// <summary>
	/// Funkcija koja podesava naslov CustomMessage-a, i ova f-ja se poziva preko button-a zajedno za f-jom ShowPopUpMessageCustomMessageText u redosledu: 1-ShowPopUpMessageTitleText 2-ShowPopUpMessageCustomMessageText
	/// </summary>
	/// <param name="messageTitleText">naslov koji treba prikazati.</param>
	public void ShowPopUpMessageTitleText(string messageTitleText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text=messageTitleText;
	}

	/// <summary>
	/// Funkcija koja podesava poruku CustomMessage, i poziva meni u vidu pop-upa, ova f-ja se poziva preko button-a zajedno za f-jom ShowPopUpMessageTitleText u redosledu: 1-ShowPopUpMessageTitleText 2-ShowPopUpMessageCustomMessageText
	/// </summary>
	/// <param name="messageText">custom poruka koju treba prikazati.</param>
	public void ShowPopUpMessageCustomMessageText(string messageText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/ContentHolder/TextBG/TextMessage").GetComponent<Text>().text=messageText;		
		ShowPopUpMenu(transform.Find("PopUps/PopUpMessage").gameObject);
	}

	/// <summary>
	/// Funkcija koja podesava naslov dialoga kao i poruku u dialogu i ova f-ja se poziva iz skripte
	/// </summary>
	/// <param name="dialogTitleText">naslov koji treba prikazati.</param>
	/// <param name="dialogMessageText">custom poruka koju treba prikazati.</param>
	public void ShowPopUpDialog(string dialogTitleText, string dialogMessageText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text=dialogTitleText;
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/ContentHolder/TextBG/TextMessage").GetComponent<Text>().text=dialogMessageText;
		ShowPopUpMenu(transform.Find("PopUps/PopUpMessage").gameObject);
	}

	/// <summary>
	/// Funkcija koja podesava naslov dialoga, ova f-ja se poziva preko button-a zajedno za f-jom ShowPopUpDialogCustomMessageText u redosledu: 1-ShowPopUpDialogTitleText 2-ShowPopUpDialogCustomMessageText
	/// </summary>
	/// <param name="dialogTitleText">naslov koji treba prikazati.</param>
	public void ShowPopUpDialogTitleText(string dialogTitleText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text=dialogTitleText;
	}

	/// <summary>
	/// Funkcija koja podesava poruku dialoga i poziva meni u vidu pop-upa, ova f-ja se poziva preko button-a zajedno za f-jom ShowPopUpDialogTitleText u redosledu: 1-ShowPopUpDialogTitleText 2-ShowPopUpDialogCustomMessageText
	/// </summary>
	/// <param name="dialogMessageText">custom poruka koju treba prikazati.</param>
	public void ShowPopUpDialogCustomMessageText(string dialogMessageText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/ContentHolder/TextBG/TextMessage").GetComponent<Text>().text=dialogMessageText;		
		ShowPopUpMenu(transform.Find("PopUps/PopUpMessage").gameObject);
	}


	public void MoveLeftWorld()
	{
		switch(GlobalVariables.numberOfWorldSelected)
		{
		case 0:
			GlobalVariables.numberOfWorldSelected = 1;
			GameObject.Find("WorldsHolder").GetComponent<Animator>().Play("WorldsMove3");
			break;
		case 1:
			GlobalVariables.numberOfWorldSelected = 2;
			GameObject.Find("WorldsHolder").GetComponent<Animator>().Play("WorldsMove1");
			break;
		case 2:
			GlobalVariables.numberOfWorldSelected = 0;
			GameObject.Find("WorldsHolder").GetComponent<Animator>().Play("WorldsMove2");
			break;
		default:
			Debug.Log("greska u GlobalVariables.numberOfWorld "+GlobalVariables.numberOfWorldSelected);
			break;
		}
		Debug.Log("SVET JE "+GlobalVariables.numberOfWorldSelected);
	}

	public void MoveRightWorld()
	{
		switch(GlobalVariables.numberOfWorldSelected)
		{
		case 0:
			GlobalVariables.numberOfWorldSelected = 2;
			GameObject.Find("WorldsHolder").GetComponent<Animator>().Play("WorldsMove2Reverse");
			break;
		case 1:
			GlobalVariables.numberOfWorldSelected = 0;
			GameObject.Find("WorldsHolder").GetComponent<Animator>().Play("WorldsMove3Reverse");
			break;
		case 2:
			GlobalVariables.numberOfWorldSelected = 1;
			GameObject.Find("WorldsHolder").GetComponent<Animator>().Play("WorldsMove1Reverse");
			break;
		default:
			Debug.Log("greska u GlobalVariables.numberOfWorld "+GlobalVariables.numberOfWorldSelected);
			break;
		}
	}

	public void SelectGameMode(int numberOfGameMode)
	{
			GameObject.Find("DeselectedHolder").transform.GetChild(GlobalVariables.GameplayMode).gameObject.SetActive(true);
			GameObject.Find("SelectedHolder").transform.GetChild(GlobalVariables.GameplayMode).gameObject.SetActive(false);

			GameObject.Find("DeselectedHolder").transform.GetChild(numberOfGameMode).gameObject.SetActive(false);
			GameObject.Find("SelectedHolder").transform.GetChild(numberOfGameMode).gameObject.SetActive(true);
			GlobalVariables.GameplayMode = numberOfGameMode;
	}



	public void SetEmptyStateOfWorldChooser()
	{
		GameObject.Find("Menus/WorldChooserMenu/WorldChooseHolder").GetComponent<Animator>().Play("Empty");
	}

	public void SetArrivingStateOfWorldChooser()
	{
		GameObject.Find("Menus/WorldChooserMenu/WorldChooseHolder").GetComponent<Animator>().Play("LevelChooseArriving");
	}

	public void StartModeOrLevelSelector()
	{
		switch(GlobalVariables.GameplayMode)
		{
		case 0:
			Debug.Log("Pusti SAMPIONAT mode");
			SoundManager.Instance.Stop_MenuMusic();
			switch(GlobalVariables.numberOfWorldSelected)
			{
			case 0:
				if(GlobalVariables.numberOfWorldUnlocked==3)
				{
					GlobalVariables.SceneToLoad = 3;
					ShowPopUpMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("LoadingMenu").gameObject);
				}
				else
				{
					GameObject.Find("World3/Locked").GetComponent<Animator>().Play("LockedWorldClick");
				}
				break;
			case 1:
				GlobalVariables.SceneToLoad = 3;
				ShowPopUpMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("LoadingMenu").gameObject);
				break;
			case 2:
				if(GlobalVariables.numberOfWorldUnlocked>=2)
				{
					GlobalVariables.SceneToLoad = 3;
					ShowPopUpMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("LoadingMenu").gameObject);
				}
				else
				{
					GameObject.Find("World2/Locked").GetComponent<Animator>().Play("LockedWorldClick");
				}
				break;
			}
		break;
		case 1:
			GameObject.Find("Canvas").transform.FindChild("Menus/LevelChooserMenu/LevelsChooseHolder/AnimationHolder/HeaderHolder/CoinHolderlHolder/CoinlLevelsChooserText").GetComponent<Text>().text = GlobalVariables.coins.ToString();

			switch(GlobalVariables.numberOfWorldSelected)
			{
			case 0:
				if(GlobalVariables.numberOfWorldUnlocked==3)
				{
					GameObject.Find("Menus").transform.FindChild("LevelChooserMenu/LevelsChooseHolder/AnimationHolder/LevelChooseText").GetComponent<Text>().text = "WORLD 3";
					ShowPopUpMenu(GameObject.Find("Canvas/Menus").transform.FindChild("LevelChooserMenu").gameObject);

					PrepareLevels();
				}
				else
				{
					GameObject.Find("World3/Locked").GetComponent<Animator>().Play("LockedWorldClick");
				}
				break;
			case 1:
				ShowPopUpMenu(GameObject.Find("Canvas/Menus").transform.FindChild("LevelChooserMenu").gameObject);
				GameObject.Find("Menus").transform.FindChild("LevelChooserMenu/LevelsChooseHolder/AnimationHolder/LevelChooseText").GetComponent<Text>().text = "WORLD 1";
				PrepareLevels();
				break;
			case 2:
				if(GlobalVariables.numberOfWorldUnlocked>=2)
				{
					GameObject.Find("Menus").transform.FindChild("LevelChooserMenu/LevelsChooseHolder/AnimationHolder/LevelChooseText").GetComponent<Text>().text = "WORLD 2";
					ShowPopUpMenu(GameObject.Find("Canvas/Menus").transform.FindChild("LevelChooserMenu").gameObject);
					PrepareLevels();
				}
				else
				{
					GameObject.Find("World2/Locked").GetComponent<Animator>().Play("LockedWorldClick");
				}
				break;
			}

			
			break;
		case 2:
			Debug.Log("Pusti TIME-ATTACK mode");

			SoundManager.Instance.Stop_MenuMusic();
			switch(GlobalVariables.numberOfWorldSelected)
			{
			case 0:
				if(GlobalVariables.numberOfWorldUnlocked==3)
				{
					GlobalVariables.SceneToLoad = 2;
					ShowMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("LoadingMenu").gameObject);

				}
				else
				{
					GameObject.Find("World3/Locked").GetComponent<Animator>().Play("LockedWorldClick");
				}
				break;
			case 1:
				GlobalVariables.SceneToLoad = 2;
				ShowMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("LoadingMenu").gameObject);
				break;
			case 2:
				if(GlobalVariables.numberOfWorldUnlocked>=2)
				{
					GlobalVariables.SceneToLoad = 2;
					ShowMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("LoadingMenu").gameObject);
				}
				else
				{
					GameObject.Find("World2/Locked").GetComponent<Animator>().Play("LockedWorldClick");
				}
				break;
			}
			break;

		}
	}

	void PrepareLevels()
	{
		int numberOfLvl = 0;
		switch(GlobalVariables.numberOfWorldSelected)
		{
		case 0:
			for(int i=0;i<60;i++)
			{
				Debug.Log("Svet 3 "+i+" "+GlobalVariables.Instance.world3Stars[i]);

				Levels.transform.GetChild(i).GetChild(1).FindChild("StarsHolder").GetChild(0).gameObject.SetActive(false);
				Levels.transform.GetChild(i).GetChild(1).FindChild("StarsHolder").GetChild(1).gameObject.SetActive(false);
				Levels.transform.GetChild(i).GetChild(1).FindChild("StarsHolder").GetChild(2).gameObject.SetActive(false);

				if(GlobalVariables.Instance.world3Stars[i]!="-1")
				{
					Levels.transform.GetChild(i).GetComponent<Button>().interactable = true;
					if(GlobalVariables.Instance.world3Stars[i]=="0")
					{
						Debug.Log("Svet 3 da otkljuca ".Colored(Colors.fuchsia));
						numberOfLvl = i;
						Levels.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
						Levels.transform.GetChild(i).GetChild(2).gameObject.SetActive(false);
						Levels.transform.GetChild(i).GetChild(1).GetComponent<Animator>().Play("LevelSelectSparkleIdle");
						
					}
					else
					{
						Levels.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
						Levels.transform.GetChild(i).GetChild(2).gameObject.SetActive(false);
						Levels.transform.GetChild(i).GetChild(1).GetComponent<Animator>().Play("New State");
						string number = GlobalVariables.Instance.world3Stars[i];
					
						for(int k=0;k<int.Parse(number);k++)
						{
							Levels.transform.GetChild(i).GetChild(1).FindChild("StarsHolder").GetChild(k).gameObject.SetActive(true);
						}
					}
				}
				else
				{

					Levels.transform.GetChild(i).GetComponent<Button>().interactable = false;
					Levels.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
					Levels.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);
					Levels.transform.GetChild(i).GetChild(1).GetComponent<Animator>().Play("New State");
				}

			}
			break;
		case 1:
			for(int i=0;i<60;i++)
			{
				Debug.Log("Svet 1 "+i+" "+GlobalVariables.Instance.world1Stars[i]);

				Levels.transform.GetChild(i).FindChild("Unlocked/StarsHolder").GetChild(0).gameObject.SetActive(false);
				Levels.transform.GetChild(i).FindChild("Unlocked/StarsHolder").GetChild(1).gameObject.SetActive(false);
				Levels.transform.GetChild(i).FindChild("Unlocked/StarsHolder").GetChild(2).gameObject.SetActive(false);

				if(GlobalVariables.Instance.world1Stars[i]!="-1")
				{
					Levels.transform.GetChild(i).GetComponent<Button>().interactable = true;
					if(GlobalVariables.Instance.world1Stars[i]=="0")
					{
						Debug.Log("Svet 1 da otkljuca ".Colored(Colors.fuchsia));
						numberOfLvl = i;
						Levels.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
						Levels.transform.GetChild(i).GetChild(2).gameObject.SetActive(false);
						Levels.transform.GetChild(i).GetChild(1).GetComponent<Animator>().Play("LevelSelectSparkleIdle");
					}
					else
					{
						Levels.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
						Levels.transform.GetChild(i).GetChild(2).gameObject.SetActive(false);
						Levels.transform.GetChild(i).FindChild("Unlocked").GetComponent<Animator>().Play("New State");
						string number = GlobalVariables.Instance.world1Stars[i];
						
						for(int k=0;k<int.Parse(number);k++)
						{
							Levels.transform.GetChild(i).FindChild("Unlocked").FindChild("StarsHolder").GetChild(k).gameObject.SetActive(true);
						}
					}
				}
				else
				{
					
					Levels.transform.GetChild(i).GetComponent<Button>().interactable = false;
					Levels.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
					Levels.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);
					Levels.transform.GetChild(i).FindChild("Unlocked").GetComponent<Animator>().Play("New State");
				}
				
			}

			break;
		case 2:
			for(int i=0;i<60;i++)
			{
				Debug.Log("Svet 2 "+i+" "+GlobalVariables.Instance.world2Stars[i]);


				Levels.transform.GetChild(i).GetChild(1).FindChild("StarsHolder").GetChild(0).gameObject.SetActive(false);
				Levels.transform.GetChild(i).GetChild(1).FindChild("StarsHolder").GetChild(1).gameObject.SetActive(false);
				Levels.transform.GetChild(i).GetChild(1).FindChild("StarsHolder").GetChild(2).gameObject.SetActive(false);
				

				if(GlobalVariables.Instance.world2Stars[i]!="-1")
				{
					Levels.transform.GetChild(i).GetComponent<Button>().interactable = true;
					if(GlobalVariables.Instance.world2Stars[i]=="0")
					{
						Debug.Log("Svet 2 da otkljuca ".Colored(Colors.fuchsia));
						numberOfLvl = i;
						Levels.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
						Levels.transform.GetChild(i).GetChild(2).gameObject.SetActive(false);
						Levels.transform.GetChild(i).GetChild(1).GetComponent<Animator>().Play("LevelSelectSparkleIdle");
						
					}
					else
					{
						Levels.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
						Levels.transform.GetChild(i).GetChild(2).gameObject.SetActive(false);
						Levels.transform.GetChild(i).GetChild(1).GetComponent<Animator>().Play("New State");
						string number = GlobalVariables.Instance.world2Stars[i];
						
						for(int k=0;k<int.Parse(number);k++)
						{
							Levels.transform.GetChild(i).GetChild(1).FindChild("StarsHolder").GetChild(k).gameObject.SetActive(true);
						}
					}
				}
				else
				{
					
					Levels.transform.GetChild(i).GetComponent<Button>().interactable = false;
					Levels.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
					Levels.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);
					Levels.transform.GetChild(i).GetChild(1).GetComponent<Animator>().Play("New State");
				}
				
			}


			break;
		}



		if(numberOfLvl<5)
		{
			GameObject.Find("Content").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1745);
		}
		else if(numberOfLvl<10)
		{
			GameObject.Find("Content").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1451);
		}
		else if(numberOfLvl<15)
		{
			GameObject.Find("Content").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1157);
		}
		else if(numberOfLvl<20)
		{
			GameObject.Find("Content").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -863);
		}
		else if(numberOfLvl<25)
		{
			GameObject.Find("Content").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -569);
		}
		else if(numberOfLvl<30)
		{
			GameObject.Find("Content").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -275);
		}
		else if(numberOfLvl<35)
		{
			GameObject.Find("Content").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 19);
		}
		else if(numberOfLvl<40)
		{
			GameObject.Find("Content").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 313);
		}
		else if(numberOfLvl<45)
		{
			GameObject.Find("Content").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 607);
		}
		else if(numberOfLvl<50)
		{
			GameObject.Find("Content").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 901);
		}
		else if(numberOfLvl<55)
		{
			GameObject.Find("Content").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 1195);
		}
		else
		{
			GameObject.Find("Content").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 1489);
		}
	}

	public void LoadLevel(int lvl)
	{

		GlobalVariables.SceneToLoad = 1;
		GlobalVariables.currentLevel = lvl;
		ShowMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("LoadingMenu").gameObject);
		SoundManager.Instance.Stop_MenuMusic();
		SoundManager.Instance.Play_LoadingArrival();
	}

	public void LoadNextLevel()
	{
		GlobalVariables.SceneToLoad = 1;
		GlobalVariables.currentLevel++;
		GameObject.Find("Canvas/PopUps").transform.FindChild("LoadingHolder").gameObject.SetActive(true);
		GameObject.Find("Canvas/PopUps").transform.FindChild("LoadingHolder/LoadingHolder/AnimationHolder").GetComponent<Animator>().Play("LoadingGameplayDeparting");
		ShowMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("LoadingHolder").gameObject);
	}

	public void StartWorldChooser()
	{
		if(GlobalVariables.tutorialPassed)
		{
			GameObject.Find("Canvas/Menus").transform.FindChild("MainMenu").GetComponent<Animator>().Play("Departing");
			GameObject.Find("Canvas").transform.FindChild("Menus/WorldChooserMenu/WorldChooseHolder/AnimationHolder/HeaderHolder/CoinHolderlHolder/CoinlWorldChooserText").GetComponent<Text>().text = GlobalVariables.coins.ToString();
		}
		else
		{

			GlobalVariables.SceneToLoad = 1;
			GlobalVariables.currentLevel = 1;
			ShowMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("LoadingMenu").gameObject);
			SoundManager.Instance.Stop_MenuMusic();
			SoundManager.Instance.Play_LoadingArrival();
		}

	}


	public void AddCoinsTest()
	{
		StartCoroutine(GlobalVariables.Instance.moneyCounter(1000, GameObject.Find("CoinShopText").GetComponent<Text>()));
	}

	public void CloseUnlockedPopUpAndStartGoalPopUp()
	{
		StartCoroutine("CloseUnlockedPopUp");
	}

	IEnumerator CloseUnlockedPopUp()
	{
		ClosePopUpMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("PopUpUnlocked").gameObject);
		yield return new WaitForSeconds(0.75f);
		ShowPopUpMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("PopUpGoal").gameObject);
	}

	public void PlayButtonSound()
	{
		SoundManager.Instance.Play_ButtonClick();
	}

	public void PlayWorldChooseSound()
	{
		SoundManager.Instance.Play_WorldChosse();
	}

	public void PlayIngredientSound()
	{
		SoundManager.Instance.Play_Ingredient();
	}

	public void PlayWrongIngredientSound()
	{
		SoundManager.Instance.Play_WrongIngredient();
	}

	public void PlaySideFoodSound()
	{
		SoundManager.Instance.Play_SideFood();
	}
	
	public void PlayPowerUpSound()
	{
		SoundManager.Instance.Play_PowerUp();
	}

	public void ShowInterstitial()
	{
		if(!GlobalVariables.removeAds)
		{
			AdsManager.Instance.ShowInterstitial();
		}
	}

	public void ShowVideoReward(int ID)
	{
		AdsManager.Instance.ShowVideoReward(ID);
	}

	public void VideoRewardAvailable(int ID)
	{
		AdsManager.Instance.IsVideoRewardAvailable();
	}

	public void RefreshShopCoins()
	{
		GameObject.Find("CoinShopText").GetComponent<Text>().text = GlobalVariables.coins.ToString();
	}

	public void RefreshCoins()
	{
		if(Application.loadedLevelName.Equals("MainScene"))
		{
			GameObject.Find("Canvas").transform.FindChild("Menus/WorldChooserMenu/WorldChooseHolder/AnimationHolder/HeaderHolder/CoinHolderlHolder/CoinlWorldChooserText").GetComponent<Text>().text = GlobalVariables.coins.ToString();
			GameObject.Find("Canvas").transform.FindChild("Menus/LevelChooserMenu/WorldChooseHolder/AnimationHolder/HeaderHolder/CoinHolderlHolder/CoinlLevelsChooserText").GetComponent<Text>().text = GlobalVariables.coins.ToString();
		}
		else
		{
			GameObject.Find("CoinsText").GetComponent<Text>().text = GlobalVariables.coins.ToString();
		}
	}

	public void testTimer()
	{
		if(Timer.counterTime<=1800)
		{
			Timer.counterTime=0;
			
			Debug.Log("OnFullScreenADdisplayed psotqavi na 0");
			GameObject.Find("Available").SetActive(false);
			GameObject.Find("ChefDeselected").GetComponent<Button>().interactable = true;
			GameObject.Find("ChefDeselected/ChefBlue").GetComponent<Image>().color = Color.white;
			PlayerPrefs.SetFloat("ChefTimer", Timer.counterTime);
			PlayerPrefs.SetInt("ChefPlayed",1);
			PlayerPrefs.Save();
		}
		else
		{
			Debug.Log("OnFullScreenADdisplayed smanji za vece od 1800");
			Timer.counterTime -=1800;
			PlayerPrefs.SetInt("ChefPlayed",0);
			PlayerPrefs.SetFloat("ChefTimer", Timer.counterTime);
			PlayerPrefs.Save();
		}
	}

	public void AddCoinsTest(int amount)
	{
		StartCoroutine(GlobalVariables.Instance.moneyCounter(amount , GameObject.Find("CoinShopText").GetComponent<Text>()));
	}

	public void TestRemoveAds()
	{
		GameObject.Find("ShopPartHolder/BGShopContent").transform.FindChild("RemoveAdsButton/RemoveAdsButton/Body").GetComponent<Image>().color=Siva;
		GameObject.Find("ShopPartHolder/BGShopContent").transform.FindChild("RemoveAdsButton/RemoveAdsButton/Icon").GetComponent<Image>().color=Siva;
		GameObject.Find("BGShopContent/RemoveAdsButton").GetComponent<Button>().interactable=false;
		GlobalVariables.removeAds = true;
		PlayerPrefs.SetInt("RemoveAds",357);
		PlayerPrefs.Save();

	}

	public void TestVideoReward()
	{
		GameObject.Find("CoinHomeHolder").GetComponent<Animator>().Play("HomeCoinsWatchVideo");
		StartCoroutine(GlobalVariables.Instance.moneyCounter(100,GameObject.Find("CoinHomeText").GetComponent<Text>()));
	}
}
