namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IPhotoRepository PhotoRepository { get; }
        IUserRepository UserRepository { get; }
        IMessageRepository MessageRepository { get; }
        ILikesRepository LikesRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}