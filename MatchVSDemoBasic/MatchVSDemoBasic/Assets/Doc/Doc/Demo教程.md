## Demo简介

为了便于开发者使用和理解MatchVS的实时联网SDK，MatchVS提供了简洁的Demo来展示多人实时游戏的开发过程和效果。  

Demo支持两人或三人同时游戏，匹配成功后，玩家通过向左、向右、加速按钮可以操纵图标移动，先到达终点者胜利。    

下载Demo源码后，使用Unity 5+打开工程。

## 游戏配置
Demo运行之前需要去[官网](http://www.matchvs.com )配置游戏相关信息，以获取Demo运行所需要的GameID、AppKey、SecretID。如图：

![](http://imgs.matchvs.com/static/2_1.png)

![](http://imgs.matchvs.com/static/2_2.png)

获取到相关游戏信息之后，运行Demo，填写GameID,AppKey,SecretID,点击登录。
如图所示：
![](http://imgs.matchvs.com/static/2_3.jpg)
## 初始化SDK 

MatchVS SDK提供了两个很重要的文件，`MatchVSEngine`和`MatchVSResponse`。想要获取游戏中玩家加入、离开房间，数据收发的信息，需要先实现`MatchVSResponse`中的抽象方法。  

我们现在新建子类MatchVSResponseInner继承自抽象类MatchVSResponse，如下：

```
// 以下是 MatchVSResponseInner.cs 文件的代码片段
public class MatchVSResponseInner : MatchVSResponse
{
	//实现所有父类的抽象方法
}
```

文件路径:`Assets\Script\MainUI\MatchVSResponseInner.cs  `



完成以上步骤后，我们可以调用初始化接口建立相关资源。

```
engine.init(matchVSResponses, channel, platform, gameid);
```

初始化操作，在GameManager.cs文件中完成。

文件路径:`Assets\Script\MainUI\GameManager.cs  ` 



**注意：**在整个应用全局，你需要且只需要对引擎做一次初始化。





## 建立连接
接下来，我们就可以从MatchVS获取一个合法的用户ID，通过该ID连接至MatchVS服务端。  

获取用户ID：

```
engine.registerUser();
```

登录：

```
engine.login(userID,token,gameid,gameVersion,appkey,
    secret,deviceID,gatewayid);
```

文件路径 ：`Assets\Script\MainUI\GameManager.cs ` 



## 加入房间

成功连接至MatchVS后，即可点击随机匹配加入一个房间进行游戏。  
代码如下:

```
engine.joinRandomRoom(int iMaxPlayer, string strUserProfile);
```

匹配界面配图：

![](http://imgs.matchvs.com/static/2_6.png)

文件路径:`Assets\Script\MainUI\GameManager.cs`  



## 停止加入

如果游戏人数已经满足开始条件，此时告诉MatchVS不要再向房间里加人。  

代码如下:
```
engine.joinOver(roomID,proto);
```
我们设定如果3秒内有3个真人玩家匹配成功则调用JoinOver并立即开始游戏；  

如果3秒内只有2个真人玩家，则再添加一个机器人（机器人头像默认为一个即可，昵称为：机器人1），调用JoinOver并开始游戏；  

如果3秒内只有1个真人玩家，则添加两个机器人（机器人头像默认即可，昵称为：机器人1，机器人2），调用JoinOver并开始游戏。

```
	// 匹配时间判断
	if (matchTime >= 3)
	{
		// 房主身份判定
		if (!GameManager.Instance.RoomOwner)
			return;
		// 当前房间人数判定
		if (currentCount < 3)
		{
			// 确定剩余人数
			int cout = 3 - currentCount;
			int userid;
			for (int i = 0; i < cout; i++)
			{   
                // 随机创建用户ID
				userid = Random.Range(10000, 100000000);
				GameManager.Instance.AddRoomPlayer(new UserInfo(userid,true));

                items[currentCount].UpdateInfo(userid);

                // 创建负载消息（cpProto），
				JsonData data = new JsonData();
				data["action"] = "robot";
				data["userid"] = userid;
				string value = data.ToJson();
                // 向其他玩家发送机器人消息
				GameManager.SendEvent(value);

				currentCount++;
			}
	
			StartCoroutine(ShowGameRoom());
			// 调用JoinOver结束房间匹配
			GameManager.JoinOver(roomInfo.roomID, roomInfo.roomProperty);
		}
	}
	else
	{
         // 时间统计
		 matchTime += Time.deltaTime;
    }
```

文件路径：`Assets\Script\MainUI\MatchingBoard.cs`  



## 游戏数据传输

当一个玩家进行向左、向右、加速操作时，我们将这些操作广播给房间其他玩家。界面上同步展示各个玩家的状态变化。  

代码如下：

```
engine.sendEvent(int iPriority, int iType,string pMsg,int iTargetType,int[] pTargetUserId);
```



![](http://imgs.matchvs.com/static/2_7.png)


数据传输回调 ：

```
int sendEventResponse(int status)
```



收到其他人发的数据：

```
int sendEventNotify(MsMsgNotify tRsp)
{
	tRsp.srcUid;
	tRsp.priority;
	tRsp.cpProto;
}
```

  

向左、向右、加速操作代码如下:

```
	//加速操作
public void AccelerateDown()
{
		//修改移动速度
        target.GetComponent<CharacterMove>().moveSpeed += 0.2f;
        GetPlayerInfoByID(GameManager.userID).UpdateSpeed(80);

		//发送当前状态
        JsonData data = new JsonData();
        data["action"] = "AccelerateDown";
        string value = data.ToJson();
        GameManager.SendEvent(value);
		
		//修改状态消息
        Message("加速");
}

//向右
public void RightDown()
{
		//修改移动速度
        target.GetComponent<CharacterMove>().rightMoveSpeed += 0.2f;

		//发送当前状态
        JsonData data = new JsonData();
        data["action"] = "RightDown";
        string value = data.ToJson();
        GameManager.SendEvent(value);

		//修改状态消息
        Message("开始向右");
}

//向左
public void LeftDown()
{
	// 修改移动速度
    target.GetComponent<CharacterMove>().leftMoveSpeed += 0.2f;
	
	// 发送当前状态
    JsonData data = new JsonData();
    data["action"] = "LeftDown";
    string value = data.ToJson();
    GameManager.SendEvent(value);
		
	// 修改状态消息
     Message("开始向左");
}
	
	
```
在执行相关操作的同时，向其他终端发送相关指令并在控制台输出相关状态变化日志。

文件路径：`Assets\Script\MainUI\GameRoomBoard.cs ` 



## 离开房间

抵达终点后游戏结束，游戏结束后离开房间。  



![](http://imgs.matchvs.com/static/2_8.png)


中途离开房间，

```
 engine.leaveRoom(string payload);
```

游戏进行中离开房间可能遇到两种情况：  

1.匹配过程中离开房间   

2.游戏过程中离开房间。  

a.匹配过程中离开房间，分为房主离开房间和成员离开房间。

1.成员离开房间：

```
public void LeaveRoom()
{
	GameManager.LeaveRoom();
}
```
文件路径：`Assets\Script\MainUI\MatchingBoard.cs ` 

其他成员消息处理：

```
private void LeavePeerNofity(MsRoomPeerLeaveRsp tRsp)
{
	int userID = tRsp.userID;
	for (int i = 0; i < items.Length; i++)
	{
		items[i].RemoveNotify(userID);
	}
	....
}
```
文件路径：`Assets\Script\MainUI\MatchingBoard.cs`  

做相应的界面更新即可。  



2.房主离开房间：

```
public void LeaveRoom()
{
	GameManager.LeaveRoom();
}
```
文件路径：`Assets\Script\MainUI\MatchingBoard.cs`  



其他成员消息处理:

```
private void LeavePeerNofity(MsRoomPeerLeaveRsp tRsp)
{
....

	if (!GameManager.Instance.RoomOwner)
	{
		GameManager.Instance.RoomOwner = true;
		for (int i = 0; i < items.Length; i++) {
		items[i].RemoveNotify(GameManager.userID);
		}
		items[0].UpdateInfo(GameManager.userID);
	}
	
	GameManager.Instance.RemovePlayer(userID);
}
```
此时需要确立新房主，并做相应的界面更新。
文件路径：`Assets\Script\MainUI\MatchingBoard.cs ` 



b.游戏过程中离开房间,分为成员离开房间和房主离开房间


1.成员离开房间:

```
public void OnBack()
{
	....		
	GameManager.LeaveRoom();
}
```
文件路径：`Assets\Script\MainUI\GameRoomBoard.cs`  

房间其他成员：

```
private void LeavePeerNofity(MsRoomPeerLeaveRsp tRsp)
{
    currentCount--;
}
```
文件路径：`Assets\Script\MainUI\GameRoomBoard.cs`  



2.房主离开房间:
```
public void OnBack()
{
	if (GameManager.Instance.RoomOwner) {
		int roomOwner = 0;
		for (int i = 0; i < GameManager.Instance.RoomPlayers.Count; i++) {
			if (GameManager.Instance.RoomPlayers[i].userid != GameManager.userID &&
			!GameManager.Instance.RoomPlayers[i].robot) {
				roomOwner = GameManager.Instance.RoomPlayers[i].userid;
				break;
			}
		}
	JsonData data = new JsonData();
	data["action"] = "roomLeaveRoom";
	data["RoomOwner"] = roomOwner;
	string value = data.ToJson();
	GameManager.SendEvent(value);
	}
		
	....
}
```
文件路径：`Assets\Script\MainUI\GameRoomBoard.cs`


房主离开房间，此时需要指定房间内其他成员为房主，并发送相关消息，通知其他成员，此时的房主ID。成为房主后，就要完成房主需要完成的相关事宜。  

房间其他成员:

```
private void LeavePeerNofity(MsRoomPeerLeaveRsp tRsp)
{
	currentCount--;
}
	
....
if (action.Equals("roomLeaveRoom"))
{
	int gameowner = (int) data["RoomOwner"];
	if (GameManager.userID == gameowner)
	{
		GameManager.Instance.RoomOwner = true;
	}
}
....
	
```
文件路径：`Assets\Script\MainUI\GameRoomBoard.cs`
房间人数调整，另外确定新房主。

## 创建房间
玩家可以自己创建一个房间，点击Demo首页“创建房间”按钮即可提交创建，创建成功会返回当前房号。如果有其他玩家加入房间，房间内成员列表会更新。同时，房主拥有踢人权限。

如果房主中途离开房间，系统会自动指定下一个房主。

![](../3_1_1.png)

创建房间代码：

```
engine.createRoom(info, userProfile);
```

创建房间回调：

```
 int createRoomResponse(MsCreateRoomRsp tRsp)
```

踢人代码:

```
 engine.kickPlayer(userid,cpProto);
```

踢人回调:

```
int kickPlayerResponse(int status)
```

房间其他成员获取通知:

```
int kickPlayerNotify(MsKickPlayerNotify tRsp)
```

文件路径：`Assets\Script\MainUI\CreateRoomBoard.cs`

创建房间回调代码:

```
		GameManager.Instance.RoomOwner = true;
		roomID.text = tRsp.roomID;
		roomPlayer[currentCount].UpdateInfo(GameManager.userID);

		....

		GameManager.Instance.AddRoomPlayer(new UserInfo(GameManager.userID, false));
```

创建房间的玩家确立为房主，同时刷新界面数据。
文件路径: `Assets\Script\MainUI\GameRoomBoard.cs`

房主离开房间:

```
		....

		for (int i = 0; i < GameManager.Instance.RoomPlayers.Count; i++) {
			if (i == 0) {
				if (GameManager.userID == GameManager.Instance.RoomPlayers[i].userid) {
					GameManager.Instance.RoomOwner = true;
				}
			}
			roomPlayer[i].UpdateInfo(GameManager.Instance.RoomPlayers[i].userid);
		}

		....

```

当房主离开房间，房主的权利需要顺位。
文件路径: `Assets\Script\MainUI\GameRoomBoard.cs`

房间成员离开房间:

```
		....		

		for (int i = 0; i < GameManager.Instance.RoomPlayers.Count; i++) {
			
			....			

			roomPlayer[i].UpdateInfo(GameManager.Instance.RoomPlayers[i].userid);
		}
	
		....
```

当有成员离开房间，刷新房间数据。
文件路径: `Assets\Script\MainUI\GameRoomBoard.cs`


## 加入指定房间
可以通过输入房间号来加入用户自定义房间。

![](../3_1_2.png)

加入指定房间：

```
 engine.joinRoom(roomID, profile);
```

加入指定房间代码如下:

```
		//获取输入框ID
   	    string roomID = this.roomID.text;
        GameManager.JoinSpecifiedRoom(roomID, "matchvs");
```


加入房间回调：

```
int joinRoomResponse(MsJoinRandomRsp tRsp)
```

文件路径：`Assets\Script\MainUI\GameRoomBoard.cs`

加入房间通知:


```
		....		

		for (int i = 0; i < GameManager.Instance.RoomPlayers.Count; i++) {
			
			....			

			roomPlayer[i].UpdateInfo(GameManager.Instance.RoomPlayers[i].userid);
		}
	
		....
```
刷新房间数据。

文件路径：`Assets\Script\MainUI\GameRoomBoard.cs`


## 自定义属性匹配
可以自定义匹配方案，匹配相同属性的玩家。

![](../3_1_3.png)

```
engine.joinRoomWithProperties(info, userProfile);
```

加入房间回调：

```
int joinRoomResponse(MsJoinRandomRsp tRsp)
```

文件路径：`Assets\Script\MainUI\GameRoomBoard.cs`


## 消息订阅分组

开始游戏后，游戏区域会有个黄色区域，黄色区域内的成员可以互相通信，圆圈内和圆圈外的成员不可以互相通信。

当成员进入黄色区域时即订阅消息，走出圆圈则取消订阅该范围消息。

![](../3_1_5.png)

订阅与取消订阅:

```
int subscribeEventGroup(string[] subGroups, string[] unsubGroups)
```

订阅与取消订阅代码如下：

```
	//订阅
 	GameManager.SubscribteEventGroup(new []{"specialArea"},new string[]{});
	//取消订阅
     GameManager.SubscribteEventGroup(new string[] {}, new string[] { "specialArea" });
```

订阅与取消订阅回调:

```
int subscribeEventGroupRsp(MsSubscribeEventGroupRsp tRsp)
```

发送消息到消息订阅组:

```
int sendEventGroup(int priority, string cpProto, string[] groups)
```

发送订阅消息代码如下

```
		//创建负载数据
 		JsonData data = new JsonData();
        data["action"] = "RightDown";
        string value = data.ToJson();

        GameManager.SendEventGroup(3, value, new[] { "specialArea" });
```

订阅消息推送:

```
int sendEventGroupNotify(MsSendEventGroupNotify tRsp)
```

文件路径：`Assets\Script\MainUI\GameRoomBoard.cs`

当进入特殊区域，订阅消息:

```
	....
		//订阅消息
		 GameManager.SubscribteEventGroup(new []{"specialArea"},new string[]{});
	....
```

文件路径：`Assets\Script\MainUI\GameRoomBoard.cs`

在订阅消息回调函数中，发送自己状态变化的请求，代码如下:

```
	....
		JsonData data = new JsonData();
	    data["action"] = "enter";
	    data["id"] = GameManager.userID;
	    string value = JsonUtil.toJson(data);
	    GameManager.SendEventGroup(3,value,new string[] { "specialArea" });
	....
```

文件路径：`Assets\Script\MainUI\GameRoomBoard.cs`

消息订阅消息通知,同时发送自己当前状态:

```
	....
		int id = (int) data["id"];
        GameObject target = GetPlayerInfoByID(id).Target;

		//调整相应车子的状态
        target.GetComponent<CharacterMove>().OnEnter();
		
		//发送自己当前状态
        data = new JsonData();
        data["action"] = "enterResponse";
        data["id"] = GameManager.userID;
        string value = JsonUtil.toJson(data);
        GameManager.SendEventGroup(3, value, new string[] {"specialArea"});
	....

```

文件路径：`Assets\Script\MainUI\GameRoomBoard.cs`

返回消息获取:

```
	....
	    int id = (int) data["id"];
        GameObject target = GetPlayerInfoByID(id).Target;
		//调整相应车子的状态
        target.GetComponent<CharacterMove>().OnEnter();
    ....
```

文件路径：`Assets\Script\MainUI\GameRoomBoard.cs`

## 展示房间列表
点击Demo首页“查看房间列表”按钮即可查看当前所有玩家主动创建的房间列表。点击房间列表里的某个房间即可加入该房间。

![](../3_1_4.png)

获取房间列表:

```
engine.getRoomList(filter);
```

代码如下：
```
        MsRoomFilter filter = new MsRoomFilter(3, 0, 0, "");
        GameManager.GetRoomList(filter);
```

消息回调:

```
int getRoomListResponse(MsRoomListRsp tRsp)
```

文件路径：`Assets\Script\MainUI\GameRoomBoard.cs`

回调函数数据调整:

```
	...
		 MsRoomInfoEx[] info = tRsp.roomInfos;
	    for (int i = 0; i < info.Length; i++)
	    {
	        GameObject go = Instantiate(prefab, parent, false);
	        RoomListItem item = go.GetComponent<RoomListItem>();
	        item.UpdateInfo(info[i]);
			list.Add(go);
	    }
	...
```
文件路径：`Assets\Script\MainUI\RoomListBoard.cs`

##  数据存取

将游戏里产生的金币和钻石存储至服务器。

```
 MatchVSHttp.HashSet(this,gameid,userID,key,value,appkey,token, (context,error) => { })
```

文件路径：`Assets\Script\MainUI\GameRoomBoard.cs`

将存储的数据取出来：
```
 MatchVSHttp.HashGet(this,gameid,userID,key,appkey,token,rsp)
```

文件路径：`Assets\Script\MainUI\GameRoomBoard.cs`

**注意 ：** 数据存取ID和token来进行存取，因此需要将ID和Token缓存在本地。
用户本地数据存储
```
 		PlayerPrefs.SetInt("userID",userID);
		PlayerPrefs.SetString("token",token);
```

文件路径: `Assets\Script\MainUI\MainMenuBoard.cs`

用户服务器数据存取：

```
		//保存数据
	 	JsonData data = new JsonData();
        data["username"] = username;
        data["diamod"] = diamond;

        HashSet("playerInfo",JsonUtil.toJson(data));
```

```	
		//获取数据
		HashGet("playerInfo", (context, error) =>
		{
			JsonData json = JsonUtil.toObject(context);
			String dataJson = (String)json["data"];
			JsonData data = JsonUtil.toObject(dataJson);
			UserName =(String)data["username"];
			Diamond = (int)data["diamod"];
		});
```

文件路径: `Assets\Script\MainUI\GameManager.cs`

##  游戏登出

退出游戏时，需要调用登出接口将与MatchVS的连接断开。

```
engine.logout();
```

文件路径：`Assets\Script\MainUI\GameManager.cs ` 



## 反初始化

在登出后，调用反初始化对资源进行回收。  

```
engine.uninit();
```

文件路径：`Assets\Script\MainUI\GameManager.cs ` 



## 错误处理  

```
int errorResponse(string error)
```

**注意 ：**MatchVS SDK相关的异常信息可通过该接口获取

文件路径：`Assets\Script\MainUI\GameManager.cs`