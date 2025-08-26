using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public enum AudioChannel { Master, Sfx, Music };

    public float masterVolumePercent { get; private set; } = 1f;//다른 클래스가 값을 가져올 수는 있지만 수정은 불가
    public float sfxVolumePercent { get; private set; } = 1f;
    public float musicVolumePercent { get; private set; } = 1f;

    [SerializeField] AudioSource sfx2DSource;
    [SerializeField] AudioSource[] musicSources;
    int activeMusicSourceIndex;

    public static AudioManager instance;

    Transform audioListener;
    Transform playerT;

    SoundLibrary library;
    private void Awake()
    {
        //PlayerPrefs.DeleteAll(); // 모든 키 삭제
        //PlayerPrefs.Save();
        //PlayerPrefs.SetFloat("master vol", 1f);
        //PlayerPrefs.SetFloat("music vol", 1f);
        //PlayerPrefs.SetFloat("sfx vol", 1f);
        //PlayerPrefs.Save();

        //if (!PlayerPrefs.HasKey("master vol"))
        //{
        //    PlayerPrefs.SetFloat("master vol", 1f);
        //    PlayerPrefs.SetFloat("music vol", 1f);
        //    PlayerPrefs.SetFloat("sfx vol", 1f);
        //    PlayerPrefs.Save();
        //}

        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {

            instance = this;
            DontDestroyOnLoad(gameObject);
            library = GetComponent<SoundLibrary>();
            //musicSources = new AudioSource[2];
            //for (int i = 0; i < 2; i++)
            //{
            //    GameObject newMusicSource = new GameObject("Music source " + (i + 1));
            //    newMusicSource.transform.parent = transform;
            //    musicSources[i] = newMusicSource.AddComponent<AudioSource>();
            //    musicSources[i].loop = true;

            //}
            //GameObject newSfx2Dsource = new GameObject("2D sfx source");
            //newSfx2Dsource.transform.parent = transform;
            //sfx2DSource = newSfx2Dsource.AddComponent<AudioSource>();

            audioListener = FindObjectOfType<AudioListener>().transform;
            if (FindObjectOfType<Player>() != null)
            {
                playerT = FindObjectOfType<Player>().transform;

            }

            masterVolumePercent = PlayerPrefs.GetFloat("master vol", 1f);//master vol이 null이면 default masterVolumePercent로 실행
            musicVolumePercent = PlayerPrefs.GetFloat("music vol", 1f);
            sfxVolumePercent = PlayerPrefs.GetFloat("sfx vol", 1f);


            foreach (var source in musicSources)
            {
                source.volume = musicVolumePercent * masterVolumePercent;
            }
            sfx2DSource.volume = sfxVolumePercent * masterVolumePercent;

            ApplyVolumes();
        }
    }
    private void Update()
    {
        if (playerT != null)
        {
            audioListener.position = playerT.position;
        }
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyVolumes();
    }

    private void ApplyVolumes()
    {
        //Debug.Log($"[AudioManager] ApplyVolumes master={masterVolumePercent}, music={musicVolumePercent}, sfx={sfxVolumePercent}");

        foreach (var source in musicSources)
        {
            if (source != null)
            {

                source.volume = musicVolumePercent * masterVolumePercent;
                //Debug.Log($"Set musicSource {source.name} volume={source.volume}");
            }

        }
        if (sfx2DSource != null)
        {
            sfx2DSource.volume = sfxVolumePercent * masterVolumePercent;
            //Debug.Log($"Set sfx2DSource volume={sfx2DSource.volume}");
        }
    }

    public void SetVolume(float volumePercent, AudioChannel channel)
    {
        volumePercent = Mathf.Max(0.001f, volumePercent);

        switch (channel)
        {
            case AudioChannel.Master:
                masterVolumePercent = volumePercent;
                break;
            case AudioChannel.Sfx:
                sfxVolumePercent = volumePercent;
                break;
            case AudioChannel.Music:
                musicVolumePercent = volumePercent;
                break;

        }

        if (musicSources != null && musicSources.Length >= 2)
        {
            musicSources[0].volume = musicVolumePercent * masterVolumePercent;
            musicSources[1].volume = musicVolumePercent * masterVolumePercent;
            //Debug.Log($"[AudioManager] Music slider={volumePercent}, Applied volume={musicSources[0].volume}");

        }
        if (sfx2DSource != null)
        {

            sfx2DSource.volume = sfxVolumePercent * masterVolumePercent;
            //Debug.Log($"[AudioManager] SFX slider={volumePercent}, Applied volume={sfx2DSource.volume}");
        }



        PlayerPrefs.SetFloat("master vol", masterVolumePercent);
        PlayerPrefs.SetFloat("sfx vol", sfxVolumePercent);
        PlayerPrefs.SetFloat("music vol", musicVolumePercent);
        PlayerPrefs.Save();

    }
    public void PlayMusic(AudioClip clip, float fadeDuration = 1)
    {
        activeMusicSourceIndex = 1 - activeMusicSourceIndex;//0 아니면 1 만 반환

        musicSources[activeMusicSourceIndex].clip = clip;
        musicSources[activeMusicSourceIndex].volume = musicVolumePercent * masterVolumePercent; // 🔥 시작값을 바로 맞춤

        musicSources[activeMusicSourceIndex].Play();

        StartCoroutine(AnimateMusicCrossfade(fadeDuration));
    }
    public void PlaySound(AudioClip clip, Vector3 pos)
    {
        if (clip != null)
        {

            AudioSource.PlayClipAtPoint(clip, pos, sfxVolumePercent * masterVolumePercent);
        }
    }


    public void PlaySound(string soundName, Vector3 pos)
    {
        PlaySound(library.GetClipFromName(soundName), pos);
    }

    public void PlaySound2D(string soundName)
    {
        sfx2DSource.PlayOneShot(library.GetClipFromName(soundName), sfxVolumePercent * masterVolumePercent);
    }
    IEnumerator AnimateMusicCrossfade(float duration)
    {
        float percent = 0;
        //float targetVolume = Mathf.Max(0.001f, musicVolumePercent * masterVolumePercent);

        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(0, musicVolumePercent * masterVolumePercent, percent);
            musicSources[1 - activeMusicSourceIndex].volume = Mathf.Lerp(musicVolumePercent * masterVolumePercent, 0, percent);
            yield return null;
        }

        //while (percent < 1)
        //{
        //    percent += Time.deltaTime * 1 / duration;

        //    musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(0, targetVolume, percent);
        //    musicSources[1 - activeMusicSourceIndex].volume = Mathf.Lerp(targetVolume, 0, percent);

        //    yield return null;
        //}
    }


}
