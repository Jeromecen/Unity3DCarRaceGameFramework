public abstract class AsyncEventBase{
    protected enum AE_Status
    {
        E_WAIT,
        E_RUNNING,
        E_FINISH,
    }
    protected AE_Status status;

    public abstract void  OnUpdate(float deltaTime);
    public abstract void Destroy();
    public AsyncEventBase()
    {
        status = AE_Status.E_WAIT;
    }
    // 事件要触发一定要记得调用Begin,否则OnUpdate不会结束
    public virtual void Begin()
    {
        status = AE_Status.E_RUNNING;
    }
	
    public bool IsFinish()
    {
        return status == AE_Status.E_FINISH;
    }

    public void End()
    {
        status = AE_Status.E_FINISH;
    }


}
