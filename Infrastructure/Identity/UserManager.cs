#region

using Application.Common.Interfaces;
using Domain.Value_Object;

#endregion

namespace Infrastructure.Identity;

public class UserManager : IUserManager
{
    private CurrentUser? _currentUser;

    public CurrentUser User
    {
        get
        {
            if (_currentUser == null)
                throw new Exception("User not logged in");
            return _currentUser;
        }
        set => _currentUser = value;
    }
}