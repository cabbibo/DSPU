using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioForm : MonoBehaviour {

  public int count;
  public int structSize;
  public bool bufferMade;

  public ComputeBuffer _buffer;

  void Awake(){ 
    _buffer = MakeBuffer();
    bufferMade = true;
  }

  void OnDestroy(){
    ReleaseBuffer();
  }

  public float[] GetFloatData(){
    float[] val = new float[count*structSize];
    GetData(val);
    return val;
  }

  public float[] GetData(){
    return GetFloatData();
  }

  public void GetData( float[] values ){ _buffer.GetData(values); }

  public void SetData( float[] values ){ _buffer.SetData( values );}
  public void SetData( int[] values ){ _buffer.SetData( values ); }

  public ComputeBuffer MakeBuffer(){
    return new ComputeBuffer( count, sizeof(float) * structSize );
  }

  public void ReleaseBuffer(){
   if(_buffer != null){ _buffer.Release(); }
  }


 
}
