using UnityEngine;

namespace Screenshot
{
    public class PlaySoundOnScreenshot : MonoBehaviour, IScreenshotExtension
    {
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip screenshotSound;

        public void BeforeScreenshot()
        {
            // 音を鳴らす前の処理
        }

        public void AfterScreenshot(string filePath)
        {
            // スクリーンショット後に音を鳴らす
            if (screenshotSound != null)
            {
                AudioSource.PlayClipAtPoint(screenshotSound, Camera.main.transform.position);
            }
        }
    }
}
