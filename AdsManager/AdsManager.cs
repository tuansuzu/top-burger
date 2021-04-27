﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using System;
using UnityEngine.Advertisements;

/**
  * Scene:All
  * Object:WebelinxCMS
  * Description: Scripta za komunikaciju sa native WebelinxCMS-om. Radi i za Android i za iOS. U sebi sadrzi f-je za prikaz Interstial-a, video reklama, za prikaz i sklanjanje banner-a, kao i za uzimanje raznih vrednosti sa servera.
  * Pre upotrebe skripte procitati uputstvo koje se nalazi na putanji Z:\+Unity\Unity Integration Guide for WebelinxCMS\
  **/
public class AdsManager : MonoBehaviour {

	#region AdMob
	[Header("Admob")]
	public string adMobAppID;
	public string interstitalAdMobId;
	public string videoAdMobId;
	InterstitialAd interstitialAdMob;
	private RewardBasedVideoAd rewardBasedAdMobVideo; 
	AdRequest requestAdMobInterstitial, AdMobVideoRequest;
	#endregion
	[Space(15)]
	#region
	[Header("UnityAds")]
	public string unityAdsGameId;
	public string unityAdsVideoPlacementId = "rewardedVideo";
	#endregion

	static AdsManager instance;

	public static AdsManager Instance
	{
		get
		{
			if(instance == null)
				instance = GameObject.FindObjectOfType(typeof(AdsManager)) as AdsManager;
			
			return instance;
		}
	}

	void Awake ()
	{
		gameObject.name = this.GetType().Name;
		DontDestroyOnLoad(gameObject);
		InitializeAds();
	}

	public void ShowInterstitial()
	{
		ShowAdMob();
	}

	public void IsVideoRewardAvailable()
	{
		if(isVideoAvaiable())
		{
			if(Application.loadedLevelName.Equals("MainScene"))
			{
//				if((GameObject.Find("Canvas").GetComponent<MenuManager>().currentPopUpMenu == GameObject.Find("Canvas").transform.FindChild("Menus/ShopHolder").GetComponent<Menu>()) && GameObject.Find("Canvas").GetComponent<MenuManager>().currentPopUpMenu.gameObject.activeSelf)
//				{
//					GameObject.Find("BGFreeCoinsContent").transform.FindChild("WatchVideoButton/WatchVideoButton/NumberCoins").GetComponent<Text>().text = "100";
//				}
//				else
				{
					GameObject.Find("Canvas").transform.FindChild("Menus/WorldChooserMenu/WorldChooseHolder/AnimationHolder/ModeHolder/DeselectedHolder/ChefDeselected/ChefBlue/Available/ButtonSpeedUp").gameObject.SetActive(true);
				}
			}
			else if(Application.loadedLevelName.Equals("GamePlay"))
			{
//				if(GameObject.Find("Canvas").GetComponent<MenuManager>().currentPopUpMenu.name == "ShopHolder")
//					GameObject.Find("BGFreeCoinsContent").transform.FindChild("WatchVideoButton/WatchVideoButton/NumberCoins").GetComponent<Text>().text = "100";
				if(GlobalVariables.GameplayMode == 1)
				{
					if(!LevelGenerator.timeVideoWatched)
						GameObject.Find("PopUps").transform.FindChild("PopUpGoal/AnimationHolder/Body/WatchVideoHolder").gameObject.SetActive(true);
				}
				else if(GlobalVariables.GameplayMode == 0)
				{
					LevelGenerator.videoChampAvailable = true;

				}
			}
		}
		else
		{
//			if((GameObject.Find("Canvas").GetComponent<MenuManager>().currentPopUpMenu != null) &&(GameObject.Find("Canvas").GetComponent<MenuManager>().currentPopUpMenu.name == "ShopHolder") && (GameObject.Find("Canvas").GetComponent<MenuManager>().currentPopUpMenu.gameObject.activeSelf))
//				GameObject.Find("BGFreeCoinsContent").transform.FindChild("WatchVideoButton/WatchVideoButton/NumberCoins").GetComponent<Text>().text = "NO VIDEO AVAILABLE";
			if(Application.loadedLevelName.Equals("MainScene"))
			{
				GameObject.Find("Canvas").transform.FindChild("Menus/WorldChooserMenu/WorldChooseHolder/AnimationHolder/ModeHolder/DeselectedHolder/ChefDeselected/ChefBlue/Available/ButtonSpeedUp").gameObject.SetActive(false);
			}
			else if(Application.loadedLevelName.Equals("GamePlay"))
			{
				if(GlobalVariables.GameplayMode == 1)
				{
					GameObject.Find("PopUps").transform.FindChild("PopUpGoal/AnimationHolder/Body/WatchVideoHolder").gameObject.SetActive(false);
				}
				else if(GlobalVariables.GameplayMode == 0)
				{
					LevelGenerator.videoChampAvailable = false;
				}
			}
		}
	}

	public void ShowVideoReward(int ID)
	{
		if(Advertisement.IsReady(unityAdsVideoPlacementId))
		{
			UnityAdsShowVideo();
		}
		else if(rewardBasedAdMobVideo.IsLoaded())
		{
			AdMobShowVideo();
		}
	}

	private void RequestInterstitial()
	{
		// Initialize an InterstitialAd.
		interstitialAdMob = new InterstitialAd(interstitalAdMobId);

		// Called when an ad request has successfully loaded.
		interstitialAdMob.OnAdLoaded += HandleOnAdLoaded;
		// Called when an ad request failed to load.
		interstitialAdMob.OnAdFailedToLoad += HandleOnAdFailedToLoad;
		// Called when an ad is shown.
		interstitialAdMob.OnAdOpening += HandleOnAdOpened;
		// Called when the ad is closed.
		interstitialAdMob.OnAdClosed += HandleOnAdClosed;
		// Called when the ad click caused the user to leave the application.
		interstitialAdMob.OnAdLeavingApplication += HandleOnAdLeavingApplication;

		// Create an empty ad request.
		requestAdMobInterstitial = new AdRequest.Builder().Build();
		// Load the interstitial with the request.
		interstitialAdMob.LoadAd(requestAdMobInterstitial);
	}

	public void ShowAdMob()
	{
		if(interstitialAdMob.IsLoaded())
		{
			interstitialAdMob.Show();
		}
		else
		{
			interstitialAdMob.LoadAd(requestAdMobInterstitial);
		}
	}

	public void HandleOnAdLoaded(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdLoaded event received");
	}

	public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args.Message);
	}

	public void HandleOnAdOpened(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdOpened event received");
	}

	public void HandleOnAdClosed(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdClosed event received");
		interstitialAdMob.LoadAd(requestAdMobInterstitial);
	}

	public void HandleOnAdLeavingApplication(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdLeftApplication event received");
	}

	private void RequestRewardedVideo()
	{
		// Called when an ad request has successfully loaded.
		rewardBasedAdMobVideo.OnAdLoaded += HandleRewardBasedVideoLoadedAdMob;
		// Called when an ad request failed to load.
		rewardBasedAdMobVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoadAdMob;
		// Called when an ad is shown.
		rewardBasedAdMobVideo.OnAdOpening += HandleRewardBasedVideoOpenedAdMob;
		// Called when the ad starts to play.
		rewardBasedAdMobVideo.OnAdStarted += HandleRewardBasedVideoStartedAdMob;
		// Called when the user should be rewarded for watching a video.
		rewardBasedAdMobVideo.OnAdRewarded += HandleRewardBasedVideoRewardedAdMob;
		// Called when the ad is closed.
		rewardBasedAdMobVideo.OnAdClosed += HandleRewardBasedVideoClosedAdMob;
		// Called when the ad click caused the user to leave the application.
		rewardBasedAdMobVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplicationAdMob;
		// Create an empty ad request.
		AdMobVideoRequest = new AdRequest.Builder().Build();
		// Load the rewarded video ad with the request.
		this.rewardBasedAdMobVideo.LoadAd(AdMobVideoRequest, videoAdMobId);
	}

	public void HandleRewardBasedVideoLoadedAdMob(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
		if(Application.loadedLevelName.Equals("MainScene"))
		{
//			if((GameObject.Find("Canvas").GetComponent<MenuManager>().currentPopUpMenu == GameObject.Find("Canvas").transform.FindChild("Menus/ShopHolder").GetComponent<Menu>()) && GameObject.Find("Canvas").GetComponent<MenuManager>().currentPopUpMenu.gameObject.activeSelf)
//			{
//				GameObject.Find("BGFreeCoinsContent").transform.FindChild("WatchVideoButton/WatchVideoButton/NumberCoins").GetComponent<Text>().text = "100";
//			}
//			else
			{
				GameObject.Find("Canvas").transform.FindChild("Menus/WorldChooserMenu/WorldChooseHolder/AnimationHolder/ModeHolder/DeselectedHolder/ChefDeselected/ChefBlue/Available/ButtonSpeedUp").gameObject.SetActive(true);
			}
		}
		else if(Application.loadedLevelName.Equals("GamePlay"))
		{
//			if(GameObject.Find("Canvas").GetComponent<MenuManager>().currentPopUpMenu.name == "ShopHolder")
//				GameObject.Find("BGFreeCoinsContent").transform.FindChild("WatchVideoButton/WatchVideoButton/NumberCoins").GetComponent<Text>().text = "100";
			if(GlobalVariables.GameplayMode == 1)
			{
				if(!LevelGenerator.timeVideoWatched)
					GameObject.Find("PopUps").transform.FindChild("PopUpGoal/AnimationHolder/Body/WatchVideoHolder").gameObject.SetActive(true);
			}
			else if(GlobalVariables.GameplayMode == 0)
			{
				LevelGenerator.videoChampAvailable = true;

			}
		}
	}

	public void HandleRewardBasedVideoFailedToLoadAdMob(object sender, AdFailedToLoadEventArgs args)
	{
		MonoBehaviour.print("HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);
//		GameObject.Find("BGFreeCoinsContent").transform.FindChild("WatchVideoButton/WatchVideoButton/NumberCoins").GetComponent<Text>().text = "NO VIDEO AVAILABLE";
		if(Application.loadedLevelName.Equals("MainScene"))
		{
			GameObject.Find("Canvas").transform.FindChild("Menus/WorldChooserMenu/WorldChooseHolder/AnimationHolder/ModeHolder/DeselectedHolder/ChefDeselected/ChefBlue/Available/ButtonSpeedUp").gameObject.SetActive(false);
		}
		else if(Application.loadedLevelName.Equals("GamePlay"))
		{
			if(GlobalVariables.GameplayMode == 1)
			{
				GameObject.Find("PopUps").transform.FindChild("PopUpGoal/AnimationHolder/Body/WatchVideoHolder").gameObject.SetActive(false);
			}
			else if(GlobalVariables.GameplayMode == 0)
			{
				LevelGenerator.videoChampAvailable = false;
			}
		}
	}

	public void HandleRewardBasedVideoOpenedAdMob(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
	}

	public void HandleRewardBasedVideoStartedAdMob(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
	}

	public void HandleRewardBasedVideoClosedAdMob(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
		this.rewardBasedAdMobVideo.LoadAd(AdMobVideoRequest, videoAdMobId);
//		if((GameObject.Find("Canvas").GetComponent<MenuManager>().currentPopUpMenu.gameObject.name == "MainMenu") && GameObject.Find("Canvas").GetComponent<MenuManager>().currentPopUpMenu.gameObject.activeSelf && Application.loadedLevelName.Equals("MainScene"))
//		{
////			GameObject.Find("BGFreeCoinsContent").transform.FindChild("WatchVideoButton/WatchVideoButton/NumberCoins").GetComponent<Text>().text = "100";
//			GameObject.Find("CoinHomeHolder").GetComponent<Animator>().Play("HomeCoinsWatchVideo");
//			StartCoroutine(GlobalVariables.Instance.moneyCounter(100,GameObject.Find("CoinHomeText").GetComponent<Text>()));
//		}
//		else
//		{
//			if(Application.loadedLevelName.Equals("GamePlay"))
//			{
//				if(GlobalVariables.GameplayMode == 1)
//				{
//					LevelGenerator.timeVideoWatched = true;
//					GameObject.Find("PopUpGoal/AnimationHolder/Body/WatchVideoHolder").SetActive(false);
//					GameObject.Find("TimerText").GetComponent<Text>().text = "01:15";
//					GameObject.Find("LevelTimer").GetComponent<LevelTimer>().counterTime = 75;
//				}
//				else
//				{
//					LevelGenerator.videoChampWatched = true;
//					GameObject.Find("LevelGenerator").GetComponent<LevelGenerator>().CloseWatchVideoReview();
//				}
//
//			}
//			else
//			{
//				if(Timer.counterTime<=1800)
//				{
//					Timer.counterTime=0;
//
//					Debug.Log("OnFullScreenADdisplayed psotqavi na 0");
//					GameObject.Find("Available").SetActive(false);
//					GameObject.Find("ChefDeselected").GetComponent<Button>().interactable = true;
//					GameObject.Find("ChefDeselected/ChefBlue").GetComponent<Image>().color = Color.white;
//					PlayerPrefs.SetFloat("ChefTimer", Timer.counterTime);
//					PlayerPrefs.SetInt("ChefPlayed",1);
//					PlayerPrefs.Save();
//				}
//				else
//				{
//					Timer.counterTime -=1800;
//					PlayerPrefs.SetInt("ChefPlayed",0);
//					PlayerPrefs.SetFloat("ChefTimer", Timer.counterTime);
//					PlayerPrefs.Save();
//				}
//			}
//		}


	}

	public void HandleRewardBasedVideoRewardedAdMob(object sender, Reward args)
	{
		string type = args.Type;
		double amount = args.Amount;
		MonoBehaviour.print("HandleRewardBasedVideoRewarded event received for " + amount.ToString() + " " + type);
//		this.rewardBasedAdMobVideo.LoadAd(AdMobVideoRequest, videoAdMobId);
		if(Application.loadedLevelName.Equals("MainScene"))
		{
			//			GameObject.Find("BGFreeCoinsContent").transform.FindChild("WatchVideoButton/WatchVideoButton/NumberCoins").GetComponent<Text>().text = "100";

			if(GameObject.Find("Canvas").GetComponent<MenuManager>().currentMenu.name.Contains("MainMenu"))
			{
				StartCoroutine(GlobalVariables.Instance.moneyCounter(100,GameObject.Find("CoinHomeText").GetComponent<Text>()));
				GameObject.Find("CoinHomeHolder").GetComponent<Animator>().Play("HomeCoinsWatchVideo");
			}
			else
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
					Timer.counterTime -=1800;
					PlayerPrefs.SetInt("ChefPlayed",0);
					PlayerPrefs.SetFloat("ChefTimer", Timer.counterTime);
					PlayerPrefs.Save();
				}
			}
		}
		else
		{
			if(Application.loadedLevelName.Equals("GamePlay"))
			{
				if(GlobalVariables.GameplayMode == 1)
				{
					LevelGenerator.timeVideoWatched = true;
					GameObject.Find("PopUpGoal/AnimationHolder/Body/WatchVideoHolder").SetActive(false);
					GameObject.Find("TimerText").GetComponent<Text>().text = "01:15";
					GameObject.Find("LevelTimer").GetComponent<LevelTimer>().counterTime = 75;
				}
				else
				{
					LevelGenerator.videoChampWatched = true;
					GameObject.Find("LevelGenerator").GetComponent<LevelGenerator>().CloseWatchVideoReview();
				}

			}
		}
	}

	public void HandleRewardBasedVideoLeftApplicationAdMob(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
	}

	void InitializeAds()
	{
		MobileAds.Initialize(adMobAppID);
		this.rewardBasedAdMobVideo = RewardBasedVideoAd.Instance;
		this.RequestRewardedVideo();
		Advertisement.Initialize(unityAdsGameId);
		RequestInterstitial();
	}


	void AdMobShowVideo()
	{
		rewardBasedAdMobVideo.Show();	
	}

	void UnityAdsShowVideo()
	{
		ShowOptions options = new ShowOptions();
		options.resultCallback = HandleShowResultUnity;

		Advertisement.Show(unityAdsVideoPlacementId, options);
	}

	void HandleShowResultUnity (ShowResult result)
	{
		if(result == ShowResult.Finished) {
			Debug.Log("Video completed - Offer a reward to the player");
			if(Application.loadedLevelName.Equals("MainScene"))
			{
				//			GameObject.Find("BGFreeCoinsContent").transform.FindChild("WatchVideoButton/WatchVideoButton/NumberCoins").GetComponent<Text>().text = "100";

				if(GameObject.Find("Canvas").GetComponent<MenuManager>().currentMenu.name.Contains("MainMenu"))
				{
					SoundManager.Instance.Play_Coins();
					StartCoroutine(GlobalVariables.Instance.moneyCounter(100,GameObject.Find("CoinHomeText").GetComponent<Text>()));
					GameObject.Find("CoinHomeHolder").GetComponent<Animator>().Play("HomeCoinsWatchVideo");
				}
				else
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
						Timer.counterTime -=1800;
						PlayerPrefs.SetInt("ChefPlayed",0);
						PlayerPrefs.SetFloat("ChefTimer", Timer.counterTime);
						PlayerPrefs.Save();
					}
				}
			}
			else
			{
				if(Application.loadedLevelName.Equals("GamePlay"))
				{
					if(GlobalVariables.GameplayMode == 1)
					{
						LevelGenerator.timeVideoWatched = true;
						GameObject.Find("PopUpGoal/AnimationHolder/Body/WatchVideoHolder").SetActive(false);
						GameObject.Find("TimerText").GetComponent<Text>().text = "01:15";
						GameObject.Find("LevelTimer").GetComponent<LevelTimer>().counterTime = 75;
					}
					else
					{
						LevelGenerator.videoChampWatched = true;
						GameObject.Find("LevelGenerator").GetComponent<LevelGenerator>().CloseWatchVideoReview();
					}

				}
			}

		}else if(result == ShowResult.Skipped) {
			Debug.LogWarning("Video was skipped - Do NOT reward the player");

		}else if(result == ShowResult.Failed) {
			Debug.LogError("Video failed to show");
		}
	}

	bool isVideoAvaiable()
	{
		#if !UNITY_EDITOR
		if(Advertisement.IsReady(unityAdsVideoPlacementId))
		{
			return true;
		}
		else if(rewardBasedAdMobVideo.IsLoaded())
		{
			return true;
		}
		#endif
		return false;
	}
}
