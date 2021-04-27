using UnityEngine;
using System.Collections;
using UnityEngine.UI;

///<summary>
///<para>Scene:All/NameOfScene/NameOfScene1,NameOfScene2,NameOfScene3...</para>
///<para>Object:N/A</para>
///<para>Description: Sample Description </para>
///</summary>
[ExecuteInEditMode]
public class SetSortingLayer : MonoBehaviour {
	public string sortingLayerName;
	public int sortingOrder = 0;
	Renderer _renderer;
	ParticleSystem _particleSystem;
	TrailRenderer _trailRenderer;
	
	void Awake () {
		_renderer = GetComponent<Renderer> ();
		_particleSystem = GetComponent<ParticleSystem> ();
		_trailRenderer = GetComponent<TrailRenderer> ();
		
		if(_renderer!=null)
		{
			Debug.Log(transform.name.Colored(Colors.lime)+sortingLayerName);
			_renderer.sortingLayerName = sortingLayerName;
			_renderer.sortingOrder = sortingOrder;
		}
		
		if (_particleSystem != null)
		{
			Debug.Log(transform.name.Colored(Colors.lime)+sortingLayerName);
			
			_particleSystem.GetComponent<Renderer>().sortingLayerName = sortingLayerName;
			_particleSystem.GetComponent<Renderer>().sortingOrder = sortingOrder;
		}
		
		if (_trailRenderer != null)
		{
			Debug.Log(transform.name.Colored(Colors.lime)+sortingLayerName);
			
			_trailRenderer.sortingLayerName = sortingLayerName;
			_trailRenderer.sortingOrder = sortingOrder;
		}


	}
	
}
