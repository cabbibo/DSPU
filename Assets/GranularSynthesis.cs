using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GranularSynthesis : MonoBehaviour {


public AudioClip clip;

     float[] clipData;

	public int position = 0;
    public int sampleRate = 1024;
    public float frequency = 440;
    public int channels;


    public float oTime;

    public int positionInClip;

    public int timeIndex = 0;

    public float timeAudio;
    public float timeFixed;

    public bool started;
    public float startTime;

    public int totalClipSamples;

    public float lastGrainTime;

    public float clipLength;
    public float grainLength;
    

    void OnEnable()
    {



        //AudioSettings.outputSampleRate = clip.frequency;
        // setting our sample rate
        sampleRate = AudioSettings.outputSampleRate;

        // getting our full clip data into a data buffer
        totalClipSamples = clip.samples * clip.channels;
        clipData = new float[totalClipSamples];

        clip.GetData(clipData,0);

        started = true;

        clipLength = clip.length;



    }


    public List<Grain> grains = new List<Grain>();

    void OnAudioFilterRead(float[] data , int channels ){


        if( started ){  


        //Creates a sinewave
        for(int i = 0; i < data.Length; i+=channels){
            
            timeIndex ++;
            timeAudio = (float)timeIndex / (float)sampleRate;

            for( int j = 0; j< grains.Count; j++ ){
                data[i]   += grains[j].getSample(0);
                data[i+1]   += grains[j].getSample(0);
            }
            
        
        }

        for( int i = 0; i < grains.Count; i++ ){

           // print( grains[i].finished );
            if( grains[i].finished== true ){
                print("fin");
                grains.RemoveAt(i--);
            }
        }

      

    }


    }


    public void FixedUpdate(){
        if( timeAudio - lastGrainTime > grainLength ){
            lastGrainTime = timeAudio;
            grainLength= 1000;//Random.Range( 0,2);
           grains.Add( new Grain(this) );
        }
    }

    public struct Grain{
        public float startLocation;
        public float playbackSpeed;
        public float length;
        public int   indexInClip;
        public float timeInClip;
        public float normalizedTime;

        public int startTimeReference;
        public bool finished;

        public int startSample;
        public int baseTime;

        GranularSynthesis synth;




        public Grain(GranularSynthesis s){
            synth = s;
            
            startTimeReference = synth.timeIndex;
            length = 1;
            finished = false;
            startLocation = ((Mathf.Sin( (float)synth.timeIndex ) +1)/2) * ( synth.clipLength - length*2 );
            startSample = (int)Mathf.Floor( startLocation * synth.sampleRate );
            playbackSpeed = Random.Range(1f,2f);
            indexInClip = (synth.timeIndex - startTimeReference)+ startSample;
            baseTime = (synth.timeIndex - startTimeReference);
            timeInClip = (float)baseTime/(float)synth.sampleRate; 
            normalizedTime = timeInClip / length;


        }



        public float getSample(int channel){


            baseTime = (synth.timeIndex - startTimeReference);

            int baseIndex = (int)Mathf.Floor(baseTime * playbackSpeed) + startSample;
            int ceilIndex = (int)Mathf.Ceil(baseTime * playbackSpeed) + startSample;

            float lerpVal = baseTime * playbackSpeed - Mathf.Floor(baseTime * playbackSpeed);

            float d1 = synth.clipData[baseIndex];
            float d2 = synth.clipData[ceilIndex];


            float fVal = d1 + (d2-d1)*lerpVal;

            indexInClip = (int)Mathf.Round(baseTime * playbackSpeed) + startSample;


            // gives us our time in our clip so we can fade in and out correctly
            timeInClip = (float)baseTime/(float)synth.sampleRate;
            normalizedTime = timeInClip / length;


            print( finished );
            if( normalizedTime >= 1 ){
                finished = true;
                return 0;
            }
            

            float vol = Mathf.Min( normalizedTime * 10, (1-normalizedTime) );

            return fVal  * vol;




        }


    }

    public float CreateSine(){
      return Mathf.Sin(2 * Mathf.PI * timeIndex * frequency / (float)sampleRate);
    }


}


