
namespace MatchVS
{
    public class MsRoomFilter
    {
        /// <summary>
        /// 最大人数
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
        /// 房间属性
        /// </summary>
        public string roomProperty;

        public MsRoomFilter(int maxPlayer, int mode, int canWatch, string roomProperty)
        {
            this.maxPlayer = maxPlayer;
            this.mode = mode;
            this.canWatch = canWatch;
            this.roomProperty = roomProperty;
        }
    }
}

