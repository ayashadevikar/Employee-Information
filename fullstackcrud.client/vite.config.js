import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  server: {
    https: false, // Disable HTTPS for Vercel if necessary
  },
  build: {
    outDir: 'dist',
  },

  plugins: [react()],
});
