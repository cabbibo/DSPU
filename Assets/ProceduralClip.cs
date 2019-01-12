using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralClip : MonoBehaviour {

  public AudioLife life;
  public AudioForm form;

	 public int position = 0;
    public int samplerate = 44100;
    public float frequency = 440;
    public int channels;

    public int clipLength;

    public float oTime;

    public int positionInClip;

    public float[] second1;
    public float[] second2;
    public float[] second3;

    public float[] dataBuffer;

    public int timeIndex = 0;

    public float timeAudio;
    public float secondAudio;
    public float timeFixed;
    public float secondFixed;

    public bool started;
    public float startTime;

    public float baseTime;

    void Start()
    {

      second1 = new float[samplerate];
      second2 = new float[samplerate];
      second3 = new float[samplerate];

      for( int i = 0; i < samplerate; i++ ){
        float t = ((float)i/(float)samplerate);
        //second1[i] = Mathf.Sin(t * 2 * Mathf.PI* frequency);
      }

      for( int i = 0; i < samplerate; i++ ){
        float t = 1+((float)i/(float)samplerate);
        //second2[i] = Mathf.Sin(t * 2 * Mathf.PI*frequency);
      }

       for( int i = 0; i < samplerate; i++ ){
        float t = 2+((float)i/(float)samplerate);
        //second3[i] = Mathf.Sin(t * 2 * Mathf.PI * frequency);
      }


      started = true;
      startTime = Time.time-.5f;



    }

    void OnAudioFilterRead(float[] data , int channels ){

      if( started ){

   //Creates a sinewave
      for(int i = 0; i < data.Length; i+=channels){

      

        //idInBuffer = timeIndex; //((float)timeIndex / (float)samplerate);

        float d = 1;
        if( secondAudio == 0 ){
          d = second1[timeIndex];
        }else if( secondAudio == 1 ){
          d = second2[timeIndex];
        }else if( secondAudio == 2 ){
          d = second3[timeIndex];
        }

        data[i]   = d;//CreateSine();
        data[i+1] = d;//CreateSine();
        
        timeIndex ++;
        if( timeIndex == samplerate){
          timeIndex = 0;
          secondAudio ++;
          secondAudio %= 3;
        }

       // timeIndex %= samplerate;
      }

      timeAudio = (float)timeIndex / (float)samplerate;
    }

    }

    public float CreateSine(){
      return Mathf.Sin(2 * Mathf.PI * timeIndex * frequency / (float)samplerate);
    }

    void FixedUpdate(){
      timeFixed = Time.time-startTime;

//      print( timeFixed );
      
      if( timeFixed > 1 ){ 
        timeFixed-=1;
        startTime += 1;
        secondFixed ++;
        secondFixed %=3;
        baseTime ++;

        life.shader.SetFloat("_BaseTime",baseTime);
        life.Live();

        if(secondFixed == 0 ){
          form._buffer.GetData(second1);
          for( int i = 0; i < 100; i++ ){
            print( second1[i]);
          }
        }else if( secondFixed == 1 ){
          form._buffer.GetData(second2);
        }else if( secondFixed == 2 ){
          form._buffer.GetData(second3);
        }

/*
              if( secondFixed == 0 ){
        for( int i = 0; i < samplerate; i++){
          float t = baseTime + ((float)i/(float)samplerate); 
          second1[i] = getVal(t); 
        }
      }else if( secondFixed == 1 ){
        for( int i = 0; i < samplerate; i++){
          float t = baseTime + ((float)i/(float)samplerate); 
          second2[i] = getVal(t); 
        }
      }else if( secondFixed == 2 ){
        for( int i = 0; i < samplerate; i++){
          float t = baseTime + ((float)i/(float)samplerate); 
          second3[i] = getVal( t );//Mathf.Sin( t * 2 * Mathf.PI * frequency ); 
        }
      }*/

      }




    }


    float getVal(float t){
      return Mathf.Cos(2*Mathf.PI*( frequency*t + 40*Mathf.Sin(2*Mathf.PI*t)/(2 * Mathf.PI) ));
    }

}


