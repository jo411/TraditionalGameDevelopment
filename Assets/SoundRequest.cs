using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRequest : MonoBehaviour {
    [System.Serializable]
    public struct NamedSound
    {
        public string name;
        public AudioClip sound;
    };
    public NamedSound[] sfx;
    Dictionary<string,AudioClip> sounds;

    public NamedSound[] music;
    Dictionary<string, AudioClip> songs;

    AudioSource player;

    public float volume = .5f;

    public AudioClip defaultSound;
    public string initialSong;
    // Use this for initialization
    void Start () {
        sounds = new Dictionary<string, AudioClip>();
        songs = new Dictionary<string, AudioClip>();


        player = GetComponent<AudioSource>();
        player.loop = true;
		foreach(NamedSound current in sfx)
        {
            sounds.Add(current.name, current.sound);
        }

        foreach (NamedSound current in music)
        {
            songs.Add(current.name, current.sound);
        }

        DontDestroyOnLoad(transform.gameObject);
        requestSong(initialSong);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private AudioClip getSound(string name)
    {
        AudioClip request;
        if (sounds.TryGetValue(name, out request))
            return request;
        return defaultSound;

    }
    private AudioClip getSong(string name)
    {
        AudioClip request;
        if (songs.TryGetValue(name, out request))
            return request;
        return defaultSound;
    }


    public void requestSound(string name)
    {        
        player.PlayOneShot(getSound(name));
    }
    public void requestSong(string name)
    {
        player.Stop();
        player.clip = getSong(name);       
        player.Play();
    }

    public void stopAll()
    {
        player.Stop();
    }

}
