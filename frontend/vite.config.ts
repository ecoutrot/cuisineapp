/// <reference types="vitest/config" />
import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'

// https://vite.dev/config/
export default defineConfig({
  base: "/",
  plugins: [
    react()
  ],
  test: {
    exclude: ['**/node_modules/**', '**/dist/**', '**/build/**', '**/playwright/**'], 
  },
})
