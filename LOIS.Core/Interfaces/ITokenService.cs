using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOIS.Core.Models;

namespace LOIS.Core.Interfaces
{
    public interface ITokenService
    {
        Token GenerateToken(int userId);

        bool ValidateToken(string authToken);

        bool Kill(string tokenId);

        bool DeleteByUserId(int userId);
    }

}
