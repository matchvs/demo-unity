using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextManager {
	private Stack<BaseContext> _stack = new Stack<BaseContext>();
	public ContextManager() {
		Push(new MainMenuContext(), false);
	}



	public void ShowView(BaseContext context, bool isDialog) {
		Push(context, isDialog);
	}

    public void Back()
    {
        Pop();
    }

    public void Push(BaseContext context, bool isDialog) {
		if (_stack.Count > 0 && !isDialog) {
			BaseContext _curContext = _stack.Peek();
			BaseView _curView = SingleTone<UIManager>.Instance.GetSingleUI(_curContext.Type).GetComponent<BaseView>();
			_curView.OnPause();
		}

		BaseView _lastView = SingleTone<UIManager>.Instance.GetSingleUI(context.Type).GetComponent<BaseView>();
		_stack.Push(context);
		_lastView.OnEnter();
	}

	public void Pop() {
		if (_stack.Count > 0) {
			BaseContext _curContext = _stack.Pop();
			BaseView _curView = SingleTone<UIManager>.Instance.GetSingleUI(_curContext.Type).GetComponent<BaseView>();
			_curView.OnPause();
		}

		if (_stack.Count > 0) {
			BaseContext _nextContext = _stack.Peek();
			BaseView _lastView = SingleTone<UIManager>.Instance.GetSingleUI(_nextContext.Type).GetComponent<BaseView>();
			_lastView.OnResume();
		}
	}
}