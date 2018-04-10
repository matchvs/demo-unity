
namespace MatchVS
{
    public class MsFrameData
    {
        /// <summary>
        /// 帧编号
        /// </summary>
        public int frameIndex;

        /// <summary>
        /// 帧数据
        /// </summary>
        public MsFrameItem[] frameItems;

        /// <summary>
        /// 等待的帧数
        /// </summary>
        public int frameWaitCount;
    }
}

