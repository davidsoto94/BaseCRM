/* eslint-disable react-refresh/only-export-components */
import { I18nextProvider, Trans, useTranslation } from "react-i18next";
import i18n, { supportedLangs, type Lang } from "./i18n";

export type { Lang } from "./i18n";

export function I18nProvider({ children }: { children: React.ReactNode }) {
  return <I18nextProvider i18n={i18n}>{children}</I18nextProvider>;
}

export function useI18n(namespace = "common") {
  const { t } = useTranslation(namespace);
  const lang = (i18n.language as Lang) || supportedLangs[0];
  const setLang = (l: Lang) => {
    if (l !== lang) {
      if (typeof window !== "undefined") localStorage.setItem("lang", l);
      i18n.changeLanguage(l);
    }
  };
  return { lang, setLang, t, supportedLangs };
}

export { Trans, useTranslation };
