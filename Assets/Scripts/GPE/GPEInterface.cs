namespace GPE
{
    
    public interface IResettable
    {
        // c'est bien ca !
        public void ResetToInitialState();
    }
    
    public interface ISubject
    {
        public void Subscribe(IResettable resettable);
        public void Unsubscribe(IResettable resettable);
        public void ResetNotify();
    }
}
