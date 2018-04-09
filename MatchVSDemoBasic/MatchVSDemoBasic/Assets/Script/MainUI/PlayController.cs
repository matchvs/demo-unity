using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ControllerType {
    left,
    right,
    accelerate,
}

public class PlayController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    public ControllerType type;
    private GameRoomBoard roomBoard;
	private GsGameRoomBoard gsRoomBoard;

    private void OnEnable()
    {
	    GameObject go = GameObject.Find("GameRoomBoard(Clone)");
		if(go != null)
			roomBoard = go.GetComponent<GameRoomBoard>();

	    go = GameObject.Find("GsGameRoomBoard(Clone)");
		if(go != null)
			gsRoomBoard =go.GetComponent<GsGameRoomBoard>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        switch (type) {
            case ControllerType.accelerate:
				if(roomBoard != null)
					roomBoard.AccelerateDown();
				if(gsRoomBoard != null)
					gsRoomBoard.AccelerateDown();
                break;
            case ControllerType.left:
	            if (roomBoard != null)
					roomBoard.LeftDown();
	            if (gsRoomBoard != null)
		            gsRoomBoard.LeftDown();
				break;
            case ControllerType.right:
	            if (roomBoard != null)
					roomBoard.RightDown();
	            if (gsRoomBoard != null)
		            gsRoomBoard.RightDown();
				break;
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        switch (type) {
            case ControllerType.accelerate:
	            if (roomBoard != null)
					roomBoard.AccelerateUp();
	            if (gsRoomBoard != null)
		            gsRoomBoard.AccelerateUp();
				break;
            case ControllerType.left:
	            if (roomBoard != null)
					roomBoard.LeftUp();
	            if (gsRoomBoard != null)
		            gsRoomBoard.LeftUp();
				break;
            case ControllerType.right:
	            if (roomBoard != null)
					roomBoard.RightUp();
	            if (gsRoomBoard != null)
		            gsRoomBoard.RightUp();
				break;
        }
    }
}
