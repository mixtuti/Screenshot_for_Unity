using UnityEngine;
using System;

namespace Screenshot
{
    public class ScreenshotSettings : MonoBehaviour
    {
        // 設定項目
        public string saveDirectory;
        public string fileNamePrefix = "Screenshot_";
        public string fileExtension = ".png";
        public KeyCode screenshotKey = KeyCode.Space;

        // タイムスタンプの形式を選べる列挙型
        public enum TimestampFormat
        {
            None,               // タイムスタンプなし
            Simple,             // yyyyMMdd_HHmmss
            Detailed,           // yyyy-MM-dd_HH-mm-ss
            Custom,             // カスタム形式（ユーザー定義）
            Basic,              // yyyyMMddHHmmss
            Short,              // yyyyMMdd_HHmm
            DateOnly,           // yyyyMMdd
            TimeOnly,           // HHmmss
            Millisecond,        // yyyyMMdd_HHmmssfff
        }

        public TimestampFormat timestampFormat = TimestampFormat.Basic; // デフォルトのタイムスタンプ形式

        // シングルトンインスタンス
        public static ScreenshotSettings Instance { get; private set; }

        void Awake()
        {
            // シングルトンを保持
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject); // 他のインスタンスがあれば破棄
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // シーン変更時にも破棄されない

                // ビルド設定から会社名とゲーム名を取得
                string companyName = Application.companyName;
                string productName = Application.productName;

                // 最初にピクチャフォルダをデフォルトの保存先として設定
                if (string.IsNullOrEmpty(saveDirectory))
                {
                    // ピクチャフォルダのパスを取得
                    string picturesFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), companyName, productName);
                    saveDirectory = picturesFolder;
                }

                // フォルダが存在しない場合は作成
                if (!System.IO.Directory.Exists(saveDirectory))
                {
                    System.IO.Directory.CreateDirectory(saveDirectory);
                }
            }
        }

        // タイムスタンプ付きのファイル名を生成するメソッド
        public string GetScreenshotFileName()
        {
            string timestamp = GetTimestamp();
            return fileNamePrefix + timestamp + fileExtension;
        }

        // 現在のタイムスタンプを取得するメソッド
        private string GetTimestamp()
        {
            string timestamp = string.Empty;
            switch (timestampFormat)
            {
                case TimestampFormat.Simple:
                    timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    break;
                case TimestampFormat.Detailed:
                    timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    break;
                case TimestampFormat.Custom:
                    // カスタム形式を設定する場合、ユーザーが設定した形式を利用
                    timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss"); // 必要に応じてカスタマイズ
                    break;
                case TimestampFormat.Basic:
                    timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    break;
                case TimestampFormat.Short:
                    timestamp = DateTime.Now.ToString("yyyyMMdd_HHmm");
                    break;
                case TimestampFormat.DateOnly:
                    timestamp = DateTime.Now.ToString("yyyyMMdd");
                    break;
                case TimestampFormat.TimeOnly:
                    timestamp = DateTime.Now.ToString("HHmmss");
                    break;
                case TimestampFormat.Millisecond:
                    timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmssfff");
                    break;
                case TimestampFormat.None:
                default:
                    // タイムスタンプなし
                    timestamp = string.Empty;
                    break;
            }
            return timestamp;
        }
    }
}
