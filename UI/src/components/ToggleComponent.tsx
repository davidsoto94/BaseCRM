import { useTheme } from "../style/ThemeContext";

export const ThemeToggle = () => {
  const { theme, setTheme } = useTheme();
  const isDark = theme === "dark";

  return (
    <label className="inline-flex items-center gap-3 cursor-pointer select-none p-2 rounded-lg border border-transparent dark:border-transparent bg-transparent shadow-sm">
      <input
        type="checkbox"
        role="switch"
        aria-checked={isDark}
        aria-label="Toggle theme"
        checked={isDark}
        onChange={() => setTheme(isDark ? "light" : "dark")}
        className="sr-only peer"
      />
      <div
        className={[
          "relative w-12 h-6 rounded-full transition-colors duration-200",
          isDark ? "bg-indigo-600" : "bg-gray-300 dark:bg-gray-700",
          "peer-focus:outline-none peer-focus:ring-2 peer-focus:ring-indigo-400",
        ].join(" ")}>
        <span
          className={[
            "absolute top-0.5 left-0.5 h-5 w-5 bg-white dark:bg-gray-100 rounded-full shadow",
            "transition-transform duration-200 flex items-center justify-center",
            isDark ? "translate-x-6" : "translate-x-0",
          ].join(" ")}>
          <span aria-hidden="true" className="text-sm leading-none">
            {isDark ? "🌙" : "☀️"}
          </span>
        </span>
      </div>
    </label>
  );
};
