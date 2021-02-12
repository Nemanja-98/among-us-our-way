const PROXY_CONFIG = [
    {
        context: [
            "/user",
            "/game"
        ],
        target: "https://192.168.1.200:5002",
        secure: false,
        changeOrigin: true
    }
  ]
  
  module.exports = PROXY_CONFIG;
  