namespace Screenshot
{
    public interface IScreenshotExtension
    {
        // スクリーンショットを撮る前に呼ばれる
        void BeforeScreenshot();

        // スクリーンショットを撮った後に呼ばれる
        void AfterScreenshot(string filePath);
    }
}
