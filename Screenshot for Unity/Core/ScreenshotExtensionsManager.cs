using System;
using System.Linq;
using UnityEngine;

namespace Screenshot
{
    public class ScreenshotExtensionsManager : MonoBehaviour
    {
        private IScreenshotExtension[] screenshotExtensions;

        void Start()
        {
            // シーン内の全てのIScreenshotExtensionを取得
            screenshotExtensions = FindObjectsOfType<MonoBehaviour>().OfType<IScreenshotExtension>().ToArray();
            Debug.Log($"Found {screenshotExtensions.Length} screenshot extensions.");
        }

        public void InvokeBeforeScreenshot()
        {
            // 各拡張機能のBeforeScreenshotメソッドを呼び出す
            foreach (var extension in screenshotExtensions)
            {
                extension.BeforeScreenshot();
            }
        }

        public void InvokeAfterScreenshot(string filePath)
        {
            // 各拡張機能のAfterScreenshotメソッドを呼び出す
            foreach (var extension in screenshotExtensions)
            {
                extension.AfterScreenshot(filePath);
            }
        }
    }
}
