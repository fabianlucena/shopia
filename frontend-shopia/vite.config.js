import { defineConfig } from 'vite';
import { svelte } from '@sveltejs/vite-plugin-svelte';
import path from 'path';

// https://vite.dev/config/
export default defineConfig({
  plugins: [svelte()],
  resolve: {
    alias: {
      $components: path.resolve(__dirname, 'src/components'),
      $pages: path.resolve(__dirname, 'src/pages'),
      $libs: path.resolve(__dirname, 'src/libs'),
      $stores: path.resolve(__dirname, 'src/stores'),
      $services: path.resolve(__dirname, 'src/services'),
      $icons: path.resolve(__dirname, 'src/icons'),
    },
  },
});
