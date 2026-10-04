import react from '@vitejs/plugin-react'
import { defineConfig } from 'vite'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    proxy: {
      // Backend API isteklerini .NET Web API'ye yönlendir
      '/api': {
        target: 'http://localhost:5062',
        changeOrigin: true,
        secure: false,
      },
    },
  },
})


