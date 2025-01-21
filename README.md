# Screenshot_for_Unity

![代替テキスト](https://img.shields.io/badge/Unity-2022.3+-orange) <img src="http://img.shields.io/badge/UniTask-2.5.10-orange.svg?style=flat"> <img src="http://img.shields.io/badge/License-MIT-blue.svg?style=flat"> <img src="http://img.shields.io/badge/Language-C%23-green.svg?style=flat"><br>
Unityでスクリーンショットを撮影するためのライブラリです。<br>
指定のレイヤーのみ撮影すること、撮影した画像を画面に表示など最低限のスクリーンショット機能を実装しました。

## システム要件

Unity 2022.3.28 での動作は確認済みです。</br>
UniTask 2.5.10 が必要になります。

https://github.com/Cysharp/UniTask

## 概要
Unityでスクリーンショットを撮影するためのライブラリです。<br>
指定のレイヤーのみ撮影すること、撮影した画像を画面に表示など最低限のスクリーンショット機能を実装しました。

## 依存関係

UniTask 2.5.10

## 導入方法

### 1. プロジェクトへの導入
導入方法は大きく分けて2つあります。お好きな方法で導入してください。

#### 1. Unity Package Managerを使う方法
「Window > Package Manager」を開き、「Add Package from git URL」を選択します。<br>
その後、以下のURLを入力してください。
```
https://github.com/mixtuti/Screenshot_for_Unity.git?path=Screenshot%20for%20Unity
```
#### 2. Import Packageを使う方法
リリースから最新のUnity Packageをダウンロードし、インポートします。
> [!TIP]
> 更新が遅くなることが多いので1の方法を使うことをお勧めします。

### 2. 利用方法
プレハブフォルダに入っているオブジェクトをシーン内に配置します。<br>
その後、適当なゲームオブジェクトにScreenshotCameraExtension.csをアタッチしてください。<br>
必要に応じてPlaySoundOnScreenshot.csやScreenshotDisplayExtension.csをアタッチしてください。

## スクリプトの解説

### 1. ScreenShotCore.cs
スクリーンショットを撮るための機能を提供するスクリプト<br>
撮影するカメラやレイヤー解像度などを設定できます。

### 2. ScreenshotExtensionsManager.cs
スクリーンショットの前後でカスタムの処理をするためのスクリプト

### 3. IScreenshotExtension.cs
スクリーンショットを撮る前後に呼ばれる2つのメソッドを定義するインターフェイス

### 4. ScreenshotSettings.cs
スクリーンショットの設定をまとめたスクリプト

### 5. PlaySoundOnScreenshot.cs
スクリーンショット撮影後に効果音を鳴らすためのスクリプト

### 6. ScreenshotCameraExtension.cs
カメラ撮影に関する拡張機能を提供するスクリプト<br>
任意のカメラで撮影、撮影するレイヤーの指定、解像度指定などをサポート

### 7. ScreenshotDisplayExtension.cs
スクリーンショット撮影後に、画像を画面上に表示するためのスクリプト<br>
表示する位置や時間、サイズなどを指定できる

## 関数

### 1. ScreenShotCore.cs
#### 1. TakeScreenshot()
スクリーンショットを撮影するための関数。<br>
- 引数: なし
- 戻り値: なし<br>
このメソッドはスクリーンショットを撮影するためのメインのメソッドです。ScreenshotExtensionsManager.csが存在すれば、撮影前後に拡張機能の処理を実行します。<br>
その後、ScreenCapture.CaptureScreenshotを使用してスクリーンショットを指定されたファイルパスに保存します。

#### 2. CaptureScreenshot(string filePath, Camera targetCamera = null, Vector2? customResolution = null, List<string> layersToCapture = null)
指定されたファイルパスにスクリーンショットを保存するメソッド。<br>
- 引数:
  - filePath (string): 保存するファイルパス。
  - targetCamera (Camera, オプション): スクリーンショットを撮影するカメラ。指定しない場合は画面全体をキャプチャ。
  - customResolution (Vector2?, オプション): カスタム解像度を指定する場合の解像度。
  - layersToCapture (List<string>, オプション): 撮影するレイヤーのリスト。指定しない場合、すべてのレイヤーがキャプチャされます。
- 戻り値: なし<br>
このメソッドは、指定されたファイルパスにスクリーンショットを保存します。<br>
targetCameraが指定されていればそのカメラから、指定されていなければ画面全体をキャプチャします。<br>
また、解像度の設定やレイヤーの制御も行います。

#### 3. InvokeBeforeScreenshot()
スクリーンショット撮影前に拡張機能を実行するメソッド。<br>
- 引数: なし
- 戻り値: なし<br>
このメソッドは、スクリーンショットを撮影する前に呼び出されます。<br>
拡張機能があれば、それらの BeforeScreenshotメソッドを実行します。

#### 4. InvokeAfterScreenshot(string filePath)
スクリーンショット撮影後に拡張機能を実行するメソッド。<br>
- 引数:
  - filePath (string): 保存されたスクリーンショットのファイルパス。
- 戻り値: なし<br>
このメソッドは、スクリーンショットを撮影した後に呼び出されます。<br>
拡張機能があれば、それらの AfterScreenshot メソッドを実行し、撮影後の処理を行います。

### 2. ScreenshotExtensionsManager.cs
#### 1. InvokeBeforeScreenshot()
スクリーンショット撮影前に拡張機能を実行するメソッド。<br>
- 引数: なし
- 戻り値: なし<br>
このメソッドは、スクリーンショットを撮影する前に呼び出されます。<br>
シーン内のすべての拡張機能 (IScreenshotExtension インターフェースを実装したオブジェクト) の BeforeScreenshot メソッドを実行します。<br>
これにより、スクリーンショットが撮影される前に、各拡張機能が任意の処理を行うことができます。

#### 2. InvokeAfterScreenshot(string filePath)
スクリーンショット撮影後に拡張機能を実行するメソッド。<br>
- 引数:
  - filePath (string): 保存されたスクリーンショットのファイルパス。
- 戻り値: なし<br>
このメソッドは、スクリーンショットを撮影した後に呼び出されます。<br>
拡張機能があれば、それらの AfterScreenshot メソッドを実行し、撮影後の処理を行います。<br>
filePath には、保存されたスクリーンショットのファイルパスが渡され、拡張機能はそれを利用して後処理を行うことができます。

### 6. ScreenshotCameraExtension
#### 1. BeforeScreenshot()
スクリーンショット撮影前に呼ばれるメソッド。<br>
- 引数: なし
- 戻り値: なし<br>
このメソッドは、スクリーンショットが撮影される前に呼び出されます。<br>
現状では特別な処理は行っていませんが、必要に応じて拡張することができます。

#### 2. AfterScreenshot(string filePath)
スクリーンショット撮影後に呼ばれるメソッド。<br>
- 引数:
  - filePath (string): 保存されたスクリーンショットのファイルパス。
- 戻り値: なし<br>
このメソッドは、スクリーンショットが撮影された後に呼び出されます。<br>
現状では特別な処理は行っていませんが、拡張機能を実行するなど、撮影後の処理を追加できます。

#### 3. CaptureMultipleResolutions()
複数解像度でのスクリーンショットを撮影するメソッド。<br>
- 引数: なし
- 戻り値: なし<br>
このメソッドは、指定された解像度で複数のスクリーンショットを撮影します。<br>
解像度は、1920x1080、1280x720、640x480の3つのプリセットで撮影します。<br>
それぞれの解像度でスクリーンショットを保存するために、CaptureScreenshot() メソッドを呼び出します。

#### 4. GenerateFilePath(string baseFilePath, int width, int height)
解像度に基づいてファイルパスを生成するメソッド。<br>
- 引数:
  - baseFilePath (string): 基本となるファイルパス。
  - width (int): 解像度の横幅。
  - height (int): 解像度の縦幅。
- 戻り値: (string): 解像度が追加された新しいファイルパス。<br>
このメソッドは、解像度に基づいてファイルパスを生成します。<br>
例えば、ファイル名に 1920x1080 の解像度を追加する形で保存先を決定します。

#### 5. CaptureScreenshot(string filePath)
指定した解像度とカメラ設定でスクリーンショットを撮るメソッド。<br>
- 引数:
  - filePath (string): 保存先のファイルパス。
- 戻り値: なし<br>
このメソッドは、指定された filePath にスクリーンショットを保存します。<br>
レイヤー設定に基づいて、カメラの cullingMask を設定し、特定のレイヤーをキャプチャ対象にします。

#### 6. ConvertLayerMasksToStrings(List<LayerMask> layerMasks)
レイヤーマスクをレイヤー名に変換するメソッド。<br>
- 引数:
  - layerMasks (List<LayerMask>): レイヤーマスクのリスト。
- 戻り値: (List<string>): レイヤー名のリスト。<br>
このメソッドは、指定された LayerMask のリストをレイヤー名に変換します。<br>
「Everything」の場合にはその文字列を返し、他のレイヤーの場合はそれに対応するレイヤー名を返します。

#### 7. TakeScreenshot()
スクリーンショットを撮影するメソッド。<br>
- 引数: なし
- 戻り値: なし<br>
このメソッドは、captureMultipleResolutions が true の場合、複数の解像度でスクリーンショットを撮影します。<br>
それ以外の場合は、CaptureScreenshot() メソッドを呼び出して通常のスクリーンショットを撮影します。

### 7. ScreenshotDisplayExtension.cs
#### 1. BeforeScreenshot()
スクリーンショット撮影前に呼ばれるメソッド。<br>
- 引数: なし
- 戻り値: なし<br>
このメソッドは、スクリーンショットが撮影される前に呼び出されます。<br>
現在は特別な処理を行っていませんが、必要に応じて拡張することができます。

#### 2. AfterScreenshot(string filePath)
スクリーンショット撮影後に呼ばれるメソッド。<br>
- 引数:
  - filePath (string): 保存されたスクリーンショットのファイルパス。
- 戻り値: なし<br>
このメソッドは、スクリーンショットが撮影された後に呼び出されます。<br>
ファイルが存在するまで待機し、画像を読み込んで表示します。<br>
オプションで、フェードアウトや縮小処理を実行できます。

#### 3. WaitForFileToExist(string filePath)
指定されたファイルが存在するまで待機するメソッド。<br>
- 引数:
  - filePath (string): ファイルのパス。
- 戻り値: なし<br>
このメソッドは、指定されたファイルが存在するまで待機します。<br>
ファイルが存在するようになると、処理を続行します。

#### 4. FadeOut(RawImage image, float duration)
指定された RawImage をフェードアウトさせるメソッド。<br>
- 引数:
  - image (RawImage): フェードアウトさせる対象の RawImage コンポーネント。
  - duration (float): フェードアウトにかかる時間（秒）。
- 戻り値: UniTask<br>
このメソッドは、指定された RawImage を指定された duration の間にフェードアウトさせます。<br>
オプションで、フェードアウト中に画像のサイズを縮小する処理も行えます。

#### 5. initImage()
RawImage の初期設定を行うメソッド。<br>
- 引数: なし
- 戻り値: なし<br>
このメソッドは、RawImage の色を透明に初期化します。<br>
画像の表示を開始する前に、このメソッドを呼び出しておきます。

#### 6. SetRawImageResolution()
解像度プリセットに基づいて RawImage の解像度を設定するメソッド。<br>
- 引数: なし
- 戻り値: なし<br>
このメソッドは、resolutionPreset に応じて RawImage の解像度を設定します。<br>
解像度プリセットは、FullHD、HD、SD、画面サイズの半分、四分の一、八分の一、またはカスタム解像度に設定できます。

#### 7. SetRawImageSize(float width, float height)
RawImage のサイズを設定するメソッド。<br>
- 引数:
  - width (float): 設定する幅。
  - height (float): 設定する高さ。
- 戻り値: なし<br>
このメソッドは、指定した width と height に基づいて RawImage のサイズを設定します。

#### 8. SetRawImagePosition()
解像度プリセットに基づいて RawImage の位置を設定するメソッド。<br>
- 引数: なし
- 戻り値: なし<br>
このメソッドは、positionPreset に基づいて RawImage の位置を設定します。<br>
位置の選択肢として、中央、右上、右下、左上、左下、またはカスタム位置を選べます。
