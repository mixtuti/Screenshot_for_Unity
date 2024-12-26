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
