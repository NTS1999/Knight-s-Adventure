using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Section4
{
    public class CameraShake : MonoBehaviour
    {
        public static CameraShake m_Instance;
        public static CameraShake Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = FindObjectOfType<CameraShake>();
                return m_Instance;
            }
        }

        [SerializeField] private CinemachineVirtualCamera[] virtualCameras;
        [SerializeField] private float shakeAmplitude = 1.2f;
        [SerializeField] private float shakeFrequency = 2.0f;

        private float shakeDuration;
        private CinemachineBasicMultiChannelPerlin[] virtualCameraNoise;

        private void Awake()
        {
            if (m_Instance == null)
                m_Instance = this;
            else if (m_Instance != this)
                Destroy(gameObject);
        }

        private void Start()
        {
            virtualCameraNoise = new CinemachineBasicMultiChannelPerlin[virtualCameras.Length];
            for (int i = 0; i < virtualCameras.Length; i++)
            {
                virtualCameraNoise[i] = virtualCameras[i].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            }
        }

        public void Shake(float duration)
        {
            shakeDuration = duration;
        }

        private void Update()
        {
            if (shakeDuration > 0)
            {
                shakeDuration -= Time.deltaTime;

                for (int i = 0; i < virtualCameraNoise.Length; i++)
                {
                    virtualCameraNoise[i].m_AmplitudeGain = shakeAmplitude;
                    virtualCameraNoise[i].m_FrequencyGain = shakeFrequency;
                }

                if (shakeDuration <= 0)
                {
                    for (int i = 0; i < virtualCameraNoise.Length; i++)
                    {
                        virtualCameraNoise[i].m_AmplitudeGain = 0;
                        virtualCameraNoise[i].m_FrequencyGain = 0;
                    }
                }
            }
        }
    }
}

