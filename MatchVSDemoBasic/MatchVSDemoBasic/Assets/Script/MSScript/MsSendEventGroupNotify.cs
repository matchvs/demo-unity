namespace MatchVS
{

	public class MsSendEventGroupNotify
	{
        /// <summary>
        /// 用户ID
        /// </summary>
		public int srcUserID;

        /// <summary>
        /// 订阅组
        /// </summary>
		public string[] groups;

        /// <summary>
        /// 负载数据
        /// </summary>
		public string cpProto;
	}
}
