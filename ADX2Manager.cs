using CriWare;
using System.Collections;
using UnityEngine;

public class ADX2Manager : MonoBehaviour
{
    public static ADX2Manager Instance { get; private set; }
    private CriAtomExPlayer exPlayer, exPlayer2, exSePlayer;
    private CriAtomExAcb bgmAcb, seAcb;
    public CriAtomExPlayback playBack { get; private set; }
    private int fadeInTime = 1000;
    private int fadeOutTime = 1000;
    public static bool CueSheetLoaded { get; private set; } = false;

    private void Awake() {
        if (ADX2LEManager.Instance == null) {
            ADX2LEManager.Instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(this);
            return;
        }

        exPlayer = new CriAtomExPlayer();
        exPlayer.Loop(true);
        exPlayer.AttachFader();
        exPlayer.SetFadeInTime(fadeInTime);
        exPlayer.SetFadeOutTime(fadeOutTime);

        exPlayer2 = new CriAtomExPlayer();
        exPlayer2.Loop(true);
        exPlayer2.AttachFader();
        exPlayer2.SetFadeInTime(fadeInTime);
        exPlayer2.SetFadeOutTime(fadeOutTime);

        exSePlayer = new CriAtomExPlayer();
    }

    private IEnumerator Start() {
        yield return new WaitUntil(() => !CriAtom.CueSheetsAreLoading);

        bgmAcb = CriAtom.GetAcb("BGM");
        seAcb = CriAtom.GetAcb("SE");
        
        CueSheetLoaded = true;
    }

    public void PlayBGM(string _CueName) {
        if (exPlayer.GetStatus() != CriAtomExPlayer.Status.Playing) {
            if (exPlayer2.GetStatus() == CriAtomExPlayer.Status.Playing) {
                exPlayer2.Stop();
            }

            exPlayer.SetCue(bgmAcb, _CueName);
            playBack = exPlayer.Start();
            return;
        } else if (exPlayer2.GetStatus() != CriAtomExPlayer.Status.Playing) {
            if (exPlayer.GetStatus() == CriAtomExPlayer.Status.Playing) {
                exPlayer.Stop();
            }

            exPlayer2.SetCue(bgmAcb, _CueName);
            playBack = exPlayer2.Start();
            return;
        } else {
            if (exPlayer2.GetStatus() == CriAtomExPlayer.Status.Playing) {
                exPlayer2.Stop();
            }

            exPlayer.SetCue(bgmAcb, _CueName);
            playBack = exPlayer.Start();
        }
    }

    public void StopAllBGM() {
        if (exPlayer.GetStatus() == CriAtomExPlayer.Status.Playing)
            exPlayer.Stop();
        if (exPlayer2.GetStatus() == CriAtomExPlayer.Status.Playing)
            exPlayer2.Stop();
    }

    public void SetAisacValue(string name, float value) {
        if (exPlayer.GetStatus() == CriAtomExPlayer.Status.Playing)
        {
            exPlayer.SetAisacControl(name, value);
            exPlayer.UpdateAll();
        }
        else if (exPlayer2.GetStatus() == CriAtomExPlayer.Status.Playing)
        {
            exPlayer2.SetAisacControl(name, value);
            exPlayer2.UpdateAll();
        }
    }

    public void PlaySE(string name) {
        exSePlayer.SetCue(seAcb, name);
        exSePlayer.Start();
    }
    
    public bool PauseNowPlayBGM() {
        playBack.Pause();
        return playBack.IsPaused();
    }
    
    public bool ResumeNowPauseBGM() {
        playBack.Resume(CriAtomEx.ResumeMode.PausedPlayback);
        return playBack.IsPaused();
    }
    
    public void SetCategoryVolume(string name, float volume) => CriAtom.SetCategoryVolume(name, volume);
    
    public void SetNextBlockIndex(int index) => playBack.SetNextBlockIndex(index);
    public long GetPlayTime() => playBack.GetTime();
}
