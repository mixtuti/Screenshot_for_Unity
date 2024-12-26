using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace Screenshot
{
    public static class ScreenShotCore
    {
        public static void TakeScreenshot()
        {
            // 拡張機能の処理を実行（撮影前）
            var extensionManager = GameObject.FindObjectOfType<ScreenshotExtensionsManager>();
            if (extensionManager != null)
            {
                extensionManager.InvokeBeforeScreenshot();
            }

            // スクリーンショットの処理
            string fileName = ScreenshotSettings.Instance.GetScreenshotFileName();  // タイムスタンプを含むファイル名を取得
            string filePath = Path.Combine(ScreenshotSettings.Instance.saveDirectory, fileName);

            if (!Directory.Exists(ScreenshotSettings.Instance.saveDirectory))
            {
                Directory.CreateDirectory(ScreenshotSettings.Instance.saveDirectory);
            }

            // ScreenCapture.CaptureScreenshot を使用してスクリーンショットを撮る
            ScreenCapture.CaptureScreenshot(filePath);
            Debug.Log($"Screenshot saved to {filePath}");

            // 拡張機能の処理を実行（撮影後）
            if (extensionManager != null)
            {
                extensionManager.InvokeAfterScreenshot(filePath);
            }
        }

        // スクリーンショットを撮るメソッド
        public static void CaptureScreenshot(string filePath, Camera targetCamera = null, Vector2? customResolution = null, List<string> layersToCapture = null)
        {
            // 拡張機能の処理を実行（撮影前）
            var extensionManager = GameObject.FindObjectOfType<ScreenshotExtensionsManager>();
            if (extensionManager != null)
            {
                extensionManager.InvokeBeforeScreenshot();
            }

            // カメラのcullingMaskを撮影前に保存
            int originalCullingMask = targetCamera != null ? targetCamera.cullingMask : 0;

            // 解像度の設定
            if (customResolution.HasValue)
            {
                Screen.SetResolution((int)customResolution.Value.x, (int)customResolution.Value.y, false);
            }

            // レイヤー設定の処理
            if (layersToCapture == null || layersToCapture.Count == 0 || layersToCapture.Contains("Everything"))
            {
                // "Everything" または空の場合は、すべてのレイヤーを対象に設定
                if (targetCamera != null)
                {
                    targetCamera.cullingMask = -1;  // すべてのレイヤーを表示
                }
            }
            else
            {
                // レイヤーに基づいてカメラのcullingMaskを設定
                int layerMask = 0;
                foreach (var layer in layersToCapture)
                {
                    layerMask |= 1 << LayerMask.NameToLayer(layer);
                }

                if (targetCamera != null)
                {
                    targetCamera.cullingMask = layerMask;
                }
            }

            // カメラが指定されていない場合、デフォルトで画面全体をキャプチャ
            if (targetCamera == null)
            {
                ScreenCapture.CaptureScreenshot(filePath);
            }
            else
            {
                // RenderTextureを使ってカメラからの映像をキャプチャ
                RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
                targetCamera.targetTexture = renderTexture;

                Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
                targetCamera.Render();

                // RenderTextureからデータを取得して保存
                RenderTexture.active = renderTexture;
                screenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
                screenshotTexture.Apply();

                byte[] bytes = screenshotTexture.EncodeToPNG();
                File.WriteAllBytes(filePath, bytes);

                // 使用後にリソースを解放
                RenderTexture.active = null;
                targetCamera.targetTexture = null;
                UnityEngine.Object.Destroy(renderTexture);
            }

            // 拡張機能の処理を実行（撮影後）
            if (extensionManager != null)
            {
                extensionManager.InvokeAfterScreenshot(filePath);
            }

            // 撮影後、元のcullingMaskをカメラに戻す
            if (targetCamera != null)
            {
                targetCamera.cullingMask = originalCullingMask;
            }

            Debug.Log($"Screenshot saved to {filePath}");
        }

        // スクリーンショットキーのチェックを行うメソッド
        public static void CheckForScreenshotKey()
        {
            if (Input.GetKeyDown(ScreenshotSettings.Instance.screenshotKey))
            {
                TakeScreenshot();
            }
        }

        // 拡張されたスクリーンショットキーのチェックを行うメソッド
        public static void CheckForExtendedScreenshotKey(Camera targetCamera = null, Vector2? customResolution = null, List<string> layersToCapture = null)
        {
            if (Input.GetKeyDown(ScreenshotSettings.Instance.screenshotKey))
            {
                // キーが押されたらスクリーンショットを撮る
                string filePath = Path.Combine(ScreenshotSettings.Instance.saveDirectory, ScreenshotSettings.Instance.GetScreenshotFileName());
                CaptureScreenshot(filePath, targetCamera, customResolution, layersToCapture);
            }
        }
    }
}
