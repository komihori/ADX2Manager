using CriWare;
using System.Collections;
using UnityEngine;

public class ADX2Manager : MonoBehaviour
{
    public static ADX2Manager Instance { get; private set; }
    private CriAtomExPlayer exPlayer, exPlayer2, exSePlayer;
    private CriAtomExAcb bgmAcb, seAcb;
    private int fadeInTime = 1000;
    private int fadeOutTime = 1000;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
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

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => !CriAtom.CueSheetsAreLoading);

        bgmAcb = CriAtom.GetAcb("BGM");
        seAcb = CriAtom.GetAcb("SE");
    }

    public void PlayBGM(string _CueName)
    {
        if (exPlayer.GetStatus() != CriAtomExPlayer.Status.Playing)
        {
            if (exPlayer2.GetStatus() == CriAtomExPlayer.Status.Playing)
                exPlayer2.Stop();
            exPlayer.SetCue(bgmAcb, _CueName);
            exPlayer.Start();
            return;
        }
        else if (exPlayer2.GetStatus() != CriAtomExPlayer.Status.Playing)
        {
            if (exPlayer.GetStatus() == CriAtomExPlayer.Status.Playing)
                exPlayer.Stop();
            exPlayer2.SetCue(bgmAcb, _CueName);
            exPlayer2.Start();
            return;
        }
    }

    public void StopAllBGM()
    {
        if (exPlayer.GetStatus() == CriAtomExPlayer.Status.Playing)
            exPlayer.Stop();
        if (exPlayer2.GetStatus() == CriAtomExPlayer.Status.Playing)
            exPlayer2.Stop();
    }

    public void SetAisacValue(string name, float value)
    {
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

    public void PlaySE(string name)
    {
        exSePlayer.SetCue(seAcb, name);
        exSePlayer.Start();
    }

}