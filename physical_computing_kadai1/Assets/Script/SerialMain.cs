using System;
using System.IO.Ports;
using UnityEngine;
using UniRx;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SerialMain : MonoBehaviour {

    public string portName;
    public int baurate;
    SerialPort serial;
    [NonSerialized]
    public bool isLoop = true;
    public float speed = 0;
    [SerializeField] GameObject[] background = new GameObject[2];
    DontShowScreenReset[] controller = new DontShowScreenReset[2];
    [SerializeField] PlayerManager playerManager;

    void Start() {
        this.serial = new SerialPort(portName, baurate, Parity.None, 8, StopBits.One);

        try {
            this.serial.Open();
            Scheduler.ThreadPool.Schedule(() => ReadData()).AddTo(this);
        } catch (Exception e) {
            Debug.Log("can not open serial port");
        }
        for(int i=0;i<=1; i++) {
            controller[i] = background[i].GetComponent<DontShowScreenReset>();
        }
    }
    void Update() {
    }

    private void ReadData() {
        while (this.isLoop && this.serial != null && this.serial.IsOpen) {
            try {
                string str = this.serial.ReadLine();
                
                speed = Convert.ToInt32(str);
                speed = Mathf.Abs(Map(speed, 70, 180, 0, 13));
                playerManager.SetSpeed(speed);
                for (int i=0; i<=1; i++) {
                    if (speed >= 1)
                        controller[i].speed = speed;
                    else
                        controller[i].speed = 0f;
                }
            } catch (System.Exception e) {
            }
        }
    }
    public void Write(byte[] buffer) {
        try {
            this.serial.Write(buffer, 0, 1);
        } catch (System.Exception e) {
            Debug.LogWarning(e.Message);
        }
    }

    public void Scene() {
        this.isLoop = false;
        try {
            this.serial.Close();
        } catch (System.Exception e) {
            Debug.Log(e.Message);
        }

        Debug.Log("SerialClosed");
    }

    float Map(float value, float start1, float stop1, float start2, float stop2) {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }
}