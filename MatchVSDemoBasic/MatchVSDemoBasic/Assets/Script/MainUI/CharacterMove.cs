using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMove : MonoBehaviour
{
    public Transform destination;
    public float moveSpeed = 0.5f;
    public float leftBoarder;
    public float rightBoarder;
    public float rightMoveSpeed = 0f;
    public float leftMoveSpeed = 0f;
    public Color changeColor;
    private Color originalColor;

    private GameRoomBoard roomBoard;
	private GsGameRoomBoard gsRoomBoard;

    private void Awake()
    {
        originalColor = GetComponent<Image>().color;
    }

    private void OnEnable()
    {
	    GameObject obj = GameObject.Find("GameRoomBoard(Clone)");
	    if (obj == null)
	    {
			obj = GameObject.Find("GsGameRoomBoard(Clone)");
		    gsRoomBoard = obj.GetComponent<GsGameRoomBoard>();
	    }
	    else
	    {
		    roomBoard = obj.GetComponent<GameRoomBoard>();
	    }

		moveSpeed = 0.5f;
        rightMoveSpeed = 0;
        leftMoveSpeed = 0;

        OnExist();
    }

    public void EnterSpecial()
    {
		if(roomBoard != null)
			roomBoard.OnEnterSpecialArea(gameObject);

		if(gsRoomBoard != null)
			gsRoomBoard.OnEnterSpecialArea(gameObject);
    }

    public void ExitSpecial()
    {
		if(roomBoard != null)
			roomBoard.OnExitSpecialArea(gameObject);
		if(gsRoomBoard != null)
			gsRoomBoard.OnExitSpecialArea(gameObject);
    }

    private void Update()
    {
        if (GameManager.Instance.Gamestart && !GameManager.Instance.Gameover)
        {
            transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
            if (transform.localPosition.x < rightBoarder)
                transform.Translate(Vector3.right * Time.deltaTime * rightMoveSpeed);
            if (transform.localPosition.x > leftBoarder)
                transform.Translate(Vector3.left * Time.deltaTime * leftMoveSpeed);
            if (transform.localPosition.y > destination.localPosition.y)
            {
	            if (roomBoard != null)
	            {
					GameManager.Instance.Gameover = true;
					roomBoard.GameOver();
	            }
				if(gsRoomBoard != null)
					gsRoomBoard.GameOver();
            }
        }
    }

    public void OnEnter()
    {
        GetComponent<Image>().color = changeColor;
    }

    public void OnExist()
    {
        GetComponent<Image>().color = originalColor;
    }

	public void Reward(int index)
	{
		if(gsRoomBoard != null)
			gsRoomBoard.OnReward(gameObject,index);
	}
}
