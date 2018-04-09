
using System.Collections.Generic;

namespace MatchVS
{
    public class MsMatchInfo
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
        /// 匹配条件
        /// </summary>
        public List<object> tags;

        public MsMatchInfo(int maxPlayer, int mode, int canWatch, params MsMatchInfoTag[] objs)
        {
            this.maxPlayer = maxPlayer;
            this.mode = mode;
            this.canWatch = canWatch;
            this.tags = new List<object>();
            for (int i = 0; i < objs.Length; i++)
            {
                tags.Add(objs[i].ToJson());
            }
        }
    }
}

