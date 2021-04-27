using UnityEngine;
using System.Collections;
using UnityEngine.UI;

///<summary>
///<para>Scene:All/NameOfScene/NameOfScene1,NameOfScene2,NameOfScene3...</para>
///<para>Object:N/A</para>
///<para>Description: Sample Description </para>
///</summary>

public class LevelTimer : MonoBehaviour
{

	Image timerImage;
	Text timerText;
	Color MyGreen = new Color( 0.01961f,  0.97255f,  0.45490f);
	Color Add2GreenMain = new Color( 0.69804f,  1.00000f,  0.00000f);
	Color Add2GreenOutline = new Color( 0.00392f,  0.30980f,  0.04314f);
	Color Remove2RedMain = new Color( 0.66275f,  0.01961f,  0.01961f);
	Color PowerMain = new Color( 1.00000f,  0.92941f,  0.00000f);
	Color PowerOutline = new Color( 0.58039f,  0.00000f,  0.00000f);
	public float counterTime;
	float counterTimeStart, yellowLimit, redLimit;
	int minutes, seconds;
	float timeUnit;
	bool isPause = false;
	bool timeFrezzeActive = false;

	// Use this for initialization
	void Start ()
	{
		timeUnit = 100 / counterTime;
		yellowLimit = counterTime * 2 / 3;
		redLimit = counterTime / 3;
		counterTimeStart = counterTime;
		timerImage = transform.GetComponent<Image> ();
		timerText = transform.GetChild (0).GetComponent<Text> ();
		SetTimerText();

	}

	public void StartLevelTimer()
	{
		StartCoroutine ("TimerDecrease");
	}

	public void StartTimerDecreaseForTut()
	{
		StartCoroutine("TimerDecrease");
	}

	public void StopTimerDecrease()
	{
		StopCoroutine("TimerDecrease");
	}

	public IEnumerator TimerDecrease()
	{

		while (LevelGenerator.gameActive)
		{
			yield return new WaitForSeconds(1);
			timerImage.fillAmount -= timeUnit*0.01f;
			if(timerImage.fillAmount<0.33)
			{
				timerImage.color=Color.red;
			}
			else if(timerImage.fillAmount<0.66)
			{
				timerImage.color=Color.yellow;
			}
			else
			{
				timerImage.color=MyGreen;
			}
			counterTime--;
			if(counterTime==10)
			{
				GameObject.Find("ClockHolder").GetComponent<Animator>().Play("ClockLastTenSecondsAndEnd");
				SoundManager.Instance.Play_TimeCountDown();
			}
			SetTimerText();

		}

	}

	void TimeUpDisableInteraction()
	{
		for(int i=0;i<14;i++)
		{
			GameObject.Find("IngredientsHolder").transform.GetChild(i).GetChild(0).GetComponent<Button>().interactable = false;
		}
		
		for(int i=0;i<4;i++)
		{
			GameObject.Find("SideFoodsHolder").transform.GetChild(i).GetChild(0).GetComponent<Button>().interactable = false;
		}
		
		GameObject.Find("AddTimeImage").GetComponent<Button>().interactable = false;
		GameObject.Find("StopTimeImage").GetComponent<Button>().interactable = false;
		
	}

	void SetTimerText()
	{
		minutes = (int)counterTime / 60;
		seconds = (int)counterTime % 60;
		if (seconds < 1 && minutes < 1) 
		{
			TimeUpDisableInteraction();
			timerText.text = "00:00";
			LevelGenerator.gameActive = false;
			SoundManager.Instance.Stop_TimeCountDown();
			LevelGenerator.hintsPowerActive = false;
			LevelGenerator.tipsMasterPowerActive =false; 
			LevelGenerator.happyTimePowerActive = false;
			if(GlobalVariables.GameplayMode==0)
			{
				Debug.Log("Chef Championship MODE".Colored(Colors.cyan));
			}
			else if(GlobalVariables.GameplayMode==1)
			{
				Invoke("ShowWinLosePopUpWithDelay",0.5f);
			}
			else if(GlobalVariables.GameplayMode==2)
			{
				SoundManager.Instance.Stop_GameplayMusic();
				Debug.Log("TimeAttack MODE".Colored(Colors.cyan));
				SoundManager.Instance.Play_Win();
				GameObject.Find("Canvas").GetComponent<MenuManager>().ShowMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("TimeAttackEndMenu").gameObject);
				GameObject.Find("TimeAttackFinalText").GetComponent<Text>().text = LevelGenerator.mealEarningFinal.ToString();
				
			}

		}
		else if (seconds >= 0 && seconds <= 9) 
		{
			
			if (minutes < 10) {
				timerText.text = "0" + minutes.ToString () + ":" + "0" + seconds.ToString ();
			} 
			else 
			{
				timerText.text = minutes.ToString () + ":" + "0" + seconds.ToString ();
			}
		} 
		else 
		{
			
			if (minutes < 10) 
			{
				timerText.text = "0" + minutes.ToString () + ":" + seconds.ToString ();
			} 
			else
			{
				timerText.text = minutes.ToString () + ":" + seconds.ToString ();
			}
		}
	}

	void ShowWinLosePopUpWithDelay()
	{
		if(LevelGenerator.tutorial)
		{
			for(int i=0;i<6;i++)
			{
				GameObject.Find("IngredientsHolder").transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(false);
			}
		}
		SoundManager.resumeCountdownTime = 0f;
		SoundManager.Instance.Stop_TimeCountDown();
		SoundManager.Instance.Stop_CustomerMad();
		SoundManager.Instance.Stop_CustomerMadDeparting();
		SoundManager.Instance.Stop_CustomerHappyDeparting();
		GameObject.Find("HeadBG").transform.FindChild("ParticlesHolder/SmokeParticleRight").gameObject.SetActive(false);
		GameObject.Find("HeadBG").transform.FindChild("ParticlesHolder/SmokeParticleLeft").gameObject.SetActive(false);
		GameObject.Find("ClockHolder").transform.FindChild("AnimationHolder/SmokeHolder/SmokeParticleMidle").gameObject.SetActive(false);
		for(int i=0;i<14;i++)
		{
			GameObject.Find("IngredientsHolder").transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(false);
		}

		for(int i=0;i<4;i++)
		{
			GameObject.Find("SideFoodsHolder").transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(false);
		}

//		if(LevelGenerator.score>=20)
		if(LevelGenerator.score>=LevelsParser.ListOfLevels[GlobalVariables.currentLevel-1].levelGoal)
		{
			//0-10% 1 zvezdica, 10-30% 2 zvezdice, 30+% 3 zvezdice

			LevelGenerator.winGame=true;
			if(LevelGenerator.tutorial)
			{
				GlobalVariables.tutorialPassed = true;
				PlayerPrefs.SetInt("tutorialPassed",1);
				PlayerPrefs.Save();
			}

			SoundManager.Instance.Stop_GameplayMusic();
			SoundManager.Instance.Play_Win();
			MenuManager.popupType = 0;

			GameObject.Find("Canvas").GetComponent<MenuManager>().ShowMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("WinMenu").gameObject);

			if(LevelGenerator.score==LevelGenerator.mealEarningFinal+LevelGenerator.tipsEarningsFinal)
			{
				GameObject.Find("IngredientsScoreWinText").GetComponent<Text>().text = LevelGenerator.mealEarningFinal.ToString();
				GameObject.Find("TipsScoreWinText").GetComponent<Text>().text = LevelGenerator.tipsEarningsFinal.ToString();
				GameObject.Find("GoalFinalWinText").GetComponent<Text>().text = "<color=#00ff00ff>"+(LevelGenerator.mealEarningFinal+LevelGenerator.tipsEarningsFinal).ToString()+"</color>"+"/"+LevelsParser.ListOfLevels[GlobalVariables.currentLevel-1].levelGoal;
			}
			else
			{	
				LevelGenerator.mealEarningFinal = LevelGenerator.score - LevelGenerator.tipsEarningsFinal;
				GameObject.Find("IngredientsScoreWinText").GetComponent<Text>().text = LevelGenerator.mealEarningFinal.ToString();
				GameObject.Find("TipsScoreWinText").GetComponent<Text>().text = LevelGenerator.tipsEarningsFinal.ToString();
				GameObject.Find("GoalFinalWinText").GetComponent<Text>().text = "<color=#00ff00ff>"+(LevelGenerator.score).ToString()+"</color>"+"/"+LevelsParser.ListOfLevels[GlobalVariables.currentLevel-1].levelGoal;
			}
			if(LevelGenerator.score<=LevelsParser.ListOfLevels[GlobalVariables.currentLevel-1].levelGoal+(LevelsParser.ListOfLevels[GlobalVariables.currentLevel-1].levelGoal*10)/100)
			{
				GameObject.Find("Star2Holder").gameObject.SetActive(false);
				GameObject.Find("Star3Holder").gameObject.SetActive(false);
				
				GameObject.Find("GlobalVariables").GetComponent<GlobalVariables>().ChangeStarsOnWorld(GlobalVariables.numberOfWorldSelected,1);
			}
			else if(LevelGenerator.score<=LevelsParser.ListOfLevels[GlobalVariables.currentLevel-1].levelGoal+(LevelsParser.ListOfLevels[GlobalVariables.currentLevel-1].levelGoal*30)/100)
			{
				GameObject.Find("Star3Holder").gameObject.SetActive(false);
				GameObject.Find("GlobalVariables").GetComponent<GlobalVariables>().ChangeStarsOnWorld(GlobalVariables.numberOfWorldSelected,2);
			}
			else
			{
				GameObject.Find("GlobalVariables").GetComponent<GlobalVariables>().ChangeStarsOnWorld(GlobalVariables.numberOfWorldSelected,3);
			}
		}
		else
		{
			SoundManager.Instance.Stop_GameplayMusic();
			SoundManager.Instance.Play_Lose();
			MenuManager.popupType = 0;




			GameObject.Find("Canvas").GetComponent<MenuManager>().ShowMenu(GameObject.Find("Canvas/PopUps").transform.FindChild("LoseMenu").gameObject);

			if(LevelGenerator.score==LevelGenerator.mealEarningFinal+LevelGenerator.tipsEarningsFinal)
			{
				GameObject.Find("IngredientsScoreLoseText").GetComponent<Text>().text = LevelGenerator.mealEarningFinal.ToString();
				GameObject.Find("TipsScoreLoseText").GetComponent<Text>().text = LevelGenerator.tipsEarningsFinal.ToString();
				GameObject.Find("GoalFinalLoseText").GetComponent<Text>().text = "<color=#ff0000ff>"+(LevelGenerator.mealEarningFinal+LevelGenerator.tipsEarningsFinal).ToString()+"</color>"+"/"+LevelsParser.ListOfLevels[GlobalVariables.currentLevel-1].levelGoal;
			}
			else
			{	
				LevelGenerator.mealEarningFinal = LevelGenerator.score - LevelGenerator.tipsEarningsFinal;
				GameObject.Find("IngredientsScoreLoseText").GetComponent<Text>().text = LevelGenerator.mealEarningFinal.ToString();
				GameObject.Find("TipsScoreLoseText").GetComponent<Text>().text = LevelGenerator.tipsEarningsFinal.ToString();
				GameObject.Find("GoalFinalLoseText").GetComponent<Text>().text = "<color=#ff0000ff>"+(LevelGenerator.score).ToString()+"</color>"+"/"+LevelsParser.ListOfLevels[GlobalVariables.currentLevel-1].levelGoal;
			}

//			GameObject.Find("IngredientsScoreLoseText").GetComponent<Text>().text = LevelGenerator.mealEarningFinal.ToString();
//			GameObject.Find("TipsScoreLoseText").GetComponent<Text>().text = LevelGenerator.tipsEarningsFinal.ToString();
//			GameObject.Find("GoalFinalLoseText").GetComponent<Text>().text = "<color=#ff0000ff>"+LevelGenerator.score.ToString()+"</color>"+"/"+LevelsParser.ListOfLevels[GlobalVariables.currentLevel-1].levelGoal;
		}
	}

	IEnumerator TimeFreezeCoroutine()
	{
		timeFrezzeActive = true;
		SoundManager.Instance.Pause_TimeCountDown();
		GameObject.Find("ClockHolder").GetComponent<Animator>().speed = 0;
		LevelGenerator.gameActive = false;
		LevelGenerator.timeFreezeActive = true;
		GameObject.Find("IceHolderBg").GetComponent<Animator>().Play("IceArriving");
		GameObject.Find("IceHolderMainTimer").GetComponent<Animator>().Play("IceArriving");
		if(GlobalVariables.GameplayMode==1)
		{
			GameObject.Find("IceHolderCustomerTimer").GetComponent<Animator>().Play("IceArriving");
		}
		yield return new WaitForSeconds (5);
		while(isPause)
			yield return null;
		SoundManager.Instance.Resume_TimeCountDown();
		GameObject.Find("ClockHolder").GetComponent<Animator>().speed = 1;
		GameObject.Find("IceHolderBg").GetComponent<Animator>().Play("IceDeparting");
		GameObject.Find("IceHolderMainTimer").GetComponent<Animator>().Play("IceDeparting");
		if(GlobalVariables.GameplayMode==1)
		{
			GameObject.Find("IceHolderCustomerTimer").GetComponent<Animator>().Play("IceDeparting");
			LevelGenerator.customerActive = true;
			LevelGenerator.gameActive = true;
			GameObject.Find("CustomerTimerBar").GetComponent<CustomerTimer>().ContinueCustomerTimer();
		}
		LevelGenerator.gameActive = true;
		LevelGenerator.timeFreezeActive = false;
		timeFrezzeActive = false;
		StopCoroutine ("TimerDecrease");
		StartCoroutine ("TimerDecrease");
	}

	public void TimeFreeze()
	{
		Debug.Log("TimeFreeze "+GlobalVariables.coins);
		if(!LevelGenerator.timeFreezeActive && GlobalVariables.coins>=500)
		{
			if(minutes>0)
			{
				SoundManager.Instance.Play_PowerUp();
				//StartCoroutine(GlobalVariables.Instance.moneyCounter(-500, GameObject.Find("CoinsText").GetComponent<Text>()));
				GlobalVariables.coins-=500;
				PlayerPrefs.SetInt("coins", GlobalVariables.coins);
				PlayerPrefs.Save();
				SoundManager.Instance.Play_Coins();
				StartCoroutine ("TimeFreezeCoroutine");
				
				#if UNITY_ANDROID
				try
				{
					using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) 
					{
						using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) 
						{
							obj_Activity.Call("FlurryInGamePowerUsed","TimeFreeze");
						}
					} 
					return;
				}
				catch
				{
					Debug.Log("Error Contacting Android - TimeFreeze! ");
					return;
				}
				#endif
			}
			else if(minutes==0 && seconds>1)
			{
				SoundManager.Instance.Play_PowerUp();
				//StartCoroutine(GlobalVariables.Instance.moneyCounter(-500, GameObject.Find("CoinsText").GetComponent<Text>()));
				GlobalVariables.coins-=500;
				PlayerPrefs.SetInt("coins", GlobalVariables.coins);
				PlayerPrefs.Save();
				SoundManager.Instance.Play_Coins();
				StartCoroutine ("TimeFreezeCoroutine");
				
				#if UNITY_ANDROID
				try
				{
					using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) 
					{
						using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) 
						{
							obj_Activity.Call("FlurryInGamePowerUsed","TimeFreeze");
						}
					} 
					return;
				}
				catch
				{
					Debug.Log("Error Contacting Android - TimeFreeze! ");
					return;
				}
				#endif
			}

		}
		else
		{
			if(GlobalVariables.coins<500)
			{
				SoundManager.Instance.Play_NoMoney();
				GameObject.Find("HeaderHolder/CoinHolderlHolder").GetComponent<Animator>().Play("NotEnoughCoins");
			}
		}
	}

	public void AddOrRemoveTime(int numerOfSeconds)
	{
		GameObject.Find("AddTimeSecHolder").GetComponent<Animator>().Play("AddTime");


		if(numerOfSeconds>0)
		{
			GameObject.Find("AddTimeSecHolder").transform.Find("+10").GetComponent<Text>().text ="+"+numerOfSeconds.ToString();
			GameObject.Find("AddTimeSecHolder").transform.Find("+10").GetComponent<Text>().color = Add2GreenMain;
			GameObject.Find("AddTimeSecHolder").transform.Find("+10").GetComponent<Outline>().effectColor = Add2GreenOutline;
		}
		else
		{
			GameObject.Find("AddTimeSecHolder").transform.Find("+10").GetComponent<Text>().text = numerOfSeconds.ToString();
			GameObject.Find("AddTimeSecHolder").transform.Find("+10").GetComponent<Text>().color = Remove2RedMain;
			GameObject.Find("AddTimeSecHolder").transform.Find("+10").GetComponent<Outline>().effectColor = Color.white;
		}
		if (counterTime + numerOfSeconds < counterTimeStart)
		{
			counterTime += numerOfSeconds;
		}
		else 
		{
			counterTime = counterTimeStart;
		}
		if(counterTime>10)
		{
			GameObject.Find("ClockHolder").GetComponent<Animator>().Play("ClockIdle");
		}
		else
		{
			GameObject.Find("ClockHolder").GetComponent<Animator>().Play("ClockLastTenSecondsAndEnd");
		}
		if(LevelGenerator.gameActive)
		{
			SetTimerText();
			timerImage.fillAmount += numerOfSeconds*timeUnit*0.01f;
			if(timerImage.fillAmount<0.33)
			{
				timerImage.color=Color.red;
			}
			else if(timerImage.fillAmount<0.66)
			{
				timerImage.color=Color.yellow;
			}
			else
			{
				timerImage.color=MyGreen;
			}
		}
	}

	public void AddTime()
	{
		Debug.Log("AddTime "+GlobalVariables.coins);
		int timeMax;
		if(LevelGenerator.timeVideoWatched)
		{
			timeMax = 75;
		}
		else
		{
			timeMax = 60;
		}

		if(GlobalVariables.coins>=500 && timeMax>counterTime)
		{
			GameObject.Find("AddTimeImage").GetComponent<Button>().interactable = false;
			SoundManager.Instance.Play_PowerUp();
//			StartCoroutine(GlobalVariables.Instance.moneyCounter(-500, GameObject.Find("CoinsText").GetComponent<Text>()));

			GlobalVariables.coins-=500;
			GameObject.Find("CoinsText").GetComponent<Text>().text = GlobalVariables.coins.ToString();
			PlayerPrefs.SetInt("coins", GlobalVariables.coins);
			PlayerPrefs.Save();

			GameObject.Find("AddTimeSecHolder").transform.Find("+10").GetComponent<Text>().text = "+10";
			GameObject.Find("AddTimeSecHolder").GetComponent<Animator>().Play("AddTime");
			GameObject.Find("AddTimeSecHolder").transform.Find("+10").GetComponent<Text>().color = PowerMain;
			GameObject.Find("AddTimeSecHolder").transform.Find("+10").GetComponent<Outline>().effectColor = PowerOutline;
			float timeToAdd=10;
			if(LevelGenerator.timeVideoWatched)
				timeToAdd=15;
			if (counterTime + timeToAdd < counterTimeStart)
			{
				counterTime += timeToAdd;
			}
			else
			{
				counterTime = counterTimeStart;
			}
			if(counterTime>10)
			{
				GameObject.Find("ClockHolder").GetComponent<Animator>().Play("ClockIdle");
				SoundManager.Instance.Stop_TimeCountDown();
			}
			else
			{
				GameObject.Find("ClockHolder").GetComponent<Animator>().Play("ClockLastTenSecondsAndEnd");
			}
			if(LevelGenerator.gameActive)
			{
				SetTimerText();
				timerImage.fillAmount += 10*timeUnit*0.01f;
				if(timerImage.fillAmount<0.33)
				{
					timerImage.color=Color.red;
				}
				else if(timerImage.fillAmount<0.66)
				{
					timerImage.color=Color.yellow;
				}
				else
				{
					timerImage.color=MyGreen;
				}
			}
			GameObject.Find("AddTimeImage").GetComponent<Button>().interactable = true;
			#if UNITY_ANDROID
			try
			{
				using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) 
				{
					using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) 
					{
						obj_Activity.Call("FlurryInGamePowerUsed","AddTime");
					}
				} 
				return;
			}
			catch
			{
				Debug.Log("Error Contacting Android - AddTime! ");
				return;
			}
			#endif
		}
		else
		{
			if(GlobalVariables.coins<500)
			{
				SoundManager.Instance.Play_NoMoney();
				GameObject.Find("HeaderHolder/CoinHolderlHolder").GetComponent<Animator>().Play("NotEnoughCoins");
			}
		}

	}

	public void SetGameActive()
	{
		if(LevelGenerator.gameStarted)
		{
			LevelGenerator.gameActive = true;
			if(GlobalVariables.GameplayMode!=0)
			{
				UnpauseTips();
				Debug.Log("isMadCustomer "+LevelGenerator.isMadCustomer);
				if(LevelGenerator.isMadCustomer)
				{
					Debug.Log("isMadCustomer "+LevelGenerator.isMadCustomer);
					Invoke("ActivateParticlesForCharacter",0.5f);
				}
				isPause = false;
				GameObject.Find("ClockHolder").GetComponent<Animator>().speed = 1;
				SoundManager.Instance.Resume_TimeCountDown();
				if(!timeFrezzeActive)
					StartCoroutine ("TimerDecrease");
			}
			if(GlobalVariables.GameplayMode==1)
			{
				GameObject.Find("CustomerTimerBar").GetComponent<CustomerTimer>().ContinueCustomerTimer();
			}
		}

	}

	void ActivateParticlesForCharacter()
	{
		GameObject.Find("HeadBG").transform.FindChild("ParticlesHolder").GetChild(0).gameObject.SetActive(true);
		GameObject.Find("HeadBG").transform.FindChild("ParticlesHolder").GetChild(1).gameObject.SetActive(true);
	}

	public void PauseGame()
	{
		LevelGenerator.gameActive = false;
		isPause = true;
		StopCoroutine("TimerDecrease");
		if(GlobalVariables.GameplayMode!=0)
		{
			PauseTips();
			Debug.Log("isMadCustomer "+LevelGenerator.isMadCustomer);
			if(LevelGenerator.isMadCustomer)
			{
				GameObject.Find("HeadBG").transform.FindChild("ParticlesHolder").GetChild(0).gameObject.SetActive(false);
				GameObject.Find("HeadBG").transform.FindChild("ParticlesHolder").GetChild(1).gameObject.SetActive(false);
			}
			GameObject.Find("ClockHolder").GetComponent<Animator>().speed = 0;
			SoundManager.Instance.Pause_TimeCountDown();
		}
	}

	void PauseTips()
	{
		int numOfTips = GameObject.Find("Canvas/MainMenu/AnimationHolder/RestorantHolder/Tips").transform.childCount;
		if(numOfTips>0)
		{
			for(int i=0;i<numOfTips;i++)
			{
				if(GameObject.Find("Canvas/MainMenu/AnimationHolder/RestorantHolder/Tips").transform.GetChild(i).transform.childCount>0)
					GameObject.Find("Canvas/MainMenu/AnimationHolder/RestorantHolder/Tips").transform.GetChild(i).GetChild(0).GetComponent<Animator>().speed = 0;
			}
		}
	}

	void UnpauseTips()
	{
		int numOfTips = GameObject.Find("Canvas/MainMenu/AnimationHolder/RestorantHolder/Tips").transform.childCount;
		if(numOfTips>0)
		{
			for(int i=0;i<numOfTips;i++)
			{
				if(GameObject.Find("Canvas/MainMenu/AnimationHolder/RestorantHolder/Tips").transform.GetChild(i).transform.childCount>0)
					GameObject.Find("Canvas/MainMenu/AnimationHolder/RestorantHolder/Tips").transform.GetChild(i).GetChild(0).GetComponent<Animator>().speed = 1;
			}
		}
	}
}
