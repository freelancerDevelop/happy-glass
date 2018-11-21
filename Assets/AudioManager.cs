using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip audioClip;
    [HideInInspector]
    public AudioSource source;
    public bool loop;
}
public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;
    public List<Sound> ListAudio;
	// Use this for initialization
	void Start () {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(this);
        }
        foreach (var s in ListAudio)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip;
            s.source.playOnAwake = false;
            s.source.loop = s.loop;
        }
	}
	public void Play(string name)
    {
        Sound s = ListAudio.Find(x => x.name == name);
        s.source.volume = 1;
        s.source.Play();
    }
    public void Stop(string name)
    {
        ListAudio.Find(s => s.name == name).source.DOFade(0, 1f);
    }
}
