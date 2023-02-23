using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Contracts
{
    public interface IGameService
    {
        Task<int> StartGame(int player1ID, int player2ID);
    }
}
