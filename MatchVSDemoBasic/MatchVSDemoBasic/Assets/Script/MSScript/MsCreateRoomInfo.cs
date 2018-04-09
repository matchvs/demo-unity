namespace MatchVS
{
    public class MsCreateRoomInfo
    {
       /// <summary>
       /// 房间名称
       /// </summary>
        public string name;

        /// <summary>
        /// 最大人數
        /// </summary>
        public int maxPlayer;

        /// <summary>
        /// 模式
        /// </summary>
        public int mode;

        /// <summary>
        /// 可否观战
        /// </summary>
        public int canWatch;

        /// <summary>
        /// 是否可见
        /// </summary>
        public int visibility;

        /// <summary>
        /// 房间属性
        /// </summary>
        public string roomProperty;

        public MsCreateRoomInfo(string name, int maxPlayer, int mode, int canWatch, int visibility, string roomProperty)
        {
            this.name = name;
            this.maxPlayer = maxPlayer;
            this.mode = mode;
            this.canWatch = canWatch;
            this.visibility = visibility;
            this.roomProperty = roomProperty;
        }
    }
}

