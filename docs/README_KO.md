<div align="center">
  <a href="https://github.com/Cirnouo/STranslate.Plugin.Tts.Vocu">
    <img src="images/icon.svg" alt="Vocu TTS" width="128" height="128" />
  </a>

  <h1>Vocu TTS</h1>

  <p>
    <a href="https://vocu.ai">Vocu</a> / <a href="https://wusound.cn">Wusound</a> 음성 합성 플러그인 for <a href="https://github.com/ZGGSONG/STranslate">STranslate</a>
  </p>

  <p>
    <img alt="License" src="https://img.shields.io/github/license/Cirnouo/STranslate.Plugin.Tts.Vocu?style=flat-square" />
    <img alt="Release" src="https://img.shields.io/github/v/release/Cirnouo/STranslate.Plugin.Tts.Vocu?style=flat-square" />
    <img alt="Downloads" src="https://img.shields.io/github/downloads/Cirnouo/STranslate.Plugin.Tts.Vocu/total?style=flat-square" />
    <img alt=".NET" src="https://img.shields.io/badge/.NET-10.0-512bd4?style=flat-square" />
    <img alt="WPF" src="https://img.shields.io/badge/WPF-Plugin-blue?style=flat-square" />
  </p>

  <p>
    <a href="../README.md">简体中文</a> | <a href="README_TW.md">繁體中文</a> | <a href="README_EN.md">English</a> | <a href="README_JA.md">日本語</a> | <b>한국어</b>
  </p>
</div>

---

<div align="center">
  <img src="images/overview.png" alt="플러그인 개요" width="700" />
</div>

## 기능

- **듀얼 사이트 지원** — [Vocu](https://vocu.ai) (국제판)와 [Wusound](https://wusound.cn) (중국 내수판)를 동시 관리, 원클릭 전환
- **감정 제어** — 분노, 기쁨, 평온, 슬픔, 텍스트 추종 — 각각 0~10단계 조절 가능
- **풍부한 매개변수** — 말하기 속도, 프리셋 (creative / balance / stable), 자동 언어 감지, 시드 제어
- **고급 모드** — 텍스트 감정 우선, 생동감 표현 (V3.0), 저지연 Flash 모드
- **계정 정보** — 실시간 크레딧 잔액, 아바타, API 지연 표시 (녹색 / 노란색 / 빨간색)
- **다국어 UI** — 简体中文, 繁體中文, English, 日本語, 한국어

## 설치

1. [Releases](https://github.com/Cirnouo/STranslate.Plugin.Tts.Vocu/releases) 페이지에서 최신 `.spkg` 파일을 다운로드
2. STranslate에서 **설정 → 플러그인 → 플러그인 설치**를 열기
3. 다운로드한 `.spkg` 파일을 선택하고 STranslate를 재시작

> [!TIP]
> `.spkg`는 ZIP 파일입니다. STranslate가 자동으로 압축을 풀고 로드합니다.

## 사전 요구 사항

다음 플랫폼 중 하나에서 계정을 등록하고 API Key를 발급받으세요:

| 사이트 | URL | 대상 지역 |
| :--: | :-- | :--: |
| **Vocu** | [vocu.ai](https://vocu.ai) | 국제 |
| **Wusound** | [wusound.cn](https://wusound.cn) | 중국 대륙 |

두 사이트의 API는 완전히 호환됩니다 — 엔드포인트, 매개변수, 응답 형식이 모두 동일합니다.

## 설정

<details>
<summary><b>매개변수 목록</b> (클릭하여 펼치기)</summary>

| 매개변수 | 기본값 | 설명 |
| :-- | :--: | :-- |
| 사이트 | Vocu | Vocu (국제) 또는 Wusound (중국 내수) |
| API Key | — | 선택한 사이트의 Bearer Token |
| 음성 ID | — | 음성 캐릭터 ID (필수), 사이트에서 생성 |
| 프롬프트 스타일 | `default` | 프롬프트 스타일 ID |
| 프리셋 | `balance` | `creative` / `balance` / `stable` |
| 언어 | `auto` | 자동 감지 또는 언어 지정 |
| 말하기 속도 | `1.0` | 0.5 ~ 2.0 |
| 텍스트 감정 | 켜짐 | 음색 클론보다 텍스트 감정 스타일 우선 |
| 생동감 표현 | 꺼짐 | V3.0 음성 전용 생동감 모드 |
| 저지연 | 꺼짐 | Flash 저지연 모드 |
| 감정 제어 | `0` | 분노 / 기쁨 / 평온 / 슬픔 / 텍스트 추종 (각 0-10) |
| 시드 | `-1` | -1은 랜덤 |

</details>

### 스크린샷

<details>
<summary><b>UI 스크린샷</b> (클릭하여 펼치기)</summary>

#### 사이트 전환

<img src="images/site_switcher.png" alt="사이트 전환" width="240" />

#### 계정 및 API

<img src="images/account_and_api.png" alt="계정 및 API" width="450" />

#### 음성 설정

<img src="images/voice_config.png" alt="음성 설정" width="450" />

#### 생성 매개변수

<img src="images/generation_parameters.png" alt="생성 매개변수" width="450" />

#### 감정 제어

<img src="images/emotion_control.png" alt="감정 제어" width="450" />

#### 고급 옵션

<img src="images/advanced_options.png" alt="고급 옵션" width="450" />

#### 기타 및 지원

<img src="images/others_and_support.png" alt="기타 및 지원" width="450" />

</details>

## 빌드

```powershell
# 표준 빌드 (Debug + .spkg 패키징)
.\build.ps1

# 클린 빌드
.\build.ps1 -Clean

# Release 빌드
.\build.ps1 -Configuration Release
```

빌드 결과물은 저장소 루트에 `STranslate.Plugin.Tts.Vocu.spkg`로 출력됩니다.

<details>
<summary><b>필수 환경</b></summary>

- .NET 10.0 SDK
- Windows (WPF 프로젝트)

</details>

## 감사의 말

- [STranslate](https://github.com/ZGGSONG/STranslate) — 바로 사용할 수 있는 번역 및 OCR 도구
- [Vocu](https://vocu.ai) / [Wusound](https://wusound.cn) — 음성 합성 API 제공자
- [iNKORE WPF Modern UI](https://github.com/iNKORE-NET/UI.WPF.Modern) — WPF 모던 UI 컨트롤 라이브러리

## 라이선스

[MIT](../LICENSE)
