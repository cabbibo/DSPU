using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoAudioLife : MonoBehaviour {

  public AudioForm form;
  public AudioLife life;

  public float sampleRate = 44100;
  public float waveLengthInSeconds = 2.0f;

  public float frequency;
 
    AudioSource audioSource;
    int timeIndex = 0;
    int oTimeIndex;

    float oTime;

  float[] d;

  void Start(){
    var configuration = AudioSettings.GetConfiguration();
    print(configuration.dspBufferSize);
    print(configuration.sampleRate);
    d = new float[2048];
    for(int i = 0; i < d.Length; i++ ){
      d[i] = Mathf.Sin( (float)i * .1f);
    }
  }
  void FixedUpdate(){
    int dTime = timeIndex - oTimeIndex;
    //print( dTime);

    float dFloat = Time.time - oTime;

    print( dFloat );

    oTimeIndex = timeIndex;
    oTime = Time.time;



  }


  void OnAudioFilterRead( float[] data , int channels ){
    if( form.bufferMade ){

      //Creates a sinewave
      for(int i = 0; i < data.Length; i+= channels){
        data[i] = CreateSine( timeIndex , frequency , sampleRate );
        data[i+1] = CreateSine( timeIndex , frequency * 2 , sampleRate );
        timeIndex ++;
      }
    }
  }

     public float CreateSine(int timeIndex, float frequency, float sampleRate)
    {
        return Mathf.Sin(2 * Mathf.PI * timeIndex * frequency / sampleRate);
    }
}
