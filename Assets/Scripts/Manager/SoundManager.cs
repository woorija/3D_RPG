using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : SingletonBehaviour<SoundManager>
{
    [SerializeField]
    GameObject SettingUI;

    [SerializeField] Sound[] bgm;

    [SerializeField] AudioClip[] monster_die_sfx;
    [SerializeField] AudioClip[] unit_sfx;
    [SerializeField] AudioClip[] normal_sfx;

    AudioClip temp_sfx;

    [SerializeField] AudioSource bgmPlayer;
    [SerializeField] AudioSource sfxPlayer;


    public float bgmVolume { get; private set; }
    public float sfxVolume { get; private set; }
    public string cur_BGMName { get; private set; } = "";

    private void Start()
    {
        LoadVolume();
    }

    public void Popup_settingUI()
    {
        SettingUI.SetActive(true);
    }
    /*
    public void EXIT_settingUI()
    {
        PlayNormalSfx(3);
        switch (CustomSceneManager.Instance.GetSceneName())
        {
            case "TitleScene":
                Time.timeScale = 1;
                break;
            case "PlayScene":
            case "InfiScene":
                UIManager.Instance.EXIT_Option();
                break;
        }
        SettingUI.SetActive(false);
    }
    */
    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    void LoadVolume()
    {
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 0.3f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.1f);

        bgmPlayer.volume = bgmVolume;
        sfxPlayer.volume = sfxVolume;
    }

    public void SetBGMvolume(float _volume)
    {
        bgmVolume = _volume;
        bgmPlayer.volume = bgmVolume;
    }

    public void SetSFXvolume(float _volume)
    {
        sfxVolume = _volume;
        sfxPlayer.volume = sfxVolume;
    }


    public void StopMainBGM()
    {
        StopBgm();
    }

    public void PlayBgm(string _name)
    {
        if (cur_BGMName != _name) // 현재 플레이곡하고 다를경우에만 체크
        {
            for (int i = 0; i < bgm.Length; i++)
            {
                if (_name == bgm[i].name)
                {
                    bgmPlayer.clip = bgm[i].clip;
                    cur_BGMName = bgm[i].name;
                    bgmPlayer.Play();
                    return;
                }
            }
        }
    }

    public void StopBgm()
    {
        if (bgmPlayer.clip != null)
        {
            bgmPlayer.Stop();
            cur_BGMName = "";
        }
    }

    public void PauseBgm()
    {
        if (bgmPlayer.clip != null)
        {
            bgmPlayer.Pause();
        }
    }

    public void UnpauseBgm()
    {
        if (bgmPlayer.clip != null)
        {
            bgmPlayer.UnPause();
        }
    }


    public void PlayNormalSfx(int _index) // 0스테이지 시작    1스테이지클리어    2스테이지실패    3UI클릭    4아티팩트획득
    {
        temp_sfx = normal_sfx[_index];
        PlaySfx();
    }

    public void PlayDieSfx(int _index)
    {
        temp_sfx = monster_die_sfx[_index];
        PlaySfx();
    }

    public void PlayAttackSfx(int _index)
    {
        temp_sfx = unit_sfx[_index];
        PlaySfx();
    }

    void PlaySfx()
    {
        sfxPlayer.PlayOneShot(temp_sfx);
    }
}
