using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Input;
using UnityEngine;

public class MicrophoneInput : PlayerMicInput
{
        public static float MicLoudness;

        [SerializeField] private SettingWindow SettingWindow;
 
        private string _device;
        
        private bool _isInitialized;

        private AudioClip _clipRecord;
        private int _sampleWindow = 128;

        [SerializeField] private float maxVolume = 1;

        private bool micEnabled;


        private void Start()
        {
            SettingWindow.ReInitMic += () =>
            {
                ReInit();
            };
        }

        private void OnEnable()
        {
            micEnabled = PlayerPrefs.HasKey("useMic") ? (PlayerPrefs.GetInt("useMic") == 1 ? true : false) : true;
            if (micEnabled)
            {
                InitMic();
                _isInitialized = true;
            }else if (!micEnabled)
            {
                _isInitialized = false;
            }
        }

        public override (float micVolume, bool isTalking, bool isEnabled) MicInput()
        {
            if (micEnabled)
            {
                MicLoudness = LevelMax();
                
                return (MicLoudness / maxVolume,
                    _isInitialized,
                    micEnabled);
            }

            return (0, _isInitialized, micEnabled);
        }

        private void ReInit()
        {
            if(micEnabled)
                Microphone.End(_device);
            _isInitialized = false;
            _device = null;
            micEnabled = PlayerPrefs.HasKey("useMic") ? (PlayerPrefs.GetInt("useMic") == 1 ? true : false) : true;
            if (micEnabled)
            {
                InitMic();
                _isInitialized = true;
            }else if (!micEnabled)
            {
                _isInitialized = false;
            }
        }

        private void InitMic(){
            if(_device == null)
                if (PlayerPrefs.HasKey("microphone"))
                {
                    var mic = PlayerPrefs.GetString("microphone");
                    if (Microphone.devices.Contains(mic))
                    {
                        _device = mic;
                    }
                }
                else
                {
                    _device = Microphone.devices[0];
                }

            Debug.Log(_device);
            _clipRecord = Microphone.Start(_device, true, 999, 44100);
        }

        private float  LevelMax()
        {
            float levelMax = 0;
            float[] waveData = new float[_sampleWindow];
            int micPosition = Microphone.GetPosition(null)-(_sampleWindow+1);
            if (micPosition < 0) return 0;
            _clipRecord.GetData(waveData, micPosition);
            
            for (int i = 0; i < _sampleWindow; i++) {
                float wavePeak = waveData[i] * waveData[i];
                if (levelMax < wavePeak) {
                    levelMax = wavePeak;
                }
            }
            return Mathf.Sqrt(Mathf.Sqrt(levelMax));
        }

        private void OnDisable()
        {
            if(micEnabled)
                Microphone.End(_device);
        }
 
        private void OnDestroy()
        {
            if(micEnabled)
                Microphone.End(_device);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (micEnabled)
            {
                if (hasFocus)
                {
                    if (!_isInitialized)
                    {
                        Debug.Log("Init Mic");
                        InitMic();
                        _isInitialized = true;
                    }
                }

                if (!hasFocus)
                {
                    Microphone.End(_device);
                    _isInitialized = false;
                }
            }else if (!micEnabled)
            {
                //
            }
        }
}
