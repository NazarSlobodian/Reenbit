const { env } = require('process');

const target = 'http://test.presentation:8080'

const PROXY_CONFIG = [
  {
    context: [
      "/api",
      "/hubs/booking"
    ],
    target,
    secure: false,
    ws: true
  }
]

module.exports = PROXY_CONFIG;
