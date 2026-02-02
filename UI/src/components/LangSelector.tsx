import { useI18n, type Lang } from "../i18n/I18nProvider";

export default function LangSelector() {
    
    const { t, lang, setLang, supportedLangs } = useI18n();
    return(
        <select
            value={lang}
            onChange={(e) => setLang(e.target.value as Lang)}
            className="bg-transparent text-sm text-gray-700 dark:text-gray-200 border border-gray-300 dark:border-gray-600 rounded px-2 py-1"
            aria-label={t("language.aria_label")}
        >
            {supportedLangs.map((code) => (
            <option
              key={code}
              value={code}
              className="bg-white text-gray-900 hover:bg-blue-600 hover:text-white dark:bg-gray-800 dark:text-gray-100 rounded"
            >
              {t(`language.${code}`)}
            </option>
            ))}
        </select>
    )
}