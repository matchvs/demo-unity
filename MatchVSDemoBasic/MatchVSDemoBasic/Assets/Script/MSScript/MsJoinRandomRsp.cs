
namespace MatchVS
{
    public class MsJoinRandomRsp {
        /// <summary>
        /// 返回值
        /// </summary>
        public int status;

        /// <summary>
        /// 房间已有用户列表
        /// </summary>
        public MsUserInfoList[] userInfoList;

        /// <summary>
        /// 房间信息
        /// </summary>
        public MsRoomInfo roomInfo;

        /// <summary>
        /// 负载数据
        /// </summary>
        public string cpProto;
    }

}


