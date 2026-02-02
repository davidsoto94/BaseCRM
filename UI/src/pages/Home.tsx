import auth from '../services/auth'
import { useNavigate } from 'react-router-dom'
import NavigationBar from '../components/NavigationBar'
import { useI18n } from '../i18n/I18nProvider'

export default function Home() {
  const navigate = useNavigate()
  const { t } = useI18n()

  function handleLogout() {
    auth.logout()
    navigate('/login')
  }

  return (
    <div className="min-h-screen bg-gray-50 dark:bg-gray-900">
      <NavigationBar />
      <main className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 py-12">
        <section className="rounded-xl bg-white dark:bg-gray-800 shadow-sm border border-gray-200 dark:border-gray-700 p-8">
          <h1 className="text-3xl font-semibold text-gray-900 dark:text-gray-100">
            {t('home.title')}
          </h1>
          <p className="mt-2 text-gray-600 dark:text-gray-300">
            {t('home.subtitle')}
          </p>
          <div className="mt-6">
            <button
              onClick={handleLogout}
              className="inline-flex items-center rounded-md bg-gray-900 text-white px-4 py-2 text-sm font-medium hover:bg-gray-800 focus:outline-none focus:ring-2 focus:ring-gray-900 focus:ring-offset-2 focus:ring-offset-gray-100 dark:bg-gray-200 dark:text-gray-900 dark:hover:bg-gray-300 dark:focus:ring-gray-200 dark:focus:ring-offset-gray-900"
            >
              {t('home.logout')}
            </button>
          </div>
        </section>
      </main>
    </div>
  )
}
