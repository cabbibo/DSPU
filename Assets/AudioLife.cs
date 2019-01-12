using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLife : MonoBehaviour {

  public AudioForm form;
  public ComputeShader shader;
  public string kernelName;
  public int kernel;
  protected int numGroups;
  protected uint numThreads;

  public void FindKernel(){
    kernel = shader.FindKernel( kernelName );
  }

  public void GetNumThreads(){
    uint y; uint z;
    shader.GetKernelThreadGroupSizes(kernel, out numThreads , out y, out z);
  }


  public void GetNumGroups(){
    numGroups = (form.count+((int)numThreads-1))/(int)numThreads;
  }

  public void Start(){
    FindKernel();
  }


  public void Live(){

    GetNumThreads();
    GetNumGroups();

    SetBuffer( "_AudioBuffer", form);
    shader.Dispatch( kernel,numGroups ,1,1);

  }

  private void SetBuffer(string name , AudioForm form){
    if( form._buffer != null ){
      shader.SetBuffer( kernel , name , form._buffer);
      shader.SetInt(name+"_COUNT" , form.count );
    }else{
      print("no buffer");
    }
  }



}
