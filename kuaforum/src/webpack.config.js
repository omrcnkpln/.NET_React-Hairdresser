module.exports = {
   target:  'node',
    resolve: {
        fallback: {
            "util": require.resolve("util/"),
            "crypto": false,
            "buffer": require.resolve("buffer"),
            "stream": require.resolve("stream"),
        }
    }
    
  };