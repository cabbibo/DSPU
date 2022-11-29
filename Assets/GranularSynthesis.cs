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


    public float grainSize;
    public float grainSizeRandomness;
    public float grainPlaybackRate;
    public float grainPlaybackRateRandomness;
    public float playbackLocation;
    public float playbackLocationRandomness;
    public float fadeInSize;
    public float fadeOutSize;

    public float timeBetweenGrains;
    public float timeBetweenGrainsRandomness;
    

    int ti2;

    float rand(int v){
        return (Mathf.Sin((float)v * 14.1455f + 10 * Mathf.Sin((float)v*4141.44f)) + 1)/2;

    }


    // where we start playing our sample from ( in sample space)
    public int[] startTimeInClip;
 
    // how many samples long our clip is
    public int[] clipLengths;

    // what time our 'sample clock' started playing at.
    public int[] clipStartPlayTime;

    public float[] playbackSpeed;

    public bool[] toDestroy;

    public int maxGrains = 100;
    public int currentNumberGrains = 0;

    void OnEnable()
    {

        startTimeInClip = new int[maxGrains];
        clipLengths = new int[maxGrains];
        clipStartPlayTime = new int[maxGrains];
        toDestroy = new bool[maxGrains];
        playbackSpeed = new float[maxGrains];

        for( int i =0 ; i < maxGrains; i++ ){
            startTimeInClip[i] = 0;
            clipLengths[i] = 0;
            clipStartPlayTime[i] = 00;
            toDestroy[i] = false;
            playbackSpeed[i] = 1110;
        }



        //AudioSettings.outputSampleRate = clip.frequency;
        // setting our sample rate
        sampleRate = AudioSettings.outputSampleRate;


        // getting our full clip data into a data buffer
        totalClipSamples = clip.samples * clip.channels;
        clipData = new float[totalClipSamples];

        clip.GetData(clipData,0);

        started = true;

        clipLength = clip.length;
        timeIndex = 0;



    }



    void OnAudioFilterRead(float[] data , int channels ){


        if( started ){  

            for(int i = 0; i < data.Length; i+=channels){
                
                timeIndex ++;
                timeAudio = (float)timeIndex / (float)sampleRate;

                for( int j = 0; j< currentNumberGrains; j++ ){
            
                    data[i]   += sampleGrain(j,0);
                    data[i+1]   += sampleGrain(j,1);

                }
                
            }

            
            FlattenGrains();

    

        }


     
    }



float fGrainLength = 0;
void FixedUpdate(){



            if( timeAudio - lastGrainTime > fGrainLength ){
                fGrainLength = timeBetweenGrains + Random.Range(-timeBetweenGrainsRandomness,timeBetweenGrainsRandomness);
                lastGrainTime = timeAudio;
                MakeNewGrain();
            }

}


float  sampleGrain(int id , int channel){




    int sampleTimePlayed = timeIndex - clipStartPlayTime[id];

    int speedCorrected = (int)((float)sampleTimePlayed * playbackSpeed[id]);

    float nTime = (float)speedCorrected / (float)clipLengths[id];
    
    int finalSample = startTimeInClip[id] + (int)((float)sampleTimePlayed * playbackSpeed[id]);

    float v = clipData[finalSample*2+ channel];

    if( speedCorrected > clipLengths[id]){
        v = 0;
        toDestroy[id] = true;
    }

    v *= Mathf.Min(nTime * 3 , 1-nTime);

    return v;

}


void FlattenGrains(){



bool completed = false;
int i = 0;
while (completed == false) 
{

    if( toDestroy[i] == true ){

    }
    i++;

    if( i == maxGrains ){
        completed = true;
    }



}




for( int j = 0; j< currentNumberGrains; j++ ){
          
    int id = j;

    if( toDestroy[id] == true ){

            if( id == currentNumberGrains-1){
                // just destroy
                toDestroy[id] = false;
                //clipLengths[id] = 0; 
                //clipStartPlayTime[id] = 0;
                //startTimeInClip[id] = 0;
                //playbackSpeed[id] = 1110;
            }else{

                if( toDestroy[id] == toDestroy[id+1]){
                    print("SAME");
                }
                toDestroy[id] = toDestroy[id+1];
                //clipLengths[id] = clipLengths[id+1];
                //clipStartPlayTime[id] = clipStartPlayTime[id+1];
                //playbackSpeed[id] = playbackSpeed[id+1];
                //startTimeInClip[id] = startTimeInClip[id+1];
            }

            currentNumberGrains --;
        }

    }
}

    void MakeNewGrain(){

        if( currentNumberGrains >= maxGrains ){
                 
                    return;
                }


                clipLengths[currentNumberGrains] = (int)((grainSize + Random.Range(-grainSizeRandomness,grainSizeRandomness)) * (float)sampleRate); // just making it 1 second to start!

                playbackSpeed[currentNumberGrains] = grainPlaybackRate + Random.Range(-grainPlaybackRateRandomness,grainPlaybackRateRandomness);
            
                clipStartPlayTime[currentNumberGrains] = timeIndex; // start playing now!
                
                
                startTimeInClip[currentNumberGrains] = (int)( (playbackLocation + Random.Range(-playbackLocationRandomness,playbackLocationRandomness)) * (float)sampleRate);// Random.Range( 0, totalClipSamples/2 - (int)((float)clipLengths[currentNumberGrains]*2 * playbackSpeed[currentNumberGrains])); // starting at 0 to test!

                
                currentNumberGrains ++;


    }






}


