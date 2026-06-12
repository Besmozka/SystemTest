namespace Server
{
    public interface IRequestsController
    {
        public void EnqueueRequest(BackendRequest request) { }
        public void DequeueRequest(BackendRequest request) { }
    }
}