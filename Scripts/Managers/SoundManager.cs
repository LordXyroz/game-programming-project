using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    // Enum for fading bgm
    private enum FadeState
    {
        None,
        FadingOut,
        FadingIn
    }

    [Header("Audio")]
    public AudioSource bgmSource;
    public AudioSource efxSource;
    public AudioSource loopingEfxSource;

    public AudioClip defaultClip;

    [Header("Controls")]
    public bool bgmIsPlaying = true;
    public bool efxIsPlaying = true;
    public bool loopingEfxIsPlaying = true;

    private float bgmVolume = 1.0f;
    private float efxVolume = 1.0f;

    public float bgmMaxVolume = 1.0f;
    public float efxMaxVolume = 1.0f;

    private AudioClip nextClip;
    [SerializeField]
    private FadeState state;
    [SerializeField]
    private float fadeThreshold = 0.01f;
    [SerializeField]
    private float fadeInSpeed = 0.1f;
    [SerializeField]
    private float fadeOutSpeed = 0.5f;

	void Awake () {
        // Sets default values
        bgmSource.loop = true;
        loopingEfxSource.loop = true;

        bgmMaxVolume = bgmVolume;
        efxMaxVolume = efxVolume;
        bgmVolume = bgmSource.volume;
        efxVolume = efxSource.volume;
        PlayBGM(defaultClip);
    }
	
	public void PlayBGM(AudioClip clip)
    {
        // Starts playing background music
        bgmSource.clip = clip;
        bgmSource.Play();
        bgmIsPlaying = true;
    }

    public void PlayEfx(AudioClip clip)
    {
        // Starts playing a sound effect
        efxSource.clip = clip;
        efxSource.Play();
        efxIsPlaying = true;
    }

    public void PlayEfx(AudioClip clip, bool loop)
    {
        // Starts playing a looping sound effect (e.g. burning campfire)
        loopingEfxSource.clip = clip;
        loopingEfxSource.loop = loop;
        loopingEfxSource.Play();
        loopingEfxIsPlaying = true;
    }

    // Function for changing music volume ingame
    // TODO: implement sliders in options menu for this
    public void SetBGMVolume(float vol)
    {
        bgmMaxVolume = vol;
        bgmSource.volume = vol;
        bgmVolume = bgmSource.volume;
    }

    // Function for changing sound effect volume ingame
    // TODO: implement sliders in options menu for this
    public void SetEfxVolume(float vol)
    {
        efxMaxVolume = vol;
        efxSource.volume = vol;
        loopingEfxSource.volume = vol;
        efxVolume = efxSource.volume;
    }

    public void PauseBGM()
    {
        // Toggles between pause and play for background music
        if (bgmIsPlaying)
        {
            bgmSource.Pause();
            bgmIsPlaying = false;
        }
        else
        {
            bgmSource.UnPause();
            bgmIsPlaying = true;
        }
    }

    public void PauseEfx()
    {
        // Toggles between pause and play for sound effects
        if (efxIsPlaying)
        {
            efxSource.Pause();
            efxIsPlaying = false;
        }
        else
        {
            efxSource.UnPause();
            efxIsPlaying = true;
        }

        if (loopingEfxIsPlaying)
        {
            loopingEfxSource.Pause();
            loopingEfxIsPlaying = false;
        }
        else
        {
            loopingEfxSource.UnPause();
            loopingEfxIsPlaying = true;
        }
    }

    public void StopBGM()
    {
        if (bgmSource.isPlaying)
            bgmSource.Stop();
    }

    public void StopEfx()
    {
        if (efxSource.isPlaying)
            efxSource.Stop();
        if (loopingEfxSource.isPlaying)
            loopingEfxSource.Stop();
    }

    // Function for fading between current playing music and next clip
    // Has default fade speed set if not given through function parameters
    public void Fade(AudioClip clip, float inSpeed = 0.05f, float outSpeed = 0.2f)
    {
        // Checks if there is a clip, and it's not currently playing
        if (clip == null || clip == bgmSource.clip)
            return;

        nextClip = clip;
        fadeInSpeed = inSpeed;
        fadeOutSpeed = outSpeed;

        // Checks if the audiosource is enabled
        // Used while developing cause hearing the same song 1000s of times can be annoying
        if (bgmSource.enabled)
        {
            // if anything is playing, start to fade out,
            // else start to fade in
            if (bgmSource.isPlaying)
                state = FadeState.FadingOut;
            else
                FadeToNextClip();
        }
        else
            FadeToNextClip();
    }

    private void FadeToNextClip()
    {
        // Starts to fade in the next clip
        bgmSource.clip = nextClip;
        state = FadeState.FadingIn;

        // Plays next clip if audiosource is enabled
        // Used while developing cause hearing the same song 1000s of times can be annoying
        if (bgmSource.enabled)
            bgmSource.Play();
    }

    void Update()
    {
        // Used while developing cause hearing the same song 1000s of times can be annoying
        if (!bgmSource.enabled)
            return;

        // If we are fading out, slowly decrease volume over time
        if (state == FadeState.FadingOut)
        {
            // If above a threshold, continue fading out, else start to fade in
            if (bgmVolume > fadeThreshold)
            {
                bgmVolume -= fadeOutSpeed * Time.deltaTime;
                bgmSource.volume = bgmVolume;
            }
            else
            {
                FadeToNextClip();
            }
        }
        // if we are fading in, slowly increase volume over time
        else if (state == FadeState.FadingIn)
        {
            // if we are below max volume, continue to fade it, else stop fading
            if (bgmVolume <= bgmMaxVolume)
            {
                bgmVolume += fadeInSpeed * Time.deltaTime;
                bgmSource.volume = bgmVolume;
            }
            else
            {
                state = FadeState.None;
            }
        }
    }
}
