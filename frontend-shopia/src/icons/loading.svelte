<svg
  class="spinner"
  viewBox="0 0 50 50"
  xmlns="http://www.w3.org/2000/svg"
>
  <defs>
    <linearGradient id="grad" x1="0%" y1="0%" x2="100%" y2="0%">
      <stop offset="0%" stop-color="#00ffaa" />
      <stop offset="100%" stop-color="#008844" />
    </linearGradient>
  </defs>

  <circle
    class="path"
    cx="25"
    cy="25"
    r="20"
    fill="none"
    stroke="url(#grad)"
    stroke-width="5"
    stroke-linecap="round"
  />
</svg>

<style>
  .spinner {
    animation: rotate 2s linear infinite;
    width: 50px;
    height: 50px;
  }

  .path {
    stroke-dasharray: 150, 200;
    stroke-dashoffset: -10;
    animation: dash 1.5s ease-in-out infinite;
  }

  @keyframes rotate {
    100% {
      transform: rotate(360deg);
    }
  }

  @keyframes dash {
    0% {
      stroke-dasharray: 1, 200;
      stroke-dashoffset: 0;
    }
    50% {
      stroke-dasharray: 90, 200;
      stroke-dashoffset: -40px;
    }
    100% {
      stroke-dasharray: 90, 200;
      stroke-dashoffset: -120px;
    }
  }
</style>