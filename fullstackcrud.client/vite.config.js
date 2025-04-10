import { defineConfig } from 'vite';

export default defineConfig({
  server: {
    https: false, // Disable HTTPS for Vercel if necessary
  },
  build: {
    outDir: 'dist',
  },
});
