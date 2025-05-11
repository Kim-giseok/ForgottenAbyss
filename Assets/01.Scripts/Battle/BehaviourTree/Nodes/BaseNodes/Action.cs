using System;

namespace BT
{
    public class Action : Node
    {
        private readonly Action<Node> _callback;
    
        public Action(Action<Node> callback) => this._callback = callback;
    
        public override void Start()
        {
            _callback?.Invoke(this);
            SetStatus(Status.Success);
        }
    }
}
