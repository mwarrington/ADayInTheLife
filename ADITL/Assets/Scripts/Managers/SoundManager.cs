using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    static AudioSource aSource;
    public static int sourcesCount = 8;
    static AudioSource[] sources;
    static AudioSource loopSource;
    static float mainVolume;

    static void Init() { }

    public static bool stopPlayingMusic,
    isPlayingSound;

	//change volume. volume range: 0.0 - 1.0
	//out-of-range values are automatically handled. 
	public static void VolumeChange(float change){
		Debug.Log ("VolumeChange: " + change + "\n" + "Current volume: " + loopSource.audio.volume);
		loopSource.audio.volume += change;

		return;
		}

    void Start()
    {
        sources = new AudioSource[sourcesCount];
        if (mainVolume == null) mainVolume = 1.0f;
        for (int i = 0; i < sourcesCount; i++)
        {
            sources[i] = gameObject.AddComponent("AudioSource") as AudioSource;
            sources[i].volume = mainVolume;
        }
		loopSource = gameObject.AddComponent("AudioSource") as AudioSource;
		loopSource.name = "LoopSource";
        loopSource.volume = mainVolume;
		loopSource.loop = true;
    }

    void Update()
    {
        if (stopPlayingMusic)
            audio.enabled = false;

    }

    static void Clear()
    {
        for (int i = 0; i < sourcesCount; i++)
        {
            sources[i].Stop();
        }
		loopSource.Stop();
    }


    static void  Play ( AudioClip clip  ){
        Play(clip, mainVolume);
    }

	


    public static void Play(AudioClip clip, float volume = 1.0f)
    {
        for (int i = 0; i < sourcesCount; i++)
        {
            if (!sources[i].isPlaying)
            {
                sources[i].clip = clip;
                sources[i].volume = volume;
				sources[i].Play();
                return;
            }
        }
    }

    public static void Stop()
    {
        for (int i = 0; i < sourcesCount; i++)
        {
            if (sources[i].isPlaying)
            {
                sources[i].Stop();
            }
        }
    }

    public static void Stop(AudioClip clip)
    {
        for (int i = 0; i < sourcesCount; i++)
        {
            if (sources[i].isPlaying && sources[i].clip == clip)
            {
                sources[i].Stop();
            }
        }
    }

    public static void Pause()
    {
        for (int i = 0; i < sourcesCount; i++)
        {
            if (sources[i].isPlaying)
            {
                sources[i].Pause();
            }
        }
    }

    public static void Pause(AudioClip clip)
    {
        for (int i = 0; i < sourcesCount; i++)
        {
            if (sources[i].isPlaying && sources[i].clip == clip)
            {
                sources[i].Pause();
            }
        }
    }

    public static void Unpause(AudioClip clip)
    {
        for (int i = 0; i < sourcesCount; i++)
        {
            if (sources[i].clip == clip)
            {
                sources[i].Play();
            }
        }
    }


    public static void PlayLoop(AudioClip clip, float volume)
    {
        if (!loopSource.isPlaying)
        {
            loopSource.clip = clip;
            loopSource.volume = volume;
            loopSource.Play();
            return;
        }
    }

    public static void StopLoop(AudioClip clip)
    {
        if (loopSource.isPlaying)
        {
            loopSource.clip = clip;
            loopSource.Stop();
            return;
        }
    }


    static void StopMusic(AudioClip aClip)
    {
        aSource = sources[0];
        if (aSource.isPlaying && aSource.clip == aClip)
        {
            aSource.Stop();
        }
    }

    static void StopSfx(AudioClip aClip)
    {
        aSource = sources[1];
        if (aSource.isPlaying && aSource.clip == aClip)
        {
            aSource.Stop();
        }
    }

    static void StopAll()
    {
		loopSource.Stop();
        for (int i = 0; i < sourcesCount; i++)
        {
            sources[i].Stop();
        }
    }

    static void Mute(AudioClip clip)
    {
		loopSource.mute = true;
        for (int i = 0; i < sourcesCount; i++)
        {
            if (sources[i].isPlaying && sources[i].clip == clip)
            {
                sources[i].mute = true;
                return;
            }
        }
    }

    static void Unmute(AudioClip clip)
    {
		loopSource.mute = false;
        for (int i = 0; i < sourcesCount; i++)
        {
            if (sources[i].isPlaying && sources[i].clip == clip)
            {
                sources[i].mute = false;
                return;
            }
        }
    }

    static void IsMusicPlaying(AudioClip clip)
    {
        if (sources[0].isPlaying && sources[0].clip == clip)
        {
            return;
        }
        else
        {
            return;
        }
    }

    public static bool IsPlaying(AudioClip clip)
    {
        for (int i = 0; i < sourcesCount; i++)
        {
            if (sources[i].isPlaying && sources[i].clip == clip)
            {
                return true;
            }
        }
        return false;
    }



}