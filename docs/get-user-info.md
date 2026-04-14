# 获取当前用户账户信息

## OpenAPI Specification

```yaml
openapi: 3.0.1
info:
  title: ''
  description: ''
  version: 1.0.0
paths:
  /api/account/info:
    get:
      summary: 获取当前用户账户信息
      deprecated: false
      description: ''
      tags:
        - 用户账户
        - 用户账户
      parameters: []
      responses:
        '200':
          description: ''
          content:
            application/json:
              schema:
                type: object
                properties:
                  status:
                    type: integer
                    description: 状态码
                    examples:
                      - 200
                    x-apifox-mock: '@pick([''200''])'
                  message:
                    type: string
                    description: 状态信息
                    examples:
                      - OK
                    x-apifox-mock: OK
                  user:
                    type: object
                    description: 用户信息
                    properties:
                      id:
                        type: string
                        description: 用户 ID
                        x-apifox-mock: '{{$string.uuid}}'
                      name:
                        type: string
                        description: 用户昵称
                        x-apifox-mock: '{{$internet.userName}}'
                      email:
                        type: string
                        description: 用户邮箱，如果是手机号注册的用户，会隐藏部分信息
                        x-apifox-mock: '{{$internet.email}}'
                      phone:
                        type: string
                        description: 用户手机号，隐藏中间四位
                        x-apifox-mock: >-
                          {{$helpers.arrayElement(['137','186','191','188','139','138','135','132'])}}****{{$string.numeric(length=4)}}
                      avatar:
                        type: string
                        description: 用户头像 URL，如果用户没有设置头像，会使用根据邮箱生成的 Cravatar 头像
                        x-apifox-mock: '{{$image.url}}'
                      role:
                        type: string
                        description: 用户权限组
                        enum:
                          - user
                          - admin
                          - banned
                          - enterprise
                        x-apifox-enum:
                          - name: ''
                            value: user
                            description: 普通用户
                          - name: ''
                            value: admin
                            description: 管理员
                          - name: ''
                            value: banned
                            description: 已封禁
                          - name: ''
                            value: enterprise
                            description: 企业用户
                        default: user
                        examples:
                          - user
                        x-apifox-mock: user
                      isPaid:
                        type: boolean
                        description: 是否为已付费用户
                      notifications:
                        type: string
                        description: 未读通知数量，超过 99 则显示为 '99+'
                        examples:
                          - '20'
                        x-apifox-mock: '@integer(0, 99)'
                      credits:
                        type: number
                        description: 用户剩余点数
                    x-apifox-orders:
                      - id
                      - name
                      - email
                      - phone
                      - avatar
                      - role
                      - isPaid
                      - notifications
                      - credits
                    required:
                      - id
                      - name
                      - email
                      - phone
                      - avatar
                      - role
                      - isPaid
                      - notifications
                      - credits
                x-apifox-orders:
                  - status
                  - message
                  - user
                required:
                  - status
                  - message
                  - user
          headers: {}
          x-apifox-name: 成功获取用户账户信息
      security:
        - bearer: []
      x-apifox-folder: 用户账户
      x-apifox-status: released
      x-run-in-apifox: https://app.apifox.com/web/project/5878926/apis/api-261932870-run
components:
  schemas: {}
  securitySchemes:
    bearer:
      type: http
      scheme: bearer
servers:
  - url: https://v1.vocu.ai
    description: 后端API
security:
  - bearer: []

```
