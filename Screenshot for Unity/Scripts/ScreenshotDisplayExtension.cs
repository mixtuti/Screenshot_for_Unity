using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace Screenshot
{
    // 解像度プリセットを追加
    public enum ResolutionPreset
    {
        FullHD,    // 1920x1080
        HD,        // 1280x720
        SD,        // 640x480
        Half,      // 画面サイズの半分
        Quarter,   // 画面サイズの四分の一
        Eighth,    // 画面サイズの八分の一
        Custom     // カスタムサイズ
    }

    public enum PositionPreset
    {
        Center,        // 真ん中
        TopRight,      // 右上
        BottomRight,   // 右下
        TopLeft,       // 左上
        BottomLeft,    // 左下
        Custom         // カスタム位置
    }

    public class ScreenshotDisplayExtension : MonoBehaviour, IScreenshotExtension
    {
        public RawImage rawImage;
        public PositionPreset positionPreset = PositionPreset.Center; // 位置プリセット
        public Vector3 customPosition = new Vector3(0, 0, 0); // カスタム位置
        public bool shouldFadeOut = false; // フェードアウト処理を行うかどうか
        public float fadeDuration = 1.0f; // フェードアウトにかかる時間
        public bool shouldShrinkOnFade = false; // フェードアウト中にサイズを小さくするか
        public float shrinkFactor = 0.5f; // サイズ縮小の割合

        // 新たに追加された解像度プリセット
        public ResolutionPreset resolutionPreset = ResolutionPreset.FullHD;

        // カスタムサイズ
        public float customWidth = 1920f;
        public float customHeight = 1080f;

        // 画像を表示後に削除するかどうか
        public bool deleteAfterDisplay = false;

        void Start()
        {
            initImage();
        }

        // スクリーンショットを撮る前に呼ばれる
        public void BeforeScreenshot()
        {
            // 特に何もする必要なし
        }

        // スクリーンショットを撮った後に呼ばれる
        public async void AfterScreenshot(string filePath)
        {
            await WaitForFileToExist(filePath);

            if (File.Exists(filePath))
            {
                byte[] fileData = File.ReadAllBytes(filePath);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(fileData);

                // 解像度を設定
                SetRawImageResolution();

                // 画像の表示位置を設定
                SetRawImagePosition();

                // 表示
                rawImage.color = new Color(255, 255, 255, 255);
                rawImage.texture = texture;
                rawImage.enabled = true;
                Debug.Log("Screenshot loaded and displayed.");

                // フェードアウト処理を開始
                if (shouldFadeOut)
                {
                    await FadeOut(rawImage, fadeDuration);
                }
                else
                {
                    // フェードしない場合、指定した時間後に即座に透明にする
                    await UniTask.Delay((int)(fadeDuration * 1000)); // ミリ秒単位で待機
                    rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, 0f);
                    rawImage.enabled = false;
                    Debug.Log("RawImage immediately faded out.");
                }

                // 画像を表示後に削除する場合
                if (deleteAfterDisplay)
                {
                    // 画像を削除
                    File.Delete(filePath);
                    Debug.Log($"Screenshot file deleted: {filePath}");
                }
            }
            else
            {
                Debug.LogError($"File not found at path: {filePath}");
            }
        }

        // ファイルが存在するまで待機するメソッド
        private async UniTask WaitForFileToExist(string filePath)
        {
            while (!File.Exists(filePath))
            {
                await UniTask.Yield(); // 次のフレームまで待機
            }
            Debug.Log("File exists, proceeding.");
        }

        // RawImageをフェードアウトさせるメソッド
        private async UniTask FadeOut(RawImage image, float duration)
        {
            float timeElapsed = 0f;
            Color startColor = image.color;
            Vector2 startSize = image.rectTransform.sizeDelta;

            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, timeElapsed / duration);

                // サイズ縮小処理
                if (shouldShrinkOnFade)
                {
                    // サイズ縮小の割合をリニアに変更
                    float shrinkFactorValue = Mathf.Lerp(1f, shrinkFactor, timeElapsed / duration);
                    image.rectTransform.sizeDelta = new Vector2(startSize.x * shrinkFactorValue, startSize.y * shrinkFactorValue);
                }

                image.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                await UniTask.Yield(); // 次のフレームまで待機
            }

            // 最終的な透明化
            image.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
            image.rectTransform.sizeDelta = new Vector2(startSize.x * shrinkFactor, startSize.y * shrinkFactor); // 最終的に縮小
            image.enabled = false;
            Debug.Log("RawImage faded out and hidden.");
        }

        public void initImage()
        {
            Color initColor = rawImage.color;
            rawImage.color = new Color(initColor.r, initColor.g, initColor.b, 0f);
        }

        // 解像度をプリセットに応じて設定
        private void SetRawImageResolution()
        {
            switch (resolutionPreset)
            {
                case ResolutionPreset.FullHD:
                    SetRawImageSize(1920f, 1080f); // FullHD
                    break;
                case ResolutionPreset.HD:
                    SetRawImageSize(1280f, 720f); // HD
                    break;
                case ResolutionPreset.SD:
                    SetRawImageSize(640f, 480f); // SD
                    break;
                case ResolutionPreset.Half:
                    SetRawImageSize(Screen.width / 2f, Screen.height / 2f); // 画面サイズの半分
                    break;
                case ResolutionPreset.Quarter:
                    SetRawImageSize(Screen.width / 4f, Screen.height / 4f); // 画面サイズの四分の一
                    break;
                case ResolutionPreset.Eighth:
                    SetRawImageSize(Screen.width / 8f, Screen.height / 8f); // 画面サイズの八分の一
                    break;
                case ResolutionPreset.Custom:
                    SetRawImageSize(customWidth, customHeight); // カスタムサイズ
                    break;
                default:
                    SetRawImageSize(1920f, 1080f); // デフォルトはFullHD
                    break;
            }
        }

        // RawImageのサイズを設定するメソッド
        private void SetRawImageSize(float width, float height)
        {
            rawImage.rectTransform.sizeDelta = new Vector2(width, height); // サイズ設定
        }

        // RawImageの位置を設定するメソッド
        private void SetRawImagePosition()
        {
            switch (positionPreset)
            {
                case PositionPreset.Center:
                    rawImage.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                    break;
                case PositionPreset.TopRight:
                    rawImage.transform.position = new Vector3(Screen.width - rawImage.rectTransform.rect.width / 2, Screen.height - rawImage.rectTransform.rect.height / 2, 0);
                    break;
                case PositionPreset.BottomRight:
                    rawImage.transform.position = new Vector3(Screen.width - rawImage.rectTransform.rect.width / 2, rawImage.rectTransform.rect.height / 2, 0);
                    break;
                case PositionPreset.TopLeft:
                    rawImage.transform.position = new Vector3(rawImage.rectTransform.rect.width / 2, Screen.height - rawImage.rectTransform.rect.height / 2, 0);
                    break;
                case PositionPreset.BottomLeft:
                    rawImage.transform.position = new Vector3(rawImage.rectTransform.rect.width / 2, rawImage.rectTransform.rect.height / 2, 0);
                    break;
                case PositionPreset.Custom:
                    rawImage.transform.position = customPosition;
                    break;
                default:
                    rawImage.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                    break;
            }
        }
    }
}
