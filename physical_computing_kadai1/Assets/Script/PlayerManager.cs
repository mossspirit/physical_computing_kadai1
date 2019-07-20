using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour {
    
    float moveLength = 0f;
    float MAX_MOVE = 200;
    float mySpeed = 0f;
    float atLength = 0f;
    int staycount = 0;
    bool end = false;
    AudioSource audioSource;
    [SerializeField] Text speedUI;
    [SerializeField] GameObject convini;
    [SerializeField] GameObject gameover;
    [SerializeField] GameObject gameclear;
    [SerializeField] DontShowScreenReset[] gameManager = new DontShowScreenReset[2];
    [SerializeField] ParticleSystem particle;
    [SerializeField] AudioClip burn;
    [SerializeField] SerialMain serialMain;
    
    void Start () {
        audioSource = GetComponent<AudioSource>();
    }
	
	
	void Update () {

        if (Input.GetKeyDown(KeyCode.R)) {
            serialMain.Scene();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        int num = (int)mySpeed;
        speedUI.text = num.ToString();

        if(atLength < 0) {
            convini.SetActive(true);
        }
        if (staycount > 100) {
            gameclear.SetActive(true);
            for(int i=0; i<=1; i++)
                Destroy(gameManager[i]);
        }
    }
    
    public void SetSpeed(float speed) {
        if (speed >= 1) {
            mySpeed = Mathf.Abs(Map(speed, 0, 13, 0, 120));
        } else {
            mySpeed = 0f;
        }
    }
    public void scrollCount(float length) {
        moveLength += length;
        atLength = MAX_MOVE - moveLength / 100;
        //Debug.Log("残り" + (atLength) + "メートル");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Finish") {
            Debug.Log("taisei");
            gameover.SetActive(true);
            if (!end) {
                particle.Play();
                audioSource.PlayOneShot(burn);
                end = true;
            }
            Destroy(gameclear);
            for (int i = 0; i <= 1; i++) {
                gameManager[i].game = false;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "Clear") {
            staycount += 1;
        }
    }
    float Map(float value, float start1, float stop1, float start2, float stop2) {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }
}
