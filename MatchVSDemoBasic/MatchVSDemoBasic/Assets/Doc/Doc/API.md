## init

```
 public int init(MatchVSResponse[] pMatchVSResponse,string channel,string platform,int gameid);
```



### 参数

| 参数               | 类型              | 描述            | 示例值       |
| ---------------- | --------------- | ------------- | --------- |
| pMatchVSResponse | MatchVSResponse | 注册回调函数        |           |
| channel          | string          | 渠道，固定值        | "MatchVS" |
| platform         | string          | 平台，选择测试or正式环境 | "Alpha"   |
| gameid           | int             | 游戏ID          | 2001003   |



### 说明

在连接至 MatchVS 前须对SDK进行初始化操作。此时选择连接测试环境还是正式环境，如果游戏属于调试阶段则连接至测试环境，游戏调试完成后即可发布到正式环境运行。发布之前须到官网控制台申请“发布上线” ，申请通过后在接口传”release“才会生效，否则将不能使用release环境。  



## 错误码

| 错误码  | 含义                                       |
| ---- | ---------------------------------------- |
| -1   | 失败                                       |
| -2   | channel 非法，请检查是否正确填写为 “MatchVS”          |
| -3   | platform 非法，请检查是否正确填写为 “alpha”  或 “release” |



## uninit

```
  public int uninit()
```

### 说明

SDK反初始化工作

## 错误码

| 错误码  | 含义   |
| ---- | ---- |
| -1   | 失败   |



## registerUser

```
public int registerUser()
```



### 返回值

| 参数     | 类型     | 描述      | 示例值    |
| ------ | ------ | ------- | ------ |
| userID | int    | 用户ID    | 123456 |
| token  | string | 用户Token | ""     |



### 说明

注册用户信息，用以获取一个合法的userID，通过此ID可以连接至MatchVS服务器。一个用户只需注册一次，不必重复注册。



## login

```
public int login(int userid, string token, int gameid, int gameVersion, string appkey, string secretkey, string deviceid, int gatewayid)
```



### 参数

| 参数          | 类型     | 描述                          | 示例值    |
| ----------- | ------ | --------------------------- | ------ |
| userid      | int    | 用户ID，调用注册接口后获取              | 123546 |
| token       | string | 用户token，调用注册接口后获取           | ""     |
| gameid      | int    | 游戏ID，来自MatchVS控制台游戏信息       | 210329 |
| gameVersion | int    | 游戏版本，自定义，用于隔离匹配空间           | 1      |
| appkey      | string | 游戏App key，来自MatchVS控制台游戏信息  | ""     |
| secretkey   | string | secret key，来自MatchVS控制台游戏信息 | ""     |
| deviceid    | string | 设备ID，用于多端登录检测，请保证是唯一ID      | ""     |
| gatewayid   | int    | 服务器节点ID，默认为0                | 0      |



### 返回值

| 参数     | 类型   | 描述   | 示例值    |
| ------ | ---- | ---- | ------ |
| status | int  | 状态返回 | 200    |
| roomID | int  | 房间号  | 210039 |



### 说明

登录MatchVS服务端，与MatchVS建立连接。

服务端会校验游戏信息是否合法，保证连接的安全性。

如果一个账号在两台设备上登录，则后登录的设备会连接失败。



## 错误码

| 错误码  | 含义             |
| ---- | -------------- |
| -1   | 失败             |
| -2   | 未初始化，请先调用初始化接口 |
| -3   | 正在登录           |
| -4   | 已经登录，请勿重复登录    |



## logout

```
public int logout()
```




### 返回值

| 参数     | 类型   | 描述   | 示例值  |
| ------ | ---- | ---- | ---- |
| status | int  | 返回值  | 200  |

### 说明

退出登录，断开与MatchVS的连接。

## 错误码

| 错误码  | 含义   |
| ---- | ---- |
| -1   | 失败   |



## joinRandom

```
 public int joinRandomRoom(int iMaxPlayer, string strUserProfile)
```



### 参数

| 参数             | 类型     | 描述       | 示例值  |
| -------------- | ------ | -------- | ---- |
| iMaxPlayer     | int    | 房间内最大玩家数 | 3    |
| strUserProfile | string | 玩家简介     | ""   |



### 返回值

| 参数           | 类型     | 描述       | 示例值    |
| ------------ | ------ | -------- | ------ |
| status       | int    | 返回值      | 200    |
| userInfoList |        | 房间已有用户列表 |        |
| userId       | int    | 用户ID     | 321    |
| userProfile  | string | 用户简介     | ""     |
| roomInfo     |        | 房间信息     |        |
| roomID       | int    | 房间号      | 210096 |
| roomProperty | string | 房间属性     | ""     |
| cpProto      | string | 负载数据     | ""     |

### 说明

当房间里人数等于IMaxPlayer时，房间人满。系统会将玩家随机加入到人未满且没有JoinOver的房间。

## 错误码

| 错误码  | 含义                            |
| ---- | ----------------------------- |
| -1   | 正在加入房间                        |
| -4   | 未login，请先调用login              |
| -12  | 已经JoinRoom并JoinOver，不允许重复加入房间 |
| -20  | maxPlayer 超出范围 ，maxPlayer须≤20 |



## joinOver

```
public int joinOver(string cpProto)
```



### 参数

| 参数      | 类型     | 描述   | 示例值  |
| ------- | ------ | ---- | ---- |
| cpProto | string | 负载信息 | ""   |


### 返回值

| 参数      | 类型     | 描述   | 示例值  |
| ------- | ------ | ---- | ---- |
| status  | int    | 返回值  | 200  |
| cpProto | string | 负载信息 |      |

### 说明

客户端调用该接口通知服务端：房间人数已够，不要再向房间加人。

## 错误码

| 错误码  | 含义   |
| ---- | ---- |
| -1   | 失败   |

## leaveRoom

```
public int leaveRoom(string payload)
```



### 参数

| 参数      | 类型     | 描述   | 示例值  |
| ------- | ------ | ---- | ---- |
| cpProto | string | 负载信息 | ""   |


### 返回值

| 参数      | 类型     | 描述   | 示例值    |
| ------- | ------ | ---- | ------ |
| status  | int    | 返回值  | 200    |
| roomID  | int    | 房间号  | 210039 |
| userID  | int    | 用户ID | 321    |
| cpProto | string | 负载信息 | ""     |



### 说明

退出房间，玩家退出房间后将不能再发送数据，也不能再接收到其他玩家发的数据。  

## 错误码

| 错误码  | 含义           |
| ---- | ------------ |
| -1   | 失败           |
| -6   | 未加入房间，请先加入房间 |



## sendEvent

```
public int sendEvent(string pMsg)
```

### 参数

| 参数   | 类型     | 描述   | 示例值     |
| ---- | ------ | ---- | ------- |
| pMsg | string | 消息内容 | “hello” |

### 返回值

| 参数     | 类型   | 描述   | 示例值  |
| ------ | ---- | ---- | ---- |
| status | int  | 返回值  | 200  |

### 说明

在进入房间后即可调用该接口进行消息发送，消息会发给房间里所有成员。

如果您想只发给部分成员，可以使用如下接口。



```
public int sendEvent(int iPriority, int iType,string pMsg,int iTargetType,int[] pTargetUserId)
```



### 参数

| 参数            | 类型     | 描述                                       | 示例值         |
| ------------- | ------ | ---------------------------------------- | ----------- |
| iPriority     | int    | 消息优先级，0~3，值越小越优先处理                       | 0           |
| iType         | int    | 消息类型。0表示转发给房间成员；1表示转发给game server；2表示转发给房间成员及game server | 0           |
| pMsg          | string | 消息内容                                     | ""          |
| iTargetType   | int    | 目标类型。0表示发送目标为目标列表成员；1表示发送目标为除目标列表成员以外的房间成员 | 0           |
| pTargetUserId | int[]  | 目标列表                                     | [1001,1002] |



### 返回值

| 参数     | 类型   | 描述   | 示例值  |
| ------ | ---- | ---- | ---- |
| status | int  | 返回值  | 200  |



### 说明

发送消息，可以指定发送对象，比如只给玩家A发送消息。  

优先级是相对的，SDK收到多个消息，会优先将优先级低的消息传给上层应用。如果所有消息优先级一样，则SDK会根据接收顺序依次将消息传给上层应用。



## 错误码

| 错误码  | 含义                            |
| ---- | ----------------------------- |
| -1   | 失败                            |
| -3   | priority 非法，priority须在0-3范围内  |
| -4   | type 非法                       |
| -5   | targetType 非法                 |
| -6   | targetnum 非法，targetnum不可以超过20 |



## joinRoomNotify



### 返回值

| 参数          | 类型     | 描述   | 示例值  |
| ----------- | ------ | ---- | ---- |
| userID      | int    | 用户ID | 321  |
| userProfile | string | 用户简介 | ""   |



### 说明

加入房间广播通知，当有其他玩家加入房间时客户端会收到该通知。



## leaveRoomNotify



### 返回值

| 参数     | 类型   | 描述   | 示例值  |
| ------ | ---- | ---- | ---- |
| userID | int  | 用户ID | 321  |



### 说明

离开房间广播通知，当房间内其他玩家离开房间时客户端会收到该通知。  



## sendEventNotify



### 返回值

| 参数       | 类型     | 描述      | 示例值  |
| -------- | ------ | ------- | ---- |
| srcUid   | int    | 推送方用户ID | 321  |
| priority | int    | 优先级     | 1    |
| cpProto  | string | 负载数据    | “”   |



### 说明

其他玩家发送的消息


## createRoom

```
public int createRoom(MsCreateRoomInfo roomInfo, string userProfile)
```

### 参数

| 参数             | 类型     | 描述       | 示例值  |
| -------------- | ------ | -------- | ---- |
| roomInfo     | MsCreateRoomInfo    | 房间信息 |    |
| name | string | 房间名称     | "matchvs"   | 
| maxplayer | int | 最大人数     | 3  | 
| mode | int | 模式     | 0  |
| canWatch | int | 可否观战     | 0  |
| visibility | int | 可否可见     | 0  |
| roomProperty | string | 房间属性     | ""  |


### 返回值

| 参数           | 类型     | 描述       | 示例值    |
| ------------ | ------ | -------- | ------ |
| status       | int    | 返回值      | 200    |
| roomID       | string | 房间号      | "15452154312454"       |
 

### 说明
创建房间



## joinRoom

```
public int joinRoom(string roomID,string userProfile)
```



### 参数

| 参数             | 类型     | 描述       | 示例值  |
| -------------- | ------ | -------- | ---- |
| roomID     | string    | 房间ID | "152451234542"    |
| userProfile | string | 玩家简介     | ""   |



### 返回值

| 参数           | 类型     | 描述       | 示例值    |
| ------------ | ------ | -------- | ------ |
| status       | int    | 返回值      | 200    |
| userInfoList |        | 房间已有用户列表 |        |
| userId       | int    | 用户ID     | 321    |
| userProfile  | string | 用户简介     | ""     |
| roomInfo     |        | 房间信息     |        |
| roomID       | int    | 房间号      | 210096 |
| roomProperty | string | 房间属性     | ""     |
| cpProto      | string | 负载数据     | ""     |

### 说明

通过输入房间号码来加入用户自定义房间。


## joinRoomWithProperties

```
public int joinRoomWithProperties(MsMatchInfo matchInfo,string userProfile)
```



### 参数

| 参数             | 类型     | 描述       | 示例值  |
| -------------- | ------ | -------- | ---- |
| maxPlayer     | int    | 最大人数 | 3   |
| mode | int | 模式     | 0   |
| canWatch | int | 可否观看     | 0   |
| tags | list | 属性标签     | 0   |
| key | string | 标签key     | “key”   |
| value | string | 标签value     | “A”   |


### 返回值

| 参数           | 类型     | 描述       | 示例值    |
| ------------ | ------ | -------- | ------ |
| status       | int    | 返回值      | 200    |
| userInfoList |        | 房间已有用户列表 |        |
| userId       | int    | 用户ID     | 321    |
| userProfile  | string | 用户简介     | ""     |
| roomInfo     |        | 房间信息     |        |
| roomID       | int    | 房间号      | 210096 |
| roomProperty | string | 房间属性     | ""     |
| cpProto      | string | 负载数据     | ""     |

### 说明

通过指定匹配属性匹配指定玩家


## joinOver

```
public int joinOver(string cpProto)
```



### 参数

| 参数      | 类型     | 描述   | 示例值  |
| ------- | ------ | ---- | ---- |
| cpProto | string | 负载信息 | ""   |


### 返回值

| 参数      | 类型     | 描述   | 示例值  |
| ------- | ------ | ---- | ---- |
| status  | int    | 返回值  | 200  |
| cpProto | string | 负载信息 |      |

### 说明

客户端调用该接口通知服务端：房间人数已够，不要再向房间加人。


## subscribeEventGroup

```
public int subscribeEventGroup(string[] subGroups, string[] unsubGroups)
```



### 参数

| 参数      | 类型     | 描述   | 示例值  |
| ------- | ------ | ---- | ---- |
| subGroups | string[] | 订阅分组 | ""   |
| unsubGroups | string[] | 取消订阅分组 | ""   |


### 返回值

| 参数      | 类型     | 描述   | 示例值  |
| ------- | ------ | ---- | ---- |
| status  | int    | 返回值  | 200  |
| subGroups | string[] | 订阅分组 |      |

### 说明

订阅分组与取消订阅分组


## sendEventGroup

```
public int sendEventGroup(int priority, string cpProto, string[] groups)
```



### 参数

| 参数      | 类型     | 描述   | 示例值  |
| ------- | ------ | ---- | ---- |
| priority | int | 权重 | 3   |
| cpProto | string | 消息 | ""   |
| groups | string[] | 订阅组 | {"matchvs"}   |


### 返回值

| 参数      | 类型     | 描述   | 示例值  |
| ------- | ------ | ---- | ---- |
| status  | int    | 返回值  | 200  |
|  dstNum  |  int  |    目标人数     |   2  |     


### 说明

发送订阅消息


## kickPlayer

```
public int kickPlayer(int userID,string cpProto)
```



### 参数

| 参数      | 类型     | 描述   | 示例值  |
| ------- | ------ | ---- | ---- |
| userID | int | 玩家ID | 553584   |
| cpProto | string | 消息 | ""   |


### 返回值

| 参数      | 类型     | 描述   | 示例值  |
| ------- | ------ | ---- | ---- |
| status  | int    | 返回值  | 200  |


### 说明
踢除房间成员


## getRoomList

```
public int getRoomList(MsRoomFilter roomFilter)
```





### 参数

| 参数      | 类型     | 描述   | 示例值  |
| ------- | ------ | ---- | ---- |
| maxPlayer | int | 最大人数 | 3   |
| mode | int | 模式 | 0   |
| canWatch | int | 可否观看 | 0   |
| roomProperty | string | 房间属性 | 0   |


### 返回值

| 参数      | 类型     | 描述   | 示例值  |
| ------- | ------ | ---- | ---- |
| status  | int    | 返回值  | 200  |
| roomID | string | 房间ID |  "156231454561"    |
| roomName | string | 房间名称 |  "matchvs"    |
| maxPlayer | int | 最大人数 |  3    |
| mode | int | 模式 |  0    |
| canWatch | int | 可否观战 |  0    |
| roomProperty | string | 房间属性 |  ""    |


### 说明

获取房间列表


## leaveRoom

```
public int leaveRoom(string payload)
```



### 参数

| 参数      | 类型     | 描述   | 示例值  |
| ------- | ------ | ---- | ---- |
| cpProto | string | 负载信息 | ""   |


### 返回值

| 参数      | 类型     | 描述   | 示例值    |
| ------- | ------ | ---- | ------ |
| status  | int    | 返回值  | 200    |
| roomID  | int    | 房间号  | 210039 |
| userID  | int    | 用户ID | 321    |
| cpProto | string | 负载信息 | ""     |



### 说明

退出房间，玩家退出房间后将不能再发送数据，也不能再接收到其他玩家发的数据。  



## sendEvent

```
public int sendEvent(int iPriority, int iType,string pMsg,int iTargetType,int[] pTargetUserId)
```



### 参数

| 参数            | 类型     | 描述                                       | 示例值         |
| ------------- | ------ | ---------------------------------------- | ----------- |
| iPriority     | int    | 消息优先级，0~3，值越小越优先处理                       | 0           |
| iType         | int    | 消息类型。0表示转发给其他玩家；1表示转发给game server；2表示转发给其他玩家及game server | 0           |
| pMsg          | string | 消息内容                                     | ""          |
| iTargetType   | int    | 目标类型。0表示发送目标为pTargetUserId；1表示发送目标为除pTargetUserId以外的房间其他人 | 0           |
| pTargetUserId | int[]  | 目标列表                                     | [1001,1002] |



### 返回值

| 参数     | 类型   | 描述   | 示例值  |
| ------ | ---- | ---- | ---- |
| status | int  | 返回值  | 200  |



### 说明

发送消息，可以指定发送对象，比如只给玩家A发送消息。  



## joinRoomNotify



### 返回值

| 参数          | 类型     | 描述   | 示例值  |
| ----------- | ------ | ---- | ---- |
| userID      | int    | 用户ID | 321  |
| userProfile | string | 用户简介 | ""   |



### 说明

加入房间广播通知，当有其他玩家加入房间时客户端会收到该通知。



## leaveRoomNotify



### 返回值

| 参数     | 类型   | 描述   | 示例值  |
| ------ | ---- | ---- | ---- |
| userID | int  | 用户ID | 321  |



### 说明

离开房间广播通知，当房间内其他玩家离开房间时客户端会收到该通知。  



## sendEventNotify



### 返回值

| 参数       | 类型     | 描述      | 示例值  |
| -------- | ------ | ------- | ---- |
| srcUid   | int    | 推送方用户ID | 321  |
| priority | int    | 优先级     | 1    |
| cpProto  | string | 负载均衡    | “”   |



### 说明

数据发送通知，当房间内其他玩家发送数据时，客户端会接收到该通知。

## hashSet

**接口地址**
`http://alphavsopen.matchvs.com/wc5/hashSet.do`

**注意 ：**

MatchVS 环境分为测试环境（alpha）和 正式环境（release），所以在使用http接口时，需要通过域名进行区分。使用正式环境需要先在官网将您的游戏发布上线。

**alpha环境域名：alphavsopen.matchvs.com**

**release环境域名：releasevsopen.matchvs.com**



**说明**

服务器存储设置 

**参数**

| 参数名    | 说明        |
| ------ | --------- |
| gameID | 游戏ID      |
| userID | 用户ID      |
| key    | 自定义存储字段编号 |
| value  | 自定义存储字段的值 |
| sign   |           |

**返回值**

- 部分参数说明

| Key    | 含义                          |
| ------ | --------------------------- |
| data   | data=success表示存储数据成功，其他代表异常 |
| status | status=0 表示成功，其他代表异常        |

## hashGet

**接口地址**
`http://alphavsopen.matchvs.com/wc5/hashGet.do`



**注意 ：**

MatchVS 环境分为测试环境（alpha）和 正式环境（release），所以在使用http接口时，需要通过域名进行区分。使用正式环境需要先在官网将您的游戏发布上线。

**alpha环境域名：alphavsopen.matchvs.com**

**release环境域名：releasevsopen.matchvs.com**



**说明**

服务器存储查询

**参数**

| 参数名    | 说明        |
| ------ | --------- |
| gameID | 游戏ID      |
| userid | 用户ID      |
| key    | 自定义存储字段键值 |
| sign   |           |

**返回值**

- 部分参数说明

| Key    | 含义                   |
| ------ | -------------------- |
| data   | 查询的数据结果              |
| status | status=0 表示成功，其他代表异常 |



## sign值获取方法

##### 1. 按照如下格式拼接出字符串:

```
appKey&param1=value1&param2=value2&param3=value3&token
```

- `appKey`为您在官网配置游戏所得

- `param1、param2、param3`等所有参数，按照数字`0-9`、英文字母`a~z`的顺序排列

  例 ： 有三个参数`gameID`、`userID`、`key`，则按照`appkey&gameID=xxx&key=xxx&userID=xxx&token` 的顺序拼出字符串。

- `token`通过用户注册请求获取

##### 2. 计算第一步拼接好的字符串的`MD5`值，即为`sign`的值。



## 通用错误码

| status | 含义        |
| ------ | --------- |
| 200    | 成功        |
| 500    | 网络错误      |
| 501    | gateway错误 |
| 502    | 房间管理错误    |

