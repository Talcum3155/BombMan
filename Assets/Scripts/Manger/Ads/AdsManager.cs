using System;
using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

namespace Manger.Ads
{
    public class AdsManager : Singleton<AdsManager>, IUnityAdsLoadListener, IUnityAdsShowListener, IUnityAdsInitializationListener
    {
        [SerializeField] public Button _showAdButton;
        [SerializeField] string _androidAdUnitId = "Rewarded_Android";
        [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
        string _adUnitId = null; // This will remain null for unsupported platforms
        
        [SerializeField] string _androidGameId;
        [SerializeField] string _iOSGameId;
        [SerializeField] bool _testMode;
        private string _gameId;

        void Awake()
        {
            DontDestroyOnLoad(this);
            InitializeAds();
            
            // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
            _adUnitId = _androidAdUnitId;
#endif

            //Disable the button until the ad is ready to show:
            _showAdButton.interactable = false;
        }

        // Load content to the Ad Unit:
        public void LoadAd()
        {
            // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
            Debug.Log("加载广告: " + _adUnitId);
            Advertisement.Load(_adUnitId, this);
        }

        // If the ad successfully loads, add a listener to the button and enable it:
        public void OnUnityAdsAdLoaded(string adUnitId)
        {
            if (adUnitId.Equals(_adUnitId))
            {
                Debug.Log("广告已加载: " + adUnitId);
                
                // Configure the button to call the ShowAd() method when clicked:
                _showAdButton.onClick.AddListener(ShowAd);
                Debug.Log("绑定事件");
                // Enable the button for users to click:
                _showAdButton.interactable = true;
            }
        }

        // Implement a method to execute when the user clicks the button:
        public void ShowAd()
        {
            Debug.Log("播放广告");
            
            // Disable the button:
            _showAdButton.interactable = false;
            // Then show the ad:
            Advertisement.Show(_adUnitId, this);
        }

        // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
        public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            // if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            if (adUnitId.Equals(_adUnitId))
            {
                Debug.Log("广告播放完毕，给予奖励");
                // Grant a reward.

                var player = GameManager.Instance.player;
                player.isDead = false;
                player.health = 3;
                player.god = true;
                UIManager.Instance.UpdateHealth(3);
                UIManager.Instance.joystick.gameObject.SetActive(true);
                //复活
                player.animator.SetBool(PlayerController.Resurrection,true);
                UIManager.Instance.gameOverPanel.SetActive(false);

                // Load another ad:
                Advertisement.Load(_adUnitId, this);
            }
        }

        // Implement Load and Show Listener error callbacks:
        public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"广告加载异常 Ad Unit {adUnitId}: {error.ToString()} - {message}");
            // Use the error details to determine whether to try to load another ad.
        }

        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
            // Use the error details to determine whether to try to load another ad.
        }

        public void OnUnityAdsShowStart(string adUnitId)
        {
            Debug.Log("开始");
        }

        public void OnUnityAdsShowClick(string adUnitId)
        {
            Debug.Log("点击");
        }

        void OnDestroy()
        {
            // Clean up the button listeners:
            _showAdButton.onClick.RemoveAllListeners();
        }

        public void InitializeAds()
        {
            _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? _iOSGameId
                : _androidGameId;
            Advertisement.Initialize(_gameId, _testMode, this);
        }
        
        public void OnInitializationComplete()
        {
            Debug.Log("广告初始化完成");
            LoadAd();
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"广告初始化失败: {error.ToString()} - {message}");
        }
    }
}