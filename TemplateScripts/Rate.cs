﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

  /**
  * Scene: MainScene
  * Object:PopUpRate
  * Description: Skripta koja je zaduzena za PopUpRate Menu i rejtovanje aplikacije
  **/
public class Rate : MonoBehaviour {

	string rateURL;
	[Header("Linkovi za RATE")]
	string rateUrlAndroid = "https://play.google.com/store/apps/details?id=";
	public string rateUrlIOS;
	public static int appStartedNumber,alreadyRated;
	bool rateClicked = false;

	// Use this for initialization
	void Awake () {

		if(!PlayerPrefs.HasKey("appStartedNumber"))
		{
			appStartedNumber = 0;
			Debug.Log(Application.buildGUID);
		}
		else
		{
			appStartedNumber = PlayerPrefs.GetInt("appStartedNumber");
		}
		#if UNITY_ANDROID
		rateURL = rateUrlAndroid+Application.buildGUID;
		#elif UNITY_IOS
		rateURL = rateUrlIOS;
		#endif
	}

	/// <summary>
	/// Funkcija koja zavisno od broja(1-5), vodi(4-5) ili ne vodi(1-3) korisnika da rejtuje aplikaciju
	/// </summary>
	/// /// <param name="number">Game object koji se prosledjuje i koji treba da se upali</param>
	public void RateClicked(int number)
	{
		if(!rateClicked)
		{
			alreadyRated = 1;
			PlayerPrefs.SetInt("alreadyRated",alreadyRated);
			PlayerPrefs.Save();
			rateClicked=true;
			StartCoroutine("ActivateStars",number);
		}
	}

	/// <summary>
	/// Coroutine koja zavisno od broja(1-5), vodi(4-5) ili ne vodi(1-3) korisnika da rejtuje aplikaciju,  i pamti da je korisnik rate-ovao aplikaciju, i samim tim vise ne izlazi Rate PopUpMenu
	/// </summary>
	/// <param name="number">Game object koji se prosledjuje i koji treba da se upali</param>
	IEnumerator ActivateStars(int number)
	{
		switch(number)
		{
		case 1: case 2: case 3:
			for(int i=1;i<=number;i++)
			{
				GameObject.Find("PopUpRate/AnimationHolder/Body/ContentHolder/StarsHolder/StarBG"+i+"/Star"+i).GetComponent<Image>().enabled = true;
			}
			yield return new WaitForSeconds(0.5f);
			HideRateMenu(GameObject.Find("PopUpRate"));
			break;
		case 4: case 5:
			for(int i=1;i<=number;i++)
			{
				GameObject.Find("PopUpRate/AnimationHolder/Body/ContentHolder/StarsHolder/StarBG"+i+"/Star"+i).GetComponent<Image>().enabled = true;
			}
			Application.OpenURL(rateURL);
			yield return new WaitForSeconds(0.5f);
			HideRateMenu(GameObject.Find("PopUpRate"));
			yield return new WaitForSeconds(0.5f);

			break;
		}
		yield return null;
		alreadyRated = 1;
		PlayerPrefs.SetInt("alreadyRated",alreadyRated);
		PlayerPrefs.Save();

	}

	/// <summary>
	/// F-ja koja prikazuje Rate Menu
	/// </summary>
	public void ShowRateMenu()
	{
		transform.GetComponent<Animator>().Play("Open");
	}

	/// <summary>
	/// F-ja koja sklanja Rate Menu
	/// </summary>
	/// <param name="menu">Game object koji se prosledjuje i koji treba da se skloni</param>
	public void HideRateMenu(GameObject menu)
	{
		GameObject.Find("Canvas").GetComponent<MenuManager>().ClosePopUpMenu(menu);
	}

	/// <summary>
	/// F-ja koja sklanja Rate Menu, i pamti da korisnik nece da rate-uje aplikaciju, i samim tim vise ne izlazi Rate PopUpMenu
	/// </summary>
	/// <param name="menu">Game object koji se prosledjuje i koji treba da se skloni</param>
	public void NoThanks()
	{

		alreadyRated = 1;
		PlayerPrefs.SetInt("alreadyRated",alreadyRated);
		PlayerPrefs.Save();
		HideRateMenu(GameObject.Find("PopUpRate"));
	}
}
