<div align="center">
  <a href="https://github.com/Cirnouo/STranslate.Plugin.Tts.Vocu">
    <img src="images/icon.svg" alt="Vocu TTS" width="128" height="128" />
  </a>

  <h1>Vocu TTS</h1>

  <p>
    <a href="https://vocu.ai">Vocu</a> / <a href="https://wusound.cn">Wusound</a> 音声合成プラグイン for <a href="https://github.com/ZGGSONG/STranslate">STranslate</a>
  </p>

  <p>
    <img alt="License" src="https://img.shields.io/github/license/Cirnouo/STranslate.Plugin.Tts.Vocu?style=flat-square" />
    <img alt="Release" src="https://img.shields.io/github/v/release/Cirnouo/STranslate.Plugin.Tts.Vocu?style=flat-square" />
    <img alt="Downloads" src="https://img.shields.io/github/downloads/Cirnouo/STranslate.Plugin.Tts.Vocu/total?style=flat-square" />
    <img alt=".NET" src="https://img.shields.io/badge/.NET-10.0-512bd4?style=flat-square" />
    <img alt="WPF" src="https://img.shields.io/badge/WPF-Plugin-blue?style=flat-square" />
  </p>

  <p>
    <a href="../README.md">简体中文</a> | <a href="README_TW.md">繁體中文</a> | <a href="README_EN.md">English</a> | <b>日本語</b> | <a href="README_KO.md">한국어</a>
  </p>
</div>

---

<div align="center">
  <img src="images/overview.png" alt="プラグイン概要" width="700" />
</div>

## 機能

- **デュアルサイト対応** — [Vocu](https://vocu.ai)（国際版）と [Wusound](https://wusound.cn)（中国国内版）を同時管理、ワンクリックで切り替え
- **感情コントロール** — 怒り、喜び、平穏、悲しみ、テキスト追従 — 各 0〜10 段階で調整可能
- **豊富なパラメータ** — 話速、プリセット（creative / balance / stable）、言語自動検出、シード制御
- **アドバンスモード** — テキスト感情優先、ビビッド表現（V3.0）、低レイテンシ Flash モード
- **アカウント情報** — 残高、アバター、API レイテンシ表示（緑 / 黄 / 赤）をリアルタイム表示
- **多言語 UI** — 簡体字中国語、繁体字中国語、English、日本語、한국어

## インストール

1. [Releases](https://github.com/Cirnouo/STranslate.Plugin.Tts.Vocu/releases) ページから最新の `.spkg` ファイルをダウンロード
2. STranslate で **設定 → プラグイン → プラグインをインストール** を開く
3. ダウンロードした `.spkg` ファイルを選択し、STranslate を再起動

> [!TIP]
> `.spkg` は ZIP ファイルです。STranslate が自動的に解凍して読み込みます。

## 前提条件

以下のいずれかのプラットフォームでアカウントを登録し、API Key を取得してください：

| サイト | URL | 対象地域 |
| :--: | :-- | :--: |
| **Vocu** | [vocu.ai](https://vocu.ai) | 国際 |
| **Wusound** | [wusound.cn](https://wusound.cn) | 中国大陸 |

両サイトの API は完全互換 — エンドポイント、パラメータ、レスポンス形式がすべて同一です。

## 設定

<details>
<summary><b>パラメータ一覧</b>（クリックで展開）</summary>

| パラメータ | デフォルト | 説明 |
| :-- | :--: | :-- |
| サイト | Vocu | Vocu（国際）または Wusound（中国国内） |
| API Key | — | 選択サイトの Bearer Token |
| ボイス ID | — | ボイスキャラクター ID（必須）、サイトで作成 |
| プロンプトスタイル | `default` | プロンプトスタイル ID |
| プリセット | `balance` | `creative` / `balance` / `stable` |
| 言語 | `auto` | 自動検出または言語指定 |
| 話速 | `1.0` | 0.5〜2.0 |
| テキスト感情 | オン | 音色クローンよりテキスト感情スタイルを優先 |
| ビビッド表現 | オフ | V3.0 ボイス専用ビビッドモード |
| 低レイテンシ | オフ | Flash 低レイテンシモード |
| 感情コントロール | `0` | 怒り / 喜び / 平穏 / 悲しみ / テキスト追従（各 0-10） |
| シード | `-1` | -1 でランダム |

</details>

### スクリーンショット

<details>
<summary><b>UI スクリーンショット</b>（クリックで展開）</summary>

#### サイト切り替え

<img src="images/site_switcher.png" alt="サイト切り替え" width="240" />

#### アカウントと API

<img src="images/account_and_api.png" alt="アカウントと API" width="450" />

#### ボイス設定

<img src="images/voice_config.png" alt="ボイス設定" width="450" />

#### 生成パラメータ

<img src="images/generation_parameters.png" alt="生成パラメータ" width="450" />

#### 感情コントロール

<img src="images/emotion_control.png" alt="感情コントロール" width="450" />

#### アドバンスオプション

<img src="images/advanced_options.png" alt="アドバンスオプション" width="450" />

#### その他とサポート

<img src="images/others_and_support.png" alt="その他とサポート" width="450" />

</details>

## ビルド

```powershell
# 標準ビルド（Debug + .spkg パッケージング）
.\build.ps1

# クリーンビルド
.\build.ps1 -Clean

# Release ビルド
.\build.ps1 -Configuration Release
```

ビルド成果物はリポジトリルートに `STranslate.Plugin.Tts.Vocu.spkg` として出力されます。

<details>
<summary><b>必要環境</b></summary>

- .NET 10.0 SDK
- Windows（WPF プロジェクト）

</details>

## 謝辞

- [STranslate](https://github.com/ZGGSONG/STranslate) — すぐに使える翻訳・OCR ツール
- [Vocu](https://vocu.ai) / [Wusound](https://wusound.cn) — 音声合成 API プロバイダー
- [iNKORE WPF Modern UI](https://github.com/iNKORE-NET/UI.WPF.Modern) — WPF モダン UI コントロールライブラリ

## ライセンス

[MIT](../LICENSE)
