# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2026-04-15

首个正式发布版本。

### Added

- 双站点支持：Vocu（国际版）和悟声 Wusound（国内版），ComboBox 一键切换，各站点独立配置（API Key、语音角色、参数互不干扰）
- 情绪控制：愤怒、开心、平静、悲伤、跟随文本，每项 0-10 级独立可调
- 生成参数：语速（0.5-2.0）、参数预设（creative / balance / stable）、语言自动检测、种子控制
- 高级模式：文本情感优先、生动表达（V3.0 角色）、低延迟 Flash 模式
- 账户信息：实时显示剩余点数、头像、API 延迟指示（绿 ≤300ms / 黄 ≤800ms / 红 >800ms）
- 多语言 UI：简体中文、繁體中文、English、日本語、한국어
- v1 → v2 配置迁移：自动检测旧版扁平格式并迁移到双站点结构，一次性、幂等
- 构建脚本 `build.ps1`：支持 Debug/Release、Clean、CleanOnly，自动生成 `.spkg` 插件包
- GitHub Actions CI：tag push 自动构建并上传 `.spkg` 到 Release

[1.0.0]: https://github.com/Cirnouo/STranslate.Plugin.Tts.Vocu/releases/tag/v1.0.0
