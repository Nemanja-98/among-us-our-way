const PROXY_CONFIG = [
    {
        context: [
            "/user",
            "/game"
        ],
        target: "https://localhost:5001",
        secure: false,
        changeOrigin: true
    }
  ]
  
  module.exports = PROXY_CONFIG;
  