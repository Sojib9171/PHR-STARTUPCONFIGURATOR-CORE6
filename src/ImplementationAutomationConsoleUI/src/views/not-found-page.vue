<template>
  <div class="not-found-container">
    <div class="not-found-card">
      <h1>404 Error</h1>
      <p>Page not found!</p>
    </div>
  </div>
</template>

<style scoped>
.not-found-container {
  display: flex;
  align-items: center;
  justify-content: center;
  height: 100vh;
}

.not-found-card {
  background-color: #fff;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  padding: 40px;
  border-radius: 8px;
  text-align: center;
  max-width: 400px;
  width: 100%;
  transition: transform 0.3s;
}

.not-found-card:hover {
  transform: scale(1.02);
}

h1 {
  color: #ff6347;
  margin-bottom: 20px;
}

p {
  color: #555;
  margin-bottom: 30px;
}

.home-button {
  background-color: #4caf50;
  color: #fff;
  padding: 12px 24px;
  font-size: 16px;
  border: none;
  cursor: pointer;
  border-radius: 4px;
  transition: background-color 0.3s;
}

.home-button:hover {
  background-color: #45a049;
}</style>