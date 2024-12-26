using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Screenshot;

namespace Screenshot
{
    // カメラに関するスクリーンショット拡張機能
    public class ScreenshotCameraExtension : MonoBehaviour, IScreenshotExtension
    {
        public Camera targetCamera;            // 撮影対象のカメラ
        public List<LayerMask> layersToCapture;   // 撮影するレイヤー（LayerMaskで保持）
        public bool captureMultipleResolutions = false; // 複数解像度での撮影を有効にするか
        public Vector2 customResolution = new Vector2(1920, 1080); // カスタム解像度

        // 解像度のプリセット
        public enum ResolutionPreset
        {
            FullHD,    // 1920x1080
            HD,        // 1280x720
            SD         // 640x480
        }

        public ResolutionPreset resolutionPreset = ResolutionPreset.FullHD; // 初期解像度設定

        void Update()
        {
            if (targetCamera == null)
            {
                ScreenShotCore.CheckForScreenshotKey();
            }
            else
            {
                // layersToCaptureがnullまたは空の場合にデフォルト値を設定
                if (layersToCapture == null || layersToCapture.Count == 0)
                {
                    layersToCapture = new List<LayerMask> { LayerMask.NameToLayer("Everything") }; // "Everything" レイヤーを追加
                }
                ScreenShotCore.CheckForExtendedScreenshotKey(targetCamera, customResolution, ConvertLayerMasksToStrings(layersToCapture));
            }
        }

        public void BeforeScreenshot()
        {
            // 特に設定しない
        }

        public void AfterScreenshot(string filePath)
        {
            // 撮影後の処理はここに記述（拡張機能を実行するなど）
        }

        // 複数解像度での撮影処理をまとめたメソッド
        public void CaptureMultipleResolutions()
        {
            string baseFilePath = Path.Combine(ScreenshotSettings.Instance.saveDirectory, ScreenshotSettings.Instance.GetScreenshotFileName());

            // 解像度に基づくスクリーンショット撮影
            Screenshot.ScreenShotCore.CaptureScreenshot(GenerateFilePath(baseFilePath, 1920, 1080), targetCamera, new Vector2(1920, 1080), ConvertLayerMasksToStrings(layersToCapture));
            Screenshot.ScreenShotCore.CaptureScreenshot(GenerateFilePath(baseFilePath, 1280, 720), targetCamera, new Vector2(1280, 720), ConvertLayerMasksToStrings(layersToCapture));
            Screenshot.ScreenShotCore.CaptureScreenshot(GenerateFilePath(baseFilePath, 640, 480), targetCamera, new Vector2(640, 480), ConvertLayerMasksToStrings(layersToCapture));
        }

        // ファイルパスを解像度に基づいて生成するメソッド
        private string GenerateFilePath(string baseFilePath, int width, int height)
        {
            return baseFilePath.Replace(Path.GetExtension(baseFilePath), $"_{width}x{height}{Path.GetExtension(baseFilePath)}");
        }

        // 通常のスクリーンショットを撮るメソッド
        public void CaptureScreenshot(string filePath)
        {
            // 複数レイヤーをLayerMaskに追加
            int combinedLayerMask = 0;

            // 各LayerMaskをビット演算で組み合わせる
            foreach (var layerMask in layersToCapture)
            {
                combinedLayerMask |= layerMask.value; // valueプロパティでレイヤーマスクの値を取得
            }

            // カメラの cullingMask を設定
            if (targetCamera != null)
            {
                targetCamera.cullingMask = combinedLayerMask;
            }

            Screenshot.ScreenShotCore.CaptureScreenshot(filePath, targetCamera, customResolution, ConvertLayerMasksToStrings(layersToCapture));
        }

        private List<string> ConvertLayerMasksToStrings(List<LayerMask> layerMasks)
        {
            List<string> layerNames = new List<string>();

            foreach (LayerMask layerMask in layerMasks)
            {
                // LayerMaskが「Everything」の場合
                if (layerMask.value == -1) // すべてのレイヤーを対象にしている場合
                {
                    layerNames.Add("Everything");
                }
                else
                {
                    // LayerMask からレイヤー名を取得
                    int layerIndex = Mathf.FloorToInt(Mathf.Log(layerMask.value, 2));
                    string layerName = LayerMask.LayerToName(layerIndex);
                    layerNames.Add(layerName);
                }
            }
            return layerNames;
        }

        // スクリーンショットを撮る処理をまとめたメソッド
        public void TakeScreenshot()
        {
            // 解像度とカメラ設定をチェック
            if (captureMultipleResolutions)
            {
                CaptureMultipleResolutions();
            }
            else
            {
                string filePath = Path.Combine(ScreenshotSettings.Instance.saveDirectory, ScreenshotSettings.Instance.GetScreenshotFileName());
                CaptureScreenshot(filePath);
            }
        }
    }
}
