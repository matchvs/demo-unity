namespace MatchVS
{
    public class MsSetChannelFrameSyncNotify
    {
        /// <summary>
        /// 要帧同步的消息通道
        /// </summary>
        public int priority;

        /// <summary>
        /// 帧率
        /// </summary>
        public int framRate;

        /// <summary>
        /// 初始帧编号
        /// </summary>
        public int startIndex;
    }
}
