namespace Contracts
{
	public interface IRepositoryWrapper
	{
		ICardRepository Card { get; }
		void Save();
	}
}