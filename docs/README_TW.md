<div align="center">
  <a href="https://github.com/Cirnouo/STranslate.Plugin.Tts.Vocu">
    <img src="images/icon.svg" alt="Vocu TTS" width="128" height="128" />
  </a>

  <h1>Vocu TTS</h1>

  <p>
    <a href="https://vocu.ai">Vocu</a> / <a href="https://wusound.cn">悟聲</a> 語音合成外掛 for <a href="https://github.com/ZGGSONG/STranslate">STranslate</a>
  </p>

  <p>
    <img alt="License" src="https://img.shields.io/github/license/Cirnouo/STranslate.Plugin.Tts.Vocu?style=flat-square" />
    <img alt="Release" src="https://img.shields.io/github/v/release/Cirnouo/STranslate.Plugin.Tts.Vocu?style=flat-square" />
    <img alt="Downloads" src="https://img.shields.io/github/downloads/Cirnouo/STranslate.Plugin.Tts.Vocu/total?style=flat-square" />
    <img alt=".NET" src="https://img.shields.io/badge/.NET-10.0-512bd4?style=flat-square" />
    <img alt="WPF" src="https://img.shields.io/badge/WPF-Plugin-blue?style=flat-square" />
  </p>

  <p>
    <a href="../README.md">简体中文</a> | <b>繁體中文</b> | <a href="README_EN.md">English</a> | <a href="README_JA.md">日本語</a> | <a href="README_KO.md">한국어</a>
  </p>
</div>

---

<div align="center">
  <img src="images/overview.png" alt="外掛總覽" width="700" />
</div>

## 功能特色

- **雙站點支援** — 同時管理 [Vocu](https://vocu.ai)（國際版）和 [悟聲 Wusound](https://wusound.cn)（國內版），一鍵切換
- **情緒控制** — 憤怒、開心、平靜、悲傷、跟隨文字，每項 0-10 級可調
- **豐富參數** — 語速、參數預設（creative / balance / stable）、語言自動偵測、種子控制
- **進階模式** — 文字情感優先、生動表達（V3.0）、低延遲 Flash 模式
- **帳戶資訊** — 即時顯示剩餘點數、頭像、API 延遲指示
- **多語言 UI** — 簡體中文、繁體中文、English、日本語、한국어

## 安裝

1. 前往 [Releases](https://github.com/Cirnouo/STranslate.Plugin.Tts.Vocu/releases) 頁面下載最新 `.spkg` 檔案
2. 在 STranslate 中進入 **設定 → 外掛 → 安裝外掛**
3. 選擇下載的 `.spkg` 檔案，重新啟動 STranslate

> [!TIP]
> `.spkg` 本質是 ZIP 檔案，STranslate 會自動解壓載入。

## 前置條件

在以下任一平台註冊帳號並取得 API Key：

| 站點 | 地址 | 適用地區 |
| :--: | :-- | :--: |
| **Vocu** | [vocu.ai](https://vocu.ai) | 國際 |
| **悟聲 Wusound** | [wusound.cn](https://wusound.cn) | 中國大陸 |

兩個站點的 API 完全相容，功能和參數一致。

## 設定說明

<details>
<summary><b>參數一覽</b>（點擊展開）</summary>

| 參數 | 預設值 | 說明 |
| :-- | :--: | :-- |
| 站點 | Vocu | Vocu（國際）或 悟聲（國內） |
| API Key | — | 對應站點的 Bearer Token |
| 語音角色 ID | — | 語音角色（必填），在對應站點建立 |
| 角色風格 | `default` | Prompt 風格 ID |
| 參數預設 | `balance` | `creative` / `balance` / `stable` |
| 語言 | `auto` | 自動偵測或指定語言 |
| 語速 | `1.0` | 0.5 ~ 2.0 |
| 文字情感 | 開 | 偏向文字情感風格而非音色克隆 |
| 生動表達 | 關 | V3.0 角色生動模式 |
| 低延遲 | 關 | Flash 低延遲模式 |
| 情緒控制 | `0` | 憤怒 / 開心 / 平靜 / 悲傷 / 跟隨文字（各 0-10） |
| 種子 | `-1` | -1 為隨機 |

</details>

### 截圖

<details>
<summary><b>介面截圖</b>（點擊展開）</summary>

#### 站點切換

<img src="images/site_switcher.png" alt="站點切換" width="240" />

#### 帳戶與 API

<img src="images/account_and_api.png" alt="帳戶與 API" width="450" />

#### 語音角色設定

<img src="images/voice_config.png" alt="語音角色設定" width="450" />

#### 生成參數

<img src="images/generation_parameters.png" alt="生成參數" width="450" />

#### 情緒控制

<img src="images/emotion_control.png" alt="情緒控制" width="450" />

#### 進階選項

<img src="images/advanced_options.png" alt="進階選項" width="450" />

#### 其他與支援

<img src="images/others_and_support.png" alt="其他與支援" width="450" />

</details>

## 建置

```powershell
# 標準建置（Debug + .spkg 打包）
.\build.ps1

# 清理後建置
.\build.ps1 -Clean

# Release 建置
.\build.ps1 -Configuration Release
```

建置產物輸出到倉庫根目錄 `STranslate.Plugin.Tts.Vocu.spkg`。

<details>
<summary><b>環境需求</b></summary>

- .NET 10.0 SDK
- Windows（WPF 專案）

</details>

## 致謝

- [STranslate](https://github.com/ZGGSONG/STranslate) — 即用即走的翻譯和 OCR 工具
- [Vocu](https://vocu.ai) / [悟聲](https://wusound.cn) — 語音合成 API 提供商
- [iNKORE WPF Modern UI](https://github.com/iNKORE-NET/UI.WPF.Modern) — WPF 現代 UI 控制項庫

## 授權條款

[MIT](../LICENSE)
