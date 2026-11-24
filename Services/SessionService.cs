using ClubNegociosUDLA.Models;

namespace ClubNegociosUDLA.Services;

public class SessionService
{
    public Member? CurrentUser { get; private set; }

    public bool IsAuthenticated => CurrentUser is not null;

    public void SignIn(Member member) => CurrentUser = member;

    public void SignOut() => CurrentUser = null;
}
