namespace DefaultNamespace
{
    public class DumbInteractable : Interactable
    {
        public override void InteractStart()
        {
            print("start");
        }
        public override void InteractUpdate()
        {
        }
        public override void InteractEnd()
        {
            print("end");
        }
    }
}
