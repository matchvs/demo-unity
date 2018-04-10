## Demo简介

Matchvs 提供了gameServer模式的Demo，开发者可以前往[下载Demo](/service?page=unity 竞速 Demo)。

在gameServer模式下，开始游戏的时机，机器人逻辑，游戏过程中的随机资源以及结算过程均放在gameServer里进行处理。

打开Demo，点击首页“gameServer模式”，即可进入相应流程。

![](../Pic/3_2_1.png)


##加入房间
首先点击'GameServer'模式，随机加入房间：

```
  engine.joinRandomRoom(3, "Matchvs");
```
代码路径：Assets\Script\MainUI\GameManager.cs

Demo中，在GameServer模式下，仍确立房主机制。

##终止玩家加入房间
房间人数等于房间最大人数或到达匹配的最长时间，则终止玩家加入该房间。

房间人满：
```
	...
  	if (currentCount == 3)
    {
        GameManager.JoinOver(roomInfo.roomID,"matchvs");
    }
	...
```
代码路径:Assets\Script\MainUI\GsMatchingBoard.cs

达到匹配最长时间:

```
	...
  	if (GameManager.Instance.RoomOwner && !joinOver)
    {
        if (timer < 9.5f)
        {
            timer += Time.deltaTime;
        }
        else
        {
            GameManager.JoinOver(roomInfo.roomID, "matchvs");
            joinOver = true;
        }
    }
	...
```
代码路径:Assets\Script\MainUI\GsMatchingBoard.cs

在GameServer中的逻辑处理。

首先判定判断当前房间人数。
如果当前房间人数满足游戏开始
```
	...
	if (room.Count == room.MaxPlayerNum)
	{
		JObject data = new JObject();
		data["action"] = "gsReady";
		ByteString cpProto = JsonUtils.EncodetoByteString(data);

		PushToHotelMsg pushMsg = new PushToHotelMsg()
		{
			PushType = PushMsgType.UserTypeExclude,
			GameID = request.GameId,
			RoomID = request.RoomId,
			CpProto = cpProto,
		};

		PushToHotel(request.RoomId, pushMsg);
	}
	else
	{
		//创建机器人
		room.CreateRobot();
	}
	...
```

当GameServer收到客户端的JoinOver的通知之后，
若当前房间人数满足游戏开始条件，GameServer发送准备通知；
若当前房间人数不满足游戏开始条件，GameServer创建相应数量的机器人，同时发送准备通知。

满足游戏开始条件：
```
		...
		if (room.Count == room.MaxPlayerNum)
		{
			JObject data = new JObject();
			data["action"] = "gsReady";
			ByteString cpProto = JsonUtils.EncodetoByteString(data);

			PushToHotelMsg pushMsg = new PushToHotelMsg()
			{
				PushType = PushMsgType.UserTypeExclude,
				GameID = request.GameId,
				RoomID = request.RoomId,
				CpProto = cpProto,
			};

			PushToHotel(request.RoomId, pushMsg);
		}	
		else
		{
			//创建机器人
			...
		}
		...
```

不满足游戏开始条件:
```
	...
	for (uint i = 0;i< remainCount;i++)
    {
        int userid = random.Next(100000, 1000000);

        Player player = new Player((uint)userid);
        player.robot = true;
        
        players.Add((uint)userid, player);
        remainPlayer.Add(player);

        JObject data = new JObject();
        data["action"] = "gsRobot";
        data["userid"] = userid;
        ByteString cpProtoRobot =JsonUtils.EncodetoByteString(data);
        PushToHotelMsg pushMsgRobot = new PushToHotelMsg() {
            PushType = PushMsgType.UserTypeExclude,
            GameID = GameId,
            RoomID = RoomId,
            CpProto = cpProtoRobot,
        };

        cpsServer.PushToHotel(RoomId, pushMsgRobot);
    }
	//发送准备请求
	JObject readyData = new JObject();
	readyData["action"] = "gsReady";
	ByteString cpProto = JsonUtils.EncodetoByteString(readyData);

	PushToHotelMsg pushMsg = new PushToHotelMsg()
	{
		PushType = PushMsgType.UserTypeExclude,
		GameID = GameId,
		RoomID = RoomId,
		CpProto = cpProto,
	};

	cpsServer.PushToHotel(RoomId, pushMsg);
	...
```

#接收到机器人信息
收到GameServer创建的机器人之后，客户端获取数据并做相应的界面调整。
代码如下：

```
		...
		if (action.Equals("gsRobot")) {
			int userid = (int)data["userid"];
			UserInfo user = new UserInfo(userid, true);
			GameManager.Instance.AddRoomPlayer(user);
			items[currentCount].UpdateInfo(userid);
			currentCount++;
		}
		...
```
代码路径:Assets\Script\MainUI\GsMatchingBoard.cs

#接收到准备消息
当客户端接收到服务器发送的准备消息之后，进行页面跳转，同时返回相关信息，通知服务器已经准备完毕，随时准备开始。

页面跳转：
```
	...
		if (action.Equals("gsReady")) {
			SingleTone<ContextManager>.Instance.ShowView(new GsGameRoomContext(), false);
		}
	...
```
代码路径:Assets\Script\MainUI\GsMatchingBoard.cs

反馈消息：
```
	...
	JsonData jo = new JsonData();
	jo["action"] = "gsReadyRsp";
	string value = jo.ToJson();
	GameManager.SendEvent(1,value, new int[] { GameManager.userID });
	...
```
代码路径:Assets\Script\MainUI\GsGameRoomBoard.cs

## 开始游戏

GameServer在接收到所有玩家得准备反馈消息之后，发送游戏开始通知,同时创建相应数量的资源点。

```
		...
	   if (room.Ready(broadcast.UserID))
        {
            JObject data= new JObject();
            data["action"] = "gsStart";
            JArray array = new JArray();
            for (int i = 0; i < 3; i++)
            {
                RewardItem item = room.CreateRrewardItem();
                JObject rewardObj = new JObject();
                rewardObj["x"] = item.x;
                rewardObj["y"] = item.y;
                array.Add(rewardObj);
            }
   		    data["rewards"] = array;
            ByteString cpProto = JsonUtils.EncodetoByteString(data);
            PushToHotelMsg pushMsg = new PushToHotelMsg() {
                PushType = PushMsgType.UserTypeExclude,
                GameID = broadcast.GameID,
                RoomID = broadcast.RoomID,
                CpProto = cpProto,
            };

            PushToHotel(broadcast.RoomID, pushMsg);
        }
		...
```

客户端接收请求,开始游戏,同时创建相应数量的资源点：

```
		...
		if (action.Equals("gsStart")) {
			JsonData resources = data["rewards"];
			for (int i = 0; i < resources.Count; i++) {
				int x = (int)resources[i]["x"];
				int y = (int)resources[i]["y"];

				//Creat Reward
				GameObject go = Resources.Load<GameObject>("Reward");
				GameObject target = Instantiate(go, road, false);
				target.GetComponent<RectTransform>().localPosition = new Vector3(x,y);
				Reward reward = target.GetComponent<Reward>();
				reward.UpdateInfo(i);
				rewards.Add(target);
			}

			GameManager.Instance.Gamestart = true;
			Message("游戏开始");
		}
	 ...
```
代码路径:Assets\Script\MainUI\GsGameRoomBoard.cs

##获取资源点
客户端小车获取到资源点之后，向服务器发送通知，GameServer接收消息后做出仲裁，广播发送最终结果。

消息通知发送：

```
	...
	JsonData data = new JsonData();
	data["action"] = "gsReward";
	data["rewardID"] = index;
    data["userID"] = id;
	string value = data.ToJson();
	GameManager.SendEvent(1,value,new int[] {GameManager.userID});
	...
```
代码路径:Assets\Script\MainUI\GsGameRoomBoard.cs

GameServer接到数据总裁,并发送仲裁结果。

```
	...
	if (action.Equals("gsReward")) {
        int rewardID = (int)obj["rewardID"];
        int userID = (int)obj["userID"];
        Room room = roomMgr.GetRoom(broadcast.RoomID);
        bool eated = room.GetReward(rewardID);
        if (eated)
        {
            JObject data = new JObject();
            data["action"] = "gsRewardRsp";
            data["rewardID"] = rewardID;
            data["userID"] = userID;
            ByteString cpProto = JsonUtils.EncodetoByteString(data);
            PushToHotelMsg pushMsg = new PushToHotelMsg() {
	            PushType = PushMsgType.UserTypeExclude,
	            GameID = broadcast.GameID,
	            RoomID = broadcast.RoomID,
	            CpProto = cpProto,
            };

            PushToHotel(broadcast.RoomID, pushMsg);
		}
    }
	...
```

客户端接收到仲裁结果，做出相关数据的调整：

```
	...
	if (action.Equals("gsRewardRsp"))
	{
		int rewardID = (int)data["rewardID"];
		int userid = (int) data["userID"];

		rewards[rewardID].SetActive(false);

		GetPlayerInfoByID(userid).GetReward();

        Message("玩家" + userid + "吃到了资源点,+1金币。");
	}
	...
```
代码路径:Assets\Script\MainUI\GsGameRoomBoard.cs

## 游戏结算

根据游戏里玩家抵达终点的时间进行排名，并给予相应的奖励。

玩家抵达终点之后，发送各自的游戏数据（里程和资源点数量），如果是房主，并且存在机器人，房主上报数据，需携带上机器人的里程和资源点数量。
GameServer在收到所有玩家上报的数据之后，进行仲裁，广播通知最终总裁结果。


上报数据:

```
	...
			GameManager.Instance.Gameover = true;
			JsonData info = new JsonData();
			info["action"] = "gsScore";
			info["score"] = (int)GetPlayerInfoByID(GameManager.userID).mileageValue;
			info["rewardNum"] = GetPlayerInfoByID(GameManager.userID).rewardNum;
			if (GameManager.Instance.RoomOwner)
			{
				info["roomOwner"] = true;
				JsonData list = new JsonData();
				int robotCount = 0;
				for (int i = 0; i < GameManager.Instance.RoomPlayers.Count; i++)
				{
					if (GameManager.Instance.RoomPlayers[i].robot)
					{
						JsonData robotSocre = new JsonData();
						int robotUserID = GameManager.Instance.RoomPlayers[i].userid;
						robotSocre["userid"] = robotUserID;
						robotSocre["score"] = (int) GetPlayerInfoByID(robotUserID).mileageValue;
						robotSocre["rewardNum"] = GetPlayerInfoByID(robotUserID).rewardNum;

						list.Add(robotSocre);
						robotCount++;
					}
				}

				if (robotCount > 0)
				{
					info["robotScore"] = list;
				}
				else
				{
					info["roomOwner"] = false;
				}
			}
			else
			{
				info["roomOwner"] = false;
			}
			string value = info.ToJson();
			GameManager.SendEvent(1,value,new int[] {GameManager.userID});	
	...
```
代码路径:Assets\Script\MainUI\GsGameRoomBoard.cs

GameServer仲裁结果，并广播通知。
```
	...
		   if (action.Equals("gsScore")) {
				int score = (int)obj["score"];
				int rewardNum = (int)obj["rewardNum"];
				bool roomOwner = (bool)obj["roomOwner"];
				Room room = roomMgr.GetRoom(broadcast.RoomID);
	            if (roomOwner)
	            {
					JArray robotScore = (JArray)obj["robotScore"];
					for (int i = 0; i < robotScore.Count; i++)
		            {
			            JObject item = (JObject) robotScore[i];
						int robotuserid = (int)item["userid"];
			            int robotscore = (int)item["score"];
			            int robotrewardNum = (int)item["rewardNum"];
			            room.ReportScore((uint)robotuserid, robotscore, robotrewardNum);
		            }
	            }

	            bool flag = room.ReportScore(broadcast.UserID,score,rewardNum);
	            if (flag)
	            {
                    JObject data = new JObject();
	                data["action"] = "gsResult";;
                    JArray resultList = new JArray();
                    for (int i = 0; i < room.playerResults.Count; i++)
	                {
	                    Player player = room.playerResults[i];
                        JObject playerResult = new JObject();
	                    playerResult["userid"] = player.Uid;
	                    playerResult["rewardNum"] = player.Attr_1;
                        resultList.Add(playerResult);
	                }
	                data["resultList"] = resultList;
	                ByteString cpProto = JsonUtils.EncodetoByteString(data);
		            PushToHotelMsg pushMsg = new PushToHotelMsg() {
			            PushType = PushMsgType.UserTypeExclude,
			            GameID = broadcast.GameID,
			            RoomID = broadcast.RoomID,
			            CpProto = cpProto,
		            };

		            PushToHotel(broadcast.RoomID, pushMsg);
				}
            }
	...
```
客户端接收到相关信息，做相应的结果展示。
```
	...
			JsonData list = data["resultList"];
			for (int i = 0; i < list.Count; i++)
			{
				JsonData playerInfo = list[i];
				int userID = (int)playerInfo["userid"];
			    int rewardNum = (int) playerInfo["rewardNum"];
				GameManager.Instance.AddUserID(new PlayerResult() {userid = userID,rewardNum = rewardNum});
			}
	...
```
代码路径:Assets\Script\MainUI\GsGameRoomBoard.cs