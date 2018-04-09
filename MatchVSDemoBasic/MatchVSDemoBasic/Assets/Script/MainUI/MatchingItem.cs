using UnityEngine;
using UnityEngine.UI;

public class MatchingItem : MonoBehaviour
{
    public Text userID;
    private int userId;

    private void OnEnable()
    {
        this.userID.text = "用户ID";
    }

    public void UpdateInfo(int userID)
    {
        this.userID.text = userID.ToString();
        this.userId = userID;
    }

    public void RemoveNotify(int userID)
    {
        if (userId == userID)
        {
            this.userID.text = "用户ID";
        }
    }
}
