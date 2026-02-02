import i18n from "i18next";
import { initReactI18next } from "react-i18next";
import enCommon from "../locales/en/common.json";
import esCommon from "../locales/es/common.json";
import frCommon from "../locales/fr/common.json";

export const supportedLangs = ["en", "es", "fr"] as const;
export type Lang = (typeof supportedLangs)[number];

const resolveInitialLang = (): Lang => {
  if (typeof window === "undefined") return supportedLangs[0];
  const stored = localStorage.getItem("lang") as Lang | null;
  if (stored && supportedLangs.includes(stored)) return stored;
  const candidates = [
    ...(navigator.languages ?? []),
    navigator.language,
  ].filter(Boolean) as string[];
  const match = candidates
    .map((code) => code.split("-")[0])
    .find((code) => supportedLangs.includes(code as Lang));
  const resolved = (match as Lang) ?? supportedLangs[0];
  localStorage.setItem("lang", resolved);
  return resolved;
};

const savedLang = resolveInitialLang();

i18n
  .use(initReactI18next)
  .init({
    resources: {
      en: { common: enCommon },
      es: { common: esCommon },
      fr: { common: frCommon },
    },
    lng: savedLang,
    fallbackLng: supportedLangs[0],
    supportedLngs: supportedLangs,
    ns: ["common"],
    defaultNS: "common",
    interpolation: { escapeValue: false },
  });

export default i18n;
