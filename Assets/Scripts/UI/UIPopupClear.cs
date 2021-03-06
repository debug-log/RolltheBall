﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIPopupClear : UIPopup
{
    public UIImageSwitcher[] imageStars;
    public UnityAdsHelper unityAdsHelper;

    public Button btnRetry;
    public Button btnExit;
    public Button btnNext;

    public Text textNext;

    public void Activate (int numStars)
    {
        OpenWithDelay (0.5f, () => SetActiveStarImages (numStars));
    }

    private void SetActiveStarImages (int numStars)
    {
        for (int i = 0; i < numStars && i < imageStars.Length; i++)
        {
            imageStars[i].SetImageChanged ();
        }
    }

    private void Start ()
    {
        int nextSceneId = Player.Instance.GetPlayerHasNextStage (Player.Instance.GetPlayerSelectedStageId ());
        if (nextSceneId == 0)
        {
            btnNext.interactable = false;
            textNext.text = "...";
        }

        btnRetry.onClick.AddListener (OnClickRetryButton);
        btnExit.onClick.AddListener (OnClickExitButton);
        btnNext.onClick.AddListener (OnClickNextButton);

        unityAdsHelper = UnityAdsHelper.Instance;

        AudioManager.Instance.PlayAudioEffect (AudioInfo.AudioType.Success);
    }

    private void OnClickRetryButton()
    {
        bool unityAdsPlay = Player.Instance.GetShowOrNotUnityAdsPlay ();
        if (unityAdsPlay && unityAdsHelper != null)
        {
            unityAdsHelper.ShowAds (StartCurrentStage);
        }
        else
        {
            StartCurrentStage ();
        }
        AudioManager.Instance.PlayAudioEffect (AudioInfo.AudioType.UiTap);
    }

    private void OnClickExitButton ()
    {
        SceneManager.LoadScene (SceneName.SCENE_MAIN);
        AudioManager.Instance.PlayAudioEffect (AudioInfo.AudioType.UiTap);
    }

    private void OnClickNextButton()
    {
        int nextSceneId = Player.Instance.GetPlayerHasNextStage (Player.Instance.GetPlayerSelectedStageId ());
        if (nextSceneId != 0)
        {
            Player.Instance.SetPlayerSelectedStageId (nextSceneId);
        }

        bool unityAdsPlay = Player.Instance.GetShowOrNotUnityAdsPlay ();
        if (unityAdsPlay && unityAdsHelper != null)
        {
            unityAdsHelper.ShowAds (StartNextStage);
        }
        else
        {
            StartNextStage ();
        }
        AudioManager.Instance.PlayAudioEffect (AudioInfo.AudioType.UiTap);
    }

    private void StartCurrentStage ()
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }

    private void StartNextStage ()
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }
}
