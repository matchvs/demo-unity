using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIType {

	public string Path { get; private set; }
	public string Name { get; private set; }

	public UIType(string path) {
		Path = path;
		Name = path;
	}

	public static UIType MainMenuBoard = new UIType("MainMenuBoard");
    public static UIType GameLobbyBoard = new UIType("GameLobbyBoard");
    public static UIType MatchingBoard = new UIType("MatchingBoard");
    public static UIType GameRoomBoard = new UIType("GameRoomBoard");
    public static UIType GameOverBoard = new UIType("GameOverBoard");
	public static UIType CreateRoomBoard = new UIType("CreateRoomBoard");
	public static UIType JoinSpecifiedRoomBoard = new UIType("JoinSpecifiedRoomBoard");
	public static UIType MatchAttributeBoard = new UIType("MatchAttributeBoard");
    public static UIType RoomListBoard = new UIType("RoomListBoard");
    public static UIType GsMatchingBoard = new UIType("GsMatchingBoard");
	public static UIType GsGameRoomBoard = new UIType("GsGameRoomBoard");
}