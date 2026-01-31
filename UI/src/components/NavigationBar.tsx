import { ThemeToggle } from "./ToggleComponent";
import auth from "../services/auth";
import { Permissions } from "../Enums/PermitionEnum";
import { Link } from "react-router-dom";

// Add a type definition for auth if missing
type AuthType = {
  getToken?: () => string | null;
};

const typedAuth = auth as AuthType;

interface JwtPayload {
  name?: string;
  given_name?: string;
  family_name?: string;
  email?: string;
  picture?: string;
  Permissions?: string;
};

function decodeJwt(token: string): JwtPayload | null {
  try {
    const payload = token.split(".")[1];
    const json = atob(payload.replace(/-/g, "+").replace(/_/g, "/"));
    return JSON.parse(json);
  } catch {
    return null;
  }
}

function getInitials(text: string) {
  const parts = text.trim().split(/\s+/);
  const first = parts[0]?.[0] ?? "";
  const last = parts[1]?.[0] ?? "";
  return (first + last || text[0] || "?").toUpperCase();
}

export default function NavigationBar() {
  const token =
    typedAuth.getToken?.() ??
    localStorage.getItem("auth_token") ??
    localStorage.getItem("token");

  const payload = token ? decodeJwt(token) : null;
  const displayName =
    payload?.name || payload?.given_name || payload?.email || "Guest";
  const email = payload?.email;
  const avatarUrl = payload?.picture;

  // Adjust the keys below to match your permission naming
  const canAddUser =
    Array.isArray(payload?.Permissions) &&
    (payload!.Permissions as string[]).some((p) =>
      Object.values(Permissions).includes(String(p))
    );
    console.log("User Permissions:", canAddUser);

  return (
    <header className="sticky top-0 z-50 backdrop-blur bg-white/70 dark:bg-gray-900/50 border-b border-gray-200 dark:border-gray-700">
      <div className="mx-auto max-w-6xl px-4 sm:px-6">
        <div className="h-14 flex items-center justify-between">
          <div className="flex items-center gap-2">
            <span className="text-base sm:text-lg font-semibold text-gray-900 dark:text-gray-100">
              BaseCRM
            </span>
          </div>
          <div className="flex items-center gap-3">
            {canAddUser && (
              <Link
                to="/Register"
                className="inline-flex items-center rounded-md bg-indigo-600 text-white px-3 py-1.5 text-sm font-medium hover:bg-indigo-500 focus:outline-none focus:ring-2 focus:ring-indigo-600 focus:ring-offset-2 focus:ring-offset-white dark:focus:ring-offset-gray-900"
              >
                New User
              </Link>
            )}
            <ThemeToggle />
            <div className="flex items-center gap-2">
              {avatarUrl ? (
                <img
                  src={avatarUrl}
                  alt={displayName}
                  className="h-8 w-8 rounded-full object-cover shadow"
                />
              ) : (
                <div className="h-8 w-8 rounded-full bg-indigo-600 text-white flex items-center justify-center text-xs font-semibold shadow">
                  {getInitials(displayName)}
                </div>
              )}
              <div className="hidden sm:flex flex-col leading-tight">
                <span className="text-sm text-gray-900 dark:text-gray-100">
                  {displayName}
                </span>
                {email && (
                  <span className="text-xs text-gray-500 dark:text-gray-400">
                    {email}
                  </span>
                )}
              </div>
            </div>
          </div>
        </div>
      </div>
    </header>
  );
}
