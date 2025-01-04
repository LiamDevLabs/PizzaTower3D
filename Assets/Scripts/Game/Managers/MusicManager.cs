using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    bool paused = false;
    [SerializeField] AudioSource[] musics;

    [SerializeField] private AudioSource pauseMusic;

    PeppinoController peppinoController;

    int countUnpaused = 0;
    int unpausedMusicIndex=0;

    private void Awake()
    {
        paused = false;
        pauseMusic.enabled = false;
    }

    private IEnumerator Start()
    {
        yield return null;
        yield return null;
        peppinoController = FindObjectOfType<PeppinoController>();
    }

    void Update()
    {
        if (Time.timeScale == 0)
        {
            //Poniendo la pausa
            if (!paused)
            {
                if (musics.Length > 0) PauseMusic(true);
                if(pauseMusic) pauseMusic.enabled = true;
                if(peppinoController) peppinoController.AudioSource.enabled = false;
                paused = true;
            }
        }
        else
        {
            //Quitando la pausa
            if (paused)
            {
                if (musics.Length > 0) PauseMusic(false);
                if (pauseMusic) pauseMusic.enabled = false;
                if (peppinoController) peppinoController.AudioSource.enabled = true;
                paused = false;
            }
        }
    }

    void PauseMusic(bool pause)
    {
        int i = 0;
        if (pause)
        {
            countUnpaused = 0;
            foreach (AudioSource music in musics)
            {
                countUnpaused = music.isPlaying ? countUnpaused++ : countUnpaused;
                unpausedMusicIndex = music.isPlaying ? i : unpausedMusicIndex;
                if (music.isPlaying) Debug.Log(music.clip.name);
                music.Pause();
                i++;
            }

        }
        else
        {
            foreach (AudioSource music in musics)
            {
                if(countUnpaused > 1)
                {
                    music.UnPause();
                    Debug.Log("unpaused all" +music.clip.name);
                }
                else
                {
                    
                    if (unpausedMusicIndex == i)
                    {
                        music.UnPause();
                        Debug.Log("unpaused 1" +music.clip.name);
                    }
                }
                i++;
            }
        }
    }
}
