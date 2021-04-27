using UnityEngine;
using System.Collections;

/**
  * Scene:All
  * Object:SoundManager
  * Description: Skripta zaduzena za zvuke u apliakciji, njihovo pustanje, gasenje itd...
  **/
public class SoundManager : MonoBehaviour {

	public static int musicOn = 1;
	public static int soundOn = 1;
	public static bool forceTurnOff = false;

	public AudioSource buttonClick;
	public AudioSource menuMusic;
	public AudioSource gameplayMusic;
	public AudioSource Ingredient;
	public AudioSource WrongIngredient;
	public AudioSource SideFood;
	public AudioSource CustomerArrival;
	public AudioSource CustomerMadDeparting;
	public AudioSource CustomerMad;
	public AudioSource CustomerHappyDeparting;
	public AudioSource UnlockNewItem;
	public AudioSource Coins;
	public AudioSource NoMoney;
	public AudioSource PowerUp;
	public AudioSource Boost;
	public AudioSource Win;
	public AudioSource Lose;
	public AudioSource TimeCountDown;
	public AudioSource TipsCollect;
	public AudioSource SpecialOffer;
	public AudioSource WorldChosse;
	public AudioSource LoadingArrival;
	public AudioSource LoadingDeparting;
	public AudioSource LockedItem;

	public static float resumeCountdownTime = 0f;



	static SoundManager instance;

	public static SoundManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = GameObject.FindObjectOfType(typeof(SoundManager)) as SoundManager;
			}

			return instance;
		}
	}

	void Start () 
	{
		DontDestroyOnLoad(this.gameObject);

		if(PlayerPrefs.HasKey("SoundOn"))
		{
			musicOn = PlayerPrefs.GetInt("MusicOn");
			soundOn = PlayerPrefs.GetInt("SoundOn");
		}

		Screen.sleepTimeout = SleepTimeout.NeverSleep; 
	}

	public void Play_TipsCollect()
	{
		if(TipsCollect.clip != null && soundOn == 1)
			TipsCollect.Play();
	}
	
	public void Play_SpecialOffer()
	{
		if(SpecialOffer.clip != null && soundOn == 1)
			SpecialOffer.Play();
	}

	public void Play_WorldChosse()
	{
		if(WorldChosse.clip != null && soundOn == 1)
			WorldChosse.Play();
	}

	public void Play_PowerUp()
	{
		if(PowerUp.clip != null && soundOn == 1)
			PowerUp.Play();
	}

	public void Play_LoadingArrival()
	{
		if(LoadingArrival.clip != null && soundOn == 1)
			LoadingArrival.Play();
	}
	
	public void Play_LoadingDeparting()
	{
		if(LoadingDeparting.clip != null && soundOn == 1)
			LoadingDeparting.Play();
	}

	public void Play_LockedItem()
	{
		if(LockedItem.clip != null && soundOn == 1)
			LockedItem.Play();
	}

	public void Play_Coins()
	{
		if(Coins.clip != null && soundOn == 1)
			Coins.Play();
	}
	
	public void Play_NoMoney()
	{
		if(NoMoney.clip != null && soundOn == 1)
			NoMoney.Play();
	}
	
	public void Play_Win()
	{
		if(Win.clip != null && soundOn == 1)
			Win.Play();
	}
	
	public void Play_Boost()
	{
		if(Boost.clip != null && soundOn == 1)
			Boost.Play();
	}
	
	public void Play_Lose()
	{
		if(Lose.clip != null && soundOn == 1)
			Lose.Play();
	}
	
	public void Play_TimeCountDown()
	{
		if(TimeCountDown.clip != null && soundOn == 1)
		{
			TimeCountDown.time = 0;
			TimeCountDown.Play();
		}
	}

	public void Pause_TimeCountDown()
	{
		if(TimeCountDown.clip !=null && TimeCountDown.isPlaying)
		{
			TimeCountDown.Pause();
			resumeCountdownTime= TimeCountDown.time;
		}
	}

	public void Resume_TimeCountDown()
	{
		if(TimeCountDown.clip !=null && resumeCountdownTime>0)
		{
			TimeCountDown.time = resumeCountdownTime;
			TimeCountDown.Play();
			resumeCountdownTime = 0;
		}
	}

	public void Stop_TimeCountDown()
	{
		if(TimeCountDown.clip != null && soundOn == 1)
		{
			TimeCountDown.Stop();
		}
	}

	public void Stop_CustomerHappyDeparting()
	{
		if(CustomerHappyDeparting.clip != null && soundOn == 1)
		{
			CustomerHappyDeparting.Stop();
		}
	}

	public void Play_CustomerHappyDeparting()
	{
		if(CustomerHappyDeparting.clip != null && soundOn == 1)
			CustomerHappyDeparting.Play();
	}
	
	public void Play_UnlockNewItem()
	{
		if(UnlockNewItem.clip != null && soundOn == 1)
			UnlockNewItem.Play();
	}

	public void Play_Ingredient()
	{
		if(Ingredient.clip != null && soundOn == 1)
			Ingredient.Play();
	}

	public void Play_WrongIngredient()
	{
		if(WrongIngredient.clip != null && soundOn == 1)
			WrongIngredient.Play();
	}
	
	public void Play_SideFood()
	{
		if(SideFood.clip != null && soundOn == 1)
			SideFood.Play();
	}
	
	public void Play_CustomerArrival()
	{
		if(CustomerArrival.clip != null && soundOn == 1)
			CustomerArrival.Play();
	}
	
	public void Play_CustomerMadDeparting()
	{
		if(CustomerMadDeparting.clip != null && soundOn == 1 )
			CustomerMadDeparting.Play();
	}

	public void Stop_CustomerMadDeparting()
	{
		if(CustomerMadDeparting.clip != null && soundOn == 1)
			CustomerMadDeparting.Stop();
	}

	public void Play_CustomerMad()
	{
		if(CustomerMad.clip != null && soundOn == 1 && !CustomerMad.isPlaying)
			CustomerMad.Play();
	}

	public void Stop_CustomerMad()
	{
		if(CustomerMad.clip != null && soundOn == 1)
			CustomerMad.Stop();
	}

	public void Play_ButtonClick()
	{
		if(buttonClick.clip != null && soundOn == 1)
			buttonClick.Play();
	}

	public void Play_MenuMusic()
	{
		if(menuMusic.clip != null && musicOn == 1)
			menuMusic.Play();
	}

	public void Stop_MenuMusic()
	{
		if(menuMusic.clip != null && musicOn == 1)
			menuMusic.Stop();
	}

	public void Play_GameplayMusic()
	{
		if(gameplayMusic.clip != null && musicOn == 1)
		{
			gameplayMusic.Play();
		}
	}

	public void Stop_GameplayMusic()
	{
		if(gameplayMusic.clip != null && musicOn == 1)
		{
			gameplayMusic.Stop();
		}
	}

	/// <summary>
	/// Corutine-a koja za odredjeni AudioSource, kroz prosledjeno vreme, utisava AudioSource do 0, gasi taj AudioSource, a zatim vraca pocetni Volume na pocetan kako bi AudioSource mogao opet da se koristi
	/// </summary>
	/// <param name="sound">AudioSource koji treba smanjiti/param>
	/// <param name="time">Vreme za koje treba smanjiti Volume/param>
	IEnumerator FadeOut(AudioSource sound, float time)
	{
		float originalVolume = sound.volume;
		while(sound.volume != 0)
		{
			sound.volume = Mathf.MoveTowards(sound.volume, 0, time);
			yield return null;
		}
		sound.Stop();
		sound.volume = originalVolume;
	}
	
}
