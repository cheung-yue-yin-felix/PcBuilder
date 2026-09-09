import { AuthenticatedHeader } from "./Authenticated";
import { AnonymousHeader } from "./Anonymous";

export function Header({ isAuthenticated, user, isAdmin, isMember, handleLogout }: Readonly<{ isAuthenticated: boolean, user: { email: string } | null, isAdmin: boolean, isMember: boolean, handleLogout: () => Promise<void> }>) {
    return (
        isAuthenticated ? (
          <AuthenticatedHeader
            user={user}
            isAdmin={isAdmin}
            isMember={isMember}
            handleLogout={handleLogout}
          />
        ) : (
          <AnonymousHeader/>
        )
    )
}