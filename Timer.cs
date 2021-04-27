using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

///<summary>
///<para>Scene:All/NameOfScene/NameOfScene1,NameOfScene2,NameOfScene3...</para>
///<para>Object:N/A</para>
///<para>Description: Sample Description </para>
///</summary>

public class Timer : MonoBehaviour
{

	public Text timerText;
	public static float counterTime = 7200;
	float counterTimeStart, yellowLimit, redLimit;
	int minutes, seconds, hours = 0;
	float timeUnit;
	string previousDateTimeString, currentDateTimeString;
	public GameObject Available, ChefDeselected;

	// Use this for initialization
	void Start ()
	{
		if(PlayerPrefs.HasKey("ChefTimer"))
		{
			previousDateTimeString = PlayerPrefs.GetString("VremeQuit");
			currentDateTimeString = System.DateTime.Now.ToString();
			counterTime = PlayerPrefs.GetFloat("ChefTimer");
			TimeSpan resultTime = DateTime.Parse(currentDateTimeString) - DateTime.Parse(previousDateTimeString);

		

			counterTime -= (float) resultTime.TotalSeconds;
			if(counterTime<=0)
			{
				PlayerPrefs.SetInt("ChefPlayed",1);
				PlayerPrefs.Save();
				Available.SetActive(false);
				ChefDeselected.GetComponent<Button>().interactable = true;
				ChefDeselected.transform.FindChild("ChefBlue").GetComponent<Image>().color = Color.white;
			}
			else
			{
				PlayerPrefs.SetInt("ChefPlayed",0);
				PlayerPrefs.Save();
				timeUnit = 100 / counterTime;
				yellowLimit = counterTime * 2 / 3;
				redLimit = counterTime / 3;
				counterTimeStart = counterTime;
				SetTimerText();
				StartCoroutine ("TimerDecrease");
			}
			

		}
		else
		{
			PlayerPrefs.SetInt("ChefPlayed",0);
			Available.SetActive(false);
			ChefDeselected.GetComponent<Button>().interactable = true;
			ChefDeselected.transform.FindChild("ChefBlue").GetComponent<Image>().color = Color.white;
		}



	}

	public void StartTimer()
	{
//		if(transform.gameObject.active)
//			StartCoroutine ("TimerDecrease");
	}

	IEnumerator TimerDecrease()
	{
		while (counterTime>=1)
		{
			yield return new WaitForSeconds(1);
			counterTime--;
			SetTimerText();
		}
		GameObject.Find("Available").SetActive(false);
		GameObject.Find("ChefDeselected").GetComponent<Button>().interactable = true;
		GameObject.Find("ChefDeselected/ChefBlue").GetComponent<Image>().color = Color.white;
		PlayerPrefs.Save();
	}

	void SetTimerText()
	{
		minutes = (int)counterTime / 60;
		seconds = (int)counterTime % 60;

		if(minutes > 60)
		{
			hours = minutes/60;
			minutes = minutes-hours*60;
		}
		else
		{
			hours = 0;
		}

		if (hours<1 && seconds < 1 && minutes < 1) 
		{
			timerText.text = "00:00:00";
			GameObject.Find("Available").SetActive(false);
			GameObject.Find("ChefDeselected").GetComponent<Button>().interactable = true;
			GameObject.Find("ChefDeselected/ChefBlue").GetComponent<Image>().color = Color.white;
		}
		else if (seconds >= 0 && seconds <= 9) 
		{
			if (minutes < 10) 
			{
				timerText.text = "0" + hours.ToString () + ":" +"0" + minutes.ToString () + ":" + "0" + seconds.ToString ();
			} 
			else 
			{
				timerText.text = "0" + hours.ToString () + ":" +minutes.ToString () + ":" + "0" + seconds.ToString ();
			}
		} 
		else 
		{
			
			if (minutes < 10) 
			{
				timerText.text = "0" + hours.ToString () + ":" +"0" + minutes.ToString () + ":" + seconds.ToString ();
			} 
			else
			{
				timerText.text = "0" + hours.ToString () + ":" + minutes.ToString () + ":" + seconds.ToString ();
			}
		}
	}



	public void AddOrRemoveTime(int numerOfSeconds)
	{
		if (counterTime + numerOfSeconds < counterTimeStart)
		{
			counterTime += numerOfSeconds;
		}
		else 
		{
			counterTime = counterTimeStart;
		}
		SetTimerText();
	}
	
}
