using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraEffect : MonoBehaviour
{
    public static CameraEffect cameraInstance { get; private set; }
    CinemachineVirtualCamera vCam;
    CinemachineBasicMultiChannelPerlin vCamMultiChanelPerlin;
    float shakeTimer;
    float shakeTimerTotal;
    float startingIntensity;

    private void Awake()
    {
        cameraInstance = this;
        vCam = GetComponent<CinemachineVirtualCamera>();
        vCamMultiChanelPerlin = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void CameraShakeEffect(float intensity, float timer)
    {
        startingIntensity = intensity;
        vCamMultiChanelPerlin.m_AmplitudeGain = intensity;
        shakeTimer = timer;
        shakeTimerTotal = timer;
    }

    void Update()
    {
        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if(shakeTimer <= 0)
            {
                //vCamMultiChanelPerlin.m_AmplitudeGain = 0f;
                Mathf.Lerp(startingIntensity, 0f, (1 - (shakeTimer / shakeTimerTotal)));
            }
        }
    }
}
