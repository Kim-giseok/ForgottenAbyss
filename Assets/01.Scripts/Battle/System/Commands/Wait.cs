namespace BT
{
    public class Wait
    {
        // struct로 뺄까?
        public string animName;
        public float duration = 0f;
        public bool isStopPos = true;
        public bool isLookTarget = false;

        public Wait Animation(string animName)
        {
            this.animName = animName;
            return this;
        }
        
        public Wait Stop(bool isStopPos) {
            this.isStopPos = isStopPos;
            return this;
        }
    
        public Wait LookTarget(bool isLookTarget)
        {
            this.isLookTarget = isLookTarget;
            return this;
        }
        
        public Wait Duration(float duration)
        {
            this.duration = duration;
            return this;
        }

        public Node Build()
        {
            return new WaitNode(this);
        }
    }
}

