using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneEnter : MonoBehaviour
{
    
        public GameObject thePlayer;
        public GameObject cutsceneCam;
        //public GameObject cutsceneCam2;

        void OnTriggerEnter(Collider other)
        {
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
            cutsceneCam.SetActive(true);
            //cutsceneCam2.SetActive(false);
            thePlayer.SetActive(false);
            //StartCoroutine(KeepGoing());
            StartCoroutine(FinishCut());
        }
        /*
        IEnumerator KeepGoing()
        {
            yield return new WaitForSeconds(2);
            thePlayer.SetActive(false);
            cutsceneCam.SetActive(false);
            cutsceneCam2.SetActive(true);
            StartCoroutine(FinishCut());
        }
        */
        IEnumerator FinishCut()
        {

            
            yield return new WaitForSeconds(33);

            thePlayer.SetActive(true);
            cutsceneCam.SetActive(false);
            //cutsceneCam2.SetActive(false);

        }
 }
