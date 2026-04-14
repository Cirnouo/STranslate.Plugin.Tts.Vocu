<div align="center">
  <a href="https://github.com/Cirnouo/STranslate.Plugin.Tts.Vocu">
    <img src="docs/images/icon.svg" alt="Vocu TTS" width="128" height="128" />
  </a>

  <h1>Vocu TTS</h1>

  <p>
    <a href="https://vocu.ai">Vocu</a> / <a href="https://wusound.cn">悟声</a> 语音合成插件 for <a href="https://github.com/ZGGSONG/STranslate">STranslate</a>
  </p>

  <p>
    <img alt="License" src="https://img.shields.io/github/license/Cirnouo/STranslate.Plugin.Tts.Vocu?style=flat-square" />
    <img alt="Release" src="https://img.shields.io/github/v/release/Cirnouo/STranslate.Plugin.Tts.Vocu?style=flat-square" />
    <img alt="Downloads" src="https://img.shields.io/github/downloads/Cirnouo/STranslate.Plugin.Tts.Vocu/total?style=flat-square" />
    <img alt=".NET" src="https://img.shields.io/badge/.NET-10.0-512bd4?style=flat-square" />
    <img alt="WPF" src="https://img.shields.io/badge/WPF-Plugin-blue?style=flat-square" />
  </p>

  <p>
    <b>简体中文</b> | <a href="docs/README_EN.md">English</a>
  </p>
</div>

---

<div align="center">
  <img src="docs/images/overview.png" alt="插件总览" width="700" />
</div>

## 功能特性

- **双站点支持** — 同时管理 [Vocu](https://vocu.ai)（国际版）和 [悟声 Wusound](https://wusound.cn)（国内版），一键切换
- **情绪控制** — 愤怒、开心、平静、悲伤、跟随文本，每项 0-10 级可调
- **丰富参数** — 语速、参数预设（creative / balance / stable）、语言自动检测、种子控制
- **高级模式** — 文本情感优先、生动表达（V3.0）、低延迟 Flash 模式
- **账户信息** — 实时显示剩余点数、头像、API 延迟指示
- **多语言 UI** — 简体中文、繁體中文、English、日本語、한국어

## 安装

1. 前往 [Releases](https://github.com/Cirnouo/STranslate.Plugin.Tts.Vocu/releases) 页面下载最新 `.spkg` 文件
2. 在 STranslate 中进入 **设置 → 插件 → 安装插件**
3. 选择下载的 `.spkg` 文件，重启 STranslate

> [!TIP]
> `.spkg` 本质是 ZIP 文件，STranslate 会自动解压加载。

## 前置条件

在以下任一平台注册账号并获取 API Key：

| 站点 | 地址 | 适用地区 |
| :--: | :-- | :--: |
| **Vocu** | [vocu.ai](https://vocu.ai) | 国际 |
| **悟声 Wusound** | [wusound.cn](https://wusound.cn) | 中国大陆 |

两个站点的 API 完全兼容，功能和参数一致。

## 配置说明

<details>
<summary><b>参数一览</b>（点击展开）</summary>

| 参数 | 默认值 | 说明 |
| :-- | :--: | :-- |
| 站点 | Vocu | Vocu（国际）或 悟声（国内） |
| API Key | — | 对应站点的 Bearer Token |
| 语音角色 ID | — | 语音角色（必填），在对应站点创建 |
| 角色风格 | `default` | Prompt 风格 ID |
| 参数预设 | `balance` | `creative` / `balance` / `stable` |
| 语言 | `auto` | 自动检测或指定语言 |
| 语速 | `1.0` | 0.5 ~ 2.0 |
| 文本情感 | 开 | 偏向文本情感风格而非音色克隆 |
| 生动表达 | 关 | V3.0 角色生动模式 |
| 低延迟 | 关 | Flash 低延迟模式 |
| 情绪控制 | `0` | 愤怒 / 开心 / 平静 / 悲伤 / 跟随文本（各 0-10） |
| 种子 | `-1` | -1 为随机 |

</details>

### 截图

<details>
<summary><b>界面截图</b>（点击展开）</summary>

#### 站点切换

<img src="docs/images/site_switcher.png" alt="站点切换" width="240" />

#### 账户与 API

<img src="docs/images/account_and_api.png" alt="账户与 API" width="450" />

#### 语音角色配置

<img src="docs/images/voice_config.png" alt="语音角色配置" width="450" />

#### 生成参数

<img src="docs/images/generation_parameters.png" alt="生成参数" width="450" />

#### 情绪控制

<img src="docs/images/emotion_control.png" alt="情绪控制" width="450" />

#### 高级选项

<img src="docs/images/advanced_options.png" alt="高级选项" width="450" />

#### 其他与支持

<img src="docs/images/others_and_support.png" alt="其他与支持" width="450" />

</details>

## 构建

```powershell
# 标准构建（Debug + .spkg 打包）
.\build.ps1

# 清理后构建
.\build.ps1 -Clean

# Release 构建
.\build.ps1 -Configuration Release
```

构建产物输出到仓库根目录 `STranslate.Plugin.Tts.Vocu.spkg`。

<details>
<summary><b>环境要求</b></summary>

- .NET 10.0 SDK
- Windows（WPF 项目）

</details>

## 致谢

- [STranslate](https://github.com/ZGGSONG/STranslate) — 即用即走的翻译和 OCR 工具
- [Vocu](https://vocu.ai) / [悟声](https://wusound.cn) — 语音合成 API 提供商
- [iNKORE WPF Modern UI](https://github.com/iNKORE-NET/UI.WPF.Modern) — WPF 现代 UI 控件库

## 许可证

[MIT](LICENSE)

