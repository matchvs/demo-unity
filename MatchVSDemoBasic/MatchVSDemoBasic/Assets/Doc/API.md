## init

```
 public int init(MatchVSResponse[] pMatchVSResponse,string channel,string platform,int gameid) {
```

### 说明

SDK初始化

### 参数

| 参数   | 类型   | 描述   | 示例值  |
| ---- | ---- | ---- | ---- |
|  pMatchVSResponse |  MatchVSResponse   |   注册回调函数   |
|  channel |  string   |   渠道   |    "MatchVS"  |
|  platform |  string   |   平台   |    "Alpha"  |
|  gameid |  int   |   游戏ID   |    2001003  |


## uninit

```
  public int uninit()
```

### 说明

SDK反初始化工作

## registerUser

```
public int registerUser()
```

### 说明

注册用户信息





### 返回值

| 参数   | 类型   | 描述   | 示例值  |
| ---- | ---- | ---- | ---- |
|  userID    |   int   |   用户ID   | 123456  |
|  token    |   String   |   用户Token   | ""  |

## login

```
public int login(int userid, string token, int gameid, int gameVersion, string appkey, string secretkey,
		    string deviceid, int gatewayid)
```

### 说明

登录MatchVS服务端，与MatchVS建立连接。

### 参数

| 参数   | 类型   |  描述   | 示例值  |
| ---- | ---- |  ---- | ---- |
| userid  |int   |用户ID  |  123546    |
| token  |String   |用户token  |  ""    |
| gameid  |int   |游戏ID |  210329    |
| gameVersion  |int   |游戏版本号 |  1    |
| appkey  |string   |appkey|  ""    |
| secretkey  |string   |secretkey|  ""    |
| deviceid  |string   |设备ID|  ""    |
| gatewayid  |int   |gateWayID 登录默认gateway时为0|0    |

### 返回值

| 参数   | 类型   | 描述   | 示例值  |
| ---- | ---- | ---- | ---- |
|   status   |    int  |   返回值   |200|      
|   roomID   |    int  |   房间号   |210039|      


## logout

```
public int logout()
```

### 说明

退出登录


### 返回值

| 参数   | 类型   | 描述   | 示例值  |
| ---- | ---- | ---- | ---- |
|   status   |    int  |   返回值   |200|      


## joinRandom

```
 public int joinRandomRoom(int iMaxPlayer, string strUserProfile)
```

### 说明

随机加入房间

### 参数

| 参数   | 类型   |  描述   | 示例值  |
| ---- | ---- |  ---- | ---- |
| iMaxPlayer  |int   |最大玩家数  |  3    |
| strUserProfile  |String   |玩家简介  |  ""    |


### 返回值

| 参数   | 类型   | 描述   | 示例值  |
| ---- | ---- | ---- | ---- |
|   status   |    int  |   返回值   |200|      
|   userInfoList   |      |   房间用户列表   | | 
|   userId   |    int |   用户ID   | 321 | 
|   userProfile   |  string    |   用户简介   | "" | 
|   roomInfo   |      |   房间信息   |  | 
|   roomID   |    int  |   房间号  | 210096 | 
|   roomProperty   |    String  |   房间属性   | "" | 
|   cpProto   |    String  |   负载数据   | "" | 


## joinOver

```
public int joinOver(string cpProto)
```

### 说明

结束房间匹配

### 参数

| 参数   | 类型   |  描述   | 示例值  |
| ---- | ---- |  ---- | ---- |
| cpProto  |String   |负载信息  |  ""    |


### 返回值

| 参数   | 类型   | 描述   | 示例值  |
| ---- | ---- | ---- | ---- |
|   status   |    int  |   返回值   |200|      
|   cpProto   |   String   |   负载信息   | | 


## leaveRoom

```
public int leaveRoom(String payload)
```

### 说明

退出房间

### 参数

| 参数   | 类型   |  描述   | 示例值  |
| ---- | ---- |  ---- | ---- |
| cpProto  |String   |负载信息  |  ""    |


### 返回值

| 参数   | 类型   | 描述   | 示例值  |
| ---- | ---- | ---- | ---- |
|   status   |    int  |   返回值   |200|      
|   roomID   |    int  |   房间号   |210039|   
|   userID   |    int  |   用户ID   |321|   
|   cpProto   |   String   |   负载信息   | "" | 


## sendEvent

```
public int sendEvent(int iPriority, int iType,string pMsg,int iTargetType,int[] pTargetUserId)
```

### 说明

发送消息

### 参数

| 参数   | 类型   |  描述   | 示例值  |
| ---- | ---- |  ---- | ---- |
| iPriority  |int   |消息优先级，0~3，值越小越优先处理  | 0    |
| iType  |int   |消息类型。0表示转发给其他玩家；1表示转发给game server；2表示转发给其他玩家及game server  |  0    |
| pMsg  |String   |消息内容  |  ""    |
| iTargetType  |int   |目标类型。0表示发送目标为pTargetUserId；1表示发送目标为除pTargetUserId以外的房间其他人  |  0    |
| pTargetUserId  |int[]   |目标列表  |  [1001,1002]    |


### 返回值

| 参数   | 类型   | 描述   | 示例值  |
| ---- | ---- | ---- | ---- |
|   status   |    int  |   返回值   |200|

## joinRoomNotify

### 说明

加入房间广播通知

### 返回值

| 参数   | 类型   | 描述   | 示例值  |
| ---- | ---- | ---- | ---- |
|   userID   |    int  |   用户ID   |321|   
|   userProfile   |    String  |   用户简介   |""|        

## leaveRoomNotify


### 说明

离开房间广播通知

### 返回值

| 参数   | 类型   | 描述   | 示例值  |
| ---- | ---- | ---- | ---- |
|   userID   |    int  |   用户ID   |321|   

## sendEventNotify


### 说明

离开房间广播通知

### 返回值

| 参数   | 类型   | 描述   | 示例值  |
| ---- | ---- | ---- | ---- |
|   srcUid   |    int  |   推送方用户ID   |321|       
|   priority   |    int  |   优先级   |1|    
|   cpProto   |    String  |   负载均衡   |“”|             