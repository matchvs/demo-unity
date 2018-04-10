namespace MatchVS {

public  class MsBusiMsgRsp{
	
	    /// <summary>
        /// 用户ID
        /// </summary>
		public int userID;

        /// <summary>
        /// 标签
        /// </summary>
		public byte							flags;

        /// <summary>
        /// 事件ID
        /// </summary>
		public int eventID;

        /// <summary>
        /// 消息大小
        /// </summary>
		public int payloadSize;

        /// <summary>
        /// 负载消息
        /// </summary>
		public bool							hasPayload;

        /// <summary>
        /// 消息内容
        /// </summary>
		public string payload;

        /// <summary>
        /// 是否有分数
        /// </summary>
		public bool							hasScore;
	}
}