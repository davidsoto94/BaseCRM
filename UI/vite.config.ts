import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'


// https://vite.dev/config/
export default defineConfig({
  build:{
    outDir: "../wwwroot",
    emptyOutDir: true
  },
  plugins: [react(),
    tailwindcss(),
  ],
  server: {
    proxy: {
      // Proxy requests starting with /api to the .NET backend
      '/api': {
        target: 'https://localhost:44313', // Replace with your .NET URL
        changeOrigin: true,
        secure: false, // Set to true if using valid HTTPS certificates
        rewrite: (path) => path.replace(/^\/api/, '') // Optional: remove /api prefix
      }
    }
  }

})
