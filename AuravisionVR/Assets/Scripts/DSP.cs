using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class DSP : MonoBehaviour
{
    private string[] keyWords;
    private KeywordRecognizer recog;
    public float sensitivity = 100.0f;
    public float frequency = 0.0f;
    public int samplerate = 44100;
    public float[] data = new float[256];
    public int fireCount = 0;
    public int catCount = 0;
    public int dogCount = 0;
    public float loudness;
    public float fundaFreq = 0;
    public float maxVal = 0;
    public float enemySize = 0;

    public GameObject target;

    public List<float> testArr = new List<float>();

    GameObject mic;
    public AudioSource aud1;
    void Start()
    {
        
        keyWords = new string[3];
        keyWords[0] = "Strike";
        keyWords[1] = "Water";
        keyWords[2] = "Dog";
        recog = new KeywordRecognizer(keyWords);
        recog.OnPhraseRecognized += OnPhraseRecognized;
        recog.Start();
        foreach(string device in Microphone.devices)
        {
            Debug.Log(Microphone.devices[0]);
        }
        aud1 = GetComponent<AudioSource>();
        aud1.clip = Microphone.Start(Microphone.devices[0], true, 1, 44100);
        aud1.loop = true; // Set the AudioClip to loop
        aud1.mute = true; // Mute the sound, we don't want the player to hear it

        mic = GameObject.Find("MicControllerC");
        //aud1.Play(); // Play the audio source!
    }

    void Update()
    {
        loudness = mic.GetComponent<MicControlC>().loudness;
        fundaFreq = mic.GetComponent<MicControlC>().fundaFreq;
        testArr.Add(loudness);
        maxVal = testArr.Max();
        if(testArr.Count > 200)
        {
            testArr.Clear();
        }
       //frequency = GetFundamentalFrequency();
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args )
    {
        enemySize = maxVal;
        if(args.text == keyWords[0] /*&& args.confidence > ConfidenceLevel.Low*/)
        {
            Debug.Log("Strike");
            fireCount = fireCount + 1;
            Instantiate(target, new Vector3(21, 1, 30), Quaternion.identity);
        }

        if (args.text == keyWords[1] /*&& args.confidence > ConfidenceLevel.Low*/)
        {
            Debug.Log("Water");
            catCount = catCount + 1;
        }

        if (args.text == keyWords[2] /*&& args.confidence > ConfidenceLevel.Low*/)
        {
            Debug.Log("Dog");
            dogCount = dogCount + 1;
        }
    }
}