using UnityEngine;
using System.Collections;
using UnityEngine.UI;

///<summary>
///<para>Scene:All/NameOfScene/NameOfScene1,NameOfScene2,NameOfScene3...</para>
///<para>Object:N/A</para>
///<para>Description: Sample Description </para>
///</summary>

public class TipsCollect : MonoBehaviour {

	[SerializeField]
	public int tipsAmount;
	public void Collect()
	{
		if(LevelGenerator.numberOfTips>0)
		{
			LevelGenerator.numberOfTips--;
			SoundManager.Instance.Play_TipsCollect();
		}
		Debug.Log("smanjen, ukupno ih ima "+LevelGenerator.numberOfTips);
		if(transform.GetChild(0).GetChild(2).gameObject.activeSelf)
		{
			transform.GetChild(0).GetComponent<Animator>().Play("TipsCollected");
		}
		else
		{
			transform.GetChild(0).GetComponent<Animator>().Play("TipsCollectedShort");
		}
			
	}

	public void ContinueTutorial()
	{
		if(!LevelGenerator.tipsTutorial)
		{
		LevelGenerator.gameActive = true;
		Destroy(GameObject.Find("ArrowTips"));
		if(GlobalVariables.GameplayMode!=0)
		{
			GameObject.Find("ClockHolder").GetComponent<Animator>().speed = 1;
			GameObject.Find("LevelTimer").GetComponent<LevelTimer>().StartTimerDecreaseForTut();
//			StartCoroutine ("TimerDecrease");
		}
		if(GlobalVariables.GameplayMode==1)
		{
			GameObject.Find("CustomerTimerBar").GetComponent<CustomerTimer>().ContinueCustomerTimer();
		}
		GameObject.Find("TipsTutorialHolder").GetComponent<Animator>().Play("TipsTutorialDeparting");
		LevelGenerator.tipsTutorial = true;
		}
	}
}
