using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAudioRead : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  void OnAudioFilterRead(float[] data , int channels){
    print(data.Length);
  }
}
