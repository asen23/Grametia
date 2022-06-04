#region

#endregion

#region

using Domain.Value_Object;

#endregion

namespace Application.Common.Interfaces;

public interface IUserManager
{
    public CurrentUser User { get; set; }
}