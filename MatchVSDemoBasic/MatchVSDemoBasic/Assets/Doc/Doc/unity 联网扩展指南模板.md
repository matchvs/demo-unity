## 属性匹配

MatchVS 提供了属性匹配功能，开发者可以利用该功能实现各种自定义的规则匹配。

属性匹配机制：

您可以将需要使用的匹配参数例如 “等级：5-10 ”   “地图 ： A”  以 key-value 的方式填至 `matchInfo`，MatchVS 将会严格对比各个玩家携带的 `matchInfo`，然后将`matchInfo`完全相同的玩家匹配到一起。  

如果玩家当前匹配不到合适的对象，MatchVS 会创建一个房间给该玩家。如果您想扩大范围再次进行匹配，可以退出当前房间，修改matchInfo，然后发起匹配。

如果您希望将玩家的个人信息（比如：昵称、等级）广播给成功匹配到的房间成员，您可以将这些信息填至`userProfile` 。MatchVS会在每一个成员加入房间时将成员信息广播给当前房间成员，同时将已有成员的信息通知给新加入成员。

```
public int joinRoomWithProperties(MsMatchInfo matchInfo,string userProfile)
```

参数说明：

| 参数             | 类型     | 描述       | 示例值  |
| -------------- | ------ | -------- | ---- |
| maxPlayer     | int    | 最大人数 | 3   |
| mode | int | 模式     | 0   |
| canWatch | int | 可否观看     | 0   |
| tags | list | 属性标签     | 0   |
| key | string | 标签key     | “key”   |
| value | string | 标签value     | “A”   |


示例代码如下:

```
   MsMatchInfoTag tag = new MsMatchInfoTag() { key = "key", value = "B" };
   MsMatchInfo info = new MsMatchInfo(3, 0, 0, tag);
   engine.MatchAttributeRoom(info, "matchvs");
```


加入房间的回调：

```
int joinRoomResponse(MsJoinRandomRsp tRsp)
{
	//返回值
	int status = tRsp.status;
	//用户ID
	int userid = tRsp.userid;
	//用户信息
	string userProfile = tRsp.userProfile;
	//房间ID
	string roomID = tRsp.roomID;
	//房间属性
	string roomProperty = tRsp.roomProperty;
	//负载数据
	string cpProto = tRsp.cpProto;
}
```

参数说明：

| 参数             | 类型     | 描述       | 示例值  |
| -------------- | ------ | -------- | ---- |
| status  | int | 返回值 | 200|
| userid  | int | 用户ID | 123546|
| userProfile  | string | 用户信息 | ""|
| roomID  | string | 房间ID | "154124156745"|
| roomProperty  | string | 房间属性 | ""|
| cpProto | string | 负载数据 | "" |

其他玩家加入房间的回调：

```
int joinRoomNotify(MsRoomPeerJoinRsp tRsp)
{
	//用户ID
	int userID = tRsp.userID;
	//用户信息
	string userProfile = tRsp.userProfile;
}
```

参数说明：
| 参数             | 类型     | 描述       | 示例值  |
| -------------- | ------ | -------- | ---- |
| userID  | int | 用户ID | 154321456|
| userProfile  | string | 用户信息 | ""|




## 创建房间

MatchVS 提供了创建房间的功能，开发者可以从客户端主动创建一个房间。

玩家创建房间后，MatchVS 会将该玩家自动加入此房间，该玩家即是房主。如果房主离开了房间，MatchVS 会指定下一个房主并通知给房间所有成员。

在创建房间的时候可以指定该房间的属性，比如房间地图、房间人数等。

**注意：**  玩家主动创建的房间和MatchVS 创建的房间是分开的。玩家通过随机匹配或者属性匹配是无法匹配到玩家主动创建的房间里的。

```
public int createRoom(MsCreateRoomInfo roomInfo, string userProfile)
```

参数说明：

| 参数             | 类型     | 描述       | 示例值  |
| -------------- | ------ | -------- | ---- |
| roomInfo     | MsCreateRoomInfo    | 房间信息 |    |
| name | string | 房间名称     | "matchvs"   | 
| maxplayer | int | 最大人数     | 3  | 
| mode | int | 模式     | 0  |
| canWatch | int | 可否观战     | 0  |
| visibility | int | 可否可见     | 0  |
| roomProperty | string | 房间属性     | ""  |

示例代码如下:

```
   MsCreateRoomInfo info = new MsCreateRoomInfo("MatchVS", 3, 0, 0, 0, "matchvs");
   engine.CreateRoom(info, "matchvs");
```


创建房间的回调：

```
int createRoomResponse(MsCreateRoomRsp tRsp)
{
	//返回值
	int status = tRsp.status;
	//房间号
	string roomID = tRsp.roomID;
}
```

参数说明:

| 参数           | 类型     | 描述       | 示例值    |
| ------------ | ------ | -------- | ------ |
| status       | int    | 返回值      | 200    |
| roomID       | string | 房间号      | "15452154312454"       |


## 获取房间列表

MatchVS 提供了获取房间列表的功能，该列表为玩家主动创建的房间列表。

您可以通过房间属性过滤获取房间列表。比如您只想获取地图为A的所有房间列表，您可以将地图A作为过滤条件来获取列表。

```
public int getRoomList(MsRoomFilter roomFilter)
```

参数说明：

| 参数      | 类型     | 描述   | 示例值  |
| ------- | ------ | ---- | ---- |
| maxPlayer | int | 最大人数 | 3   |
| mode | int | 模式 | 0   |
| canWatch | int | 可否观看 | 0   |
| roomProperty | string | 房间属性 | 0   |

示例代码如下:

```
        MsRoomFilter filter = new MsRoomFilter(3, 0, 0, "");
        engine.GetRoomList(filter);;
```


获取房间列表的回调：

```
int getRoomListResponse(MsRoomListRsp tRsp)
{
	//返回值
	int status = tRsp.status;
	//房间号
	string roomID = tRsp.roomID;
	//房间名称
	string roomName = tRsp.roomName;
	//最大人数
	int maxPlayer = tRsp.maxPlayer;
	//模式
	int mode = tRsp.mode;
	//可否观战
	int canWatch = tRsp.canWatch;
	//房间属性
	string roomProperty = tRsp.roomProperty;
}
```
参数说明：

| 参数      | 类型     | 描述   | 示例值  |
| ------- | ------ | ---- | ---- |
| status  | int    | 返回值  | 200  |
| roomID | string | 房间ID |  "156231454561"    |
| roomName | string | 房间名称 |  "matchvs"    |
| maxPlayer | int | 最大人数 |  3    |
| mode | int | 模式 |  0    |
| canWatch | int | 可否观战 |  0    |
| roomProperty | string | 房间属性 |  ""    |


## 加入指定房间

MatchVS 提供了加入指定房间的功能，在获取到房间ID后即可以通过此接口加入一个指定房间。

如果玩家希望和好友一起游戏，则可以创建一个房间后，将该房间ID给到好友，好友通过该ID进入房间。

```
public int joinRoom(string roomID,string userProfile)
```

参数说明：

| 参数             | 类型     | 描述       | 示例值  |
| -------------- | ------ | -------- | ---- |
| roomID     | string    | 房间ID | "152451234542"    |
| userProfile | string | 玩家简介     | ""   |

示例代码如下:

```
        engine.joinRoom("12345785", "");
```


加入指定房间的回调：

```
 int joinRoomResponse(MsJoinRandomRsp tRsp)
{
	//返回值
	int status = tRsp.status;
	//房间已有用户列表
	MsUserInfoList userInfoList = tRsp.userInfoList;
	//房间信息
	MsRoomInfo roomInfo = tRsp.roomInfo;
	//负载数据
	string cpProto = tRsp.cpProto;
}
```
参数说明:

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


## 踢除房间成员

MatchVS 提供了踢除房间成员的功能，如果游戏中有成员恶意不准备等情况，则可以使用该功能。

如果房间是 MatchVS 服务端创建的，房间里任意成员均有踢人权限；如果房间是玩家主动创建的，则只有房主拥有踢人权限。



```
public int kickPlayer(int userID,string cpProto)
```

参数说明：

| 参数      | 类型     | 描述   | 示例值  |
| ------- | ------ | ---- | ---- |
| userID | int | 玩家ID | 553584   |
| cpProto | string | 消息 | ""   |

示例代码如下:

```
         engine.kickPlayer(12345485,"");
```


加入指定房间的回调：

```
int kickPlayerResponse(int status)
{
	//返回值
	int status = status;
}
```



## 消息订阅分组

在游戏中由于玩家视野各不相同、玩家队伍不同，游戏数据不必每次都广播给房间内所有成员。MatchVS 提供了消息订阅分组的机制，您可以将不同类型的消息进行分组，然后动态地让玩家订阅或者取消订阅消息。这样可以很方便地实现消息管理同时节省流量。

当玩家订阅某个消息组后，所有该消息组的内容他均可以收到；如果取消订阅，则不会再收到该组消息。

**注意：**如果玩家主动离开房间，则之前订阅的消息会自动被取消。如果玩家再次进入同样的房间，需要再重新订阅该房间消息。

如下图，游戏地图超级大， 玩家（小黄点）动态分布在对应区域的小地图里。这种情况下，玩家只需要知道当前区域的消息。又由于区域的玩家是动态变化的，所以发送消息给指定玩家会比较麻烦。

故提供消息分组订阅，玩家可以在进入区域时订阅该区域的消息，离开区域时取消订阅该区域信息。下图右上角小地图里有绿色的小塔，当塔发生变化时，塔的消息发给该区域玩家即可。



![](http://imgs.matchvs.com/static/3-1.png)



```
public int subscribeEventGroup(string[] subGroups, string[] unsubGroups)
```

参数说明：

| 参数      | 类型     | 描述   | 示例值  |
| ------- | ------ | ---- | ---- |
| subGroups | string[] | 订阅分组 | ""   |
| unsubGroups | string[] | 取消订阅分组 | 

示例代码如下:

```
//订阅
         engine.subscribeEventGroup(new []{"specialArea"},new string[]{});
//取消订阅
engine.subscribeEventGroup(new []{},new string[]{"specialArea"});
```


订阅的回调：

```
int subscribeEventGroupRsp(MsSubscribeEventGroupRsp tRsp)
{
	//返回值
	int status = tRsp.status;
	//订阅分组
	string[] subGroups = tRsp.subGroups;
}
```

参数说明:

| 参数      | 类型     | 描述   | 示例值  |
| ------- | ------ | ---- | ---- |
| status  | int    | 返回值  | 200  |
| subGroups | string[] | 订阅分组 |      |


```
int sendEventGroup(int priority, string cpProto, string[] groups)
```

参数说明：


| 参数      | 类型     | 描述   | 示例值  |
| ------- | ------ | ---- | ---- |
| priority | int | 权重 | 3   |
| cpProto | string | 发送消息 | "" |
| groups  | string[] | 订阅分组 | {"matchvs"}|  

示例代码如下:

```
   		 JsonData data = new JsonData();
        data["action"] = "AccelerateDown";
        string value = data.ToJson();
        engine.SendEventGroup(3,value, new [] { "specialArea" });
```

发送的回调：

```
int sendEventGroupRsp(MsSendEventGroupRsp tRsp)
{
	//返回值
	int status = tRsp.status;
	//目标人数
	int dstNum = tRsp.dstNum;
}
```

参数说明:


| 参数      | 类型     | 描述   | 示例值  |
| ------- | ------ | ---- | ---- |
| status  | int    | 返回值  | 200  |
|  dstNum  |  int  |    目标人数     |   2  |   


发送消息的广播通知:

```
int sendEventGroupNotify(MsSendEventGroupNotify tRsp)
{
	// 发送用户ID
	int srcUserID = tRsp.srcUserID;
	// 权重
	int priority = tRsp.priority;
	//订阅组
	string[] groups = tRsp.groups;
	//负载消息
	string cpProto = tRsp.cpProto;
}
```

参数说明：

| 参数      | 类型     | 描述   | 示例值  |
| ------- | ------ | ---- | ---- |
| srcUserID  | int    | 发送用户ID  | 1532156  |
|  priority  |  int  |    权重     |   3  |   
|  groups  |  string[]  |    订阅组     |     |  
|  cpProto  |  string  |    负载消息     |     |    