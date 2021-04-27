using UnityEngine;
using System.Collections;
using UnityEngine.UI;

///<summary>
///<para>Scene:All/NameOfScene/NameOfScene1,NameOfScene2,NameOfScene3...</para>
///<para>Object:N/A</para>
///<para>Description: Sample Description </para>
///</summary>

public class CustomerTimer : MonoBehaviour
{

	Image timerImage;
	public float counterTime;
	GameObject CharacterChangerObject;
	float counterTimeStart, yellowLimit, redLimit;
	int minutes, seconds;
	float timeUnit;
	Color MyGreen = new Color( 0.01961f,  0.97255f,  0.45490f);
	Animator Character;
	GameObject CorrectHolder;

	// Use this for initialization
	void Start ()
	{
		CharacterChangerObject = GameObject.Find("CharacterHolder");
		CorrectHolder = GameObject.Find("CorrectHolder");
		Character = GameObject.Find("CharacterHolder/AnimationHolder").GetComponent<Animator>();
		timeUnit = 100 / counterTime;
		yellowLimit = counterTime * 2 / 3;
		redLimit = counterTime / 3;
		if(LevelGenerator.happyTimePowerActive)
		{
			counterTime=13;
		}
		else
		{
			counterTime=12;
		}
		counterTimeStart = counterTime;
		timerImage = transform.GetComponent<Image> ();

	}

	IEnumerator TimerDecrease()
	{

		while (LevelGenerator.customerActive && LevelGenerator.gameActive)
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
			if(counterTime==0)
			{
//				CorrectHolder.transform.GetChild(1).GetComponent<Image>().sprite = CorrectSprite;
//				CorrectHolder.GetComponent<Animator>().Play("CorrectArrival");
//				StopCustomerTimer();

				if(LevelGenerator.hintsPowerActive)
				{
					for(int i=0;i<14;i++)
						GameObject.Find("IngredientsHolder").transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(false);

				}
				else if(LevelGenerator.tutorial)
				{
					for(int i=0;i<14;i++)
						GameObject.Find("IngredientsHolder").transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(false);
				}

				SoundManager.Instance.Stop_CustomerMad();
				SoundManager.Instance.Play_CustomerMadDeparting();
				Character.Play("CharacterMadDeparting");
				ResetCustomerTimer();
				Invoke("GenerateNewOrder",0.3f);
//			
			}
			else if(counterTime<4)
			{
				SoundManager.Instance.Play_CustomerMad();
				LevelGenerator.isMadCustomer = true;
				Character.Play("CharacterMad");
			}
			else if(counterTime<8)
			{
				Character.Play("CharacterNervous");
			}
			else
			{
				Character.Play("CharacterIdle");
			}
		}

	}

	void GenerateNewOrder()
	{
		GameObject.Find("LevelGenerator").GetComponent<LevelGenerator>().GenerateNewOrder();
	}

	public void ContinueCustomerTimer()
	{
		StartCoroutine ("TimerDecrease");
	}

	IEnumerator TimeFreezeCoroutine()
	{
		LevelGenerator.customerActive = false;
		yield return new WaitForSeconds (5);
		LevelGenerator.customerActive = true;
		StartCoroutine ("TimerDecrease");
	}

	public void TimeFreeze()
	{
		StartCoroutine ("TimeFreezeCoroutine");
	}

	public void AddTime()
	{
		if (counterTime + 10 < counterTimeStart)
		{
			counterTime += 10;
		}
		else 
		{
			counterTime = counterTimeStart;
		}

		timerImage.fillAmount += 10*timeUnit*0.01f;

	}

	public void StopCustomerTimer()
	{
		StopCoroutine("TimerDecrease");
		LevelGenerator.customerActive = false;
	}

	public void StartCustomerTimer()
	{
		LevelGenerator.customerActive = true;
		StartCoroutine("TimerDecrease");
	}

	public void ResetCustomerTimer()
	{
//		Invoke("NewCharacter",1f);
		LevelGenerator.isMadCustomer = false;
		if(LevelGenerator.happyTimePowerActive)
		{
			counterTime=13;
		}
		else
		{
			counterTime=12;
		}
		StopCustomerTimer();
		timerImage.color=MyGreen;
		timerImage.fillAmount=1f;
		Invoke ("StartCustomerTimer",0.7f);
	}

//	void NewCharacter()
//	{
//		CharacterChangerObject.GetComponent<CharacterChanger>().ChangeCharacter(LevelGenerator.currentCharacter);
//	}
}
